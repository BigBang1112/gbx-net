using GBX.NET.Components;
using GBX.NET.Extensions;
using GBX.NET.Managers;
using Microsoft.Extensions.Logging;

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace GBX.NET;

public interface IGbx
{
    string? FilePath { get; set; }
    GbxHeader Header { get; }
    GbxRefTable? RefTable { get; }
    GbxBody Body { get; }
    GbxReadSettings ReadSettings { get; }
    CMwNod? Node { get; }
    int? IdVersion { get; set; }
    byte? PackDescVersion { get; set; }
    int? DeprecVersion { get; set; }
    ClassIdRemapMode ClassIdRemapMode { get; set; }
    GbxCompression BodyCompression { get; set; }

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

/// <summary>
/// Represents a Gbx, which can be either known (<see cref="Node"/> is not null) or unknown (<see cref="Node"/> is null).
/// </summary>
public partial class Gbx : IGbx
{
    /// <summary>
    /// Magic (intial binary letters) for Gbx files.
    /// </summary>
    public const string Magic = "GBX";

    /// <summary>
    /// File path where the Gbx was read from. This can be set externally. It is not used when saving the Gbx.
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    /// Header of the Gbx.
    /// </summary>
    public GbxHeader Header { get; init; }

    /// <summary>
    /// Reference table of the Gbx. This is <see langword="null"/> the Gbx has no external Gbx references (Gbx files or <see cref="CPlugFile"/>-related files, <see cref="PackDesc"/> does not count here).
    /// </summary>
    public GbxRefTable? RefTable { get; protected set; }

    /// <summary>
    /// Body of the Gbx. This only handles body metadata and raw data (in case this reading method was picked).
    /// </summary>
    public GbxBody Body { get; init; }

    /// <summary>
    /// Settings used to read the Gbx. They are stored just for convenience, they don't affect writing when using <see cref="Save(Stream, GbxWriteSettings)"/> or <see cref="Save(string, GbxWriteSettings)"/>.
    /// </summary>
    public GbxReadSettings ReadSettings { get; protected set; }

	/// <summary>
	/// Main node of the Gbx.
	/// </summary>
	public CMwNod? Node { get; protected set; }

    public int? IdVersion { get; set; }
    public byte? PackDescVersion { get; set; }
    public int? DeprecVersion { get; set; }

    /// <summary>
    /// Class ID remap mode used during serialization.
    /// </summary>
    public ClassIdRemapMode ClassIdRemapMode { get; set; }

    /// <summary>
    /// Compression of the body part of the Gbx. This wraps <see cref="GbxHeaderBasic.CompressionOfBody"/> of <see cref="Header"/>.
    /// </summary>
    public GbxCompression BodyCompression
    {
        get => Header.Basic.CompressionOfBody;
        set => Header.Basic = Header.Basic with { CompressionOfBody = value };
    }

    internal static bool StrictBooleans { get; set; }
    internal static bool StrictIdIndices { get; set; }

    public static ILzo? LZO { get; set; }
    public static ICrc32? CRC32 { get; set; }
    public static IZLib? ZLib { get; set; }

    internal Gbx(GbxHeader header, GbxBody body)
    {
        Header = header ?? throw new ArgumentNullException(nameof(header));
        Body = body ?? throw new ArgumentNullException(nameof(body));
    }

    public override string ToString()
    {
        return $"Gbx ({ClassManager.GetName(Header.ClassId)}, 0x{Header.ClassId:X8})";
    }

    public string? GetFileNameWithoutExtension()
    {
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
#if !NETSTANDARD2_0
        await
#endif
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
#if !NETSTANDARD2_0
        await
#endif
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
    /// Compresses the body part of the Gbx file, also setting the header parameter so that the outputted Gbx file is compatible with the game. Compression algorithm is determined from <see cref="LZO"/>. If the file is already detected compressed, it is recompressed.
    /// </summary>
    /// <param name="input">Gbx stream to compress.</param>
    /// <param name="output">Output Gbx stream in the compressed form.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task CompressAsync(Stream input, Stream output, CancellationToken cancellationToken = default)
    {
        await GbxCompressionUtils.CompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task CompressAsync(string inputFilePath, Stream output, CancellationToken cancellationToken = default)
    {
        using var input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        await CompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task CompressAsync(Stream input, string outputFilePath, CancellationToken cancellationToken = default)
    {
        using var output = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        await CompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task CompressAsync(string inputFilePath, string outputFilePath, CancellationToken cancellationToken = default)
    {
        using var input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        using var output = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        await CompressAsync(input, output, cancellationToken: cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static void Compress(string inputFilePath, Stream output)
    {
        using var input = File.OpenRead(inputFilePath);
        Compress(input, output);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static void Compress(Stream input, string outputFilePath)
    {
        using var output = File.Create(outputFilePath);
        Compress(input, output);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static void Compress(string inputFilePath, string outputFilePath)
    {
        using var input = File.OpenRead(inputFilePath);
        using var output = File.Create(outputFilePath);
        Compress(input, output);
    }


    /// <summary>
    /// Decompresses the body part of the Gbx file, also setting the header parameter so that the outputted Gbx file is compatible with the game. Decompression algorithm is determined from <see cref="LZO"/>. If the file is already detected decompressed, the input is just copied over to the output.
    /// </summary>
    /// <param name="input">Gbx stream to decompress.</param>
    /// <param name="output">Output Gbx stream in the decompressed form.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task DecompressAsync(Stream input, Stream output, CancellationToken cancellationToken = default)
    {
        await GbxCompressionUtils.DecompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task DecompressAsync(string inputFilePath, Stream output, CancellationToken cancellationToken = default)
    {
        using var input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        await DecompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task DecompressAsync(Stream input, string outputFilePath, CancellationToken cancellationToken = default)
    {
        using var output = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        await DecompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static async Task DecompressAsync(string inputFilePath, string outputFilePath, CancellationToken cancellationToken = default)
    {
        using var input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        using var output = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        await DecompressAsync(input, output, cancellationToken);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static void Decompress(string inputFilePath, Stream output)
    {
        using var input = File.OpenRead(inputFilePath);
        Decompress(input, output);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static void Decompress(Stream input, string outputFilePath)
    {
        using var output = File.Create(outputFilePath);
        Decompress(input, output);
    }

    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    public static void Decompress(string inputFilePath, string outputFilePath)
    {
        using var input = File.OpenRead(inputFilePath);
        using var output = File.Create(outputFilePath);
        Decompress(input, output);
    }

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    public static async Task<uint> ParseClassIdAsync(Stream stream, bool remap = true, CancellationToken cancellationToken = default)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        var minimalData = new byte[13];
#if NETSTANDARD2_0
        var count = await stream.ReadAsync(minimalData, 0, minimalData.Length, cancellationToken);
#else
        var count = await stream.ReadAsync(minimalData, cancellationToken);
#endif
        if (count != minimalData.Length)
        {
            throw new NotAGbxException("Not enough data to parse the class ID.");
        }

        if (minimalData[0] != 'G' || minimalData[1] != 'B' || minimalData[2] != 'X')
        {
            throw new NotAGbxException();
        }

        var classId = BitConverter.ToUInt32(minimalData, 9);

        return remap ? ClassManager.Wrap(classId) : classId;
    }

    /// <exception cref="NotAGbxException"></exception>
    public static async Task<uint> ParseClassIdAsync(string filePath, bool remap = true, CancellationToken cancellationToken = default)
    {
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 128, useAsync: true);
        return await ParseClassIdAsync(fs, remap, cancellationToken);
    }

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    public static uint ParseClassId(Stream stream, bool remap = true)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

#if NETSTANDARD2_0
        var minimalData = new byte[13];
        var count = stream.Read(minimalData, 0, minimalData.Length);
#else
        Span<byte> minimalData = stackalloc byte[13];
        var count = stream.Read(minimalData);
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
        var classId = BitConverter.ToUInt32(minimalData, 9);
#else
        var classId = BitConverter.ToUInt32(minimalData.Slice(9));
#endif

        return remap ? ClassManager.Wrap(classId) : classId;
    }

    /// <exception cref="NotAGbxException"></exception>
    public static uint ParseClassId(string filePath, bool remap = true)
    {
        using var fs = File.OpenRead(filePath);
        return ParseClassId(fs, remap);
    }

    public static bool IsUncompressed(Stream stream)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

#if NETSTANDARD2_0
        var minimalData = new byte[8];
        var count = stream.Read(minimalData, 0, minimalData.Length);
#else
        Span<byte> minimalData = stackalloc byte[8];
        var count = stream.Read(minimalData);
#endif

        if (count != minimalData.Length)
        {
            throw new NotAGbxException("Not enough data to check if the Gbx is uncompressed.");
        }

        if (minimalData[0] != 'G' || minimalData[1] != 'B' || minimalData[2] != 'X')
        {
            throw new NotAGbxException();
        }

        return minimalData[7] == (byte)GbxCompression.Uncompressed;
    }

    public static bool IsUncompressed(string filePath)
    {
        using var fs = File.OpenRead(filePath);
        return IsUncompressed(fs);
    }

    public static async Task<bool> IsUncompressed(Stream stream, CancellationToken cancellationToken = default)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        var minimalData = new byte[8];
#if NETSTANDARD2_0
        var count = await stream.ReadAsync(minimalData, 0, minimalData.Length, cancellationToken);
#else
        var count = await stream.ReadAsync(minimalData, cancellationToken);
#endif
        if (count != minimalData.Length)
        {
            throw new NotAGbxException("Not enough data to check if the Gbx is uncompressed.");
        }

        if (minimalData[0] != 'G' || minimalData[1] != 'B' || minimalData[2] != 'X')
        {
            throw new NotAGbxException();
        }

        return minimalData[7] == (byte)GbxCompression.Uncompressed;
    }

    public static async Task<bool> IsUncompressed(string filePath, CancellationToken cancellationToken = default)
    {
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await IsUncompressed(fs, cancellationToken);
    }

    public static bool IsCompressed(Stream stream)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

#if NETSTANDARD2_0
        var minimalData = new byte[8];
        var count = stream.Read(minimalData, 0, minimalData.Length);
#else
        Span<byte> minimalData = stackalloc byte[8];
        var count = stream.Read(minimalData);
#endif

        if (count != minimalData.Length)
        {
            throw new NotAGbxException("Not enough data to check if the Gbx is compressed.");
        }

        if (minimalData[0] != 'G' || minimalData[1] != 'B' || minimalData[2] != 'X')
        {
            throw new NotAGbxException();
        }

        return minimalData[7] == (byte)GbxCompression.Compressed;
    }

    public static bool IsCompressed(string filePath)
    {
        using var fs = File.OpenRead(filePath);
        return IsCompressed(fs);
    }

    public static async Task<bool> IsCompressed(Stream stream, CancellationToken cancellationToken = default)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        var minimalData = new byte[8];
#if NETSTANDARD2_0
        var count = await stream.ReadAsync(minimalData, 0, minimalData.Length, cancellationToken);
#else
        var count = await stream.ReadAsync(minimalData, cancellationToken);
#endif
        if (count != minimalData.Length)
        {
            throw new NotAGbxException("Not enough data to check if the Gbx is compressed.");
        }

        if (minimalData[0] != 'G' || minimalData[1] != 'B' || minimalData[2] != 'X')
        {
            throw new NotAGbxException();
        }

        return minimalData[7] == (byte)GbxCompression.Compressed;
    }

    public static async Task<bool> IsCompressed(string filePath, CancellationToken cancellationToken = default)
    {
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await IsCompressed(fs, cancellationToken);
    }

    /// <summary>
    /// Implicitly casts <see cref="Gbx"/> to its <see cref="Node"/>.
    /// </summary>
    /// <param name="gbx">Gbx.</param>
    public static implicit operator CMwNod?(Gbx gbx) => gbx.Node;
}

/// <summary>
/// Represents a Gbx with a main node of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Type of the main node.</typeparam>
public class Gbx<
#if NET6_0_OR_GREATER
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
    T> : Gbx, IGbx<T> where T : CMwNod
{
    /// <summary>
    /// Main typed node of the Gbx.
    /// </summary>
    public new T Node => (T)(base.Node ?? throw new Exception("Null node is not expected here."));

	/// <summary>
	/// Typed header of the Gbx.
	/// </summary>
	public new GbxHeader<T> Header => (GbxHeader<T>)base.Header;

    internal Gbx(GbxHeader<T> header, GbxBody body, T node) : base(header, body)
    {
        base.Node = node ?? throw new ArgumentNullException(nameof(node));
    }

	/// <summary>
	/// Creates a new Gbx wrap of <typeparamref name="T"/> with <paramref name="node"/> and basic header parameters.
	/// </summary>
	/// <param name="node">Node.</param>
	/// <param name="headerBasic">Basic header parameters.</param>
	public Gbx(T node, GbxHeaderBasic headerBasic) : this(new GbxHeader<T>(headerBasic), new GbxBody(), node)
    {

    }

    /// <summary>
    /// Creates a new Gbx wrap of <typeparamref name="T"/> with <paramref name="node"/>.
    /// </summary>
    /// <param name="node">Node.</param>
    public Gbx(T node) : this(node, GbxHeaderBasic.Default)
    {
        
    }

    /// <summary>
    /// Creates a new Gbx with all defaults. This should be ONLY used for deserialization purposes.
    /// </summary>
    internal Gbx() : this(new GbxHeader<T>(GbxHeaderBasic.Default), new(), Activator.CreateInstance<T>())
    {

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