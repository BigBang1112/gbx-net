using System.Diagnostics;
using System.Reflection;

namespace GBX.NET;

public class SkippableChunk<T> : Chunk<T>, ISkippableChunk, IState where T : Node
{
    private readonly uint? id;

    public bool Discovered { get; set; }
    public byte[] Data { get; set; }

    Guid? IState.StateGuid { get; set; }

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
        if (Discovered)
        {
            return;
        }

        Discovered = true;

        var stateGuid = ((IState)Node).StateGuid.GetValueOrDefault();
        var hasOwnIdState = false;

        if (NodeCacheManager.ChunkAttributesByType.TryGetValue(GetType(), out IEnumerable<Attribute>? chunkAttributes))
        {
            foreach (var attribute in chunkAttributes)
            {
                switch (attribute)
                {
                    case IgnoreChunkAttribute:
                        return;
                    case ChunkWithOwnIdStateAttribute:
                        hasOwnIdState = true;
                        break;
                }
            }
        }

        using var ms = new MemoryStream(Data);
        using var r = new GameBoxReader(ms, stateGuid);
        var rw = new GameBoxReaderWriter(r);

        if (hasOwnIdState)
        {
            r.StartIdSubState();
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
        w.WriteBytes(Data);
    }

    public void Write(GameBoxWriter w)
    {
        w.WriteBytes(Data);
    }

    public async Task WriteAsync(GameBoxWriter w, CancellationToken cancellationToken)
    {
        await w.WriteBytesAsync(Data, cancellationToken);
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
