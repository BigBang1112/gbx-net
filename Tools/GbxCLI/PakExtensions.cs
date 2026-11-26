using GBX.NET.PAK;
using MD5 = GBX.NET.Crypto.MD5;

namespace GbxCLI;

internal static class PakExtensions
{
    /// <summary>
    /// Selects the entries matching <paramref name="pattern"/>, sorted by resolved path. Without a pattern, all of them are selected.
    /// A pattern with <c>*</c> or <c>?</c> is matched against the stored and resolved path as described in <see cref="PathPattern.Matches"/>.
    /// Any other pattern selects, in this order of preference: the entry with exactly that stored path, the entry with exactly
    /// that resolved path, or the entries whose stored or resolved path ends with it, or whose hashed name is the hash of it.
    /// </summary>
    public static IEnumerable<PakEntry> Select(this Pak pak, string? pattern)
    {
        var entries = pak.Files
            .Select(pair => new PakEntry(pair.Key, PakFileHashes.ResolvePath(pair.Key), pair.Value))
            .OrderBy(entry => entry.Path, StringComparer.Ordinal)
            .ThenBy(entry => entry.StoredPath, StringComparer.Ordinal)
            .ToArray();

        if (pattern is null)
        {
            return entries;
        }

        if (PathPattern.HasWildcard(pattern))
        {
            return entries.Where(entry => PathPattern.Matches(pattern, entry.StoredPath) || PathPattern.Matches(pattern, entry.Path));
        }

        // A stored path always identifies the original entry, even if a resolved name collides with it.
        var exact = entries.Where(entry => PathPattern.Equal(entry.StoredPath, pattern)).ToArray();

        if (exact.Length == 0)
        {
            exact = entries.Where(entry => PathPattern.Equal(entry.Path, pattern)).ToArray();
        }

        if (exact.Length > 0)
        {
            return exact;
        }

        var hashes = GetHashedSuffixes(PathPattern.Normalize(pattern));

        return entries.Where(entry => PathPattern.EndsWith(entry.StoredPath, pattern)
            || PathPattern.EndsWith(entry.Path, pattern)
            || PathPattern.Suffixes(entry.StoredPath).Any(hashes.Contains));
    }

    /// <summary>
    /// Finds the single entry matching <paramref name="pattern"/>.
    /// </summary>
    /// <returns>The entry, or null if there is none.</returns>
    /// <exception cref="InvalidDataException">The pattern matches more than one entry.</exception>
    public static PakEntry? Find(this Pak pak, string pattern)
    {
        var matches = pak.Select(pattern).Take(11).ToArray();

        if (matches.Length > 1)
        {
            throw new InvalidDataException($"PAK entry '{pattern}' is ambiguous. Specify a full path or stored hash. Matches include: {string.Join(", ", matches.Take(10).Select(entry => entry.StoredPath))}");
        }

        return matches.FirstOrDefault();
    }

    /// <summary>
    /// Throws if the file index could not be read, because the private header needs a key.
    /// </summary>
    public static void EnsureIndex(this Pak pak, bool hasKey)
    {
        if (pak.IsHeaderPrivate && !hasKey)
        {
            throw new InvalidDataException("The PAK file index is unavailable because the private header requires a matching key.");
        }
    }

    /// <summary>
    /// A name outside the hash catalogue is matched by its PAK filename hash. The hashed part can be
    /// a filename or a path relative to a stored folder, so each of those is hashed.
    /// </summary>
    private static HashSet<string> GetHashedSuffixes(string path)
    {
        var hashes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

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

        return hashes;
    }
}
