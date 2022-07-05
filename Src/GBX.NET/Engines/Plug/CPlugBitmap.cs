namespace GBX.NET.Engines.Plug;

/// <summary>
/// A texture.
/// </summary>
/// <remarks>ID: 0x09011000</remarks>
[Node(0x09011000), WritingNotSupported]
[NodeExtension("Texture")]
public class CPlugBitmap : CPlug
{
    protected CPlugBitmap()
    {

    }

    /// <summary>
    /// CPlugBitmap 0x015 chunk
    /// </summary>
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

    /// <summary>
    /// CPlugBitmap 0x017 chunk
    /// </summary>
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

    /// <summary>
    /// CPlugBitmap 0x018 chunk
    /// </summary>
    [Chunk(0x09011018), AutoReadWriteChunk]
    public class Chunk09011018 : Chunk<CPlugBitmap>
    {
        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            // Weird structure in TMS, TMNESWC, TMNF and TMTurbo
        }
    }

    /// <summary>
    /// CPlugBitmap 0x019 chunk
    /// </summary>
    [Chunk(0x09011019)]
    public class Chunk09011019 : Chunk<CPlugBitmap>
    {
        public float U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x01B chunk
    /// </summary>
    [Chunk(0x0901101B)]
    public class Chunk0901101B : Chunk<CPlugBitmap>
    {
        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            var flags = rw.Int32();
        }
    }

    /// <summary>
    /// CPlugBitmap 0x01C chunk
    /// </summary>
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

    /// <summary>
    /// CPlugBitmap 0x01D chunk
    /// </summary>
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

    /// <summary>
    /// CPlugBitmap 0x01E chunk
    /// </summary>
    [Chunk(0x0901101E)]
    public class Chunk0901101E : Chunk<CPlugBitmap>
    {
        public Vec2[]? U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Array<Vec2>(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x020 chunk
    /// </summary>
    [Chunk(0x09011020)]
    public class Chunk09011020 : Chunk<CPlugBitmap>
    {
        public float[]? U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Array<float>(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x022 chunk
    /// </summary>
    [Chunk(0x09011022)]
    public class Chunk09011022 : Chunk<CPlugBitmap>
    {
        public Node? TextureFile;
        public int? TextureFileIndex;
        public ulong U01;
        public float U02;
        public float U03;
        public float U04;
        public int U05;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref TextureFile, ref TextureFileIndex);
            rw.UInt64(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Int32(ref U05);

            if (U05 != 0)
            {
                throw new Exception("U05 != 0");
            }
        }
    }

    /// <summary>
    /// CPlugBitmap 0x024 chunk
    /// </summary>
    [Chunk(0x09011024)]
    public class Chunk09011024 : Chunk<CPlugBitmap>
    {
        public int U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }
}
