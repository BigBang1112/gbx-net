namespace GBX.NET.Tool.CLI;

public sealed class ConsoleOptions
{
    public bool DisableUpdateCheck { get; set; }
    public bool SkipIntro { get; set; }
    public bool NoPause { get; set; }
    public string? ConfigFilePath { get; set; }
}
