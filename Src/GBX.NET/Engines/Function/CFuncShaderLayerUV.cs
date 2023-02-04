namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05015000</remarks>
[Node(0x05015000)]
[NodeExtension("FuncShader")]
public class CFuncShaderLayerUV : CFuncShader
{
    #region Fields

    private string? layerName;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk05015005>]
    public string? LayerName { get => layerName; set => layerName = value; }

    #endregion

    #region Constructors

    internal CFuncShaderLayerUV()
    {

    }

    #endregion

    #region Chunks

    #region 0x005 chunk

    /// <summary>
    /// CFuncShaderLayerUV 0x005 chunk
    /// </summary>
    [Chunk(0x05015005)]
    public class Chunk05015005 : Chunk<CFuncShaderLayerUV>
    {
        public int U01;

        public override void ReadWrite(CFuncShaderLayerUV n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.layerName);
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x009 chunk

    /// <summary>
    /// CFuncShaderLayerUV 0x009 chunk
    /// </summary>
    [Chunk(0x05015009)]
    public class Chunk05015009 : Chunk<CFuncShaderLayerUV>
    {
        public Vec2 U01;
        public Vec2 U02;

        public override void ReadWrite(CFuncShaderLayerUV n, GameBoxReaderWriter rw)
        {
            rw.Vec2(ref U01);
            rw.Vec2(ref U02);
        }
    }

    #endregion

    #region 0x00A chunk

    /// <summary>
    /// CFuncShaderLayerUV 0x00A chunk
    /// </summary>
    [Chunk(0x0501500A)]
    public class Chunk0501500A : Chunk05015009
    {
        public Vec2 U03;

        public override void ReadWrite(CFuncShaderLayerUV n, GameBoxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Vec2(ref U03);
        }
    }

    #endregion

    #region 0x00D chunk

    /// <summary>
    /// CFuncShaderLayerUV 0x00D chunk
    /// </summary>
    [Chunk(0x0501500D)]
    public class Chunk0501500D : Chunk<CFuncShaderLayerUV>
    {
        public Vec2 U01;
        public Vec2 U02;
        public Vec2 U03;

        public override void ReadWrite(CFuncShaderLayerUV n, GameBoxReaderWriter rw)
        {
            rw.Vec2(ref U01);
            rw.Vec2(ref U02);
            rw.Vec2(ref U03);
        }
    }

    #endregion

    #region 0x012 chunk

    /// <summary>
    /// CFuncShaderLayerUV 0x012 chunk
    /// </summary>
    [Chunk(0x05015012)]
    public class Chunk05015012 : Chunk<CFuncShaderLayerUV>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CFuncShaderLayerUV n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    #endregion

    #region 0x014 chunk

    /// <summary>
    /// CFuncShaderLayerUV 0x014 chunk
    /// </summary>
    [Chunk(0x05015014)]
    public class Chunk05015014 : Chunk<CFuncShaderLayerUV>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;

        public override void ReadWrite(CFuncShaderLayerUV n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
        }
    }

    #endregion

    #region 0x015 chunk

    /// <summary>
    /// CFuncShaderLayerUV 0x015 chunk
    /// </summary>
    [Chunk(0x05015015)]
    public class Chunk05015015 : Chunk<CFuncShaderLayerUV>
    {
        public int U01;
        public int U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CFuncShaderLayerUV n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04); // should be bool but it isnt?
        }
    }

    #endregion

    #region 0x016 chunk

    /// <summary>
    /// CFuncShaderLayerUV 0x016 chunk
    /// </summary>
    [Chunk(0x05015016)]
    public class Chunk05015016 : Chunk<CFuncShaderLayerUV>
    {
        public uint U01;
        
        public override void ReadWrite(CFuncShaderLayerUV n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01); // DoData
        }
    }

    #endregion

    #endregion
}
