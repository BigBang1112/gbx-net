namespace GBX.NET.Engines.Plug;

/// <summary>
/// Shader (0x09002000)
/// </summary>
[Node(0x09002000)]
public abstract class CPlugShader : CPlug
{
    protected CPlugShader()
    {

    }

    [Chunk(0x0900200E)]
    public class Chunk0900200E : Chunk<CPlugShader>
    {
        public int U01;
        public int U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
        }
    }

    [Chunk(0x09002010)]
    public class Chunk09002010 : Chunk<CPlugShader>
    {
        public ulong U01;
        public float U02;
        public CMwNod? U03;
        public short U04;

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.Single(ref U02);
            rw.NodeRef(ref U03);
            rw.Int16(ref U04);
        }
    }

    [Chunk(0x09002014)]
    public class Chunk09002014 : Chunk<CPlugShader>
    {
        public ulong U01;
        public float U02;
        public CMwNod? U03;
        public short U04;

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.Single(ref U02);
            rw.NodeRef(ref U03);
            rw.Int16(ref U04);
        }
    }

    [Chunk(0x09002015)]
    public class Chunk09002015 : Chunk<CPlugShader>
    {
        public ulong U01;
        public float U02;
        public CMwNod? U03;
        public short U04;

        public override void ReadWrite(CPlugShader n, GameBoxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.Single(ref U02);
            rw.NodeRef(ref U03);
            rw.Int16(ref U04);
        }
    }
}