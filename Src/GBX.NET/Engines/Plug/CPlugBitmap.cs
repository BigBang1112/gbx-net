namespace GBX.NET.Engines.Plug;

/// <summary>
/// A texture.
/// </summary>
/// <remarks>ID: 0x09011000</remarks>
[Node(0x09011000), WritingNotSupported]
[NodeExtension("Texture")]
public class CPlugBitmap : CPlug
{
    internal CPlugBitmap()
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
    [Chunk(0x09011018)]
    public class Chunk09011018 : Chunk<CPlugBitmap>
    {
        public Node? TextureFile;
        public GameBoxRefTable.File? TextureFileIndex;
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
    public class Chunk09011022 : Chunk09011018
    {
        
    }

    #region 0x023 chunk

    /// <summary>
    /// CPlugBitmap 0x023 chunk
    /// </summary>
    [Chunk(0x09011023)]
    public class Chunk09011023 : Chunk<CPlugBitmap>
    {
        public uint U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }

    #endregion

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

    #region 0x025 chunk

    /// <summary>
    /// CPlugBitmap 0x025 chunk
    /// </summary>
    [Chunk(0x09011025)]
    public class Chunk09011025 : Chunk<CPlugBitmap>
    {
        public Vec2 U01;
        public Vec2 U02;
        public float U03;
        public uint U04;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Vec2(ref U01);
            rw.Vec2(ref U02);
            rw.Single(ref U03);
            rw.UInt32(ref U04); // DoMask
        }
    }

    #endregion

    #region 0x028 chunk

    /// <summary>
    /// CPlugBitmap 0x028 chunk
    /// </summary>
    [Chunk(0x09011028)]
    public class Chunk09011028 : Chunk<CPlugBitmap>
    {
        public Int2 U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Int2(ref U01);
        }
    }

    #endregion

    #region 0x02A chunk

    /// <summary>
    /// CPlugBitmap 0x02A chunk
    /// </summary>
    [Chunk(0x0901102A)]
    public class Chunk0901102A : Chunk<CPlugBitmap>
    {
        public Node? U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x02B chunk

    /// <summary>
    /// CPlugBitmap 0x02B chunk
    /// </summary>
    [Chunk(0x0901102B)]
    public class Chunk0901102B : Chunk<CPlugBitmap>
    {
        public Node? U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x02C chunk

    /// <summary>
    /// CPlugBitmap 0x02C chunk
    /// </summary>
    [Chunk(0x0901102C)]
    public class Chunk0901102C : Chunk<CPlugBitmap>
    {
        public Node? U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x02D chunk

    /// <summary>
    /// CPlugBitmap 0x02D chunk
    /// </summary>
    [Chunk(0x0901102D)]
    public class Chunk0901102D : Chunk<CPlugBitmap>, IVersionable
    {
        private int version;

        public int U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x030 chunk

    /// <summary>
    /// CPlugBitmap 0x030 chunk
    /// </summary>
    [Chunk(0x09011030)]
    public class Chunk09011030 : Chunk<CPlugBitmap>, IVersionable
    {
        private int version;

        public int U01;
        public Int3 U02;
        public float U03;
        public float U04;
        public float U05;
        public uint U06;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U01); // texture node?

            if (version >= 1)
            {
                rw.Int3(ref U02); // DoData
            }

            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.UInt32(ref U06); // DoMask
        }
    }

    #endregion

    #region 0x032 chunk

    /// <summary>
    /// CPlugBitmap 0x032 chunk
    /// </summary>
    [Chunk(0x09011032)]
    public class Chunk09011032 : Chunk<CPlugBitmap>
    {
        private int version;

        public uint U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.UInt32(ref U01); // DoData
        }
    }

    #endregion
}
