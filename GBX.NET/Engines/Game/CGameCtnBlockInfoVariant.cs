using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Game
{
    [Node(0x0315B000)]
    public class CGameCtnBlockInfoVariant : CMwNod
    {
        public string Name { get; set; }
        public CGameCtnBlockInfoMobil[] Mobils { get; set; }
        public CGameCtnBlockUnitInfo[] BlockUnitInfos { get; set; }
        public bool HasManualSymmetryH { get; set; }
        public float SpawnTransX { get; set; }
        public float SpawnTransY { get; set; }
        public float SpawnTransZ { get; set; }

        [Chunk(0x0315B002)]
        public class Chunk0315B002 : Chunk<CGameCtnBlockInfoVariant>
        {
            private int u01;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
            }
        }

        [Chunk(0x0315B003)]
        public class Chunk0315B003 : Chunk<CGameCtnBlockInfoVariant>
        {
            private int u01;
            private int u02;
            private short u03;
            private byte u04;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public short U03
            {
                get => u03;
                set => u03 = value;
            }

            public byte U04
            {
                get => u04;
                set => u04 = value;
            }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
                rw.Int32(ref u02);
                rw.Int16(ref u03);
                rw.Byte(ref u04);
            }
        }

        [Chunk(0x0315B004)]
        public class Chunk0315B004 : Chunk<CGameCtnBlockInfoVariant>
        {
            private short u01;

            public short U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int16(ref u01);
            }
        }

        [Chunk(0x0315B005)]
        public class Chunk0315B005 : Chunk<CGameCtnBlockInfoVariant>
        {
            private int version;
            private int u01;
            private int u02;
            private int u03;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public int U03
            {
                get => u03;
                set => u03 = value;
            }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Int32(ref u01);
                n.Mobils = rw.Array(n.Mobils,
                    r => r.ReadNodeRef<CGameCtnBlockInfoMobil>(),
                    (x, w) => w.Write(x));

                if (Version >= 2)
                {
                    rw.Int32(ref u02);
                    rw.Int32(ref u03);
                }
            }
        }

        [Chunk(0x0315B006)]
        public class Chunk0315B006 : Chunk<CGameCtnBlockInfoVariant>
        {
            private int version;
            private int u01;
            private int u02;
            private int u03;
            private int u04;
            private int u05;
            private int u06;
            private int u07;
            private int u08;
            private int u09;
            private int u10;
            private int u11;
            private int u12;
            private int u13;
            private int u14;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public int U03
            {
                get => u03;
                set => u03 = value;
            }

            public int U04
            {
                get => u04;
                set => u04 = value;
            }

            public int U05
            {
                get => u05;
                set => u05 = value;
            }

            public int U06
            {
                get => u06;
                set => u06 = value;
            }

            public int U07
            {
                get => u07;
                set => u07 = value;
            }

            public int U08
            {
                get => u08;
                set => u08 = value;
            }

            public int U09
            {
                get => u09;
                set => u09 = value;
            }

            public int U10
            {
                get => u10;
                set => u10 = value;
            }

            public int U11
            {
                get => u11;
                set => u11 = value;
            }

            public int U12
            {
                get => u12;
                set => u12 = value;
            }

            public int U13
            {
                get => u13;
                set => u13 = value;
            }

            public int U14
            {
                get => u14;
                set => u14 = value;
            }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Int32(ref u01);
                rw.Int32(ref u02);
                rw.Int32(ref u03);
                rw.Int32(ref u04);
                rw.Int32(ref u05);
                rw.Int32(ref u06);
                rw.Int32(ref u07);
                rw.Int32(ref u08);
                rw.Int32(ref u09);
                rw.Int32(ref u10);

                if (Version >= 10)
                {
                    rw.Int32(ref u11);
                    rw.Int32(ref u12);

                    if (Version >= 11)
                    {
                        rw.Int32(ref u13);
                        rw.Int32(ref u14);
                    }
                }
            }
        }

        [Chunk(0x0315B007)]
        public class Chunk0315B007 : Chunk<CGameCtnBlockInfoVariant>
        {
            private int u01;
            private int u02;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
                rw.Int32(ref u02);
            }
        }

        [Chunk(0x0315B008)]
        public class Chunk0315B008 : Chunk<CGameCtnBlockInfoVariant>
        {
            private int version;
            private int u01;
            private int u02;
            private int u03;
            private int u04;
            private int u05;
            private Vec3 u06;
            private int u07;
            private int u08;
            private int u09;
            private string u10;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);

                n.BlockUnitInfos = rw.Array(n.BlockUnitInfos,
                    r => r.ReadNodeRef<CGameCtnBlockUnitInfo>(),
                    (x, w) => w.Write(x));

                rw.Int32(ref u01);
                rw.Int32(ref u02);
                rw.Int32(ref u03);
                rw.Int32(ref u04);
                rw.Int32(ref u05);
                rw.Vec3(ref u06);
                rw.Int32(ref u07);
                rw.Int32(ref u08);

                if (Version >= 2)
                    rw.Int32(ref u09);

                rw.String(ref u10);
            }
        }

        [Chunk(0x0315B009)]
        public class Chunk0315B009 : Chunk<CGameCtnBlockInfoVariant>
        {
            private int u01;
            private int u02;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
                rw.Reader.ReadArray(r => r.ReadArray<int>(5));
                rw.Int32(ref u02);
            }
        }

        [Chunk(0x0315B00A)]
        public class Chunk0315B00A : Chunk<CGameCtnBlockInfoVariant>
        {
            private int u01;
            private int u02;
            private float u03;
            private int u04;
            private int u05;
            private int u06;
            private float u07;
            private int u08;
            private int u09;
            private int u10;
            private float u11;
            private int u12;
            private int u13;
            private int u14;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public float U03
            {
                get => u03;
                set => u03 = value;
            }

            public int U04
            {
                get => u04;
                set => u04 = value;
            }

            public int U05
            {
                get => u05;
                set => u05 = value;
            }

            public int U06
            {
                get => u06;
                set => u06 = value;
            }

            public float U07
            {
                get => u07;
                set => u07 = value;
            }

            public int U08
            {
                get => u08;
                set => u08 = value;
            }

            public int U09
            {
                get => u09;
                set => u09 = value;
            }

            public int U10
            {
                get => u10;
                set => u10 = value;
            }

            public float U11
            {
                get => u11;
                set => u11 = value;
            }

            public int U12
            {
                get => u12;
                set => u12 = value;
            }

            public int U13
            {
                get => u13;
                set => u13 = value;
            }

            public int U14
            {
                get => u14;
                set => u14 = value;
            }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
                rw.Int32(ref u02);
                rw.Single(ref u03);
                rw.Int32(ref u04);
                rw.Int32(ref u05);
                rw.Int32(ref u06);
                rw.Single(ref u07);
                rw.Int32(ref u08);
                rw.Int32(ref u09);
                rw.Int32(ref u10);
                rw.Single(ref u11);
                rw.Int32(ref u12);
                rw.Int32(ref u13);
                rw.Int32(ref u14);
            }
        }

        [Chunk(0x0315B00B)]
        public class Chunk0315B00B : Chunk<CGameCtnBlockInfoVariant>
        {
            private int u01;
            private int u02;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
                rw.Int32(ref u02);
            }
        }

        [Chunk(0x0315B00C)]
        public class Chunk0315B00C : Chunk<CGameCtnBlockInfoVariant>
        {
            public int U01;
            public int U02;

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref U01);
                rw.Int32(ref U02);
            }
        }
    }
}