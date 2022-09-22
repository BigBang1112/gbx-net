namespace GBX.NET.Engines.Game;

public partial class CGameCtnReplayRecord
{
    internal interface IFull
    {
        string? AuthorExtraInfo { get; }
        string? AuthorLogin { get; }
        string? AuthorNickname { get; }
        int? AuthorVersion { get; }
        string? AuthorZone { get; }
        CGameCtnMediaClip? Clip { get; }
        ControlEntry[]? ControlEntries { get; }
        CCtnMediaBlockEventTrackMania? Events { get; }
        TimeInt32? EventsDuration { get; }
        long[]? Extras { get; }
        string? Game { get; }
        CGameCtnGhost[] Ghosts { get; }
        HeaderChunkSet HeaderChunks { get; }
        CGameCtnChallenge Challenge { get; }
        Ident? MapInfo { get; }
        string? PlayerLogin { get; }
        string? PlayerNickname { get; }
        CPlugEntRecordData? RecordData { get; }
        CCtnMediaBlockUiTMSimpleEvtsDisplay? SimpleEventsDisplay { get; }
        TimeInt32? Time { get; }
        string? TitleID { get; }
        string? XML { get; }

        IEnumerable<CGameCtnGhost> GetGhosts(bool alsoInClips = true);
    }
}
