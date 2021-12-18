namespace GBX.NET.Engines.Plug;

[Node(0x09004000)]
public class CPlugShaderGeneric : CPlugShader
{
    protected CPlugShaderGeneric()
    {

    }

    [Chunk(0x09004003)]
    public class Chunk09004003 : Chunk<CPlugShaderGeneric>
    {
        public float[]? U01;

        public override void ReadWrite(CPlugShaderGeneric n, GameBoxReaderWriter rw)
        {
            rw.Array<float>(ref U01, count: 22);
        }
    }
}