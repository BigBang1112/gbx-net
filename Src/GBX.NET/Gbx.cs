using GBX.NET.Components;
using GBX.NET.Extensions;
using GBX.NET.Managers;

namespace GBX.NET;

public interface IGbx : ICloneable
{
    string? FileName { get; }
    GbxHeader Header { get; }
    GbxRefTable? RefTable { get; }
    IClass? Node { get; }

    void Save(string fileName, ClassIdRemapMode remap = default);
    void Save(Stream stream, ClassIdRemapMode remap = default);

#if NET8_0_OR_GREATER
    static abstract Gbx ParseHeader(Stream stream, GbxReadSettings settings = default);
    static abstract Gbx ParseHeader(string fileName, GbxReadSettings settings = default);
    static abstract Gbx<T> ParseHeader<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new();
    static abstract Gbx<T> ParseHeader<T>(string fileName, GbxReadSettings settings = default) where T : IClass, new();
    static abstract Gbx Parse(Stream stream, GbxReadSettings settings = default);
    static abstract Gbx Parse(string fileName, GbxReadSettings settings = default);
    static abstract Gbx<T> Parse<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new();
    static abstract Gbx<T> Parse<T>(string fileName, GbxReadSettings settings = default) where T : IClass, new();
    static abstract IClass? ParseHeaderNode(Stream stream, GbxReadSettings settings = default);
    static abstract IClass? ParseHeaderNode(string fileName, GbxReadSettings settings = default);
    static abstract T ParseHeaderNode<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new();
    static abstract T ParseHeaderNode<T>(string fileName, GbxReadSettings settings = default) where T : IClass, new();
    static abstract T ParseNode<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new();
    static abstract T ParseNode<T>(string fileName, GbxReadSettings settings = default) where T : IClass, new();
    static abstract IClass? ParseNode(string fileName, GbxReadSettings settings = default);
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

    public string? FileName => throw new NotImplementedException();
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

        if (node is null) // aka, header is GbxHeaderUnknown
        {
            return new Gbx(header)
            {
                RefTable = refTable,
                Body = GbxBody.Parse(reader, header.Basic.CompressionOfBody, settings.ReadRawBody),
                ReadSettings = settings
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

        return gbx;
    }

    public static Gbx Parse(string fileName, GbxReadSettings settings = default)
    {
        using var fs = File.OpenRead(fileName);
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
            DeprecVersion = reader.DeprecVersion
        };
    }

    public static Gbx<T> Parse<T>(string fileName, GbxReadSettings settings = default) where T : IClass, new()
    {
        using var fs = File.OpenRead(fileName);
        return Parse<T>(fs, settings);
    }

    public static Gbx ParseHeader(Stream stream, GbxReadSettings settings = default)
    {
        using var reader = new GbxReader(stream, settings.LeaveOpen);

        var header = GbxHeader.Parse(reader, settings, out var node);
        var refTable = GbxRefTable.Parse(reader, header, settings);
        var body = GbxBody.Parse(reader, header.Basic.CompressionOfBody, settings.ReadRawBody);

        if (node is null) // aka, header is GbxHeaderUnknown
        {
            return new Gbx(header)
            {
                RefTable = refTable,
                ReadSettings = settings
            }; 
        }
        
        var gbx = ClassManager.NewGbx(header, node) ?? new Gbx(header);
        gbx.RefTable = refTable;
        gbx.Body = body;
        gbx.ReadSettings = settings;
        gbx.IdVersion = reader.IdVersion;
        gbx.PackDescVersion = reader.PackDescVersion;
        gbx.DeprecVersion = reader.DeprecVersion;

        return gbx;
    }

    public static Gbx ParseHeader(string fileName, GbxReadSettings settings = default)
    {
        using var fs = File.OpenRead(fileName);
        return ParseHeader(fs, settings);
    }

    public static Gbx<T> ParseHeader<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new()
    {
        using var reader = new GbxReader(stream, settings.LeaveOpen);

        var header = GbxHeader.Parse<T>(reader, settings, out var node);
        var refTable = GbxRefTable.Parse(reader, header, settings);
        var body = GbxBody.Parse(reader, header.Basic.CompressionOfBody, settings.ReadRawBody);

        return new Gbx<T>(header, node)
        {
            RefTable = refTable,
            Body = body,
            ReadSettings = settings,
            IdVersion = reader.IdVersion,
            PackDescVersion = reader.PackDescVersion,
            DeprecVersion = reader.DeprecVersion
        };
    }

    public static Gbx<T> ParseHeader<T>(string fileName, GbxReadSettings settings = default) where T : IClass, new()
    {
        using var fs = File.OpenRead(fileName);
        return ParseHeader<T>(fs, settings);
    }

    public static IClass? ParseHeaderNode(Stream stream, GbxReadSettings settings = default)
    {
        return ParseHeader(stream, settings).Node;
    }

    public static IClass? ParseHeaderNode(string fileName, GbxReadSettings settings = default)
    {
        return ParseHeader(fileName, settings).Node;
    }

    public static T ParseNode<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new()
    {
        return Parse<T>(stream, settings).Node;
    }

    public static T ParseNode<T>(string fileName, GbxReadSettings settings = default) where T : IClass, new()
    {
        return Parse<T>(fileName, settings).Node;
    }

    public static IClass? ParseNode(string fileName, GbxReadSettings settings = default)
    {
        return Parse(fileName, settings).Node;
    }

    public static IClass? ParseNode(Stream stream, GbxReadSettings settings = default)
    {
        return Parse(stream, settings).Node;
    }

    public static T ParseHeaderNode<T>(Stream stream, GbxReadSettings settings = default) where T : IClass, new()
    {
        return ParseHeader<T>(stream, settings).Node;
    }

    public static T ParseHeaderNode<T>(string fileName, GbxReadSettings settings = default) where T : IClass, new()
    {
        return ParseHeader<T>(fileName, settings).Node;
    }

    public static TVersion ParseNode<TClass, TVersion>(Stream stream, GbxReadSettings settings = default)
        where TClass : IClass, TVersion, new()
        where TVersion : IClassVersion<TClass>
    {
        return Parse<TClass>(stream, settings).Node;
    }

    public static TVersion ParseNode<TClass, TVersion>(string fileName, GbxReadSettings settings = default)
        where TClass : IClass, TVersion, new()
        where TVersion : IClassVersion<TClass>
    {
        return Parse<TClass>(fileName, settings).Node;
    }

    public static TVersion ParseHeaderNode<TClass, TVersion>(Stream stream, GbxReadSettings settings = default)
        where TClass : IClass, TVersion, new()
        where TVersion : IClassVersion<TClass>
    {
        return ParseHeader<TClass>(stream, settings).Node;
    }

    public static TVersion ParseHeaderNode<TClass, TVersion>(string fileName, GbxReadSettings settings = default)
        where TClass : IClass, TVersion, new()
        where TVersion : IClassVersion<TClass>
    {
        return ParseHeader<TClass>(fileName, settings).Node;
    }

    public object Clone()
    {
        throw new NotImplementedException();
    }

    public virtual void Save(string fileName, ClassIdRemapMode remap = 0)
    {
        throw new NotImplementedException();
    }

    public virtual void Save(Stream stream, ClassIdRemapMode remap = 0)
    {
        throw new NotImplementedException();
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