using GBX.NET.LZO;
using GBX.NET.Tool.CLI.Exceptions;
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
        settingsManager = new SettingsManager(runningDir);
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

        var introWriterTask = default(Task);

        if (!toolSettings.ConsoleSettings.SkipIntro)
        {
            introWriterTask = IntroWriter<T>.WriteIntroAsync(args);
        }

        // Request update info and additional stuff
        var updateChecker = toolSettings.ConsoleSettings.DisableUpdateCheck
            ? null
            : ToolUpdateChecker.Check(http);

        if (introWriterTask is not null)
        {
            await introWriterTask;
        }

        // Check for updates here if received. If not, check at the end of the tool execution
        var updateCheckCompleted = updateChecker is null
            || await updateChecker.TryCompareVersionAsync(cancellationToken);

        AnsiConsole.WriteLine();

        var logger = new SpectreConsoleLogger();

        // See what the tool can do
        var toolFunctionality = ToolFunctionalityResolver<T>.Resolve(logger);

        if (toolSettings.Inputs.Count == 0)
        {
            throw new ConsoleProblemException("No files passed to the tool.");
        }

        // If the tool has setup, apply tool things below to setup

        var toolInstanceMaker = new ToolInstanceMaker<T>(toolFunctionality, toolSettings, logger);
        var outputDistributor = new OutputDistributor(runningDir, toolSettings, logger);

        await foreach (var toolInstance in toolInstanceMaker.MakeToolInstancesAsync(cancellationToken))
        {
            // Load config into each instance (may be worth caching later)

            if (toolInstance is IConfigurable<Config> configurable)
            {
                var configName = string.IsNullOrWhiteSpace(toolSettings.ConsoleSettings.ConfigName) ? "Default"
                    : toolSettings.ConsoleSettings.ConfigName;

                await settingsManager.PopulateConfigAsync(configName, configurable.Config, options.JsonSerializerContext, cancellationToken);
            }

            // Run all produce methods in parallel and run mutate methods in sequence

            if (toolFunctionality.ProduceMethods.Length == 1)
            {
                var produceMethod = toolFunctionality.ProduceMethods[0];
                var result = produceMethod.Invoke(toolInstance, null);

                await outputDistributor.DistributeOutputAsync(result, cancellationToken);
            }
            else if (toolFunctionality.ProduceMethods.Length > 1)
            {
                var produceTasks = toolFunctionality.ProduceMethods
                    .Select(method => Task.Run(() => method.Invoke(toolInstance, null)))
                    .ToList();

                while (produceTasks.Count > 0)
                {
                    var completedTask = await Task.WhenAny(produceTasks);
                    produceTasks.Remove(completedTask);
                    await outputDistributor.DistributeOutputAsync(completedTask.Result, cancellationToken);
                }
            }

            if (toolFunctionality.MutateMethods.Length == 1)
            {
                var mutateMethod = toolFunctionality.MutateMethods[0];
                var result = mutateMethod.Invoke(toolInstance, null);

                await outputDistributor.DistributeOutputAsync(result, cancellationToken);
            }
            else if (toolFunctionality.MutateMethods.Length > 1)
            {
                foreach (var mutateMethod in toolFunctionality.MutateMethods)
                {
                    var result = mutateMethod.Invoke(toolInstance, null);

                    await outputDistributor.DistributeOutputAsync(result, cancellationToken);
                }
            }
        }

        // Check again for updates if not done before
        if (!updateCheckCompleted && updateChecker is not null)
        {
            updateCheckCompleted = await updateChecker.TryCompareVersionAsync(cancellationToken);
        }
    }

    private static void PressAnyKeyToContinue()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Markup("Press any key to continue...");
        Console.ReadKey(true);
    }
}
