using System;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Camera effect inetrial tracking
/// </summary>
[Node(0x03166000)]
public class CGameCtnMediaBlockCameraEffectInertialTracking : CGameCtnMediaBlockCameraEffect, CGameCtnMediaBlock.IHasTwoKeys
{
    #region Fields

    private TimeSpan start;
    private TimeSpan end = TimeSpan.FromSeconds(3);
    private bool tracking;
    private bool autoFocus;
    private bool autoZoom;

    #endregion

    #region Properties

    [NodeMember]
    public TimeSpan Start
    {
        get => start;
        set => start = value;
    }

    [NodeMember]
    public TimeSpan End
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

    private CGameCtnMediaBlockCameraEffectInertialTracking()
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
            rw.Single_s(ref n.start);
            rw.Single_s(ref n.end);
            rw.Boolean(ref n.tracking);
            rw.Boolean(ref n.autoZoom);
            rw.Boolean(ref n.autoFocus);
        }
    }

    #endregion

    #endregion
}
