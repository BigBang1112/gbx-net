namespace GBX.NET.Engines.Plug;

public partial class CPlugDynaModel
{
    public override void ReadWrite(GbxReaderWriter rw)
    {
        ReadWrite(rw, v: 0);
    }
}
