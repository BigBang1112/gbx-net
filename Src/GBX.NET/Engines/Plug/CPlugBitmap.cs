namespace GBX.NET.Engines.Plug;

public partial class CPlugBitmap
{
    public EGxUVGenerate GenerateUV => (EGxUVGenerate)(Flags2 >> 16);
}
