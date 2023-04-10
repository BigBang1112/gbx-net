using GBX.NET.Debugging;

namespace GBX.NET;

/// <summary>
/// An unknown serialized GameBox node with additional attributes. This class can represent deserialized .Gbx file.
/// </summary>
public partial class GameBox
{
    public const string Magic = "GBX";

    public GameBoxHeader Header { get; }
    public GameBoxRefTable? RefTable { get; }

    /// <summary>
    /// If to ignore exceptions on certain chunk versions that are unknown, but shouldn't crash the reading/writing, however, could return unexpected values.
    /// </summary>
    /// <remarks>Example where this could happen is <see cref="CGameCtnMediaBlockCameraCustom.Key.ReadWrite(GameBoxReaderWriter, int)"/>.</remarks>
    public static bool IgnoreUnseenVersions { get; set; }

    /// <summary>
    /// If to strictly throw an exception when the supposedly-read boolean is not 0 or 1.
    /// </summary>
    public static bool StrictBooleans { get; set; }

    /// <summary>
    /// If to come back at the original position of an unskippable chunk, to read the pure data form without the usage of "double mode" <see cref="GameBoxReaderWriter"/>. Doesn't work with "read uncompressed body directly" option.
    /// </summary>
    public static bool SeekForRawChunkData { get; set; }

    /// <summary>
    /// Solves the occasional bug with OpenPlanet extraction where the header chunks are not properly written into the Gbx, while the length of this data section is still set to a non-zero value.
    /// </summary>
    public static bool OpenPlanetHookExtractMode { get; set; }

    public Node? Node { get; private set; }
    public GameBoxBody? RawBody { get; private set; }
    public GameBoxBodyDebugger? Debugger { get; private set; }

    internal GbxState? State { get; set; }

    public IExternalGameData? ExternalGameData { get; set; }

    /// <summary>
    /// File path of the Gbx.
    /// </summary>
    public string? FileName { get; }

    /// <summary>
    /// File path inside the Pak.
    /// </summary>
    internal string? PakFileName { get; set; }

    /// <summary>
    /// Creates a new GameBox object with the most common parameters without the node (object).
    /// </summary>
    /// <param name="id">ID of the expected node - should be provided remapped to latest.</param>
    public GameBox(uint id)
    {
        Header = new GameBoxHeader(id);
    }

    /// <summary>
    /// Creates a new GameBox object with the most common parameters.
    /// </summary>
    /// <param name="node">Node to wrap in.</param>
    public GameBox(Node node)
    {
        Header = new GameBoxHeader(node.Id);
        Node = node;
        Node.SetGbx(this);
    }

    public GameBox(GameBoxHeader header, GameBoxRefTable? refTable, string? fileName = null)
    {
        Header = header;
        RefTable = refTable;

        FileName = fileName;
    }

    /// <summary>
    /// Tries to get the <see cref="Node"/> of this GBX.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="Node"/> to look for.</typeparam>
    /// <param name="node">A node that is being extracted from this <see cref="GameBox"/> object. Null if unsuccessful.</param>
    /// <returns>True if the type of this <see cref="GameBox"/> is <see cref="GameBox{T}"/> and <typeparamref name="T"/> matches. Otherwise false.</returns>
    public bool TryNode<T>(out T? node) where T : Node
    {
        var property = GetType().GetProperty(nameof(Node));

        if (property?.PropertyType == typeof(T))
        {
            node = property.GetValue(this) as T;
            return true;
        }

        node = null;
        return false;
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="HeaderOnlyParseLimitationException">Writing is not supported in <see cref="GameBox"/> where only the header was parsed (without raw body being read).</exception>
    internal void Write(Stream stream, IDRemap remap, ILogger? logger)
    {
        logger?.LogDebug("Writing the body...");

        using var ms = new MemoryStream();
        using var bodyW = new GameBoxWriter(ms, remap, asyncAction: null, logger, new());

        (RawBody ?? new GameBoxBody()).Write(this, bodyW);

        logger?.LogDebug("Writing the header...");

        using var headerW = new GameBoxWriter(stream, remap, asyncAction: null, logger, new());

        Header.Write(Node ?? throw new ThisShouldNotHappenException(), headerW);

        // Num nodes
        headerW.Write(RawBody is null ? bodyW.State.AuxNodes.Count + bodyW.State.ExtAuxNodes.Count + 1 : Header.NumNodes);

        logger?.LogDebug("Writing the reference table...");

        if (RefTable is null)
        {
            headerW.Write(0);
        }
        else
        {
            RefTable.Write(Header, headerW);
        }

        headerW.Write(ms.ToArray());
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="HeaderOnlyParseLimitationException">Writing is not supported in <see cref="GameBox"/> where only the header was parsed (without raw body being read).</exception>
    internal async Task WriteAsync(Stream stream, IDRemap remap, ILogger? logger, GameBoxAsyncWriteAction? asyncAction, CancellationToken cancellationToken)
    {
        logger?.LogDebug("Writing the body...");

        using var ms = new MemoryStream();
        using var bodyW = new GameBoxWriter(ms, remap, asyncAction, logger, new());

        await (RawBody ?? new GameBoxBody()).WriteAsync(this, bodyW, cancellationToken);

        logger?.LogDebug("Writing the header...");

        using var headerW = new GameBoxWriter(stream, remap, asyncAction, logger, new());

        Header.Write(Node ?? throw new ThisShouldNotHappenException(), headerW);

        // Num nodes
        headerW.Write(RawBody is null ? bodyW.State.AuxNodes.Count + bodyW.State.ExtAuxNodes.Count + 1 : Header.NumNodes);

        logger?.LogDebug("Writing the reference table...");

        if (RefTable is null)
            headerW.Write(0);
        else
            RefTable.Write(Header, headerW);

        logger?.LogDebug("Copying body content...");

        headerW.Write(ms.ToArray());
    }

    /// <summary>
    /// Saves the serialized <see cref="GameBox{T}"/> to a stream.
    /// </summary>
    /// <param name="stream">Any kind of stream that supports writing.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="HeaderOnlyParseLimitationException">Saving is not supported in <see cref="GameBox"/> where only the header was parsed (without raw body being read).</exception>
    public void Save(Stream stream, IDRemap remap = default, ILogger? logger = null)
    {
        Write(stream, remap, logger);
    }

    /// <summary>
    /// Saves the serialized <see cref="GameBox{T}"/> on a disk.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path. Null will pick the <see cref="GameBox.FileName"/> value instead.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    /// <exception cref="PropertyNullException"><see cref="GameBox.FileName"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="fileName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by System.IO.Path.InvalidPathChars.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fileName"/> is null.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="fileName"/> specified a file that is read-only. -or- <paramref name="fileName"/> specified a file that is hidden. -or- This operation is not supported on the current platform. -or- <paramref name="fileName"/> specified a directory. -or- The caller does not have the required permission.</exception>
    /// <exception cref="NotSupportedException"><paramref name="fileName"/> is in an invalid format.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="HeaderOnlyParseLimitationException">Saving is not supported in <see cref="GameBox"/> where only the header was parsed (without raw body being read).</exception>
    public void Save(string? fileName = default, IDRemap remap = default, ILogger? logger = null)
    {
        fileName ??= (FileName ?? throw new PropertyNullException(nameof(FileName)));

        using var fs = File.Create(fileName);

        Save(fs, remap);

        logger?.LogDebug("GBX file {fileName} saved.", fileName);
    }

    /// <summary>
    /// Saves the serialized <see cref="GameBox{T}"/> to a stream.
    /// </summary>
    /// <param name="stream">Any kind of stream that supports writing.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="asyncAction">Specialized executions during asynchronous writing.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="HeaderOnlyParseLimitationException">Saving is not supported in <see cref="GameBox"/> where only the header was parsed (without raw body being read).</exception>
    public async Task SaveAsync(Stream stream, IDRemap remap = default, ILogger? logger = null, GameBoxAsyncWriteAction? asyncAction = null, CancellationToken cancellationToken = default)
    {
        await WriteAsync(stream, remap, logger, asyncAction, cancellationToken);
    }

    /// <summary>
    /// Saves the serialized <see cref="GameBox{T}"/> on a disk.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path. Null will pick the <see cref="FileName"/> value instead.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="asyncAction">Specialized executions during asynchronous writing.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="PropertyNullException"><see cref="FileName"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="fileName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by System.IO.Path.InvalidPathChars.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fileName"/> is null.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="fileName"/> specified a file that is read-only. -or- <paramref name="fileName"/> specified a file that is hidden. -or- This operation is not supported on the current platform. -or- <paramref name="fileName"/> specified a directory. -or- The caller does not have the required permission.</exception>
    /// <exception cref="NotSupportedException"><paramref name="fileName"/> is in an invalid format.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="HeaderOnlyParseLimitationException">Saving is not supported in <see cref="GameBox"/> where only the header was parsed (without raw body being read).</exception>
    public async Task SaveAsync(string? fileName = default, IDRemap remap = default, ILogger? logger = null, GameBoxAsyncWriteAction? asyncAction = null, CancellationToken cancellationToken = default)
    {
        fileName ??= (FileName ?? throw new PropertyNullException(nameof(FileName)));

        using var fs = File.Create(fileName);

        await SaveAsync(fs, remap, logger, asyncAction, cancellationToken);

        logger?.LogDebug("GBX file {fileName} saved.", fileName);
    }

    /// <summary>
    /// Changes the compression of the body to apply on saving. This is not supported for header-only parses.
    /// </summary>
    /// <param name="compression">Compression type.</param>
    /// <exception cref="HeaderOnlyParseLimitationException"></exception>
    public void ChangeBodyCompression(GameBoxCompression compression)
    {
        if (RawBody is not null)
        {
            throw new HeaderOnlyParseLimitationException("Compression cannot be changed with RawBody parse.");
        }

        Header.CompressionOfBody = compression;
    }

    /// <summary>
    /// Implicitly casts <see cref="GameBox"/> to its <see cref="Node"/>.
    /// </summary>
    /// <param name="gbx"></param>
    public static implicit operator Node?(GameBox gbx) => gbx.Node;

    private static uint? ReadNodeID(GameBoxReader reader)
    {
        if (!reader.HasMagic(Magic)) // If the file doesn't have GBX magic
            return null;

        var version = reader.ReadInt16(); // Version

        if (version < 3)
            return null;
        
        reader.ReadBytes(3);

        if (version >= 4)
            reader.ReadByte();

        return reader.ReadUInt32();
    }

    /// <summary>
    /// Reads the GBX node ID the quickest possible way.
    /// </summary>
    /// <param name="stream">Stream to read from.</param>
    /// <returns>ID of the main node presented in GBX. Null if not a GBX stream.</returns>
    public static uint? ReadNodeID(Stream stream)
    {
        using var r = new GameBoxReader(stream);
        return ReadNodeID(r);
    }

    /// <summary>
    /// Reads the GBX node ID the quickest possible way.
    /// </summary>
    /// <param name="fileName">File to read from.</param>
    /// <returns>ID of the main node presented in GBX. Null if not a GBX stream.</returns>
    public static uint? ReadNodeID(string fileName)
    {
        using var fs = File.OpenRead(fileName);
        return ReadNodeID(fs);
    }

    private static Type? ReadNodeType(GameBoxReader reader)
    {
        var classId = ReadNodeID(reader);

        if (classId.HasValue)
        {
            return NodeManager.GetClassTypeById(Node.RemapToLatest(classId.Value));
        }

        return null;
    }

    /// <summary>
    /// Reads the type of the main node from GBX file.
    /// </summary>
    /// <param name="fileName">File to read from.</param>
    /// <returns>Type of the main node.</returns>
    public static Type? ReadNodeType(string fileName)
    {
        using var fs = File.OpenRead(fileName);
        return ReadNodeType(fs);
    }

    /// <summary>
    /// Reads the type of the main node from GBX stream.
    /// </summary>
    /// <param name="stream">Stream to read from.</param>
    /// <returns>Type of the main node.</returns>
    public static Type? ReadNodeType(Stream stream)
    {
        using var r = new GameBoxReader(stream);
        return ReadNodeType(r);
    }

    /// <summary>
    /// Decompresses the body part of the GBX file, also setting the header parameter so that the outputted GBX file is compatible with the game. If the file is already detected decompressed, the input is just copied over to the output and false is returned, otherwise true.
    /// </summary>
    /// <param name="input">GBX stream to decompress.</param>
    /// <param name="output">Output GBX stream in the decompressed form.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">One of the streams is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="VersionNotSupportedException">GBX files below version 3 are not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static bool Decompress(Stream input, Stream output)
    {
        return GbxCompressor.Decompress(input, output);
    }

    /// <summary>
    /// Decompresses the body part of the GBX file, also setting the header parameter so that the outputted GBX file is compatible with the game. If the file is already detected decompressed, the input is just copied over to the output and false is returned, otherwise true.
    /// </summary>
    /// <param name="inputFileName">GBX file to decompress.</param>
    /// <param name="output">Output GBX stream in the decompressed form.</param>
    /// <exception cref="ArgumentException"><paramref name="inputFileName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.InvalidPathChars"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="inputFileName"/> is null.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="inputFileName"/> specified a directory. -or- The caller does not have the required permission.</exception>
    /// <exception cref="FileNotFoundException">The file specified in path was not found.</exception>
    /// <exception cref="NotSupportedException"><paramref name="inputFileName"/> is in an invalid format.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">One of the streams is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="VersionNotSupportedException">GBX files below version 3 are not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static bool Decompress(string inputFileName, Stream output)
    {
        using var fs = File.OpenRead(inputFileName);
        return Decompress(fs, output);
    }

    /// <summary>
    /// Decompresses the body part of the GBX file, also setting the header parameter so that the outputted GBX file is compatible with the game. If the file is already detected decompressed, the input is just copied over to the output and false is returned, otherwise true.
    /// </summary>
    /// <param name="input">GBX stream to decompress.</param>
    /// <param name="outputFileName">Output GBX file in the decompressed form.</param>
    /// <exception cref="ArgumentException"><paramref name="outputFileName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.InvalidPathChars"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="outputFileName"/> is null.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission. -or- path specified a file that is read-only.</exception>
    /// <exception cref="NotSupportedException"><paramref name="outputFileName"/> is in an invalid format.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">One of the streams is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="VersionNotSupportedException">GBX files below version 3 are not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static bool Decompress(Stream input, string outputFileName)
    {
        using var fs = File.Create(outputFileName);
        return Decompress(input, fs);
    }

    /// <summary>
    /// Decompresses the body part of the GBX file, also setting the header parameter so that the outputted GBX file is compatible with the game. If the file is already detected decompressed, the input is just copied over to the output and false is returned, otherwise true.
    /// </summary>
    /// <param name="inputFileName">GBX file to decompress.</param>
    /// <param name="outputFileName">Output GBX file in the decompressed form.</param>
    /// <exception cref="ArgumentException"><paramref name="inputFileName"/> or <paramref name="outputFileName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.InvalidPathChars"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="inputFileName"/> or <paramref name="outputFileName"/> is null.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="inputFileName"/> or <paramref name="outputFileName"/> specified a directory. -or- The caller does not have the required permission.  -or- path specified a file that is read-only.</exception>
    /// <exception cref="FileNotFoundException">The file specified in path was not found.</exception>
    /// <exception cref="NotSupportedException"><paramref name="inputFileName"/> is in an invalid format.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">One of the streams is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="VersionNotSupportedException">GBX files below version 3 are not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static bool Decompress(string inputFileName, string outputFileName)
    {
        using var fsInput = File.OpenRead(inputFileName);
        using var fsOutput = File.Create(outputFileName);
        return Decompress(fsInput, fsOutput);
    }
    
    public static bool Compress(Stream input, Stream output)
    {
        return GbxCompressor.Compress(input, output);
    }
    
    public static bool Compress(string inputFileName, Stream output)
    {
        using var fs = File.OpenRead(inputFileName);
        return Compress(fs, output);
    }

    public static bool Compress(Stream input, string outputFileName)
    {
        using var fs = File.Create(outputFileName);
        return Compress(input, fs);
    }

    public static bool Compress(string inputFileName, string outputFileName)
    {
        using var fsInput = File.OpenRead(inputFileName);
        using var fsOutput = File.Create(outputFileName);
        return Compress(fsInput, fsOutput);
    }
}
