namespace GBX.NET.Engines.Hms;

/// <remarks>ID: 0x06022000</remarks>
[Class(0x06022000)]
public partial class CHmsLightMapCache : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x06022000;




    private int[]? mapT3s;
    [AppliedWithChunk<Chunk0602200B>]
    public int[]? MapT3s { get => mapT3s; set => mapT3s = value; }

    private EQuality quality;
    [AppliedWithChunk<Chunk0602200F>]
    public EQuality Quality { get => quality; set => quality = value; }

    private DateTime? timeWrite;
    [AppliedWithChunk<Chunk06022013>]
    public DateTime? TimeWrite { get => timeWrite; set => timeWrite = value; }

    private ulong lightmapCacheUid;
    [AppliedWithChunk<Chunk06022015>]
    public ulong LightmapCacheUid { get => lightmapCacheUid; set => lightmapCacheUid = value; }

    private string? decoration;
    [AppliedWithChunk<Chunk06022015>]
    public string? Decoration { get => decoration; set => decoration = value; }

    private EVersion version;
    [AppliedWithChunk<Chunk06022016>]
    public EVersion Version { get => version; set => version = value; }

    private int decal2D;
    [AppliedWithChunk<Chunk06022017>]
    public int Decal2D { get => decal2D; set => decal2D = value; }

    private int decal3D;
    [AppliedWithChunk<Chunk06022017>]
    public int Decal3D { get => decal3D; set => decal3D = value; }

    private EQualityVer qualityVer;
    [AppliedWithChunk<Chunk06022019>]
    public EQualityVer QualityVer { get => qualityVer; set => qualityVer = value; }

    private SMap[]? maps;
    [AppliedWithChunk<Chunk0602201A>]
    public SMap[]? Maps { get => maps; set => maps = value; }

    private int ambSample;
    [AppliedWithChunk<Chunk0602201A>]
    public int AmbSample { get => ambSample; set => ambSample = value; }

    private int dirSamples;
    [AppliedWithChunk<Chunk0602201A>]
    public int DirSamples { get => dirSamples; set => dirSamples = value; }

    private int pntSamples;
    [AppliedWithChunk<Chunk0602201A>]
    public int PntSamples { get => pntSamples; set => pntSamples = value; }

    private ESortMode sortMode;
    [AppliedWithChunk<Chunk0602201A>]
    public ESortMode SortMode { get => sortMode; set => sortMode = value; }

    private EAllocMode allocMode;
    [AppliedWithChunk<Chunk0602201A>]
    public EAllocMode AllocMode { get => allocMode; set => allocMode = value; }

    private ECompressMode compressMode;
    [AppliedWithChunk<Chunk0602201A>]
    public ECompressMode CompressMode { get => compressMode; set => compressMode = value; }

    private EBump bump;
    [AppliedWithChunk<Chunk0602201A>]
    public EBump Bump { get => bump; set => bump = value; }

    private SFrame[]? frames;
    [AppliedWithChunk<Chunk0602201A>]
    public SFrame[]? Frames { get => frames; set => frames = value; }

    private bool spriteOriginY_WasWronglyTop;
    [AppliedWithChunk<Chunk0602201A>]
    public bool SpriteOriginY_WasWronglyTop { get => spriteOriginY_WasWronglyTop; set => spriteOriginY_WasWronglyTop = value; }

    private SMapping? mapping;
    [AppliedWithChunk<Chunk0602201A>]
    public SMapping? Mapping { get => mapping; set => mapping = value; }

    private EPlugGpuPlatform gpuPlatform;
    [AppliedWithChunk<Chunk0602201A>]
    public EPlugGpuPlatform GpuPlatform { get => gpuPlatform; set => gpuPlatform = value; }

    private float allocatedTexelByMeter;
    [AppliedWithChunk<Chunk0602201A>]
    public float AllocatedTexelByMeter { get => allocatedTexelByMeter; set => allocatedTexelByMeter = value; }


    /// <summary>
    /// CHmsLightMapCache 0x00B skippable chunk
    /// </summary>
    [Chunk(0x0602200B)]
    public partial class Chunk0602200B : SkippableChunk<CHmsLightMapCache>
    {
        /// <inheritdoc />
        public override uint Id => 0x0602200B;


        public override void ReadWrite(CHmsLightMapCache n, GbxReaderWriter rw)
        {
            rw.Array<int>(ref n.mapT3s!);
        }
    }

    /// <summary>
    /// CHmsLightMapCache 0x00F skippable chunk
    /// </summary>
    [Chunk(0x0602200F)]
    public partial class Chunk0602200F : SkippableChunk<CHmsLightMapCache>
    {
        /// <inheritdoc />
        public override uint Id => 0x0602200F;

        public int U01;

        public override void ReadWrite(CHmsLightMapCache n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EQuality>(ref n.quality);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CHmsLightMapCache 0x013 skippable chunk
    /// </summary>
    [Chunk(0x06022013)]
    public partial class Chunk06022013 : SkippableChunk<CHmsLightMapCache>
    {
        /// <inheritdoc />
        public override uint Id => 0x06022013;

        public bool U01;
        public bool U02;

        public override void ReadWrite(CHmsLightMapCache n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.FileTime(ref n.timeWrite);
        }
    }

    /// <summary>
    /// CHmsLightMapCache 0x015 skippable chunk
    /// </summary>
    [Chunk(0x06022015)]
    public partial class Chunk06022015 : SkippableChunk<CHmsLightMapCache>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x06022015;

        public int Version { get; set; }

        public int U01;
        public bool U02;
        public int U03;
        public TimeSpan? U04;
        public int U05;
        public TimeSpan? U06;
        public string? U07;

        public override void ReadWrite(CHmsLightMapCache n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.UInt64(ref n.lightmapCacheUid);
            n.Collection = rw.Id(n.Collection);
            rw.Id(ref n.decoration);
            rw.Int32(ref U01);
            if (Version <= 3)
            {
                rw.Boolean(ref U02);
            }
            if (Version >= 2)
            {
                rw.Int32(ref U03);
                rw.TimeOfDay(ref U04);
                if (Version >= 3)
                {
                    rw.Int32(ref U05);
                    rw.TimeOfDay(ref U06);
                    if (Version >= 5)
                    {
                        rw.String(ref U07);
                    }
                }
            }
        }
    }

    /// <summary>
    /// CHmsLightMapCache 0x016 skippable chunk
    /// </summary>
    [Chunk(0x06022016)]
    public partial class Chunk06022016 : SkippableChunk<CHmsLightMapCache>
    {
        /// <inheritdoc />
        public override uint Id => 0x06022016;


        public override void ReadWrite(CHmsLightMapCache n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EVersion>(ref n.version);
        }
    }

    /// <summary>
    /// CHmsLightMapCache 0x017 skippable chunk
    /// </summary>
    [Chunk(0x06022017)]
    public partial class Chunk06022017 : SkippableChunk<CHmsLightMapCache>
    {
        /// <inheritdoc />
        public override uint Id => 0x06022017;


        public override void ReadWrite(CHmsLightMapCache n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.decal2D);
            rw.Int32(ref n.decal3D);
        }
    }

    /// <summary>
    /// CHmsLightMapCache 0x018 skippable chunk
    /// </summary>
    [Chunk(0x06022018)]
    public partial class Chunk06022018 : SkippableChunk<CHmsLightMapCache>
    {
        /// <inheritdoc />
        public override uint Id => 0x06022018;

        public ulong U01;

        public override void ReadWrite(CHmsLightMapCache n, GbxReaderWriter rw)
        {
            rw.UInt64(ref U01);
        }
    }

    /// <summary>
    /// CHmsLightMapCache 0x019 skippable chunk
    /// </summary>
    [Chunk(0x06022019)]
    public partial class Chunk06022019 : SkippableChunk<CHmsLightMapCache>
    {
        /// <inheritdoc />
        public override uint Id => 0x06022019;


        public override void ReadWrite(CHmsLightMapCache n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EQualityVer>(ref n.qualityVer);
        }
    }

    /// <summary>
    /// CHmsLightMapCache 0x01A skippable chunk
    /// </summary>
    [Chunk(0x0602201A)]
    public partial class Chunk0602201A : SkippableChunk<CHmsLightMapCache>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0602201A;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public bool U08;
        public Vec3 U09;
        public int U10;
        public int U11;
        public int U12;

        public override void ReadWrite(CHmsLightMapCache n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<SMap>(ref n.maps!);
            rw.Int32(ref n.ambSample);
            rw.Int32(ref n.dirSamples);
            rw.Int32(ref n.pntSamples);
            rw.EnumInt32<ESortMode>(ref n.sortMode);
            rw.EnumInt32<EAllocMode>(ref n.allocMode);
            rw.Int32(ref U01);
            rw.EnumInt32<ECompressMode>(ref n.compressMode);
            rw.Int32(ref U02);
            if (Version >= 6)
            {
                rw.EnumInt32<EBump>(ref n.bump);
            }
            if (Version <= 2)
            {
                rw.Single(ref U03);
                rw.Single(ref U04);
                rw.Single(ref U05);
                rw.Single(ref U06);
                rw.Single(ref U07);
            }
            if (Version >= 3)
            {
                rw.ArrayReadableWritable<SFrame>(ref n.frames!, version: Version);
            }
            rw.Boolean(ref U08);
            rw.Boolean(ref n.spriteOriginY_WasWronglyTop);
            rw.ReadableWritable<SMapping>(ref n.mapping);
            if (Version >= 1)
            {
                rw.EnumInt32<EPlugGpuPlatform>(ref n.gpuPlatform);
                if (Version == 2)
                {
                    rw.Vec3_6(ref U09);
                }
                if (Version >= 5)
                {
                    rw.Single(ref n.allocatedTexelByMeter);
                    if (Version >= 11)
                    {
                        rw.Int32(ref U10);
                        rw.Int32(ref U11);
                        if (Version >= 15)
                        {
                            rw.Int32(ref U12);
                        }
                    }
                }
            }
        }
    }


    public sealed partial class SFrame : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private TimeSpan? u02;
        public TimeSpan? U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        private float u07;
        public float U07 { get => u07; set => u07 = value; }

        private bool u08;
        public bool U08 { get => u08; set => u08 = value; }

        private Vec3 u09;
        public Vec3 U09 { get => u09; set => u09 = value; }

        private int u10;
        public int U10 { get => u10; set => u10 = value; }

        private int u11;
        public int U11 { get => u11; set => u11 = value; }

        private int u12 = 2;
        public int U12 { get => u12; set => u12 = value; }

        private float u13;
        public float U13 { get => u13; set => u13 = value; }

        private float u14;
        public float U14 { get => u14; set => u14 = value; }

        private float u15;
        public float U15 { get => u15; set => u15 = value; }

        private float u16;
        public float U16 { get => u16; set => u16 = value; }

        private float u17;
        public float U17 { get => u17; set => u17 = value; }

        private float u18;
        public float U18 { get => u18; set => u18 = value; }

        private float u19;
        public float U19 { get => u19; set => u19 = value; }

        private float u20;
        public float U20 { get => u20; set => u20 = value; }

        private float u21;
        public float U21 { get => u21; set => u21 = value; }

        private float u22;
        public float U22 { get => u22; set => u22 = value; }

        private int u23;
        public int U23 { get => u23; set => u23 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.TimeOfDay(ref u02);
            if (v >= 10)
            {
                rw.Single(ref u03);
            }
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Boolean(ref u08);
            rw.Vec3_6(ref u09);
            if (v >= 4)
            {
                rw.Int32(ref u10);
                rw.Int32(ref u11);
                if (v >= 12)
                {
                    rw.Int32(ref u12);
                }
                if (v >= 6)
                {
                    if (v <= 7)
                    {
                        rw.Single(ref u13);
                        rw.Single(ref u14);
                        rw.Single(ref u15);
                    }
                }
                if (v == 8)
                {
                    rw.Single(ref u16);
                    rw.Single(ref u17);
                    rw.Single(ref u18);
                    rw.Single(ref u19);
                }
                if (v >= 9)
                {
                    rw.Single(ref u20);
                    rw.Single(ref u21);
                    rw.Single(ref u22);
                }
                if (v >= 7)
                {
                    rw.Int32(ref u23);
                }
            }
        }
    }

    public sealed partial class SMapping : IReadableWritable
    {
    }

    public sealed partial class ProbeGridBox : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private int u07;
        public int U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        private int u09;
        public int U09 { get => u09; set => u09 = value; }

        private float u10;
        public float U10 { get => u10; set => u10 = value; }

        private float u11;
        public float U11 { get => u11; set => u11 = value; }

        private float u12;
        public float U12 { get => u12; set => u12 = value; }

        private float u13;
        public float U13 { get => u13; set => u13 = value; }

        private float u14;
        public float U14 { get => u14; set => u14 = value; }

        private float u15;
        public float U15 { get => u15; set => u15 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
            rw.Int32(ref u06);
            rw.Int32(ref u07);
            rw.Int32(ref u08);
            rw.Int32(ref u09);
            rw.Single(ref u10);
            rw.Single(ref u11);
            rw.Single(ref u12);
            rw.Single(ref u13);
            rw.Single(ref u14);
            rw.Single(ref u15);
        }
    }

    public sealed partial class ProbeGridBoxOld : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private int u07;
        public int U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        private int u09;
        public int U09 { get => u09; set => u09 = value; }

        private float u10;
        public float U10 { get => u10; set => u10 = value; }

        private float u11;
        public float U11 { get => u11; set => u11 = value; }

        private float u12;
        public float U12 { get => u12; set => u12 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
            rw.Int32(ref u06);
            rw.Int32(ref u07);
            rw.Int32(ref u08);
            rw.Int32(ref u09);
            rw.Single(ref u10);
            rw.Single(ref u11);
            rw.Single(ref u12);
        }
    }

    public sealed partial class SMap : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
        }
    }

    public sealed partial class Frame : IReadable, IWritable
    {

        public void Read(GbxReader r, int v = 0)
        {
            Data = r.ReadData();
            if (v >= 3)
            {
                Data2 = r.ReadData();
                if (v >= 6)
                {
                    Data3 = r.ReadData();
                }
            }
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.WriteData(Data);
            if (v >= 3)
            {
                w.WriteData(Data2);
                if (v >= 6)
                {
                    w.WriteData(Data3);
                }
            }
        }
    }


    public enum ESortMode
    {
        None,
        HDiagCenter,
    }

    public enum EVersion
    {
        Invalid,
        _2011_07_19_Beta1,
        _2011_07_21_Beta1,
        _2011_07_26_Beta1d,
        _2011_08_04_Beta2a,
        _2011_08_08_Beta3a,
        _2014_03_14_Update3_Storm,
        _2017_03_07_ManiaPlanet4,
        _2020_03_25_Beta1,
    }

    public enum EAllocMode
    {
        _64_2,
        _64_2PUseFree,
        BestSizePUseFree,
    }

    public enum EQualityVer
    {
        UltraMapperUnalignWith1k,
        BounceShadowFiltered,
        TinyAlloc_16b,
        ShadowCube_GeomToEyeLengthBias,
        BlockLight_WrongRotations,
        R11G11B10F_No_BounceFactor,
        HBasis_LQ_SignSqrt_BIntensScales,
        ProbeGrid_HdrScaleAmbient,
        ModelSplit2_GmPackReal2_V0,
        ShadowLQ,
        UnmappedBlock_FullCovering,
        StadiumColorisableBounces,
        Item_Prefab_MultiMesh,
        ShaderQLow_NeedTgtPixel,
        Current,
    }

    public enum EBump
    {
        TxTyTz,
        TxTyTz_Intens,
        None,
        HBasis_Color,
        HBasis_Intens,
    }

    public enum EQuality
    {
        VFast,
        Fast,
        Default,
        High,
        Ultra,
    }

    public enum EPlugGpuPlatform
    {
        _00,
        D3D11,
        pf3,
        pf4,
        pf5,
        pf6,
    }

    public enum ECompressMode
    {
        Ldr_DXT1,
        sRGB_Hyper_DXT1,
        Hyper_sRGB_DXT1,
        Scale_sRGB_DXT1,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0602200B => new Chunk0602200B(),
        0x0602200F => new Chunk0602200F(),
        0x06022013 => new Chunk06022013(),
        0x06022015 => new Chunk06022015(),
        0x06022016 => new Chunk06022016(),
        0x06022017 => new Chunk06022017(),
        0x06022018 => new Chunk06022018(),
        0x06022019 => new Chunk06022019(),
        0x0602201A => new Chunk0602201A(),
        _ => base.NewChunk(chunkId),
    };
}
