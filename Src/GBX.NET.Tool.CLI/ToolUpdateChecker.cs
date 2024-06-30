using Spectre.Console;

namespace GBX.NET.Tool.CLI;

internal sealed class ToolUpdateChecker
{
    private readonly Task<HttpResponseMessage> updateInfoResponseTask;

    public ToolUpdateChecker(Task<HttpResponseMessage> updateInfoResponseTask)
    {
        this.updateInfoResponseTask = updateInfoResponseTask;
    }

    public static ToolUpdateChecker Check(HttpClient client)
    {
        var responseTask = client.GetAsync("https://api.github.com/repos/GBX.NET/GBX.NET.Tool/releases/latest");
        return new ToolUpdateChecker(responseTask);
    }

    public async ValueTask<bool> TryCompareVersionAsync(CancellationToken cancellationToken)
    {
        if (!updateInfoResponseTask.IsCompleted)
        {
            return false;
        }

        AnsiConsole.WriteLine();

        var updateInfoResponse = await updateInfoResponseTask;

        if (updateInfoResponse.IsSuccessStatusCode)
        {
            AnsiConsole.Write(new Rule("Check for updates / auto-updater").LeftJustified().RuleStyle("yellow"));
            AnsiConsole.WriteLine();

            //var updateInfo = await updateInfoResponse.Content.ReadFromJsonAsync<UpdateInfo>(cancellationToken);

            //if (updateInfo is not null)
            //{
            AnsiConsole.MarkupLine($"[yellow]New version available:[/] [green]tag[/]");
            AnsiConsole.MarkupLine($"[yellow]Release notes:[/] [green]url[/]");
            //}

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

        return true;
    }
}
