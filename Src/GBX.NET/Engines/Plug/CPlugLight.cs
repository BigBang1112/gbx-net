namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0901D000</remarks>
[Node(0x0901D000)]
[NodeExtension("Light")]
public class CPlugLight : CPlug
{
    private GxLight? gxLightModel;
    private CFuncLight? funcLight;
    private CPlugBitmap? bitmapFlare;
    private GameBoxRefTable.File? bitmapFlareFile;
    private CPlugBitmap? bitmapProjector;
    private GameBoxRefTable.File? bitmapProjectorFile;

    [NodeMember(ExactName = "m_GxLightModel")]
    [AppliedWithChunk<Chunk0901D000>]
    [AppliedWithChunk<Chunk0901D002>]
    public GxLight? GxLightModel { get => gxLightModel; set => gxLightModel = value; }

    [NodeMember(ExactName = "m_FuncLight")]
    [AppliedWithChunk<Chunk0901D000>]
    [AppliedWithChunk<Chunk0901D002>]
    public CFuncLight? FuncLight { get => funcLight; set => funcLight = value; }

    [NodeMember(ExactName = "m_BitmapFlare")]
    [AppliedWithChunk<Chunk0901D000>]
    [AppliedWithChunk<Chunk0901D002>]
    public CPlugBitmap? BitmapFlare
    {
        get => bitmapFlare = GetNodeFromRefTable(bitmapFlare, bitmapFlareFile) as CPlugBitmap;
        set => bitmapFlare = value;
    }

    [NodeMember(ExactName = "m_BitmapProjector")]
    [AppliedWithChunk<Chunk0901D000>]
    [AppliedWithChunk<Chunk0901D002>]
    public CPlugBitmap? BitmapProjector
    {
        get => bitmapProjector = GetNodeFromRefTable(bitmapProjector, bitmapProjectorFile) as CPlugBitmap;
        set => bitmapProjector = value;
    }

    internal CPlugLight()
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
            rw.NodeRef<CPlugBitmap>(ref n.bitmapFlare, ref n.bitmapFlareFile);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapProjector, ref n.bitmapProjectorFile);
        }
    }

    /// <summary>
    /// CPlugLight 0x002 chunk
    /// </summary>
    [Chunk(0x0901D002)]
    public class Chunk0901D002 : Chunk0901D000
    {
        public uint U01;

        public override void ReadWrite(CPlugLight n, GameBoxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.UInt32(ref U01); // DoData
        }
    }
}