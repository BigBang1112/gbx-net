using GBX.NET.Managers;

namespace GBX.NET.Components;

public sealed class GbxHeaderUnknown(GbxHeaderBasic basic, uint classId) : GbxHeader(basic)
{
    public override uint ClassId => classId;
    public ISet<HeaderChunk> UserData { get; } = new SortedSet<HeaderChunk>(ChunkIdComparer.Default);

    public override string ToString()
    {
        return $"GbxHeader ({ClassManager.GetName(ClassId)}, 0x{ClassId:X8}, unknown)";
    }
}
