using Microsoft.Extensions.Logging;

namespace GBX.NET.Tool.CLI;

internal sealed class OutputDistributor
{
    private readonly ToolSettings toolSettings;
    private readonly ILogger logger;

    private readonly string outputDir;

    public OutputDistributor(string runningDir, ToolSettings toolSettings, ILogger logger)
    {
        this.toolSettings = toolSettings;
        this.logger = logger;

        outputDir = string.IsNullOrWhiteSpace(toolSettings.ConsoleSettings.OutputDirPath)
            ? Path.Combine(runningDir, "Output")
            : toolSettings.ConsoleSettings.OutputDirPath;

        if (toolSettings.ConsoleSettings.HidePath)
        {
            logger.LogDebug("Output directory: {OutputDir}", outputDir);
        }
        else
        {
            logger.LogInformation("Output directory: {OutputDir}", outputDir);
        }
    }

    public async Task DistributeOutputsAsync(IEnumerable<object> outputs, CancellationToken cancellationToken)
    {
        foreach (var output in outputs)
        {
            await DistributeOutputAsync(output, cancellationToken);
        }
    }

    public async ValueTask DistributeOutputAsync(object? output, CancellationToken cancellationToken)
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

                logger.LogInformation("Saving Gbx ({FilePath})...", filePath);

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

                logger.LogInformation("Gbx ({FilePath}) saved.", filePath);

                break;
            default:
                throw new NotSupportedException($"Output type '{output.GetType().Name}' is not supported.");
        }
    }
}