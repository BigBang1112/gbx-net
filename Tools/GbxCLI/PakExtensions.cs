using GBX.NET.PAK;
using System.IO.Enumeration;
using MD5 = GBX.NET.Crypto.MD5;

namespace GbxCLI;

internal static class PakExtensions
{
    /// <summary>
    /// Lazily selects the entries matching <paramref name="pattern"/>, sorted by resolved path.
    /// Without a pattern, all of them are selected. A pattern with * or ? is matched against the stored and resolved path
    /// and every part of them starting after a separator. Any other pattern selects, in this order of preference:
    /// the entry with exactly that stored path, the entry with exactly that resolved path, or the entries whose
    /// stored or resolved path ends with it, or whose hashed name is the hash of it. Both / and \ can be used as separators.
    /// </summary>
    public static IEnumerable<PakEntry> Select(this Pak pak, string? pattern)
    {
        var entries = pak.Files.Select(pair => new PakEntry(pair.Key, PakFileHashes.ResolvePath(pair.Key), pair.Value))
            .OrderBy(entry => entry.Path, StringComparer.Ordinal)
            .ThenBy(entry => entry.StoredPath, StringComparer.Ordinal);

        if (pattern is null)
        {
            return entries;
        }

        pattern = Normalize(pattern);

        if (pattern.IndexOfAny(['*', '?']) >= 0)
        {
            return entries.Where(entry => Matches(pattern, entry.StoredPath) || Matches(pattern, entry.Path));
        }

        return SelectPath(entries, pattern);
    }

    /// <summary>
    /// Finds the single file matching <paramref name="pattern"/>, which is a path rather than a wildcard pattern.
    /// </summary>
    /// <returns>The file, or null if there is none.</returns>
    /// <exception cref="InvalidDataException">The pattern matches more than one file.</exception>
    public static PakFile? Find(this Pak pak, string pattern)
    {
        var matches = pak.Select(pattern).ToArray();

        if (matches.Length > 1)
        {
            throw new InvalidDataException($"PAK entry '{pattern}' is ambiguous ({matches.Length} matches). Specify a full path or stored hash. First matches: {string.Join(", ", matches.Take(10).Select(entry => entry.StoredPath))}");
        }

        return matches.FirstOrDefault()?.File;
    }

    private static IEnumerable<PakEntry> SelectPath(IEnumerable<PakEntry> source, string path)
    {
        var entries = source.ToArray();

        // The first stage with any match wins.
        Func<IEnumerable<PakEntry>>[] stages =
        [
            // A stored path always identifies the original entry, even if a resolved alias collides with it.
            () => entries.Where(entry => Equal(entry.StoredPath, path)),
            () => entries.Where(entry => Equal(entry.Path, path)),
            () =>
            {
                var hashes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // The hashed portion can be a filename or a path relative to a stored folder.
                for (var start = 0; start < path.Length;)
                {
                    hashes.Add(path[..start] + MD5.Compute136(path[start..].Replace('/', '\\')));
                    var separator = path.IndexOf('/', start);

                    if (separator < 0)
                    {
                        break;
                    }

                    start = separator + 1;
                }

                return entries.Where(entry => HasSuffix(entry.StoredPath, path) || HasSuffix(entry.Path, path)
                    || PathSuffixes(entry.StoredPath).Any(hashes.Contains));
            }
        ];

        foreach (var stage in stages)
        {
            var found = false;

            foreach (var entry in stage())
            {
                found = true;
                yield return entry;
            }

            if (found)
            {
                yield break;
            }
        }
    }

    private static string Normalize(string path) => path.Replace('\\', '/');

    private static IEnumerable<string> PathSuffixes(string path)
    {
        var normalized = Normalize(path);
        yield return normalized;

        for (var separator = normalized.IndexOf('/'); separator >= 0; separator = normalized.IndexOf('/', separator + 1))
        {
            yield return normalized[(separator + 1)..];
        }
    }

    private static bool Equal(string left, string right) => string.Equals(Normalize(left), right, StringComparison.OrdinalIgnoreCase);

    private static bool HasSuffix(string path, string suffix) => Equal(path, suffix)
        || Normalize(path).EndsWith('/' + suffix, StringComparison.OrdinalIgnoreCase);

    private static bool Matches(string pattern, string path) => PathSuffixes(path)
        .Any(suffix => FileSystemName.MatchesSimpleExpression(pattern, suffix, ignoreCase: true));
}

internal sealed record PakEntry(string StoredPath, string Path, PakFile File)
{
    public bool IsResolved => !PakFileHashes.IsHash(Path);
}
