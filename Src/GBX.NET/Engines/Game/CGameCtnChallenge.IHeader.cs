namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallenge
{
    public interface IHeader : INodeHeader<CGameCtnChallenge>
    {
        /// <summary>
        /// Time of the bronze medal.
        /// </summary>
        TimeInt32? TMObjective_BronzeTime { get; set; }

        /// <summary>
        /// Time of the silver medal.
        /// </summary>
        TimeInt32? TMObjective_SilverTime { get; set; }

        /// <summary>
        /// Time of the gold medal.
        /// </summary>
        TimeInt32? TMObjective_GoldTime { get; set; }

        /// <summary>
        /// Time of the author medal.
        /// </summary>
        TimeInt32? TMObjective_AuthorTime { get; set; }

        /// <summary>
        /// Display cost of the track (or copper cost) explaining the performance of the map.
        /// </summary>
        int? Cost { get; set; }

        /// <summary>
        /// Usually author time or stunts score.
        /// </summary>
        int? AuthorScore { get; set; }

        /// <summary>
        /// In which editor settings the map was made.
        /// </summary>
        EditorMode Editor { get; set; }

        /// <summary>
        /// If the map was made using the simple editor.
        /// </summary>
        bool CreatedWithSimpleEditor { get; }

        /// <summary>
        /// If the map uses ghost blocks.
        /// </summary>
        bool HasGhostBlocks { get; }

        /// <summary>
        /// Map type in which the track was validated in.
        /// </summary>
        PlayMode? Mode { get; set; }

        /// <summary>
        /// If the map is a multilap.
        /// </summary>
        bool? TMObjective_IsLapRace { get; set; }

        /// <summary>
        /// Number of laps.
        /// </summary>
        int? TMObjective_NbLaps { get; set; }

        /// <summary>
        /// Number of checkpoints.
        /// </summary>
        int? NbCheckpoints { get; set; }

        /// <summary>
        /// Map UID, environment, and author login.
        /// </summary>
        Ident MapInfo { get; set; }

        /// <summary>
        /// The map's environment.
        /// </summary>
        Id Collection { get; set; }

        /// <summary>
        /// The map's UID.
        /// </summary>
        string MapUid { get; set; }

        /// <summary>
        /// Login of the map author.
        /// </summary>
        string AuthorLogin { get; set; }

        /// <summary>
        /// Nickname of the map author.
        /// </summary>
        string? AuthorNickname { get; set; }

        /// <summary>
        /// Zone of the map author.
        /// </summary>
        string? AuthorZone { get; set; }

        string? AuthorExtraInfo { get; set; }

        /// <summary>
        /// The map's name.
        /// </summary>
        string MapName { get; set; }

        /// <summary>
        /// The map's intended use.
        /// </summary>
        MapKind Kind { get; set; }

        /// <summary>
        /// Password of the map used by older maps.
        /// </summary>
        string? Password { get; set; }

        /// <summary>
        /// The map's decoration (time of the day or scenery)
        /// </summary>
        Ident? Decoration { get; set; }

        /// <summary>
        /// Target of the map.
        /// </summary>
        Vec2? MapCoordTarget { get; set; }

        /// <summary>
        /// Origin of the map.
        /// </summary>
        Vec2? MapCoordOrigin { get; set; }

        /// <summary>
        /// Name of the map type script.
        /// </summary>
        string? MapType { get; set; }

        /// <summary>
        /// Style of the map (Fullspeed, LOL, Tech), usually unused and defined by user.
        /// </summary>
        string? MapStyle { get; set; }

        /// <summary>
        /// UID of the lightmap data stored in cache.
        /// </summary>
        ulong? LightmapCacheUID { get; set; }

        /// <summary>
        /// Version of the lightmap calculation.
        /// </summary>
        byte? LightmapVersion { get; set; }

        /// <summary>
        /// Title pack the map was built in.
        /// </summary>
        string? TitleID { get; set; }

        /// <summary>
        /// XML track information and dependencies.
        /// </summary>
        string? XML { get; set; }

        /// <summary>
        /// The map's author comments.
        /// </summary>
        string? Comments { get; set; }

        /// <summary>
        /// Thumbnail JPEG data.
        /// </summary>
        byte[]? Thumbnail { get; set; }
    }
}
