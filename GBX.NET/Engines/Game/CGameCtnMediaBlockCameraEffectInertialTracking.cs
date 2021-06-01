namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block - Camera effect inetrial tracking
    /// </summary>
    [Node(0x03166000)]
    public class CGameCtnMediaBlockCameraEffectInertialTracking : CGameCtnMediaBlockCameraEffect
    {
        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; }

        [NodeMember]
        public bool Tracking { get; set; }

        [NodeMember]
        public bool AutoFocus { get; set; }

        [NodeMember]
        public bool AutoZoom { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraEffectInertialTracking 0x000 chunk
        /// </summary>
        [Chunk(0x03166000)]
        public class Chunk03166000 : Chunk<CGameCtnMediaBlockCameraEffectInertialTracking>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockCameraEffectInertialTracking n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                n.Tracking = rw.Boolean(n.Tracking);
                n.AutoZoom = rw.Boolean(n.AutoZoom);
                n.AutoFocus = rw.Boolean(n.AutoFocus);
            }
        }

        #endregion

        #endregion
    }
}
