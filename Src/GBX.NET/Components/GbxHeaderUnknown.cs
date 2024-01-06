namespace GBX.NET.Components;

public sealed class GbxHeaderUnknown(GbxHeaderBasic basic, uint classId, IHeaderChunkSet? userData = null) : GbxHeader(basic, userData)
{
    public override uint ClassId => classId;
}
