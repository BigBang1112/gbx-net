using NationsConverterWeb;
using Spectre.Console;
using System.Net.Http.Json;

namespace GBX.NET.Tool.CLI;

internal sealed class ToolUpdateChecker
{
    private readonly Task<HttpResponseMessage> updateInfoResponseTask;

    public ToolUpdateChecker(Task<HttpResponseMessage> updateInfoResponseTask)
    {
        this.updateInfoResponseTask = updateInfoResponseTask;
    }

    public static ToolUpdateChecker? Check(HttpClient client, string? githubRepo, CancellationToken cancellationToken)
    {
        if (githubRepo is null)
        {
            return null;
        }

        var responseTask = client.GetAsync($"https://api.github.com/repos/{githubRepo}/releases/latest", cancellationToken);
        return new ToolUpdateChecker(responseTask);
    }

    public async ValueTask<bool> TryCompareVersionAsync(CancellationToken cancellationToken)
    {
        if (!updateInfoResponseTask.IsCompleted)
        {
            return false;
        }

        await CompareVersionAsync(cancellationToken);

        return true;
    }

    public async Task CompareVersionAsync(CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();

        var updateInfoResponse = await updateInfoResponseTask;

        if (updateInfoResponse.IsSuccessStatusCode)
        {
            AnsiConsole.Write(new Rule("Check for updates / auto-updater").LeftJustified().RuleStyle("yellow"));
            AnsiConsole.WriteLine();

            try
            {
                var updateInfo = await updateInfoResponse.Content.ReadFromJsonAsync(GitHubJsonContext.Default.UpdateInfo, cancellationToken);

                if (updateInfo is not null)
                {
                    AnsiConsole.MarkupLine($"[yellow]Latest version available:[/] [green]{updateInfo.TagName?.TrimStart('v')}[/]");
                    AnsiConsole.MarkupLine($"[yellow]Release notes:[/] [green]{updateInfo.HtmlUrl}[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("[red]Failed to parse update information.[/]");
                AnsiConsole.WriteException(ex);
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule().RuleStyle("yellow"));
        }
        else
        {
            AnsiConsole.Write(new Rule("Check for updates / auto-updater").LeftJustified().RuleStyle("yellow dim"));
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLineInterpolated($"  [red]Failed to check for updates. Status code: {updateInfoResponse.StatusCode}[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule().RuleStyle("yellow dim"));
        }
    }
}
