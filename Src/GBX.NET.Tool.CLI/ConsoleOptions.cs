﻿namespace GBX.NET.Tool.CLI;

public sealed class ConsoleOptions
{
    public bool DisableUpdateCheck { get; set; }
    public bool SkipIntro { get; set; }
    public bool NoPause { get; set; }
    public bool DirectOutput { get; set; }
    public string? OutputDirPath { get; set; }
    public string? ConfigFilePath { get; set; }
}
