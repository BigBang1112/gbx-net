using GBX.NET.LZO;
using GBX.NET.Tool.CLI.Exceptions;
using GBX.NET.Tool.CLI.Inputs;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

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

        // If the tool has setup, apply tool things below to setup

        // See what the tool can do
        var toolFunctionality = ToolFunctionalityResolver<T>.Resolve(logger);

        var resolvedInputsDict = new Dictionary<Input, object?>();
        var pickedCtor = default(ConstructorInfo);
        var paramsForCtor = new List<object>();

        foreach (var constructor in toolFunctionality.Constructors)
        {
            var isInvalidCtor = false;
            var parameters = constructor.GetParameters();

            if (parameters.Length == 0)
            {
                // inputless tools?
                continue;
            }

            // issue here is with multiple inputs where it should repeat tool constructors
            var inputEnumerator = toolConfig.Inputs.GetEnumerator();
            if (!inputEnumerator.MoveNext())
            {
                continue;
            }
            var input = inputEnumerator.Current;

            foreach (var parameter in parameters)
            {
                var type = parameter.ParameterType;

                if (type == typeof(ILogger))
                {
                    paramsForCtor.Add(logger);
                    continue;
                }

                if (type == typeof(Gbx))
                {
                    var resolvedObject = await input.ResolveAsync(cancellationToken);

                    if (resolvedObject is not Gbx)
                    {
                        isInvalidCtor = true;
                        break;
                    }

                    paramsForCtor.Add(resolvedObject);
                    continue;
                }

                if (!type.IsGenericType)
                {
                    continue;
                }

                var typeDef = type.GetGenericTypeDefinition();

                if (typeDef == typeof(Gbx<>))
                {
                    var nodeType = type.GetGenericArguments()[0];

                    var resolvedObject = await input.ResolveAsync(cancellationToken);

                    if (resolvedObject is not Gbx gbx || gbx.Node?.GetType() != nodeType)
                    {
                        isInvalidCtor = true;
                        break;
                    }

                    paramsForCtor.Add(resolvedObject);
                    continue;
                }

                if (typeDef == typeof(IEnumerable<>))
                {
                    var elementType = type.GetGenericArguments()[0];
                    continue;
                }
            }

            if (isInvalidCtor)
            {
                paramsForCtor.Clear();
            }
            else
            {
                pickedCtor = constructor;
                break;
            }
        }

        // Check again for updates if not done before
        if (!updateCheckCompleted && updateChecker is not null)
        {
            updateCheckCompleted = await updateChecker.TryCompareVersionAsync(cancellationToken);
        }

        // Instantiate the tool
        if (pickedCtor is null)
        {
            throw new ConsoleProblemException("Invalid files passed to the tool.");
        }

        var toolInstance = pickedCtor.Invoke(paramsForCtor.ToArray());

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
