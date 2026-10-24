namespace GBX.NET;

/// <summary>
/// Metadata from a verified GBX.NET validation signature.
/// </summary>
/// <param name="Version">Version of the signature format.</param>
/// <param name="CommitHash">GBX.NET source commit used to save the file.</param>
/// <param name="Tag">GBX.NET source tag used to save the file.</param>
/// <param name="Timestamp">UTC time when the file was saved.</param>
/// <param name="ByteCount">Number of uncompressed body bytes covered by the hash.</param>
/// <param name="Hash">SHA-256 digest of the covered body bytes.</param>
public sealed record GbxSignature(int Version, string CommitHash, string Tag, DateTimeOffset Timestamp, long ByteCount, Checksum256 Hash);
