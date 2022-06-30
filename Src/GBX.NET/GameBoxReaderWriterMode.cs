namespace GBX.NET;

/// <summary>
/// Reader-writer mode.
/// </summary>
[Flags]
public enum GameBoxReaderWriterMode
{
    /// <summary>
    /// Read mode.
    /// </summary>
    Read = 1,
    /// <summary>
    /// Write mode.
    /// </summary>
    Write = 2
}
