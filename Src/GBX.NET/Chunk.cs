using GBX.NET.Debugging;
using System.Reflection;
using System.Runtime.Serialization;

namespace GBX.NET;

public abstract class Chunk : IChunk, IComparable<Chunk>
{
    public virtual uint ID => ((ChunkAttribute)NodeCacheManager.AvailableChunkAttributesByType[GetType()]
        .First(x => x is ChunkAttribute)).ID;

    /// <summary>
    /// Stream of unknown bytes
    /// </summary>
    [IgnoreDataMember]
    public MemoryStream Unknown { get; } = new MemoryStream();

    public CMwNod Node { get; internal set; }

    public int Progress { get; set; }

#if DEBUG
    public ChunkDebugger Debugger { get; } = new();
#endif

    protected Chunk(CMwNod node)
    {
        Node = node;
    }

    public override int GetHashCode() => (int)ID;

    public override bool Equals(object? obj) => Equals(obj as Chunk);

    public override string ToString() => $"Chunk 0x{ID:X8}";

    public bool Equals(Chunk? chunk) => chunk is not null && chunk.ID == ID;

    protected internal GameBoxReader CreateReader(Stream input)
    {
        return new GameBoxReader(input, Node.GBX?.Body, this as ILookbackable);
    }

    protected internal GameBoxWriter CreateWriter(Stream input)
    {
        return new GameBoxWriter(input, Node.GBX?.Body, this as ILookbackable);
    }

    public static uint Remap(uint chunkID, IDRemap remap = IDRemap.Latest)
    {
        var classPart = chunkID & 0xFFFFF000;
        var chunkPart = chunkID & 0xFFF;

        switch (remap)
        {
            case IDRemap.Latest:
                {
                    uint newClassPart = classPart;
                    while (NodeCacheManager.Mappings.TryGetValue(newClassPart, out uint newID))
                        newClassPart = newID;
                    return newClassPart + chunkPart;
                }
            case IDRemap.TrackMania2006:
                {
                    return classPart == 0x03078000 // Not ideal solution
                        ? 0x24061000 + chunkPart
                        : NodeCacheManager.Mappings.Last(x => x.Value == classPart).Key + chunkPart;
                }
            default:
                return chunkID;
        }
    }

    public virtual void OnLoad() { }

    public int CompareTo(Chunk? other)
    {
        return ID.CompareTo(other?.ID);
    }

    void IChunk.Read(CMwNod n, GameBoxReader r)
    {
        throw new NotSupportedException();
    }

    void IChunk.Write(CMwNod n, GameBoxWriter w)
    {
        throw new NotSupportedException();
    }

    void IChunk.ReadWrite(CMwNod n, GameBoxReaderWriter rw)
    {
        throw new NotSupportedException();
    }
}