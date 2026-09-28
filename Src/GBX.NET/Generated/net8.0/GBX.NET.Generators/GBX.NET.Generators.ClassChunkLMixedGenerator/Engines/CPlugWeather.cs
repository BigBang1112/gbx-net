namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0917E000</remarks>
[Class(0x0917E000)]
public partial class CPlugWeather : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0917E000;




    private Vec2 lDirSpecIntens;
    [AppliedWithChunk<Chunk0917E00D>]
    public Vec2 LDirSpecIntens { get => lDirSpecIntens; set => lDirSpecIntens = value; }

    private Vec2 lDirSpecPower;
    [AppliedWithChunk<Chunk0917E00D>]
    public Vec2 LDirSpecPower { get => lDirSpecPower; set => lDirSpecPower = value; }

    private Vec3 seaTwkWaterColor_Night;
    [AppliedWithChunk<Chunk0917E00D>]
    public Vec3 SeaTwkWaterColor_Night { get => seaTwkWaterColor_Night; set => seaTwkWaterColor_Night = value; }

    private Vec3 seaTwkWaterColor_Day;
    [AppliedWithChunk<Chunk0917E00D>]
    public Vec3 SeaTwkWaterColor_Day { get => seaTwkWaterColor_Day; set => seaTwkWaterColor_Day = value; }

    private CPlugFileImg? imageLightAmb;
    [AppliedWithChunk<Chunk0917E00E>]
    public CPlugFileImg? ImageLightAmb { get => imageLightAmbFile?.GetNode(ref imageLightAmb) ?? imageLightAmb; set => imageLightAmb = value; }
    private Components.GbxRefTableFile? imageLightAmbFile;
    public Components.GbxRefTableFile? ImageLightAmbFile { get => imageLightAmbFile; set => imageLightAmbFile = value; }
    public CPlugFileImg? GetImageLightAmb(GbxReadSettings settings = default, bool exceptions = false) => imageLightAmbFile?.GetNode(ref imageLightAmb, settings, exceptions) ?? imageLightAmb;

    private CPlugFileImg? imageLightDirSun;
    [AppliedWithChunk<Chunk0917E00E>]
    public CPlugFileImg? ImageLightDirSun { get => imageLightDirSunFile?.GetNode(ref imageLightDirSun) ?? imageLightDirSun; set => imageLightDirSun = value; }
    private Components.GbxRefTableFile? imageLightDirSunFile;
    public Components.GbxRefTableFile? ImageLightDirSunFile { get => imageLightDirSunFile; set => imageLightDirSunFile = value; }
    public CPlugFileImg? GetImageLightDirSun(GbxReadSettings settings = default, bool exceptions = false) => imageLightDirSunFile?.GetNode(ref imageLightDirSun, settings, exceptions) ?? imageLightDirSun;

    private CPlugFileImg? imageLightDirMoon;
    [AppliedWithChunk<Chunk0917E00E>]
    public CPlugFileImg? ImageLightDirMoon { get => imageLightDirMoonFile?.GetNode(ref imageLightDirMoon) ?? imageLightDirMoon; set => imageLightDirMoon = value; }
    private Components.GbxRefTableFile? imageLightDirMoonFile;
    public Components.GbxRefTableFile? ImageLightDirMoonFile { get => imageLightDirMoonFile; set => imageLightDirMoonFile = value; }
    public CPlugFileImg? GetImageLightDirMoon(GbxReadSettings settings = default, bool exceptions = false) => imageLightDirMoonFile?.GetNode(ref imageLightDirMoon, settings, exceptions) ?? imageLightDirMoon;

    private CPlugFileImg? bitmapFlareSun;
    [AppliedWithChunk<Chunk0917E00E>]
    public CPlugFileImg? BitmapFlareSun { get => bitmapFlareSunFile?.GetNode(ref bitmapFlareSun) ?? bitmapFlareSun; set => bitmapFlareSun = value; }
    private Components.GbxRefTableFile? bitmapFlareSunFile;
    public Components.GbxRefTableFile? BitmapFlareSunFile { get => bitmapFlareSunFile; set => bitmapFlareSunFile = value; }
    public CPlugFileImg? GetBitmapFlareSun(GbxReadSettings settings = default, bool exceptions = false) => bitmapFlareSunFile?.GetNode(ref bitmapFlareSun, settings, exceptions) ?? bitmapFlareSun;

    private CPlugFileImg? bitmapFlareMoon;
    [AppliedWithChunk<Chunk0917E00E>]
    public CPlugFileImg? BitmapFlareMoon { get => bitmapFlareMoonFile?.GetNode(ref bitmapFlareMoon) ?? bitmapFlareMoon; set => bitmapFlareMoon = value; }
    private Components.GbxRefTableFile? bitmapFlareMoonFile;
    public Components.GbxRefTableFile? BitmapFlareMoonFile { get => bitmapFlareMoonFile; set => bitmapFlareMoonFile = value; }
    public CPlugFileImg? GetBitmapFlareMoon(GbxReadSettings settings = default, bool exceptions = false) => bitmapFlareMoonFile?.GetNode(ref bitmapFlareMoon, settings, exceptions) ?? bitmapFlareMoon;

    private float flareAngularSizeSun;
    [AppliedWithChunk<Chunk0917E00E>]
    public float FlareAngularSizeSun { get => flareAngularSizeSun; set => flareAngularSizeSun = value; }

    private float flareAngularSizeMoon;
    [AppliedWithChunk<Chunk0917E00E>]
    public float FlareAngularSizeMoon { get => flareAngularSizeMoon; set => flareAngularSizeMoon = value; }

    private float cameraFarZ;
    [AppliedWithChunk<Chunk0917E00E>]
    public float CameraFarZ { get => cameraFarZ; set => cameraFarZ = value; }

    private CPlugBitmap? bitmapRainFid;
    [AppliedWithChunk<Chunk0917E00E>]
    public CPlugBitmap? BitmapRainFid { get => bitmapRainFid; set => bitmapRainFid = value; }

    private CMwNod? sceneFxFid;
    [AppliedWithChunk<Chunk0917E00E>]
    public CMwNod? SceneFxFid { get => sceneFxFid; set => sceneFxFid = value; }

    private CPlugFileImg? imageLightDirDblSided;
    [AppliedWithChunk<Chunk0917E00F>]
    public CPlugFileImg? ImageLightDirDblSided { get => imageLightDirDblSidedFile?.GetNode(ref imageLightDirDblSided) ?? imageLightDirDblSided; set => imageLightDirDblSided = value; }
    private Components.GbxRefTableFile? imageLightDirDblSidedFile;
    public Components.GbxRefTableFile? ImageLightDirDblSidedFile { get => imageLightDirDblSidedFile; set => imageLightDirDblSidedFile = value; }
    public CPlugFileImg? GetImageLightDirDblSided(GbxReadSettings settings = default, bool exceptions = false) => imageLightDirDblSidedFile?.GetNode(ref imageLightDirDblSided, settings, exceptions) ?? imageLightDirDblSided;

    private CPlugFileImg? bitmapSkyGradV;
    [AppliedWithChunk<Chunk0917E011>]
    public CPlugFileImg? BitmapSkyGradV { get => bitmapSkyGradVFile?.GetNode(ref bitmapSkyGradV) ?? bitmapSkyGradV; set => bitmapSkyGradV = value; }
    private Components.GbxRefTableFile? bitmapSkyGradVFile;
    public Components.GbxRefTableFile? BitmapSkyGradVFile { get => bitmapSkyGradVFile; set => bitmapSkyGradVFile = value; }
    public CPlugFileImg? GetBitmapSkyGradV(GbxReadSettings settings = default, bool exceptions = false) => bitmapSkyGradVFile?.GetNode(ref bitmapSkyGradV, settings, exceptions) ?? bitmapSkyGradV;

    private CPlugFileImg? imageFogColor;
    [AppliedWithChunk<Chunk0917E013>]
    public CPlugFileImg? ImageFogColor { get => imageFogColorFile?.GetNode(ref imageFogColor) ?? imageFogColor; set => imageFogColor = value; }
    private Components.GbxRefTableFile? imageFogColorFile;
    public Components.GbxRefTableFile? ImageFogColorFile { get => imageFogColorFile; set => imageFogColorFile = value; }
    public CPlugFileImg? GetImageFogColor(GbxReadSettings settings = default, bool exceptions = false) => imageFogColorFile?.GetNode(ref imageFogColor, settings, exceptions) ?? imageFogColor;

    private CPlugFileImg? imageSeaColor;
    [AppliedWithChunk<Chunk0917E014>]
    public CPlugFileImg? ImageSeaColor { get => imageSeaColorFile?.GetNode(ref imageSeaColor) ?? imageSeaColor; set => imageSeaColor = value; }
    private Components.GbxRefTableFile? imageSeaColorFile;
    public Components.GbxRefTableFile? ImageSeaColorFile { get => imageSeaColorFile; set => imageSeaColorFile = value; }
    public CPlugFileImg? GetImageSeaColor(GbxReadSettings settings = default, bool exceptions = false) => imageSeaColorFile?.GetNode(ref imageSeaColor, settings, exceptions) ?? imageSeaColor;

    private CPlugClouds? clouds;
    [AppliedWithChunk<Chunk0917E016>]
    public CPlugClouds? Clouds { get => cloudsFile?.GetNode(ref clouds) ?? clouds; set => clouds = value; }
    private Components.GbxRefTableFile? cloudsFile;
    public Components.GbxRefTableFile? CloudsFile { get => cloudsFile; set => cloudsFile = value; }
    public CPlugClouds? GetClouds(GbxReadSettings settings = default, bool exceptions = false) => cloudsFile?.GetNode(ref clouds, settings, exceptions) ?? clouds;

    private GxFogBlender? fogBlender;
    [AppliedWithChunk<Chunk0917E017>]
    public GxFogBlender? FogBlender { get => fogBlenderFile?.GetNode(ref fogBlender) ?? fogBlender; set => fogBlender = value; }
    private Components.GbxRefTableFile? fogBlenderFile;
    public Components.GbxRefTableFile? FogBlenderFile { get => fogBlenderFile; set => fogBlenderFile = value; }
    public GxFogBlender? GetFogBlender(GbxReadSettings settings = default, bool exceptions = false) => fogBlenderFile?.GetNode(ref fogBlender, settings, exceptions) ?? fogBlender;


    /// <summary>
    /// CPlugWeather 0x007 chunk
    /// </summary>
    [Chunk(0x0917E007)]
    public partial class Chunk0917E007 : Chunk<CPlugWeather>
    {
        /// <inheritdoc />
        public override uint Id => 0x0917E007;

    }

    /// <summary>
    /// CPlugWeather 0x00B chunk
    /// </summary>
    [Chunk(0x0917E00B)]
    public partial class Chunk0917E00B : Chunk<CPlugWeather>
    {
        /// <inheritdoc />
        public override uint Id => 0x0917E00B;

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;

        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
        }
    }

    /// <summary>
    /// CPlugWeather 0x00D chunk
    /// </summary>
    [Chunk(0x0917E00D)]
    public partial class Chunk0917E00D : Chunk<CPlugWeather>
    {
        /// <inheritdoc />
        public override uint Id => 0x0917E00D;

        public Vec2 U01;
        public Vec2 U02;
        public Vec2 U03;
        public Vec2 U04;

        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            rw.Vec2(ref U01);
            rw.Vec2(ref n.lDirSpecIntens);
            rw.Vec2(ref n.lDirSpecPower);
            rw.Vec3(ref n.seaTwkWaterColor_Night);
            rw.Vec3(ref n.seaTwkWaterColor_Day);
            rw.Vec2(ref U02);
            rw.Vec2(ref U03);
            rw.Vec2(ref U04);
        }
    }

    /// <summary>
    /// CPlugWeather 0x00E chunk
    /// </summary>
    [Chunk(0x0917E00E)]
    public partial class Chunk0917E00E : Chunk<CPlugWeather>
    {
        /// <inheritdoc />
        public override uint Id => 0x0917E00E;

        public byte[]? U01;
        public byte[]? U02;
        public string? U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            rw.Data(ref U01!, 28);
            rw.Data(ref U02!, 28);
            rw.Id(ref U03);
            rw.NodeRef<CPlugFileImg>(ref n.imageLightAmb, ref n.imageLightAmbFile);
            rw.NodeRef<CPlugFileImg>(ref n.imageLightDirSun, ref n.imageLightDirSunFile);
            rw.NodeRef<CPlugFileImg>(ref n.imageLightDirMoon, ref n.imageLightDirMoonFile);
            rw.NodeRef<CPlugFileImg>(ref n.bitmapFlareSun, ref n.bitmapFlareSunFile);
            rw.NodeRef<CPlugFileImg>(ref n.bitmapFlareMoon, ref n.bitmapFlareMoonFile);
            rw.Single(ref n.flareAngularSizeSun);
            rw.Single(ref n.flareAngularSizeMoon);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref n.cameraFarZ);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapRainFid);
            rw.NodeRef<CMwNod>(ref n.sceneFxFid);
        }
    }

    /// <summary>
    /// CPlugWeather 0x00F chunk
    /// </summary>
    [Chunk(0x0917E00F)]
    public partial class Chunk0917E00F : Chunk<CPlugWeather>
    {
        /// <inheritdoc />
        public override uint Id => 0x0917E00F;


        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugFileImg>(ref n.imageLightDirDblSided, ref n.imageLightDirDblSidedFile);
        }
    }

    /// <summary>
    /// CPlugWeather 0x011 chunk
    /// </summary>
    [Chunk(0x0917E011)]
    public partial class Chunk0917E011 : Chunk<CPlugWeather>
    {
        /// <inheritdoc />
        public override uint Id => 0x0917E011;


        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugFileImg>(ref n.bitmapSkyGradV, ref n.bitmapSkyGradVFile);
        }
    }

    /// <summary>
    /// CPlugWeather 0x013 chunk
    /// </summary>
    [Chunk(0x0917E013)]
    public partial class Chunk0917E013 : Chunk<CPlugWeather>
    {
        /// <inheritdoc />
        public override uint Id => 0x0917E013;


        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugFileImg>(ref n.imageFogColor, ref n.imageFogColorFile);
        }
    }

    /// <summary>
    /// CPlugWeather 0x014 chunk
    /// </summary>
    [Chunk(0x0917E014)]
    public partial class Chunk0917E014 : Chunk<CPlugWeather>
    {
        /// <inheritdoc />
        public override uint Id => 0x0917E014;


        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugFileImg>(ref n.imageSeaColor, ref n.imageSeaColorFile);
        }
    }

    /// <summary>
    /// CPlugWeather 0x016 chunk
    /// </summary>
    [Chunk(0x0917E016)]
    public partial class Chunk0917E016 : Chunk<CPlugWeather>
    {
        /// <inheritdoc />
        public override uint Id => 0x0917E016;


        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugClouds>(ref n.clouds, ref n.cloudsFile);
        }
    }

    /// <summary>
    /// CPlugWeather 0x017 chunk
    /// </summary>
    [Chunk(0x0917E017)]
    public partial class Chunk0917E017 : Chunk<CPlugWeather>
    {
        /// <inheritdoc />
        public override uint Id => 0x0917E017;


        public override void ReadWrite(CPlugWeather n, GbxReaderWriter rw)
        {
            rw.NodeRef<GxFogBlender>(ref n.fogBlender, ref n.fogBlenderFile);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0917E007 => new Chunk0917E007(),
        0x0917E00B => new Chunk0917E00B(),
        0x0917E00D => new Chunk0917E00D(),
        0x0917E00E => new Chunk0917E00E(),
        0x0917E00F => new Chunk0917E00F(),
        0x0917E011 => new Chunk0917E011(),
        0x0917E013 => new Chunk0917E013(),
        0x0917E014 => new Chunk0917E014(),
        0x0917E016 => new Chunk0917E016(),
        0x0917E017 => new Chunk0917E017(),
        _ => base.NewChunk(chunkId),
    };
}
