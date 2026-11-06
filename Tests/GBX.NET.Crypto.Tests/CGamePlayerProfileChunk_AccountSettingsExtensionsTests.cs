using GBX.NET.Engines.Game;

namespace GBX.NET.Crypto.Tests;

public class CGamePlayerProfileChunk_AccountSettingsExtensionsTests
{
    [Test]
    [Arguments("00010203040506079DD2E057932C92C4", "password")]
    [Arguments("00010203040506079dd2e057932c92c473e804e0c3b7254f", "password-123")]
    [Arguments("0001020304050607B3EC0CCF7501F568E4A947EA1FC486BD", "p\u00E4ss\U0001F511")]
    public async Task DecryptOnlinePassword_NativeFormatVector_ReturnsPlaintext(string hex, string expected)
    {
        // Independent OpenSSL BF_cbc_encrypt vectors using the native key, IV 0001020304050607, and zero padding.
        var settings = new CGamePlayerProfileChunk_AccountSettings { OnlinePassword = hex };

        await Assert.That(settings.DecryptOnlinePassword()).IsEqualTo(expected);
        await Assert.That(settings.OnlinePassword).IsEqualTo(hex);
    }

    [Test]
    public async Task DecryptOnlineSupportKey_NativeFormatVector_ReturnsPlaintext()
    {
        const string hex = "000102030405060727F9FC270D7E739E";
        var settings = new CGamePlayerProfileChunk_AccountSettings { OnlineSupportKey = hex };

        await Assert.That(settings.DecryptOnlineSupportKey()).IsEqualTo("12345678");
        await Assert.That(settings.OnlineSupportKey).IsEqualTo(hex);
    }

    [Test]
    public async Task DecryptOnlineValidationCode_NativeFormatVector_ReturnsPlaintext()
    {
        const string hex = "00010203040506076CBB6D73FA3787C98990CCC79394A3CD";
        var settings = new CGamePlayerProfileChunk_AccountSettings { OnlineValidationCode = hex };

        await Assert.That(settings.DecryptOnlineValidationCode()).IsEqualTo("ABC-DEFGH-IJK");
        await Assert.That(settings.OnlineValidationCode).IsEqualTo(hex);
    }

    [Test]
    [Arguments("test.player", "0001020304050607DB004DE4B223D219BDE8983151234019")]
    [Arguments("another.player", "0001020304050607E21AEF6EC0C4700C75E9E2D0A6C0777B")]
    [Arguments("Test.Player", "0001020304050607FA0113FECB76BB188BE43719E3966871")]
    [Arguments("", "0001020304050607823E401A6CC250EB0E6BEB491A4698D2")]
    [Arguments((string?)null, "0001020304050607823E401A6CC250EB0E6BEB491A4698D2")]
    public async Task DecryptKey_NativeFormatVector_UsesRawLoginDigest(string? login, string hex)
    {
        // Independent hashlib MD5 and OpenSSL Blowfish-CBC vectors using IV 0001020304050607.
        var settings = new CGamePlayerProfileChunk_AccountSettings
        {
            OnlineLogin = login,
            EncryptedKeyHexa = hex
        };

        await Assert.That(settings.DecryptKey()).IsEqualTo("account-key-data");
        await Assert.That(settings.OnlineLogin).IsEqualTo(login);
        await Assert.That(settings.EncryptedKeyHexa).IsEqualTo(hex);
    }

    [Test]
    [Arguments((string?)null)]
    [Arguments("")]
    public async Task Decrypt_NoStoredValues_ReturnsEmptyStrings(string? hex)
    {
        var settings = new CGamePlayerProfileChunk_AccountSettings
        {
            OnlinePassword = hex,
            OnlineSupportKey = hex,
            OnlineValidationCode = hex,
            EncryptedKeyHexa = hex
        };

        await Assert.That(settings.DecryptOnlinePassword()).IsEqualTo(string.Empty);
        await Assert.That(settings.DecryptOnlineSupportKey()).IsEqualTo(string.Empty);
        await Assert.That(settings.DecryptOnlineValidationCode()).IsEqualTo(string.Empty);
        await Assert.That(settings.DecryptKey()).IsEqualTo(string.Empty);
    }

    [Test]
    public void Decrypt_NullSettings_ThrowsArgumentNullException()
    {
        CGamePlayerProfileChunk_AccountSettings settings = null!;

        Assert.Throws<ArgumentNullException>(() => settings.DecryptOnlinePassword());
        Assert.Throws<ArgumentNullException>(() => settings.DecryptOnlineSupportKey());
        Assert.Throws<ArgumentNullException>(() => settings.DecryptOnlineValidationCode());
        Assert.Throws<ArgumentNullException>(() => settings.DecryptKey());
    }
}
