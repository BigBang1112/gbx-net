namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04002000</remarks>
[Node(0x04002000)]
public class GxLightBall : GxLightPoint
{
    protected GxLightBall()
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
            rw.Array<float>(ref U01, 7);
        }
    }
}