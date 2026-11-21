namespace GbxCLI;

internal static class OutputFile
{
    /// <summary>
    /// Writes to a temporary file beside <paramref name="path"/>, then moves it over <paramref name="path"/>.
    /// The temporary file is deleted if writing fails.
    /// </summary>
    /// <param name="backup">Keep an existing file as <c>&lt;path&gt;.bak</c>.</param>
    /// <returns>Path of the backup, or null if none was made.</returns>
    public static async Task<string?> WriteAsync(string path, Func<Stream, CancellationToken, Task> write, bool backup = true, CancellationToken cancellationToken = default)
    {
        var temporaryPath = $"{path}.{Guid.NewGuid():N}.tmp";

        try
        {
            await using (var destination = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await write(destination, cancellationToken);
            }

            if (!File.Exists(path))
            {
                File.Move(temporaryPath, path);
                return null;
            }

            var backupPath = backup ? path + ".bak" : null;
            File.Replace(temporaryPath, path, backupPath);
            return backupPath;
        }
        catch
        {
            TryDelete(temporaryPath);
            throw;
        }
    }

    private static void TryDelete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            // The original exception is more useful than this one.
        }
    }
}
