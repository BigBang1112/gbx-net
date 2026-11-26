namespace GbxCLI;

/// <summary>
/// A file selected by a folder or pattern on disk.
/// </summary>
/// <param name="RelativePath">Path under the folder the pattern starts from.</param>
internal sealed record DiskFile(string FullPath, string RelativePath);
