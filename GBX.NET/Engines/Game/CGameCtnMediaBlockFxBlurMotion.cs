namespace GBX.NET.Engines.Game
{
    [Node(0x03082000)]
    public class CGameCtnMediaBlockFxBlurMotion : CGameCtnMediaBlockFxBlur
    {
        #region Properties

        [NodeMember]
        public float? Start { get; set; }

        [NodeMember]
        public float? End { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03082000)]
        public class Chunk03082000 : Chunk<CGameCtnMediaBlockFxBlurMotion>
        {
            public override void ReadWrite(CGameCtnMediaBlockFxBlurMotion n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start.GetValueOrDefault());
                n.End = rw.Single(n.End.GetValueOrDefault(3));
            }
        }

        #endregion

        #endregion
    }
}
