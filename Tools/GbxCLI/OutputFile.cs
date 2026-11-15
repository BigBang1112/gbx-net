namespace GbxCLI;

internal static class OutputFile
{
    public static async Task<string?> WriteAsync(string path, Func<Stream, CancellationToken, Task> write, bool backup = true, CancellationToken cancellationToken = default)
    {
        var outputPath = $"{path}.{Guid.NewGuid():N}.tmp";

        await using (var destination = new FileStream(outputPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
        {
            await write(destination, cancellationToken);
        }

        var backupPath = backup ? path + ".bak" : null;

        if (File.Exists(path))
        {
            File.Replace(outputPath, path, backupPath);
        }
        else
        {
            File.Move(outputPath, path);
        }

        return backupPath;
    }
}
