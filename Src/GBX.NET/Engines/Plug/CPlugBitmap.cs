namespace GBX.NET.Engines.Plug;

/// <summary>
/// A texture.
/// </summary>
/// <remarks>ID: 0x09011000</remarks>
[Node(0x09011000)]
[NodeExtension("Texture")]
public class CPlugBitmap : CPlug
{
    private float mipMapLowerAlpha;
    private float bumpScaleFactor;
    private float mipMapLodBiasDefault;
    private float bumpScaleMipLevel;
    private Vec2 defaultTexCoordScale;
    private Node? image;
    private GameBoxRefTable.File? imageFile;
    private Vec2 defaultTexCoordTrans;
    private float defaultTexCoordRotate;
    private float[]? mipMapFadeAlphas;
    private CPlugSpriteParam? spriteParam;
    private CPlugBitmapAtlas? atlas;
    private CPlugBitmapDecals? decals;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09011015>]
    [AppliedWithChunk<Chunk09011018>]
    [AppliedWithChunk<Chunk09011022>]
    [AppliedWithChunk<Chunk09011030>]
    public float MipMapLowerAlpha { get => mipMapLowerAlpha; set => mipMapLowerAlpha = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09011015>]
    [AppliedWithChunk<Chunk09011018>]
    [AppliedWithChunk<Chunk09011022>]
    [AppliedWithChunk<Chunk09011030>]
    public float BumpScaleFactor { get => bumpScaleFactor; set => bumpScaleFactor = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09011015>]
    [AppliedWithChunk<Chunk09011018>]
    [AppliedWithChunk<Chunk09011022>]
    [AppliedWithChunk<Chunk09011030>]
    public float MipMapLodBiasDefault { get => mipMapLodBiasDefault; set => mipMapLodBiasDefault = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09011017>]
    [AppliedWithChunk<Chunk0901101C>]
    public Vec2 DefaultTexCoordScale { get => defaultTexCoordScale; set => defaultTexCoordScale = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0901101C>]
    public Vec2 DefaultTexCoordTrans { get => defaultTexCoordTrans; set => defaultTexCoordTrans = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0901101C>]
    public float DefaultTexCoordRotate { get => defaultTexCoordRotate; set => defaultTexCoordRotate = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09011018>]
    [AppliedWithChunk<Chunk09011022>]
    [AppliedWithChunk<Chunk09011030>]
    public Node? Image { get => image; set => image = value; }
    public GameBoxRefTable.File? ImageFile { get => imageFile; set => imageFile = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09011019>]
    public float BumpScaleMipLevel { get => bumpScaleMipLevel; set => bumpScaleMipLevel = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09011020>]
    public float[]? MipMapFadeAlphas { get => mipMapFadeAlphas; set => mipMapFadeAlphas = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0901102A>]
    public CPlugSpriteParam? SpriteParam { get => spriteParam; set => spriteParam = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0901102B>]
    public CPlugBitmapAtlas? Atlas { get => atlas; set => atlas = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0901102C>]
    public CPlugBitmapDecals? Decals { get => decals; set => decals = value; }

    internal CPlugBitmap()
    {

    }

    /// <summary>
    /// CPlugBitmap 0x015 chunk
    /// </summary>
    [Chunk(0x09011015)]
    public class Chunk09011015 : Chunk<CPlugBitmap>
    {
        public ulong U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.UInt64(ref U01);
            rw.Single(ref n.mipMapLowerAlpha);
            rw.Single(ref n.bumpScaleFactor);
            rw.Single(ref n.mipMapLodBiasDefault);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x017 chunk
    /// </summary>
    [Chunk(0x09011017)]
    public class Chunk09011017 : Chunk<CPlugBitmap>
    {
        public int U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Vec2(ref n.defaultTexCoordScale);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x018 chunk
    /// </summary>
    [Chunk(0x09011018)]
    public class Chunk09011018 : Chunk<CPlugBitmap>
    {
        public ulong U01;
        public int U05;
        public Node? U06;
        
        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.image, ref n.imageFile);
            rw.UInt64(ref U01);
            rw.Single(ref n.mipMapLowerAlpha);
            rw.Single(ref n.bumpScaleFactor);
            rw.Single(ref n.mipMapLodBiasDefault);
            rw.Int32(ref U05);

            if (n.image is CPlugFileGen)
            {
                rw.NodeRef(ref U06);
            }
        }
    }

    /// <summary>
    /// CPlugBitmap 0x019 chunk
    /// </summary>
    [Chunk(0x09011019)]
    public class Chunk09011019 : Chunk<CPlugBitmap>
    {
        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.bumpScaleMipLevel);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x01B chunk
    /// </summary>
    [Chunk(0x0901101B)]
    public class Chunk0901101B : Chunk<CPlugBitmap>
    {
        public uint Flags;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref Flags);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x01C chunk
    /// </summary>
    [Chunk(0x0901101C)]
    public class Chunk0901101C : Chunk<CPlugBitmap>
    {
        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Vec2(ref n.defaultTexCoordScale);
            rw.Vec2(ref n.defaultTexCoordTrans);
            rw.Single(ref n.defaultTexCoordRotate);
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

    #region 0x01F chunk

    /// <summary>
    /// CPlugBitmap 0x01F chunk
    /// </summary>
    [Chunk(0x0901101F)]
    public class Chunk0901101F : Chunk<CPlugBitmap>
    {
        public uint U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }

    #endregion

    /// <summary>
    /// CPlugBitmap 0x020 chunk
    /// </summary>
    [Chunk(0x09011020)]
    public class Chunk09011020 : Chunk<CPlugBitmap>
    {
        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Array<float>(ref n.mipMapFadeAlphas);
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
            rw.UInt32(ref U01); // DoData
        }
    }

    #endregion

    #region 0x024 chunk

    /// <summary>
    /// CPlugBitmap 0x024 chunk
    /// </summary>
    [Chunk(0x09011024)]
    public class Chunk09011024 : Chunk<CPlugBitmap>
    {
        public uint U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01); // DoData
        }
    }

    #endregion

    #region 0x025 chunk

    /// <summary>
    /// CPlugBitmap 0x025 chunk
    /// </summary>
    [Chunk(0x09011025)]
    public class Chunk09011025 : Chunk0901101C
    {
        public uint U01;

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.UInt32(ref U01); // DoMask
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
        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugSpriteParam>(ref n.spriteParam);
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
        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmapAtlas>(ref n.atlas);
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
        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmapDecals>(ref n.decals);
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
        
        public Int3 U02;
        public float U03;
        public float U04;
        public float U05;
        public uint U06;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugBitmap n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef(ref n.image, ref n.imageFile);

            if (version >= 1)
            {
                rw.Int3(ref U02); // DoData
            }

            if (version == 2)
            {
                // GrassId_ImageFid
            }

            rw.Single(ref n.mipMapLowerAlpha);
            rw.Single(ref n.bumpScaleFactor);
            rw.Single(ref n.mipMapLodBiasDefault);
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
