using GBX.NET.PAK;

namespace GbxCLI;

/// <param name="StoredPath">Path as stored in the PAK, where the filename can be a hash.</param>
/// <param name="Path">Path with the filename hash resolved from the catalogue, if it is known.</param>
internal sealed record PakEntry(string StoredPath, string Path, PakFile File)
{
    public bool IsResolved => !PakFileHashes.IsHash(Path);

    /// <summary>
    /// Newer title packs list entries without stored data.
    /// </summary>
    public bool IsUnstored => File.Offset == uint.MaxValue && File.UncompressedSize == 0;
}
