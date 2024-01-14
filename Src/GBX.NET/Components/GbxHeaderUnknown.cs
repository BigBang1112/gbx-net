namespace GBX.NET.Components;

public sealed class GbxHeaderUnknown(GbxHeaderBasic basic, uint classId) : GbxHeader(basic)
{
    public override uint ClassId => classId;
    public ISet<HeaderChunk> UserData { get; } = new SortedSet<HeaderChunk>();
}
