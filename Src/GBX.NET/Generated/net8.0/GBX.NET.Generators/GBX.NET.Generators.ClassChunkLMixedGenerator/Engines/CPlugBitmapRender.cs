namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09086000</remarks>
[Class(0x09086000)]
public partial class CPlugBitmapRender : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x09086000;




    private float fogCustomFarZ;
    [AppliedWithChunk<Chunk0908600A>]
    public float FogCustomFarZ { get => fogCustomFarZ; set => fogCustomFarZ = value; }

    private CPlugBitmap? bitmapClear;
    [AppliedWithChunk<Chunk0908600B>]
    public CPlugBitmap? BitmapClear { get => bitmapClear; set => bitmapClear = value; }

    private Vec2? bitmapClearUV;
    [AppliedWithChunk<Chunk0908600B>]
    public Vec2? BitmapClearUV { get => bitmapClearUV; set => bitmapClearUV = value; }

    private CPlugBitmapRenderSub? renderSub;
    [AppliedWithChunk<Chunk0908600C>]
    public CPlugBitmapRenderSub? RenderSub { get => renderSub; set => renderSub = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugBitmapRender"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugBitmapRender() { }


    /// <summary>
    /// CPlugBitmapRender 0x003 chunk
    /// </summary>
    [Chunk(0x09086003)]
    public partial class Chunk09086003 : Chunk<CPlugBitmapRender>
    {
        /// <inheritdoc />
        public override uint Id => 0x09086003;

        public short U01;
        public short U02;
        public short U03;
        public short U04;

        public override void ReadWrite(CPlugBitmapRender n, GbxReaderWriter rw)
        {
            rw.Int16(ref U01);
            rw.Int16(ref U02);
            rw.Int16(ref U03);
            rw.Int16(ref U04);
        }
    }

    /// <summary>
    /// CPlugBitmapRender 0x00A chunk
    /// </summary>
    [Chunk(0x0908600A)]
    public partial class Chunk0908600A : Chunk<CPlugBitmapRender>
    {
        /// <inheritdoc />
        public override uint Id => 0x0908600A;


        public override void ReadWrite(CPlugBitmapRender n, GbxReaderWriter rw)
        {
            rw.Single(ref n.fogCustomFarZ);
        }
    }

    /// <summary>
    /// CPlugBitmapRender 0x00B chunk
    /// </summary>
    [Chunk(0x0908600B)]
    public partial class Chunk0908600B : Chunk<CPlugBitmapRender>
    {
        /// <inheritdoc />
        public override uint Id => 0x0908600B;

        public int U01;
        public uint U02;

        public override void ReadWrite(CPlugBitmapRender n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.UInt32(ref U02);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapClear);
            rw.Vec2(ref n.bitmapClearUV);
        }
    }

    /// <summary>
    /// CPlugBitmapRender 0x00C chunk
    /// </summary>
    [Chunk(0x0908600C)]
    public partial class Chunk0908600C : Chunk<CPlugBitmapRender>
    {
        /// <inheritdoc />
        public override uint Id => 0x0908600C;


        public override void ReadWrite(CPlugBitmapRender n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmapRenderSub>(ref n.renderSub);
        }
    }

    /// <summary>
    /// CPlugBitmapRender 0x00D chunk
    /// </summary>
    [Chunk(0x0908600D)]
    public partial class Chunk0908600D : Chunk<CPlugBitmapRender>
    {
        /// <inheritdoc />
        public override uint Id => 0x0908600D;

        public uint U01;

        public override void ReadWrite(CPlugBitmapRender n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmapRender 0x00E chunk
    /// </summary>
    [Chunk(0x0908600E)]
    public partial class Chunk0908600E : Chunk<CPlugBitmapRender>
    {
        /// <inheritdoc />
        public override uint Id => 0x0908600E;

        public uint U01;

        public override void ReadWrite(CPlugBitmapRender n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09086003 => new Chunk09086003(),
        0x0908600A => new Chunk0908600A(),
        0x0908600B => new Chunk0908600B(),
        0x0908600C => new Chunk0908600C(),
        0x0908600D => new Chunk0908600D(),
        0x0908600E => new Chunk0908600E(),
        _ => base.NewChunk(chunkId),
    };
}
