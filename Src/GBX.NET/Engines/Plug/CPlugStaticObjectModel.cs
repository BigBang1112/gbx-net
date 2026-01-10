namespace GBX.NET.Engines.Plug;

public partial class CPlugStaticObjectModel : IVersionable
{
    public override void ReadWrite(GbxReaderWriter rw)
    {
        ReadWrite(rw, v: 0);
    }
}
