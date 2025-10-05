namespace GbxExplorerOld.Client.Sections.SectionEditor;

public class Debouncer
{
    private CancellationTokenSource? cts;

    public async Task DebounceAsync(Func<Task> action, int delayMs = 1000)
    {
        cts?.Cancel();
        cts = new CancellationTokenSource();

        try
        {
            await Task.Delay(delayMs, cts.Token);
            await action();
        }
        catch (TaskCanceledException)
        {}
    }
}
