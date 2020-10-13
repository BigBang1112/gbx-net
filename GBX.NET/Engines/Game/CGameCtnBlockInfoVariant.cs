namespace GBX.NET.Engines.Game
{
    [Node(0x0315B000)]
    public class CGameCtnBlockInfoVariant : Node
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
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0315B003)]
        public class Chunk0315B003 : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int16(Unknown);
                rw.Byte(Unknown);
            }
        }

        [Chunk(0x0315B004)]
        public class Chunk0315B004 : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int16(Unknown);
            }
        }

        [Chunk(0x0315B005)]
        public class Chunk0315B005 : Chunk<CGameCtnBlockInfoVariant>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                rw.Int32(Unknown);
                n.Mobils = rw.Array(n.Mobils,
                    i => rw.Reader.ReadNodeRef<CGameCtnBlockInfoMobil>(),
                    x => rw.Writer.Write(x));

                if(Version >= 2)
                {
                    rw.Int32(Unknown);
                    rw.Int32(Unknown);
                }
            }
        }

        [Chunk(0x0315B006)]
        public class Chunk0315B006 : Chunk<CGameCtnBlockInfoVariant>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);

                if(Version >= 10)
                {
                    rw.Int32(Unknown);
                    rw.Int32(Unknown);

                    if(Version >= 11)
                    {
                        rw.Int32(Unknown);
                        rw.Int32(Unknown);
                    }
                }
            }
        }

        [Chunk(0x0315B007)]
        public class Chunk0315B007 : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0315B008)]
        public class Chunk0315B008 : Chunk<CGameCtnBlockInfoVariant>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                var hsdh = rw.Reader.PeekUInt32();
                n.BlockUnitInfos = rw.Array(n.BlockUnitInfos,
                    i => rw.Reader.ReadNodeRef<CGameCtnBlockUnitInfo>(),
                    x => rw.Writer.Write(x));
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Vec3(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                if (Version >= 2)
                    rw.Int32(Unknown);
                rw.String(Unknown);
            }
        }

        [Chunk(0x0315B009)]
        public class Chunk0315B009 : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Reader.ReadArray(i => rw.Reader.ReadArray<int>(5));
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0315B00A)]
        public class Chunk0315B00A : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Single(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Single(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Single(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0315B00B)]
        public class Chunk0315B00B : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x0315B00C)]
        public class Chunk0315B00C : Chunk<CGameCtnBlockInfoVariant>
        {
            public override void ReadWrite(CGameCtnBlockInfoVariant n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }
    }
}