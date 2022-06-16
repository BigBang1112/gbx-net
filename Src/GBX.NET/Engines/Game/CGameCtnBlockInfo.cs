namespace GBX.NET.Engines.Game;

/// <summary>
/// CGameCtnBlockInfo (0x0304E000)
/// </summary>
[Node(0x0304E000), WritingNotSupported]
public abstract class CGameCtnBlockInfo : CGameCtnCollector
{ 
    private CGameCtnBlockUnitInfo[]? units;
    private CGameCtnBlockUnitInfo[]? units2;
    private CSceneMobil?[][]? mobils;
    private CSceneMobil?[][]? mobils2;
    private bool isPillar;

    public enum EWayPointType
    {
        Start,
        Finish,
        Checkpoint,
        None,
        StartFinish,
        Dispenser
    }

    public CGameCtnBlockUnitInfo[]? Units
    {
        get => units;
        set => units = value;
    }

    public CGameCtnBlockUnitInfo[]? Units2
    {
        get => units2;
        set => units2 = value;
    }

    public CSceneMobil?[][]? Mobils
    {
        get => mobils;
        set => mobils = value;
    }

    public CSceneMobil?[][]? Mobils2
    {
        get => mobils2;
        set => mobils2 = value;
    }

    public bool IsPillar
    {
        get => isPillar;
        set => isPillar = value;
    }

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
    public bool NoRespawn { get; set; }
    public EWayPointType WayPointType { get; set; }
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

        public override void Read(CGameCtnBlockInfo n, GameBoxReader r)
        {
            U01 = r.ReadId();
            U02 = r.ReadInt32();
            U03 = r.ReadInt32();
            U04 = r.ReadInt32();
            U05 = r.ReadBoolean();
            U06 = r.ReadInt32();
            U07 = r.ReadInt32();

            U08 = r.ReadNodeRef(); // null in every TMEDClassic

            n.units = r.ReadArray(r => r.ReadNodeRef<CGameCtnBlockUnitInfo>()!);
            n.units2 = r.ReadArray(r => r.ReadNodeRef<CGameCtnBlockUnitInfo>()!);

            n.mobils = r.ReadArray(r =>
            {
                return r.ReadArray(r => r.ReadNodeRef<CSceneMobil>()); // External refs to some mobils
            });

            // may not be it but could be
            n.mobils2 = r.ReadArray(r =>
            {
                return r.ReadArray(r => r.ReadNodeRef<CSceneMobil>());
            });

            U12 = r.ReadByte();
            U13 = r.ReadInt32();
            U14 = r.ReadInt16();
            U15 = r.ReadInt16();
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
            rw.Iso4(ref U01);
            rw.Iso4(ref U02);
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
        public CMwNod?[]? U01;

        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            rw.ArrayNode(ref U01);
        }
    }

    [Chunk(0x0304E00F)]
    public class Chunk0304E00F : Chunk<CGameCtnBlockInfo>
    {
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
        {
            n.NoRespawn = rw.Boolean(n.NoRespawn);
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
        public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw, ILogger? logger)
        {
            if (rw.Mode == GameBoxReaderWriterMode.Read)
            {
                n.VariantBaseGround = Parse<CGameCtnBlockInfoVariantGround>(rw.Reader!, 0x0315C000, progress: null, logger);
                n.VariantBaseAir = Parse<CGameCtnBlockInfoVariantAir>(rw.Reader!, 0x0315D000, progress: null, logger);
            }
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
