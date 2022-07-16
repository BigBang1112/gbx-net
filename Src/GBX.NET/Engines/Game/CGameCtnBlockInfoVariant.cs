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

    [NodeMember(ExactlyNamed = true)]
    public EMultiDir MultiDir { get => multiDir; set => multiDir = value; }

    [NodeMember(ExactlyNamed = true)]
    public int SymmetricalVariantIndex { get => symmetricalVariantIndex; set => symmetricalVariantIndex = value; }

    [NodeMember(ExactlyNamed = true)]
    public ECardinalDir CardinalDir { get => cardinalDir; set => cardinalDir = value; }

    [NodeMember(ExactlyNamed = true)]
    public EVariantBaseType VariantBaseType { get => variantBaseType; set => variantBaseType = value; }

    [NodeMember(ExactlyNamed = true)]
    public byte NoPillarBelowIndex { get => noPillarBelowIndex; set => noPillarBelowIndex = value; }

    [NodeMember]
    public CGameCtnBlockInfoMobil?[][]? Mobils { get => mobils; set => mobils = value; }

    [NodeMember(ExactlyNamed = true)]
    public CMwNod? ScreenInteractionTriggerSolid
    {
        get => screenInteractionTriggerSolid = GetNodeFromRefTable(screenInteractionTriggerSolid, screenInteractionTriggerSolidFile) as CMwNod;
        set => screenInteractionTriggerSolid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CMwNod? WaypointTriggerSolid
    {
        get => waypointTriggerSolid = GetNodeFromRefTable(waypointTriggerSolid, waypointTriggerSolidFile) as CMwNod;
        set => waypointTriggerSolid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CGameGateModel? Gate { get => gate; set => gate = value; }

    [NodeMember(ExactlyNamed = true)]
    public CGameTeleporterModel? Teleporter { get => teleporter; set => teleporter = value; }

    [NodeMember(ExactlyNamed = true)]
    public CGameTurbineModel? Turbine { get => turbine; set => turbine = value; }

    [NodeMember(ExactlyNamed = true)]
    public CPlugFlockModel? FlockModel
    {
        get => flockModel = GetNodeFromRefTable(flockModel, flockModelFile) as CPlugFlockModel;
        set => flockModel = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CGameSpawnModel? SpawnModel
    {
        get => spawnModel = GetNodeFromRefTable(spawnModel, spawnModelFile) as CGameSpawnModel;
        set => spawnModel = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CPlugEntitySpawner?[]? EntitySpawners { get => entitySpawners; set => entitySpawners = value; }

    [NodeMember(ExactlyNamed = true)]
    public CPlugProbe? Probe { get => probe; set => probe = value; }

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnBlockUnitInfo?[]? BlockUnitModels { get => blockUnitModels; set => blockUnitModels = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool HasManualSymmetryH { get => hasManualSymmetryH; set => hasManualSymmetryH = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool HasManualSymmetryV { get => hasManualSymmetryV; set => hasManualSymmetryV = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool HasManualSymmetryD1 { get => hasManualSymmetryD1; set => hasManualSymmetryD1 = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool HasManualSymmetryD2 { get => hasManualSymmetryD2; set => hasManualSymmetryD2 = value; }

    [NodeMember(ExactlyNamed = true)]
    public Vec3 SpawnTrans { get => spawnTrans; set => spawnTrans = value; }

    [NodeMember(ExactlyNamed = true)]
    public float SpawnYaw { get => spawnYaw; set => spawnYaw = value; }

    [NodeMember(ExactlyNamed = true)]
    public float SpawnPitch { get => spawnPitch; set => spawnPitch = value; }

    [NodeMember(ExactlyNamed = true)]
    public string? Name { get => name; set => name = value; }

    [NodeMember(ExactlyNamed = true)]
    public CGameObjectPhyCompoundModel? CompoundModel { get => compoundModel; set => compoundModel = value; }

    [NodeMember(ExactlyNamed = true)]
    public Iso4 CompoundLoc { get => compoundLoc; set => compoundLoc = value; }

    protected CGameCtnBlockInfoVariant()
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

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.Array<CGameCtnBlockInfoMobil?[]>(ref n.mobils,
                (i, r) => r.ReadArray(r => r.ReadNodeRef<CGameCtnBlockInfoMobil>()),
                (x, w) => w.WriteArray(x, (x, w) => w.Write(x)));

            if (version >= 2)
            {
                rw.Int32(ref U02);
                rw.Int32(ref U03); // FacultativeHelperSolidFid?
            }

            // HelperSolidFid?
            // FacultativeHelperSolidFid?
        }
    }

    [Chunk(0x0315B006)]
    public class Chunk0315B006 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public CMwNod? U01;
        public int U02;
        public CMwNod? U03;
        public int U04;
        public int U05;

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

                                if (version >= 8)
                                {
                                    rw.NodeRef<CGameSpawnModel>(ref n.spawnModel, ref n.spawnModelFile);

                                    if (version >= 10)
                                    {
                                        rw.ArrayNode<CPlugEntitySpawner>(ref n.entitySpawners);

                                        if (version >= 11)
                                        {
                                            rw.Int32(ref U04);
                                            rw.Int32(ref U05);
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

    [Chunk(0x0315B007)]
    public class Chunk0315B007 : Chunk<CGameCtnBlockInfoVariant>, IVersionable
    {
        private int version;

        public int U01;
        public int U02;

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
                rw.Vec3(n.spawnTrans);
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
                rw.Iso4(ref n.compoundLoc);
            }
        }
    }

    [Chunk(0x0315B00B)]
    public class Chunk0315B00B : Chunk<CGameCtnBlockInfoVariant>, IVersionable
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
}