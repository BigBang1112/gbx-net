namespace GBX.NET.Engines.Game
{
    [Node(0x03085000)]
    public class CGameCtnMediaBlockTime : CGameCtnMediaBlock
    {
        public CGameCtnMediaBlockTime(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03085000)]
        public class Chunk000 : Chunk
        {
            public Key[] Keys { get; set; }

            public Chunk000(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Keys = rw.Array(Keys, i => new Key()
                {
                    Time = rw.Reader.ReadSingle(),
                    TimeValue = rw.Reader.ReadSingle(),
                    Tangent = rw.Reader.ReadSingle()
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.TimeValue);
                    rw.Writer.Write(x.Tangent);
                });
            }
        }

        public class Key : MediaBlockKey
        {
            public float TimeValue { get; set; }
            public float Tangent { get; set; }
        }
    }
}
