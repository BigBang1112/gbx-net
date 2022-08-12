namespace GBX.NET.Engines.Game;

public partial class CGameCtnReplayRecord
{
    public interface IHeader : INodeHeader<CGameCtnReplayRecord>
    {
        /// <summary>
        /// Map UID, environment, and author login of the map the replay orients in.
        /// </summary>
        Ident? MapInfo { get; }

        /// <summary>
        /// The record time.
        /// </summary>
        TimeInt32? Time { get; }

        /// <summary>
        /// Nickname of the record owner.
        /// </summary>
        string? PlayerNickname { get; }

        /// <summary>
        /// Login of the record owner.
        /// </summary>
        string? PlayerLogin { get; }

        /// <summary>
        /// Title pack the replay orients in.
        /// </summary>
        string? TitleID { get; }

        /// <summary>
        /// XML replay information.
        /// </summary>
        string? XML { get; }

        int? AuthorVersion { get; }

        /// <summary>
        /// Login of the replay creator.
        /// </summary>
        string? AuthorLogin { get; }

        /// <summary>
        /// Nickname of the replay creator.
        /// </summary>
        string? AuthorNickname { get; }

        /// <summary>
        /// Zone of the replay creator.
        /// </summary>
        string? AuthorZone { get; }

        string? AuthorExtraInfo { get; }
    }
}
