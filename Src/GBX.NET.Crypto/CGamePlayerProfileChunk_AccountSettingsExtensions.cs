using GBX.NET.Engines.Game;
using System.Text;

namespace GBX.NET.Crypto;

public static class CGamePlayerProfileChunk_AccountSettingsExtensions
{
    private static readonly byte[] passwordEncryptionKey =
        [0xD0, 0x6A, 0xFD, 0x5E, 0x23, 0x1A, 0x4B, 0x74, 0x05, 0x04, 0x0D, 0x9C, 0x82, 0xF7, 0x34, 0x2E];

    /// <summary>
    /// Decrypts <see cref="CGamePlayerProfileChunk_AccountSettings.OnlinePassword"/> using the account-password encryption key.
    /// </summary>
    /// <param name="settings">The account settings containing the encrypted hexadecimal password.</param>
    /// <returns>The decrypted password, or an empty string if no password is stored.</returns>
    public static string DecryptOnlinePassword(this CGamePlayerProfileChunk_AccountSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        return CGamePlayerProfileExtensions.DecryptKey(settings.OnlinePassword, passwordEncryptionKey);
    }

    /// <summary>
    /// Decrypts <see cref="CGamePlayerProfileChunk_AccountSettings.OnlineSupportKey"/> using the support-key encryption key.
    /// </summary>
    /// <param name="settings">The account settings containing the encrypted hexadecimal support key.</param>
    /// <returns>The decrypted support key, or an empty string if no key is stored.</returns>
    public static string DecryptOnlineSupportKey(this CGamePlayerProfileChunk_AccountSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        return CGamePlayerProfileExtensions.DecryptKey(settings.OnlineSupportKey);
    }

    /// <summary>
    /// Decrypts <see cref="CGamePlayerProfileChunk_AccountSettings.OnlineValidationCode"/> using the support-key encryption key.
    /// </summary>
    /// <param name="settings">The account settings containing the encrypted hexadecimal validation code.</param>
    /// <returns>The decrypted validation code, or an empty string if no code is stored.</returns>
    public static string DecryptOnlineValidationCode(this CGamePlayerProfileChunk_AccountSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        return CGamePlayerProfileExtensions.DecryptKey(settings.OnlineValidationCode);
    }

    /// <summary>
    /// Decrypts <see cref="CGamePlayerProfileChunk_AccountSettings.EncryptedKeyHexa"/> using the MD5 digest of <see cref="CGamePlayerProfileChunk_AccountSettings.OnlineLogin"/>.
    /// </summary>
    /// <param name="settings">The account settings containing the encrypted hexadecimal key and its associated login.</param>
    /// <returns>The decrypted key, or an empty string if no key is stored.</returns>
    public static string DecryptKey(this CGamePlayerProfileChunk_AccountSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        var key = MD5.Compute(Encoding.UTF8.GetBytes(settings.OnlineLogin ?? string.Empty));
        return CGamePlayerProfileExtensions.DecryptKey(settings.EncryptedKeyHexa, key);
    }
}
