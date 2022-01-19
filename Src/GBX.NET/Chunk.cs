using GBX.NET.Debugging;
using System.Reflection;
using System.Runtime.Serialization;

namespace GBX.NET;

public abstract class Chunk : IChunk, IComparable<Chunk>
{
    private uint? id;

    /// <summary>
    /// Stream of unknown bytes
    /// </summary>
    [IgnoreDataMember]
    public MemoryStream Unknown { get; } = new MemoryStream();

    internal Node Node { get; set; }

    public int Progress { get; set; }

    public ChunkDebugger? Debugger { get; internal set; }

    protected Chunk(CMwNod node)
    {
        Node = node;
    }

    public uint Id => GetStoredChunkId();

    private uint GetStoredChunkId()
    {
        id ??= GetChunkId();
        return id.Value;
    }

    protected abstract uint GetChunkId();

    public override int GetHashCode() => (int)Id;

    public override bool Equals(object? obj) => Equals(obj as Chunk);

    public override string ToString() => $"Chunk 0x{Id:X8}";

    public bool Equals(Chunk? chunk) => chunk is not null && chunk.Id == Id;

    protected internal GameBoxReader CreateReader(Stream input, ILogger? logger = null)
    {
        return new GameBoxReader(input, Node.GBX?.Body, this as ILookbackable, logger);
    }

    protected internal GameBoxWriter CreateWriter(Stream input, ILogger? logger = null)
    {
        return new GameBoxWriter(input, Node.GBX?.Body, this as ILookbackable, logger);
    }

    public static uint Remap(uint chunkID, IDRemap remap = IDRemap.Latest)
    {
        var classPart = chunkID & 0xFFFFF000;
        var chunkPart = chunkID & 0xFFF;

        switch (remap)
        {
            case IDRemap.Latest:
                {
                    var newClassPart = classPart;

                    while (NodeCacheManager.Mappings.TryGetValue(newClassPart, out uint newID))
                    {
                        newClassPart = newID;
                    }

                    return newClassPart + chunkPart;
                }
            case IDRemap.TrackMania2006:
                {
                    if (NodeCacheManager.Mappings.ContainsValue(classPart))
                    {
                        return NodeCacheManager.Mappings.Last(x => x.Value == classPart).Key + chunkPart;
                    }

                    return chunkID;
                }
            default:
                return chunkID;
        }
    }

    public virtual void OnLoad() { }

    public int CompareTo(Chunk? other)
    {
        return Id.CompareTo(other?.Id);
    }

    void IChunk.Read(Node n, GameBoxReader r)
    {
        throw new NotSupportedException();
    }

    void IChunk.Write(Node n, GameBoxWriter w)
    {
        throw new NotSupportedException();
    }

    void IChunk.ReadWrite(Node n, GameBoxReaderWriter rw)
    {
        throw new NotSupportedException();
    }
}