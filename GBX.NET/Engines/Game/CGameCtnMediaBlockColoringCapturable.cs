namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block - Coloring capturable
    /// </summary>
    [Node(0x0316C000)]
    public class CGameCtnMediaBlockColoringCapturable : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnMediaBlockColoringCapturable 0x000 chunk
        /// </summary>
        [Chunk(0x0316C000)]
        public class Chunk0316C000 : Chunk<CGameCtnMediaBlockColoringCapturable>
        {
            public int U01;

            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockColoringCapturable n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                rw.Int32(ref U01);

                n.Keys = rw.Array(n.Keys, r => new Key()
                {
                    Time = r.ReadSingle(),
                    Hue = r.ReadSingle(),
                    Gauge = r.ReadSingle(),
                    Emblem = r.ReadInt32()
                },
                (x, w) =>
                {
                    w.Write(x.Time);
                    w.Write(x.Hue);
                    w.Write(x.Gauge);
                    w.Write(x.Emblem);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float Hue { get; set; }
            public float Gauge { get; set; }
            public int Emblem { get; set; }
        }

        #endregion
    }
}
