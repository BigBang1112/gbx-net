using GBX.NET.LZO;
using GBX.NET.Tool.CLI.Exceptions;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Tool.CLI;

/// <summary>
/// Represents the CLI implementation of GBX.NET tool.
/// </summary>
/// <typeparam name="T">Tool type.</typeparam>
public class ToolConsole<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods)] T> where T : class, ITool
{
    private readonly string[] args;
    private readonly HttpClient http;
    private readonly ToolConsoleOptions options;

    private readonly string runningDir;
    private readonly SettingsManager settingsManager;
    private readonly ArgsResolver argsResolver;

    private bool noPause;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToolConsole{T}"/> class.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <param name="http">HTTP client to use for requests.</param>
    /// <param name="options">Options for the tool console. These should be hardcoded for purpose.</param>
    /// <exception cref="ArgumentNullException"><paramref name="args"/>, <paramref name="http"/>, or <paramref name="options"/> is null.</exception>
    public ToolConsole(string[] args, HttpClient http, ToolConsoleOptions options)
    {
        this.args = args ?? throw new ArgumentNullException(nameof(args));
        this.http = http ?? throw new ArgumentNullException(nameof(http));
        this.options = options ?? throw new ArgumentNullException(nameof(options));

        runningDir = AppDomain.CurrentDomain.BaseDirectory;
        settingsManager = new SettingsManager(runningDir, options.JsonContext, options.JsonOptions);
        argsResolver = new ArgsResolver(args, http);
    }

    static ToolConsole()
    {
        Gbx.LZO = new Lzo();
    }

    /// <summary>
    /// Runs the tool CLI implementation with the specified arguments.
    /// </summary>
    /// <param name="args">Command line arguments. Use the 'args' keyword here.</param>
    /// <param name="options">Options for the tool console. These should be hardcoded for purpose.</param>
    /// <returns>Result of the tool execution (if wanted to use later).</returns>
    [RequiresDynamicCode(DynamicCodeMessages.DynamicRunMessage)]
    [RequiresUnreferencedCode(DynamicCodeMessages.UnreferencedRunMessage)]
    public static async Task<ToolConsoleRunResult<T>> RunAsync(string[] args, ToolConsoleOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(args);

        using var http = new HttpClient();
        http.DefaultRequestHeaders.UserAgent.ParseAdd("GBX.NET.Tool.CLI");

        using var cts = new CancellationTokenSource();

        var tool = new ToolConsole<T>(args, http, options ?? new());

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

        if (!tool.noPause)
        {
            PressAnyKeyToContinue();
        }

        return new ToolConsoleRunResult<T>(tool);
    }

    [RequiresDynamicCode(DynamicCodeMessages.DynamicRunMessage)]
    [RequiresUnreferencedCode(DynamicCodeMessages.UnreferencedRunMessage)]
    private async Task RunAsync(CancellationToken cancellationToken)
    {
        // Load console settings from file if exists otherwise create one
        var consoleSettings = await settingsManager.GetOrCreateFileAsync("ConsoleSettings",
            ToolJsonContext.Default.ConsoleSettings,
            cancellationToken: cancellationToken);

        var toolSettings = argsResolver.Resolve(consoleSettings);

        // Not ideal mutative state, but it's fine for now
        noPause = toolSettings.ConsoleSettings.NoPause;

        var logger = new SpectreConsoleLogger(toolSettings.ConsoleSettings.LogLevel);

        logger.LogDebug("Running directory: {RunningDir}", runningDir);
        logger.LogDebug("CLI settings from file: {Settings}", toolSettings.ConsoleSettings);
        logger.LogDebug("Settings/config overwrites: {Overwrites}", toolSettings.ConfigOverwrites.Count == 0
            ? "None"
            : string.Join(", ", toolSettings.ConfigOverwrites.Select(kv => $"{kv.Key}={kv.Value}")));
        logger.LogDebug("Inputs: {Inputs}", toolSettings.Inputs.Count == 0
            ? "None"
            : string.Join(", ", toolSettings.Inputs));
        logger.LogTrace("No pause: {NoPause}", noPause);

        var introWriterTask = default(Task);

        if (!toolSettings.ConsoleSettings.SkipIntro)
        {
            introWriterTask = IntroWriter<T>.WriteIntroAsync(args, toolSettings);
        }

        logger.LogTrace("Checking for updates...");

        // Request update info and additional stuff
        var updateChecker = toolSettings.ConsoleSettings.DisableUpdateCheck
            ? null
            : ToolUpdateChecker.Check(http, cancellationToken);

        if (introWriterTask is not null)
        {
            await introWriterTask;
            logger.LogTrace("Intro finished.");
        }

        // Check for updates here if received. If not, check at the end of the tool execution
        var updateCheckCompleted = updateChecker is null
            || await updateChecker.TryCompareVersionAsync();

        logger.LogDebug("Update check completed: {UpdateCheckCompleted}", updateCheckCompleted);

        AnsiConsole.WriteLine();

        // See what the tool can do
        var toolFunctionality = ToolFunctionalityResolver<T>.Resolve(logger);

        if (toolSettings.Inputs.Count == 0)
        {
            // write tool specific intro
            if (!string.IsNullOrWhiteSpace(options.IntroText))
            {
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine(options.IntroText);
            }

            if (!updateCheckCompleted && updateChecker is not null)
            {
                await updateChecker.CompareVersionAsync();
            }

            AnsiConsole.WriteLine();

            throw new ConsoleProblemException("No files were passed to the tool.\nPlease drag and drop files onto the executable, or include the input paths as the command line arguments.\nFile paths, directory paths, or URLs are supported in any order.");
        }

        // If the tool has setup, apply tool things below to setup

        var toolInstanceMaker = new ToolInstanceMaker<T>(toolFunctionality, toolSettings, logger);
        var outputDistributor = new OutputDistributor(runningDir, toolSettings, logger);

        AnsiConsole.WriteLine();
        logger.LogInformation("Starting tool instance creation...");
        AnsiConsole.WriteLine();
        var counter = 0;

        await foreach (var toolInstance in toolInstanceMaker.MakeToolInstancesAsync(cancellationToken))
        {
            counter++;
            logger.LogInformation("Tool instance #{Number} created.", counter);

            // Load config into each instance (may be worth caching later)

            if (toolInstance is IConfigurable<Config> configurable)
            {
                var configName = string.IsNullOrWhiteSpace(toolSettings.ConsoleSettings.ConfigName) ? "Default"
                    : toolSettings.ConsoleSettings.ConfigName;

                logger.LogInformation("Populating tool config (name: {ConfigName}, type: {ConfigType})...", configName, configurable.Config.GetType());

                await settingsManager.PopulateConfigAsync(configName, configurable.Config, cancellationToken);
            }

            // Run all produce methods in parallel and run mutate methods in sequence

            if (toolFunctionality.ProduceMethods.Length == 1)
            {
                logger.LogInformation("Producing...");

                var produceMethod = toolFunctionality.ProduceMethods[0];
                var result = produceMethod.Invoke(toolInstance, null);

                if (result is IEnumerable<object>)
                {
                    logger.LogInformation("Producing for each distributed output...");
                }
                else
                {
                    logger.LogInformation("Produced! Distributing output...");
                }

                await outputDistributor.DistributeOutputAsync(result, cancellationToken);
            }
            else if (toolFunctionality.ProduceMethods.Length > 1)
            {
                logger.LogInformation("Producing ({Count} methods)...", toolFunctionality.ProduceMethods.Length);

                var produceTasks = toolFunctionality.ProduceMethods
                    .Select(method => Task.Run(() => method.Invoke(toolInstance, null)))
                    .ToList();

                while (produceTasks.Count > 0)
                {
                    var completedTask = await Task.WhenAny(produceTasks);
                    produceTasks.Remove(completedTask);

                    var result = completedTask.Result;

                    if (result is IEnumerable<object>)
                    {
                        if (produceTasks.Count > 0)
                        {
                            logger.LogInformation("Producing for each distributed output ({Count} remaining)...", produceTasks.Count);
                        }
                        else
                        {
                            logger.LogInformation("Producing for each distributed output (last output)...");
                        }
                    }
                    else
                    {
                        if (produceTasks.Count > 0)
                        {
                            logger.LogInformation("Produced! Distributing output ({Count} remaining)...", produceTasks.Count);
                        }
                        else
                        {
                            logger.LogInformation("Produced! Distributing last output...");
                        }
                    }

                    await outputDistributor.DistributeOutputAsync(result, cancellationToken);
                }
            }

            if (toolFunctionality.MutateMethods.Length == 1)
            {
                logger.LogInformation("Mutating...");

                var mutateMethod = toolFunctionality.MutateMethods[0];
                var result = mutateMethod.Invoke(toolInstance, null);

                if (result is IEnumerable<object>)
                {
                    logger.LogInformation("Mutationing while distributing output...");
                }
                else
                {
                    logger.LogInformation("Mutated! Distributing output...");
                }

                await outputDistributor.DistributeOutputAsync(result, cancellationToken);
            }
            else if (toolFunctionality.MutateMethods.Length > 1)
            {
                for (int i = 0; i < toolFunctionality.MutateMethods.Length; i++)
                {
                    logger.LogInformation("Mutating ({Count}/{Total})...", i + 1, toolFunctionality.MutateMethods.Length);

                    var mutateMethod = toolFunctionality.MutateMethods[i];
                    var result = mutateMethod.Invoke(toolInstance, null);

                    if (result is IEnumerable<object>)
                    {
                        logger.LogInformation("Mutating while distributing output...");
                    }
                    else
                    {
                        logger.LogInformation("Mutated! Distributing output...");
                    }

                    await outputDistributor.DistributeOutputAsync(result, cancellationToken);
                }
            }

            logger.LogInformation("Tool instance #{Number} completed.", counter);
        }

        AnsiConsole.WriteLine();

        logger.LogInformation("Completed!");

        // Check again for updates if not done before
        if (!updateCheckCompleted && updateChecker is not null)
        {
            await updateChecker.CompareVersionAsync();
        }
    }

    private static void PressAnyKeyToContinue()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Markup("Press any key to continue...");
        Console.ReadKey(true);
    }
}
