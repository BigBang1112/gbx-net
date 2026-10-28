using GBX.NET.Serialization;

namespace GBX.NET.Engines.Game;

/// <summary>
/// Preserves an unimplemented or empty profile archive and its original class ID.
/// </summary>
public sealed class CGamePlayerProfileChunk_Unknown : CGamePlayerProfileChunk
{
    public uint ClassId { get; }
    public byte[] Data { get; set; }

    public CGamePlayerProfileChunk_Unknown(uint classId, byte[] data)
    {
        ClassId = classId;
        Data = data;
    }

    internal override void DeepCloneFields(CMwNod clone, DeepCloneContext context)
    {
        base.DeepCloneFields(clone, context);
        ((CGamePlayerProfileChunk_Unknown)clone).Data = context.CloneArray(Data)!;
    }
}
