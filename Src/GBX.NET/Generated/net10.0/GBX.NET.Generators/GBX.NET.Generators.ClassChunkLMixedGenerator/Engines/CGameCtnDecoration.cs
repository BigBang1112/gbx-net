namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03038000</remarks>
[Class(0x03038000)]
public partial class CGameCtnDecoration : CGameCtnCollector, IClass
{
    [Hexadecimal] public static new uint Id => 0x03038000;




    private CGameCtnDecorationSize? decoSize;
    [AppliedWithChunk<Chunk03038011>]
    public CGameCtnDecorationSize? DecoSize { get => decoSizeFile?.GetNode(ref decoSize) ?? decoSize; set => decoSize = value; }
    private Components.GbxRefTableFile? decoSizeFile;
    public Components.GbxRefTableFile? DecoSizeFile { get => decoSizeFile; set => decoSizeFile = value; }
    public CGameCtnDecorationSize? GetDecoSize(GbxReadSettings settings = default, bool exceptions = false) => decoSizeFile?.GetNode(ref decoSize, settings, exceptions) ?? decoSize;

    private CGameCtnDecorationAudio? decoAudio;
    [AppliedWithChunk<Chunk03038012>]
    [AppliedWithChunk<Chunk03038019>]
    public CGameCtnDecorationAudio? DecoAudio { get => decoAudioFile?.GetNode(ref decoAudio) ?? decoAudio; set => decoAudio = value; }
    private Components.GbxRefTableFile? decoAudioFile;
    public Components.GbxRefTableFile? DecoAudioFile { get => decoAudioFile; set => decoAudioFile = value; }
    public CGameCtnDecorationAudio? GetDecoAudio(GbxReadSettings settings = default, bool exceptions = false) => decoAudioFile?.GetNode(ref decoAudio, settings, exceptions) ?? decoAudio;

    private CGameCtnDecorationMood? decoMood;
    [AppliedWithChunk<Chunk03038013>]
    public CGameCtnDecorationMood? DecoMood { get => decoMoodFile?.GetNode(ref decoMood) ?? decoMood; set => decoMood = value; }
    private Components.GbxRefTableFile? decoMoodFile;
    public Components.GbxRefTableFile? DecoMoodFile { get => decoMoodFile; set => decoMoodFile = value; }
    public CGameCtnDecorationMood? GetDecoMood(GbxReadSettings settings = default, bool exceptions = false) => decoMoodFile?.GetNode(ref decoMood, settings, exceptions) ?? decoMood;

    private CPlugDecoratorSolid? decoratorSolidWarp;
    [AppliedWithChunk<Chunk03038014>]
    public CPlugDecoratorSolid? DecoratorSolidWarp { get => decoratorSolidWarpFile?.GetNode(ref decoratorSolidWarp) ?? decoratorSolidWarp; set => decoratorSolidWarp = value; }
    private Components.GbxRefTableFile? decoratorSolidWarpFile;
    public Components.GbxRefTableFile? DecoratorSolidWarpFile { get => decoratorSolidWarpFile; set => decoratorSolidWarpFile = value; }
    public CPlugDecoratorSolid? GetDecoratorSolidWarp(GbxReadSettings settings = default, bool exceptions = false) => decoratorSolidWarpFile?.GetNode(ref decoratorSolidWarp, settings, exceptions) ?? decoratorSolidWarp;

    private CGameCtnDecorationTerrainModifier? terrainModifierCovered;
    [AppliedWithChunk<Chunk03038015>]
    public CGameCtnDecorationTerrainModifier? TerrainModifierCovered { get => terrainModifierCoveredFile?.GetNode(ref terrainModifierCovered) ?? terrainModifierCovered; set => terrainModifierCovered = value; }
    private Components.GbxRefTableFile? terrainModifierCoveredFile;
    public Components.GbxRefTableFile? TerrainModifierCoveredFile { get => terrainModifierCoveredFile; set => terrainModifierCoveredFile = value; }
    public CGameCtnDecorationTerrainModifier? GetTerrainModifierCovered(GbxReadSettings settings = default, bool exceptions = false) => terrainModifierCoveredFile?.GetNode(ref terrainModifierCovered, settings, exceptions) ?? terrainModifierCovered;

    private CGameCtnDecorationTerrainModifier? terrainModifierBase;
    [AppliedWithChunk<Chunk03038016>]
    public CGameCtnDecorationTerrainModifier? TerrainModifierBase { get => terrainModifierBaseFile?.GetNode(ref terrainModifierBase) ?? terrainModifierBase; set => terrainModifierBase = value; }
    private Components.GbxRefTableFile? terrainModifierBaseFile;
    public Components.GbxRefTableFile? TerrainModifierBaseFile { get => terrainModifierBaseFile; set => terrainModifierBaseFile = value; }
    public CGameCtnDecorationTerrainModifier? GetTerrainModifierBase(GbxReadSettings settings = default, bool exceptions = false) => terrainModifierBaseFile?.GetNode(ref terrainModifierBase, settings, exceptions) ?? terrainModifierBase;

    private string? decorationZoneFrontierId;
    [AppliedWithChunk<Chunk03038017>]
    public string? DecorationZoneFrontierId { get => decorationZoneFrontierId; set => decorationZoneFrontierId = value; }

    private bool isWaterOutsidePlayField;
    [AppliedWithChunk<Chunk03038017>]
    public bool IsWaterOutsidePlayField { get => isWaterOutsidePlayField; set => isWaterOutsidePlayField = value; }

    private CPlugGameSkin? vehicleFxSkin;
    [AppliedWithChunk<Chunk03038018>]
    public CPlugGameSkin? VehicleFxSkin { get => vehicleFxSkinFile?.GetNode(ref vehicleFxSkin) ?? vehicleFxSkin; set => vehicleFxSkin = value; }
    private Components.GbxRefTableFile? vehicleFxSkinFile;
    public Components.GbxRefTableFile? VehicleFxSkinFile { get => vehicleFxSkinFile; set => vehicleFxSkinFile = value; }
    public CPlugGameSkin? GetVehicleFxSkin(GbxReadSettings settings = default, bool exceptions = false) => vehicleFxSkinFile?.GetNode(ref vehicleFxSkin, settings, exceptions) ?? vehicleFxSkin;

    private string? vehicleFxFolder;
    [AppliedWithChunk<Chunk03038018>]
    public string? VehicleFxFolder { get => vehicleFxFolder; set => vehicleFxFolder = value; }

    private CPlugSound? decoAudioAmbient;
    [AppliedWithChunk<Chunk03038019>]
    public CPlugSound? DecoAudioAmbient { get => decoAudioAmbientFile?.GetNode(ref decoAudioAmbient) ?? decoAudioAmbient; set => decoAudioAmbient = value; }
    private Components.GbxRefTableFile? decoAudioAmbientFile;
    public Components.GbxRefTableFile? DecoAudioAmbientFile { get => decoAudioAmbientFile; set => decoAudioAmbientFile = value; }
    public CPlugSound? GetDecoAudioAmbient(GbxReadSettings settings = default, bool exceptions = false) => decoAudioAmbientFile?.GetNode(ref decoAudioAmbient, settings, exceptions) ?? decoAudioAmbient;

    private CGameCtnDecorationMaterialModifiers? decoMaterialModifiers;
    [AppliedWithChunk<Chunk0303801A>]
    public CGameCtnDecorationMaterialModifiers? DecoMaterialModifiers { get => decoMaterialModifiers; set => decoMaterialModifiers = value; }

    private CGameCtnChallenge? decoMap;
    [AppliedWithChunk<Chunk0303801B>]
    public CGameCtnChallenge? DecoMap { get => decoMapFile?.GetNode(ref decoMap) ?? decoMap; set => decoMap = value; }
    private Components.GbxRefTableFile? decoMapFile;
    public Components.GbxRefTableFile? DecoMapFile { get => decoMapFile; set => decoMapFile = value; }
    public CGameCtnChallenge? GetDecoMap(GbxReadSettings settings = default, bool exceptions = false) => decoMapFile?.GetNode(ref decoMap, settings, exceptions) ?? decoMap;

    private CMwNod? decoMapLightMap;
    /// <summary>
    /// Deco.LightMap.Gbx of the DecoMap like from cache - zip with LightMapCache.Gbx inside
    /// </summary>
    [AppliedWithChunk<Chunk0303801B>]
    public CMwNod? DecoMapLightMap { get => decoMapLightMapFile?.GetNode(ref decoMapLightMap) ?? decoMapLightMap; set => decoMapLightMap = value; }
    private Components.GbxRefTableFile? decoMapLightMapFile;
    public Components.GbxRefTableFile? DecoMapLightMapFile { get => decoMapLightMapFile; set => decoMapLightMapFile = value; }
    public CMwNod? GetDecoMapLightMap(GbxReadSettings settings = default, bool exceptions = false) => decoMapLightMapFile?.GetNode(ref decoMapLightMap, settings, exceptions) ?? decoMapLightMap;

    /// <summary>
    /// [SMoodRemaping] CGameCtnDecoration 0x000 header chunk
    /// </summary>
    [Chunk(0x03038000)]
    public partial class HeaderChunk03038000 : HeaderChunk<CGameCtnDecoration>
    {
        /// <inheritdoc />
        public override uint Id => 0x03038000;

    }

    /// <summary>
    /// [SLightMap] CGameCtnDecoration 0x001 header chunk (LightMap)
    /// </summary>
    [Chunk(0x03038001, "LightMap")]
    public partial class HeaderChunk03038001 : HeaderChunk<CGameCtnDecoration>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03038001;

        public int Version { get; set; }

        /// <summary>
        /// CHmsLightMap Tech
        /// </summary>
        public int U01;

        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.VersionByte(this);
            rw.Int32(ref U01); // CHmsLightMap Tech
        }
    }


    /// <summary>
    /// CGameCtnDecoration 0x011 chunk (DecoSize)
    /// </summary>
    [Chunk(0x03038011, "DecoSize")]
    public partial class Chunk03038011 : Chunk<CGameCtnDecoration>
    {
        /// <inheritdoc />
        public override uint Id => 0x03038011;


        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationSize>(ref n.decoSize, ref n.decoSizeFile);
        }
    }

    /// <summary>
    /// CGameCtnDecoration 0x012 chunk (DecoAudio)
    /// </summary>
    [Chunk(0x03038012, "DecoAudio")]
    public partial class Chunk03038012 : Chunk<CGameCtnDecoration>
    {
        /// <inheritdoc />
        public override uint Id => 0x03038012;


        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationAudio>(ref n.decoAudio, ref n.decoAudioFile);
        }
    }

    /// <summary>
    /// CGameCtnDecoration 0x013 chunk (DecoMood)
    /// </summary>
    [Chunk(0x03038013, "DecoMood")]
    public partial class Chunk03038013 : Chunk<CGameCtnDecoration>
    {
        /// <inheritdoc />
        public override uint Id => 0x03038013;


        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationMood>(ref n.decoMood, ref n.decoMoodFile);
        }
    }

    /// <summary>
    /// CGameCtnDecoration 0x014 chunk (DecoratorSolidWarp)
    /// </summary>
    [Chunk(0x03038014, "DecoratorSolidWarp")]
    public partial class Chunk03038014 : Chunk<CGameCtnDecoration>
    {
        /// <inheritdoc />
        public override uint Id => 0x03038014;


        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugDecoratorSolid>(ref n.decoratorSolidWarp, ref n.decoratorSolidWarpFile);
        }
    }

    /// <summary>
    /// CGameCtnDecoration 0x015 chunk (TerrainModifierCovered)
    /// </summary>
    [Chunk(0x03038015, "TerrainModifierCovered")]
    public partial class Chunk03038015 : Chunk<CGameCtnDecoration>
    {
        /// <inheritdoc />
        public override uint Id => 0x03038015;


        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationTerrainModifier>(ref n.terrainModifierCovered, ref n.terrainModifierCoveredFile);
        }
    }

    /// <summary>
    /// CGameCtnDecoration 0x016 chunk (TerrainModifierBase)
    /// </summary>
    [Chunk(0x03038016, "TerrainModifierBase")]
    public partial class Chunk03038016 : Chunk<CGameCtnDecoration>
    {
        /// <inheritdoc />
        public override uint Id => 0x03038016;


        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationTerrainModifier>(ref n.terrainModifierBase, ref n.terrainModifierBaseFile);
        }
    }

    /// <summary>
    /// CGameCtnDecoration 0x017 chunk
    /// </summary>
    [Chunk(0x03038017)]
    public partial class Chunk03038017 : Chunk<CGameCtnDecoration>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03038017;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.decorationZoneFrontierId);
            if (Version >= 1)
            {
                rw.Boolean(ref n.isWaterOutsidePlayField);
            }
        }
    }

    /// <summary>
    /// CGameCtnDecoration 0x018 chunk
    /// </summary>
    [Chunk(0x03038018)]
    public partial class Chunk03038018 : Chunk<CGameCtnDecoration>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03038018;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugGameSkin>(ref n.vehicleFxSkin, ref n.vehicleFxSkinFile);
            rw.String(ref n.vehicleFxFolder);
        }
    }

    /// <summary>
    /// CGameCtnDecoration 0x019 chunk
    /// </summary>
    [Chunk(0x03038019)]
    public partial class Chunk03038019 : Chunk<CGameCtnDecoration>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03038019;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CGameCtnDecorationAudio>(ref n.decoAudio, ref n.decoAudioFile);
            if (Version >= 1)
            {
                rw.NodeRef<CPlugSound>(ref n.decoAudioAmbient, ref n.decoAudioAmbientFile);
            }
        }
    }

    /// <summary>
    /// CGameCtnDecoration 0x01A chunk
    /// </summary>
    [Chunk(0x0303801A)]
    public partial class Chunk0303801A : Chunk<CGameCtnDecoration>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0303801A;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CGameCtnDecorationMaterialModifiers>(ref n.decoMaterialModifiers);
        }
    }

    /// <summary>
    /// CGameCtnDecoration 0x01B chunk
    /// </summary>
    [Chunk(0x0303801B)]
    public partial class Chunk0303801B : Chunk<CGameCtnDecoration>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0303801B;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CGameCtnChallenge>(ref n.decoMap, ref n.decoMapFile);
            if (Version >= 1)
            {
                rw.NodeRef<CMwNod>(ref n.decoMapLightMap, ref n.decoMapLightMapFile); // Deco.LightMap.Gbx of the DecoMap like from cache - zip with LightMapCache.Gbx inside
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03038011 => new Chunk03038011(),
        0x03038012 => new Chunk03038012(),
        0x03038013 => new Chunk03038013(),
        0x03038014 => new Chunk03038014(),
        0x03038015 => new Chunk03038015(),
        0x03038016 => new Chunk03038016(),
        0x03038017 => new Chunk03038017(),
        0x03038018 => new Chunk03038018(),
        0x03038019 => new Chunk03038019(),
        0x0303801A => new Chunk0303801A(),
        0x0303801B => new Chunk0303801B(),
        _ => base.NewChunk(chunkId),
    };
}
