namespace GBX.NET.Engines.Plug;

[Node(0x09086000)]
public abstract class CPlugBitmapRender : CPlug
{
    private float fogCustomFarZ;
    private CPlugBitmap? bitmapClear;
    private Vec2? bitmapClearUV;
    private CPlugBitmapRenderSub? renderSub;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0908600A>]
    public float FogCustomFarZ { get => fogCustomFarZ; set => fogCustomFarZ = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0908600B>]
    public CPlugBitmap? BitmapClear { get => bitmapClear; set => bitmapClear = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0908600B>]
    public Vec2? BitmapClearUV { get => bitmapClearUV; set => bitmapClearUV = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0908600C>]
    public CPlugBitmapRenderSub? RenderSub { get => renderSub; set => renderSub = value; }

    internal CPlugBitmapRender()
	{
        
	}

    #region 0x003 chunk

    /// <summary>
    /// CPlugBitmapRender 0x003 chunk
    /// </summary>
    [Chunk(0x09086003)]
    public class Chunk09086003 : Chunk<CPlugBitmapRender>
    {
        public short U01;
        public short U02;

        public override void ReadWrite(CPlugBitmapRender n, GameBoxReaderWriter rw)
        {
            for (var i = 0; i < 2; i++)
            {
                rw.Int16(ref U01);
                rw.Int16(ref U02);
            }
        }
    }

    #endregion

    #region 0x00A chunk

    /// <summary>
    /// CPlugBitmapRender 0x00A chunk
    /// </summary>
    [Chunk(0x0908600A)]
    public class Chunk0908600A : Chunk<CPlugBitmapRender>
    {
        public override void ReadWrite(CPlugBitmapRender n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.fogCustomFarZ);
        }
    }

    #endregion

    #region 0x00B chunk

    /// <summary>
    /// CPlugBitmapRender 0x00B chunk
    /// </summary>
    [Chunk(0x0908600B)]
    public class Chunk0908600B : Chunk<CPlugBitmapRender>
    {
        public int U01;
        public uint U02;

        public override void ReadWrite(CPlugBitmapRender n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // DoMask
            rw.UInt32(ref U02); // DoData
            rw.NodeRef<CPlugBitmap>(ref n.bitmapClear);
            rw.Vec2(ref n.bitmapClearUV);
        }
    }

    #endregion

    #region 0x00C chunk

    /// <summary>
    /// CPlugBitmapRender 0x00C chunk
    /// </summary>
    [Chunk(0x0908600C)]
    public class Chunk0908600C : Chunk<CPlugBitmapRender>
    {
        public override void ReadWrite(CPlugBitmapRender n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmapRenderSub>(ref n.renderSub);
        }
    }

    #endregion

    #region 0x00D chunk

    /// <summary>
    /// CPlugBitmapRender 0x00D chunk
    /// </summary>
    [Chunk(0x0908600D)]
    public class Chunk0908600D : Chunk<CPlugBitmapRender>
    {
        public uint U01;

        public override void ReadWrite(CPlugBitmapRender n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01); // DoData
        }
    }

    #endregion

    #region 0x00E chunk

    /// <summary>
    /// CPlugBitmapRender 0x00E chunk
    /// </summary>
    [Chunk(0x0908600E)]
    public class Chunk0908600E : Chunk<CPlugBitmapRender>
    {
        public uint U01;
        
        public override void ReadWrite(CPlugBitmapRender n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01); // DoData
        }
    }

    #endregion
}