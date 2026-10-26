namespace GBX.NET;

[Obsolete("Use GbxMode instead.")]
public enum GbxUnknownByte : byte
{
    Unspecified,
    /// <summary>
    /// E(ditor).
    /// </summary>
    E = (byte)'E',
    /// <summary>
    /// R(elease).
    /// </summary>
    R = (byte)'R'
}
