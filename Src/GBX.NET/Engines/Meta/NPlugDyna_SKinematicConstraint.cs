namespace GBX.NET.Engines.Meta;

public partial class NPlugDyna_SKinematicConstraint : IVersionable
{
    public int Version { get; set; }

    public override void ReadWrite(GbxReaderWriter rw)
    {
        ReadWrite(rw, v: 0);
    }
}
