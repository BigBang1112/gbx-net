using System.IO.Enumeration;
using System.IO.Compression;

namespace GbxCLI;

internal static class ZipArchiveExtensions
{
    /// <summary>
    /// Lazily selects the file entries (not directories) matching <paramref name="pattern"/>, sorted by path.
    /// Without a pattern, all of them are selected. A pattern with * or ? is matched against the path
    /// and every part of it starting after a separator. Any other pattern selects the entry with exactly that path,
    /// or else the entries whose path ends with it. Both / and \ can be used as separators.
    /// The archive must stay open while the result is enumerated.
    /// </summary>
    public static IEnumerable<ZipArchiveEntry> Select(this ZipArchive archive, string? pattern)
    {
        var files = archive.Entries
            .Where(entry => !entry.FullName.EndsWith('/'))
            .OrderBy(entry => entry.FullName.Replace('/', Path.DirectorySeparatorChar), StringComparer.Ordinal);

        if (pattern is null)
        {
            return files;
        }

        pattern = pattern.Replace('\\', '/');

        if (pattern.IndexOfAny(['*', '?']) >= 0)
        {
            return files.Where(entry => PathSuffixes(entry.FullName)
                .Any(suffix => FileSystemName.MatchesSimpleExpression(pattern, suffix, ignoreCase: true)));
        }

        return SelectPath(files, pattern);
    }

    private static IEnumerable<ZipArchiveEntry> SelectPath(IEnumerable<ZipArchiveEntry> files, string path)
    {
        var found = false;

        foreach (var entry in files.Where(entry => Equal(entry.FullName, path)))
        {
            found = true;
            yield return entry;
        }

        if (found)
        {
            yield break;
        }

        foreach (var entry in files.Where(entry => entry.FullName.Replace('\\', '/').EndsWith('/' + path, StringComparison.OrdinalIgnoreCase)))
        {
            yield return entry;
        }
    }

    private static bool Equal(string path, string pattern) => string.Equals(path.Replace('\\', '/'), pattern, StringComparison.OrdinalIgnoreCase);

    private static IEnumerable<string> PathSuffixes(string path)
    {
        path = path.Replace('\\', '/');
        yield return path;

        for (var separator = path.IndexOf('/'); separator >= 0; separator = path.IndexOf('/', separator + 1))
        {
            yield return path[(separator + 1)..];
        }
    }
}
