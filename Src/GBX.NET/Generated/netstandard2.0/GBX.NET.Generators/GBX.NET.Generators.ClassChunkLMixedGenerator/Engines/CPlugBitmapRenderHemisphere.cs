namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09058000</remarks>
[Class(0x09058000)]
public partial class CPlugBitmapRenderHemisphere : CPlugBitmapRender, IClass
{
    [Hexadecimal] public static new uint Id => 0x09058000;




    private EHemiLayout hemiLayout;
    [AppliedWithChunk<Chunk09058001>]
    public EHemiLayout HemiLayout { get => hemiLayout; set => hemiLayout = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugBitmapRenderHemisphere"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugBitmapRenderHemisphere() { }


    /// <summary>
    /// CPlugBitmapRenderHemisphere 0x001 chunk
    /// </summary>
    [Chunk(0x09058001)]
    public partial class Chunk09058001 : Chunk<CPlugBitmapRenderHemisphere>
    {
        /// <inheritdoc />
        public override uint Id => 0x09058001;

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;

        public override void ReadWrite(CPlugBitmapRenderHemisphere n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EHemiLayout>(ref n.hemiLayout);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
        }
    }



    public enum EHemiLayout
    {
        _1,
        _2_4_16,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09058001 => new Chunk09058001(),
        _ => base.NewChunk(chunkId),
    };
}
