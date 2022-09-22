namespace GBX.NET.Engines.Game;

public partial class CGameCtnReplayRecord
{
    /// <summary>
    /// Interface intended for full parse of <see cref="CGameCtnReplayRecord"/> in TM1.0.
    /// </summary>
    public interface IFullTM10 : INodeFull<CGameCtnReplayRecord>, INodeHeader<CGameCtnReplayRecord>
    {
        CGameCtnChallenge Challenge { get; }
        ControlEntry[] ControlEntries { get; }
        TimeInt32 EventsDuration { get; }
        CGameCtnGhost[] Ghosts { get; }

        IEnumerable<CGameCtnGhost> GetGhosts();
    }
    
    /// <summary>
    /// Interface intended for full parse of <see cref="CGameCtnReplayRecord"/> in TMS.
    /// </summary>
    public interface IFullTMS : IFullTM10, IHeaderTMS
    {
        CGameCtnMediaClip? Clip { get; }
        CCtnMediaBlockEventTrackMania? Events { get; }
        string Game { get; }

        IEnumerable<CGameCtnGhost> GetGhosts(bool alsoInClips = true);
    }

    /// <summary>
    /// Interface intended for full parse of <see cref="CGameCtnReplayRecord"/> in TMU.
    /// </summary>
    public interface IFullTMU : INodeFull<CGameCtnReplayRecord>, IHeaderTMU
    {
        CGameCtnChallenge Challenge { get; }
        CGameCtnMediaClip? Clip { get; }
        CGameCtnGhost[] Ghosts { get; }
    }

    /// <summary>
    /// Interface intended for full parse of <see cref="CGameCtnReplayRecord"/> in TMUF.
    /// </summary>
    public interface IFullTMUF : IFullTMU
    {
        long[] Extras { get; }
    }

    /// <summary>
    /// Interface intended for full parse of <see cref="CGameCtnReplayRecord"/> in MP3 (TM2/SM).
    /// </summary>
    public interface IFullMP3 : IFullTMUF, IHeaderMP3
    {
    }

    /// <summary>
    /// Interface intended for full parse of <see cref="CGameCtnReplayRecord"/> in MP4 (TM2/SM).
    /// </summary>
    public interface IFullMP4 : IFullMP3, IHeaderMP4
    {
        CPlugEntRecordData RecordData { get; }
    }

    /// <summary>
    /// Interface intended for full parse of <see cref="CGameCtnReplayRecord"/> in TM2020.
    /// </summary>
    public interface IFullTM2020 : IFullMP4, IHeaderTM2020
    {
    }

    /// <summary>
    /// Interface intended for full parse of <see cref="CGameCtnReplayRecord"/>.
    /// </summary>
    public interface IFull : IFullTMS, IFullTM2020, IHeader
    {
    }
}
