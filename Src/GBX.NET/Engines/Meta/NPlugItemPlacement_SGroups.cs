namespace GBX.NET.Engines.Meta;

public partial class NPlugItemPlacement_SGroups : IVersionable
{
    public int Version { get; set; } = 1;

    public override void ReadWrite(GbxReaderWriter rw)
    {
        ReadWrite(rw, v: 0);
    }
}
