﻿using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GBX.NET.Tool.CLI;

public sealed class OutputDistributor
{
    private readonly ToolSettings toolSettings;
    private readonly ILogger logger;

    private readonly string outputDir;

    public OutputDistributor(ToolSettings toolSettings, string runningDir, ILogger logger)
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

    public async Task DistributeOutputsAsync(IEnumerable<object> outputs, bool mutating, CancellationToken cancellationToken)
    {
        var watch = default(Stopwatch);

        if (outputs is not System.Collections.ICollection)
        {
            watch = Stopwatch.StartNew();
        }

        foreach (var output in outputs)
        {
            if (watch is not null)
            {
                if (mutating)
                {
                    logger.LogInformation("Mutated in {Milliseconds}ms!", watch.ElapsedMilliseconds);
                }
                else
                {
                    logger.LogInformation("Produced in {Milliseconds}ms!", watch.ElapsedMilliseconds);
                }
            }

            await DistributeOutputAsync(output, mutating, cancellationToken);
        }
    }

    public async ValueTask DistributeOutputAsync(object? output, bool mutating, CancellationToken cancellationToken)
    {
        switch (output)
        {
            case IEnumerable<object> objList:
                await DistributeOutputsAsync(objList, mutating, cancellationToken);
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

                var watch = Stopwatch.StartNew();

                await using (var fs = new FileStream(finalPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                {
                    gbx.Save(fs);
                }

                logger.LogInformation("Gbx ({FilePath}) saved in {Milliseconds}ms.", filePath, watch.ElapsedMilliseconds);

                break;
            default:
                throw new NotSupportedException($"Output type '{output.GetType().Name}' is not supported.");
        }
    }
}