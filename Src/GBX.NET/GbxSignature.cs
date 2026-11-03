namespace GBX.NET;

/// <summary>
/// Build and save metadata from a verified GBX.NET signature. The checksum does not authenticate the writer.
/// </summary>
/// <param name="Version">Signature format version.</param>
/// <param name="CommitHash">GBX.NET source commit.</param>
/// <param name="Tag">GBX.NET source description from git describe, which may be a tag or an abbreviated commit hash.</param>
/// <param name="Timestamp">UTC save time.</param>
/// <param name="ByteCount">Number of uncompressed body bytes hashed, including signature metadata.</param>
/// <param name="Hash">SHA-256 digest of those bytes.</param>
public sealed record GbxSignature(int Version, string CommitHash, string Tag, DateTimeOffset Timestamp, long ByteCount, Checksum256 Hash);
