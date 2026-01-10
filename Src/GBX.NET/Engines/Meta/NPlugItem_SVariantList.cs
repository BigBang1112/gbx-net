namespace GBX.NET.Engines.Meta;

public partial class NPlugItem_SVariantList : IVersionable
{
    public int Version { get; set; }

    public override void ReadWrite(GbxReaderWriter rw)
    {
        ReadWrite(rw, v: 0);
    }
}
