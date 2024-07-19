using GBX.NET.Tool.CLI.Inputs;

namespace GBX.NET.Tool.CLI;

internal sealed class ToolConfiguration
{
    public ConsoleOptions ConsoleOptions { get; init; } = new();
    public Dictionary<string, string> ConfigOverwrites { get; init; } = [];
    public IReadOnlyCollection<Input> Inputs { get; init; } = [];
}
