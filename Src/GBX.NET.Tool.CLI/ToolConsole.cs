using GBX.NET.LZO;
using GBX.NET.Tool.CLI.Exceptions;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Tool.CLI;

public class ToolConsole<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicConstructors)] T> where T : class, ITool
{
    private readonly string[] args;
    private readonly HttpClient http;

    public ToolConsole(string[] args, HttpClient http)
    {
        this.args = args;
        this.http = http;
    }

    static ToolConsole()
    {
        Gbx.LZO = new Lzo();
    }

    [RequiresDynamicCode(DynamicCodeMessages.MakeGenericTypeMessage)]
    public static async Task<ToolConsoleRunResult<T>> RunAsync(string[] args)
    {
        using var http = new HttpClient();
        http.DefaultRequestHeaders.Add("User-Agent", "GBX.NET.Tool.CLI");

        using var cts = new CancellationTokenSource();

        var tool = new ToolConsole<T>(args, http);

        try
        {
            await tool.RunAsync(cts.Token);
        }
        catch (ConsoleProblemException ex)
        {
            AnsiConsole.MarkupInterpolated($"[yellow]{ex.Message}[/]");
        }
        catch (OperationCanceledException)
        {
            AnsiConsole.Markup("[yellow]Operation canceled.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Markup("Press any key to continue...");
        Console.ReadKey(true);

        return new ToolConsoleRunResult<T>(tool);
    }

    [RequiresDynamicCode(DynamicCodeMessages.MakeGenericTypeMessage)]
    private async Task RunAsync(CancellationToken cancellationToken)
    {
        var argsResolver = new ArgsResolver(args, http);
        var toolConfig = argsResolver.Resolve();

        // Request update info and additional stuff
        var updateChecker = toolConfig.ConsoleOptions.DisableUpdateCheck
            ? null
            : ToolUpdateChecker.Check(http);
        
        await IntroWriter<T>.WriteIntroAsync(args);

        // Check for updates here if received. If not, check at the end of the tool execution
        var updateCheckCompleted = updateChecker is null
            || await updateChecker.TryCompareVersionAsync(cancellationToken);

        AnsiConsole.WriteLine();

        var logger = new SpectreConsoleLogger();

        if (toolConfig.Inputs.Count == 0)
        {
            throw new ConsoleProblemException("No files passed to the tool.");
        }

        // If the tool has setup, apply tool things below to setup

        // See what the tool can do
        var toolFunctionality = ToolFunctionalityResolver<T>.Resolve(logger);

        var toolInstanceMaker = new ToolInstanceMaker<T>(toolFunctionality, toolConfig, logger);

        await foreach (var toolInstance in toolInstanceMaker.MakeToolInstancesAsync(cancellationToken))
        {
            // Run all produce methods in parallel and run mutate methods in sequence
            
        }

        // Check again for updates if not done before
        if (!updateCheckCompleted && updateChecker is not null)
        {
            updateCheckCompleted = await updateChecker.TryCompareVersionAsync(cancellationToken);
        }

        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                // Define tasks
                var task1 = ctx.AddTask("[green]Reticulating splines[/]");
                var task2 = ctx.AddTask("[green]Folding space[/]");

                while (!ctx.IsFinished)
                {
                    // Simulate some work
                    await Task.Delay(250);

                    // Increment
                    task1.Increment(1.5);
                    task2.Increment(0.5);
                }
            });
    }
}
