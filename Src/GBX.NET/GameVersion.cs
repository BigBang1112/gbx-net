namespace GBX.NET;

/// <summary>
/// Set of flags for game versions in a bit field, allowing for multiple game versions to be represented simultaneously. 
/// The values for each version are distinct powers of 2, enabling bitwise operations for combining or checking specific versions.
/// </summary>
[Flags]
public enum GameVersion
{
    /// <summary>
    /// Unspecified game version
    /// </summary>
    Unspecified = 0,
    /// <summary>
    /// TrackMania (1.0)
    /// </summary>
    TM10 = 1,
    /// <summary>
    /// TrackMania Power Up
    /// </summary>
    TMPU = 2,
    /// <summary>
    /// TrackMania Sunrise
    /// </summary>
    TMS = 4,
    /// <summary>
    /// TrackMania Original
    /// </summary>
    TMO = 8,
    /// <summary>
    /// TrackMania Sunrise eXtreme
    /// </summary>
    TMSX = 16,
    /// <summary>
    /// TrackMania Nations ESWC
    /// </summary>
    TMNESWC = 32,
    /// <summary>
    /// TrackMania United
    /// </summary>
    TMU = 64,
    /// <summary>
    /// Virtual Skipper 5
    /// </summary>
    VSK5 = 128,
    /// <summary>
    /// TrackMania Forever
    /// </summary>
    TMF = 256,
    /// <summary>
    /// ManiaPlanet 1
    /// </summary>
    MP1 = 512,
    /// <summary>
    /// ManiaPlanet 2
    /// </summary>
    MP2 = 1024,
    /// <summary>
    /// ManiaPlanet 3
    /// </summary>
    MP3 = 2048,
    /// <summary>
    /// Trackmania Turbo
    /// </summary>
    TMT = 4096,
    /// <summary>
    /// ManiaPlanet 4
    /// </summary>
    MP4 = 8192,
    /// <summary>
    /// Trackmania (2020)
    /// </summary>
    TM2020 = 16384
}