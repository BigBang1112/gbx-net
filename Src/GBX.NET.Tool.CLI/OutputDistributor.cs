namespace GBX.NET.Tool.CLI;

internal sealed class OutputDistributor
{
    private readonly string runningDir;
    private readonly ToolSettings toolSettings;
    private readonly SpectreConsoleLogger logger;

    private readonly string outputDir;

    public OutputDistributor(string runningDir, ToolSettings toolSettings, SpectreConsoleLogger logger)
    {
        this.runningDir = runningDir;
        this.toolSettings = toolSettings;
        this.logger = logger;

        outputDir = string.IsNullOrWhiteSpace(toolSettings.ConsoleSettings.OutputDirPath)
            ? Path.Combine(runningDir, "Output")
            : toolSettings.ConsoleSettings.OutputDirPath;
    }

    public async Task DistributeOutputsAsync(IEnumerable<object> outputs, CancellationToken cancellationToken)
    {
        foreach (var output in outputs)
        {
            await DistributeOutputAsync(output, cancellationToken);
        }
    }

    private async Task DistributeOutputAsync(object output, CancellationToken cancellationToken)
    {
        switch (output)
        {
            case IEnumerable<object> objList:
                await DistributeOutputsAsync(objList, cancellationToken);
                break;
            case null:
                break;
            case Gbx gbx:
                var filePath = gbx.FilePath ?? "Generated.Gbx";

                if (toolSettings.ConsoleSettings.DirectOutput)
                {
                    filePath = Path.GetFileName(filePath);
                }

                var finalPath = Path.Combine(outputDir, filePath);
                var dirPath = Path.GetDirectoryName(finalPath);

                if (!string.IsNullOrWhiteSpace(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                await using (var fs = new FileStream(finalPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                {
                    gbx.Save(fs);
                }

                break;
            default:
                throw new NotSupportedException($"Output type '{output.GetType().Name}' is not supported.");
        }
    }
}