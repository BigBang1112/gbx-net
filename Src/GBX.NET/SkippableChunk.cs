using System.Diagnostics;
using System.Reflection;
using System.Xml;

namespace GBX.NET;

public class SkippableChunk<T> : Chunk<T>, ISkippableChunk where T : Node
{
    private readonly uint? id;

    public bool Discovered { get; set; }
    public byte[] Data { get; set; }
    public GameBox? Gbx { get; set; }
    
    Node? ISkippableChunk.Node { get; set; }

    public T? Node
    {
        get => (this as ISkippableChunk).Node as T;
        set => (this as ISkippableChunk).Node = value;
    }

    protected SkippableChunk()
    {
        Data = null!;
    }

    public SkippableChunk(T node, byte[] data, uint? id = null)
    {
        Node = node;
        Data = data;

        if (data == null || data.Length == 0)
        {
            Discovered = true;
        }

        this.id = id;
    }

    protected override uint GetId()
    {
        return id ?? base.GetId();
    }

    public void Discover()
    {
        if (Discovered || Node is null)
        {
            return;
        }

        Discovered = true;

        var hasOwnIdState = false;

        if (NodeCacheManager.ChunkAttributesByType.TryGetValue(GetType(), out IEnumerable<Attribute>? chunkAttributes))
        {
            foreach (var attribute in chunkAttributes)
            {
                switch (attribute)
                {
                    case IgnoreChunkAttribute:
                        return;
                }
            }
        }

        using var ms = new MemoryStream(Data);
        using var r = new GameBoxReader(ms, Gbx, asyncAction: null, logger: null, Gbx?.State ?? new());
        var rw = new GameBoxReaderWriter(r);

        if (hasOwnIdState)
        {
            //Gbx?.ResetIdState();
        }

        try
        {
            ReadWrite(Node, rw);
        }
        catch (ChunkReadNotImplementedException)
        {
            try
            {
                Read(Node, r);
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
    }

    public override void Write(T n, GameBoxWriter w)
    {
        w.Write(Data);
    }

    public void Write(GameBoxWriter w)
    {
        w.Write(Data);
    }

    public async Task WriteAsync(GameBoxWriter w, CancellationToken cancellationToken)
    {
        await w.WriteBytesAsync(Data, cancellationToken);
    }

    public override string ToString()
    {
        var chunkAttribute = default(ChunkAttribute);
        var ignoreChunkAttribute = default(IgnoreChunkAttribute);

        if (NodeCacheManager.ChunkAttributesByType.TryGetValue(GetType(), out IEnumerable<Attribute>? chunkAttributes))
        {
            foreach (var attribute in chunkAttributes)
            {
                switch (attribute)
                {
                    case ChunkAttribute chunkAtt:
                        chunkAttribute = chunkAtt;
                        break;
                    case IgnoreChunkAttribute ignoreChunkAtt:
                        ignoreChunkAttribute = ignoreChunkAtt;
                        break;
                }
            }
        }

        var nodeName = typeof(T).Name;

        if (chunkAttribute is null)
        {
            return $"{nodeName} unknown skippable chunk 0x{Id:X8}";
        }
        
        var desc = chunkAttribute.Description;
        var version = (this as IVersionable)?.Version;

        return $"{nodeName} skippable chunk 0x{Id:X8}{(string.IsNullOrEmpty(desc) ? "" : $" ({desc})")}{(ignoreChunkAttribute is null ? "" : " [ignored]")}{(version is null ? "" : $" [v{version}]")}";
    }
}
