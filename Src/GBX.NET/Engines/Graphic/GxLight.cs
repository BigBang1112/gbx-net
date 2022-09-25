namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04001000</remarks>
[Node(0x04001000)]
public class GxLight : CMwNod
{
    private float intensity;
    private float shadowIntensity;
    private float flareIntensity;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk04001008))]
    public float Intensity { get => intensity; set => intensity = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk04001008))]
    public float ShadowIntensity { get => shadowIntensity; set => shadowIntensity = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk04001008))]
    public float FlareIntensity { get => flareIntensity; set => flareIntensity = value; }

    internal GxLight()
    {

    }

    #region 0x008 chunk

    /// <summary>
    /// GxLight 0x008 chunk
    /// </summary>
    [Chunk(0x04001008)]
    public class Chunk04001008 : Chunk<GxLight>
    {
        private float U01;
        private float U02;
        private float U03;
        private int U04;
        private float U05;
        private float U06;
        private float U07;

        public override void ReadWrite(GxLight n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref n.intensity);
            rw.Int32(ref U04); // DoData
            rw.Single(ref n.shadowIntensity);
            rw.Single(ref n.flareIntensity);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
        }
    }

    #endregion
}