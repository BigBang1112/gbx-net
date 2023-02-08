namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0304E000</remarks>
[Node(0x0304E000), WritingNotSupported]
public abstract class CGameCtnBlockInfo : CGameCtnCollector
{
    public enum EWayPointType
    {
        Start,
        Finish,
        Checkpoint,
        None,
        StartFinish,
        Dispenser
    }

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

    private CGameCtnBlockUnitInfo?[]? groundBlockUnitInfos;
    private CGameCtnBlockUnitInfo?[]? airBlockUnitInfos;
    private ExternalNode<CSceneMobil>[][]? groundMobils;
    private ExternalNode<CSceneMobil>[][]? airMobils;
    private bool isPillar;
    private EWayPointType? wayPointType;
    private bool noRespawn;
    private bool iconAutoUseGround;
    private CPlugCharPhySpecialProperty? charPhySpecialProperty;
    private CGamePodiumInfo? podiumInfo;
    private CGamePodiumInfo? introInfo;
    private bool charPhySpecialPropertyCustomizable;
    private CGameCtnBlockInfoVariantGround?[]? additionalVariantsGround;
    private CGameCtnBlockInfoVariantAir?[]? additionalVariantsAir;
    private string? symmetricalBlockInfoId;
    private Direction? dir;
    private CPlugFogVolumeBox? fogVolumeBox;
    private GameBoxRefTable.File? fogVolumeBoxFile;
    private CPlugSound? sound1;
    private GameBoxRefTable.File? sound1File;
    private CPlugSound? sound2;
    private GameBoxRefTable.File? sound2File;
    private Iso4? sound1Loc;
    private Iso4? sound2Loc;
    private EMultiDir pillarShapeMultiDir;
    private CGameCtnBlockInfoClassic? pillar;
    private GameBoxRefTable.File? pillarFile;

    [NodeMember]
    [AppliedWithChunk<Chunk0304E005>]
    public CGameCtnBlockUnitInfo?[]? GroundBlockUnitInfos { get => groundBlockUnitInfos; set => groundBlockUnitInfos = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0304E005>]
    public CGameCtnBlockUnitInfo?[]? AirBlockUnitInfos { get => airBlockUnitInfos; set => airBlockUnitInfos = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0304E005>]
    public ExternalNode<CSceneMobil>[][]? GroundMobils { get => groundMobils; set => groundMobils = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0304E005>]
    public ExternalNode<CSceneMobil>[][]? AirMobils { get => airMobils; set => airMobils = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E009>]
    [AppliedWithChunk<Chunk0304E02F>]
    public bool IsPillar { get => isPillar; set => isPillar = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E00E>]
    [AppliedWithChunk<Chunk0304E026>]
    public EWayPointType? WayPointType { get => wayPointType; set => wayPointType = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E00F>]
    public bool NoRespawn { get => noRespawn; set => noRespawn = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E013>]
    public bool IconAutoUseGround { get => iconAutoUseGround; set => iconAutoUseGround = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E020>]
    public CPlugCharPhySpecialProperty? CharPhySpecialProperty { get => charPhySpecialProperty; set => charPhySpecialProperty = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E020>(sinceVersion: 2)]
    public CGamePodiumInfo? PodiumInfo { get => podiumInfo; set => podiumInfo = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E020>(sinceVersion: 3)]
    public CGamePodiumInfo? IntroInfo { get => introInfo; set => introInfo = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E020>(sinceVersion: 4)]
    public bool CharPhySpecialPropertyCustomizable { get => charPhySpecialPropertyCustomizable; set => charPhySpecialPropertyCustomizable = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E027>]
    public CGameCtnBlockInfoVariantGround?[]? AdditionalVariantsGround { get => additionalVariantsGround; set => additionalVariantsGround = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E02C>]
    public CGameCtnBlockInfoVariantAir?[]? AdditionalVariantsAir { get => additionalVariantsAir; set => additionalVariantsAir = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E028>]
    public string? SymmetricalBlockInfoId { get => symmetricalBlockInfoId; set => symmetricalBlockInfoId = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E028>]
    public Direction? Dir { get => dir; set => dir = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E029>]
    public CPlugFogVolumeBox? FogVolumeBox
    {
        get => fogVolumeBox = GetNodeFromRefTable(fogVolumeBox, fogVolumeBoxFile) as CPlugFogVolumeBox;
        set => fogVolumeBox = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E02A>]
    public CPlugSound? Sound1
    {
        get => sound1 = GetNodeFromRefTable(sound1, sound1File) as CPlugSound;
        set => sound1 = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E02A>]
    public CPlugSound? Sound2
    {
        get => sound2 = GetNodeFromRefTable(sound2, sound2File) as CPlugSound;
        set => sound2 = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E02A>]
    public Iso4? Sound1Loc { get => sound1Loc; set => sound1Loc = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E02A>]
    public Iso4? Sound2Loc { get => sound2Loc; set => sound2Loc = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E02F>]
    public EMultiDir PillarShapeMultiDir { get => pillarShapeMultiDir; set => pillarShapeMultiDir = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E023>]
    public CGameCtnBlockInfoVariantGround? VariantBaseGround { get; set; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0304E023>]
    public CGameCtnBlockInfoVariantAir? VariantBaseAir { get; set; }

    [NodeMember]
    [AppliedWithChunk<Chunk0304E005>]
    public CGameCtnBlockInfoClassic? Pillar
    {
        get => pillar = GetNodeFromRefTable(pillar, pillarFile) as CGameCtnBlockInfoClassic;
        set => pillar = value;
    }

    internal CGameCtnBlockInfo()
    {

    }

    [Chunk(0x0304E005)]
    public class Chunk0304E005 : Chunk<CGameCtnBlockInfo>
    {
        public string? U01;
        public int U02;
        public int U03;
        public int U04;
        public int U06;
        public int U07;
        public byte U12;
        public int U13;
        public short U14;
        public short U15;

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            // ChunkCrypted_Base
            rw.Id(ref U01); // Ident.Id but why it's in CGameCtnBlockInfo?? xd
            rw.Int32(ref U02); // always 0?
            rw.Int32(ref U03); // always 0?
            rw.Int32(ref U04); // always 0?
            rw.Boolean(ref n.isPillar); 
            rw.Int32(ref U06); // always 0?
            rw.Int32(ref U07); // always 0?
            //

            rw.NodeRef<CGameCtnBlockInfoClassic>(ref n.pillar, ref n.pillarFile);

            rw.ArrayNode<CGameCtnBlockUnitInfo>(ref n.groundBlockUnitInfos);
            rw.ArrayNode<CGameCtnBlockUnitInfo>(ref n.airBlockUnitInfos);
            
            rw.Array<ExternalNode<CSceneMobil>[]>(ref n.groundMobils,
                (i, r) => r.ReadExternalNodeArray<CSceneMobil>(),
                (x, w) => w.WriteExternalNodeArray(x));

            rw.Array<ExternalNode<CSceneMobil>[]>(ref n.airMobils,
                (i, r) => r.ReadExternalNodeArray<CSceneMobil>(),
                (x, w) => w.WriteExternalNodeArray(x));

            rw.Byte(ref U12); // always 0?
            rw.Int32(ref U13); // always 0?
            rw.Int16(ref U14); // always 0?
            rw.Int16(ref U15); // always 0?
        }
    }

    [Chunk(0x0304E009)]
    public class Chunk0304E009 : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isPillar);
        }
    }

    [Chunk(0x0304E00B)]
    public class Chunk0304E00B : Chunk<CGameCtnBlockInfo>
    {
        private int U01;
        private CMwNod? U02;
        private CMwNod? U03;
        private CMwNod? U04;

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.NodeRef(ref U02); // VariantBaseGround
            rw.NodeRef(ref U03); // ??
            rw.NodeRef(ref U04); // VariantBaseAir
        }
    }

    [Chunk(0x0304E00C)]
    public class Chunk0304E00C : Chunk<CGameCtnBlockInfo>
    {
        public Iso4 U01;
        public Iso4 U02;

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Iso4(ref U01); // Sound1Loc/SpawnLocAir?
            rw.Iso4(ref U02); // Sound2Loc/SpawnLocGround?
        }
    }

    [Chunk(0x0304E00D)]
    public class Chunk0304E00D : Chunk<CGameCtnBlockInfo>
    {
        public bool U01;

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01); // something with replacing?
        }
    }

    [Chunk(0x0304E00E)]
    public class Chunk0304E00E : Chunk<CGameCtnBlockInfo>
    {
        private CMwNod? U01;
        private CMwNod? U02;
        private CMwNod? U03;

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.EnumInt32<EWayPointType>(ref n.wayPointType);
            rw.NodeRef(ref U01); // helper
            rw.NodeRef(ref U02); // helper
            rw.NodeRef(ref U03); // arrow
        }
    }

    [Chunk(0x0304E00F)]
    public class Chunk0304E00F : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.noRespawn);
        }
    }

    [Chunk(0x0304E013)]
    public class Chunk0304E013 : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.iconAutoUseGround);
        }
    }

    [Chunk(0x0304E015)]
    public class Chunk0304E015 : Chunk<CGameCtnBlockInfo>
    {
        public int U01;
        public Iso4 U02;

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // node ref
            rw.Iso4(ref U02);
        }
    }

    [Chunk(0x0304E017)]
    public class Chunk0304E017 : Chunk<CGameCtnBlockInfo>
    {
        public bool U01;

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    [Chunk(0x0304E020)]
    public class Chunk0304E020 : Chunk<CGameCtnBlockInfo>, IVersionable
    {
        private int version;

        public CMwNod? U01;
        public bool U02;
        public bool U03;
        public string? U04;
        public string? U05;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef(ref n.charPhySpecialProperty);

            if (version < 7)
            {
                rw.NodeRef(ref U01);
            }

            if (version >= 2)
            {
                rw.NodeRef<CGamePodiumInfo>(ref n.podiumInfo);

                if (version >= 3)
                {
                    rw.NodeRef<CGamePodiumInfo>(ref n.introInfo);

                    if (version >= 4)
                    {
                        rw.Boolean(ref n.charPhySpecialPropertyCustomizable);

                        if (version == 5)
                        {
                            rw.Boolean(ref U02);
                        }

                        if (version >= 8)
                        {
                            rw.Boolean(ref U03);

                            if (U03)
                            {
                                rw.String(ref U04); // MatModifier
                                rw.String(ref U05); // Grass
                            }
                        }
                    }
                }
            }
        }
    }

    [Chunk(0x0304E023)]
    public class Chunk0304E023 : Chunk<CGameCtnBlockInfo>
    {
        public override void Read(CGameCtnBlockInfo n, GameBoxReader r)
        {
            n.VariantBaseGround = Parse<CGameCtnBlockInfoVariantGround>(r, 0x0315C000, progress: null);
            n.VariantBaseAir = Parse<CGameCtnBlockInfoVariantAir>(r, 0x0315D000, progress: null);
        }

        public override void Write(CGameCtnBlockInfo n, GameBoxWriter w)
        {
            if (n.VariantBaseGround is null)
            {
                w.Write(-1);
            }
            else
            {
                n.VariantBaseGround.Write(w);
            }

            if (n.VariantBaseAir is null)
            {
                w.Write(-1);
            }
            else
            {
                n.VariantBaseAir.Write(w);
            }
        }
    }

    [Chunk(0x0304E026)]
    public class Chunk0304E026 : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.EnumInt32<EWayPointType>(ref n.wayPointType);
        }
    }

    [Chunk(0x0304E027)]
    public class Chunk0304E027 : Chunk<CGameCtnBlockInfo>
    {
        private int listVersion = 10;

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref listVersion);
            rw.ArrayNode<CGameCtnBlockInfoVariantGround>(ref n.additionalVariantsGround);
        }
    }

    [Chunk(0x0304E028)]
    public class Chunk0304E028 : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.symmetricalBlockInfoId);
            rw.EnumInt32<Direction>(ref n.dir);
        }
    }

    [Chunk(0x0304E029)]
    public class Chunk0304E029 : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugFogVolumeBox>(ref n.fogVolumeBox, ref n.fogVolumeBoxFile);
        }
    }

    [Chunk(0x0304E02A)]
    public class Chunk0304E02A : Chunk<CGameCtnBlockInfo>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CPlugSound>(ref n.sound1, ref n.sound1File);
            rw.NodeRef<CPlugSound>(ref n.sound2, ref n.sound2File);
            
            if (version < 3)
            {
                rw.Iso4(ref n.sound1Loc);
                rw.Iso4(ref n.sound2Loc);
            }
            else
            {
                if (n.sound1 is not null || n.sound1File is not null)
                {
                    rw.Iso4(ref n.sound1Loc);
                }

                if (n.sound2 is not null || n.sound2File is not null)
                {
                    rw.Iso4(ref n.sound2Loc);
                }
            }
        }
    }

    [Chunk(0x0304E02B)]
    public class Chunk0304E02B : Chunk<CGameCtnBlockInfo>, IVersionable
    {
        private int version;

        public int U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x0304E02C)]
    public class Chunk0304E02C : Chunk<CGameCtnBlockInfo>
    {
        private int listVersion = 10;

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref listVersion);
            rw.ArrayNode<CGameCtnBlockInfoVariantAir>(ref n.additionalVariantsAir);
        }
    }

    [Chunk(0x0304E02F)]
    public class Chunk0304E02F : Chunk<CGameCtnBlockInfo>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public byte? U01;

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Boolean(ref n.isPillar, asByte: true);
            rw.EnumByte<EMultiDir>(ref n.pillarShapeMultiDir);

            if (version >= 1)
            {
                rw.Byte(ref U01);
            }
        }
    }

    [Chunk(0x0304E031)]
    public class Chunk0304E031 : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
            rw.Int32();
        }
    }
}
