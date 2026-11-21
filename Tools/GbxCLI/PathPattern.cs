using System.IO.Enumeration;

namespace GbxCLI;

/// <summary>
/// Entry matching rules shared by PAK and ZIP archives. Both slash styles work, and matching ignores case.
/// </summary>
internal static class PathPattern
{
    public static string Normalize(string path) => path.Replace('\\', '/');

    public static bool HasWildcard(string pattern) => pattern.AsSpan().IndexOfAny('*', '?') >= 0;

    public static bool Equal(string path, string other)
        => string.Equals(Normalize(path), Normalize(other), StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Whether the path is <paramref name="suffix"/>, or ends with it after a separator.
    /// </summary>
    public static bool EndsWith(string path, string suffix)
    {
        path = Normalize(path);
        suffix = Normalize(suffix);

        return string.Equals(path, suffix, StringComparison.OrdinalIgnoreCase)
            || path.EndsWith('/' + suffix, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Matches a wildcard pattern against the whole path and every part of it that starts after a separator.
    /// <c>*</c> also matches separators, so <c>Media/*</c> selects a whole folder.
    /// </summary>
    public static bool Matches(string pattern, string path)
    {
        pattern = Normalize(pattern);
        return Suffixes(path).Any(suffix => FileSystemName.MatchesSimpleExpression(pattern, suffix, ignoreCase: true));
    }

    /// <summary>
    /// The whole path, then every part of it that starts after a separator.
    /// </summary>
    public static IEnumerable<string> Suffixes(string path)
    {
        path = Normalize(path);
        yield return path;

        for (var separator = path.IndexOf('/'); separator >= 0; separator = path.IndexOf('/', separator + 1))
        {
            yield return path[(separator + 1)..];
        }
    }

    /// <summary>
    /// Converts the separators to the ones of the current platform, for paths shown in the output.
    /// </summary>
    public static string ToDisplay(string path) => path.Replace('\\', '/').Replace('/', Path.DirectorySeparatorChar);
}
