namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03122000</remarks>
[Class(0x03122000)]
public partial class CGameCtnBlockInfoMobil : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03122000;




    private CGameCtnSolidDecals[]? solidDecals;
    [AppliedWithChunk<Chunk03122002>]
    public CGameCtnSolidDecals[]? SolidDecals { get => solidDecals; set => solidDecals = value; }

    private CSceneMobil? oldMobil;
    [AppliedWithChunk<Chunk03122003>]
    [AppliedWithChunk<Chunk03122003>]
    public CSceneMobil? OldMobil { get => oldMobilFile?.GetNode(ref oldMobil) ?? oldMobil; set => oldMobil = value; }
    private Components.GbxRefTableFile? oldMobilFile;
    public Components.GbxRefTableFile? OldMobilFile { get => oldMobilFile; set => oldMobilFile = value; }
    public CSceneMobil? GetOldMobil(GbxReadSettings settings = default, bool exceptions = false) => oldMobilFile?.GetNode(ref oldMobil, settings, exceptions) ?? oldMobil;

    private int solidFrequency;
    [AppliedWithChunk<Chunk03122003>]
    public int SolidFrequency { get => solidFrequency; set => solidFrequency = value; }

    private bool hasGeomTransformation;
    [AppliedWithChunk<Chunk03122003>]
    public bool HasGeomTransformation { get => hasGeomTransformation; set => hasGeomTransformation = value; }

    private Vec3 geomTranslation;
    [AppliedWithChunk<Chunk03122003>]
    public Vec3 GeomTranslation { get => geomTranslation; set => geomTranslation = value; }

    private Vec3 geomRotation;
    [AppliedWithChunk<Chunk03122003>]
    public Vec3 GeomRotation { get => geomRotation; set => geomRotation = value; }

    private CPlugSolid? solidFid;
    [AppliedWithChunk<Chunk03122003>]
    public CPlugSolid? SolidFid { get => solidFidFile?.GetNode(ref solidFid) ?? solidFid; set => solidFid = value; }
    private Components.GbxRefTableFile? solidFidFile;
    public Components.GbxRefTableFile? SolidFidFile { get => solidFidFile; set => solidFidFile = value; }
    public CPlugSolid? GetSolidFid(GbxReadSettings settings = default, bool exceptions = false) => solidFidFile?.GetNode(ref solidFid, settings, exceptions) ?? solidFid;

    private CPlugPrefab? prefabFid;
    [AppliedWithChunk<Chunk03122003>]
    public CPlugPrefab? PrefabFid { get => prefabFidFile?.GetNode(ref prefabFid) ?? prefabFid; set => prefabFid = value; }
    private Components.GbxRefTableFile? prefabFidFile;
    public Components.GbxRefTableFile? PrefabFidFile { get => prefabFidFile; set => prefabFidFile = value; }
    public CPlugPrefab? GetPrefabFid(GbxReadSettings settings = default, bool exceptions = false) => prefabFidFile?.GetNode(ref prefabFid, settings, exceptions) ?? prefabFid;

    private CPlugSolid? oldSolidAggreg;
    [AppliedWithChunk<Chunk03122003>]
    public CPlugSolid? OldSolidAggreg { get => oldSolidAggregFile?.GetNode(ref oldSolidAggreg) ?? oldSolidAggreg; set => oldSolidAggreg = value; }
    private Components.GbxRefTableFile? oldSolidAggregFile;
    public Components.GbxRefTableFile? OldSolidAggregFile { get => oldSolidAggregFile; set => oldSolidAggregFile = value; }
    public CPlugSolid? GetOldSolidAggreg(GbxReadSettings settings = default, bool exceptions = false) => oldSolidAggregFile?.GetNode(ref oldSolidAggreg, settings, exceptions) ?? oldSolidAggreg;

    private CPlugPath? railPath;
    [AppliedWithChunk<Chunk03122003>]
    public CPlugPath? RailPath { get => railPathFile?.GetNode(ref railPath) ?? railPath; set => railPath = value; }
    private Components.GbxRefTableFile? railPathFile;
    public Components.GbxRefTableFile? RailPathFile { get => railPathFile; set => railPathFile = value; }
    public CPlugPath? GetRailPath(GbxReadSettings settings = default, bool exceptions = false) => railPathFile?.GetNode(ref railPath, settings, exceptions) ?? railPath;

    private CPlugPath? trafficPath;
    [AppliedWithChunk<Chunk03122003>]
    public CPlugPath? TrafficPath { get => trafficPathFile?.GetNode(ref trafficPath) ?? trafficPath; set => trafficPath = value; }
    private Components.GbxRefTableFile? trafficPathFile;
    public Components.GbxRefTableFile? TrafficPathFile { get => trafficPathFile; set => trafficPathFile = value; }
    public CPlugPath? GetTrafficPath(GbxReadSettings settings = default, bool exceptions = false) => trafficPathFile?.GetNode(ref trafficPath, settings, exceptions) ?? trafficPath;

    private CPlugRoadChunk[]? roadChunks;
    [AppliedWithChunk<Chunk03122003>]
    [AppliedWithChunk<Chunk03122003>]
    public CPlugRoadChunk[]? RoadChunks { get => roadChunks; set => roadChunks = value; }

    private CPlugPath? citizenNetworkPath;
    /// <summary>
    /// QuestMania confirmed
    /// </summary>
    [AppliedWithChunk<Chunk03122003>]
    public CPlugPath? CitizenNetworkPath { get => citizenNetworkPath; set => citizenNetworkPath = value; }

    private CMwNod? vFXs;
    [AppliedWithChunk<Chunk03122003>]
    public CMwNod? VFXs { get => vFXs; set => vFXs = value; }

    private CGameCtnBlockInfoMobilLink[]? dynaLinks;
    [AppliedWithChunk<Chunk03122004>]
    public CGameCtnBlockInfoMobilLink[]? DynaLinks { get => dynaLinks; set => dynaLinks = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnBlockInfoMobil"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnBlockInfoMobil() { }


    /// <summary>
    /// CGameCtnBlockInfoMobil 0x002 chunk
    /// </summary>
    [Chunk(0x03122002)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk03122002 : Chunk<CGameCtnBlockInfoMobil>
    {
        /// <inheritdoc />
        public override uint Id => 0x03122002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public int U01;

        public override void ReadWrite(CGameCtnBlockInfoMobil n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CGameCtnSolidDecals>(ref n.solidDecals!);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoMobil 0x003 chunk
    /// </summary>
    [Chunk(0x03122003)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4, 0, 3, 6)]
    public partial class Chunk03122003 : Chunk<CGameCtnBlockInfoMobil>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03122003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }

        public bool U01;
        public CPlugPolyLine3[]? U02;
        public bool U03;
        public CMwNod? U04;
        public int U05;
        public int U06;
        public int U07;
        public CMwNod[]? U08;
        public byte U09;
        public float U10;
        public float U11;
        public Iso4 U12;
        public Vec3 U13;
        public Vec3 U14;
        public float U15;
        public CMwNod[]? U16;
        public CMwNod? U17;

        public override void ReadWrite(CGameCtnBlockInfoMobil n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 1)
            {
                rw.Boolean(ref U01);
                if (Version == 0)
                {
                    rw.NodeRef<CSceneMobil>(ref n.oldMobil, ref n.oldMobilFile);
                }
            }
            rw.Int32(ref n.solidFrequency);
            if (Version >= 1)
            {
                rw.Boolean(ref n.hasGeomTransformation, asByte: true);
                if (n.HasGeomTransformation)
                {
                    rw.Vec3(ref n.geomTranslation);
                    rw.Vec3(ref n.geomRotation);
                }
                if (Version >= 2)
                {
                    rw.NodeRef<CPlugSolid>(ref n.solidFid, ref n.solidFidFile);
                    if (n.SolidFid==null&&n.SolidFidFile==null)
                    {
                        rw.NodeRef<CSceneMobil>(ref n.oldMobil);
                    }
                    if (Version >= 14)
                    {
                        rw.NodeRef<CPlugPrefab>(ref n.prefabFid, ref n.prefabFidFile);
                    }
                    if (Version >= 3)
                    {
                        rw.NodeRef<CPlugSolid>(ref n.oldSolidAggreg, ref n.oldSolidAggregFile);
                        if (Version >= 4)
                        {
                            if (Version >= 5)
                            {
                                if (Version == 5)
                                {
                                    rw.ArrayNodeRef<CPlugPolyLine3>(ref U02!);
                                }
                                if (Version <= 10)
                                {
                                    rw.Boolean(ref U03);
                                }
                                if (Version >= 6)
                                {
                                    rw.NodeRef<CPlugPath>(ref n.railPath, ref n.railPathFile);
                                    if (Version >= 7)
                                    {
                                        if (Version <= 22)
                                        {
                                            rw.NodeRef<CPlugPath>(ref n.trafficPath, ref n.trafficPathFile);
                                        }
                                        if (Version >= 15)
                                        {
                                            rw.NodeRef<CMwNod>(ref U04);
                                        }
                                        if (Version >= 8)
                                        {
                                            if (Version <= 12)
                                            {
                                                rw.Int32(ref U05);
                                            }
                                            if (Version >= 9)
                                            {
                                                if (Version <= 22)
                                                {
                                                    rw.ArrayNodeRef_deprec<CPlugRoadChunk>(ref n.roadChunks!);
                                                }
                                                if (Version >= 23)
                                                {
                                                    rw.ArrayNodeRef<CPlugRoadChunk>(ref n.roadChunks!);
                                                }
                                                if (Version >= 10)
                                                {
                                                    if (Version <= 12)
                                                    {
                                                        rw.Int32(ref U06);
                                                        if (Version == 12)
                                                        {
                                                            rw.Int32(ref U07);
                                                        }
                                                    }
                                                    if (Version >= 16)
                                                    {
                                                        if (Version <= 22)
                                                        {
                                                            rw.NodeRef<CPlugPath>(ref n.citizenNetworkPath); // QuestMania confirmed
                                                        }
                                                        if (Version >= 17)
                                                        {
                                                            rw.ArrayNodeRef<CMwNod>(ref U08!);
                                                            if (Version >= 18)
                                                            {
                                                                rw.NodeRef<CMwNod>(ref n.vFXs);
                                                                rw.Byte(ref U09);
                                                                if (U09==0)
                                                                {
                                                                    rw.Single(ref U10);
                                                                }
                                                                if (U09==1)
                                                                {
                                                                    rw.Single(ref U11);
                                                                }
                                                                if (Version == 18)
                                                                {
                                                                    rw.Iso4(ref U12);
                                                                }
                                                                if (Version >= 19)
                                                                {
                                                                    rw.Vec3(ref U13);
                                                                    rw.Vec3(ref U14);
                                                                }
                                                                rw.Single(ref U15);
                                                                if (Version >= 20)
                                                                {
                                                                    rw.ArrayNodeRef<CMwNod>(ref U16!);
                                                                    if (Version >= 21)
                                                                    {
                                                                        rw.NodeRef<CMwNod>(ref U17);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoMobil 0x004 chunk
    /// </summary>
    [Chunk(0x03122004)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk03122004 : Chunk<CGameCtnBlockInfoMobil>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03122004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnBlockInfoMobil n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayNodeRef_deprec<CGameCtnBlockInfoMobilLink>(ref n.dynaLinks!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03122002 => new Chunk03122002(),
        0x03122003 => new Chunk03122003(),
        0x03122004 => new Chunk03122004(),
        _ => base.NewChunk(chunkId),
    };
}
