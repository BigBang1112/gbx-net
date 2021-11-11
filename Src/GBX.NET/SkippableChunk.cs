using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using GBX.NET.Engines.MwFoundations;

namespace GBX.NET;

public class SkippableChunk<T> : Chunk<T>, ISkippableChunk where T : CMwNod
{
    private readonly uint chunkID;

    public override uint ID => chunkID;

    public bool Discovered { get; set; }
    public byte[] Data { get; set; }

    protected SkippableChunk()
    {
        chunkID = ((ChunkAttribute)NodeCacheManager.AvailableChunkAttributesByType[GetType()]
            .First(x => x is ChunkAttribute)).ID;
        Data = null!;
    }

    public SkippableChunk(T node, uint id, byte[] data) : base(node)
    {
        chunkID = id;

        Data = data;

        if (data == null || data.Length == 0)
            Discovered = true;
    }

    public SkippableChunk(T node, byte[] data) : base(node)
    {
        chunkID = ((ChunkAttribute)NodeCacheManager.AvailableChunkAttributesByType[GetType()]
            .First(x => x is ChunkAttribute)).ID;

        Data = data;

        if (data == null || data.Length == 0)
            Discovered = true;
    }

    public void Discover()
    {
        if (Discovered) return;
        Discovered = true;

        using var ms = new MemoryStream(Data);
        using var gbxr = CreateReader(ms);
        var gbxrw = new GameBoxReaderWriter(gbxr);

        try
        {
            ReadWrite(Node, gbxrw);
        }
        catch (NotImplementedException)
        {
            var unknownGbxw = CreateWriter(Unknown);

            try
            {
                Read(Node, gbxr);
            }
            catch (NotImplementedException e)
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
            return $"{typeof(T).Name} unknown skippable chunk 0x{ID:X8}";
        var desc = chunkAttribute.Description;
        var version = (this as IVersionable)?.Version;

        return $"{typeof(T).Name} skippable chunk 0x{ID:X8}{(string.IsNullOrEmpty(desc) ? "" : $" ({desc})")}{(ignoreChunkAttribute == null ? "" : " [ignored]")}{(version is null ? "" : $" [v{version}]")}";
    }
}
