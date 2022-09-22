namespace GBX.NET.Engines.Game;

public partial class CGameCtnReplayRecord
{
    /// <summary>
    /// Interface intended for full parse of <see cref="CGameCtnReplayRecord"/> in TM1.0.
    /// </summary>
    public interface IFullTM10 : INodeFull<CGameCtnReplayRecord>
    {
        /// <summary>
        /// The map the replay orients in.
        /// </summary>
        /// <exception cref="PropertyNullException"></exception>
        CGameCtnChallenge Challenge { get; }
        
        /// <summary>
        /// Inputs (keyboard, pad, wheel) of the replay. Can be null if <see cref="EventsDuration"/> is 0, which can happen when you save the replay in editor.
        /// </summary>
        ControlEntry[]? ControlEntries { get; }
        
        /// <summary>
        /// Duration of events in the replay (range of detected inputs). This can be <see cref="TimeInt32.Zero"/> if the replay was driven in editor.
        /// </summary>
        /// <exception cref="PropertyNullException"></exception>
        TimeInt32 EventsDuration { get; }
        
        /// <summary>
        /// Ghosts in the replay.
        /// </summary>
        /// <exception cref="PropertyNullException"></exception>
        CGameCtnGhost[] Ghosts { get; }

        IEnumerable<CGameCtnGhost> GetGhosts();
    }
    
    /// <summary>
    /// Interface intended for full parse of <see cref="CGameCtnReplayRecord"/> in TMS.
    /// </summary>
    public interface IFullTMS : IFullTM10, IHeaderTMS
    {
        /// <summary>
        /// MediaTracker clip of the replay.
        /// </summary>
        CGameCtnMediaClip? Clip { get; }
        
        /// <summary>
        /// Events occuring during the replay.
        /// </summary>
        CCtnMediaBlockEventTrackMania? Events { get; }
        
        /// <summary>
        /// Events occuring during the replay.
        /// </summary>
        CCtnMediaBlockUiTMSimpleEvtsDisplay? SimpleEventsDisplay { get; }
        
        /// <exception cref="PropertyNullException"></exception>
        string Game { get; }

        IEnumerable<CGameCtnGhost> GetGhosts(bool alsoInClips = true);
    }

    /// <summary>
    /// Interface intended for full parse of <see cref="CGameCtnReplayRecord"/> in TMU.
    /// </summary>
    public interface IFullTMU : INodeFull<CGameCtnReplayRecord>, IHeaderTMU
    {
        /// <summary>
        /// The map the replay orients in.
        /// </summary>
        /// <exception cref="PropertyNullException"></exception>
        CGameCtnChallenge Challenge { get; }
        
        /// <summary>
        /// MediaTracker clip of the replay.
        /// </summary>
        CGameCtnMediaClip? Clip { get; }
        
        /// <summary>
        /// Ghosts in the replay.
        /// </summary>
        /// <remarks>Some ghosts can be considered as <see cref="CGameCtnMediaBlockGhost"/>. See <see cref="Clip"/>.</remarks>
        /// <exception cref="PropertyNullException"></exception>
        CGameCtnGhost[] Ghosts { get; }
    }

    /// <summary>
    /// Interface intended for full parse of <see cref="CGameCtnReplayRecord"/> in TMUF.
    /// </summary>
    public interface IFullTMUF : IFullTMU, IHeaderTMUF
    {
        /// <exception cref="PropertyNullException"></exception>
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
        /// <exception cref="PropertyNullException"></exception>
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
