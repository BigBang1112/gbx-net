namespace GBX.NET.Engines.Plug;

public partial class NPlugItemPlacement_SClass : IVersionable
{
    public int Version { get; set; }

    public override void ReadWrite(GbxReaderWriter rw)
    {
        ReadWrite(rw, v: 0);
    }
}
