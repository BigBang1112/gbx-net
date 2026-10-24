using GBX.NET.PAK;
using System.IO.Enumeration;
using MD5 = GBX.NET.Crypto.MD5;

namespace GbxCLI;

internal sealed class PakFileIndex
{
    private readonly Entry[] entries;

    public PakFileIndex(Pak pak)
    {
        entries = pak.Files.Select(pair => new Entry(pair.Key, PakFileHashes.ResolvePath(pair.Key), pair.Value))
            .OrderBy(entry => entry.Path, StringComparer.Ordinal)
            .ThenBy(entry => entry.StoredPath, StringComparer.Ordinal)
            .ToArray();
    }

    public Entry[] Select(string? pattern)
    {
        if (pattern is null)
        {
            return entries;
        }

        pattern = Normalize(pattern);

        if (pattern.IndexOfAny(['*', '?']) >= 0)
        {
            return entries.Where(entry => Matches(pattern, entry.StoredPath) || Matches(pattern, entry.Path)).ToArray();
        }

        // A stored path always identifies the original entry, even if a resolved alias collides with it.
        var matches = entries.Where(entry => Equal(entry.StoredPath, pattern)).ToArray();

        if (matches.Length > 0)
        {
            return matches;
        }

        matches = entries.Where(entry => Equal(entry.Path, pattern)).ToArray();

        if (matches.Length > 0)
        {
            return matches;
        }

        var hashes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // The hashed portion can be a filename or a path relative to a stored folder.
        for (var start = 0; start < pattern.Length;)
        {
            hashes.Add(pattern[..start] + MD5.Compute136(pattern[start..].Replace('/', '\\')));
            var separator = pattern.IndexOf('/', start);

            if (separator < 0)
            {
                break;
            }

            start = separator + 1;
        }

        return entries.Where(entry => HasSuffix(entry.StoredPath, pattern) || HasSuffix(entry.Path, pattern)
            || PathSuffixes(entry.StoredPath).Any(hashes.Contains)).ToArray();
    }

    public PakFile? Find(string pattern)
    {
        var matches = Select(pattern);

        if (matches.Length > 1)
        {
            throw new InvalidDataException($"PAK entry '{pattern}' is ambiguous ({matches.Length} matches). Specify a full path or stored hash. First matches: {string.Join(", ", matches.Take(10).Select(entry => entry.StoredPath))}");
        }

        return matches.FirstOrDefault()?.File;
    }

    internal static string Normalize(string path) => path.Replace('\\', '/');

    private static IEnumerable<string> PathSuffixes(string path)
    {
        var normalized = Normalize(path);
        yield return normalized;

        for (var separator = normalized.IndexOf('/'); separator >= 0; separator = normalized.IndexOf('/', separator + 1))
        {
            yield return normalized[(separator + 1)..];
        }
    }

    internal static bool Equal(string left, string right) => string.Equals(Normalize(left), right, StringComparison.OrdinalIgnoreCase);

    internal static bool HasSuffix(string path, string suffix) => Equal(path, suffix)
        || Normalize(path).EndsWith('/' + suffix, StringComparison.OrdinalIgnoreCase);

    internal static bool Matches(string pattern, string path) => PathSuffixes(path)
        .Any(suffix => FileSystemName.MatchesSimpleExpression(pattern, suffix, ignoreCase: true));

    internal sealed record Entry(string StoredPath, string Path, PakFile File)
    {
        public bool IsResolved => !PakFileHashes.IsHash(Path);
    }
}
