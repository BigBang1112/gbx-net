using GBX.NET.Components;
using GBX.NET.Crypto;
using GBX.NET.Exceptions;
using GBX.NET.PAK.Exceptions;
using GBX.NET.Serialization;
using NativeSharpZlib;
using System.Collections.Immutable;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;

namespace GBX.NET.PAK;

public partial class Pak : IDisposable
#if NET5_0_OR_GREATER
    , IAsyncDisposable
#endif
{
    /// <summary>
    /// Magic (intial binary letters) for Pak files.
    /// </summary>
    public const string Magic = "NadeoPak";

    private const string PakListFileName = "packlist.dat";

    private readonly Stream stream;
    private readonly byte[]? key;
    private readonly byte[]? secondKey;

    private int metadataStart;
    private int dataStart;

    public int Version { get; }

    public byte[]? HeaderMD5 { get; private set; }
    public int Flags { get; private set; }
    public virtual bool IsHeaderEncrypted => true;

    public ImmutableDictionary<string, PakFile> Files { get; private set; } = ImmutableDictionary<string, PakFile>.Empty;

    internal Pak(Stream stream, byte[]? key, byte[]? secondKey, int version)
    {
        this.stream = stream;
        this.key = key;
        this.secondKey = secondKey;
        Version = version;
    }

    /// <summary>
    /// Parses the Pak file from the stream. Should be disposed after use, as it keeps the file open (currently at least).
    /// </summary>
    /// <param name="stream">Stream.</param>
    /// <param name="primaryKey">Key for main decryption, or only the header part for newer Pak format.</param>
    /// <param name="fileKey">Alternative key to use for decrypting individual files. Ignored for TMF Pak (v3) format and older.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task. The task result contains the parsed Pak format.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
    /// <exception cref="NotAPakException">Stream is not Pak-formatted.</exception>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<Pak> ParseAsync(Stream stream, byte[]? primaryKey = null, byte[]? fileKey = null, CancellationToken cancellationToken = default)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        var r = new GbxReader(stream);

        if (!r.ReadPakMagic())
        {
            throw new NotAPakException();
        }

        var version = r.ReadInt32();

        var pak = version < 6 
            ? new Pak(stream, primaryKey, fileKey, version) 
            : new Pak6(stream, primaryKey, fileKey, version);

        if (pak is Pak6 pak6)
        {
            await pak6.ReadUnencryptedHeaderAsync(r, version, cancellationToken);
        }

        if (pak.IsHeaderEncrypted)
        {
            if (primaryKey is null)
            {
                return pak;
            }

            var iv = r.ReadUInt64();
            var blowfishStream = new BlowfishStream(stream, primaryKey, iv, version == 18);

            await pak.ReadHeaderAsync(blowfishStream, cancellationToken);
        }
        else
        {
            await pak.ReadHeaderAsync(stream, cancellationToken);
        }

        return pak;
    }

    /// <summary>
    /// Parses the Pak file from file path. Should be disposed after use, as it keeps the file open (currently at least).
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <param name="primaryKey">Key for main decryption, or only the header part for newer Pak format.</param>
    /// <param name="fileKey">Alternative key to use for decrypting individual files. Ignored for TMF Pak (v3) format and older.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task. The task result contains the parsed Pak format.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null.</exception>
    /// <exception cref="NotAPakException">Stream is not Pak-formatted.</exception>
    public static async Task<Pak> ParseAsync(string filePath, byte[]? primaryKey = null, byte[]? fileKey = null, CancellationToken cancellationToken = default)
    {
        var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await ParseAsync(fs, primaryKey, fileKey, cancellationToken);
    }

    /// <summary>
    /// Parses the Pak file from file path. Should be disposed after use, as it keeps the file open (currently at least).
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <param name="primaryKey">Key for main decryption, or only the header part for newer Pak format.</param>
    /// <param name="fileKey">Alternative key to use for decrypting individual files. Ignored for TMF Pak (v3) format and older.</param>
    /// <returns>Parsed Pak format.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null.</exception>
    /// <exception cref="NotAPakException">Stream is not Pak-formatted.</exception>
    public static Pak Parse(string filePath, byte[]? primaryKey = null, byte[]? fileKey = null)
    {
        var fs = File.OpenRead(filePath);
        return Parse(fs, primaryKey, fileKey);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    protected async Task ReadHeaderAsync(Stream stream, CancellationToken cancellationToken)
    {
        var pak6 = this as Pak6;
        var r = new GbxReader(stream);

        HeaderMD5 = await r.ReadBytesAsync(16, cancellationToken);
        metadataStart = r.ReadInt32(); // offset to metadata section

        if (Version < 15)
        {
            dataStart = r.ReadInt32();
        }
        else
        {
            dataStart = pak6?.HeaderMaxSize ?? throw new InvalidOperationException("HeaderMaxSize is null.");
        }

        if (Version >= 2)
        {
            var gbxHeadersSize = r.ReadInt32();
            var gbxHeadersComprSize = r.ReadInt32();
        }

        if (Version >= 14)
        {
            r.SkipData(16); // unused

            if (Version >= 16)
            {
                var fileSize = r.ReadUInt32();
            }
        }

        if (Version >= 3)
        {
            r.SkipData(16); // unused

            if (Version == 6)
            {
                pak6?.ReadAuthorInfo(r);
            }
        }

        Flags = r.ReadInt32();

        var allFolders = ReadAllFolders(r);

        if (allFolders.Length > 2 && allFolders[2].Name.Length > 4)
        {
            var nameBytes = Encoding.Unicode.GetBytes(allFolders[2].Name);

            if (r.BaseStream is IEncryptionInitializer encryptionInitializer)
            {
                encryptionInitializer.Initialize(nameBytes, 4, 4);
            }
        }

        Files = ReadAllFiles(r, allFolders);
    }

    private static PakFolder[] ReadAllFolders(GbxReader r)
    {
        var numFolders = r.ReadInt32();
        var allFolders = new PakFolder[numFolders];

        for (var i = 0; i < numFolders; i++)
        {
            var parentFolderIndex = r.ReadInt32(); // index into folders; -1 if this is a root folder
            var name = r.ReadString();

            if (!name.EndsWith('\\') && !name.EndsWith('/'))
            {
                name += '\\';
            }

            allFolders[i] = new PakFolder(name, parentFolderIndex == -1 ? null : parentFolderIndex);
        }

        return allFolders;
    }

    private ImmutableDictionary<string, PakFile> ReadAllFiles(GbxReader r, PakFolder[] allFolders)
    {
        var files = ImmutableDictionary.CreateBuilder<string, PakFile>();

        var numFiles = r.ReadInt32();
        for (var i = 0; i < numFiles; i++)
        {
            var folderIndex = r.ReadInt32(); // index into folders
            var name = r.ReadString().Replace('\\', Path.DirectorySeparatorChar);
            var u01 = r.ReadInt32();
            var uncompressedSize = r.ReadInt32();
            var compressedSize = r.ReadInt32();
            var offset = r.ReadUInt32();
            var classId = r.ReadUInt32(); // indicates the type of the file
            var size = Version >= 17 ? r.ReadInt32() : default(int?);
            var checksum = Version >= 14 ? r.ReadUInt128() : default(UInt128?);

            var fileFlags = r.ReadUInt64();

            var folderPath = string.Join(Path.DirectorySeparatorChar, RecurseFoldersToParent(folderIndex, allFolders)
                .Reverse()
                .Select(f => f.Name.TrimEnd('\\')));
            var filePath = Path.Combine(folderPath, name);

            var file = new PakFile(name, folderPath, classId, offset, uncompressedSize, compressedSize, size, checksum, fileFlags);
            files[filePath] = file;
        }

        return files.ToImmutable();
    }

    private static IEnumerable<PakFolder> RecurseFoldersToParent(int folderIndex, PakFolder[] allFolders)
    {
        if (folderIndex == -1)
        {
            yield break;
        }

        var folder = allFolders[folderIndex];

        yield return folder;

        if (folder.ParentIndex is null)
        {
            yield break;
        }

        foreach (var f in RecurseFoldersToParent(folder.ParentIndex.Value, allFolders))
        {
            yield return f;
        }
    }

    public Stream OpenFile(PakFile file, out EncryptionInitializer? encryptionInitializer)
    {
        var fileKey = (this is Pak6) ? secondKey : key;

        stream.Position = dataStart + file.Offset;

        var newStream = stream;

        if (file.IsEncrypted)
        {
            var ivBuffer = new byte[8];
            if (stream.Read(ivBuffer, 0, 8) != 8)
            {
                throw new EndOfStreamException("Could not read IV from file.");
            }
            var iv = BitConverter.ToUInt64(ivBuffer, 0);

            if (fileKey is null)
            {
                throw new Exception("Encryption key is missing");
            }

            var blowfish = new BlowfishStream(newStream, fileKey, iv, Version == 18);

            encryptionInitializer = new EncryptionInitializer(blowfish);

            newStream = blowfish;
        }
        else
        {
            encryptionInitializer = null;
        }

        if (file.IsCompressed)
        {
            newStream = Version >= 18 ? 
                new LZ4Stream(newStream, file.UncompressedSize) : 
                new NativeZlibStream(newStream, CompressionMode.Decompress);
        }

        return new NonDisposingStream(newStream);
    }

    /// <summary>
    /// Attempts to open the Gbx file from Pak. If the file is not a Gbx file, <see cref="NotAGbxException"/> is thrown.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="settings"></param>
    /// <param name="importExternalNodesFromRefTable"></param>
    /// <param name="fileHashes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<Gbx> OpenGbxFileAsync(PakFile file, GbxReadSettings settings = default, bool importExternalNodesFromRefTable = false, IDictionary<string, string>? fileHashes = default, CancellationToken cancellationToken = default)
    {
        using var stream = OpenFile(file, out var encryptionInitializer);

        var settingsWithEncryption = settings with { EncryptionInitializer = encryptionInitializer };

        var gbx = await Gbx.ParseAsync(stream, settingsWithEncryption, cancellationToken);

        if (gbx.RefTable is not null && importExternalNodesFromRefTable)
        {
            // this can miss some files from other Pak files
            ImportExternalNodesFromRefTable(this, file, gbx.RefTable, settingsWithEncryption, fileHashes);
        }

        return gbx;
    }

    private static void ImportExternalNodesFromRefTable(Pak pak, PakFile file, GbxRefTable refTable, GbxReadSettings settings, IDictionary<string, string>? fileHashes)
    {
        var ancestor = string.Join('\\', Enumerable.Repeat("..", refTable.AncestorLevel));
        var currentFileName = fileHashes?.TryGetValue(file.Name, out var resolvedFileName) == true ? resolvedFileName : file.Name;
        var currentFileFolderPath = Path.GetDirectoryName(currentFileName);
        var currentPakFileFolderPath = string.IsNullOrEmpty(currentFileFolderPath) ? file.FolderPath : Path.Combine(file.FolderPath, currentFileFolderPath);

        foreach (var refTableFile in refTable.Files)
        {
            var filePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), Path.Combine(currentPakFileFolderPath, ancestor, refTableFile.FilePath)).Replace('/', '\\');

            if (!pak.Files.TryGetValue(filePath, out var refTableFileInPak))
            {
                var directoryPath = Path.GetDirectoryName(filePath);
                var fileName = Path.GetFileName(filePath);

                while (true)
                {
                    var hash = MD5.Compute136(fileName);

                    if (pak.Files.TryGetValue(directoryPath + "\\" + hash, out refTableFileInPak))
                    {
                        break;
                    }

                    fileName = Path.Combine(Path.GetFileName(directoryPath)!, fileName); // maybe can rarely fail here?
                    directoryPath = Path.GetDirectoryName(directoryPath);

                    if (string.IsNullOrEmpty(directoryPath))
                    {
                        break;
                    }
                }
            }

            if (refTableFileInPak is null)
            {
                continue;
            }

            refTable.ExternalNodes.Add(refTableFile.FilePath, () => pak.OpenGbxFile(refTableFileInPak, settings));
        }
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public Gbx OpenGbxFileHeader(PakFile file, GbxReadSettings settings = default)
    {
        using var stream = OpenFile(file, out var encryptionInitializer);
        return Gbx.ParseHeader(stream, settings with { EncryptionInitializer = encryptionInitializer });
    }

    public void Dispose()
    {
        stream.Dispose();
    }

#if NET5_0_OR_GREATER
    public async ValueTask DisposeAsync()
    {
        await stream.DisposeAsync();
    }
#endif

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="progress"></param>
    /// <param name="onlyUsedHashes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Dictionary where the key is the hash (file name) and value is the true resolved file name.</returns>
    public static async Task<Dictionary<string, string>> BruteforceFileHashesAsync(
        string directoryPath,
        PakList pakList,
        IProgress<KeyValuePair<string, string>>? progress = null,
        bool onlyUsedHashes = true,
        CancellationToken cancellationToken = default)
    {
        return await BruteforceFileHashesAsync(directoryPath,
            pakList.ToDictionary(x => x.Key, x => new PakKeyInfo(x.Value.Key)),
            progress,
            onlyUsedHashes,
            cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="progress"></param>
    /// <param name="onlyUsedHashes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Dictionary where the key is the hash (file name) and value is the true resolved file name.</returns>
    public static async Task<Dictionary<string, string>> BruteforceFileHashesAsync(
        string directoryPath,
        Dictionary<string, PakKeyInfo> keys,
        IProgress<KeyValuePair<string, string>>? progress = null,
        bool onlyUsedHashes = true,
        CancellationToken cancellationToken = default)
    {
        var allPossibleFileHashes = new Dictionary<string, string>();

        await foreach (var (pak, file) in EnumeratePakFilesAsync(directoryPath, keys, cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Gbx gbx;
            try
            {
                gbx = pak.OpenGbxFileHeader(file);
            }
            catch (NotAGbxException)
            {
                continue;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{file.Name} {e.Message}");
                continue;
            }

            var refTable = gbx.RefTable;

            if (refTable is null)
            {
                continue;
            }

            foreach (var refTableFile in refTable.Files)
            {
                var filePath = refTableFile.FilePath.Replace('/', '\\');
                var hash = MD5.Compute136(filePath);
                progress?.Report(new KeyValuePair<string, string>(hash, filePath));
                allPossibleFileHashes[hash] = filePath;

                while (filePath.Contains('\\'))
                {
                    filePath = filePath.Substring(filePath.IndexOf('\\') + 1);
                    hash = MD5.Compute136(filePath);
                    progress?.Report(new KeyValuePair<string, string>(hash, filePath));
                    allPossibleFileHashes[hash] = filePath;
                }
            }
        }

        if (!onlyUsedHashes)
        {
            return allPossibleFileHashes;
        }

        var usedHashes = new Dictionary<string, string>();

        await foreach (var (_, file) in EnumeratePakFilesAsync(directoryPath, keys, cancellationToken))
        {
            if (allPossibleFileHashes.TryGetValue(file.Name, out var name))
            {
                usedHashes[file.Name] = name;
            }
        }

        return usedHashes;
    }

    private static async IAsyncEnumerable<(Pak, PakFile)> EnumeratePakFilesAsync(
        string directoryPath,
        Dictionary<string, PakKeyInfo> keys, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var pakInfo in keys)
        {
            var fileName = $"{char.ToUpperInvariant(pakInfo.Key[0])}{pakInfo.Key.Substring(1)}.pak";
            var fullFileName = Path.Combine(directoryPath, fileName);

            if (!File.Exists(fullFileName))
            {
                continue;
            }

            using var pak = await ParseAsync(fullFileName, pakInfo.Value.PrimaryKey, pakInfo.Value.FileKey, cancellationToken: cancellationToken);

            foreach (var file in pak.Files.Values)
            {
                yield return (pak, file);
            }
        }
    }
}