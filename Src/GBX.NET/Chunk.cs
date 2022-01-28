using GBX.NET.Debugging;
using System.Reflection;
using System.Runtime.Serialization;

namespace GBX.NET;

public abstract class Chunk : IComparable<Chunk>
{
    private uint? id;

    /// <summary>
    /// Stream of unknown bytes
    /// </summary>
    [IgnoreDataMember]
    public MemoryStream Unknown { get; } = new MemoryStream();

    internal Node Node { get; set; }

    public ChunkDebugger? Debugger { get; internal set; }

    protected Chunk(Node node)
    {
        Node = node;
    }

    public uint Id => GetStoredId();

    private uint GetStoredId()
    {
        id ??= GetId();
        return id.Value;
    }

    protected abstract uint GetId();

    public override int GetHashCode() => (int)Id;

    public override bool Equals(object? obj) => Equals(obj as Chunk);

    public override string ToString() => $"Chunk 0x{Id:X8}";

    public bool Equals(Chunk? chunk) => chunk is not null && chunk.Id == Id;

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
}