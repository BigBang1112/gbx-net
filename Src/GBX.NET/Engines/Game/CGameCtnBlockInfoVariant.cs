namespace GBX.NET.Engines.Game;

[Node(0x0315B000)]
public abstract class CGameCtnBlockInfoVariant : CMwNod
{
    public enum EMultiDir
    {
        SameDir,
        SymmetricalDirs,
        AllDir,
        OpposedDirOnly,
        PerpendicularDirsOnly,
        NextDirOnly,
        PreviousDirOnly
    }

    public enum ECardinalDir
    {
        North,
        East,
        South,
        West
    }

    public enum EVariantBaseType
    {
        Inherit,
        None,
        Conductor,
        Generator
    }

    private CGameCtnBlockInfoMobil?[][]? mobils;
    private EMultiDir multiDir;
    private int symmetricalVariantIndex;
    private ECardinalDir cardinalDir;
    private EVariantBaseType variantBaseType;
    private byte noPillarBelowIndex;
    private CMwNod? screenInteractionTriggerSolid;
    private GameBoxRefTable.File? screenInteractionTriggerSolidFile;
    private CMwNod? waypointTriggerSolid;
    private GameBoxRefTable.File? waypointTriggerSolidFile;
    private CGameGateModel? gate;
    private CGameTeleporterModel? teleporter;
    private CGameTurbineModel? turbine;
    private CPlugFlockModel? flockModel;
    private GameBoxRefTable.File? flockModelFile;
    private FlockEmitterState? flockEmmiter;
    private CGameSpawnModel? spawnModel;
    private GameBoxRefTable.File? spawnModelFile;
    private CPlugEntitySpawner?[]? entitySpawners;
    private CPlugProbe? probe;
    private CGameCtnBlockUnitInfo?[]? blockUnitModels;
    private bool hasManualSymmetryH;
    private bool hasManualSymmetryV;
    private bool hasManualSymmetryD1;
    private bool hasManualSymmetryD2;
    private Vec3 spawnTrans;
    private float spawnYaw;
    private float spawnPitch;
    private string? name;
    private CGameObjectPhyCompoundModel? compoundModel;
    private Iso4 compoundLoc;
    private WaterArchive[]? waterVolumes;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B002>]
    public EMultiDir MultiDir { get => multiDir; set => multiDir = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B003>]
    public int SymmetricalVariantIndex { get => symmetricalVariantIndex; set => symmetricalVariantIndex = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B003>]
    public ECardinalDir CardinalDir { get => cardinalDir; set => cardinalDir = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B003>(sinceVersion: 1)]
    public EVariantBaseType VariantBaseType { get => variantBaseType; set => variantBaseType = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B003>(sinceVersion: 2)]
    public byte NoPillarBelowIndex { get => noPillarBelowIndex; set => noPillarBelowIndex = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0315B005>]
    public CGameCtnBlockInfoMobil?[][]? Mobils { get => mobils; set => mobils = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B006>]
    public CMwNod? ScreenInteractionTriggerSolid
    {
        get => screenInteractionTriggerSolid = GetNodeFromRefTable(screenInteractionTriggerSolid, screenInteractionTriggerSolidFile) as CMwNod;
        set => screenInteractionTriggerSolid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B006>]
    public CMwNod? WaypointTriggerSolid
    {
        get => waypointTriggerSolid = GetNodeFromRefTable(waypointTriggerSolid, waypointTriggerSolidFile) as CMwNod;
        set => waypointTriggerSolid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B006>(sinceVersion: 2)]
    public CGameGateModel? Gate { get => gate; set => gate = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B006>(sinceVersion: 3)]
    public CGameTeleporterModel? Teleporter { get => teleporter; set => teleporter = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B006>(sinceVersion: 6)]
    public CGameTurbineModel? Turbine { get => turbine; set => turbine = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B006>(sinceVersion: 7)]
    public CPlugFlockModel? FlockModel
    {
        get => flockModel = GetNodeFromRefTable(flockModel, flockModelFile) as CPlugFlockModel;
        set => flockModel = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B006>(sinceVersion: 7)]
    public FlockEmitterState? FlockEmmiter { get => flockEmmiter; set => flockEmmiter = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B006>(sinceVersion: 8)]
    public CGameSpawnModel? SpawnModel
    {
        get => spawnModel = GetNodeFromRefTable(spawnModel, spawnModelFile) as CGameSpawnModel;
        set => spawnModel = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B006>(sinceVersion: 10)]
    public CPlugEntitySpawner?[]? EntitySpawners { get => entitySpawners; set => entitySpawners = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B007>]
    public CPlugProbe? Probe { get => probe; set => probe = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B008>]
    public CGameCtnBlockUnitInfo?[]? BlockUnitModels { get => blockUnitModels; set => blockUnitModels = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B008>]
    public bool HasManualSymmetryH { get => hasManualSymmetryH; set => hasManualSymmetryH = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B008>]
    public bool HasManualSymmetryV { get => hasManualSymmetryV; set => hasManualSymmetryV = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B008>]
    public bool HasManualSymmetryD1 { get => hasManualSymmetryD1; set => hasManualSymmetryD1 = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B008>]
    public bool HasManualSymmetryD2 { get => hasManualSymmetryD2; set => hasManualSymmetryD2 = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B008>(sinceVersion: 0, upToVersion: 1)] // version 2+ exist but they are not currently retrievable
    public Vec3 SpawnTrans { get => spawnTrans; set => spawnTrans = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B008>(sinceVersion: 0, upToVersion: 1)] // version 2+ exist but they are not currently retrievable
    public float SpawnYaw { get => spawnYaw; set => spawnYaw = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B008>(sinceVersion: 0, upToVersion: 1)] // version 2+ exist but they are not currently retrievable
    public float SpawnPitch { get => spawnPitch; set => spawnPitch = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B008>]
    public string? Name { get => name; set => name = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B00A>(sinceVersion: 2)] // they could be above ver. 2 but they wont be available here
    public CGameObjectPhyCompoundModel? CompoundModel { get => compoundModel; set => compoundModel = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0315B00A>(sinceVersion: 2)] // they could be above ver. 2 but they wont be available here
    public Iso4 CompoundLoc { get => compoundLoc; set => compoundLoc = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0315B00B>]
    public WaterArchive[]? WaterVolumes { get => waterVolumes; set => waterVolumes = value; }

    internal CGameCtnBlockInfoVariant()
    {

    }

    [Chunk(0x0315B002)]
    public class Chunk0315B002 : Chunk<CGameCtnBlockInfoVariant>
    {
        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.EnumInt32<EMultiDir>(ref n.multiDir);
        }
    }

    [Chunk(0x0315B003)]
    public class Chunk0315B003 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.Int32(ref n.symmetricalVariantIndex);

            if (version == 0)
            {
                rw.EnumInt32<ECardinalDir>(ref n.cardinalDir);
            }

            if (version >= 1)
            {
                rw.EnumByte<ECardinalDir>(ref n.cardinalDir);
                rw.EnumByte<EVariantBaseType>(ref n.variantBaseType);

                if (version >= 2)
                {
                    rw.Byte(ref n.noPillarBelowIndex);
                }
            }
        }
    }

    [Chunk(0x0315B004)]
    public class Chunk0315B004 : Chunk<CGameCtnBlockInfoVariant>
    {
        public short U01;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int16(ref U01);
        }
    }

    [Chunk(0x0315B005)]
    public class Chunk0315B005 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public int U01;
        public int U02;
        public int U03;
        public int U04;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.Array<CGameCtnBlockInfoMobil?[]>(ref n.mobils,
                (i, r) => r.ReadArray(r => r.ReadNodeRef<CGameCtnBlockInfoMobil>()),
                (x, w) => w.WriteArray(x, (x, w) => w.Write(x)));

            if (version >= 2)
            {
                rw.Int32(ref U02); // HelperSolidFid?
                rw.Int32(ref U03); // FacultativeHelperSolidFid?

                if (version >= 3)
                {
                    rw.Int32(ref U04);
                }
            }
        }
    }

    [Chunk(0x0315B006)]
    public class Chunk0315B006 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public CMwNod? U01;
        public int U02;
        public CMwNod? U03;
        public CMwNod? U04;
        public GameBoxRefTable.File? U04file;
        public CMwNod? U05;
        public GameBoxRefTable.File? U05file;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version < 9)
            {
                rw.NodeRef(ref U01);
            }

            rw.NodeRef<CMwNod>(ref n.screenInteractionTriggerSolid, ref n.screenInteractionTriggerSolidFile);
            rw.NodeRef<CMwNod>(ref n.waypointTriggerSolid, ref n.waypointTriggerSolidFile);

            if (version >= 11)
            {
                rw.NodeRef(ref U04, ref U04file); // WaypointTriggerShape?
                rw.NodeRef(ref U05, ref U05file); // ScreenInteractionTriggerShape?
            }

            if (version < 9)
            {
                rw.Int32(ref U02);
            }

            if (version >= 2)
            {
                rw.NodeRef<CGameGateModel>(ref n.gate);

                if (version >= 3)
                {
                    rw.NodeRef<CGameTeleporterModel>(ref n.teleporter);

                    if (version >= 5)
                    {
                        rw.NodeRef(ref U03);

                        if (version >= 6)
                        {
                            rw.NodeRef<CGameTurbineModel>(ref n.turbine);

                            if (version >= 7)
                            {
                                rw.NodeRef<CPlugFlockModel>(ref n.flockModel, ref n.flockModelFile);

                                if (n.flockModel is not null || n.flockModelFile is not null)
                                {
                                    rw.Archive<FlockEmitterState>(ref n.flockEmmiter);
                                }

                                if (version >= 8)
                                {
                                    rw.NodeRef<CGameSpawnModel>(ref n.spawnModel, ref n.spawnModelFile);

                                    if (version >= 10)
                                    {
                                        rw.ArrayNode<CPlugEntitySpawner>(ref n.entitySpawners);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    [Chunk(0x0315B007)]
    public class Chunk0315B007 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CPlugProbe>(ref n.probe);
        }
    }

    [Chunk(0x0315B008)]
    public class Chunk0315B008 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public Box U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.ArrayNode<CGameCtnBlockUnitInfo>(ref n.blockUnitModels);

            // CGameCtnBlockInfoVariant::SSymmetryFlags::Archive
            rw.Int32();
            rw.Boolean(ref n.hasManualSymmetryH);
            rw.Boolean(ref n.hasManualSymmetryV);
            rw.Boolean(ref n.hasManualSymmetryD1);
            rw.Boolean(ref n.hasManualSymmetryD2);
            //

            if (version < 2)
            {
                rw.Vec3(ref n.spawnTrans);
                rw.Single(ref n.spawnYaw);
                rw.Single(ref n.spawnPitch);
            }
            else
            {
                rw.Box(ref U01); // SpawnTrans, SpawnYaw, SpawnPitch, SpawnRoll - I imagine
            }

            rw.String(ref n.name);
        }
    }

    [Chunk(0x0315B009)]
    public class Chunk0315B009 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public PlacedPillarParam[]? U01;
        public ReplacedPillarParam[]? U02;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.ArrayArchive<PlacedPillarParam>(ref U01);

            if (version >= 1)
            {
                rw.ArrayArchive<ReplacedPillarParam>(ref U02);
            }
        }
    }

    [Chunk(0x0315B00A)]
    public class Chunk0315B00A : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public CMwNod? U01;
        public CMwNod? U02;
        public Iso4? U03;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version < 2)
            {
                rw.NodeRef(ref U01);
                rw.NodeRef(ref U02);

                if (version >= 1)
                {
                    rw.Iso4(ref U03);
                }
            }
            else
            {
                rw.NodeRef<CGameObjectPhyCompoundModel>(ref n.compoundModel);

                if (version < 3) // Perhaps?
                {
                    rw.Iso4(ref n.compoundLoc);
                }
            }
        }
    }

    [Chunk(0x0315B00B)]
    public class Chunk0315B00B : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.ArrayArchive<WaterArchive>(ref n.waterVolumes, version);
        }
    }

    [Chunk(0x0315B00C)]
    public class Chunk0315B00C : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public int U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.Int32(ref U01);

            if (U01 > 0)
            {
                throw new Exception("U01 > 0");
            }
        }
    }

    #region 0x00D chunk

    /// <summary>
    /// CGameCtnBlockInfoVariant 0x00D chunk
    /// </summary>
    [Chunk(0x0315B00D)]
    public class Chunk0315B00D : Chunk<CGameCtnBlockInfoVariant>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    #endregion

    public class PlacedPillarParam : IReadableWritable
    {
        private CMwNod? u01;
        private GameBoxRefTable.File? u01File;
        private int u02;
        private int u03;
        private int u04;
        private int u05;

        public CMwNod? U01 { get => u01; set => u01 = value; }
        public int U02 { get => u02; set => u02 = value; }
        public int U03 { get => u03; set => u03 = value; }
        public int U04 { get => u04; set => u04 = value; }
        public int U05 { get => u05; set => u05 = value; }

        public virtual void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.NodeRef(ref u01, ref u01File);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
        }
    }

    public class ReplacedPillarParam : PlacedPillarParam
    {
        private byte u06;

        public byte U06 { get => u06; set => u06 = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);
            rw.Byte(ref u06);
        }
    }

    public class WaterArchive : IReadableWritable
    {
        private (Int3, Int3)[]? u01;
        private float u02;
        private float u03;
        private float u04;
        private float u05;
        private float u06;
        private float u07;
        private float u08;
        private string? u09;

        public (Int3, Int3)[]? U01 { get => u01; set => u01 = value; }
        public float U02 { get => u02; set => u02 = value; }
        public float U03 { get => u03; set => u03 = value; }
        public float U04 { get => u04; set => u04 = value; }
        public float U05 { get => u05; set => u05 = value; }
        public float U06 { get => u06; set => u06 = value; }
        public float U07 { get => u07; set => u07 = value; }
        public float U08 { get => u08; set => u08 = value; }
        public string? U09 { get => u09; set => u09 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Array<(Int3, Int3)>(ref u01,
                r => (r.ReadInt3(), r.ReadInt3()),
                (x, w) => { w.Write(x.Item1); w.Write(x.Item2); });
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);

            if (version > 0)
            {
                rw.Id(ref u09);
            }
        }
    }

    public class FlockEmitterState : IReadableWritable, IVersionable
    {
        private float u01;
        private float u02;
        private int u03;
        private bool u04;
        private bool u05;
        private Vec3 position;
        private Mat3? matrix;

        public int Version { get; set; } = 1;
        public float U01 { get => u01; set => u01 = value; }
        public float U02 { get => u02; set => u02 = value; }
        public int U03 { get => u03; set => u03 = value; }
        public bool U04 { get => u04; set => u04 = value; }
        public bool U05 { get => u05; set => u05 = value; }
        public Vec3 Position { get => position; set => position = value; }
        public Mat3? Matrix { get => matrix; set => matrix = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.VersionInt32(this);
            rw.Single(ref u01); // 10 is a clear state
            rw.Single(ref u02); // 0x40000000 is a clear state
            rw.Int32(ref u03); // 5 is a clear state
            rw.Boolean(ref u04);
            rw.Boolean(ref u05); // true is a clear state

            if (Version >= 1)
            {
                rw.Mat3(ref matrix);
            }

            rw.Vec3(ref position);
        }
    }
}