namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Camera effect inetrial tracking.
/// </summary>
/// <remarks>ID: 0x03166000</remarks>
[Node(0x03166000)]
public class CGameCtnMediaBlockCameraEffectInertialTracking : CGameCtnMediaBlockCameraEffect, CGameCtnMediaBlock.IHasTwoKeys
{
    private TimeSingle start;
    private TimeSingle end = TimeSingle.FromSeconds(3);
    private bool tracking;
    private bool autoFocus;
    private bool autoZoom;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03166000))]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03166000))]
    public TimeSingle End { get => end; set => end = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03166000))]
    public bool Tracking { get => tracking; set => tracking = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03166000))]
    public bool AutoFocus { get => autoFocus; set => autoFocus = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03166000))]
    public bool AutoZoom { get => autoZoom; set => autoZoom = value; }

    internal CGameCtnMediaBlockCameraEffectInertialTracking()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraEffectInertialTracking 0x000 chunk
    /// </summary>
    [Chunk(0x03166000)]
    public class Chunk03166000 : Chunk<CGameCtnMediaBlockCameraEffectInertialTracking>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaBlockCameraEffectInertialTracking n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Boolean(ref n.tracking);
            rw.Boolean(ref n.autoZoom);
            rw.Boolean(ref n.autoFocus);
        }
    }

    #endregion
}
