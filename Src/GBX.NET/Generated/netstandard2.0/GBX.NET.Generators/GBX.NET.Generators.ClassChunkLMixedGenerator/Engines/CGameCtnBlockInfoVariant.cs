namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0315B000</remarks>
[Class(0x0315B000)]
public partial class CGameCtnBlockInfoVariant : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0315B000;




    private CGameCtnBlockUnitInfo[]? blockUnitModels;
    [AppliedWithChunk<Chunk0315B000>]
    [AppliedWithChunk<Chunk0315B008>]
    public CGameCtnBlockUnitInfo[]? BlockUnitModels { get => blockUnitModels; set => blockUnitModels = value; }

    private bool hasManualSymmetryH;
    [AppliedWithChunk<Chunk0315B000>]
    [AppliedWithChunk<Chunk0315B008>]
    public bool HasManualSymmetryH { get => hasManualSymmetryH; set => hasManualSymmetryH = value; }

    private bool hasManualSymmetryV;
    [AppliedWithChunk<Chunk0315B000>]
    [AppliedWithChunk<Chunk0315B008>]
    public bool HasManualSymmetryV { get => hasManualSymmetryV; set => hasManualSymmetryV = value; }

    private bool hasManualSymmetryD1;
    [AppliedWithChunk<Chunk0315B000>]
    [AppliedWithChunk<Chunk0315B008>]
    public bool HasManualSymmetryD1 { get => hasManualSymmetryD1; set => hasManualSymmetryD1 = value; }

    private bool hasManualSymmetryD2;
    [AppliedWithChunk<Chunk0315B000>]
    [AppliedWithChunk<Chunk0315B008>]
    public bool HasManualSymmetryD2 { get => hasManualSymmetryD2; set => hasManualSymmetryD2 = value; }

    private Iso4 spawnLoc;
    [AppliedWithChunk<Chunk0315B000>]
    public Iso4 SpawnLoc { get => spawnLoc; set => spawnLoc = value; }

    private string? name;
    [AppliedWithChunk<Chunk0315B000>]
    [AppliedWithChunk<Chunk0315B008>]
    public string? Name { get => name; set => name = value; }

    private EMultiDir multiDir;
    [AppliedWithChunk<Chunk0315B002>]
    public EMultiDir MultiDir { get => multiDir; set => multiDir = value; }

    private int symmetricalVariantIndex;
    [AppliedWithChunk<Chunk0315B003>]
    public int SymmetricalVariantIndex { get => symmetricalVariantIndex; set => symmetricalVariantIndex = value; }

    private ECardinalDir cardinalDir;
    [AppliedWithChunk<Chunk0315B003>]
    [AppliedWithChunk<Chunk0315B003>]
    public ECardinalDir CardinalDir { get => cardinalDir; set => cardinalDir = value; }

    private EVariantBaseType variantBaseType;
    [AppliedWithChunk<Chunk0315B003>]
    public EVariantBaseType VariantBaseType { get => variantBaseType; set => variantBaseType = value; }

    private byte noPillarBelowIndex;
    [AppliedWithChunk<Chunk0315B003>]
    public byte NoPillarBelowIndex { get => noPillarBelowIndex; set => noPillarBelowIndex = value; }

    private CMwNod? screenInteractionTriggerSolid;
    [AppliedWithChunk<Chunk0315B006>]
    public CMwNod? ScreenInteractionTriggerSolid { get => screenInteractionTriggerSolidFile?.GetNode(ref screenInteractionTriggerSolid) ?? screenInteractionTriggerSolid; set => screenInteractionTriggerSolid = value; }
    private Components.GbxRefTableFile? screenInteractionTriggerSolidFile;
    public Components.GbxRefTableFile? ScreenInteractionTriggerSolidFile { get => screenInteractionTriggerSolidFile; set => screenInteractionTriggerSolidFile = value; }
    public CMwNod? GetScreenInteractionTriggerSolid(GbxReadSettings settings = default, bool exceptions = false) => screenInteractionTriggerSolidFile?.GetNode(ref screenInteractionTriggerSolid, settings, exceptions) ?? screenInteractionTriggerSolid;

    private CMwNod? waypointTriggerSolid;
    [AppliedWithChunk<Chunk0315B006>]
    public CMwNod? WaypointTriggerSolid { get => waypointTriggerSolidFile?.GetNode(ref waypointTriggerSolid) ?? waypointTriggerSolid; set => waypointTriggerSolid = value; }
    private Components.GbxRefTableFile? waypointTriggerSolidFile;
    public Components.GbxRefTableFile? WaypointTriggerSolidFile { get => waypointTriggerSolidFile; set => waypointTriggerSolidFile = value; }
    public CMwNod? GetWaypointTriggerSolid(GbxReadSettings settings = default, bool exceptions = false) => waypointTriggerSolidFile?.GetNode(ref waypointTriggerSolid, settings, exceptions) ?? waypointTriggerSolid;

    private CGameGateModel? gate;
    [AppliedWithChunk<Chunk0315B006>]
    public CGameGateModel? Gate { get => gateFile?.GetNode(ref gate) ?? gate; set => gate = value; }
    private Components.GbxRefTableFile? gateFile;
    public Components.GbxRefTableFile? GateFile { get => gateFile; set => gateFile = value; }
    public CGameGateModel? GetGate(GbxReadSettings settings = default, bool exceptions = false) => gateFile?.GetNode(ref gate, settings, exceptions) ?? gate;

    private CGameTeleporterModel? teleporter;
    [AppliedWithChunk<Chunk0315B006>]
    public CGameTeleporterModel? Teleporter { get => teleporter; set => teleporter = value; }

    private CGameTurbineModel? turbine;
    [AppliedWithChunk<Chunk0315B006>]
    public CGameTurbineModel? Turbine { get => turbineFile?.GetNode(ref turbine) ?? turbine; set => turbine = value; }
    private Components.GbxRefTableFile? turbineFile;
    public Components.GbxRefTableFile? TurbineFile { get => turbineFile; set => turbineFile = value; }
    public CGameTurbineModel? GetTurbine(GbxReadSettings settings = default, bool exceptions = false) => turbineFile?.GetNode(ref turbine, settings, exceptions) ?? turbine;

    private CPlugFlockModel? flockModel;
    [AppliedWithChunk<Chunk0315B006>]
    public CPlugFlockModel? FlockModel { get => flockModelFile?.GetNode(ref flockModel) ?? flockModel; set => flockModel = value; }
    private Components.GbxRefTableFile? flockModelFile;
    public Components.GbxRefTableFile? FlockModelFile { get => flockModelFile; set => flockModelFile = value; }
    public CPlugFlockModel? GetFlockModel(GbxReadSettings settings = default, bool exceptions = false) => flockModelFile?.GetNode(ref flockModel, settings, exceptions) ?? flockModel;

    private FlockEmitterState? flockEmmiter;
    [AppliedWithChunk<Chunk0315B006>]
    public FlockEmitterState? FlockEmmiter { get => flockEmmiter; set => flockEmmiter = value; }

    private CGameSpawnModel? spawnModel;
    [AppliedWithChunk<Chunk0315B006>]
    public CGameSpawnModel? SpawnModel { get => spawnModelFile?.GetNode(ref spawnModel) ?? spawnModel; set => spawnModel = value; }
    private Components.GbxRefTableFile? spawnModelFile;
    public Components.GbxRefTableFile? SpawnModelFile { get => spawnModelFile; set => spawnModelFile = value; }
    public CGameSpawnModel? GetSpawnModel(GbxReadSettings settings = default, bool exceptions = false) => spawnModelFile?.GetNode(ref spawnModel, settings, exceptions) ?? spawnModel;

    private CPlugEntitySpawner[]? entitySpawners;
    [AppliedWithChunk<Chunk0315B006>]
    public CPlugEntitySpawner[]? EntitySpawners { get => entitySpawners; set => entitySpawners = value; }

    private CPlugProbe? probe;
    [AppliedWithChunk<Chunk0315B007>]
    public CPlugProbe? Probe { get => probeFile?.GetNode(ref probe) ?? probe; set => probe = value; }
    private Components.GbxRefTableFile? probeFile;
    public Components.GbxRefTableFile? ProbeFile { get => probeFile; set => probeFile = value; }
    public CPlugProbe? GetProbe(GbxReadSettings settings = default, bool exceptions = false) => probeFile?.GetNode(ref probe, settings, exceptions) ?? probe;

    private Vec3 spawnTrans;
    [AppliedWithChunk<Chunk0315B008>]
    public Vec3 SpawnTrans { get => spawnTrans; set => spawnTrans = value; }

    private float spawnYaw;
    [AppliedWithChunk<Chunk0315B008>]
    public float SpawnYaw { get => spawnYaw; set => spawnYaw = value; }

    private float spawnPitch;
    [AppliedWithChunk<Chunk0315B008>]
    public float SpawnPitch { get => spawnPitch; set => spawnPitch = value; }

    private CGameObjectPhyCompoundModel? compoundModel;
    [AppliedWithChunk<Chunk0315B00A>]
    public CGameObjectPhyCompoundModel? CompoundModel { get => compoundModel; set => compoundModel = value; }

    private Iso4 compoundLoc;
    [AppliedWithChunk<Chunk0315B00A>]
    public Iso4 CompoundLoc { get => compoundLoc; set => compoundLoc = value; }

    private WaterVolume[]? waterVolumes;
    [AppliedWithChunk<Chunk0315B00B>]
    public WaterVolume[]? WaterVolumes { get => waterVolumes; set => waterVolumes = value; }


    /// <summary>
    /// CGameCtnBlockInfoVariant 0x000 chunk
    /// </summary>
    [Chunk(0x0315B000)]
    [ChunkGameVersion(GameVersion.MP3)]
    public partial class Chunk0315B000 : Chunk<CGameCtnBlockInfoVariant>
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B000;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3;

        public int U01;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CGameCtnBlockUnitInfo>(ref n.blockUnitModels!);
            rw.Int32(ref U01);
            rw.Boolean(ref n.hasManualSymmetryH);
            rw.Boolean(ref n.hasManualSymmetryV);
            rw.Boolean(ref n.hasManualSymmetryD1);
            rw.Boolean(ref n.hasManualSymmetryD2);
            rw.Iso4(ref n.spawnLoc);
            rw.String(ref n.name);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoVariant 0x002 chunk
    /// </summary>
    [Chunk(0x0315B002)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0315B002 : Chunk<CGameCtnBlockInfoVariant>
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CGameCtnBlockInfoVariant n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EMultiDir>(ref n.multiDir);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoVariant 0x003 chunk
    /// </summary>
    [Chunk(0x0315B003)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4, 2, 2, 2)]
    public partial class Chunk0315B003 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnBlockInfoVariant n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.symmetricalVariantIndex);
            if (Version == 0)
            {
                rw.EnumInt32<ECardinalDir>(ref n.cardinalDir);
            }
            if (Version >= 1)
            {
                rw.EnumByte<ECardinalDir>(ref n.cardinalDir);
                rw.EnumByte<EVariantBaseType>(ref n.variantBaseType);
                if (Version >= 2)
                {
                    rw.Byte(ref n.noPillarBelowIndex);
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoVariant 0x004 chunk
    /// </summary>
    [Chunk(0x0315B004)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0315B004 : Chunk<CGameCtnBlockInfoVariant>
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public short U01;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GbxReaderWriter rw)
        {
            rw.Int16(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoVariant 0x005 chunk
    /// </summary>
    [Chunk(0x0315B005)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4, 0, 2, 2)]
    public partial class Chunk0315B005 : Chunk<CGameCtnBlockInfoVariant>
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B005;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

    }

    /// <summary>
    /// CGameCtnBlockInfoVariant 0x006 chunk
    /// </summary>
    [Chunk(0x0315B006)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4, 6, 8, 10)]
    public partial class Chunk0315B006 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B006;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }

        public CMwNod? U01;
        /// <summary>
        /// WaypointTriggerShape?
        /// </summary>
        public CMwNod? U02;
        public Components.GbxRefTableFile? U02File;
        /// <summary>
        /// ScreenInteractionTriggerShape?
        /// </summary>
        public CMwNod? U03;
        public Components.GbxRefTableFile? U03File;
        public int U04;
        public CGameCaptureZoneModel? U05;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 8)
            {
                rw.NodeRef<CMwNod>(ref U01);
            }
            rw.NodeRef<CMwNod>(ref n.screenInteractionTriggerSolid, ref n.screenInteractionTriggerSolidFile);
            rw.NodeRef<CMwNod>(ref n.waypointTriggerSolid, ref n.waypointTriggerSolidFile);
            if (Version >= 11)
            {
                rw.NodeRef<CMwNod>(ref U02, ref U02File); // WaypointTriggerShape?
                rw.NodeRef<CMwNod>(ref U03, ref U03File); // ScreenInteractionTriggerShape?
            }
            if (Version <= 8)
            {
                rw.Int32(ref U04);
            }
            if (Version >= 2)
            {
                rw.NodeRef<CGameGateModel>(ref n.gate, ref n.gateFile);
                if (Version >= 3)
                {
                    rw.NodeRef<CGameTeleporterModel>(ref n.teleporter);
                    if (Version >= 5)
                    {
                        rw.NodeRef<CGameCaptureZoneModel>(ref U05);
                        if (Version >= 6)
                        {
                            rw.NodeRef<CGameTurbineModel>(ref n.turbine, ref n.turbineFile);
                            if (Version >= 7)
                            {
                                rw.NodeRef<CPlugFlockModel>(ref n.flockModel, ref n.flockModelFile);
                                if (n.FlockModel!=null||n.FlockModelFile!=null)
                                {
                                    rw.ReadableWritable<FlockEmitterState>(ref n.flockEmmiter);
                                }
                                if (Version >= 8)
                                {
                                    rw.NodeRef<CGameSpawnModel>(ref n.spawnModel, ref n.spawnModelFile);
                                    if (Version >= 10)
                                    {
                                        rw.ArrayNodeRef<CPlugEntitySpawner>(ref n.entitySpawners!);
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
    /// CGameCtnBlockInfoVariant 0x007 chunk
    /// </summary>
    [Chunk(0x0315B007)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4, 0, 0, 0)]
    public partial class Chunk0315B007 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B007;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnBlockInfoVariant n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugProbe>(ref n.probe, ref n.probeFile);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoVariant 0x008 chunk
    /// </summary>
    [Chunk(0x0315B008)]
    [ChunkGameVersion(GameVersion.TMT | GameVersion.MP4, 0, 1)]
    public partial class Chunk0315B008 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B008;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }

        public int U01;
        /// <summary>
        /// SpawnTrans, SpawnYaw, SpawnPitch, SpawnRoll - I imagine
        /// </summary>
        public BoxAligned U02;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayNodeRef<CGameCtnBlockUnitInfo>(ref n.blockUnitModels!);
            rw.Int32(ref U01);
            rw.Boolean(ref n.hasManualSymmetryH);
            rw.Boolean(ref n.hasManualSymmetryV);
            rw.Boolean(ref n.hasManualSymmetryD1);
            rw.Boolean(ref n.hasManualSymmetryD2);
            if (Version <= 1)
            {
                rw.Vec3(ref n.spawnTrans);
                rw.Single(ref n.spawnYaw);
                rw.Single(ref n.spawnPitch);
            }
            if (Version >= 2)
            {
                rw.BoxAligned(ref U02); // SpawnTrans, SpawnYaw, SpawnPitch, SpawnRoll - I imagine
            }
            rw.String(ref n.name);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoVariant 0x009 chunk
    /// </summary>
    [Chunk(0x0315B009)]
    [ChunkGameVersion(GameVersion.MP4, 1)]
    public partial class Chunk0315B009 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B009;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4;

        public int Version { get; set; }

        public PlacedPillarParam[]? U01;
        public ReplacedPillarParam[]? U02;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<PlacedPillarParam>(ref U01!);
            if (Version >= 1)
            {
                rw.ArrayReadableWritable<ReplacedPillarParam>(ref U02!);
            }
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoVariant 0x00A chunk
    /// </summary>
    [Chunk(0x0315B00A)]
    [ChunkGameVersion(GameVersion.MP4, 2)]
    public partial class Chunk0315B00A : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B00A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4;

        public int Version { get; set; }

        public CMwNod? U01;
        public CMwNod? U02;
        public Iso4? U03;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 1)
            {
                rw.NodeRef<CMwNod>(ref U01);
                rw.NodeRef<CMwNod>(ref U02);
                if (Version >= 1)
                {
                    rw.Iso4(ref U03);
                }
            }
            if (Version >= 2)
            {
                rw.NodeRef<CGameObjectPhyCompoundModel>(ref n.compoundModel);
                if (Version <= 2)
                {
                    rw.Iso4(ref n.compoundLoc);
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoVariant 0x00B chunk
    /// </summary>
    [Chunk(0x0315B00B)]
    [ChunkGameVersion(GameVersion.MP4, 1)]
    public partial class Chunk0315B00B : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B00B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnBlockInfoVariant n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<WaterVolume>(ref n.waterVolumes!, version: Version);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoVariant 0x00C chunk
    /// </summary>
    [Chunk(0x0315B00C)]
    [ChunkGameVersion(GameVersion.MP4, 1)]
    public partial class Chunk0315B00C : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B00C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            if (U01>0)
            {
                throw new ("");
            }
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoVariant 0x00D chunk
    /// </summary>
    [Chunk(0x0315B00D)]
    public partial class Chunk0315B00D : Chunk<CGameCtnBlockInfoVariant>
    {
        /// <inheritdoc />
        public override uint Id => 0x0315B00D;

        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }


    public sealed partial class ReplacedPillarParam : PlacedPillarParam, IReadableWritable
    {

        private byte u06;
        public byte U06 { get => u06; set => u06 = value; }

        public override void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            base.ReadWrite(rw, v);
            rw.Byte(ref u06);
        }
    }

    public sealed partial class FlockEmitterState : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private float u01;
        /// <summary>
        /// 10 is a clear state
        /// </summary>
        public float U01 { get => u01; set => u01 = value; }

        private float u02;
        /// <summary>
        /// 0x40000000 is a clear state
        /// </summary>
        public float U02 { get => u02; set => u02 = value; }

        private int u03;
        /// <summary>
        /// 5 is a clear state
        /// </summary>
        public int U03 { get => u03; set => u03 = value; }

        private bool u04;
        public bool U04 { get => u04; set => u04 = value; }

        private bool u05;
        /// <summary>
        /// true is a clear state
        /// </summary>
        public bool U05 { get => u05; set => u05 = value; }

        private Mat3? matrix;
        public Mat3? Matrix { get => matrix; set => matrix = value; }

        private Vec3 position;
        public Vec3 Position { get => position; set => position = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.Single(ref u01); // 10 is a clear state
            rw.Single(ref u02); // 0x40000000 is a clear state
            rw.Int32(ref u03); // 5 is a clear state
            rw.Boolean(ref u04);
            rw.Boolean(ref u05); // true is a clear state
            if (Version>=1)
            {
                rw.Mat3(ref matrix);
            }
            rw.Vec3(ref position);
        }
    }

    public sealed partial class WaterVolume : IReadableWritable
    {

        private BoxInt3[]? u01;
        public BoxInt3[]? U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

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

        private float u08;
        public float U08 { get => u08; set => u08 = value; }

        private string? u09;
        public string? U09 { get => u09; set => u09 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Array<BoxInt3>(ref u01!);
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
            if (v >= 1)
            {
                rw.Id(ref u09);
            }
        }
    }

    public partial class PlacedPillarParam : IReadableWritable
    {

        private CMwNod? u01;
        public CMwNod? U01 { get => u01File?.GetNode(ref u01) ?? u01; set => u01 = value; }
        private Components.GbxRefTableFile? u01File;
        public Components.GbxRefTableFile? U01File { get => u01File; set => u01File = value; }
        public CMwNod? GetU01(GbxReadSettings settings = default, bool exceptions = false) => u01File?.GetNode(ref u01, settings, exceptions) ?? u01;

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        public virtual void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.NodeRef<CMwNod>(ref u01, ref u01File);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
        }
    }


    public enum ECardinalDir
    {
        North,
        East,
        South,
        West,
    }

    public enum EMultiDir
    {
        SameDir,
        SymmetricalDirs,
        AllDir,
        OpposedDirOnly,
        PerpendicularDirsOnly,
        NextDirOnly,
        PreviousDirOnly,
    }

    public enum EVariantBaseType
    {
        Inherit,
        None,
        Conductor,
        Generator,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0315B000 => new Chunk0315B000(),
        0x0315B002 => new Chunk0315B002(),
        0x0315B003 => new Chunk0315B003(),
        0x0315B004 => new Chunk0315B004(),
        0x0315B005 => new Chunk0315B005(),
        0x0315B006 => new Chunk0315B006(),
        0x0315B007 => new Chunk0315B007(),
        0x0315B008 => new Chunk0315B008(),
        0x0315B009 => new Chunk0315B009(),
        0x0315B00A => new Chunk0315B00A(),
        0x0315B00B => new Chunk0315B00B(),
        0x0315B00C => new Chunk0315B00C(),
        0x0315B00D => new Chunk0315B00D(),
        _ => base.NewChunk(chunkId),
    };
}
