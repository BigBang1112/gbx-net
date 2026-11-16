using System.Collections.Frozen;
using System.IO.Compression;

namespace GbxCLI;

/// <summary>
/// Resolves hashed PAK filenames with the catalogue embedded from <c>Resources/FileHashes.txt</c>.
/// </summary>
internal static class PakFileHashes
{
    private static readonly Lazy<FrozenDictionary<string, string>> names = new(Load);

    /// <summary>
    /// Replaces a hashed filename with its known name. Other paths are returned as they are.
    /// </summary>
    public static string ResolvePath(string path)
    {
        var normalized = PathPattern.Normalize(path);
        var separator = normalized.LastIndexOf('/');
        var name = normalized[(separator + 1)..];

        if (!IsHashName(name) || !names.Value.TryGetValue(name, out var resolved))
        {
            return path;
        }

        return normalized[..(separator + 1)] + resolved;
    }

    /// <summary>
    /// Whether the filename of <paramref name="path"/> is a 136-bit hash.
    /// </summary>
    public static bool IsHash(string path)
    {
        var normalized = PathPattern.Normalize(path);
        return IsHashName(normalized[(normalized.LastIndexOf('/') + 1)..]);
    }

    private static bool IsHashName(string name) => name.Length == 34 && name.All(char.IsAsciiHexDigit);

    private static FrozenDictionary<string, string> Load()
    {
        using var stream = typeof(PakFileHashes).Assembly.GetManifestResourceStream("GbxCLI.FileHashes.txt.gz")
            ?? throw new InvalidOperationException("The embedded PAK file hash catalogue is missing.");
        using var decompressed = new GZipStream(stream, CompressionMode.Decompress);
        using var reader = new StreamReader(decompressed);
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Each line is a hash, a space and the name.
        while (reader.ReadLine() is { } line)
        {
            var separator = line.IndexOf(' ');

            if (separator < 0)
            {
                continue;
            }

            var name = line[(separator + 1)..];

            if (!string.IsNullOrWhiteSpace(name))
            {
                result[line[..separator]] = PathPattern.Normalize(name);
            }
        }

        return result.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
    }
}
