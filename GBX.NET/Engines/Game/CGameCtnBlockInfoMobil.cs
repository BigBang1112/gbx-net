using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.Game
{
    [Node(0x03122000)]
    public class CGameCtnBlockInfoMobil : Node
    {
        public CPlugRoadChunk[] RoadChunks { get; set; }
        public CGameCtnBlockInfoMobilLink[] DynaLinks { get; set; }

        [Chunk(0x03122002)]
        public class Chunk03122002 : Chunk<CGameCtnBlockInfoMobil>
        {
            public override void ReadWrite(CGameCtnBlockInfoMobil n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }


        [Chunk(0x03122003)]
        public class Chunk03122003 : Chunk<CGameCtnBlockInfoMobil>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnBlockInfoMobil n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                rw.Byte(Unknown);
                var num = rw.Reader.ReadInt32();
                
                if(num == 16777216) // TODO: figure out why
                {
                    rw.Single(Unknown);
                    rw.Single(Unknown);

                    rw.Single(Unknown);
                    rw.Single(Unknown);

                    rw.Single(Unknown);
                    rw.Single(Unknown);
                }

                if (Version >= 20)
                {
                    rw.Int32(Unknown);
                    rw.Int32(Unknown);
                }

                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);

                if (Version >= 20)
                {
                    rw.Int32(Unknown);
                    rw.Int32(Unknown);
                    rw.Int32(Unknown); // 3

                    rw.Int32(Unknown);
                    rw.Int32(Unknown);
                    rw.Int32(Unknown);
                    rw.Int32(Unknown);
                    rw.Byte(Unknown);
                    rw.Vec3(Unknown);
                    n.RoadChunks = rw.Array(n.RoadChunks,
                        i => rw.Reader.ReadNodeRef<CPlugRoadChunk>(),
                        x => rw.Writer.Write(x));
                }
            }
        }

        [Chunk(0x03122004)]
        public class Chunk03122004 : Chunk<CGameCtnBlockInfoMobil>
        {
            public override void ReadWrite(CGameCtnBlockInfoMobil n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                n.DynaLinks = rw.Reader.ReadArray(i =>
                {
                    var u01 = rw.Reader.ReadInt32();
                    var u02 = rw.Reader.ReadInt32();
                    var u03 = rw.Reader.ReadInt32();
                    var socketID = rw.Reader.ReadId();
                    var model = rw.Reader.ReadNodeRef<CGameObjectModel>();

                    return new CGameCtnBlockInfoMobilLink(socketID, model);
                });
            }
        }
    }
}