namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block - 3D stereo (0x03024000)
    /// </summary>
    [Node(0x03024000)]
    public class CGameCtnMediaBlock3dStereo : CGameCtnMediaBlock
    {
        #region Fields

        private Key[] keys;

        #endregion

        #region Properties

        [NodeMember]
        public Key[] Keys
        {
            get => keys;
            set => keys = value;
        }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnMediaBlock3dStereo 0x000 chunk
        /// </summary>
        [Chunk(0x03024000)]
        public class Chunk03024000 : Chunk<CGameCtnMediaBlock3dStereo>
        {
            public override void ReadWrite(CGameCtnMediaBlock3dStereo n, GameBoxReaderWriter rw)
            {
                rw.Array(ref n.keys, r => new Key()
                {
                    Time = r.ReadSingle(),
                    UpToMax = r.ReadSingle(),
                    ScreenDist = r.ReadSingle()
                },
                (x, w) =>
                {
                    w.Write(x.Time);
                    w.Write(x.UpToMax);
                    w.Write(x.ScreenDist);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float UpToMax { get; set; }
            public float ScreenDist { get; set; }
        }

        #endregion
    }
}
