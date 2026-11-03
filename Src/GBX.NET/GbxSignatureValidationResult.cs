using System.Diagnostics.CodeAnalysis;

namespace GBX.NET;

/// <summary>Signature validation status and verified metadata.</summary>
public sealed record GbxSignatureValidationResult
{
    public GbxSignatureStatus Status { get; }
    public GbxSignature? Signature { get; }

    /// <summary>Whether <see cref="Signature"/> contains verified metadata.</summary>
    [MemberNotNullWhen(true, nameof(Signature))]
    public bool IsValid => Signature is not null;

    public GbxSignatureValidationResult(GbxSignatureStatus status)
    {
        if (status == GbxSignatureStatus.Valid)
        {
            throw new ArgumentException("A valid result requires signature metadata.", nameof(status));
        }
        Status = status;
    }

    public GbxSignatureValidationResult(GbxSignature signature)
    {
        Signature = signature ?? throw new ArgumentNullException(nameof(signature));
        Status = GbxSignatureStatus.Valid;
    }
}
