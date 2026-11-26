using GBX.NET;

namespace GbxCLI;

/// <summary>
/// Arguments and output available to scripts run by the CLI.
/// </summary>
public sealed class ScriptGlobals(string[] args, TextWriter output, CancellationToken cancellationToken = default)
{
    /// <summary>
    /// Gets the arguments passed after the script path.
    /// </summary>
    public string[] Args { get; } = args;

    /// <summary>
    /// Gets the CLI's standard output writer.
    /// </summary>
    public TextWriter Output { get; } = output;

    /// <summary>
    /// Gets the token that Ctrl+C cancels.
    /// </summary>
    public CancellationToken CancellationToken { get; } = cancellationToken;

    /// <summary>
    /// Writes a value followed by a newline to standard output.
    /// </summary>
    public void Print(object? value) => Output.WriteLine(value);

    /// <summary>
    /// Gets the full paths of the Gbx files that a folder or a pattern on disk selects, such as <c>Maps/*.Map.Gbx</c>, the way
    /// <c>gbx set</c> and <c>gbx eval</c> select them. Only files whose name ends with <c>.Gbx</c> are included. A path to one file
    /// returns that file.
    /// </summary>
    public IReadOnlyList<string> GetGbxFiles(string path)
    {
        if (File.Exists(path))
        {
            return [Path.GetFullPath(path)];
        }

        if (GbxBatch.IsInArchive(path))
        {
            throw new NotSupportedException("GetGbxFiles selects files on disk. Open an archive with Pak or ZipArchive instead.");
        }

        return [.. GbxBatch.FindDiskFiles(path)
            .Where(file => file.RelativePath.EndsWith(".gbx", StringComparison.OrdinalIgnoreCase))
            .Select(file => file.FullPath)];
    }

    /// <summary>
    /// Saves a Gbx the way <c>gbx eval --write</c> does. The saved data is parsed again and compared with <paramref name="gbx"/>
    /// before anything is written, and the file is replaced through a temporary file.
    /// </summary>
    /// <param name="backup">Keep an existing file as <c>&lt;path&gt;.bak</c>.</param>
    /// <param name="force">Write even when reading the saved data back gives different properties.</param>
    public Task SaveAsync(Gbx gbx, string path, bool backup = true, bool force = false)
        => CheckedGbxWriter.WriteAsync(gbx, path, CliConsole.Create(Output), backup, force, CancellationToken);
}
