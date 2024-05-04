using GBX.NET.Components;
using GBX.NET.Extensions;
using GBX.NET.Managers;
using Microsoft.Extensions.Logging;

namespace GBX.NET;

public interface IGbx
{
    string? FilePath { get; set; }
    GbxHeader Header { get; }
    GbxRefTable? RefTable { get; }
    CMwNod? Node { get; }

    void Save(Stream stream, GbxWriteSettings settings = default);
    void Save(string filePath, GbxWriteSettings settings = default);

#if NET8_0_OR_GREATER
    static abstract Gbx ParseHeader(Stream stream, GbxReadSettings settings = default);
    static abstract Gbx ParseHeader(string filePath, GbxReadSettings settings = default);
    static abstract Gbx<T> ParseHeader<T>(Stream stream, GbxReadSettings settings = default) where T : CMwNod, new();
    static abstract Gbx<T> ParseHeader<T>(string filePath, GbxReadSettings settings = default) where T : CMwNod, new();
    static abstract Task<Gbx> ParseAsync(Stream stream, GbxReadSettings settings = default, CancellationToken cancellationToken = default);
    static abstract Gbx Parse(Stream stream, GbxReadSettings settings = default);
    static abstract Task<Gbx> ParseAsync(string filePath, GbxReadSettings settings = default, CancellationToken cancellationToken = default);
    static abstract Gbx Parse(string filePath, GbxReadSettings settings = default);
    static abstract Task<Gbx<T>> ParseAsync<T>(Stream stream, GbxReadSettings settings = default, CancellationToken cancellationToken = default) where T : CMwNod, new();
    static abstract Gbx<T> Parse<T>(Stream stream, GbxReadSettings settings = default) where T : CMwNod, new();
    static abstract Task<Gbx<T>> ParseAsync<T>(string filePath, GbxReadSettings settings = default, CancellationToken cancellationToken = default) where T : CMwNod, new();
    static abstract Gbx<T> Parse<T>(string filePath, GbxReadSettings settings = default) where T : CMwNod, new();
    static abstract CMwNod? ParseHeaderNode(Stream stream, GbxReadSettings settings = default);
    static abstract CMwNod? ParseHeaderNode(string filePath, GbxReadSettings settings = default);
    static abstract T ParseHeaderNode<T>(Stream stream, GbxReadSettings settings = default) where T : CMwNod, new();
    static abstract T ParseHeaderNode<T>(string filePath, GbxReadSettings settings = default) where T : CMwNod, new();
    static abstract Task<T> ParseNodeAsync<T>(Stream stream, GbxReadSettings settings = default, CancellationToken cancellationToken = default) where T : CMwNod, new();
    static abstract T ParseNode<T>(Stream stream, GbxReadSettings settings = default) where T : CMwNod, new();
    static abstract Task<T> ParseNodeAsync<T>(string filePath, GbxReadSettings settings = default, CancellationToken cancellationToken = default) where T : CMwNod, new();
    static abstract T ParseNode<T>(string filePath, GbxReadSettings settings = default) where T : CMwNod, new();
    static abstract Task<CMwNod?> ParseNodeAsync(Stream stream, GbxReadSettings settings = default, CancellationToken cancellationToken = default);
    static abstract CMwNod? ParseNode(Stream stream, GbxReadSettings settings = default);
    static abstract Task<CMwNod?> ParseNodeAsync(string filePath, GbxReadSettings settings = default, CancellationToken cancellationToken = default);
    static abstract CMwNod? ParseNode(string filePath, GbxReadSettings settings = default);

    static abstract Task<TVersion> ParseNodeAsync<TClass, TVersion>(Stream stream, GbxReadSettings settings = default, CancellationToken cancellationToken = default)
        where TClass : CMwNod, TVersion, new()
        where TVersion : IClassVersion<TClass>;
    static abstract TVersion ParseNode<TClass, TVersion>(Stream stream, GbxReadSettings settings = default)
        where TClass : CMwNod, TVersion, new()
        where TVersion : IClassVersion<TClass>;
    static abstract Task<TVersion> ParseNodeAsync<TClass, TVersion>(string filePath, GbxReadSettings settings = default, CancellationToken cancellationToken = default)
        where TClass : CMwNod, TVersion, new()
        where TVersion : IClassVersion<TClass>;
    static abstract TVersion ParseNode<TClass, TVersion>(string filePath, GbxReadSettings settings = default)
        where TClass : CMwNod, TVersion, new()
        where TVersion : IClassVersion<TClass>;
    static abstract TVersion ParseHeaderNode<TClass, TVersion>(Stream stream, GbxReadSettings settings = default)
        where TClass : CMwNod, TVersion, new()
        where TVersion : IClassVersion<TClass>;
    static abstract TVersion ParseHeaderNode<TClass, TVersion>(string filePath, GbxReadSettings settings = default)
        where TClass : CMwNod, TVersion, new()
        where TVersion : IClassVersion<TClass>;
#endif

    IGbx DeepClone();
}

public interface IGbx<out T> : IGbx where T : CMwNod
{
    new T Node { get; }
}

public partial class Gbx : IGbx
{
    public const string Magic = "GBX";

    public string? FilePath { get; set; }
    public GbxHeader Header { get; }
    public GbxRefTable? RefTable { get; protected set; }
    public GbxBody Body { get; }
    public GbxReadSettings ReadSettings { get; protected set; }
    public CMwNod? Node { get; protected set; }

    public int? IdVersion { get; set; }
    public byte? PackDescVersion { get; set; }
    public int? DeprecVersion { get; set; }
    public ClassIdRemapMode ClassIdRemapMode { get; set; }

    public GbxCompression BodyCompression
    {
        get => Header.Basic.CompressionOfBody;
        set => Header.Basic = Header.Basic with { CompressionOfBody = value };
    }

    internal static bool StrictBooleans { get; set; }

    public static ILzo? LZO { get; set; }
    public static ICrc32? CRC32 { get; set; }
    public static IZLib? ZLib { get; set; }

    internal Gbx(GbxHeader header, GbxBody body)
    {
        Header = header;
        Body = body;
    }

    public override string ToString()
    {
        return $"Gbx ({ClassManager.GetName(Header.ClassId)}, 0x{Header.ClassId:X8})";
    }

    public string? GetFileNameWithoutExtension()
    {
        if (FilePath is null) return null;
        return GbxPath.GetFileNameWithoutExtension(FilePath);
    }

    public virtual Gbx DeepClone() => new(Header.DeepClone(), Body.DeepClone())
    {
        FilePath = FilePath,
        RefTable = RefTable?.DeepClone(),
        ReadSettings = ReadSettings,
        Node = Node?.DeepClone(),
        IdVersion = IdVersion,
        PackDescVersion = PackDescVersion,
        DeprecVersion = DeprecVersion,
        ClassIdRemapMode = ClassIdRemapMode
    };

    IGbx IGbx.DeepClone() => DeepClone();

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<Gbx> ParseAsync(Stream stream, GbxReadSettings settings = default, CancellationToken cancellationToken = default)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        var logger = settings.Logger;
        if (logger is not null) LoggerExtensions.LogInformation(logger, "Gbx Parse (IMPLICIT)");

        var filePath = stream is FileStream fs ? fs.Name : null;
        if (logger is not null) LoggerExtensions.LogDebug(logger, "File path: {FilePath}", filePath);

        using var reader = new GbxReader(stream, settings);

        var header = GbxHeader.Parse(reader, out var node);
        var refTable = GbxRefTable.Parse(reader, header, Path.GetDirectoryName(filePath));

        if (node is null) // aka, header is GbxHeaderUnknown
        {
            var unknownBody = await GbxBody.ParseAsync(reader, header.Basic.CompressionOfBody, cancellationToken);

            if (logger is not null)
            {
                LoggerExtensions.LogDebug(logger, "Id version: {IdVersion}", reader.IdVersion);
                LoggerExtensions.LogDebug(logger, "Class ID remap mode: {ClassIdRemapMode}", reader.ClassIdRemapMode);
                LoggerExtensions.LogInformation(logger, "Unknown Gbx completed.");
            }

            return new Gbx(header, unknownBody)
            {
                RefTable = refTable,
                ReadSettings = settings,
                IdVersion = reader.IdVersion,
                ClassIdRemapMode = reader.ClassIdRemapMode,
                FilePath = filePath
            };
        }

        GbxBody body;

        if (settings.ReadRawBody)
        {
            body = await GbxBody.ParseAsync(reader, header.Basic.CompressionOfBody, cancellationToken);
        }
        else
        {
            reader.ResetIdState();
            reader.ExpectedNodeCount = header.NumNodes;

            body = await GbxBody.ParseAsync(node, reader, header.Basic.CompressionOfBody, cancellationToken);
            // reader is disposed here by the GbxReaderWriter in GbxBody.ParseAsync
        }

        var gbx = ClassManager.NewGbx(header, body, node) ?? new Gbx(header, body);
        gbx.RefTable = refTable;
        gbx.ReadSettings = settings;
        gbx.IdVersion = reader.IdVersion;
        gbx.PackDescVersion = reader.PackDescVersion;
        gbx.DeprecVersion = reader.DeprecVersion;
        gbx.ClassIdRemapMode = reader.ClassIdRemapMode;
        gbx.FilePath = filePath;

        if (logger is not null)
        {
            LoggerExtensions.LogDebug(logger, "Id version: {IdVersion}", reader.IdVersion);
            LoggerExtensions.LogDebug(logger, "PackDesc version: {PackDescVersion}", reader.PackDescVersion);
            LoggerExtensions.LogDebug(logger, "Deprec version: {DeprecVersion}", reader.DeprecVersion);
            LoggerExtensions.LogDebug(logger, "Class ID remap mode: {ClassIdRemapMode}", reader.ClassIdRemapMode);
            LoggerExtensions.LogInformation(logger, "Known Gbx completed.");
        }

        return gbx;
    }

    public static async Task<Gbx> ParseAsync(string filePath, GbxReadSettings settings = default, CancellationToken cancellationToken = default)
    {
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await ParseAsync(fs, settings, cancellationToken);
    }

    public static Gbx Parse(string filePath, GbxReadSettings settings = default)
    {
        using var fs = File.OpenRead(filePath);
        return Parse(fs, settings);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<Gbx<T>> ParseAsync<T>(Stream stream, GbxReadSettings settings = default, CancellationToken cancellationToken = default) where T : CMwNod, new()
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        var logger = settings.Logger;
        if (logger is not null) LoggerExtensions.LogInformation(logger, "Gbx Parse (EXPLICIT)");

        var filePath = stream is FileStream fs ? fs.Name : null;
        if (logger is not null) LoggerExtensions.LogDebug(logger, "File path: {FilePath}", filePath);

        using var reader = new GbxReader(stream, settings);

        var header = GbxHeader.Parse<T>(reader, out var node);
        var refTable = GbxRefTable.Parse(reader, header, Path.GetDirectoryName(filePath));

        reader.ResetIdState();
        reader.ExpectedNodeCount = header.NumNodes;

        var body = await GbxBody.ParseAsync(node, reader, header.Basic.CompressionOfBody, cancellationToken);
        // reader is disposed here by the GbxReaderWriter in GbxBody.ParseAsync

        if (logger is not null)
        {
            LoggerExtensions.LogDebug(logger, "Id version: {IdVersion}", reader.IdVersion);
            LoggerExtensions.LogDebug(logger, "PackDesc version: {PackDescVersion}", reader.PackDescVersion);
            LoggerExtensions.LogDebug(logger, "Deprec version: {DeprecVersion}", reader.DeprecVersion);
            LoggerExtensions.LogDebug(logger, "Class ID remap mode: {ClassIdRemapMode}", reader.ClassIdRemapMode);
            LoggerExtensions.LogInformation(logger, "Gbx completed.");
        }

        return new Gbx<T>(header, body, node)
        {
            RefTable = refTable,
            ReadSettings = settings,
            IdVersion = reader.IdVersion,
            PackDescVersion = reader.PackDescVersion,
            DeprecVersion = reader.DeprecVersion,
            ClassIdRemapMode = reader.ClassIdRemapMode,
            FilePath = filePath
        };
    }

    public static async Task<Gbx<T>> ParseAsync<T>(string filePath, GbxReadSettings settings = default, CancellationToken cancellationToken = default) where T : CMwNod, new()
    {
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await ParseAsync<T>(fs, settings, cancellationToken);
    }

    public static Gbx<T> Parse<T>(string filePath, GbxReadSettings settings = default) where T : CMwNod, new()
    {
        using var fs = File.OpenRead(filePath);
        return Parse<T>(fs, settings);
    }

    public static Gbx ParseHeader(Stream stream, GbxReadSettings settings = default)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        var logger = settings.Logger;
        logger?.LogInformation("Gbx Header Parse (IMPLICIT)");

        var filePath = stream is FileStream fs ? fs.Name : null;
        logger?.LogDebug("File path: {FilePath}", filePath);

        using var reader = new GbxReader(stream, settings);

        var header = GbxHeader.Parse(reader, out var node);
        var refTable = GbxRefTable.Parse(reader, header, Path.GetDirectoryName(filePath));
        var body = GbxBody.Parse(reader, header.Basic.CompressionOfBody);
        
        if (node is null) // aka, header is GbxHeaderUnknown
        {
            if (logger is not null)
            {
                LoggerExtensions.LogDebug(logger, "Id version: {IdVersion}", reader.IdVersion);
                LoggerExtensions.LogDebug(logger, "Class ID remap mode: {ClassIdRemapMode}", reader.ClassIdRemapMode);
                LoggerExtensions.LogInformation(logger, "Unknown Gbx completed.");
            }

            return new Gbx(header, body)
            {
                RefTable = refTable,
                ReadSettings = settings,
                IdVersion = reader.IdVersion,
                ClassIdRemapMode = reader.ClassIdRemapMode,
                FilePath = filePath,
            };
        }

        if (logger is not null)
        {
            LoggerExtensions.LogDebug(logger, "Id version: {IdVersion}", reader.IdVersion);
            LoggerExtensions.LogDebug(logger, "PackDesc version: {PackDescVersion}", reader.PackDescVersion);
            LoggerExtensions.LogDebug(logger, "Deprec version: {DeprecVersion}", reader.DeprecVersion);
            LoggerExtensions.LogDebug(logger, "Class ID remap mode: {ClassIdRemapMode}", reader.ClassIdRemapMode);
            LoggerExtensions.LogInformation(logger, "Known Gbx completed.");
        }

        var gbx = ClassManager.NewGbx(header, body, node) ?? new Gbx(header, body);
        gbx.RefTable = refTable;
        gbx.ReadSettings = settings;
        gbx.IdVersion = reader.IdVersion;
        gbx.PackDescVersion = reader.PackDescVersion;
        gbx.DeprecVersion = reader.DeprecVersion;
        gbx.ClassIdRemapMode = reader.ClassIdRemapMode;
        gbx.FilePath = filePath;

        return gbx;
    }

    public static Gbx ParseHeader(string filePath, GbxReadSettings settings = default)
    {
        using var fs = File.OpenRead(filePath);
        return ParseHeader(fs, settings);
    }

    public static Gbx<T> ParseHeader<T>(Stream stream, GbxReadSettings settings = default) where T : CMwNod, new()
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        var logger = settings.Logger;
        logger?.LogInformation("Gbx Header Parse (EXPLICIT)");

        var filePath = stream is FileStream fs ? fs.Name : null;
        logger?.LogDebug("File path: {FilePath}", filePath);

        using var reader = new GbxReader(stream, settings);

        var header = GbxHeader.Parse<T>(reader, out var node);
        var refTable = GbxRefTable.Parse(reader, header, Path.GetDirectoryName(filePath));
        var body = GbxBody.Parse(reader, header.Basic.CompressionOfBody);

        if (logger is not null)
        {
            LoggerExtensions.LogDebug(logger, "Id version: {IdVersion}", reader.IdVersion);
            LoggerExtensions.LogDebug(logger, "Class ID remap mode: {ClassIdRemapMode}", reader.ClassIdRemapMode);
            LoggerExtensions.LogInformation(logger, "Gbx completed.");
        }

        return new Gbx<T>(header, body, node)
        {
            RefTable = refTable,
            ReadSettings = settings,
            IdVersion = reader.IdVersion,
            PackDescVersion = reader.PackDescVersion,
            DeprecVersion = reader.DeprecVersion,
            ClassIdRemapMode = reader.ClassIdRemapMode,
            FilePath = filePath
        };
    }

    public static Gbx<T> ParseHeader<T>(string filePath, GbxReadSettings settings = default) where T : CMwNod, new()
    {
        using var fs = File.OpenRead(filePath);
        return ParseHeader<T>(fs, settings);
    }

    public static CMwNod? ParseHeaderNode(Stream stream, GbxReadSettings settings = default)
    {
        return ParseHeader(stream, settings).Node;
    }

    public static CMwNod? ParseHeaderNode(string filePath, GbxReadSettings settings = default)
    {
        return ParseHeader(filePath, settings).Node;
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<T> ParseNodeAsync<T>(Stream stream, GbxReadSettings settings = default, CancellationToken cancellationToken = default) where T : CMwNod, new()
    {
        return (await ParseAsync<T>(stream, settings, cancellationToken)).Node;
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<T> ParseNodeAsync<T>(string filePath, GbxReadSettings settings = default, CancellationToken cancellationToken = default) where T : CMwNod, new()
    {
        return (await ParseAsync<T>(filePath, settings, cancellationToken)).Node;
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<CMwNod?> ParseNodeAsync(Stream stream, GbxReadSettings settings = default, CancellationToken cancellationToken = default)
    {
        return (await ParseAsync(stream, settings, cancellationToken)).Node;
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<CMwNod?> ParseNodeAsync(string filePath, GbxReadSettings settings = default, CancellationToken cancellationToken = default)
    {
        return (await ParseAsync(filePath, settings, cancellationToken)).Node;
    }

    public static T ParseHeaderNode<T>(Stream stream, GbxReadSettings settings = default) where T : CMwNod, new()
    {
        return ParseHeader<T>(stream, settings).Node;
    }

    public static T ParseHeaderNode<T>(string filePath, GbxReadSettings settings = default) where T : CMwNod, new()
    {
        return ParseHeader<T>(filePath, settings).Node;
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<TVersion> ParseNodeAsync<TClass, TVersion>(Stream stream, GbxReadSettings settings = default, CancellationToken cancellationToken = default)
        where TClass : CMwNod, TVersion, new()
        where TVersion : IClassVersion<TClass> => await ParseNodeAsync<TClass>(stream, settings, cancellationToken);

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<TVersion> ParseNodeAsync<TClass, TVersion>(string filePath, GbxReadSettings settings = default, CancellationToken cancellationToken = default)
        where TClass : CMwNod, TVersion, new()
        where TVersion : IClassVersion<TClass> => await ParseNodeAsync<TClass>(filePath, settings, cancellationToken);

    public static TVersion ParseHeaderNode<TClass, TVersion>(Stream stream, GbxReadSettings settings = default)
        where TClass : CMwNod, TVersion, new()
        where TVersion : IClassVersion<TClass> => ParseHeaderNode<TClass>(stream, settings);

    public static TVersion ParseHeaderNode<TClass, TVersion>(string filePath, GbxReadSettings settings = default)
        where TClass : CMwNod, TVersion, new()
        where TVersion : IClassVersion<TClass> => ParseHeaderNode<TClass>(filePath, settings);

    public virtual void Save(Stream stream, GbxWriteSettings settings = default)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        var packDescVersion = settings.PackDescVersion ?? PackDescVersion.GetValueOrDefault(3);
        var deprecVersion = DeprecVersion.GetValueOrDefault(10);
        var classIdRemapMode = settings.ClassIdRemapMode ?? ClassIdRemapMode;

        using var writer = new GbxWriter(stream, settings)
        {
            PackDescVersion = packDescVersion,
            DeprecVersion = deprecVersion,
            ClassIdRemapMode = classIdRemapMode
        };
        
        Header.Write(writer, Node, settings);

        var hasRawBodyData = !Body.RawData.IsDefaultOrEmpty;

        var bodyUncompressedMs = default(MemoryStream);

        if (hasRawBodyData)
        {
            writer.Write(Header.NumNodes);
        }
        else if (Node is not null)
        {
            bodyUncompressedMs = new MemoryStream();

            using var bodyWriter = new GbxWriter(bodyUncompressedMs, settings with { CloseStream = false })
            {
                PackDescVersion = packDescVersion,
                DeprecVersion = deprecVersion,
                ClassIdRemapMode = classIdRemapMode
            };

            Body.WriteUncompressed(Node, bodyWriter);

            writer.Write(bodyWriter.NodeDict.Count + 1);
            writer.LoadFrom(bodyWriter);

            bodyUncompressedMs.Position = 0;
        }
        else if (Header is GbxHeaderUnknown)
        {
            throw new Exception("Gbx has an unknown header with a null Node, cannot write body. Set the ReadRawBody (in read setting) to true to fix this problem.");
        }
        else
        {
            throw new Exception("Node cannot be null for a known header, as the node contains the header chunk definitions.");
        }

        if (RefTable is null)
        {
            writer.Write(0); // number of external nodes
        }
        else
        {
            RefTable.Write(writer, Header, hasRawBodyData);
        }

        if (hasRawBodyData)
        {
            Body.WriteRaw(writer);
        }
        else if (bodyUncompressedMs is not null)
        {
            Body.Write(writer, bodyUncompressedMs, Header.Basic.CompressionOfBody);
            bodyUncompressedMs.Dispose();
        }
    }

    public virtual void Save(string filePath, GbxWriteSettings settings = default)
    {
        using var fs = File.Create(filePath);
        Save(fs, settings);
    }

    /// <summary>
    /// Compresses the body part of the Gbx file, also setting the header parameter so that the outputted Gbx file is compatible with the game. If the file is already detected compressed, the input is just copied over to the output.
    /// </summary>
    /// <param name="input">Gbx stream to compress.</param>
    /// <param name="output">Output Gbx stream in the compressed form.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>False if <paramref name="input"/> was already compressed, otherwise true.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<bool> CompressAsync(Stream input, Stream output, CancellationToken cancellationToken = default)
    {
        return await GbxCompressionUtils.CompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task<bool> CompressAsync(string inputFilePath, Stream output, CancellationToken cancellationToken = default)
    {
        using var input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await CompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task<bool> CompressAsync(Stream input, string outputFilePath, CancellationToken cancellationToken = default)
    {
        using var output = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        return await CompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task<bool> CompressAsync(string inputFilePath, string outputFilePath, CancellationToken cancellationToken = default)
    {
        using var input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        using var output = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        return await CompressAsync(input, output, cancellationToken: cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static bool Compress(string inputFilePath, Stream output)
    {
        using var input = File.OpenRead(inputFilePath);
        return Compress(input, output);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static bool Compress(Stream input, string outputFilePath)
    {
        using var output = File.Create(outputFilePath);
        return Compress(input, output);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static bool Compress(string inputFilePath, string outputFilePath)
    {
        using var input = File.OpenRead(inputFilePath);
        using var output = File.Create(outputFilePath);
        return Compress(input, output);
    }


    /// <summary>
    /// Decompresses the body part of the Gbx file, also setting the header parameter so that the outputted Gbx file is compatible with the game. If the file is already detected decompressed, the input is just copied over to the output.
    /// </summary>
    /// <param name="input">Gbx stream to decompress.</param>
    /// <param name="output">Output Gbx stream in the decompressed form.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>False if <paramref name="input"/> was already decompressed, otherwise true.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<bool> DecompressAsync(Stream input, Stream output, CancellationToken cancellationToken = default)
    {
        return await GbxCompressionUtils.DecompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task<bool> DecompressAsync(string inputFilePath, Stream output, CancellationToken cancellationToken = default)
    {
        using var input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await DecompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task<bool> DecompressAsync(Stream input, string outputFilePath, CancellationToken cancellationToken = default)
    {
        using var output = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        return await DecompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task<bool> DecompressAsync(string inputFilePath, string outputFilePath, CancellationToken cancellationToken = default)
    {
        using var input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        using var output = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        return await DecompressAsync(input, output, cancellationToken: cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static bool Decompress(string inputFilePath, Stream output)
    {
        using var input = File.OpenRead(inputFilePath);
        return Decompress(input, output);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static bool Decompress(Stream input, string outputFilePath)
    {
        using var output = File.Create(outputFilePath);
        return Decompress(input, output);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static bool Decompress(string inputFilePath, string outputFilePath)
    {
        using var input = File.OpenRead(inputFilePath);
        using var output = File.Create(outputFilePath);
        return Decompress(input, output);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<GbxCompression> RecompressAsync(Stream input, Stream output, CancellationToken cancellationToken = default)
    {
        return await GbxCompressionUtils.RecompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task<GbxCompression> RecompressAsync(string inputFilePath, Stream output, CancellationToken cancellationToken = default)
    {
        using var input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await RecompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task<GbxCompression> RecompressAsync(Stream input, string outputFilePath, CancellationToken cancellationToken = default)
    {
        using var output = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        return await RecompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task<GbxCompression> RecompressAsync(string inputFilePath, string outputFilePath, CancellationToken cancellationToken = default)
    {
        using var input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        using var output = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        return await RecompressAsync(input, output, cancellationToken: cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static GbxCompression Recompress(string inputFilePath, Stream output)
    {
        using var input = File.OpenRead(inputFilePath);
        return Recompress(input, output);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static GbxCompression Recompress(Stream input, string outputFilePath)
    {
        using var output = File.Create(outputFilePath);
        return Recompress(input, output);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static GbxCompression Recompress(string inputFilePath, string outputFilePath)
    {
        using var input = File.OpenRead(inputFilePath);
        using var output = File.Create(outputFilePath);
        return Recompress(input, output);
    }

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    public static async Task<uint> ParseClassIdAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        var minimalData = new byte[13];
        var count = await stream.ReadAsync(minimalData, 0, minimalData.Length, cancellationToken);

        if (count != minimalData.Length)
        {
            throw new NotAGbxException("Not enough data to parse the class ID.");
        }

        if (minimalData[0] != 'G' || minimalData[1] != 'B' || minimalData[2] != 'X')
        {
            throw new NotAGbxException();
        }

        return BitConverter.ToUInt32(minimalData, 9);
    }

    /// <exception cref="NotAGbxException"></exception>
    public static async Task<uint> ParseClassIdAsync(string filePath, CancellationToken cancellationToken = default)
    {
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 128, useAsync: true);
        return await ParseClassIdAsync(fs, cancellationToken);
    }

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    public static uint ParseClassId(Stream stream)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

#if NETSTANDARD2_0
        var minimalData = new byte[13];
        var count = stream.Read(minimalData, 0, minimalData.Length);
#else
        Span<byte> minimalData = stackalloc byte[13];
        var count = stream.Read(minimalData, 0, minimalData.Length);
#endif

        if (count != minimalData.Length)
        {
            throw new NotAGbxException("Not enough data to parse the class ID.");
        }

        if (minimalData[0] != 'G' || minimalData[1] != 'B' || minimalData[2] != 'X')
        {
            throw new NotAGbxException();
        }

#if NETSTANDARD2_0
        return BitConverter.ToUInt32(minimalData, 9);
#else
        return BitConverter.ToUInt32(minimalData.Slice(9));
#endif
    }

    /// <exception cref="NotAGbxException"></exception>
    public static uint ParseClassId(string filePath)
    {
        using var fs = File.OpenRead(filePath);
        return ParseClassId(fs);
    }

    /// <summary>
    /// Implicitly casts <see cref="Gbx"/> to its <see cref="Node"/>.
    /// </summary>
    /// <param name="gbx">Gbx.</param>
    public static implicit operator CMwNod?(Gbx gbx) => gbx.Node;
}

public class Gbx<T> : Gbx, IGbx<T> where T : CMwNod
{
    public new T Node => (T)(base.Node ?? throw new Exception("Null node is not expected here."));
    public new GbxHeader<T> Header => (GbxHeader<T>)base.Header;

    internal Gbx(GbxHeader<T> header, GbxBody body, T node) : base(header, body)
    {
        base.Node = node;
    }

    public override string ToString()
    {
        return $"Gbx<{typeof(T).Name}> ({ClassManager.GetName(Header.ClassId)}, 0x{Header.ClassId:X8})";
    }

#if NETSTANDARD2_0
    public override Gbx DeepClone() => new Gbx<T>((GbxHeader<T>)Header.DeepClone(), Body.DeepClone(), (T)Node.DeepClone())
#else
    public override Gbx<T> DeepClone() => new(Header.DeepClone(), Body.DeepClone(), (T)Node.DeepClone())
#endif
    {
        FilePath = FilePath,
        RefTable = RefTable?.DeepClone(),
        ReadSettings = ReadSettings,
        IdVersion = IdVersion,
        PackDescVersion = PackDescVersion,
        DeprecVersion = DeprecVersion,
        ClassIdRemapMode = ClassIdRemapMode
    };

    /// <summary>
    /// Implicitly casts <see cref="Gbx{T}"/> to its <see cref="Gbx{T}.Node"/>.
    /// </summary>
    /// <param name="gbx">Gbx.</param>
    public static implicit operator T(Gbx<T> gbx) => gbx.Node;
}