namespace GBX.NET.Engines.Plug;

[Node(0x09011000), WritingNotSupported]
public class CPlugBitmap : CPlug
{
    protected CPlugBitmap()
    {

    }

    [Chunk(0x09011015)]
    public class Chunk09011015 : Chunk<CPlugBitmap>
    {
        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            var u01 = rw.UInt64();
            var u02 = rw.Single();
            var u03 = rw.Single();
            var u04 = rw.Single();
            var u05 = rw.Int32();
            var u06 = rw.Int32();
        }
    }

    [Chunk(0x09011017)]
    public class Chunk09011017 : Chunk<CPlugBitmap>
    {
        public int U01;
        public Vec2 U02;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Vec2(ref U02);
        }
    }

    [Chunk(0x09011018), AutoReadWriteChunk]
    public class Chunk09011018 : Chunk<CPlugBitmap>
    {
        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            // Weird structure in TMS, TMNESWC, TMNF and TMTurbo
        }
    }

    [Chunk(0x09011019)]
    public class Chunk09011019 : Chunk<CPlugBitmap>
    {
        public float U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    [Chunk(0x0901101B)]
    public class Chunk0901101B : Chunk<CPlugBitmap>
    {
        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            var flags = rw.Int32();
        }
    }

    [Chunk(0x0901101C)]
    public class Chunk0901101C : Chunk<CPlugBitmap>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
        }
    }

    [Chunk(0x0901101D)]
    public class Chunk0901101D : Chunk<CPlugBitmap>
    {
        public short U01;
        public short U02;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Int16(ref U01);
            rw.Int16(ref U02);
        }
    }
}
