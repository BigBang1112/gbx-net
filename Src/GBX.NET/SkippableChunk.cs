using System.Diagnostics;
using System.Reflection;

namespace GBX.NET;

public class SkippableChunk<T> : Chunk<T>, ISkippableChunk where T : CMwNod
{
    private readonly uint? id;

    public bool Discovered { get; set; }
    public byte[] Data { get; set; }

    protected SkippableChunk()
    {
        Data = null!;
    }

    public SkippableChunk(T node, byte[] data, uint? id = null) : base(node)
    {
        Data = data;

        if (data == null || data.Length == 0)
            Discovered = true;

        this.id = id;
    }

    protected override uint GetId()
    {
        return id ?? base.GetId();
    }

    public void Discover()
    {
        if (Discovered) return;
        Discovered = true;

        if (NodeCacheManager.ChunkAttributesByType.TryGetValue(GetType(), out IEnumerable<Attribute>? chunkAttributes))
        {
            var ignoreChunkAttribute = chunkAttributes.FirstOrDefault(x => x is IgnoreChunkAttribute);
            if (ignoreChunkAttribute is not null)
                return;
        }

        using var ms = new MemoryStream(Data);
        using var gbxr = CreateReader(ms);
        var gbxrw = new GameBoxReaderWriter(gbxr);

        try
        {
            ReadWrite(Node, gbxrw);
        }
        catch (ChunkReadNotImplementedException)
        {
            try
            {
                Read(Node, gbxr);
            }
            catch (ChunkReadNotImplementedException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        if (ms.Position != ms.Length)
        {
            Debug.WriteLine($"Skippable chunk not fully parsed! ({ms.Position}/{ms.Length}) - {ToString()}");
        }

        Progress = (int)ms.Position;
    }

    public override void Write(T n, GameBoxWriter w)
    {
        w.WriteBytes(Data);
    }

    public void Write(GameBoxWriter w)
    {
        w.WriteBytes(Data);
    }

    public override string ToString()
    {
        var chunkType = GetType();
        var chunkAttribute = chunkType.GetCustomAttribute<ChunkAttribute>();
        var ignoreChunkAttribute = chunkType.GetCustomAttribute<IgnoreChunkAttribute>();

        if (chunkAttribute == null)
            return $"{typeof(T).Name} unknown skippable chunk 0x{Id:X8}";
        var desc = chunkAttribute.Description;
        var version = (this as IVersionable)?.Version;

        return $"{typeof(T).Name} skippable chunk 0x{Id:X8}{(string.IsNullOrEmpty(desc) ? "" : $" ({desc})")}{(ignoreChunkAttribute == null ? "" : " [ignored]")}{(version is null ? "" : $" [v{version}]")}";
    }
}
