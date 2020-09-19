namespace GBX.NET.Engines.Game
{
    [Node(0x03085000)]
    public class CGameCtnMediaBlockTime : CGameCtnMediaBlock
    {
        public Key[] Keys { get; set; }

        [Chunk(0x03085000)]
        public class Chunk03085000 : Chunk<CGameCtnMediaBlockTime>
        {
            public override void ReadWrite(CGameCtnMediaBlockTime n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, i => new Key()
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
