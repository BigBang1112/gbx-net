using GBX.NET.Tool.CLI.Exceptions;
using Microsoft.Extensions.Logging;
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
            AnsiConsole.WriteLine();
            AnsiConsole.Markup("Press any key to continue...");
            Console.ReadKey(true);
        }
        catch (OperationCanceledException)
        {
            AnsiConsole.Markup("[yellow]Operation canceled.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.Markup("Press any key to continue...");
            Console.ReadKey(true);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            AnsiConsole.WriteLine();
            AnsiConsole.Markup("Press any key to continue...");
            Console.ReadKey(true);
        }

        return new ToolConsoleRunResult<T>(tool);
    }

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        // Request update info and additional stuff
        var updateChecker = ToolUpdateChecker.Check(http);
        
        await IntroWriter<T>.WriteIntroAsync(args);

        // Check for updates here if received. If not, check at the end of the tool execution
        var updateCheckCompleted = await updateChecker.TryCompareVersionAsync(cancellationToken);

        AnsiConsole.WriteLine();

        var logger = new SpectreConsoleLogger();

        // If the tool has setup, apply tool things below to setup

        // See what the tool can do
        var toolFunctionality = ToolFunctionalityResolver<T>.Resolve(logger);

        // Read the files from the arguments
        // Quickly invalidate ones that do not meet functionality

        // Check again for updates if not done before
        if (!updateCheckCompleted)
        {
            updateCheckCompleted = await updateChecker.TryCompareVersionAsync(cancellationToken);
        }

        // Instantiate the tool

        // Run all produce methods in parallel and run mutate methods in sequence

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
