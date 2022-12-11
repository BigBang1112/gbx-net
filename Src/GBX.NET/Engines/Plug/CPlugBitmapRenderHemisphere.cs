namespace GBX.NET.Engines.Plug;

[Node(0x09058000)]
public class CPlugBitmapRenderHemisphere : CPlugBitmapRender
{
    public enum EHemiLayout
    {
        _1,
        _2_4_16
    }
    
    private EHemiLayout hemiLayout;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09058001>]
    public EHemiLayout HemiLayout { get => hemiLayout; set => hemiLayout = value; }

    internal CPlugBitmapRenderHemisphere()
	{
        
	}

    #region 0x001 chunk

    /// <summary>
    /// CPlugBitmapRenderHemisphere 0x001 chunk
    /// </summary>
    [Chunk(0x09058001)]
    public class Chunk09058001 : Chunk<CPlugBitmapRenderHemisphere>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;

        public override void ReadWrite(CPlugBitmapRenderHemisphere n, GameBoxReaderWriter rw)
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

    #endregion
}
