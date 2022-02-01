using GBX.NET.Engines.Graphic;

namespace GBX.NET.Engines.Plug;

[Node(0x0901D000)]
public class CPlugLight : CPlug
{
    private GxLight? gxLightModel;
    private CFuncLight? funcLight;
    private CPlugBitmap? bitmapFlare;
    private CPlugBitmap? bitmapProjector;

    public GxLight? GxLightModel
    {
        get => gxLightModel;
        set => gxLightModel = value;
    }

    public CFuncLight? FuncLight
    {
        get => funcLight;
        set => funcLight = value;
    }

    public CPlugBitmap? BitmapFlare
    {
        get => bitmapFlare;
        set => bitmapFlare = value;
    }

    public CPlugBitmap? BitmapProjector
    {
        get => bitmapProjector;
        set => bitmapProjector = value;
    }

    protected CPlugLight()
    {

    }

    [Chunk(0x0901D000)]
    public class Chunk0901D000 : Chunk<CPlugLight>
    {
        public override void ReadWrite(CPlugLight n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<GxLight>(ref n.gxLightModel);
            rw.NodeRef<CFuncLight>(ref n.funcLight);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapFlare);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapProjector);
        }
    }
}