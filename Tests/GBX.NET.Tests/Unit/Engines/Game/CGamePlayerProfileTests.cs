using GBX.NET.Engines.Game;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGamePlayerProfileTests
{
    [Test]
    [Arguments("password", "5F4DCC3B5AA765D61D8327DEB882CF99", "2F2CBF482DC817B2")]
    [Arguments("A longer password: příliš žluťoučký 🏁", "5F13EF132CF9B2E195932A2317238D8A", "1E33837C429ED793B5E34B506454E2F83B29CF63E960714CF9FAEF8237E633E62AD64A7C593D3F8A562E0AD388AC0C")]
    public async Task DecryptPassword_KnownCiphertext_ReturnsPasswordWithoutChangingEncryptedBytes(
        string password,
        string digestHex,
        string encryptedHex)
    {
        var encrypted = Convert.FromHexString(encryptedHex);
        var profile = new CGamePlayerProfile
        {
            CryptedPassword = new Checksum128(Convert.FromHexString(digestHex)),
            EncryptedPassword = encrypted
        };

        await Assert.That(profile.DecryptPassword()).IsEqualTo(password);
        await Assert.That(profile.DecryptPassword()).IsEqualTo(password);
        await Assert.That(Convert.ToHexString(profile.EncryptedPassword!)).IsEqualTo(encryptedHex);
        await Assert.That(Convert.ToHexString(encrypted)).IsEqualTo(encryptedHex);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task DecryptPassword_NoStoredPassword_ReturnsEmptyString(bool hasEmptyBuffer)
    {
        var profile = new CGamePlayerProfile
        {
            EncryptedPassword = hasEmptyBuffer ? [] : null
        };

        await Assert.That(profile.DecryptPassword()).IsEqualTo(string.Empty);
    }
}
