namespace GBX.NET.Tests.Unit;

public class GbxPathTests
{
    [Test]
    [Arguments("Race.Map.Gbx", "Race", ".Map.Gbx")]
    [Arguments("Race.Gbx", "Race", ".Gbx")]
    [Arguments("Race", "Race", "")]
    [Arguments("Race.Map.Replay.Gbx", "Race.Map", ".Replay.Gbx")]
    public async Task FileNameAndExtensionHandleGbxSuffixes(string path, string fileName, string extension)
    {
        await Assert.That(GbxPath.GetFileNameWithoutExtension(path)).IsEqualTo(fileName);
        await Assert.That(GbxPath.GetExtension(path)).IsEqualTo(extension);
    }

    [Test]
    public async Task ChangeExtensionReplacesBothGbxSuffixesAndPreservesDirectory()
    {
        var path = Path.Combine("Tracks", "Race.Map.Gbx");

        await Assert.That(GbxPath.ChangeExtension(path, "Replay.Gbx"))
            .IsEqualTo(Path.Combine("Tracks", "Race.Replay.Gbx"));
        await Assert.That(GbxPath.ChangeExtension(path, null))
            .IsEqualTo(Path.Combine("Tracks", "Race"));
    }

    [Test]
    public async Task NullPathRemainsNull()
    {
        await Assert.That(GbxPath.GetFileNameWithoutExtension(null)).IsNull();
        await Assert.That(GbxPath.GetExtension(null)).IsNull();
        await Assert.That(GbxPath.ChangeExtension(null, "Gbx")).IsNull();
    }

    [Test]
    public async Task GetValidFileNameReplacesWindowsReservedAndControlCharacters()
    {
        const string input = "A:B/C\\D*E?F\"G|H<I>J\u0001K\u001fL";

        await Assert.That(GbxPath.GetValidFileName(input)).IsEqualTo("A_B_C_D_E_F_G_H_I_J_K_L");
        await Assert.That(GbxPath.GetValidFileName("Žlutý 🏁.Map.Gbx")).IsEqualTo("Žlutý 🏁.Map.Gbx");
    }
}
