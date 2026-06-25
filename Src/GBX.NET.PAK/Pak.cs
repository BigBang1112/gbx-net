using GBX.NET.Components;
using GBX.NET.Crypto;
using GBX.NET.Exceptions;
using GBX.NET.PAK.Exceptions;
using NativeSharpZlib;
using System.Buffers.Binary;
using System.Collections.Immutable;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

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

    private readonly Stream stream;
    private readonly byte[]? key;

    private static readonly byte[] headerKey = [
        0x56, 0xee, 0xcb, 0xbb, 0xde, 0xb6, 0xbc, 0x90,
        0xa1, 0x7d, 0xfc, 0xeb, 0x76, 0x1d, 0x59, 0xce
    ];

    public int Version { get; }

    public uint GbxHeadersStart { get; private set; }
    public int? GbxHeadersSize { get; private set; }
    public int? GbxHeadersComprSize { get; private set; }
    public int? HeaderMaxSize { get; protected set; }
    public uint? Size { get; private set; }
    public byte[]? HeaderMD5 { get; private set; }
    public uint Flags { get; private set; }
    public virtual bool IsHeaderPrivate => true;
    public virtual bool UseDefaultHeaderKey => false;
    public virtual bool IsHeaderEncrypted => true;

    public AuthorInfo? AuthorInfo { get; protected set; }

    public ImmutableDictionary<string, PakFile> Files { get; private set; } = ImmutableDictionary<string, PakFile>.Empty;

    protected Pak(Stream stream, byte[]? key, int version)
    {
        this.stream = stream;
        this.key = key;
        Version = version;
    }

    /// <summary>
    /// Computes the Blowfish key used to decrypt a Pak file, given the decrypted master server key and the Pak checksum (32 bytes). The resulting key is the encryption key, so make sure you set <c>keyType</c> to <see cref="KeyType.EncryptionKey"/>
    /// </summary>
    /// <param name="msKey">Decrypted key as a 16-byte array.</param>
    /// <param name="checksum">Pak checksum, exactly 32 bytes.</param>
    /// <returns>16-byte computed Blowfish key.</returns>
    public static byte[] ComputeKeyFromMasterServer(byte[] msKey, byte[] checksum)
    {
        ArgumentNullException.ThrowIfNull(checksum);
        ArgumentNullException.ThrowIfNull(msKey);

        if (checksum.Length != 32)
            throw new ArgumentException("Checksum must be exactly 32 bytes.", nameof(checksum));

        if (msKey.Length != 16)
            throw new ArgumentException("Key must be exactly 16 bytes.", nameof(msKey));

        var derivedKey = new byte[16];

        for (var i = 0; i < 8; i++)
        {
            derivedKey[i] = (byte)(msKey[15 - i] ^ checksum[i]);
            derivedKey[i + 8] = (byte)(msKey[7 - i] ^ checksum[i + 16]);
        }

        return derivedKey;
    }

    /// <summary>
    /// Computes the Blowfish key used to decrypt a Pak file, given the base key. The resulting key is the encryption key, so make sure you set <c>keyType</c> to <see cref="KeyType.EncryptionKey"/> in parse methods.
    /// </summary>
    /// <param name="baseKey">Base key as a 16-byte array.</param>
    /// <returns>16-byte computed Blowfish key.</returns>
    public static byte[] ComputeKey(byte[] baseKey)
    {
        return MD5.Compute(Encoding.ASCII.GetBytes(Convert.ToHexString(baseKey) + Magic));
    }

    /// <summary>
    /// Parses the Pak file from the stream. Should be disposed after use, as it keeps the file open (currently at least).
    /// </summary>
    /// <param name="stream">Stream.</param>
    /// <param name="key">Key for decryption.</param>
    /// <param name="keyType">Type of the key provided.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task. The task result contains the parsed Pak format.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
    /// <exception cref="NotAPakException">Stream is not Pak-formatted.</exception>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<Pak> ParseAsync(Stream stream, byte[]? key = null, KeyType keyType = KeyType.BaseKey, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);

        var r = new AsyncGbxReader(stream);

        if (!await r.ReadPakMagicAsync(cancellationToken))
        {
            throw new NotAPakException();
        }

        var version = await r.ReadInt32Async(cancellationToken);

        Pak pak;

        if (version < 6)
        {
            if (keyType == KeyType.MasterServerKey)
            {
                throw new ArgumentException("Master server key cannot be used for Pak versions below 6, as they don't have the checksum in the header.", nameof(keyType));
            }

            if (keyType == KeyType.BaseKey && key is not null)
            {
                key = ComputeKey(key);
            }

            pak = new Pak(stream, key, version);
        }
        else
        {
            var checksum = await r.ReadBytesAsync(32, cancellationToken);

            if (key is not null)
            {
                switch (keyType)
                {
                    case KeyType.BaseKey:
                        key = ComputeKey(key);
                        break;
                    case KeyType.MasterServerKey:
                        key = ComputeKeyFromMasterServer(key, checksum);
                        break;
                }
            }

            pak = new Pak6(stream, key, version, checksum);
        }

        await pak.ReadHeaderAsync(stream, r, version, cancellationToken);

        return pak;
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    internal virtual async Task ReadHeaderAsync(Stream stream, AsyncGbxReader r, int version, CancellationToken cancellationToken)
    {
        if (!IsHeaderEncrypted)
        {
            await ReadHeaderAsync(stream, cancellationToken);
            return;
        }

        if (IsHeaderPrivate && key is null)
        {
            return;
        }

        var keyForHeader = new byte[16];

        if (IsHeaderPrivate)
        {
            if (key is null)
            {
                throw new Exception("Key is required for header decryption.");
            }

            Array.Copy(key, keyForHeader, 16);
        }

        if (UseDefaultHeaderKey)
        {
            for (var i = 0; i < 16; i++)
            {
                keyForHeader[i] ^= headerKey[i];
            }
        }

        var iv = await r.ReadUInt64Async(cancellationToken);
        var blowfishStream = new BlowfishStream(stream, keyForHeader, iv, version);

        await ReadHeaderAsync(blowfishStream, cancellationToken);
    }

    /// <summary>
    /// Parses the Pak file from file path. Should be disposed after use, as it keeps the file open (currently at least).
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <param name="key">Key for decryption.</param>
    /// <param name="keyType">Type of the key for decryption.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task. The task result contains the parsed Pak format.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null.</exception>
    /// <exception cref="NotAPakException">Stream is not Pak-formatted.</exception>
    public static async Task<Pak> ParseAsync(string filePath, byte[]? key = null, KeyType keyType = KeyType.BaseKey, CancellationToken cancellationToken = default)
    {
        var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await ParseAsync(fs, key, keyType, cancellationToken);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    private async Task ReadHeaderAsync(Stream stream, CancellationToken cancellationToken)
    {
        var r = new AsyncGbxReader(stream);

        HeaderMD5 = await r.ReadBytesAsync(16, cancellationToken);
        GbxHeadersStart = await r.ReadUInt32Async(cancellationToken); // offset to metadata section

        if (Version < 15)
        {
            HeaderMaxSize = await r.ReadInt32Async(cancellationToken); // data start
        }

        if (Version >= 2)
        {
            GbxHeadersSize = await r.ReadInt32Async(cancellationToken);
            GbxHeadersComprSize = await r.ReadInt32Async(cancellationToken);
        }

        if (Version >= 14)
        {
            await r.ReadBytesAsync(16, cancellationToken); // unused

            if (Version >= 16)
            {
                Size = await r.ReadUInt32Async(cancellationToken);
            }
        }

        if (Version >= 3)
        {
            await r.ReadBytesAsync(16, cancellationToken); // unused

            if (Version == 6)
            {
                AuthorInfo = await ReadAuthorInfoAsync(r, cancellationToken);
            }
        }

        Flags = await r.ReadUInt32Async(cancellationToken);

        var allFolders = await ReadAllFoldersAsync(r, cancellationToken);

        if (allFolders.Length > 2 && allFolders[2].Name.Length > 4)
        {
            var nameBytes = Encoding.Unicode.GetBytes(allFolders[2].Name);

            if (stream is IEncryptionInitializer encryptionInitializer)
            {
                encryptionInitializer.Initialize(nameBytes, 4, 4);
            }
        }

        Files = await ReadAllFilesAsync(r, allFolders, cancellationToken);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    internal static async Task<AuthorInfo> ReadAuthorInfoAsync(AsyncGbxReader r, CancellationToken cancellationToken)
    {
        return new AuthorInfo
        {
            AuthorVersion = await r.ReadInt32Async(cancellationToken),
            AuthorLogin = await r.ReadStringAsync(cancellationToken),
            AuthorNickname = await r.ReadStringAsync(cancellationToken),
            AuthorZone = await r.ReadStringAsync(cancellationToken),
            AuthorExtraInfo = await r.ReadStringAsync(cancellationToken)
        };
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    private static async Task<PakFolder[]> ReadAllFoldersAsync(AsyncGbxReader r, CancellationToken cancellationToken)
    {
        var numFolders = await r.ReadInt32Async(cancellationToken);
        var allFolders = new PakFolder[numFolders];

        for (var i = 0; i < numFolders; i++)
        {
            var parentFolderIndex = await r.ReadInt32Async(cancellationToken); // index into folders; -1 if this is a root folder
            var name = await r.ReadStringAsync(cancellationToken);

            if (!name.EndsWith('\\') && !name.EndsWith('/'))
            {
                name += '\\';
            }

            allFolders[i] = new PakFolder(name, parentFolderIndex == -1 ? null : parentFolderIndex);
        }

        return allFolders;
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    private async Task<ImmutableDictionary<string, PakFile>> ReadAllFilesAsync(AsyncGbxReader r, PakFolder[] allFolders, CancellationToken cancellationToken)
    {
        var files = ImmutableDictionary.CreateBuilder<string, PakFile>();

        var numFiles = await r.ReadInt32Async(cancellationToken);
        for (var i = 0; i < numFiles; i++)
        {
            var folderIndex = await r.ReadInt32Async(cancellationToken); // index into folders
            var name = (await r.ReadStringAsync(cancellationToken)).Replace('\\', Path.DirectorySeparatorChar); // should this replacement really happen?
            var u01 = await r.ReadInt32Async(cancellationToken);
            var uncompressedSize = await r.ReadInt32Async(cancellationToken);
            var compressedSize = await r.ReadInt32Async(cancellationToken);
            var offset = await r.ReadUInt32Async(cancellationToken);
            var classId = await r.ReadUInt32Async(cancellationToken); // indicates the type of the file
            var size = Version >= 17 ? await r.ReadInt32Async(cancellationToken) : default(int?);
            var checksum = Version >= 14 ? await r.ReadUInt128Async(cancellationToken) : default(UInt128?);

            var fileFlags = await r.ReadUInt64Async(cancellationToken);

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
        if (HeaderMaxSize is null)
        {
            throw new Exception("Cannot open file.");
        }

        stream.Position = HeaderMaxSize.Value + file.Offset;

        var newStream = stream;

        if (file.IsEncrypted)
        {
            var ivBuffer = new byte[8];
            stream.ReadExactly(ivBuffer);
            var iv = BitConverter.ToUInt64(ivBuffer, 0);

            if (key is null)
            {
                throw new Exception("Encryption key is missing");
            }

            var blowfish = new BlowfishStream(newStream, key, iv, Version);

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

        // Only wrap in NonDisposingStream when newStream IS the underlying pak stream (no wrappers
        // created). BlowfishStream and LZ4Stream do not propagate Dispose to their inner streams,
        // so returning them directly lets the caller properly dispose any unmanaged resources (e.g.
        // the Marshal.AllocHGlobal buffers inside LZ4Stream) without ever touching the pak stream.
        return ReferenceEquals(newStream, stream) ? new NonDisposingStream(newStream) : newStream;
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

        if (!file.DontUseDummyWrite)
        {
            settings = settings with { EncryptionInitializer = encryptionInitializer };
        }

        var gbx = await Gbx.ParseAsync(stream, settings, cancellationToken);

        if (gbx.RefTable is not null && importExternalNodesFromRefTable)
        {
            // this can miss some files from other Pak files
            ImportExternalNodesFromRefTable(this, file, gbx.RefTable, settings, fileHashes);
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

    public Gbx OpenGbxFileHeader(PakFile file, GbxReadSettings settings = default)
    {
        using var stream = OpenFile(file, out var encryptionInitializer);
        return Gbx.ParseHeader(stream, settings with { EncryptionInitializer = encryptionInitializer });
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<bool> CheckFileIsGbxAsync(PakFile file, CancellationToken cancellationToken = default)
    {
        if (file.Size == 0)
        {
            return false;
        }

        using var stream = OpenFile(file, out var _);
        return await Gbx.IsGbxAsync(stream, cancellationToken);
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
    /// <param name="game"></param>
    /// <param name="progress"></param>
    /// <param name="keepUnresolvedHashes"></param>
    /// <param name="additionalFileHashes">Additional file hashes to reference that are not found during the scan.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Dictionary where the key is the hash (file name) and value is the true resolved file name.</returns>
    public static async Task<Dictionary<string, string>> BruteforceFileHashesAsync(
        string directoryPath,
        PakListGame game = PakListGame.TM,
        IProgress<KeyValuePair<string, string>>? progress = null,
        bool keepUnresolvedHashes = false,
        IEnumerable<string>? additionalFileHashes = null,
        CancellationToken cancellationToken = default)
    {
        var pakListFilePath = Path.Combine(directoryPath, PakList.FileName);

        if (!File.Exists(pakListFilePath))
        {
            return [];
        }

        var pakList = await PakList.ParseAsync(pakListFilePath, game, cancellationToken);

        return await BruteforceFileHashesAsync(directoryPath,
            pakList.ToDictionary(x => x.Key, x => (byte[]?)Convert.FromHexString(x.Value.Key)),
            progress,
            keepUnresolvedHashes,
            additionalFileHashes,
            cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="keys"></param>
    /// <param name="progress"></param>
    /// <param name="keepUnresolvedHashes"></param>
    /// <param name="additionalFileHashes">Additional file hashes to reference that are not found during the scan.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Dictionary where the key is the hash (file name) and value is the true resolved file name.</returns>
    public static async Task<Dictionary<string, string>> BruteforceFileHashesAsync(
        string directoryPath,
        Dictionary<string, byte[]?> keys,
        IProgress<KeyValuePair<string, string>>? progress = null,
        bool keepUnresolvedHashes = false,
        IEnumerable<string>? additionalFileHashes = null,
        CancellationToken cancellationToken = default)
    {
        var allPossibleFileHashes = new Dictionary<string, string>();
        var foundFileNames = new HashSet<string>(additionalFileHashes ?? []);

        await foreach (var (pak, file) in EnumeratePakFilesAsync(directoryPath, keys, cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            foundFileNames.Add(file.Name);

            // skip non-decryptable files
            if (pak.key is null && file.IsEncrypted)
            {
                continue;
            }

            try
            { 
                // only gbx files can be checked for reference table
                if (!await pak.CheckFileIsGbxAsync(file, cancellationToken))
                {
                    // CPlugFileTextScript or CPlugFileText
                    if (file.ClassId is 0x09054000)
                    {
                        foreach (var (hash, fileName) in ResolveHashesInTextFile(pak, file))
                        {
                            allPossibleFileHashes[hash] = fileName;
                        }
                    }

                    continue;
                }
            }
            catch (Exception)
            {
                continue;
            }

            Gbx gbx;
            try
            {
                gbx = pak.OpenGbxFileHeader(file);
            }
            catch (NotAGbxException)
            {
                continue;
            }
            catch
            {
                continue;
            }

            if (gbx.RefTable is null)
            {
                continue;
            }

            foreach (var refTableFile in gbx.RefTable.Files)
            {
                var filePath = refTableFile.FilePath.Replace('/', '\\');

                var hash = MD5.Compute136(filePath);
                if (!allPossibleFileHashes.ContainsKey(hash))
                {
                    progress?.Report(new KeyValuePair<string, string>(hash, filePath));
                    allPossibleFileHashes[hash] = filePath;
                }

                var filePathDir = Path.GetDirectoryName(file.Name);
                if (!string.IsNullOrEmpty(filePathDir))
                {
                    var filePathWithDir = $"{filePathDir}\\{filePath}";
                    hash = MD5.Compute136(filePathWithDir);
                    if (!allPossibleFileHashes.ContainsKey(hash))
                    {
                        progress?.Report(new KeyValuePair<string, string>(hash, filePathWithDir));
                        allPossibleFileHashes[hash] = filePathWithDir;
                    }
                }

                while (filePath.Contains('\\'))
                {
                    filePath = filePath.Substring(filePath.IndexOf('\\') + 1);
                    hash = MD5.Compute136(filePath);

                    if (!allPossibleFileHashes.ContainsKey(hash))
                    {
                        progress?.Report(new KeyValuePair<string, string>(hash, filePath));
                        allPossibleFileHashes[hash] = filePath;
                    }
                }
            }
        }

        var usedHashes = new Dictionary<string, string>();

        foreach (var fileName in foundFileNames)
        {
            if (allPossibleFileHashes.TryGetValue(fileName, out var name))
            {
                usedHashes[fileName] = name;
            }
            else if (keepUnresolvedHashes && HashGuessRegex().IsMatch(fileName))
            {
                usedHashes[fileName] = "";
            }
        }

        return usedHashes;
    }

    private static async IAsyncEnumerable<(Pak, PakFile)> EnumeratePakFilesAsync(
        string directoryPath,
        Dictionary<string, byte[]?> keys, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var filePath in Directory.EnumerateFiles(directoryPath)
            .Where(x => x.EndsWith(".pak", StringComparison.OrdinalIgnoreCase) || x.EndsWith(".Pack.Gbx", StringComparison.OrdinalIgnoreCase)))
        {
            var identifier = Path.GetFileNameWithoutExtension(filePath);
            var key = keys.GetValueOrDefault(identifier);

            Pak pak;

            try
            {
                pak = await ParseAsync(filePath, key, cancellationToken: cancellationToken);
            }
            catch
            {
                continue;
            }

            foreach (var file in pak.Files.Values)
            {
                yield return (pak, file);
            }

            await pak.DisposeAsync();
        }
    }

    private static IEnumerable<KeyValuePair<string, string>> ResolveHashesInTextFile(Pak pak, PakFile file)
    {
        using var stream = pak.OpenFile(file, out _);
        using var reader = new StreamReader(stream);

        var text = reader.ReadToEnd();

        if (string.IsNullOrEmpty(text))
        {
            yield break;
        }

        foreach (var match in ScriptNameRegex().Matches(text)
            .Concat(IncludeRegex().Matches(text)).Cast<Match>()
            .Concat(NameRegex().Matches(text)).Cast<Match>())
        {
            var filePath = Path.GetFileName(match.Groups[1].Value);
            var hash = MD5.Compute136(filePath);
            yield return new KeyValuePair<string, string>(hash, filePath);
        }
    }

    [GeneratedRegex("^[0-9a-fA-F]{34}$")]
    private static partial Regex HashGuessRegex();

    [GeneratedRegex(@"#Const[\s]+[\w]*ScriptName[\s]+""([\w.\\/@]+)""")]
    private static partial Regex ScriptNameRegex();

    [GeneratedRegex(@"#(?:Include|Extends)[\s]+""([\w.\\/@]+)""")]
    private static partial Regex IncludeRegex();

    [GeneratedRegex(@"(?:name|url)=""([\w.\\/@]+)""")]
    private static partial Regex NameRegex();
}