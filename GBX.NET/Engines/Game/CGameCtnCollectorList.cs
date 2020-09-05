namespace GBX.NET.Engines.Game
{
    [Node(0x0301B000)]
    public class CGameCtnCollectorList : Node
    {
        public Collector[] CollectorStock
        {
            get => GetValue<Chunk000>(x => x.CollectorStock) as Collector[];
        }

        public CGameCtnCollectorList(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x0301B000)]
        public class Chunk000 : Chunk
        {
            public Collector[] CollectorStock { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                CollectorStock = rw.Array(CollectorStock,
                    i => new Collector()
                    {
                        Meta = rw.Reader.ReadMeta(),
                        Count = rw.Reader.ReadInt32()
                    },
                    x =>
                    {
                        rw.Writer.Write(x.Meta);
                        rw.Writer.Write(x.Count);
                    });
            }
        }

        #endregion

        #endregion

        public class Collector
        {
            public Meta Meta { get; set; }
            public int Count { get; set; }
        }
    }
}
