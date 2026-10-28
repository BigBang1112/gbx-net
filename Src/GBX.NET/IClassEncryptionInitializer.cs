namespace GBX.NET;

/// <summary>
/// Initializes node decryption with class information that may differ between archive generations.
/// </summary>
public interface IClassEncryptionInitializer : IEncryptionInitializer
{
    void InitializeClass(uint classId, uint parentClassId);
}
