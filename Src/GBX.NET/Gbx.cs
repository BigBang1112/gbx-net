using GBX.NET.Components;
using GBX.NET.Extensions;
using GBX.NET.Managers;

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
    public GbxRefTable? RefTable { get; private set; }
    public GbxBody Body { get; }
    public GbxReadSettings ReadSettings { get; private set; }
    public CMwNod? Node { get; protected set; }

    public int? IdVersion { get; set; }
    public byte? PackDescVersion { get; set; }
    public int? DeprecVersion { get; set; }

    internal static bool StrictBooleans { get; set; }

    public static ILzo? LZO { get; set; }
    public static ICrc32? CRC32 { get; set; }

    internal Gbx(GbxHeader header, GbxBody body)
    {
        Header = header;
        Body = body;
    }

    public override string ToString()
    {
        return $"Gbx ({ClassManager.GetName(Header.ClassId)}, 0x{Header.ClassId:X8})";
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<Gbx> ParseAsync(Stream stream, GbxReadSettings settings = default, CancellationToken cancellationToken = default)
    {
        using var reader = new GbxReader(stream, settings.LeaveOpen, settings.Logger);

        var header = GbxHeader.Parse(reader, settings, out var node);
        var refTable = GbxRefTable.Parse(reader, header, settings);

        var filePath = stream is FileStream fs ? fs.Name : null;

        if (node is null) // aka, header is GbxHeaderUnknown
        {
            var unknownBody = await GbxBody.ParseAsync(reader, header.Basic.CompressionOfBody, settings, cancellationToken);

            return new Gbx(header, unknownBody)
            {
                RefTable = refTable,
                ReadSettings = settings,
                FilePath = filePath
            };
        }

        reader.ResetIdState();
        reader.ExpectedNodeCount = header.NumNodes;

        var body = await GbxBody.ParseAsync(node, reader, header.Basic.CompressionOfBody, settings, cancellationToken);

        var gbx = ClassManager.NewGbx(header, body, node) ?? new Gbx(header, body);
        gbx.RefTable = refTable;
        gbx.ReadSettings = settings;
        gbx.IdVersion = reader.IdVersion;
        gbx.PackDescVersion = reader.PackDescVersion;
        gbx.DeprecVersion = reader.DeprecVersion;
        gbx.FilePath = filePath;

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
        using var reader = new GbxReader(stream, settings.LeaveOpen, settings.Logger);

        var header = GbxHeader.Parse<T>(reader, settings, out var node);
        var refTable = GbxRefTable.Parse(reader, header, settings);

        reader.ResetIdState();
        reader.ExpectedNodeCount = header.NumNodes;

        var body = await GbxBody.ParseAsync(node, reader, header.Basic.CompressionOfBody, settings, cancellationToken);

        return new Gbx<T>(header, body, node)
        {
            RefTable = refTable,
            ReadSettings = settings,
            IdVersion = reader.IdVersion,
            PackDescVersion = reader.PackDescVersion,
            DeprecVersion = reader.DeprecVersion,
            FilePath = stream is FileStream fs ? fs.Name : null
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
        using var reader = new GbxReader(stream, settings.LeaveOpen, settings.Logger);

        var header = GbxHeader.Parse(reader, settings, out var node);
        var refTable = GbxRefTable.Parse(reader, header, settings);
        var body = GbxBody.Parse(reader, header.Basic.CompressionOfBody, settings);

        var filePath = stream is FileStream fs ? fs.Name : null;
        
        if (node is null) // aka, header is GbxHeaderUnknown
        {
            return new Gbx(header, body)
            {
                RefTable = refTable,
                ReadSettings = settings,
                FilePath = filePath,
            }; 
        }
        
        var gbx = ClassManager.NewGbx(header, body, node) ?? new Gbx(header, body);
        gbx.RefTable = refTable;
        gbx.ReadSettings = settings;
        gbx.IdVersion = reader.IdVersion;
        gbx.PackDescVersion = reader.PackDescVersion;
        gbx.DeprecVersion = reader.DeprecVersion;
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
        using var reader = new GbxReader(stream, settings.LeaveOpen, settings.Logger);

        var header = GbxHeader.Parse<T>(reader, settings, out var node);
        var refTable = GbxRefTable.Parse(reader, header, settings);
        var body = GbxBody.Parse(reader, header.Basic.CompressionOfBody, settings);

        return new Gbx<T>(header, body, node)
        {
            RefTable = refTable,
            ReadSettings = settings,
            IdVersion = reader.IdVersion,
            PackDescVersion = reader.PackDescVersion,
            DeprecVersion = reader.DeprecVersion,
            FilePath = stream is FileStream fs ? fs.Name : null
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
        using var writer = new GbxWriter(stream, settings.LeaveOpen);
        Header.Write(writer, Node, settings);

        if (RefTable is null)
        {
            writer.Write(0);
        }
        else
        {
            RefTable.Write(writer, settings);
        }

        Body.Write(Node, writer, Header.Basic.CompressionOfBody, settings);
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
    /// <returns>False if <paramref name="input"/> was already compressed, otherwise true.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotAGbxException"></exception>
    /// <exception cref="LzoNotDefinedException"></exception>
    /// <exception cref="VersionNotSupportedException"></exception>
    /// <exception cref="TextFormatNotSupportedException"></exception>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static Task<bool> CompressAsync(Stream input, Stream output, CancellationToken cancellationToken = default)
    {
        return GbxCompressionUtils.CompressAsync(input, output, cancellationToken);
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
}

public class Gbx<T> : Gbx, IGbx<T> where T : CMwNod
{
    public new T Node => (T)(base.Node ?? throw new Exception("Null node is not expected here."));

    internal Gbx(GbxHeader<T> header, GbxBody body, T node) : base(header, body)
    {
        base.Node = node;
    }
}