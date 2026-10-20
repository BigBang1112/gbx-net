using System.Collections.Frozen;
using System.IO.Compression;

namespace GbxCLI;

internal static class PakFileHashes
{
    private static readonly Lazy<FrozenDictionary<string, string>> names = new(Load);

    public static string ResolvePath(string path)
    {
        var normalized = path.Replace('\\', '/');
        var separator = normalized.LastIndexOf('/');
        var name = normalized[(separator + 1)..];

        if (!IsHash(name) || !names.Value.TryGetValue(name, out var resolved))
        {
            return path;
        }

        return (normalized[..(separator + 1)] + resolved).Replace('/', Path.DirectorySeparatorChar);
    }

    public static bool IsHash(string path)
    {
        var normalized = path.Replace('\\', '/');
        var name = normalized[(normalized.LastIndexOf('/') + 1)..];
        return name.Length == 34 && name.All(char.IsAsciiHexDigit);
    }

    private static FrozenDictionary<string, string> Load()
    {
        using var stream = typeof(PakFileHashes).Assembly.GetManifestResourceStream("GbxCLI.FileHashes.txt.gz")
            ?? throw new InvalidOperationException("The embedded PAK file hash catalogue is missing.");
        using var decompressed = new GZipStream(stream, CompressionMode.Decompress);
        using var reader = new StreamReader(decompressed);
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

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
                result[line[..separator]] = name.Replace('\\', '/');
            }
        }

        return result.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
    }
}
