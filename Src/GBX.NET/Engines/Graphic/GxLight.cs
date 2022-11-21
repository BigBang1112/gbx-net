namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04001000</remarks>
[Node(0x04001000)]
public class GxLight : CMwNod
{
    private Vec3 color;
    private float intensity;
    private float shadowIntensity;
    private float flareIntensity;

    [NodeMember]
    [AppliedWithChunk<Chunk04001008>]
    [AppliedWithChunk<Chunk04001009>]
    public Vec3 Color { get => color; set => color = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk04001008>]
    [AppliedWithChunk<Chunk04001009>]
    public float Intensity { get => intensity; set => intensity = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk04001008>]
    [AppliedWithChunk<Chunk04001009>]
    public float ShadowIntensity { get => shadowIntensity; set => shadowIntensity = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk04001008>]
    [AppliedWithChunk<Chunk04001009>]
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
        public uint U04;
        public float U05;
        public float U06;
        public float U07;

        public override void ReadWrite(GxLight n, GameBoxReaderWriter rw)
        {
            rw.Vec3(ref n.color);
            rw.Single(ref n.intensity);
            rw.UInt32(ref U04); // DoData
            rw.Single(ref n.shadowIntensity);
            rw.Single(ref n.flareIntensity);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
        }
    }

    #endregion

    #region 0x009 chunk

    /// <summary>
    /// GxLight 0x009 chunk
    /// </summary>
    [Chunk(0x04001009)]
    public class Chunk04001009 : Chunk<GxLight>
    {
        public uint U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;

        public override void ReadWrite(GxLight n, GameBoxReaderWriter rw)
        {
            rw.Vec3(ref n.color);
            rw.UInt32(ref U04); // DoData
            rw.Single(ref n.intensity);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref n.shadowIntensity);
            rw.Single(ref n.flareIntensity);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
        }
    }

    #endregion
}