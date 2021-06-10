namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker Bloom block for TMUF and older games (0x03083000). This node causes "Couldn't load map" in ManiaPlanet.
    /// </summary>
    [Node(0x03083000)]
    public class CGameCtnMediaBlockFxBloom : CGameCtnMediaBlockFx
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x001 chunk

        /// <summary>
        /// CGameCtnMediaBlockFxBloom 0x001 chunk
        /// </summary>
        [Chunk(0x03083001)]
        public class Chunk03083001 : Chunk<CGameCtnMediaBlockFxBloom>
        {
            public override void Read(CGameCtnMediaBlockFxBloom n, GameBoxReader r)
            {
                n.Keys = r.ReadArray(r1 =>
                {
                    return new Key()
                    {
                        Time = r1.ReadSingle(),
                        Intensity = r1.ReadSingle(),
                        Sensitivity = r1.ReadSingle()
                    };
                });
            }

            public override void Write(CGameCtnMediaBlockFxBloom n, GameBoxWriter w)
            {
                w.Write(n.Keys, (x, w1) =>
                {
                    w1.Write(x.Time);
                    w1.Write(x.Intensity);
                    w1.Write(x.Sensitivity);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float Intensity { get; set; }
            public float Sensitivity { get; set; }
        }

        #endregion
    }
}
