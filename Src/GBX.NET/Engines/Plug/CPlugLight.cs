using GBX.NET.Engines.Graphic;

namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0901D000</remarks>
[Node(0x0901D000)]
public class CPlugLight : CPlug
{
    private GxLight? gxLightModel;
    private CFuncLight? funcLight;
    private CPlugBitmap? bitmapFlare;
    private CPlugBitmap? bitmapProjector;

    [NodeMember(ExactName = "m_GxLightModel")]
    public GxLight? GxLightModel { get => gxLightModel; set => gxLightModel = value; }

    [NodeMember(ExactName = "m_FuncLight")]
    public CFuncLight? FuncLight { get => funcLight; set => funcLight = value; }

    [NodeMember(ExactName = "m_BitmapFlare")]
    public CPlugBitmap? BitmapFlare { get => bitmapFlare; set => bitmapFlare = value; }

    [NodeMember(ExactName = "m_BitmapProjector")]
    public CPlugBitmap? BitmapProjector { get => bitmapProjector; set => bitmapProjector = value; }

    protected CPlugLight()
    {

    }

    /// <summary>
    /// CPlugLight 0x000 chunk
    /// </summary>
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