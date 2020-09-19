namespace GBX.NET.Engines.Game
{
    [Node(0x03165000)]
    public class CGameCtnMediaBlockDirtyLens : CGameCtnMediaBlock
    {
        public Key[] Keys { get; set; }

        [Chunk(0x03165000)]
        public class Chunk03165000 : Chunk<CGameCtnMediaBlockDirtyLens>
        {
            public int Version { get; set; }

            public override void Read(CGameCtnMediaBlockDirtyLens n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                n.Keys = r.ReadArray(i =>
                {
                    var time = r.ReadSingle();
                    var intensity = r.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        Intensity = intensity
                    };
                });
            }

            public override void Write(CGameCtnMediaBlockDirtyLens n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                w.Write(n.Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.Intensity);
                });
            }
        }
    }

    public class Key : MediaBlockKey
    {
        public float Intensity { get; set; }
    }
}