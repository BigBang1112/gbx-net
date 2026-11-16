using System.IO.Compression;

namespace GbxCLI;

internal static class ZipArchiveExtensions
{
    /// <summary>
    /// Lazily selects the file entries (not folders) matching <paramref name="pattern"/>, sorted by path.
    /// Without a pattern, all of them are selected. A pattern with <c>*</c> or <c>?</c> is matched as described in <see cref="PathPattern.Matches"/>.
    /// Any other pattern selects the entry with exactly that path, or else the entries whose path ends with it.
    /// The archive must stay open while the result is enumerated.
    /// </summary>
    public static IEnumerable<ZipArchiveEntry> Select(this ZipArchive archive, string? pattern)
    {
        var files = archive.Entries
            .Where(entry => !entry.FullName.EndsWith('/'))
            .OrderBy(entry => PathPattern.Normalize(entry.FullName), StringComparer.Ordinal);

        if (pattern is null)
        {
            return files;
        }

        if (PathPattern.HasWildcard(pattern))
        {
            return files.Where(entry => PathPattern.Matches(pattern, entry.FullName));
        }

        var exact = files.Where(entry => PathPattern.Equal(entry.FullName, pattern)).ToArray();

        return exact.Length > 0 ? exact : files.Where(entry => PathPattern.EndsWith(entry.FullName, pattern));
    }

    /// <summary>
    /// Finds the entry matching the longest leading part of <paramref name="path"/> before any wildcard
    /// and returns the rest of the path, which points inside of that entry.
    /// For example, <c>Model.Pack.Gbx/*.dds</c> finds <c>Model.Pack.Gbx</c> with <c>*.dds</c> left.
    /// </summary>
    /// <param name="requireInnerPath">Only consider leading parts that leave some path inside the entry.</param>
    /// <returns>The entry and the path inside of it, or null if there is none.</returns>
    /// <exception cref="InvalidDataException">A leading part matches more than one entry.</exception>
    public static (ZipArchiveEntry Entry, string? InnerPath)? Find(this ZipArchive archive, string path, bool requireInnerPath = false)
    {
        var pattern = PathPattern.Normalize(path).Trim('/');
        var wildcard = pattern.AsSpan().IndexOfAny('*', '?');
        var end = wildcard < 0 ? pattern.Length : pattern.LastIndexOf('/', wildcard);

        if (requireInnerPath && end == pattern.Length)
        {
            end = pattern.LastIndexOf('/');
        }

        for (; end > 0; end = pattern.LastIndexOf('/', end - 1))
        {
            var prefix = pattern[..end];
            var matches = archive.Select(prefix).Take(11).ToArray();

            if (matches.Length > 1)
            {
                throw Ambiguous(prefix, matches);
            }

            if (matches.Length == 1)
            {
                return (matches[0], end < pattern.Length ? pattern[(end + 1)..] : null);
            }
        }

        return null;
    }

    public static InvalidDataException Ambiguous(string pattern, IReadOnlyCollection<ZipArchiveEntry> matches)
        => new($"ZIP entry '{pattern}' is ambiguous. Specify a full path. Matches include: {string.Join(", ", matches.Take(10).Select(entry => entry.FullName))}");
}
