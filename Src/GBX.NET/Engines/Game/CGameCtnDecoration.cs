namespace GBX.NET.Engines.Game;

[Node(0x03038000)]
[NodeExtension("TMDecoration")]
[NodeExtension("Decoration")]
public partial class CGameCtnDecoration : CGameCtnCollector, CGameCtnDecoration.IHeader
{
    private CGameCtnDecorationSize? decoSize;
    private GameBoxRefTable.File? decoSizeFile;
    private CGameCtnDecorationAudio? decoAudio;
    private GameBoxRefTable.File? decoAudioFile;
    private CGameCtnDecorationMood? decoMood;
    private GameBoxRefTable.File? decoMoodFile;
    private CPlugDecoratorSolid? decoratorSolidWarp;
    private CGameCtnDecorationTerrainModifier? terrainModifierCovered;
    private GameBoxRefTable.File? terrainModifierCoveredFile;
    private CGameCtnDecorationTerrainModifier? terrainModifierBase;
    private GameBoxRefTable.File? terrainModifierBaseFile;
    private string? decorationZoneFrontierId;
    private bool isWaterOutsidePlayField;
    private CPlugGameSkin? vehicleFxSkin;
    private string? vehicleFxFolder;
    private CPlugSound? decoAudioAmbient;
    private CGameCtnDecorationMaterialModifiers? decoMaterialModifiers;
    private CGameCtnChallenge? decoMap;
    private GameBoxRefTable.File? decoMapFile;
    private CMwNod? decoMapLightMap;
    private GameBoxRefTable.File? decoMapLightMapFile;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03038011>]
    public CGameCtnDecorationSize? DecoSize
    {
        get => decoSize = GetNodeFromRefTable(decoSize, decoSizeFile) as CGameCtnDecorationSize;
        set => decoSize = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03038012>]
    [AppliedWithChunk<Chunk03038019>]
    public CGameCtnDecorationAudio? DecoAudio { get => decoAudio; set => decoAudio = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03038013>]
    public CGameCtnDecorationMood? DecoMood { get => decoMood; set => decoMood = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03038014>]
    public CPlugDecoratorSolid? DecoratorSolidWarp { get => decoratorSolidWarp; set => decoratorSolidWarp = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03038015>]
    public CGameCtnDecorationTerrainModifier? TerrainModifierCovered
    {
        get => terrainModifierCovered = GetNodeFromRefTable(terrainModifierCovered, terrainModifierCoveredFile) as CGameCtnDecorationTerrainModifier;
        set => terrainModifierCovered = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03038016>]
    public CGameCtnDecorationTerrainModifier? TerrainModifierBase
    {
        get => terrainModifierBase = GetNodeFromRefTable(terrainModifierBase, terrainModifierBaseFile) as CGameCtnDecorationTerrainModifier;
        set => terrainModifierBase = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03038017>]
    public string? DecorationZoneFrontierId { get => decorationZoneFrontierId; set => decorationZoneFrontierId = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03038017>(sinceVersion: 1)]
    public bool IsWaterOutsidePlayField { get => isWaterOutsidePlayField; set => isWaterOutsidePlayField = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03038018>]
    public CPlugGameSkin? VehicleFxSkin { get => vehicleFxSkin; set => vehicleFxSkin = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03038018>]
    public string? VehicleFxFolder { get => vehicleFxFolder; set => vehicleFxFolder = value; }

    [NodeMember(ExactName = "DecoAudio_Ambient")]
    [AppliedWithChunk<Chunk03038019>(sinceVersion: 1)]
    public CPlugSound? DecoAudioAmbient { get => decoAudioAmbient; set => decoAudioAmbient = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303801A>]
    public CGameCtnDecorationMaterialModifiers? DecoMaterialModifiers { get => decoMaterialModifiers; set => decoMaterialModifiers = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303801B>]
    public CGameCtnChallenge? DecoMap { get => decoMap; set => decoMap = value; }

    #region Constructors

    internal CGameCtnDecoration()
    {

    }

    #endregion

    #region Chunks

    #region 0x001 header chunk (LightMap)

    /// <summary>
    /// CGameCtnDecoration 0x001 header chunk (LightMap)
    /// </summary>
    [Chunk(0x03038001, "LightMap")]
    public class Chunk03038001 : HeaderChunk<CGameCtnDecoration>, IVersionable
    {
        private int version;
        
        public int U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Byte(ref version);
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x011 chunk (DecoSize)

    /// <summary>
    /// CGameCtnDecoration 0x011 chunk (DecoSize)
    /// </summary>
    [Chunk(0x03038011, "DecoSize")]
    public class Chunk03038011 : Chunk<CGameCtnDecoration>
    {
        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationSize>(ref n.decoSize, ref n.decoSizeFile);
        }
    }

    #endregion

    #region 0x012 chunk (DecoAudio)

    /// <summary>
    /// CGameCtnDecoration 0x012 chunk (DecoAudio)
    /// </summary>
    [Chunk(0x03038012, "DecoAudio")]
    public class Chunk03038012 : Chunk<CGameCtnDecoration>
    {
        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationAudio>(ref n.decoAudio, ref n.decoAudioFile);
        }
    }

    #endregion

    #region 0x013 chunk (DecoMood)

    /// <summary>
    /// CGameCtnDecoration 0x013 chunk (DecoMood)
    /// </summary>
    [Chunk(0x03038013, "DecoMood")]
    public class Chunk03038013 : Chunk<CGameCtnDecoration>
    {
        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationMood>(ref n.decoMood, ref n.decoMoodFile);
        }
    }

    #endregion

    #region 0x014 chunk (DecoratorSolidWarp)

    /// <summary>
    /// CGameCtnDecoration 0x014 chunk (DecoratorSolidWarp)
    /// </summary>
    [Chunk(0x03038014, "DecoratorSolidWarp")]
    public class Chunk03038014 : Chunk<CGameCtnDecoration>
    {
        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugDecoratorSolid>(ref n.decoratorSolidWarp);
        }
    }

    #endregion

    #region 0x015 chunk (TerrainModifierCovered)

    /// <summary>
    /// CGameCtnDecoration 0x015 chunk (TerrainModifierCovered)
    /// </summary>
    [Chunk(0x03038015, "TerrainModifierCovered")]
    public class Chunk03038015 : Chunk<CGameCtnDecoration>
    {
        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationTerrainModifier>(ref n.terrainModifierCovered, ref n.terrainModifierCoveredFile);
        }
    }

    #endregion

    #region 0x016 chunk (TerrainModifierBase)

    /// <summary>
    /// CGameCtnDecoration 0x016 chunk (TerrainModifierBase)
    /// </summary>
    [Chunk(0x03038016, "TerrainModifierBase")]
    public class Chunk03038016 : Chunk<CGameCtnDecoration>
    {
        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationTerrainModifier>(ref n.terrainModifierBase, ref n.terrainModifierBaseFile);
        }
    }

    #endregion

    #region 0x017 chunk

    /// <summary>
    /// CGameCtnDecoration 0x017 chunk
    /// </summary>
    [Chunk(0x03038017)]
    public class Chunk03038017 : Chunk<CGameCtnDecoration>, IVersionable
    {
        private int version = 1;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Id(ref n.decorationZoneFrontierId);

            if (version >= 1)
            {
                rw.Boolean(ref n.isWaterOutsidePlayField);
            }
        }
    }

    #endregion

    #region 0x018 chunk

    /// <summary>
    /// CGameCtnDecoration 0x018 chunk
    /// </summary>
    [Chunk(0x03038018)]
    public class Chunk03038018 : Chunk<CGameCtnDecoration>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CPlugGameSkin>(ref n.vehicleFxSkin);
            rw.String(ref n.vehicleFxFolder);
        }
    }

    #endregion

    #region 0x019 chunk

    /// <summary>
    /// CGameCtnDecoration 0x019 chunk
    /// </summary>
    [Chunk(0x03038019)]
    public class Chunk03038019 : Chunk<CGameCtnDecoration>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CGameCtnDecorationAudio>(ref n.decoAudio, ref n.decoAudioFile);

            if (version >= 1)
            {
                rw.NodeRef<CPlugSound>(ref n.decoAudioAmbient);
            }
        }
    }

    #endregion

    #region 0x01A chunk

    /// <summary>
    /// CGameCtnDecoration 0x01A chunk
    /// </summary>
    [Chunk(0x0303801A)]
    public class Chunk0303801A : Chunk<CGameCtnDecoration>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CGameCtnDecorationMaterialModifiers>(ref n.decoMaterialModifiers);
        }
    }

    #endregion

    #region 0x01B chunk

    /// <summary>
    /// CGameCtnDecoration 0x01B chunk
    /// </summary>
    [Chunk(0x0303801B)]
    public class Chunk0303801B : Chunk<CGameCtnDecoration>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }
        
        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CGameCtnChallenge>(ref n.decoMap, ref n.decoMapFile);

            if (version >= 1)
            {
                // Deco.LightMap.Gbx of the DecoMap like from cache - zip with LightMapCache.Gbx inside
                // As its a zip, it shouldn't be a node reference
                rw.NodeRef(ref n.decoMapLightMap, ref n.decoMapLightMapFile);
            }
        }
    }

    #endregion

    #endregion
}
