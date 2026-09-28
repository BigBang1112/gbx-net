namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A003000</remarks>
[Class(0x0A003000)]
public partial class CSceneLayout : CScene, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A003000;




    private float cameraFarZ;
    [AppliedWithChunk<Chunk0A003010>]
    [AppliedWithChunk<Chunk0A00301C>]
    public float CameraFarZ { get => cameraFarZ; set => cameraFarZ = value; }

    private Vec3 cameraClearColor;
    [AppliedWithChunk<Chunk0A003010>]
    public Vec3 CameraClearColor { get => cameraClearColor; set => cameraClearColor = value; }

    private CSceneSector[]? sectors;
    [AppliedWithChunk<Chunk0A003018>]
    [AppliedWithChunk<Chunk0A00301B>]
    public CSceneSector[]? Sectors { get => sectors; set => sectors = value; }

    private SceneMobil[]? scene;
    [AppliedWithChunk<Chunk0A003018>]
    [AppliedWithChunk<Chunk0A00301B>]
    public SceneMobil[]? Scene { get => scene; set => scene = value; }

    private SceneLoc[]? sceneLocations;
    [AppliedWithChunk<Chunk0A003018>]
    [AppliedWithChunk<Chunk0A00301B>]
    public SceneLoc[]? SceneLocations { get => sceneLocations; set => sceneLocations = value; }

    private External<CSceneLight>[]? lights;
    [AppliedWithChunk<Chunk0A003018>]
    [AppliedWithChunk<Chunk0A00301B>]
    public External<CSceneLight>[]? Lights { get => lights; set => lights = value; }

    private SceneLoc[]? lightLocations;
    [AppliedWithChunk<Chunk0A003018>]
    [AppliedWithChunk<Chunk0A00301B>]
    public SceneLoc[]? LightLocations { get => lightLocations; set => lightLocations = value; }

    private CSceneObject[]? sounds;
    [AppliedWithChunk<Chunk0A003018>]
    [AppliedWithChunk<Chunk0A00301B>]
    public CSceneObject[]? Sounds { get => sounds; set => sounds = value; }

    private SceneLoc[]? soundLocations;
    [AppliedWithChunk<Chunk0A003018>]
    [AppliedWithChunk<Chunk0A00301B>]
    public SceneLoc[]? SoundLocations { get => soundLocations; set => soundLocations = value; }

    private CSceneObject[]? locations;
    [AppliedWithChunk<Chunk0A003018>]
    [AppliedWithChunk<Chunk0A00301B>]
    public CSceneObject[]? Locations { get => locations; set => locations = value; }

    private SceneLoc[]? locationLocations;
    [AppliedWithChunk<Chunk0A003018>]
    [AppliedWithChunk<Chunk0A00301B>]
    public SceneLoc[]? LocationLocations { get => locationLocations; set => locationLocations = value; }

    private CSceneObject[]? fields;
    [AppliedWithChunk<Chunk0A003018>]
    public CSceneObject[]? Fields { get => fields; set => fields = value; }

    private SceneLoc[]? fieldLocations;
    [AppliedWithChunk<Chunk0A003018>]
    public SceneLoc[]? FieldLocations { get => fieldLocations; set => fieldLocations = value; }

    private CSceneGate[]? gates;
    [AppliedWithChunk<Chunk0A003018>]
    public CSceneGate[]? Gates { get => gates; set => gates = value; }

    private CScenePath[]? paths;
    [AppliedWithChunk<Chunk0A003018>]
    public CScenePath[]? Paths { get => paths; set => paths = value; }

    private CSceneTrafficGraph? trafficGraph;
    [AppliedWithChunk<Chunk0A003018>]
    public CSceneTrafficGraph? TrafficGraph { get => trafficGraph; set => trafficGraph = value; }

    private CSceneTrafficPath[]? trafficPaths;
    [AppliedWithChunk<Chunk0A003018>]
    public CSceneTrafficPath[]? TrafficPaths { get => trafficPaths; set => trafficPaths = value; }

    private CSceneFxNod? sceneFxNod;
    [AppliedWithChunk<Chunk0A003018>]
    [AppliedWithChunk<Chunk0A00301B>]
    [AppliedWithChunk<Chunk0A00301C>]
    public CSceneFxNod? SceneFxNod { get => sceneFxNodFile?.GetNode(ref sceneFxNod) ?? sceneFxNod; set => sceneFxNod = value; }
    private Components.GbxRefTableFile? sceneFxNodFile;
    public Components.GbxRefTableFile? SceneFxNodFile { get => sceneFxNodFile; set => sceneFxNodFile = value; }
    public CSceneFxNod? GetSceneFxNod(GbxReadSettings settings = default, bool exceptions = false) => sceneFxNodFile?.GetNode(ref sceneFxNod, settings, exceptions) ?? sceneFxNod;

    private CSceneObject[]? objects;
    [AppliedWithChunk<Chunk0A00301C>]
    public CSceneObject[]? Objects { get => objects; set => objects = value; }

    private Iso4[]? objectLocations;
    [AppliedWithChunk<Chunk0A00301C>]
    public Iso4[]? ObjectLocations { get => objectLocations; set => objectLocations = value; }

    private External<CPlugWeatherModel>[]? weatherModels;
    [AppliedWithChunk<Chunk0A00301C>]
    public External<CPlugWeatherModel>[]? WeatherModels { get => weatherModels; set => weatherModels = value; }

    private CPlugWeatherModel? weatherModel;
    [AppliedWithChunk<Chunk0A00301C>]
    public CPlugWeatherModel? WeatherModel { get => weatherModelFile?.GetNode(ref weatherModel) ?? weatherModel; set => weatherModel = value; }
    private Components.GbxRefTableFile? weatherModelFile;
    public Components.GbxRefTableFile? WeatherModelFile { get => weatherModelFile; set => weatherModelFile = value; }
    public CPlugWeatherModel? GetWeatherModel(GbxReadSettings settings = default, bool exceptions = false) => weatherModelFile?.GetNode(ref weatherModel, settings, exceptions) ?? weatherModel;

    private CPlugBitmap? bitmapWaterFog;
    [AppliedWithChunk<Chunk0A00301C>]
    public CPlugBitmap? BitmapWaterFog { get => bitmapWaterFogFile?.GetNode(ref bitmapWaterFog) ?? bitmapWaterFog; set => bitmapWaterFog = value; }
    private Components.GbxRefTableFile? bitmapWaterFogFile;
    public Components.GbxRefTableFile? BitmapWaterFogFile { get => bitmapWaterFogFile; set => bitmapWaterFogFile = value; }
    public CPlugBitmap? GetBitmapWaterFog(GbxReadSettings settings = default, bool exceptions = false) => bitmapWaterFogFile?.GetNode(ref bitmapWaterFog, settings, exceptions) ?? bitmapWaterFog;

    private CPlugBitmap? bitmapCubeReflectHardSpecA;
    [AppliedWithChunk<Chunk0A00301C>]
    public CPlugBitmap? BitmapCubeReflectHardSpecA { get => bitmapCubeReflectHardSpecAFile?.GetNode(ref bitmapCubeReflectHardSpecA) ?? bitmapCubeReflectHardSpecA; set => bitmapCubeReflectHardSpecA = value; }
    private Components.GbxRefTableFile? bitmapCubeReflectHardSpecAFile;
    public Components.GbxRefTableFile? BitmapCubeReflectHardSpecAFile { get => bitmapCubeReflectHardSpecAFile; set => bitmapCubeReflectHardSpecAFile = value; }
    public CPlugBitmap? GetBitmapCubeReflectHardSpecA(GbxReadSettings settings = default, bool exceptions = false) => bitmapCubeReflectHardSpecAFile?.GetNode(ref bitmapCubeReflectHardSpecA, settings, exceptions) ?? bitmapCubeReflectHardSpecA;

    private CPlugBitmap? bitmapCubeReflectHdrAlpha2;
    [AppliedWithChunk<Chunk0A00301C>]
    public CPlugBitmap? BitmapCubeReflectHdrAlpha2 { get => bitmapCubeReflectHdrAlpha2File?.GetNode(ref bitmapCubeReflectHdrAlpha2) ?? bitmapCubeReflectHdrAlpha2; set => bitmapCubeReflectHdrAlpha2 = value; }
    private Components.GbxRefTableFile? bitmapCubeReflectHdrAlpha2File;
    public Components.GbxRefTableFile? BitmapCubeReflectHdrAlpha2File { get => bitmapCubeReflectHdrAlpha2File; set => bitmapCubeReflectHdrAlpha2File = value; }
    public CPlugBitmap? GetBitmapCubeReflectHdrAlpha2(GbxReadSettings settings = default, bool exceptions = false) => bitmapCubeReflectHdrAlpha2File?.GetNode(ref bitmapCubeReflectHdrAlpha2, settings, exceptions) ?? bitmapCubeReflectHdrAlpha2;

    /// <summary>
    /// Creates a new instance of <see cref="CSceneLayout"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneLayout() { }


    /// <summary>
    /// CSceneLayout 0x00C chunk
    /// </summary>
    [Chunk(0x0A00300C)]
    public partial class Chunk0A00300C : Chunk<CSceneLayout>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A00300C;

        public int U01;

        public override void ReadWrite(CSceneLayout n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CSceneLayout 0x010 chunk
    /// </summary>
    [Chunk(0x0A003010)]
    public partial class Chunk0A003010 : Chunk<CSceneLayout>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A003010;


        public override void ReadWrite(CSceneLayout n, GbxReaderWriter rw)
        {
            rw.Single(ref n.cameraFarZ);
            rw.Vec3(ref n.cameraClearColor);
        }
    }

    /// <summary>
    /// CSceneLayout 0x014 chunk
    /// </summary>
    [Chunk(0x0A003014)]
    public partial class Chunk0A003014 : Chunk<CSceneLayout>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A003014;

        public int U01;
        public bool U02;
        public BoxAligned U03;
        public Iso4 U04;

        public override void ReadWrite(CSceneLayout n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Boolean(ref U02);
            rw.BoxAligned(ref U03);
            rw.Iso4(ref U04);
        }
    }

    /// <summary>
    /// CSceneLayout 0x017 chunk
    /// </summary>
    [Chunk(0x0A003017)]
    public partial class Chunk0A003017 : Chunk<CSceneLayout>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A003017;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CSceneLayout 0x018 chunk
    /// </summary>
    [Chunk(0x0A003018)]
    public partial class Chunk0A003018 : Chunk<CSceneLayout>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0A003018;

        public int Version { get; set; }

        public CSceneObject[]? U01;
        public SceneLoc[]? U02;

        public override void ReadWrite(CSceneLayout n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CSceneSector>(ref n.sectors!);
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<SceneMobil>(ref n.scene!);
            rw.ArrayReadableWritable<SceneLoc>(ref n.sceneLocations!);
            rw.ArrayNodeRef_deprec<CSceneObject>(ref U01!);
            rw.ArrayReadableWritable<SceneLoc>(ref U02!);
            rw.ArrayNodeRef_deprec<CSceneLight>(ref n.lights!);
            rw.ArrayReadableWritable<SceneLoc>(ref n.lightLocations!);
            rw.ArrayNodeRef_deprec<CSceneObject>(ref n.sounds!);
            rw.ArrayReadableWritable<SceneLoc>(ref n.soundLocations!);
            rw.ArrayNodeRef_deprec<CSceneObject>(ref n.locations!);
            rw.ArrayReadableWritable<SceneLoc>(ref n.locationLocations!);
            rw.ArrayNodeRef_deprec<CSceneObject>(ref n.fields!);
            rw.ArrayReadableWritable<SceneLoc>(ref n.fieldLocations!);
            rw.ArrayNodeRef_deprec<CSceneGate>(ref n.gates!);
            rw.ArrayNodeRef_deprec<CScenePath>(ref n.paths!);
            rw.NodeRef<CSceneTrafficGraph>(ref n.trafficGraph);
            rw.ArrayNodeRef_deprec<CSceneTrafficPath>(ref n.trafficPaths!);
            rw.NodeRef<CSceneFxNod>(ref n.sceneFxNod, ref n.sceneFxNodFile);
        }
    }

    /// <summary>
    /// CSceneLayout 0x019 chunk
    /// </summary>
    [Chunk(0x0A003019)]
    public partial class Chunk0A003019 : Chunk<CSceneLayout>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A003019;

        public int U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;

        public override void ReadWrite(CSceneLayout n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
        }
    }

    /// <summary>
    /// CSceneLayout 0x01B chunk
    /// </summary>
    [Chunk(0x0A00301B)]
    public partial class Chunk0A00301B : Chunk<CSceneLayout>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0A00301B;

        public int Version { get; set; }

        public CSceneObject[]? U01;
        public SceneLoc[]? U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CSceneLayout n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CSceneSector>(ref n.sectors!);
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<SceneMobil>(ref n.scene!);
            rw.ArrayReadableWritable<SceneLoc>(ref n.sceneLocations!);
            rw.ArrayNodeRef_deprec<CSceneObject>(ref U01!);
            rw.ArrayReadableWritable<SceneLoc>(ref U02!);
            rw.ArrayNodeRef_deprec<CSceneLight>(ref n.lights!);
            rw.ArrayReadableWritable<SceneLoc>(ref n.lightLocations!);
            rw.ArrayNodeRef_deprec<CSceneObject>(ref n.sounds!);
            rw.ArrayReadableWritable<SceneLoc>(ref n.soundLocations!);
            rw.ArrayNodeRef_deprec<CSceneObject>(ref n.locations!);
            rw.ArrayReadableWritable<SceneLoc>(ref n.locationLocations!);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.NodeRef<CSceneFxNod>(ref n.sceneFxNod, ref n.sceneFxNodFile);
        }
    }

    /// <summary>
    /// CSceneLayout 0x01C chunk
    /// </summary>
    [Chunk(0x0A00301C)]
    public partial class Chunk0A00301C : Chunk<CSceneLayout>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0A00301C;

        public int Version { get; set; }

        public Unknown2[]? U01;
        public Unknown3[]? U02;
        public CSceneConfig? U03;
        public uint[]? U04;
        public CSceneLocation[]? U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;
        public float U15;
        public float U16;
        public float U17;
        public float U18;
        public float U19;

        public override void ReadWrite(CSceneLayout n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 2)
            {
                rw.ArrayNodeRef_deprec<CSceneObject>(ref n.objects!);
                rw.Array<Iso4>(ref n.objectLocations!);
            }
            if (Version >= 3)
            {
                rw.ArrayReadableWritable<Unknown2>(ref U01!);
                rw.ArrayReadableWritable<Unknown3>(ref U02!, version: Version);
            }
            rw.NodeRef<CSceneConfig>(ref U03);
            if (Version <= 1)
            {
                rw.ArrayNodeRef_deprec<CPlugWeatherModel>(ref n.weatherModels!);
            }
            if (Version >= 2)
            {
                rw.NodeRef<CPlugWeatherModel>(ref n.weatherModel, ref n.weatherModelFile);
            }
            rw.Array<uint>(ref U04!);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapWaterFog, ref n.bitmapWaterFogFile);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapCubeReflectHardSpecA, ref n.bitmapCubeReflectHardSpecAFile);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapCubeReflectHdrAlpha2, ref n.bitmapCubeReflectHdrAlpha2File);
            if (Version == 0)
            {
                rw.ArrayNodeRef<CSceneLocation>(ref U05!);
            }
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);
            rw.Single(ref U14);
            rw.Single(ref U15);
            rw.Single(ref U16);
            rw.Single(ref n.cameraFarZ);
            rw.Single(ref U17);
            rw.Single(ref U18);
            rw.Single(ref U19);
            rw.NodeRef<CSceneFxNod>(ref n.sceneFxNod, ref n.sceneFxNodFile);
        }
    }


    public sealed partial class Unknown2 : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private TransQuat u02;
        public TransQuat U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private CPlugBitmap? u04;
        public CPlugBitmap? U04 { get => u04File?.GetNode(ref u04) ?? u04; set => u04 = value; }
        private Components.GbxRefTableFile? u04File;
        public Components.GbxRefTableFile? U04File { get => u04File; set => u04File = value; }
        public CPlugBitmap? GetU04(GbxReadSettings settings = default, bool exceptions = false) => u04File?.GetNode(ref u04, settings, exceptions) ?? u04;

        private CPlugBitmap? u05;
        public CPlugBitmap? U05 { get => u05File?.GetNode(ref u05) ?? u05; set => u05 = value; }
        private Components.GbxRefTableFile? u05File;
        public Components.GbxRefTableFile? U05File { get => u05File; set => u05File = value; }
        public CPlugBitmap? GetU05(GbxReadSettings settings = default, bool exceptions = false) => u05File?.GetNode(ref u05, settings, exceptions) ?? u05;

        private CPlugBitmap? u06;
        public CPlugBitmap? U06 { get => u06File?.GetNode(ref u06) ?? u06; set => u06 = value; }
        private Components.GbxRefTableFile? u06File;
        public Components.GbxRefTableFile? U06File { get => u06File; set => u06File = value; }
        public CPlugBitmap? GetU06(GbxReadSettings settings = default, bool exceptions = false) => u06File?.GetNode(ref u06, settings, exceptions) ?? u06;

        private GxLight? u07;
        public GxLight? U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.TransQuat(ref u02);
            rw.Int32(ref u03);
            rw.NodeRef<CPlugBitmap>(ref u04, ref u04File);
            rw.NodeRef<CPlugBitmap>(ref u05, ref u05File);
            rw.NodeRef<CPlugBitmap>(ref u06, ref u06File);
            rw.NodeRef<GxLight>(ref u07);
            rw.Int32(ref u08);
        }
    }

    public sealed partial class Unknown3 : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private TransQuat u02;
        public TransQuat U02 { get => u02; set => u02 = value; }

        private short u03;
        public short U03 { get => u03; set => u03 = value; }

        private ulong u04;
        public ulong U04 { get => u04; set => u04 = value; }

        private CPlugSolid? u05;
        public CPlugSolid? U05 { get => u05File?.GetNode(ref u05) ?? u05; set => u05 = value; }
        private Components.GbxRefTableFile? u05File;
        public Components.GbxRefTableFile? U05File { get => u05File; set => u05File = value; }
        public CPlugSolid? GetU05(GbxReadSettings settings = default, bool exceptions = false) => u05File?.GetNode(ref u05, settings, exceptions) ?? u05;

        private CPlugSolid2Model? u06;
        public CPlugSolid2Model? U06 { get => u06; set => u06 = value; }

        private CPlugPrefab? u07;
        public CPlugPrefab? U07 { get => u07; set => u07 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.TransQuat(ref u02);
            rw.Int16(ref u03);
            rw.UInt64(ref u04);
            rw.NodeRef<CPlugSolid>(ref u05, ref u05File);
            if (v >= 4)
            {
                rw.NodeRef<CPlugSolid2Model>(ref u06);
                if (v >= 5)
                {
                    rw.NodeRef<CPlugPrefab>(ref u07);
                }
            }
        }
    }

    public sealed partial class SceneMobil : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private CMwNod? mobil;
        public CMwNod? Mobil { get => mobil; set => mobil = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            if (U01==-2)
            {
                rw.NodeRef<CMwNod>(ref mobil);
            }
        }
    }

    public sealed partial class Unknown : IReadableWritable
    {

        private CMwNod? u01;
        public CMwNod? U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private CMwNod? u04;
        public CMwNod? U04 { get => u04; set => u04 = value; }

        private string? u05;
        public string? U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private Iso4 u07;
        public Iso4 U07 { get => u07; set => u07 = value; }

        private Iso4 u08;
        public Iso4 U08 { get => u08; set => u08 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.NodeRef<CMwNod>(ref u01);
            rw.Id(ref u02);
            rw.Int32(ref u03);
            rw.NodeRef<CMwNod>(ref u04);
            rw.Id(ref u05);
            rw.Int32(ref u06);
            rw.Iso4(ref u07);
            rw.Iso4(ref u08);
        }
    }

    public sealed partial class SceneLoc : IReadableWritable
    {

        private CMwNod? u01;
        public CMwNod? U01 { get => u01; set => u01 = value; }

        private Iso4 u02;
        public Iso4 U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.NodeRef<CMwNod>(ref u01);
            rw.Iso4(ref u02);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A00300C => new Chunk0A00300C(),
        0x0A003010 => new Chunk0A003010(),
        0x0A003014 => new Chunk0A003014(),
        0x0A003017 => new Chunk0A003017(),
        0x0A003018 => new Chunk0A003018(),
        0x0A003019 => new Chunk0A003019(),
        0x0A00301B => new Chunk0A00301B(),
        0x0A00301C => new Chunk0A00301C(),
        _ => base.NewChunk(chunkId),
    };
}
