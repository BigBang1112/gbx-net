namespace GBX.NET.Serialization;

public enum AutoPropertyMode
{
    /// <summary>
    /// Generator will pick the best option based on the structure kind.
    /// </summary>
    Default,
    /// <summary>
    /// Force generation of auto properties.
    /// </summary>
    ForceAutoProperties,
    /// <summary>
    /// Disable generation of auto properties, instead write them down manually.
    /// </summary>
    DisableAutoProperties
}
