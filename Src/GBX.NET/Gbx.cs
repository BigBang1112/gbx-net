using GBX.NET.Components;
using GBX.NET.Extensions;
using GBX.NET.Managers;

namespace GBX.NET;

public interface IGbx
{
    string? FilePath { get; set; }
    GbxHeader Header { get; }
    GbxRefTable? RefTable { get; }
    IClass? Node { get; }

    void Save(string filePath, ClassIdRemapMode remap = default);
    void Save(Stream stream, ClassIdRemapMode remap = default);

#if NET8_0_OR_GREATER
    static abstract Gbx ParseHeader(Stream stream, GbxReadSettings settings = default);
    static abstract Gbx ParseHeader(string filePath, GbxReadSettings settings = default);
    static abstract Gbx<T> ParseHeader<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new();
    static abstract Gbx<T> ParseHeader<T>(string filePath, GbxReadSettings settings = default) where T : IClass, new();
    static abstract Gbx Parse(Stream stream, GbxReadSettings settings = default);
    static abstract Gbx Parse(string filePath, GbxReadSettings settings = default);
    static abstract Gbx<T> Parse<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new();
    static abstract Gbx<T> Parse<T>(string filePath, GbxReadSettings settings = default) where T : IClass, new();
    static abstract IClass? ParseHeaderNode(Stream stream, GbxReadSettings settings = default);
    static abstract IClass? ParseHeaderNode(string filePath, GbxReadSettings settings = default);
    static abstract T ParseHeaderNode<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new();
    static abstract T ParseHeaderNode<T>(string filePath, GbxReadSettings settings = default) where T : IClass, new();
    static abstract T ParseNode<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new();
    static abstract T ParseNode<T>(string filePath, GbxReadSettings settings = default) where T : IClass, new();
    static abstract IClass? ParseNode(string filePath, GbxReadSettings settings = default);
    static abstract IClass? ParseNode(Stream stream, GbxReadSettings settings = default);
#endif
}

public interface IGbx<T> : IGbx where T : notnull, IClass
{
    new T Node { get; }
}

public class Gbx : IGbx
{
    public const string Magic = "GBX";

    public string? FilePath { get; set; }
    public GbxHeader Header { get; }
    public GbxRefTable? RefTable { get; private set; }
    public GbxBody? Body { get; private set; }
    public GbxReadSettings ReadSettings { get; private set; }
    public IClass? Node { get; protected set; }

    public int? IdVersion { get; set; }
    public byte? PackDescVersion { get; set; }
    public int? DeprecVersion { get; set; }

    internal static bool StrictBooleans { get; set; }

    public static ILzo? LZO { get; set; }
    public static ICrc32? CRC32 { get; set; }

    internal Gbx(GbxHeader header)
    {
        Header = header;
    }

    public static Gbx Parse(Stream stream, GbxReadSettings settings = default)
    {
        using var reader = new GbxReader(stream, settings.LeaveOpen);

        var header = GbxHeader.Parse(reader, settings, out var node);
        var refTable = GbxRefTable.Parse(reader, header, settings);

        var filePath = stream is FileStream fs ? fs.Name : null;

        if (node is null) // aka, header is GbxHeaderUnknown
        {
            return new Gbx(header)
            {
                RefTable = refTable,
                Body = GbxBody.Parse(reader, header.Basic.CompressionOfBody, settings),
                ReadSettings = settings,
                FilePath = filePath
            };
        }

        reader.ResetIdState();
        reader.ExpectedNodeCount = header.NumNodes;

        var gbx = ClassManager.NewGbx(header, node) ?? new Gbx(header);
        gbx.RefTable = refTable;
        gbx.Body = GbxBody.Parse(node, reader, settings, header.Basic.CompressionOfBody);
        gbx.ReadSettings = settings;
        gbx.IdVersion = reader.IdVersion;
        gbx.PackDescVersion = reader.PackDescVersion;
        gbx.DeprecVersion = reader.DeprecVersion;
        gbx.FilePath = filePath;

        return gbx;
    }

    public static Gbx Parse(string filePath, GbxReadSettings settings = default)
    {
        using var fs = File.OpenRead(filePath);
        return Parse(fs, settings);
    }

    public static Gbx<T> Parse<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new()
    {
        using var reader = new GbxReader(stream, settings.LeaveOpen);

        var header = GbxHeader.Parse<T>(reader, settings, out var node);
        var refTable = GbxRefTable.Parse(reader, header, settings);

        reader.ResetIdState();
        reader.ExpectedNodeCount = header.NumNodes;

        var body = GbxBody.Parse(node, reader, settings, header.Basic.CompressionOfBody);

        return new Gbx<T>(header, node)
        {
            RefTable = refTable,
            Body = body,
            ReadSettings = settings,
            IdVersion = reader.IdVersion,
            PackDescVersion = reader.PackDescVersion,
            DeprecVersion = reader.DeprecVersion,
            FilePath = stream is FileStream fs ? fs.Name : null
        };
    }

    public static Gbx<T> Parse<T>(string filePath, GbxReadSettings settings = default) where T : IClass, new()
    {
        using var fs = File.OpenRead(filePath);
        return Parse<T>(fs, settings);
    }

    public static Gbx ParseHeader(Stream stream, GbxReadSettings settings = default)
    {
        using var reader = new GbxReader(stream, settings.LeaveOpen);

        var header = GbxHeader.Parse(reader, settings, out var node);
        var refTable = GbxRefTable.Parse(reader, header, settings);
        var body = GbxBody.Parse(reader, header.Basic.CompressionOfBody, settings);

        var filePath = stream is FileStream fs ? fs.Name : null;
        
        if (node is null) // aka, header is GbxHeaderUnknown
        {
            return new Gbx(header)
            {
                RefTable = refTable,
                ReadSettings = settings,
                FilePath = filePath,
            }; 
        }
        
        var gbx = ClassManager.NewGbx(header, node) ?? new Gbx(header);
        gbx.RefTable = refTable;
        gbx.Body = body;
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

    public static Gbx<T> ParseHeader<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new()
    {
        using var reader = new GbxReader(stream, settings.LeaveOpen);

        var header = GbxHeader.Parse<T>(reader, settings, out var node);
        var refTable = GbxRefTable.Parse(reader, header, settings);
        var body = GbxBody.Parse(reader, header.Basic.CompressionOfBody, settings);

        return new Gbx<T>(header, node)
        {
            RefTable = refTable,
            Body = body,
            ReadSettings = settings,
            IdVersion = reader.IdVersion,
            PackDescVersion = reader.PackDescVersion,
            DeprecVersion = reader.DeprecVersion,
            FilePath = stream is FileStream fs ? fs.Name : null
        };
    }

    public static Gbx<T> ParseHeader<T>(string filePath, GbxReadSettings settings = default) where T : IClass, new()
    {
        using var fs = File.OpenRead(filePath);
        return ParseHeader<T>(fs, settings);
    }

    public static IClass? ParseHeaderNode(Stream stream, GbxReadSettings settings = default)
    {
        return ParseHeader(stream, settings).Node;
    }

    public static IClass? ParseHeaderNode(string filePath, GbxReadSettings settings = default)
    {
        return ParseHeader(filePath, settings).Node;
    }

    public static T ParseNode<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new()
    {
        return Parse<T>(stream, settings).Node;
    }

    public static T ParseNode<T>(string filePath, GbxReadSettings settings = default) where T : IClass, new()
    {
        return Parse<T>(filePath, settings).Node;
    }

    public static IClass? ParseNode(string filePath, GbxReadSettings settings = default)
    {
        return Parse(filePath, settings).Node;
    }

    public static IClass? ParseNode(Stream stream, GbxReadSettings settings = default)
    {
        return Parse(stream, settings).Node;
    }

    public static T ParseHeaderNode<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new()
    {
        return ParseHeader<T>(stream, settings).Node;
    }

    public static T ParseHeaderNode<T>(string filePath, GbxReadSettings settings = default) where T : IClass, new()
    {
        return ParseHeader<T>(filePath, settings).Node;
    }

    public static TVersion ParseNode<TClass, TVersion>(Stream stream, GbxReadSettings settings = default)
        where TClass : IClass, TVersion, new()
        where TVersion : IClassVersion<TClass>
    {
        return Parse<TClass>(stream, settings).Node;
    }

    public static TVersion ParseNode<TClass, TVersion>(string filePath, GbxReadSettings settings = default)
        where TClass : IClass, TVersion, new()
        where TVersion : IClassVersion<TClass>
    {
        return Parse<TClass>(filePath, settings).Node;
    }

    public static TVersion ParseHeaderNode<TClass, TVersion>(Stream stream, GbxReadSettings settings = default)
        where TClass : IClass, TVersion, new()
        where TVersion : IClassVersion<TClass>
    {
        return ParseHeader<TClass>(stream, settings).Node;
    }

    public static TVersion ParseHeaderNode<TClass, TVersion>(string filePath, GbxReadSettings settings = default)
        where TClass : IClass, TVersion, new()
        where TVersion : IClassVersion<TClass>
    {
        return ParseHeader<TClass>(filePath, settings).Node;
    }

    public virtual void Save(string filePath, ClassIdRemapMode remap = 0)
    {
        throw new NotImplementedException();
    }

    public virtual void Save(Stream stream, ClassIdRemapMode remap = 0)
    {
        throw new NotImplementedException();
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
    public static bool Compress(Stream input, Stream output) => GbxCompressionUtils.Compress(input, output);

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
    public static bool Decompress(Stream input, Stream output) => GbxCompressionUtils.Decompress(input, output);

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

public class Gbx<T> : Gbx, IGbx<T> where T : notnull, IClass
{
    public new T Node => (T)(base.Node ?? throw new Exception("Null node is not expected here."));

    internal Gbx(GbxHeader<T> header, T node) : base(header)
    {
        base.Node = node;
    }
}