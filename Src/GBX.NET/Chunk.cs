using GBX.NET.Debugging;
using System.Runtime.Serialization;

namespace GBX.NET;

public abstract class Chunk : IComparable<Chunk>
{
    private uint? id;
    private MemoryStream? unknown;

    /// <summary>
    /// Stream of unknown bytes
    /// </summary>
    [IgnoreDataMember]
    public MemoryStream Unknown => unknown ??= new MemoryStream();

    /// <summary>
    /// Raw data of the chunk. Always null with <see cref="GameBox.SeekForRawChunkData"/> set to false.
    /// </summary>
    public byte[]? RawData { get; internal set; }

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

                    while (NodeManager.TryGetMapping(newClassPart, out uint newID))
                    {
                        newClassPart = newID;
                    }

                    return newClassPart + chunkPart;
                }
            case IDRemap.TrackMania2006:
                {
                    if (classPart == 0x2E001000)
                    {
                        return 0x2400A000 + chunkPart;
                    }

                    if (NodeManager.TryGetReverseMapping(classPart, out uint prevId))
                    {
                        return prevId + chunkPart;
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