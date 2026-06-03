namespace GBX.NET.PAK.Tests;

public class PakTests
{
    [Fact]
    public void DeriveKeyFromMasterServer_ValidDerivedKey()
    {
        var key = "59D87AEFB33C595D00A864F27E591C54";
        var checksum = "25FA258CC701591F13702EAC817318D86ED3907404A2C5A84E663D2DCE52C67D";

        var derivedKey = Pak.DeriveKeyFromMasterServer(key, Convert.FromHexString(checksum));

        Assert.Equal(expected: "71E67CF23565F11F338AACC7EBD81DF1", actual: Convert.ToHexString(derivedKey));
    }
}
