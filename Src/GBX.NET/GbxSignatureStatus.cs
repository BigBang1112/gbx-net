namespace GBX.NET;

/// <summary>
/// Result of reading and verifying a GBX.NET validation signature.
/// </summary>
public enum GbxSignatureStatus
{
    /// <summary>No validation chunk was recognized.</summary>
    Missing,
    /// <summary>The signature was read and its SHA-256 hash matches.</summary>
    Valid,
    /// <summary>The Gbx data or recognized signature is malformed, or the signature exceeds metadata read limits.</summary>
    Malformed,
    /// <summary>The Gbx format or signature version is unsupported.</summary>
    Unsupported,
    /// <summary>The signature is well formed, but its SHA-256 hash does not match the body.</summary>
    Mismatched
}
