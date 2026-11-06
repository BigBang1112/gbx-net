using GBX.NET.Engines.Game;

namespace GBX.NET.Crypto;

public static class CGamePlayerProfileExtensions
{
    private static readonly byte[] supportKeyEncryptionKey =
        [0xC9, 0x15, 0xB8, 0xA8, 0xEA, 0xD2, 0x35, 0x12, 0xA0, 0x15, 0x34, 0xB9, 0xF5, 0x5D, 0x63, 0x2A];

    /// <summary>
    /// Decrypts <see cref="CGamePlayerProfile.OnlineSupportKeyHexa"/> using the support-key encryption key.
    /// </summary>
    /// <param name="profile">The player profile containing the encrypted hexadecimal support key.</param>
    /// <returns>The decrypted support key, or an empty string if no key is stored.</returns>
    public static string DecryptOnlineSupportKey(this CGamePlayerProfile profile)
    {
        if (profile is null)
        {
            throw new ArgumentNullException(nameof(profile));
        }

        return DecryptKey(profile.OnlineSupportKeyHexa);
    }

    /// <summary>
    /// Decrypts <see cref="CGamePlayerProfile.OnlineValidationKeyHexa"/> using the support-key encryption key.
    /// </summary>
    /// <param name="profile">The player profile containing the encrypted hexadecimal validation key.</param>
    /// <returns>The decrypted validation key, or an empty string if no key is stored.</returns>
    public static string DecryptOnlineValidationKey(this CGamePlayerProfile profile)
    {
        if (profile is null)
        {
            throw new ArgumentNullException(nameof(profile));
        }

        return DecryptKey(profile.OnlineValidationKeyHexa);
    }

    internal static string DecryptKey(string? hex, byte[]? key = null)
    {
        if (hex is null || hex.Length == 0)
        {
            return string.Empty;
        }

        return Blowfish.DecryptHexa(hex, key ?? supportKeyEncryptionKey);
    }
}
