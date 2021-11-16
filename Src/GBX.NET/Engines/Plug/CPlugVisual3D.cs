namespace GBX.NET.Engines.Plug;

[Node(0x0902C000)]
public class CPlugVisual3D : CPlugVisual
{
    protected CPlugVisual3D()
    {

    }

    [Chunk(0x0902C002)]
    public class Chunk0902C002 : Chunk<CPlugVisual3D>
    {
        public int U01;

        public override void ReadWrite(CPlugVisual3D n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x0902C004)]
    public class Chunk0902C004 : Chunk<CPlugVisual3D>
    {
        public override void ReadWrite(CPlugVisual3D n, GameBoxReaderWriter rw)
        {

        }
    }
}
