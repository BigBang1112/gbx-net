namespace GBX.NET;

/// <summary>
/// Result of reading and verifying a GBX.NET validation signature.
/// </summary>
/// <param name="Status">Whether the signature is valid, missing, malformed, unsupported, or mismatched.</param>
/// <param name="Signature">The verified metadata when the status is <see cref="GbxSignatureStatus.Valid"/>, otherwise <see langword="null"/>.</param>
public sealed record GbxSignatureReadResult(GbxSignatureStatus Status, GbxSignature? Signature);
