namespace GBX.NET.Engines.Game
{
    [Node(0x03195000)]
    public class CGameCtnMediaBlockInterface : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; }

        [NodeMember]
        public bool ShowInterface { get; set; }

        [NodeMember]
        public string Manialink { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03195000)]
        public class Chunk03195000 : Chunk<CGameCtnMediaBlockInterface>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockInterface n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                n.ShowInterface = rw.Boolean(n.ShowInterface);
                n.Manialink = rw.String(n.Manialink);
            }
        }

        #endregion

        #endregion
    }
}
