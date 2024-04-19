namespace GBX.NET;

public readonly record struct GbxWriteSettings
{
    public byte? PackDescVersion { get; init; }
    public ClassIdRemapMode? ClassIdRemapMode { get; init; }
    public bool LeaveOpen { get; init; }
}