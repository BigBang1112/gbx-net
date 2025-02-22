namespace GBX.NET.PAK;

/// <summary>
/// Key information for decrypting a Pak file.
/// </summary>
/// <param name="PrimaryKey">Key for main decryption, or only the header part for newer Pak format.</param>
/// <param name="FileKey">Alternative key to use for decrypting individual files. Ignored for TMF Pak (v3) format and older.</param>
public sealed record PakKeyInfo(byte[]? PrimaryKey, byte[]? FileKey = null);
