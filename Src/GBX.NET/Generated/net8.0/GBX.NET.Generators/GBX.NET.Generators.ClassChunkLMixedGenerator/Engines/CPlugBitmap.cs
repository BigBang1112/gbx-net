namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09011000</remarks>
[Class(0x09011000)]
public partial class CPlugBitmap : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x09011000;




    private CPlugFileImg? image;
    [AppliedWithChunk<Chunk09011014>]
    [AppliedWithChunk<Chunk09011015>]
    [AppliedWithChunk<Chunk09011018>]
    [AppliedWithChunk<Chunk09011022>]
    public CPlugFileImg? Image { get => imageFile?.GetNode(ref image) ?? image; set => image = value; }
    private Components.GbxRefTableFile? imageFile;
    public Components.GbxRefTableFile? ImageFile { get => imageFile; set => imageFile = value; }
    public CPlugFileImg? GetImage(GbxReadSettings settings = default, bool exceptions = false) => imageFile?.GetNode(ref image, settings, exceptions) ?? image;

    private ulong flags;
    [AppliedWithChunk<Chunk09011014>]
    [AppliedWithChunk<Chunk09011015>]
    [AppliedWithChunk<Chunk09011018>]
    [AppliedWithChunk<Chunk09011022>]
    public ulong Flags { get => flags; set => flags = value; }

    private float mipMapLowerAlpha;
    [AppliedWithChunk<Chunk09011014>]
    [AppliedWithChunk<Chunk09011015>]
    [AppliedWithChunk<Chunk09011018>]
    [AppliedWithChunk<Chunk09011022>]
    public float MipMapLowerAlpha { get => mipMapLowerAlpha; set => mipMapLowerAlpha = value; }

    private float bumpScaleFactor;
    [AppliedWithChunk<Chunk09011014>]
    [AppliedWithChunk<Chunk09011015>]
    [AppliedWithChunk<Chunk09011018>]
    [AppliedWithChunk<Chunk09011022>]
    public float BumpScaleFactor { get => bumpScaleFactor; set => bumpScaleFactor = value; }

    private float mipMapLodBiasDefault;
    [AppliedWithChunk<Chunk09011014>]
    [AppliedWithChunk<Chunk09011015>]
    [AppliedWithChunk<Chunk09011018>]
    [AppliedWithChunk<Chunk09011022>]
    public float MipMapLodBiasDefault { get => mipMapLodBiasDefault; set => mipMapLodBiasDefault = value; }

    private int borderRGB;
    [AppliedWithChunk<Chunk09011014>]
    [AppliedWithChunk<Chunk09011015>]
    [AppliedWithChunk<Chunk09011018>]
    [AppliedWithChunk<Chunk09011022>]
    public int BorderRGB { get => borderRGB; set => borderRGB = value; }

    private uint flags2;
    [AppliedWithChunk<Chunk09011017>]
    [AppliedWithChunk<Chunk0901101B>]
    [AppliedWithChunk<Chunk0901101F>]
    [AppliedWithChunk<Chunk09011024>]
    public uint Flags2 { get => flags2; set => flags2 = value; }

    private Vec2 defaultTexCoordScale;
    [AppliedWithChunk<Chunk09011017>]
    [AppliedWithChunk<Chunk0901101C>]
    [AppliedWithChunk<Chunk09011025>]
    public Vec2 DefaultTexCoordScale { get => defaultTexCoordScale; set => defaultTexCoordScale = value; }

    private float bumpScaleMipLevel;
    [AppliedWithChunk<Chunk09011019>]
    public float BumpScaleMipLevel { get => bumpScaleMipLevel; set => bumpScaleMipLevel = value; }

    private Vec2 defaultTexCoordTrans;
    [AppliedWithChunk<Chunk0901101C>]
    [AppliedWithChunk<Chunk09011025>]
    public Vec2 DefaultTexCoordTrans { get => defaultTexCoordTrans; set => defaultTexCoordTrans = value; }

    private float defaultTexCoordRotate;
    [AppliedWithChunk<Chunk0901101C>]
    [AppliedWithChunk<Chunk09011025>]
    public float DefaultTexCoordRotate { get => defaultTexCoordRotate; set => defaultTexCoordRotate = value; }

    private Vec2[]? atlasCountUVs;
    [AppliedWithChunk<Chunk0901101E>]
    public Vec2[]? AtlasCountUVs { get => atlasCountUVs; set => atlasCountUVs = value; }

    private float[]? mipMapFadeAlphas;
    [AppliedWithChunk<Chunk09011020>]
    public float[]? MipMapFadeAlphas { get => mipMapFadeAlphas; set => mipMapFadeAlphas = value; }

    private CPlugSpriteParam? spriteParam;
    [AppliedWithChunk<Chunk0901102A>]
    public CPlugSpriteParam? SpriteParam { get => spriteParam; set => spriteParam = value; }

    private CPlugBitmapAtlas? atlas;
    [AppliedWithChunk<Chunk0901102B>]
    public CPlugBitmapAtlas? Atlas { get => atlas; set => atlas = value; }

    private CPlugBitmapDecals? decals;
    [AppliedWithChunk<Chunk0901102C>]
    public CPlugBitmapDecals? Decals { get => decals; set => decals = value; }

    private float heightInMeters;
    [AppliedWithChunk<Chunk09011033>]
    public float HeightInMeters { get => heightInMeters; set => heightInMeters = value; }

    private CPlugImageArray? imageArray;
    [AppliedWithChunk<Chunk09011034>]
    public CPlugImageArray? ImageArray { get => imageArray; set => imageArray = value; }

    private string? imageArraySuffix;
    [AppliedWithChunk<Chunk09011034>]
    public string? ImageArraySuffix { get => imageArraySuffix; set => imageArraySuffix = value; }

    private string[]? imageArrayFids;
    [AppliedWithChunk<Chunk09011034>]
    public string[]? ImageArrayFids { get => imageArrayFids; set => imageArrayFids = value; }

    private CPlugBitmapArray? bitmapArray;
    [AppliedWithChunk<Chunk09011034>]
    public CPlugBitmapArray? BitmapArray { get => bitmapArray; set => bitmapArray = value; }

    private string? bitmapArrayElemName;
    [AppliedWithChunk<Chunk09011034>]
    public string? BitmapArrayElemName { get => bitmapArrayElemName; set => bitmapArrayElemName = value; }


    /// <summary>
    /// CPlugBitmap 0x014 chunk
    /// </summary>
    [Chunk(0x09011014)]
    public partial class Chunk09011014 : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09011014;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugFileImg>(ref n.image, ref n.imageFile);
            rw.UInt64(ref n.flags);
            rw.Single(ref n.mipMapLowerAlpha);
            rw.Single(ref n.bumpScaleFactor);
            rw.Single(ref n.mipMapLodBiasDefault);
            rw.Int32(ref n.borderRGB);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x015 chunk
    /// </summary>
    [Chunk(0x09011015)]
    public partial class Chunk09011015 : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09011015;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugFileImg>(ref n.image, ref n.imageFile);
            rw.UInt64(ref n.flags);
            rw.Single(ref n.mipMapLowerAlpha);
            rw.Single(ref n.bumpScaleFactor);
            rw.Single(ref n.mipMapLodBiasDefault);
            rw.Int32(ref n.borderRGB);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x017 chunk
    /// </summary>
    [Chunk(0x09011017)]
    public partial class Chunk09011017 : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09011017;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.UInt32(ref n.flags2);
            rw.Vec2(ref n.defaultTexCoordScale);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x018 chunk
    /// </summary>
    [Chunk(0x09011018)]
    public partial class Chunk09011018 : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09011018;

        public CMwNod? U01;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugFileImg>(ref n.image, ref n.imageFile);
            rw.UInt64(ref n.flags);
            rw.Single(ref n.mipMapLowerAlpha);
            rw.Single(ref n.bumpScaleFactor);
            rw.Single(ref n.mipMapLodBiasDefault);
            rw.Int32(ref n.borderRGB);
            if (n.Image!=null)
            {
                rw.NodeRef<CMwNod>(ref U01);
            }
        }
    }

    /// <summary>
    /// CPlugBitmap 0x019 chunk
    /// </summary>
    [Chunk(0x09011019)]
    public partial class Chunk09011019 : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09011019;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.Single(ref n.bumpScaleMipLevel);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x01B chunk
    /// </summary>
    [Chunk(0x0901101B)]
    public partial class Chunk0901101B : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901101B;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.UInt32(ref n.flags2);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x01C chunk
    /// </summary>
    [Chunk(0x0901101C)]
    public partial class Chunk0901101C : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901101C;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
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
    public partial class Chunk0901101D : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901101D;

        public short U01;
        public short U02;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.Int16(ref U01);
            rw.Int16(ref U02);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x01E chunk
    /// </summary>
    [Chunk(0x0901101E)]
    public partial class Chunk0901101E : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901101E;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.Array<Vec2>(ref n.atlasCountUVs!);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x01F chunk
    /// </summary>
    [Chunk(0x0901101F)]
    public partial class Chunk0901101F : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901101F;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.UInt32(ref n.flags2);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x020 chunk
    /// </summary>
    [Chunk(0x09011020)]
    public partial class Chunk09011020 : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09011020;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.Array<float>(ref n.mipMapFadeAlphas!);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x021 chunk
    /// </summary>
    [Chunk(0x09011021)]
    public partial class Chunk09011021 : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09011021;

        public uint U01;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x022 chunk
    /// </summary>
    [Chunk(0x09011022)]
    public partial class Chunk09011022 : Chunk09011018
    {
        /// <inheritdoc />
        public override uint Id => 0x09011022;

    }

    /// <summary>
    /// CPlugBitmap 0x023 chunk
    /// </summary>
    [Chunk(0x09011023)]
    public partial class Chunk09011023 : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09011023;

        public uint U01;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x024 chunk
    /// </summary>
    [Chunk(0x09011024)]
    public partial class Chunk09011024 : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09011024;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.UInt32(ref n.flags2);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x025 chunk
    /// </summary>
    [Chunk(0x09011025)]
    public partial class Chunk09011025 : Chunk0901101C
    {
        /// <inheritdoc />
        public override uint Id => 0x09011025;

        public uint U01;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.UInt32(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x028 chunk
    /// </summary>
    [Chunk(0x09011028)]
    public partial class Chunk09011028 : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09011028;

        public Int2 U01;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.Int2(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x02A chunk
    /// </summary>
    [Chunk(0x0901102A)]
    public partial class Chunk0901102A : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901102A;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugSpriteParam>(ref n.spriteParam);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x02B chunk
    /// </summary>
    [Chunk(0x0901102B)]
    public partial class Chunk0901102B : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901102B;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmapAtlas>(ref n.atlas);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x02C chunk
    /// </summary>
    [Chunk(0x0901102C)]
    public partial class Chunk0901102C : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901102C;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmapDecals>(ref n.decals);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x02D chunk
    /// </summary>
    [Chunk(0x0901102D)]
    public partial class Chunk0901102D : Chunk<CPlugBitmap>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0901102D;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x02E chunk
    /// </summary>
    [Chunk(0x0901102E)]
    public partial class Chunk0901102E : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901102E;

        public uint U01;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x030 chunk
    /// </summary>
    [Chunk(0x09011030)]
    public partial class Chunk09011030 : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09011030;

    }

    /// <summary>
    /// CPlugBitmap 0x032 chunk
    /// </summary>
    [Chunk(0x09011032)]
    public partial class Chunk09011032 : Chunk<CPlugBitmap>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09011032;

        public int Version { get; set; }

        public uint U01;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.UInt32(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x033 chunk
    /// </summary>
    [Chunk(0x09011033)]
    public partial class Chunk09011033 : Chunk<CPlugBitmap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09011033;


        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.Single(ref n.heightInMeters);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x034 chunk
    /// </summary>
    [Chunk(0x09011034)]
    public partial class Chunk09011034 : Chunk<CPlugBitmap>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09011034;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugImageArray>(ref n.imageArray);
            if (Version >= 1)
            {
                rw.String(ref n.imageArraySuffix);
                if (Version >= 2)
                {
                    rw.ArrayString(ref n.imageArrayFids!);
                    if (Version >= 3)
                    {
                        rw.NodeRef<CPlugBitmapArray>(ref n.bitmapArray);
                        rw.String(ref n.bitmapArrayElemName);
                        if (Version == 4)
                        {
                            rw.Int32(ref U01);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugBitmap 0x035 chunk
    /// </summary>
    [Chunk(0x09011035)]
    public partial class Chunk09011035 : Chunk<CPlugBitmap>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09011035;

        public int Version { get; set; }

        public short U01;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int16(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x036 chunk
    /// </summary>
    [Chunk(0x09011036)]
    public partial class Chunk09011036 : Chunk<CPlugBitmap>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09011036;

        public int Version { get; set; }

        public CMwNod? U01;
        public string? U02;
        public Vec2 U03;
        public Vec2 U04;
        public int U05;
        public CMwNod? U06;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CMwNod>(ref U01);
            if (Version == 0)
            {
                rw.Id(ref U02);
            }
            if (Version >= 1)
            {
                if (U01==null)
                {
                    rw.Id(ref U02);
                }
                if (U01!=null)
                {
                    rw.Vec2(ref U03);
                    rw.Vec2(ref U04);
                    rw.Int32(ref U05);
                }
            }
            rw.NodeRef<CMwNod>(ref U06);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x037 chunk
    /// </summary>
    [Chunk(0x09011037)]
    public partial class Chunk09011037 : Chunk<CPlugBitmap>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09011037;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;
        public int U07;
        public int U08;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);
            rw.Int32(ref U08);
        }
    }

    /// <summary>
    /// CPlugBitmap 0x038 chunk
    /// </summary>
    [Chunk(0x09011038)]
    public partial class Chunk09011038 : Chunk<CPlugBitmap>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09011038;

        public int Version { get; set; }

        public CPlugBitmapAtlas? U01;
        public int U02;

        public override void ReadWrite(CPlugBitmap n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugBitmapAtlas>(ref U01);
            rw.Int32(ref U02);
        }
    }


    public sealed partial class SpecularSubMapCat : IReadableWritable
    {

        private float u01;
        public float U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Single(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
        }
    }


    public enum EGxTexFilter
    {
        Point,
        Bilinear,
        Trilinear,
        Anisotropic,
    }

    public enum ECubeFace
    {
        None,
        XPos,
        XNeg,
        YPos,
        YNeg,
        ZPos,
        ZNeg,
    }

    public enum EGxUVGenerate
    {
        NoGenerate,
        CameraVertex,
        WorldVertex,
        WorldVertexXY,
        WorldVertexXZ,
        WorldVertexYZ,
        CameraNormal,
        WorldNormal,
        CameraReflectionVector,
        WorldReflectionVector,
        WorldNormalNeg,
        WaterReflectionVector,
        Hack1Vertex,
        MapTexel,
        FogPlane0,
        Vsk3SeaFoam,
        ImageSpace,
        LightDir0Reflect,
        EyeNormal,
        ShadowB1Pw01,
        Tex3AsPosPrCamera,
        FlatWaterReflect,
        FlatWaterRefract,
        FlatWaterFresnel,
    }

    public enum EUsage
    {
        Color,
        Light,
        Height2DuDv,
        Render,
        Height2DuDvLumi,
        Height2NxNyNz,
        Height2NxNy,
        Depth,
        DispH01,
        Height2NormPal8b,
        NxNyNz,
        NxNy,
        NormPal8b,
        NormPal16b,
        ColorFloat,
        RenderFloat,
        Height2DuDv1,
        Alpha,
        LightAlpha,
        Height2RxG0BzAy,
        NormRxG0BzAy,
        TexCoord,
        Render16b,
        Vertex,
        Height2BumpTxTy,
        BumpTxTy,
        Height2R0GyBzAx,
        NormR0GyBzAx,
        NormXYZ0YZX,
    }

    public enum EColorDepth
    {
        DefaultColorDepth,
        Color16b,
        Color32b,
    }

    public enum EGxTexAddress
    {
        Wrap,
        Mirror,
        Clamp,
        BorderSM3,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09011014 => new Chunk09011014(),
        0x09011015 => new Chunk09011015(),
        0x09011017 => new Chunk09011017(),
        0x09011018 => new Chunk09011018(),
        0x09011019 => new Chunk09011019(),
        0x0901101B => new Chunk0901101B(),
        0x0901101C => new Chunk0901101C(),
        0x0901101D => new Chunk0901101D(),
        0x0901101E => new Chunk0901101E(),
        0x0901101F => new Chunk0901101F(),
        0x09011020 => new Chunk09011020(),
        0x09011021 => new Chunk09011021(),
        0x09011022 => new Chunk09011022(),
        0x09011023 => new Chunk09011023(),
        0x09011024 => new Chunk09011024(),
        0x09011025 => new Chunk09011025(),
        0x09011028 => new Chunk09011028(),
        0x0901102A => new Chunk0901102A(),
        0x0901102B => new Chunk0901102B(),
        0x0901102C => new Chunk0901102C(),
        0x0901102D => new Chunk0901102D(),
        0x0901102E => new Chunk0901102E(),
        0x09011030 => new Chunk09011030(),
        0x09011032 => new Chunk09011032(),
        0x09011033 => new Chunk09011033(),
        0x09011034 => new Chunk09011034(),
        0x09011035 => new Chunk09011035(),
        0x09011036 => new Chunk09011036(),
        0x09011037 => new Chunk09011037(),
        0x09011038 => new Chunk09011038(),
        _ => base.NewChunk(chunkId),
    };
}
