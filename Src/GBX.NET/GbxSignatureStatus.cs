namespace GBX.NET;

/// <summary>Result of validating a GBX.NET signature.</summary>
public enum GbxSignatureStatus
{
    /// <summary>The signature status is unknown (no signature read has been attempted).</summary>
    Unknown,
    /// <summary>No final signature chunk was found.</summary>
    Missing,
    /// <summary>The signature metadata and body checksum are valid.</summary>
    Valid,
    /// <summary>The signature or Gbx framing is invalid.</summary>
    Malformed,
    /// <summary>The Gbx format or signature version is unsupported.</summary>
    Unsupported,
    /// <summary>The checksum does not match the body.</summary>
    Mismatched
}
