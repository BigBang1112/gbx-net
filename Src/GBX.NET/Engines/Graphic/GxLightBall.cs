namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04002000</remarks>
[Node(0x04002000)]
public class GxLightBall : GxLightPoint
{
    private Vec3 ambientRGB;
    private float emittingRadius;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk04002006>]
    public Vec3 AmbientRGB { get => ambientRGB; set => ambientRGB = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk04002006>]
    public float EmittingRadius { get => emittingRadius; set => emittingRadius = value; }

    internal GxLightBall()
    {

    }

    /// <summary>
    /// GxLightBall 0x002 chunk
    /// </summary>
    [Chunk(0x04002002)]
    public class Chunk04002002 : Chunk<GxLightBall>
    {
        public float[]? U01;

        public override void ReadWrite(GxLightBall n, GameBoxReaderWriter rw)
        {
            rw.Array<float>(ref U01, 7); // Incorrect
        }
    }

    #region 0x006 chunk

    /// <summary>
    /// GxLightBall 0x006 chunk
    /// </summary>
    [Chunk(0x04002006)]
    public class Chunk04002006 : Chunk<GxLightBall>
    {
        public uint U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;

        public override void ReadWrite(GxLightBall n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01); // DoData
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref n.emittingRadius);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Vec3(ref n.ambientRGB);
        }
    }

    #endregion
}