namespace GBX.NET.Engines.Game
{
    [Node(0x0312A000)]
    public class CGameCtnMediaBlockManialink : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; }

        [NodeMember]
        public string ManialinkURL { get; set; }

        #endregion

        #region Chunks

        // 0x000 chunk

        #region 0x001 chunk

        [Chunk(0x0312A001)]
        public class Chunk0312A001 : Chunk<CGameCtnMediaBlockManialink>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockManialink n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                n.ManialinkURL = rw.String(n.ManialinkURL);
            }
        }

        #endregion

        #endregion
    }
}
