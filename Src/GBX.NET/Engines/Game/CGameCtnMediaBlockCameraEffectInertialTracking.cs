namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Camera effect inetrial tracking (0x03166000)
/// </summary>
[Node(0x03166000)]
public class CGameCtnMediaBlockCameraEffectInertialTracking : CGameCtnMediaBlockCameraEffect, CGameCtnMediaBlock.IHasTwoKeys
{
    #region Fields

    private TimeSingle start;
    private TimeSingle end = TimeSingle.FromSeconds(3);
    private bool tracking;
    private bool autoFocus;
    private bool autoZoom;

    #endregion

    #region Properties

    [NodeMember]
    public TimeSingle Start
    {
        get => start;
        set => start = value;
    }

    [NodeMember]
    public TimeSingle End
    {
        get => end;
        set => end = value;
    }

    [NodeMember]
    public bool Tracking
    {
        get => tracking;
        set => tracking = value;
    }

    [NodeMember]
    public bool AutoFocus
    {
        get => autoFocus;
        set => autoFocus = value;
    }

    [NodeMember]
    public bool AutoZoom
    {
        get => autoZoom;
        set => autoZoom = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockCameraEffectInertialTracking()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraEffectInertialTracking 0x000 chunk
    /// </summary>
    [Chunk(0x03166000)]
    public class Chunk03166000 : Chunk<CGameCtnMediaBlockCameraEffectInertialTracking>, IVersionable
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

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

    #endregion
}
