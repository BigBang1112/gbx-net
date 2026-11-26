using GBX.NET;
using GBX.NET.PAK;
using System.IO.Compression;
using System.Runtime.CompilerServices;

namespace GbxCLI;

/// <summary>
/// Selects many Gbx files for <c>eval</c> and <c>set</c>: a folder, a pattern on disk such as <c>Maps/*.Map.Gbx</c>,
/// or a pattern inside a PAK or ZIP archive such as <c>Stadium.pak/Media/*</c>.
/// </summary>
internal static class GbxBatch
{
    /// <summary>
    /// Whether the path selects many files, because it is a folder or it has a wildcard. Any other path opens one file.
    /// </summary>
    public static bool IsBatch(string path) => !File.Exists(path) && (Directory.Exists(path) || PathPattern.HasWildcard(path));

    /// <summary>
    /// Whether the path continues inside an archive, so the selected files can only be written to an output folder.
    /// </summary>
    public static bool IsInArchive(string path) => FileObject.TrySplit(path, out _, out var entryPath) && entryPath is not null;

    /// <summary>
    /// Finds the files on disk that a folder or a pattern selects, sorted by path. A folder selects all files in it and its subfolders.
    /// A pattern starts from the folder before its first wildcard, and is matched as described in <see cref="PathPattern.Matches"/>,
    /// so <c>*</c> also matches subfolders.
    /// </summary>
    /// <exception cref="DirectoryNotFoundException">The folder, or the folder the pattern starts from, doesn't exist.</exception>
    public static IReadOnlyList<DiskFile> FindDiskFiles(string path)
    {
        string directory;
        string? pattern = null;

        if (Directory.Exists(path))
        {
            directory = path;
        }
        else
        {
            var normalized = PathPattern.Normalize(path);
            var wildcard = normalized.AsSpan().IndexOfAny('*', '?');
            var separator = wildcard < 0 ? -1 : normalized.LastIndexOf('/', wildcard);

            directory = separator switch
            {
                < 0 => ".",
                0 => "/",
                _ => normalized[..separator]
            };
            pattern = normalized[(separator + 1)..];

            if (wildcard < 0 || !Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Folder '{directory}' was not found.");
            }
        }

        return [.. Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories)
            .Select(file => new DiskFile(Path.GetFullPath(file), Path.GetRelativePath(directory, file)))
            .Where(file => pattern is null || PathPattern.Matches(pattern, file.RelativePath))
            .OrderBy(file => file.FullPath, StringComparer.OrdinalIgnoreCase)];
    }

    /// <summary>
    /// Selects the files of a batch path. On disk, only files whose name ends with <c>.Gbx</c> are selected, so backups and other files stay out.
    /// In an archive, every matching entry is selected, and those that aren't a Gbx open as null.
    /// The archive stays open until the enumeration ends, so each item has to be opened before the next one is requested.
    /// </summary>
    public static async IAsyncEnumerable<GbxBatchItem> EnumerateAsync(string path, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (!IsInArchive(path))
        {
            foreach (var file in FindDiskFiles(path).Where(file => file.RelativePath.EndsWith(".gbx", StringComparison.OrdinalIgnoreCase)))
            {
                yield return new GbxBatchItem(file.RelativePath, file.RelativePath, file.FullPath, token => OpenFileAsync(file.FullPath, token));
            }

            yield break;
        }

        await using var archive = await FileObject.OpenArchiveAsync(path, cancellationToken);
        var archivePath = GetArchivePath(path, archive);

        switch (archive.Value)
        {
            case Pak pak:
                foreach (var entry in pak.Select(archive.EntryPattern))
                {
                    yield return new GbxBatchItem(Path.Combine(archivePath, PathPattern.ToDisplay(entry.Path)), entry.Path, FilePath: null,
                        token => OpenPakEntryAsync(pak, entry, token));
                }

                break;
            case ZipArchive zip:
                foreach (var entry in zip.Select(archive.EntryPattern))
                {
                    yield return new GbxBatchItem(Path.Combine(archivePath, PathPattern.ToDisplay(entry.FullName)), entry.FullName, FilePath: null,
                        token => OpenZipEntryAsync(entry, token));
                }

                break;
        }
    }

    /// <summary>
    /// The path without the entry pattern, such as <c>skin.zip</c> for <c>skin.zip/*.Gbx</c>.
    /// </summary>
    private static string GetArchivePath(string path, FileObject archive)
    {
        var normalized = PathPattern.Normalize(path).TrimEnd('/');

        return archive.EntryPattern is { } pattern && normalized.EndsWith('/' + PathPattern.Normalize(pattern), StringComparison.Ordinal)
            ? PathPattern.ToDisplay(normalized[..^(pattern.Length + 1)])
            : PathPattern.ToDisplay(normalized);
    }

    private static async Task<Gbx?> OpenFileAsync(string path, CancellationToken cancellationToken)
    {
        await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await ParseGbxAsync(stream, cancellationToken);
    }

    private static async Task<Gbx?> OpenPakEntryAsync(Pak pak, PakEntry entry, CancellationToken cancellationToken)
        => !entry.IsUnstored && await pak.CheckFileIsGbxAsync(entry.File, cancellationToken)
            ? await pak.OpenGbxFileAsync(entry.File, cancellationToken: cancellationToken)
            : null;

    private static async Task<Gbx?> OpenZipEntryAsync(ZipArchiveEntry entry, CancellationToken cancellationToken)
    {
        await using var data = await FileObject.BufferAsync(entry, cancellationToken);
        return await ParseGbxAsync(data, cancellationToken);
    }

    private static async Task<Gbx?> ParseGbxAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (await FileSignature.ReadAsync(stream, cancellationToken) != FileKind.Gbx)
        {
            return null;
        }

        stream.Position = 0;
        return await Gbx.ParseAsync(stream, cancellationToken: cancellationToken);
    }
}
