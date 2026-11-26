namespace GbxCLI;

internal static class OutputFile
{
    private static readonly char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

    /// <summary>
    /// The full path of an output folder with a separator at the end, as <see cref="GetDestination"/> takes it.
    /// </summary>
    public static string GetRoot(string directoryPath) => Path.TrimEndingDirectorySeparator(Path.GetFullPath(directoryPath)) + Path.DirectorySeparatorChar;

    /// <summary>
    /// Joins a path from an archive or a folder to <paramref name="root"/>. Rejects rooted paths, <c>..</c> parts and invalid
    /// filename characters, and checks that the result stays under <paramref name="root"/>.
    /// </summary>
    /// <param name="root">An output folder from <see cref="GetRoot"/>.</param>
    /// <exception cref="InvalidDataException">The path is not a safe relative file path.</exception>
    public static string GetDestination(string root, string relativePath)
    {
        var parts = PathPattern.Normalize(relativePath).Split('/');

        if (Path.IsPathRooted(relativePath) || parts.Any(part => part is "" or "." or ".." || part.IndexOfAny(invalidFileNameChars) >= 0))
        {
            throw Unsafe(relativePath);
        }

        var destination = Path.GetFullPath(Path.Join(root, Path.Join(parts)));

        return destination.StartsWith(root, StringComparison.OrdinalIgnoreCase) ? destination : throw Unsafe(relativePath);
    }

    private static InvalidDataException Unsafe(string relativePath) => new($"Entry '{relativePath}' is not a safe relative file path.");

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
