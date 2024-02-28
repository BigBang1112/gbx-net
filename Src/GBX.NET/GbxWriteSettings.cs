namespace GBX.NET;

public readonly record struct GbxWriteSettings
{
    public ClassIdRemapMode ClassIdRemapMode { get; init; }
    public bool LeaveOpen { get; init; }
}