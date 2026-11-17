namespace GbxCLI;

/// <summary>
/// The input, output and state that every command receives.
/// </summary>
/// <param name="IsInteractive">Whether the browser and prompts can run, which needs a console for both input and output.</param>
internal sealed record CliContext(TextReader Input, TextWriter Output, bool IsInteractive, CancellationToken CancellationToken)
{
    private CancellationTokenSource? operation;

    public CliConsole Console { get; } = CliConsole.Create(Output);

    /// <summary>
    /// Whether an error waits for Enter before the process ends, because the console window closes with it.
    /// </summary>
    public bool PauseOnError { get; set; }

    /// <summary>
    /// Runs work that Ctrl+C cancels on its own, so an interactive session continues after it.
    /// </summary>
    /// <returns>False if Ctrl+C cancelled the work.</returns>
    public async Task<bool> TryRunCancellableAsync(Func<CancellationToken, Task> work)
    {
        using var source = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken);
        Volatile.Write(ref operation, source);

        try
        {
            await work(source.Token);
            return true;
        }
        catch (OperationCanceledException) when (source.IsCancellationRequested && !CancellationToken.IsCancellationRequested)
        {
            return false;
        }
        finally
        {
            Volatile.Write(ref operation, null);
        }
    }

    /// <summary>
    /// Cancels the work running in <see cref="TryRunCancellableAsync"/>, if there is any.
    /// </summary>
    /// <returns>False if no such work is running.</returns>
    public bool TryCancelOperation()
    {
        try
        {
            if (Volatile.Read(ref operation) is not { } source)
            {
                return false;
            }

            source.Cancel();
            return true;
        }
        catch (ObjectDisposedException)
        {
            // The work finished in the meantime.
            return false;
        }
    }
}
