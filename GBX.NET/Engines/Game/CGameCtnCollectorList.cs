using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Puzzle piece list (0x0301B000)
    /// </summary>
    /// <remarks>A list of puzzle pieces.</remarks>
    [Node(0x0301B000)]
    public class CGameCtnCollectorList : CMwNod
    {
        #region Properties

        [NodeMember]
        public Collector[] CollectorStock { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnCollectorList 0x000 chunk
        /// </summary>
        [Chunk(0x0301B000)]
        public class Chunk0301B000 : Chunk<CGameCtnCollectorList>
        {
            public override void ReadWrite(CGameCtnCollectorList n, GameBoxReaderWriter rw)
            {
                n.CollectorStock = rw.Array(n.CollectorStock,
                i => new Collector()
                {
                    Ident = rw.Reader.ReadIdent(),
                    Count = rw.Reader.ReadInt32()
                },
                x =>
                {
                    rw.Writer.Write(x.Ident);
                    rw.Writer.Write(x.Count);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Collector
        {
            public Ident Ident { get; set; }
            public int Count { get; set; }

            public override string ToString()
            {
                return $"{Count} {Ident.ID}";
            }
        }

        #endregion
    }
}
