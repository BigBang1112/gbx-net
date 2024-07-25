using Spectre.Console;
using System.Reflection;
using System.Runtime.Versioning;

namespace GBX.NET.Tool.CLI;

internal static class IntroWriter<T> where T : ITool
{
    public static async Task WriteIntroAsync(string[] args, ToolSettings toolSettings)
    {
        var assembly = typeof(T).Assembly;
        var assemblyName = assembly.GetName();

        var toolName = assemblyName.Name ?? "Tool";

        if (toolName.Length <= 16)
        {
            await AnsiConsole.Live(new FigletText(toolName.Substring(0, 1))
                .Centered())
                .StartAsync(async context =>
                {
                    for (var i = 1; i < toolName.Length; i++)
                    {
                        await Task.Delay(30);
                        context.UpdateTarget(new FigletText(toolName.Substring(0, i + 1))
                            .Centered()
                            .Color(Spectre.Console.Color.Aqua));
                    }
                });
        }
        else
        {
            AnsiConsole.Write(
                new Rule(toolName));
        }

        AnsiConsole.Write(new Rule("GBX.NET tool made with GBX.NET.Tool library set")
            .LeftJustified()
            .DoubleBorder()
            .RuleStyle("yellow dim"));

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupInterpolated($"[grey]Tool:[/] [yellow]{toolName}[/] ");
        AnsiConsole.MarkupLineInterpolated($"[yellow]{(Environment.Is64BitProcess ? "(x64)" : "(x86)")}[/]");

        if (assemblyName.Version is not null)
        {
            AnsiConsole.MarkupLineInterpolated($"[grey]Version:[/] [yellow]{assemblyName.Version.ToString(3)}[/]");
        }

        foreach (var att in assembly.GetCustomAttributes())
        {
            switch (att)
            {
                case TargetFrameworkAttribute frameworkAtt:
                    AnsiConsole.MarkupLineInterpolated($"[grey]Framework:[/] [yellow]{frameworkAtt.FrameworkDisplayName}[/]");
                    break;
                case AssemblyConfigurationAttribute configurationAtt:
                    AnsiConsole.MarkupLineInterpolated($"[grey]Configuration:[/] [yellow]{configurationAtt.Configuration}[/]");
                    break;
            }
        }

#pragma warning disable IL3000 // Avoid accessing Assembly file path when publishing as a single file
        if (!string.IsNullOrEmpty(assembly.Location))
        {
            AnsiConsole.MarkupLineInterpolated($"[grey]Assembly:[/] [yellow]{Path.GetFileName(assembly.Location)}[/]");
        }
#pragma warning restore IL3000 // Avoid accessing Assembly file path when publishing as a single file

        if (args.Length > 0)
        {
            AnsiConsole.MarkupLineInterpolated($"[grey]Arguments:[/] [yellow]{string.Join(' ', args)}[/]");
        }

        AnsiConsole.MarkupLineInterpolated($"[grey]Started:[/] [yellow]{DateTime.Now:yyyy-MM-dd HH:mm:ss}[/]");
        AnsiConsole.MarkupInterpolated($"[grey]OS:[/] [yellow]{Environment.OSVersion}[/] ");
        AnsiConsole.MarkupLineInterpolated($"[yellow]{(Environment.Is64BitOperatingSystem ? "(x64)" : "(x86)")}[/]");
        AnsiConsole.MarkupLineInterpolated($"[grey]Runtime:[/] [yellow]{Environment.Version}[/]");
        AnsiConsole.MarkupLineInterpolated($"[grey]Process ID:[/] [yellow]{Environment.ProcessId}[/]");

        if (!string.IsNullOrEmpty(Environment.ProcessPath))
        {
            AnsiConsole.MarkupLineInterpolated($"[grey]Command Line:[/] [yellow]{Path.GetFileName(Environment.CommandLine.Trim('"'))}[/]");
        }

        AnsiConsole.MarkupInterpolated($"[grey]Current Directory:[/] ");

        if (toolSettings.ConsoleSettings.HidePath)
        {
            AnsiConsole.Write("(hidden)");
        }
        else
        {
            AnsiConsole.Write(new TextPath(Environment.CurrentDirectory)
                .RootColor(Spectre.Console.Color.Red)
                .StemColor(Spectre.Console.Color.Yellow)
                .LeafColor(Spectre.Console.Color.Yellow));
        }

        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLineInterpolated($"[grey]Privileged:[/] [yellow]{Environment.IsPrivilegedProcess}[/]");
    }
}
