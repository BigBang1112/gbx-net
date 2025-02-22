namespace GBX.NET.PAK;

public sealed record PakKeyInfo(byte[]? PrimaryKey, byte[]? FileKey = null);
