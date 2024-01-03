namespace GBX.NET;

/// <summary>
/// Compression of the Gbx.
/// </summary>
public enum GbxCompression : byte
{
    /// <summary>
    /// Compressed.
    /// </summary>
    Compressed = (byte)'C',
    /// <summary>
    /// Uncompressed.
    /// </summary>
    Uncompressed = (byte)'U'
}