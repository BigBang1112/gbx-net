using System.IO.Compression;

namespace GbxCLI;

internal sealed class ZipFileIndex : IDisposable
{
    private static readonly byte[][] signatures = ["PK\x03\x04"u8.ToArray(), "PK\x05\x06"u8.ToArray(), "PK\x07\x08"u8.ToArray()];

    private readonly ZipArchive archive;
    private readonly Entry[] entries;

    public ZipFileIndex(string path)
    {
        archive = ZipFile.OpenRead(path);

        // Directory entries carry no data, so only files are indexed.
        entries = archive.Entries
            .Where(entry => !entry.FullName.EndsWith('/'))
            .Select(entry => new Entry(entry.FullName.Replace('/', Path.DirectorySeparatorChar), entry))
            .OrderBy(entry => entry.Path, StringComparer.Ordinal)
            .ToArray();
    }

    public static async Task<bool> IsZipAsync(string path)
    {
        await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        var signature = new byte[4];
        var bytesRead = await stream.ReadAtLeastAsync(signature, signature.Length, throwOnEndOfStream: false);
        return bytesRead == signature.Length && signatures.Any(x => x.AsSpan().SequenceEqual(signature));
    }

    public Entry[] Select(string? pattern)
    {
        if (pattern is null)
        {
            return entries;
        }

        pattern = PakFileIndex.Normalize(pattern);

        if (pattern.IndexOfAny(['*', '?']) >= 0)
        {
            return entries.Where(entry => PakFileIndex.Matches(pattern, entry.Path)).ToArray();
        }

        var matches = entries.Where(entry => PakFileIndex.Equal(entry.Path, pattern)).ToArray();
        return matches.Length > 0 ? matches : entries.Where(entry => PakFileIndex.HasSuffix(entry.Path, pattern)).ToArray();
    }

    public void Dispose() => archive.Dispose();

    internal sealed record Entry(string Path, ZipArchiveEntry File);
}
