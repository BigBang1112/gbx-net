using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Scene;

namespace GBX.NET.Engines.Game
{
    [Node(0x0304E000)]
    public class CGameCtnBlockInfo : CGameCtnCollector
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

        public string BlockName { get; set; }
        public CGameCtnBlockInfoVariantGround VariantBaseGround { get; set; }
        public CGameCtnBlockInfoVariantAir VariantBaseAir { get; set; }
        public CGameCtnBlockInfoVariantGround[] AdditionalVariantsGround { get; set; }
        public CGameCtnBlockInfoVariantAir[] AdditionalVariantsAir { get; set; }
        public Node CharPhySpecialProperty { get; set; }
        public Node CharPhySpecialPropertyCustomizable { get; set; }
        public CGamePodiumInfo PodiumInfo { get; set; }
        public CGamePodiumInfo IntroInfo { get; set; }
        public bool IconAutoUseGround { get; set; }
        public bool NoRespawn { get; set; }
        public EWayPointType WayPointType { get; set; }
        public string SymmetricalBlockInfoId { get; set; }
        public Direction Dir { get; set; }

        [Chunk(0x0304E005)]
        public class Chunk0304E005 : Chunk<CGameCtnBlockInfo>
        {
            public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw) // WIP
            {
                n.BlockName = rw.Id(n.BlockName);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Reader.ReadArray(r => r.ReadNodeRef<CGameCtnBlockUnitInfo>());
                rw.Reader.ReadArray(r => r.ReadNodeRef<CGameCtnBlockUnitInfo>());
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Reader.ReadNodeRef<CSceneMobil>();
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Reader.ReadNodeRef<CSceneMobil>();
                rw.Byte(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0304E009)]
        public class Chunk0304E009 : Chunk<CGameCtnBlockInfo>
        {
            public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0304E00C)]
        public class Chunk0304E00C : Chunk<CGameCtnBlockInfo>
        {
            public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
            {
                rw.Array<float>(Unknown, 24);
            }
        }

        [Chunk(0x0304E00D)]
        public class Chunk0304E00D : Chunk<CGameCtnBlockInfo>
        {
            public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0304E00E)]
        public class Chunk0304E00E : Chunk<CGameCtnBlockInfo>
        {
            public Node[] Unknown1 { get; set; }

            public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
            {
                Unknown1 = rw.Array(Unknown1, r => r.ReadNodeRef(), (x, w) => w.Write(x));
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
                rw.Int32(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
                rw.Single(Unknown);
            }
        }

        [Chunk(0x0304E017)]
        public class Chunk0304E017 : Chunk<CGameCtnBlockInfo>
        {
            public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
            {
                rw.Boolean(Unknown);
            }
        }

        [Chunk(0x0304E020)]
        public class Chunk0304E020 : Chunk<CGameCtnBlockInfo>
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
                            rw.Int32(Unknown);

                            if (Version == 5)
                                rw.Boolean(Unknown);

                            if (Version >= 8)
                            {
                                rw.Boolean(Unknown);
                                rw.String(Unknown); // MatModifier
                                rw.String(Unknown); // Grass
                            }
                        }
                    }
                }

                
            }
        }

        [Chunk(0x0304E023)]
        public class Chunk0304E023 : Chunk<CGameCtnBlockInfo>
        {
            public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
            {
                n.VariantBaseGround = Parse<CGameCtnBlockInfoVariantGround>(rw.Reader, 0x0315C000);
                n.VariantBaseAir = Parse<CGameCtnBlockInfoVariantAir>(rw.Reader, 0x0315D000);
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
                rw.Int32(Unknown);
                n.AdditionalVariantsGround = rw.Array(n.AdditionalVariantsGround,
                    r => r.ReadNodeRef<CGameCtnBlockInfoVariantGround>(),
                    (x, w) => w.Write(x));
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
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0304E02A)]
        public class Chunk0304E02A : Chunk<CGameCtnBlockInfo>
        {
            public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Array<float>(Unknown, 24);
            }
        }

        [Chunk(0x0304E02B)]
        public class Chunk0304E02B : Chunk<CGameCtnBlockInfo>
        {
            public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0304E02C)]
        public class Chunk0304E02C : Chunk<CGameCtnBlockInfo>
        {
            public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                n.AdditionalVariantsAir = rw.Array(n.AdditionalVariantsAir,
                    r => r.ReadNodeRef<CGameCtnBlockInfoVariantAir>(),
                    (x, w) => w.Write(x));
            }
        }

        [Chunk(0x0304E02F)]
        public class Chunk0304E02F : Chunk<CGameCtnBlockInfo>
        {
            public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Byte(Unknown);
                rw.Int16(Unknown);
            }
        }

        [Chunk(0x0304E031)]
        public class Chunk0304E031 : Chunk<CGameCtnBlockInfo>
        {
            public override void ReadWrite(CGameCtnBlockInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }
    }
}