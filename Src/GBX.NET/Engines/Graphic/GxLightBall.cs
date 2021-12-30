namespace GBX.NET.Engines.Graphic;

[Node(0x04002000)]
public class GxLightBall : GxLightPoint
{
    protected GxLightBall()
    {

    }

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