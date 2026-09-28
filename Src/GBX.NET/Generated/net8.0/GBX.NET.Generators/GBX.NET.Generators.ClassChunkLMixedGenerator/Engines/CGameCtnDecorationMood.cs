namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0303A000</remarks>
[Class(0x0303A000)]
public partial class CGameCtnDecorationMood : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0303A000;




    private float latitude;
    [AppliedWithChunk<Chunk0303A000>]
    public float Latitude { get => latitude; set => latitude = value; }

    private float longitude;
    [AppliedWithChunk<Chunk0303A000>]
    public float Longitude { get => longitude; set => longitude = value; }

    private float deltaGMT;
    [AppliedWithChunk<Chunk0303A000>]
    public float DeltaGMT { get => deltaGMT; set => deltaGMT = value; }

    private TimeInt32 timeSunRise;
    [AppliedWithChunk<Chunk0303A000>]
    public TimeInt32 TimeSunRise { get => timeSunRise; set => timeSunRise = value; }

    private TimeInt32 timeSunFall;
    [AppliedWithChunk<Chunk0303A000>]
    public TimeInt32 TimeSunFall { get => timeSunFall; set => timeSunFall = value; }

    private float remappedStartDayTime;
    [AppliedWithChunk<Chunk0303A001>]
    [AppliedWithChunk<Chunk0303A012>]
    public float RemappedStartDayTime { get => remappedStartDayTime; set => remappedStartDayTime = value; }

    private CPlugGameSkin? remapping;
    [AppliedWithChunk<Chunk0303A001>]
    [AppliedWithChunk<Chunk0303A012>]
    public CPlugGameSkin? Remapping { get => remappingFile?.GetNode(ref remapping) ?? remapping; set => remapping = value; }
    private Components.GbxRefTableFile? remappingFile;
    public Components.GbxRefTableFile? RemappingFile { get => remappingFile; set => remappingFile = value; }
    public CPlugGameSkin? GetRemapping(GbxReadSettings settings = default, bool exceptions = false) => remappingFile?.GetNode(ref remapping, settings, exceptions) ?? remapping;

    private string? remapFolder;
    [AppliedWithChunk<Chunk0303A001>]
    [AppliedWithChunk<Chunk0303A012>]
    public string? RemapFolder { get => remapFolder; set => remapFolder = value; }

    private int shadowCountCarHuman;
    [AppliedWithChunk<Chunk0303A002>]
    public int ShadowCountCarHuman { get => shadowCountCarHuman; set => shadowCountCarHuman = value; }

    private int shadowCountCarOpponent;
    [AppliedWithChunk<Chunk0303A002>]
    public int ShadowCountCarOpponent { get => shadowCountCarOpponent; set => shadowCountCarOpponent = value; }

    private float shadowCarIntensity;
    [AppliedWithChunk<Chunk0303A002>]
    public float ShadowCarIntensity { get => shadowCarIntensity; set => shadowCarIntensity = value; }

    private bool shadowScene;
    [AppliedWithChunk<Chunk0303A002>]
    public bool ShadowScene { get => shadowScene; set => shadowScene = value; }

    private bool backgroundIsLocallyLighted;
    [AppliedWithChunk<Chunk0303A002>]
    public bool BackgroundIsLocallyLighted { get => backgroundIsLocallyLighted; set => backgroundIsLocallyLighted = value; }

    private bool solidLightAreSkinned;
    [AppliedWithChunk<Chunk0303A003>]
    public bool SolidLightAreSkinned { get => solidLightAreSkinned; set => solidLightAreSkinned = value; }

    private CHmsLightMap? hmsLightMap;
    [AppliedWithChunk<Chunk0303A004>]
    public CHmsLightMap? HmsLightMap { get => hmsLightMapFile?.GetNode(ref hmsLightMap) ?? hmsLightMap; set => hmsLightMap = value; }
    private Components.GbxRefTableFile? hmsLightMapFile;
    public Components.GbxRefTableFile? HmsLightMapFile { get => hmsLightMapFile; set => hmsLightMapFile = value; }
    public CHmsLightMap? GetHmsLightMap(GbxReadSettings settings = default, bool exceptions = false) => hmsLightMapFile?.GetNode(ref hmsLightMap, settings, exceptions) ?? hmsLightMap;

    private CHmsAmbientOcc? hmsAmbientOcc;
    [AppliedWithChunk<Chunk0303A005>]
    public CHmsAmbientOcc? HmsAmbientOcc { get => hmsAmbientOccFile?.GetNode(ref hmsAmbientOcc) ?? hmsAmbientOcc; set => hmsAmbientOcc = value; }
    private Components.GbxRefTableFile? hmsAmbientOccFile;
    public Components.GbxRefTableFile? HmsAmbientOccFile { get => hmsAmbientOccFile; set => hmsAmbientOccFile = value; }
    public CHmsAmbientOcc? GetHmsAmbientOcc(GbxReadSettings settings = default, bool exceptions = false) => hmsAmbientOccFile?.GetNode(ref hmsAmbientOcc, settings, exceptions) ?? hmsAmbientOcc;

    private float sunMoonIntensity;
    [AppliedWithChunk<Chunk0303A006>]
    [AppliedWithChunk<Chunk0303A007>]
    public float SunMoonIntensity { get => sunMoonIntensity; set => sunMoonIntensity = value; }

    private float localLightScale;
    [AppliedWithChunk<Chunk0303A007>]
    public float LocalLightScale { get => localLightScale; set => localLightScale = value; }

    private float toneMapExposureStaticBase;
    [AppliedWithChunk<Chunk0303A00C>]
    public float ToneMapExposureStaticBase { get => toneMapExposureStaticBase; set => toneMapExposureStaticBase = value; }

    private int toneMapFilmCurve;
    [AppliedWithChunk<Chunk0303A00C>]
    public int ToneMapFilmCurve { get => toneMapFilmCurve; set => toneMapFilmCurve = value; }

    private CFuncKeysReal? toneMapAutoExp_FidAvgLumiToKeyValue;
    [AppliedWithChunk<Chunk0303A00C>]
    public CFuncKeysReal? ToneMapAutoExp_FidAvgLumiToKeyValue { get => toneMapAutoExp_FidAvgLumiToKeyValue; set => toneMapAutoExp_FidAvgLumiToKeyValue = value; }

    private Vec3 tech3SpecularFake_ExpScaleMax;
    [AppliedWithChunk<Chunk0303A00F>]
    public Vec3 Tech3SpecularFake_ExpScaleMax { get => tech3SpecularFake_ExpScaleMax; set => tech3SpecularFake_ExpScaleMax = value; }

    private Vec2 tech3SpecularLocal;
    [AppliedWithChunk<Chunk0303A00F>]
    public Vec2 Tech3SpecularLocal { get => tech3SpecularLocal; set => tech3SpecularLocal = value; }

    private Vec3 tech3Bloom;
    [AppliedWithChunk<Chunk0303A00F>]
    public Vec3 Tech3Bloom { get => tech3Bloom; set => tech3Bloom = value; }

    private Vec4 tech3ToneMapAutoExp;
    [AppliedWithChunk<Chunk0303A00F>]
    public Vec4 Tech3ToneMapAutoExp { get => tech3ToneMapAutoExp; set => tech3ToneMapAutoExp = value; }

    private CFuncKeysReal? fxBloom_FidFuncIntensAtHdrNorm;
    [AppliedWithChunk<Chunk0303A00F>]
    public CFuncKeysReal? FxBloom_FidFuncIntensAtHdrNorm { get => fxBloom_FidFuncIntensAtHdrNorm; set => fxBloom_FidFuncIntensAtHdrNorm = value; }

    private bool waterReflectFakeCube;
    [AppliedWithChunk<Chunk0303A00F>]
    public bool WaterReflectFakeCube { get => waterReflectFakeCube; set => waterReflectFakeCube = value; }

    private CPlugFxHdrScales_Tech3? fxHdrScalesT3;
    [AppliedWithChunk<Chunk0303A00F>]
    public CPlugFxHdrScales_Tech3? FxHdrScalesT3 { get => fxHdrScalesT3File?.GetNode(ref fxHdrScalesT3) ?? fxHdrScalesT3; set => fxHdrScalesT3 = value; }
    private Components.GbxRefTableFile? fxHdrScalesT3File;
    public Components.GbxRefTableFile? FxHdrScalesT3File { get => fxHdrScalesT3File; set => fxHdrScalesT3File = value; }
    public CPlugFxHdrScales_Tech3? GetFxHdrScalesT3(GbxReadSettings settings = default, bool exceptions = false) => fxHdrScalesT3File?.GetNode(ref fxHdrScalesT3, settings, exceptions) ?? fxHdrScalesT3;

    private CPlugMoodBlender? moodBlender;
    [AppliedWithChunk<Chunk0303A00F>]
    public CPlugMoodBlender? MoodBlender { get => moodBlenderFile?.GetNode(ref moodBlender) ?? moodBlender; set => moodBlender = value; }
    private Components.GbxRefTableFile? moodBlenderFile;
    public Components.GbxRefTableFile? MoodBlenderFile { get => moodBlenderFile; set => moodBlenderFile = value; }
    public CPlugMoodBlender? GetMoodBlender(GbxReadSettings settings = default, bool exceptions = false) => moodBlenderFile?.GetNode(ref moodBlender, settings, exceptions) ?? moodBlender;

    private bool enableStars;
    [AppliedWithChunk<Chunk0303A012>]
    public bool EnableStars { get => enableStars; set => enableStars = value; }

    private CFuncCloudsSolids? cloudsSolids;
    [AppliedWithChunk<Chunk0303A012>]
    public CFuncCloudsSolids? CloudsSolids { get => cloudsSolidsFile?.GetNode(ref cloudsSolids) ?? cloudsSolids; set => cloudsSolids = value; }
    private Components.GbxRefTableFile? cloudsSolidsFile;
    public Components.GbxRefTableFile? CloudsSolidsFile { get => cloudsSolidsFile; set => cloudsSolidsFile = value; }
    public CFuncCloudsSolids? GetCloudsSolids(GbxReadSettings settings = default, bool exceptions = false) => cloudsSolidsFile?.GetNode(ref cloudsSolids, settings, exceptions) ?? cloudsSolids;

    private CPlugFxLightning? fxLightning;
    [AppliedWithChunk<Chunk0303A012>]
    public CPlugFxLightning? FxLightning { get => fxLightningFile?.GetNode(ref fxLightning) ?? fxLightning; set => fxLightning = value; }
    private Components.GbxRefTableFile? fxLightningFile;
    public Components.GbxRefTableFile? FxLightningFile { get => fxLightningFile; set => fxLightningFile = value; }
    public CPlugFxLightning? GetFxLightning(GbxReadSettings settings = default, bool exceptions = false) => fxLightningFile?.GetNode(ref fxLightning, settings, exceptions) ?? fxLightning;

    private CPlugFxWindOnDecal? fxWindOnDecal;
    [AppliedWithChunk<Chunk0303A012>]
    public CPlugFxWindOnDecal? FxWindOnDecal { get => fxWindOnDecalFile?.GetNode(ref fxWindOnDecal) ?? fxWindOnDecal; set => fxWindOnDecal = value; }
    private Components.GbxRefTableFile? fxWindOnDecalFile;
    public Components.GbxRefTableFile? FxWindOnDecalFile { get => fxWindOnDecalFile; set => fxWindOnDecalFile = value; }
    public CPlugFxWindOnDecal? GetFxWindOnDecal(GbxReadSettings settings = default, bool exceptions = false) => fxWindOnDecalFile?.GetNode(ref fxWindOnDecal, settings, exceptions) ?? fxWindOnDecal;

    private CPlugFxWindOnTreeSprite? fxWindOnTreeSprite;
    [AppliedWithChunk<Chunk0303A012>]
    public CPlugFxWindOnTreeSprite? FxWindOnTreeSprite { get => fxWindOnTreeSpriteFile?.GetNode(ref fxWindOnTreeSprite) ?? fxWindOnTreeSprite; set => fxWindOnTreeSprite = value; }
    private Components.GbxRefTableFile? fxWindOnTreeSpriteFile;
    public Components.GbxRefTableFile? FxWindOnTreeSpriteFile { get => fxWindOnTreeSpriteFile; set => fxWindOnTreeSpriteFile = value; }
    public CPlugFxWindOnTreeSprite? GetFxWindOnTreeSprite(GbxReadSettings settings = default, bool exceptions = false) => fxWindOnTreeSpriteFile?.GetNode(ref fxWindOnTreeSprite, settings, exceptions) ?? fxWindOnTreeSprite;

    private float editorHelperHdrScale;
    [AppliedWithChunk<Chunk0303A013>]
    public float EditorHelperHdrScale { get => editorHelperHdrScale; set => editorHelperHdrScale = value; }


    /// <summary>
    /// CGameCtnDecorationMood 0x000 chunk
    /// </summary>
    [Chunk(0x0303A000)]
    public partial class Chunk0303A000 : Chunk<CGameCtnDecorationMood>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303A000;


        public override void ReadWrite(CGameCtnDecorationMood n, GbxReaderWriter rw)
        {
            rw.Single(ref n.latitude);
            rw.Single(ref n.longitude);
            rw.Single(ref n.deltaGMT);
            rw.TimeInt32(ref n.timeSunRise);
            rw.TimeInt32(ref n.timeSunFall);
        }
    }

    /// <summary>
    /// CGameCtnDecorationMood 0x001 chunk
    /// </summary>
    [Chunk(0x0303A001)]
    public partial class Chunk0303A001 : Chunk<CGameCtnDecorationMood>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303A001;


        public override void ReadWrite(CGameCtnDecorationMood n, GbxReaderWriter rw)
        {
            rw.Single(ref n.remappedStartDayTime);
            rw.NodeRef<CPlugGameSkin>(ref n.remapping, ref n.remappingFile);
            rw.String(ref n.remapFolder);
        }
    }

    /// <summary>
    /// CGameCtnDecorationMood 0x002 chunk
    /// </summary>
    [Chunk(0x0303A002)]
    public partial class Chunk0303A002 : Chunk<CGameCtnDecorationMood>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303A002;


        public override void ReadWrite(CGameCtnDecorationMood n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.shadowCountCarHuman);
            rw.Int32(ref n.shadowCountCarOpponent);
            rw.Single(ref n.shadowCarIntensity);
            rw.Boolean(ref n.shadowScene);
            rw.Boolean(ref n.backgroundIsLocallyLighted);
        }
    }

    /// <summary>
    /// CGameCtnDecorationMood 0x003 chunk
    /// </summary>
    [Chunk(0x0303A003)]
    public partial class Chunk0303A003 : Chunk<CGameCtnDecorationMood>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303A003;


        public override void ReadWrite(CGameCtnDecorationMood n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.solidLightAreSkinned);
        }
    }

    /// <summary>
    /// CGameCtnDecorationMood 0x004 chunk
    /// </summary>
    [Chunk(0x0303A004)]
    public partial class Chunk0303A004 : Chunk<CGameCtnDecorationMood>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303A004;


        public override void ReadWrite(CGameCtnDecorationMood n, GbxReaderWriter rw)
        {
            rw.NodeRef<CHmsLightMap>(ref n.hmsLightMap, ref n.hmsLightMapFile);
        }
    }

    /// <summary>
    /// CGameCtnDecorationMood 0x005 chunk
    /// </summary>
    [Chunk(0x0303A005)]
    public partial class Chunk0303A005 : Chunk<CGameCtnDecorationMood>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303A005;


        public override void ReadWrite(CGameCtnDecorationMood n, GbxReaderWriter rw)
        {
            rw.NodeRef<CHmsAmbientOcc>(ref n.hmsAmbientOcc, ref n.hmsAmbientOccFile);
        }
    }

    /// <summary>
    /// CGameCtnDecorationMood 0x006 chunk
    /// </summary>
    [Chunk(0x0303A006)]
    public partial class Chunk0303A006 : Chunk<CGameCtnDecorationMood>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303A006;


        public override void ReadWrite(CGameCtnDecorationMood n, GbxReaderWriter rw)
        {
            rw.Single(ref n.sunMoonIntensity);
        }
    }

    /// <summary>
    /// CGameCtnDecorationMood 0x007 chunk
    /// </summary>
    [Chunk(0x0303A007)]
    public partial class Chunk0303A007 : Chunk<CGameCtnDecorationMood>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303A007;


        public override void ReadWrite(CGameCtnDecorationMood n, GbxReaderWriter rw)
        {
            rw.Single(ref n.sunMoonIntensity);
            rw.Single(ref n.localLightScale);
        }
    }

    /// <summary>
    /// CGameCtnDecorationMood 0x00C chunk
    /// </summary>
    [Chunk(0x0303A00C)]
    public partial class Chunk0303A00C : Chunk<CGameCtnDecorationMood>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303A00C;

        public Vec3 U01;

        public override void ReadWrite(CGameCtnDecorationMood n, GbxReaderWriter rw)
        {
            rw.Vec3(ref U01);
            rw.Single(ref n.toneMapExposureStaticBase);
            rw.Int32(ref n.toneMapFilmCurve);
            rw.NodeRef<CFuncKeysReal>(ref n.toneMapAutoExp_FidAvgLumiToKeyValue);
        }
    }

    /// <summary>
    /// CGameCtnDecorationMood 0x00F chunk
    /// </summary>
    [Chunk(0x0303A00F)]
    public partial class Chunk0303A00F : Chunk<CGameCtnDecorationMood>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0303A00F;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnDecorationMood n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Vec3(ref n.tech3SpecularFake_ExpScaleMax);
            rw.Vec2(ref n.tech3SpecularLocal);
            rw.Vec3(ref n.tech3Bloom);
            rw.Vec4(ref n.tech3ToneMapAutoExp);
            rw.NodeRef<CFuncKeysReal>(ref n.fxBloom_FidFuncIntensAtHdrNorm);
            if (Version >= 1)
            {
                rw.Boolean(ref n.waterReflectFakeCube);
                if (Version >= 2)
                {
                    rw.NodeRef<CPlugFxHdrScales_Tech3>(ref n.fxHdrScalesT3, ref n.fxHdrScalesT3File);
                    if (Version >= 3)
                    {
                        rw.NodeRef<CPlugMoodBlender>(ref n.moodBlender, ref n.moodBlenderFile);
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnDecorationMood 0x012 chunk
    /// </summary>
    [Chunk(0x0303A012)]
    public partial class Chunk0303A012 : Chunk<CGameCtnDecorationMood>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0303A012;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnDecorationMood n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref n.remappedStartDayTime);
            rw.Boolean(ref n.isNight);
            rw.NodeRef<CPlugGameSkin>(ref n.remapping, ref n.remappingFile);
            rw.String(ref n.remapFolder);
            rw.Boolean(ref n.enableStars);
            rw.NodeRef<CFuncCloudsSolids>(ref n.cloudsSolids, ref n.cloudsSolidsFile);
            rw.NodeRef<CPlugFxLightning>(ref n.fxLightning, ref n.fxLightningFile);
            rw.NodeRef<CPlugFxWindOnDecal>(ref n.fxWindOnDecal, ref n.fxWindOnDecalFile);
            if (Version >= 1)
            {
                rw.NodeRef<CPlugFxWindOnTreeSprite>(ref n.fxWindOnTreeSprite, ref n.fxWindOnTreeSpriteFile);
            }
        }
    }

    /// <summary>
    /// CGameCtnDecorationMood 0x013 chunk
    /// </summary>
    [Chunk(0x0303A013)]
    public partial class Chunk0303A013 : Chunk<CGameCtnDecorationMood>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0303A013;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnDecorationMood n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref n.editorHelperHdrScale);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0303A000 => new Chunk0303A000(),
        0x0303A001 => new Chunk0303A001(),
        0x0303A002 => new Chunk0303A002(),
        0x0303A003 => new Chunk0303A003(),
        0x0303A004 => new Chunk0303A004(),
        0x0303A005 => new Chunk0303A005(),
        0x0303A006 => new Chunk0303A006(),
        0x0303A007 => new Chunk0303A007(),
        0x0303A00C => new Chunk0303A00C(),
        0x0303A00F => new Chunk0303A00F(),
        0x0303A012 => new Chunk0303A012(),
        0x0303A013 => new Chunk0303A013(),
        _ => base.NewChunk(chunkId),
    };
}
