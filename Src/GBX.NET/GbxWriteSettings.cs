namespace GBX.NET;

public readonly record struct GbxWriteSettings
{
    public byte? PackDescVersion { get; init; }
    public ClassIdRemapMode? ClassIdRemapMode { get; init; }
    /// <summary>
    /// Leave the stream open after writing.
    /// </summary>
    public bool LeaveOpen { get; init; }
}