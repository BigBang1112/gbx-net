using GBX.NET;

namespace GbxCLI;

/// <summary>
/// One file selected by <see cref="GbxBatch"/>.
/// </summary>
/// <param name="DisplayPath">Path shown in the output.</param>
/// <param name="RelativePath">Path under the selected folder or inside the archive, which <c>-o</c> keeps in the output folder.</param>
/// <param name="FilePath">The file on disk, which can be written in place, or null for an archive entry.</param>
/// <param name="OpenAsync">Parses the Gbx, or returns null if the file is not a Gbx.</param>
internal sealed record GbxBatchItem(string DisplayPath, string RelativePath, string? FilePath, Func<CancellationToken, Task<Gbx?>> OpenAsync);
