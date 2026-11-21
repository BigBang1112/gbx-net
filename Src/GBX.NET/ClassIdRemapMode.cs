namespace GBX.NET;

public enum ClassIdRemapMode
{
    /// <summary>
    /// Latest class IDs.
    /// </summary>
    Latest,
    /// <summary>
    /// The original GBX.NET legacy mapping with the 2008 <see cref="CGameCtnCollector"/> ID.
    /// </summary>
    Id2008,
    /// <summary>
    /// The original GBX.NET legacy write mapping.
    /// </summary>
    Id2006,
    /// <summary>Earlier media and scene class IDs still found in later packs.</summary>
    EarlyMedia,
    /// <summary>The <see cref="EarlyMedia"/> mapping with the original CMotionManagerWeathers ID.</summary>
    EarlyMediaWithWeather,
    /// <summary>The CPlugShaderSprite root class ID (0x09068000).</summary>
    ShaderSprite,
    /// <summary>The CPlugShaderMaterial root class ID (0x09063000).</summary>
    ShaderMaterial,
    /// <summary>The collector and item IDs used by early ManiaPlanet builds.</summary>
    EarlyManiaPlanet,
    /// <summary>Class IDs used by TrackMania Forever.</summary>
    TrackManiaForever,
    /// <summary>Construction class IDs used by TrackMania 1.0 and Power Up.</summary>
    EarlyTrackMania,
    /// <summary>Construction class IDs with the derived replay root used by Sunrise eXtreme and Nations ESWC.</summary>
    TrackManiaSunriseExtreme,
    /// <summary>Construction class IDs with derived replay and profile roots, used by United and Virtual Skipper 5.</summary>
    TrackManiaUnited
}
