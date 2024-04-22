namespace GBX.NET;

public readonly record struct GbxWriteSettings
{
    public byte? PackDescVersion { get; init; }
    public ClassIdRemapMode? ClassIdRemapMode { get; init; }
    /// <summary>
    /// Closes the stream after writing is finished. Default is <see langword="false"/>.
    /// </summary>
    public bool CloseStream { get; init; }
}