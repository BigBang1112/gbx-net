namespace GbxCLI;

/// <summary>
/// How <see cref="GbxBatchRunner"/> writes the changed files.
/// </summary>
/// <param name="OutputDirectory">The folder to write into, keeping the relative paths, or null to replace the files.</param>
/// <param name="DryRun">Check the changed files, but write nothing.</param>
/// <param name="Backup">Keep an existing file as <c>&lt;path&gt;.bak</c>.</param>
/// <param name="Force">Write even when reading the saved data back gives different properties.</param>
/// <param name="CompareWithOriginal">
/// Find out whether a file changed by comparing its properties with a second, unchanged copy, for edits that don't know it,
/// such as C# expressions. Saved data can't tell, because GBX.NET writes the time of saving into every body.
/// </param>
internal sealed record GbxBatchWrite(string? OutputDirectory, bool DryRun, bool Backup, bool Force, bool CompareWithOriginal = false);
