using GBX.NET.Crypto;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;
using NativeSharpZlib;
using System.Collections.Immutable;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;

namespace GBX.NET.PAK;

public sealed partial class Pak : IDisposable
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
    private readonly byte[] key;
    private readonly byte[] headerMD5;
    private readonly int metadataStart;
    private readonly int dataStart;

    public int Version { get; }
    public int Flags { get; }
    public ImmutableDictionary<string, PakFile> Files { get; }

    private Pak(Stream stream,
        byte[] key,
        int version,
        byte[] headerMD5,
        int metadataStart,
        int dataStart,
        int flags,
        ImmutableDictionary<string, PakFile> files)
    {
        this.stream = stream;
        this.key = key;
        Version = version;
        this.headerMD5 = headerMD5;
        this.metadataStart = metadataStart;
        this.dataStart = dataStart;
        Flags = flags;
        Files = files;
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<Pak> ParseAsync(Stream stream, byte[] key, CancellationToken cancellationToken = default)
    {
        var r = new GbxReader(stream);

        if (!r.ReadPakMagic())
        {
            throw new NotAPakException();
        }

        var version = r.ReadInt32();

        if (version > 5)
        {
            throw new Exception($"Version >5 (actual: {version}) is not supported. Use Pak6 for this file.");
        }

        var headerIV = r.ReadUInt64();

        var decryptStream = new BlowfishStream(stream, key, headerIV);
        var decryptReader = new GbxReader(decryptStream);
        
        return await ParseEncryptedAsync(decryptReader, stream, version, key, cancellationToken);
    }

    public static async Task<Pak> ParseAsync(string filePath, byte[] key, CancellationToken cancellationToken = default)
    {
        var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await ParseAsync(fs, key, cancellationToken);
    }

    public static Pak Parse(string filePath, byte[] key)
    {
        var fs = File.OpenRead(filePath);
        return Parse(fs, key);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    private static async Task<Pak> ParseEncryptedAsync(
        GbxReader r, 
        Stream originalStream, 
        int version,
        byte[] key,
        CancellationToken cancellationToken)
    {
        var headerMD5 = await r.ReadBytesAsync(16, cancellationToken);
        var metadataStart = r.ReadInt32(); // offset to metadata section
        var dataStart = r.ReadInt32();

        if (version >= 2)
        {
            var gbxHeadersSize = r.ReadInt32();
            var gbxHeadersComprSize = r.ReadInt32();
        }

        if (version >= 3)
        {
            r.SkipData(16); // unused
        }

        var flags = r.ReadInt32();

        var allFolders = ReadAllFolders(r);

        if (allFolders.Length > 2 && allFolders[2].Name.Length > 4)
        {
            var nameBytes = Encoding.Unicode.GetBytes(allFolders[2].Name);
            ((IEncryptionInitializer)r.BaseStream).Initialize(nameBytes, 4, 4);
        }

        var files = ReadAllFiles(r, allFolders);

        return new Pak(originalStream, key, version, headerMD5, metadataStart, dataStart, flags, files);
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

    private static ImmutableDictionary<string, PakFile> ReadAllFiles(GbxReader r, PakFolder[] allFolders)
    {
        var files = ImmutableDictionary.CreateBuilder<string, PakFile>();

        var numFiles = r.ReadInt32();
        for (var i = 0; i < numFiles; i++)
        {
            var folderIndex = r.ReadInt32(); // index into folders
            var name = r.ReadString();
            var u01 = r.ReadInt32();
            var uncompressedSize = r.ReadInt32();
            var compressedSize = r.ReadInt32();
            var offset = r.ReadInt32();
            var classId = r.ReadUInt32(); // indicates the type of the file
            var fileFlags = r.ReadUInt64();

            var folderPath = string.Concat(RecurseFoldersToParent(folderIndex, allFolders)
                .Reverse()
                .Select(f => f.Name));
            var filePath = string.Concat(folderPath, name);

            var file = new PakFile(name, folderPath, classId, offset, uncompressedSize, compressedSize, fileFlags);
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

    public Stream OpenFile(PakFile file, out EncryptionInitializer encryptionInitializer)
    {
        stream.Position = dataStart + file.Offset;

        var ivBuffer = new byte[8];
        if (stream.Read(ivBuffer, 0, 8) != 8)
        {
            throw new EndOfStreamException("Could not read IV from file.");
        }
        var iv = BitConverter.ToUInt64(ivBuffer, 0);

        var blowfish = new BlowfishStream(stream, key, iv);

        encryptionInitializer = new EncryptionInitializer(blowfish);

        if (!file.IsCompressed)
        {
            return blowfish;
        }

        return new NativeZlibStream(blowfish, CompressionMode.Decompress);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<Gbx> OpenGbxFileAsync(PakFile file, GbxReadSettings settings = default, CancellationToken cancellationToken = default)
    {
        using var stream = OpenFile(file, out var encryptionInitializer);
        return await Gbx.ParseAsync(stream, settings with { EncryptionInitializer = encryptionInitializer }, cancellationToken);
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
        IProgress<KeyValuePair<string, string>>? progress = null,
        bool onlyUsedHashes = true,
        CancellationToken cancellationToken = default)
    {
        var pakList = await PakList.ParseAsync(Path.Combine(directoryPath, PakListFileName), cancellationToken);

        var allPossibleFileHashes = new Dictionary<string, string>();

        await foreach (var (pak, file) in EnumeratePakFilesAsync(directoryPath, pakList, cancellationToken))
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

            var refTable = gbx.RefTable;

            if (refTable is null)
            {
                continue;
            }

            foreach (var refTableFile in refTable.Files)
            {
                var filePath = refTableFile.FilePath;
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

        await foreach (var (_, file) in EnumeratePakFilesAsync(directoryPath, pakList, cancellationToken))
        {
            if (allPossibleFileHashes.TryGetValue(file.Name, out var name))
            {
                usedHashes[file.Name] = name;
            }
        }

        return usedHashes;
    }

    private static async IAsyncEnumerable<(Pak, PakFile)> EnumeratePakFilesAsync(string directoryPath, PakList pakList, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var pakInfo in pakList)
        {
            var fileName = $"{char.ToUpperInvariant(pakInfo.Key[0])}{pakInfo.Key.Substring(1)}.pak";
            var fullFileName = Path.Combine(directoryPath, fileName);

            if (!File.Exists(fullFileName))
            {
                continue;
            }

            using var pak = await ParseAsync(fullFileName, pakInfo.Value.Key, cancellationToken);

            foreach (var file in pak.Files.Values)
            {
                yield return (pak, file);
            }
        }
    }
}