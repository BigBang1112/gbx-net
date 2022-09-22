namespace GBX.NET.Engines.Game;

public partial class CGameCtnReplayRecord
{
    public interface IHeaderTMS : INodeHeader<CGameCtnReplayRecord>
    {
        /// <summary>
        /// Map UID, environment, and author login of the map the replay orients in.
        /// </summary>
        Ident MapInfo { get; }

        /// <summary>
        /// The record time.
        /// </summary>
        TimeInt32? Time { get; }

        /// <summary>
        /// Nickname of the record owner.
        /// </summary>
        string PlayerNickname { get; }

        /// <summary>
        /// XML replay information.
        /// </summary>
        string XML { get; }
    }

    public interface IHeaderTMU : IHeaderTMS
    {
        /// <summary>
        /// Login of the record owner.
        /// </summary>
        string PlayerLogin { get; }
    }

    public interface IHeaderTMUF : IHeaderTMU
    {
    }

    public interface IHeaderMP3 : IHeaderTMUF
    {
        /// <summary>
        /// Title pack the replay orients in.
        /// </summary>
        string TitleID { get; }

        int AuthorVersion { get; }

        /// <summary>
        /// Login of the replay creator.
        /// </summary>
        string AuthorLogin { get; }

        /// <summary>
        /// Nickname of the replay creator.
        /// </summary>
        string AuthorNickname { get; }

        /// <summary>
        /// Zone of the replay creator.
        /// </summary>
        string AuthorZone { get; }

        string AuthorExtraInfo { get; }
    }

    public interface IHeaderMP4 : IHeaderMP3
    {
    }

    public interface IHeaderTM2020 : IHeaderMP4
    {
    }

    public interface IHeader : IHeaderTM2020
    {
    }
}
