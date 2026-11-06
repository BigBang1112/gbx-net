using GBX.NET.Engines.Game;

namespace GBX.NET.Crypto.Tests;

public class CGamePlayerProfileExtensionsTests
{
    [Test]
    [Arguments("00010203040506071D64EF84BA1E1150F117FE2D82CC0C58")]
    [Arguments("00010203040506071d64ef84ba1e1150f117fe2d82cc0c58")]
    public async Task DecryptOnlineSupportKey_NativeFormatVector_ReturnsPlaintext(string hex)
    {
        // Reconstructed from TmForever's CBC algorithm with IV 0001020304050607 and zero padding.
        var profile = new CGamePlayerProfile { OnlineSupportKeyHexa = hex };

        await Assert.That(profile.DecryptOnlineSupportKey()).IsEqualTo("SupportKey");
    }

    [Test]
    [Arguments((string?)null)]
    [Arguments("")]
    public async Task DecryptOnlineSupportKey_NoStoredKey_ReturnsEmptyString(string? hex)
    {
        var profile = new CGamePlayerProfile { OnlineSupportKeyHexa = hex };

        await Assert.That(profile.DecryptOnlineSupportKey()).IsEqualTo(string.Empty);
    }

    [Test]
    public void DecryptOnlineSupportKey_NullProfile_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => CGamePlayerProfileExtensions.DecryptOnlineSupportKey(null!));
    }
}
