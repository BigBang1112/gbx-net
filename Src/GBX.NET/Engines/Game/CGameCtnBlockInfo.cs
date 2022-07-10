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

    private CGameCtnBlockUnitInfo?[]? groundBlockUnitInfos;
    private CGameCtnBlockUnitInfo?[]? airBlockUnitInfos;
    private CSceneMobil?[][]? groundMobils;
    private CSceneMobil?[][]? airMobils;
    private bool isPillar;
    private EWayPointType? wayPointType;
    private bool noRespawn;

    [NodeMember]
    public CGameCtnBlockUnitInfo?[]? GroundBlockUnitInfos { get => groundBlockUnitInfos; set => groundBlockUnitInfos = value; }

    [NodeMember]
    public CGameCtnBlockUnitInfo?[]? AirBlockUnitInfos { get => airBlockUnitInfos; set => airBlockUnitInfos = value; }

    [NodeMember]
    public CSceneMobil?[][]? GroundMobils { get => groundMobils; set => groundMobils = value; }

    [NodeMember]
    public CSceneMobil?[][]? AirMobils { get => airMobils; set => airMobils = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool IsPillar { get => isPillar; set => isPillar = value; }

    [NodeMember(ExactlyNamed = true)]
    public EWayPointType? WayPointType { get => wayPointType; set => wayPointType = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool NoRespawn { get => noRespawn; set => noRespawn = value; }

    public string? BlockName { get; set; }
    public CGameCtnBlockInfoVariantGround? VariantBaseGround { get; set; }
    public CGameCtnBlockInfoVariantAir? VariantBaseAir { get; set; }
    public CGameCtnBlockInfoVariantGround?[] AdditionalVariantsGround { get; set; }
    public CGameCtnBlockInfoVariantAir?[] AdditionalVariantsAir { get; set; }
    public CMwNod? CharPhySpecialProperty { get; set; }
    public CMwNod? CharPhySpecialPropertyCustomizable { get; set; }
    public CGamePodiumInfo? PodiumInfo { get; set; }
    public CGamePodiumInfo? IntroInfo { get; set; }
    public bool IconAutoUseGround { get; set; }
    public string? SymmetricalBlockInfoId { get; set; }
    public Direction Dir { get; set; }

    protected CGameCtnBlockInfo()
    {
        AdditionalVariantsGround = Array.Empty<CGameCtnBlockInfoVariantGround>();
        AdditionalVariantsAir = Array.Empty<CGameCtnBlockInfoVariantAir>();
    }

    [Chunk(0x0304E005)]
    public class Chunk0304E005 : Chunk<CGameCtnBlockInfo>
    {
        public string? U01;
        public int U02;
        public int U03;
        public int U04;
        public bool U05;
        public int U06;
        public int U07;
        public Node? U08;
        public byte U12;
        public int U13;
        public short U14;
        public short U15;

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            // ChunkCrypted_Base
            rw.Id(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Boolean(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);
            //

            rw.NodeRef(ref U08); // null in every TMEDClassic

            rw.ArrayNode<CGameCtnBlockUnitInfo>(ref n.groundBlockUnitInfos);
            rw.ArrayNode<CGameCtnBlockUnitInfo>(ref n.airBlockUnitInfos);

            rw.Array<CSceneMobil?[]>(ref n.groundMobils,
                (i, r) => r.ReadArray(r => r.ReadNodeRef<CSceneMobil>()),
                (x, w) => w.WriteArray(x, (x, w) => w.Write(x)));

            rw.Array<CSceneMobil?[]>(ref n.airMobils,
                (i, r) => r.ReadArray(r => r.ReadNodeRef<CSceneMobil>()),
                (x, w) => w.WriteArray(x, (x, w) => w.Write(x)));

            rw.Byte(ref U12);
            rw.Int32(ref U13);
            rw.Int16(ref U14);
            rw.Int16(ref U15);
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
            rw.Boolean(ref U01);
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
            n.IconAutoUseGround = rw.Boolean(n.IconAutoUseGround);
        }
    }

    [Chunk(0x0304E015)]
    public class Chunk0304E015 : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Single();
            rw.Single();
            rw.Single();
            rw.Single();
            rw.Single();
            rw.Single();
            rw.Single();
            rw.Single();
            rw.Single();
            rw.Single();
            rw.Single();
            rw.Single();
        }
    }

    [Chunk(0x0304E017)]
    public class Chunk0304E017 : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Boolean();
        }
    }

    [Chunk(0x0304E020)]
    public class Chunk0304E020 : Chunk<CGameCtnBlockInfo>, IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            Version = rw.Int32(Version);
            n.CharPhySpecialProperty = rw.NodeRef(n.CharPhySpecialProperty);

            if (Version < 7)
            {

            }

            if (Version >= 2)
            {
                n.PodiumInfo = rw.NodeRef<CGamePodiumInfo>(n.PodiumInfo);

                if (Version >= 3)
                {
                    n.IntroInfo = rw.NodeRef<CGamePodiumInfo>(n.IntroInfo);

                    if (Version >= 4)
                    {
                        rw.Int32();

                        if (Version == 5)
                            rw.Boolean();

                        if (Version >= 8)
                        {
                            rw.Boolean();
                            rw.String(); // MatModifier
                            rw.String(); // Grass
                        }
                    }
                }
            }


        }
    }

    [Chunk(0x0304E023)]
    public class Chunk0304E023 : Chunk<CGameCtnBlockInfo>
    {
        public override void Read(CGameCtnBlockInfo n, GameBoxReader r, ILogger? logger)
        {
            n.VariantBaseGround = Parse<CGameCtnBlockInfoVariantGround>(r, 0x0315C000, progress: null, logger);
            n.VariantBaseAir = Parse<CGameCtnBlockInfoVariantAir>(r, 0x0315D000, progress: null, logger);
        }
    }

    [Chunk(0x0304E026)]
    public class Chunk0304E026 : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            n.WayPointType = (EWayPointType)rw.Int32((int)n.WayPointType);
        }
    }

    [Chunk(0x0304E027)]
    public class Chunk0304E027 : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            n.AdditionalVariantsGround = rw.ArrayNode<CGameCtnBlockInfoVariantGround>(n.AdditionalVariantsGround)!;
        }
    }

    [Chunk(0x0304E028)]
    public class Chunk0304E028 : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            n.SymmetricalBlockInfoId = rw.Id(n.SymmetricalBlockInfoId);
            n.Dir = (Direction)rw.Int32((int)n.Dir);
        }
    }

    [Chunk(0x0304E029)]
    public class Chunk0304E029 : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
        }
    }

    [Chunk(0x0304E02A)]
    public class Chunk0304E02A : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
            rw.Int32();
            for (var i = 0; i < 24; i++)
                rw.Single();
        }
    }

    [Chunk(0x0304E02B)]
    public class Chunk0304E02B : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
        }
    }

    [Chunk(0x0304E02C)]
    public class Chunk0304E02C : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            n.AdditionalVariantsAir = rw.ArrayNode<CGameCtnBlockInfoVariantAir>(n.AdditionalVariantsAir)!;
        }
    }

    [Chunk(0x0304E02F)]
    public class Chunk0304E02F : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Byte();
            rw.Int16();
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
