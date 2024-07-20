namespace GBX.NET.Tool.CLI;

public sealed class ConsoleSettings
{
    public bool DisableUpdateCheck { get; set; }
    public bool SkipIntro { get; set; }
    public bool NoPause { get; set; }
    public bool DirectOutput { get; set; } = true;
    public string? OutputDirPath { get; set; }
    public string? ConfigName { get; set; }
}
