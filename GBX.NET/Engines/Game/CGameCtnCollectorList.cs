namespace GBX.NET.Engines.Game
{
    [Node(0x0301B000)]
    public class CGameCtnCollectorList : Node
    {
        public Collector[] CollectorStock { get; set; }

        public CGameCtnCollectorList(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x0301B000)]
        public class Chunk0301B000 : Chunk<CGameCtnCollectorList>
        {
            public override void ReadWrite(CGameCtnCollectorList n, GameBoxReaderWriter rw)
            {
                n.CollectorStock = rw.Array(n.CollectorStock,
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
