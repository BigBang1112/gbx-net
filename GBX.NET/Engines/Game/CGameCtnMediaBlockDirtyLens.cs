namespace GBX.NET.Engines.Game
{
    [Node(0x03165000)]
    public class CGameCtnMediaBlockDirtyLens : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockDirtyLens(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03165000)]
        public class Chunk000 : Chunk
        {
            public int Version { get; set; }
            public Key[] Keys { get; set; }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                Keys = r.ReadArray(i =>
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

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                w.Write(Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.Intensity);
                });
            }
        }
    }

    public class Key
    {
        public float Time { get; set; }
        public float Intensity { get; set; }
    }
}