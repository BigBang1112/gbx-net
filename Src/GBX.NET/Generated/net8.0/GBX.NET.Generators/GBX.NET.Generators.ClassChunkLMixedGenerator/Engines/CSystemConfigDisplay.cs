namespace GBX.NET.Engines.System;

/// <remarks>ID: 0x0B013000</remarks>
[Class(0x0B013000)]
public partial class CSystemConfigDisplay : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0B013000;




    private Int2 screenSizeFS;
    [AppliedWithChunk<Chunk0B013001>]
    [AppliedWithChunk<Chunk0B013029>]
    [AppliedWithChunk<Chunk0B013036>]
    public Int2 ScreenSizeFS { get => screenSizeFS; set => screenSizeFS = value; }

    private int refreshRate;
    [AppliedWithChunk<Chunk0B013001>]
    [AppliedWithChunk<Chunk0B013029>]
    [AppliedWithChunk<Chunk0B013036>]
    public int RefreshRate { get => refreshRate; set => refreshRate = value; }

    private bool emulateCursorGDI;
    [AppliedWithChunk<Chunk0B013003>]
    [AppliedWithChunk<Chunk0B013026>]
    public bool EmulateCursorGDI { get => emulateCursorGDI; set => emulateCursorGDI = value; }

    private bool optimPartDynaGeom;
    [AppliedWithChunk<Chunk0B013003>]
    [AppliedWithChunk<Chunk0B013026>]
    public bool OptimPartDynaGeom { get => optimPartDynaGeom; set => optimPartDynaGeom = value; }

    private bool enableFullscreenGDI;
    [AppliedWithChunk<Chunk0B013005>]
    public bool EnableFullscreenGDI { get => enableFullscreenGDI; set => enableFullscreenGDI = value; }

    private int zClipNbBlock;
    [AppliedWithChunk<Chunk0B013008>]
    public int ZClipNbBlock { get => zClipNbBlock; set => zClipNbBlock = value; }

    private bool postFx;
    [AppliedWithChunk<Chunk0B013009>]
    public bool PostFx { get => postFx; set => postFx = value; }

    private bool disableZBufferRange;
    [AppliedWithChunk<Chunk0B01300B>]
    public bool DisableZBufferRange { get => disableZBufferRange; set => disableZBufferRange = value; }

    private float agpUseFactor;
    [AppliedWithChunk<Chunk0B01300B>]
    public float AgpUseFactor { get => agpUseFactor; set => agpUseFactor = value; }

    private bool customize;
    [AppliedWithChunk<Chunk0B01300D>]
    [AppliedWithChunk<Chunk0B013022>]
    [AppliedWithChunk<Chunk0B01302A>]
    public bool Customize { get => customize; set => customize = value; }

    private float geomLodScaleZ;
    [AppliedWithChunk<Chunk0B013010>]
    public float GeomLodScaleZ { get => geomLodScaleZ; set => geomLodScaleZ = value; }

    private bool disableWindowedAntiAlias;
    [AppliedWithChunk<Chunk0B013015>]
    public bool DisableWindowedAntiAlias { get => disableWindowedAntiAlias; set => disableWindowedAntiAlias = value; }

    private bool enableCheckLags;
    [AppliedWithChunk<Chunk0B013016>]
    public bool EnableCheckLags { get => enableCheckLags; set => enableCheckLags = value; }

    private bool multiThread;
    [AppliedWithChunk<Chunk0B01301C>]
    public bool MultiThread { get => multiThread; set => multiThread = value; }

    private int threadCountMax;
    [AppliedWithChunk<Chunk0B01301C>]
    public int ThreadCountMax { get => threadCountMax; set => threadCountMax = value; }

    private bool stereoByDefault;
    [AppliedWithChunk<Chunk0B013020>]
    public bool StereoByDefault { get => stereoByDefault; set => stereoByDefault = value; }

    private bool stereoAdvanced;
    [AppliedWithChunk<Chunk0B013020>]
    public bool StereoAdvanced { get => stereoAdvanced; set => stereoAdvanced = value; }

    private bool waterGeomStadium;
    [AppliedWithChunk<Chunk0B013021>]
    public bool WaterGeomStadium { get => waterGeomStadium; set => waterGeomStadium = value; }

    private EFilterAnisoQ filterAnisoQ;
    [AppliedWithChunk<Chunk0B013022>]
    [AppliedWithChunk<Chunk0B01302A>]
    public EFilterAnisoQ FilterAnisoQ { get => filterAnisoQ; set => filterAnisoQ = value; }

    private EFxBloomHdr fxBloomHdr;
    [AppliedWithChunk<Chunk0B013025>]
    public EFxBloomHdr FxBloomHdr { get => fxBloomHdr; set => fxBloomHdr = value; }

    private EGpuSync gpuSync0;
    [AppliedWithChunk<Chunk0B013026>]
    public EGpuSync GpuSync0 { get => gpuSync0; set => gpuSync0 = value; }

    private EDisplaySync displaySync;
    [AppliedWithChunk<Chunk0B013029>]
    [AppliedWithChunk<Chunk0B013036>]
    public EDisplaySync DisplaySync { get => displaySync; set => displaySync = value; }

    private EShaderQ shaderQuality;
    [AppliedWithChunk<Chunk0B01302A>]
    public EShaderQ ShaderQuality { get => shaderQuality; set => shaderQuality = value; }

    private EVehicleReflect vehicleReflect;
    [AppliedWithChunk<Chunk0B01302C>]
    public EVehicleReflect VehicleReflect { get => vehicleReflect; set => vehicleReflect = value; }

    private EFxMotionBlur fxMotionBlur;
    [AppliedWithChunk<Chunk0B01302D>]
    public EFxMotionBlur FxMotionBlur { get => fxMotionBlur; set => fxMotionBlur = value; }

    private int maxFps;
    [AppliedWithChunk<Chunk0B01302F>]
    public int MaxFps { get => maxFps; set => maxFps = value; }

    private ELightMapQuality lM_Quality;
    [AppliedWithChunk<Chunk0B013030>]
    public ELightMapQuality LM_Quality { get => lM_Quality; set => lM_Quality = value; }

    private bool lM_QUltra;
    [AppliedWithChunk<Chunk0B013030>]
    public bool LM_QUltra { get => lM_QUltra; set => lM_QUltra = value; }

    private bool lM_iLight;
    [AppliedWithChunk<Chunk0B013030>]
    public bool LM_iLight { get => lM_iLight; set => lM_iLight = value; }

    private bool decals_3D__TextureDecals;
    [AppliedWithChunk<Chunk0B013031>]
    public bool Decals_3D__TextureDecals { get => decals_3D__TextureDecals; set => decals_3D__TextureDecals = value; }

    private bool decals_2D__TextureDecals;
    [AppliedWithChunk<Chunk0B013031>]
    public bool Decals_2D__TextureDecals { get => decals_2D__TextureDecals; set => decals_2D__TextureDecals = value; }

    private bool disableHdrCubeRenderMipMap;
    [AppliedWithChunk<Chunk0B013033>]
    public bool DisableHdrCubeRenderMipMap { get => disableHdrCubeRenderMipMap; set => disableHdrCubeRenderMipMap = value; }

    private float fxMotionBlurIntens;
    [AppliedWithChunk<Chunk0B013035>]
    public float FxMotionBlurIntens { get => fxMotionBlurIntens; set => fxMotionBlurIntens = value; }

    private EDisplayMode displayMode;
    [AppliedWithChunk<Chunk0B013036>]
    public EDisplayMode DisplayMode { get => displayMode; set => displayMode = value; }

    private string? adapter;
    [AppliedWithChunk<Chunk0B013036>]
    public string? Adapter { get => adapter; set => adapter = value; }

    private EScreenShotExt screenShotExt;
    [AppliedWithChunk<Chunk0B013039>]
    public EScreenShotExt ScreenShotExt { get => screenShotExt; set => screenShotExt = value; }

    private float particleMaxGpuLoadMs;
    [AppliedWithChunk<Chunk0B01303B>]
    public float ParticleMaxGpuLoadMs { get => particleMaxGpuLoadMs; set => particleMaxGpuLoadMs = value; }

    private bool asyncRender;
    [AppliedWithChunk<Chunk0B01303D>]
    public bool AsyncRender { get => asyncRender; set => asyncRender = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CSystemConfigDisplay"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSystemConfigDisplay() { }


    /// <summary>
    /// CSystemConfigDisplay 0x001 skippable chunk
    /// </summary>
    [Chunk(0x0B013001)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF)]
    public partial class Chunk0B013001 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF;

        public int U01;
        public int U02;
        public int U03;
        /// <summary>
        /// DisplaySync and DisplayMode
        /// </summary>
        public bool U04;
        public bool U05;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int2(ref n.screenSizeFS);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref n.refreshRate);
            rw.Boolean(ref U04); // DisplaySync and DisplayMode
            rw.Boolean(ref U05);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x003 skippable chunk
    /// </summary>
    [Chunk(0x0B013003)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF)]
    public partial class Chunk0B013003 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF;

        public bool U01;
        /// <summary>
        /// something with GpuSync0
        /// </summary>
        public int U02;
        public int U03;
        public bool U04;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Int32(ref U02); // something with GpuSync0
            rw.Boolean(ref n.emulateCursorGDI);
            rw.Int32(ref U03);
            rw.Boolean(ref n.optimPartDynaGeom);
            rw.Boolean(ref U04);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x004 skippable chunk
    /// </summary>
    [Chunk(0x0B013004)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013004 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        /// <summary>
        /// sometimes weird number?
        /// </summary>
        public bool U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01); // sometimes weird number?
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x005 skippable chunk
    /// </summary>
    [Chunk(0x0B013005)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013005 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013005;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.enableFullscreenGDI);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x008 skippable chunk
    /// </summary>
    [Chunk(0x0B013008)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013008 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013008;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public int U01;
        public int U02;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref n.zClipNbBlock);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x009 skippable chunk
    /// </summary>
    [Chunk(0x0B013009)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013009 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013009;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public int U01;
        public int U02;
        public bool U03;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Boolean(ref n.postFx);
            rw.Boolean(ref U03);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x00A skippable chunk
    /// </summary>
    [Chunk(0x0B01300A)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF)]
    public partial class Chunk0B01300A : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01300A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF;

        public bool U01;
        public bool U02;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x00B skippable chunk
    /// </summary>
    [Chunk(0x0B01300B)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B01300B : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01300B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.disableZBufferRange);
            rw.Single(ref n.agpUseFactor);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x00D skippable chunk
    /// </summary>
    [Chunk(0x0B01300D)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5)]
    public partial class Chunk0B01300D : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01300D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5;

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;
        public int U07;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Boolean(ref n.customize);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x00E skippable chunk
    /// </summary>
    [Chunk(0x0B01300E)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk0B01300E : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01300E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC;

        public bool U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x00F skippable chunk
    /// </summary>
    [Chunk(0x0B01300F)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B01300F : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01300F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public bool U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x010 skippable chunk
    /// </summary>
    [Chunk(0x0B013010)]
    [ChunkGameVersion(GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF)]
    public partial class Chunk0B013010 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013010;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Single(ref n.geomLodScaleZ);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x011 skippable chunk
    /// </summary>
    [Chunk(0x0B013011)]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF)]
    public partial class Chunk0B013011 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013011;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF;

        public bool U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x013 skippable chunk
    /// </summary>
    [Chunk(0x0B013013)]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF)]
    public partial class Chunk0B013013 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013013;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF;

        public int U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x015 skippable chunk
    /// </summary>
    [Chunk(0x0B013015)]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013015 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013015;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.disableWindowedAntiAlias);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x016 skippable chunk
    /// </summary>
    [Chunk(0x0B013016)]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013016 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013016;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.enableCheckLags);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x017 skippable chunk
    /// </summary>
    [Chunk(0x0B013017)]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013017 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013017;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        /// <summary>
        /// bool but weird number
        /// </summary>
        public int U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01); // bool but weird number
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x018 skippable chunk
    /// </summary>
    [Chunk(0x0B013018)]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013018 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013018;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public int U01 = 8;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x019 skippable chunk
    /// </summary>
    [Chunk(0x0B013019)]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013019 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013019;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        /// <summary>
        /// bool but weird number
        /// </summary>
        public int U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01); // bool but weird number
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x01A skippable chunk
    /// </summary>
    [Chunk(0x0B01301A)]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.VSK5)]
    public partial class Chunk0B01301A : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01301A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.VSK5;

        public bool U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x01B skippable chunk
    /// </summary>
    [Chunk(0x0B01301B)]
    [ChunkGameVersion(GameVersion.VSK5 | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B01301B : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01301B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.VSK5 | GameVersion.TMU | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public int U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x01C skippable chunk
    /// </summary>
    [Chunk(0x0B01301C)]
    [ChunkGameVersion(GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B01301C : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01301C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.multiThread);
            rw.Int32(ref n.threadCountMax);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x01D skippable chunk
    /// </summary>
    [Chunk(0x0B01301D)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk0B01301D : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01301D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

        public int U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x01E skippable chunk
    /// </summary>
    [Chunk(0x0B01301E)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk0B01301E : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01301E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

        public bool U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x020 skippable chunk
    /// </summary>
    [Chunk(0x0B013020)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013020 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013020;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.stereoByDefault);
            rw.Boolean(ref n.stereoAdvanced);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x021 skippable chunk
    /// </summary>
    [Chunk(0x0B013021)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013021 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013021;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.waterGeomStadium);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x022 skippable chunk
    /// </summary>
    [Chunk(0x0B013022)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk0B013022 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013022;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Boolean(ref n.customize);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.EnumInt32<EFilterAnisoQ>(ref n.filterAnisoQ);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x025 skippable chunk
    /// </summary>
    [Chunk(0x0B013025)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013025 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013025;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public int U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.EnumInt32<EFxBloomHdr>(ref n.fxBloomHdr);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x026 skippable chunk
    /// </summary>
    [Chunk(0x0B013026)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013026 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013026;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        /// <summary>
        /// bool but weird number
        /// </summary>
        public int U01;
        public int U02;
        /// <summary>
        /// bool but weird number
        /// </summary>
        public int U03;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01); // bool but weird number
            rw.EnumInt32<EGpuSync>(ref n.gpuSync0);
            rw.Boolean(ref n.emulateCursorGDI);
            rw.Int32(ref U02);
            rw.Boolean(ref n.optimPartDynaGeom);
            rw.Int32(ref U03); // bool but weird number
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x029 skippable chunk
    /// </summary>
    [Chunk(0x0B013029)]
    public partial class Chunk0B013029 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013029;

        public int U01;
        public int U02;
        public int U03;
        public bool U04;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int2(ref n.screenSizeFS);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref n.refreshRate);
            rw.EnumInt32<EDisplaySync>(ref n.displaySync);
            rw.Boolean(ref U04);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x02A skippable chunk
    /// </summary>
    [Chunk(0x0B01302A)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B01302A : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01302A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public int U01;
        public int U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Boolean(ref n.customize);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.EnumInt32<EShaderQ>(ref n.shaderQuality);
            rw.Int32(ref U04);
            rw.EnumInt32<EFilterAnisoQ>(ref n.filterAnisoQ);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x02C skippable chunk
    /// </summary>
    [Chunk(0x0B01302C)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B01302C : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01302C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EVehicleReflect>(ref n.vehicleReflect);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x02D skippable chunk
    /// </summary>
    [Chunk(0x0B01302D)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B01302D : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01302D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EFxMotionBlur>(ref n.fxMotionBlur);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x02E skippable chunk
    /// </summary>
    [Chunk(0x0B01302E)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B01302E : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01302E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public bool U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x02F skippable chunk
    /// </summary>
    [Chunk(0x0B01302F)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B01302F : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01302F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.maxFps);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x030 skippable chunk
    /// </summary>
    [Chunk(0x0B013030)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013030 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013030;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public int U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.EnumInt32<ELightMapQuality>(ref n.lM_Quality);
            rw.Boolean(ref n.lM_QUltra);
            rw.Boolean(ref n.lM_iLight);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x031 skippable chunk
    /// </summary>
    [Chunk(0x0B013031)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013031 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013031;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.decals_3D__TextureDecals);
            rw.Boolean(ref n.decals_2D__TextureDecals);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x032 skippable chunk
    /// </summary>
    [Chunk(0x0B013032)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013032 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013032;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public int U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x033 skippable chunk
    /// </summary>
    [Chunk(0x0B013033)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013033 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013033;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.disableHdrCubeRenderMipMap);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x035 skippable chunk
    /// </summary>
    [Chunk(0x0B013035)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013035 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013035;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Single(ref n.fxMotionBlurIntens);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x036 skippable chunk
    /// </summary>
    [Chunk(0x0B013036)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013036 : SkippableChunk<CSystemConfigDisplay>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013036;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;
        public int U07;
        public int U08;
        public int U09;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int2(ref n.screenSizeFS);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref n.refreshRate);
            rw.EnumInt32<EDisplaySync>(ref n.displaySync);
            rw.EnumInt32<EDisplayMode>(ref n.displayMode);
            rw.Int32(ref U04);
            if (Version >= 1)
            {
                rw.Int32(ref U05);
                if (Version >= 2)
                {
                    rw.Int32(ref U06);
                    rw.Int32(ref U07);
                    rw.Int32(ref U08);
                    rw.Int32(ref U09);
                    if (Version >= 3)
                    {
                        rw.String(ref n.adapter);
                    }
                }
            }
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x038 skippable chunk
    /// </summary>
    [Chunk(0x0B013038)]
    [ChunkGameVersion(GameVersion.MP3)]
    public partial class Chunk0B013038 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013038;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3;

        public bool U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x039 skippable chunk
    /// </summary>
    [Chunk(0x0B013039)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B013039 : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B013039;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EScreenShotExt>(ref n.screenShotExt);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x03A skippable chunk
    /// </summary>
    [Chunk(0x0B01303A)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B01303A : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01303A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public int U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x03B skippable chunk
    /// </summary>
    [Chunk(0x0B01303B)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B01303B : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01303B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Single(ref n.particleMaxGpuLoadMs);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x03C skippable chunk
    /// </summary>
    [Chunk(0x0B01303C)]
    [ChunkGameVersion(GameVersion.MP4)]
    public partial class Chunk0B01303C : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01303C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4;

        public bool U01;

        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfigDisplay 0x03D skippable chunk
    /// </summary>
    [Chunk(0x0B01303D)]
    [ChunkGameVersion(GameVersion.MP4)]
    public partial class Chunk0B01303D : SkippableChunk<CSystemConfigDisplay>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B01303D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4;


        public override void ReadWrite(CSystemConfigDisplay n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.asyncRender);
        }
    }



    public enum EGpuSync
    {
        None,
        _3Frames,
        _2Frames,
        _1Frame,
        Immediate,
    }

    public enum EVehicleReflect
    {
        Low,
        HighInReplay,
        High,
    }

    public enum ELightMapQuality
    {
        None,
        VeryFast,
        Fast,
        Default,
        High,
    }

    public enum EDisplaySync
    {
        DisplaySync_None,
        DisplaySync_1_Interval,
        DisplaySync_2_Intervals,
        DisplaySync_3_Intervals,
    }

    public enum EDisplayMode
    {
        DisplayMode_FullScreen,
        DisplayMode_Windowed,
        DisplayMode_WindowedFul,
    }

    public enum EFxMotionBlur
    {
        Off,
        On,
    }

    public enum EFilterAnisoQ
    {
        Bilinear,
        Trilinear,
        Anisotropic__2x,
        Anisotropic__4x,
        Anisotropic__8x,
        Anisotropic_16x,
        Aniso_16x_everywhere,
    }

    public enum EShaderQ
    {
        Very_Fast,
        Fast,
        Nice,
        Very_Nice,
    }

    public enum EFxBloomHdr
    {
        None,
        Medium,
        High,
    }

    public enum EScreenShotExt
    {
        JPG,
        WebP,
        TGA,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0B013001 => new Chunk0B013001(),
        0x0B013003 => new Chunk0B013003(),
        0x0B013004 => new Chunk0B013004(),
        0x0B013005 => new Chunk0B013005(),
        0x0B013008 => new Chunk0B013008(),
        0x0B013009 => new Chunk0B013009(),
        0x0B01300A => new Chunk0B01300A(),
        0x0B01300B => new Chunk0B01300B(),
        0x0B01300D => new Chunk0B01300D(),
        0x0B01300E => new Chunk0B01300E(),
        0x0B01300F => new Chunk0B01300F(),
        0x0B013010 => new Chunk0B013010(),
        0x0B013011 => new Chunk0B013011(),
        0x0B013013 => new Chunk0B013013(),
        0x0B013015 => new Chunk0B013015(),
        0x0B013016 => new Chunk0B013016(),
        0x0B013017 => new Chunk0B013017(),
        0x0B013018 => new Chunk0B013018(),
        0x0B013019 => new Chunk0B013019(),
        0x0B01301A => new Chunk0B01301A(),
        0x0B01301B => new Chunk0B01301B(),
        0x0B01301C => new Chunk0B01301C(),
        0x0B01301D => new Chunk0B01301D(),
        0x0B01301E => new Chunk0B01301E(),
        0x0B013020 => new Chunk0B013020(),
        0x0B013021 => new Chunk0B013021(),
        0x0B013022 => new Chunk0B013022(),
        0x0B013025 => new Chunk0B013025(),
        0x0B013026 => new Chunk0B013026(),
        0x0B013029 => new Chunk0B013029(),
        0x0B01302A => new Chunk0B01302A(),
        0x0B01302C => new Chunk0B01302C(),
        0x0B01302D => new Chunk0B01302D(),
        0x0B01302E => new Chunk0B01302E(),
        0x0B01302F => new Chunk0B01302F(),
        0x0B013030 => new Chunk0B013030(),
        0x0B013031 => new Chunk0B013031(),
        0x0B013032 => new Chunk0B013032(),
        0x0B013033 => new Chunk0B013033(),
        0x0B013035 => new Chunk0B013035(),
        0x0B013036 => new Chunk0B013036(),
        0x0B013038 => new Chunk0B013038(),
        0x0B013039 => new Chunk0B013039(),
        0x0B01303A => new Chunk0B01303A(),
        0x0B01303B => new Chunk0B01303B(),
        0x0B01303C => new Chunk0B01303C(),
        0x0B01303D => new Chunk0B01303D(),
        _ => base.NewChunk(chunkId),
    };
}
