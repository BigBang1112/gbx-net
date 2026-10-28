using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugTreeGeneratorLayoutTests
{
    [Test]
    public async Task Constructor_NativeDefaults_ApplyToDerivedGenerators()
    {
        var node = new CPlugTreeGenerator();
        var crystal = new CPlugCrystal();
        await Assert.That(node.Flags).IsEqualTo(2u);
        await Assert.That(node.IsSaveGenerated).IsFalse();
        await Assert.That(node.IsToKeepInSaveAsRelease).IsTrue();
        await Assert.That(crystal.Flags).IsEqualTo(2u);
    }

    [Test]
    [Arguments(false, false, 0x80000000u)]
    [Arguments(true, false, 0x80000001u)]
    [Arguments(false, true, 0x80000002u)]
    [Arguments(true, true, 0x80000003u)]
    public async Task FlagProperties_WhenChanged_PreserveOtherBits(bool saveGenerated, bool keepInRelease, uint expected)
    {
        var node = new CPlugTreeGenerator { Flags = 0x80000003u };
        node.IsSaveGenerated = saveGenerated;
        node.IsToKeepInSaveAsRelease = keepInRelease;
        await Assert.That(node.Flags).IsEqualTo(expected);
        await Assert.That(node.IsSaveGenerated).IsEqualTo(saveGenerated);
        await Assert.That(node.IsToKeepInSaveAsRelease).IsEqualTo(keepInRelease);

        node.Flags = 0x80000000u;
        node.IsSaveGenerated = saveGenerated;
        node.IsToKeepInSaveAsRelease = keepInRelease;
        await Assert.That(node.Flags).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0u)]
    [Arguments(1u)]
    [Arguments(2u)]
    [Arguments(3u)]
    [Arguments(uint.MaxValue)]
    public async Task ReadWrite_Flags_KeepTheirFourByteWireLayout(uint flags)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(flags);
            writer.Write(0x11223344);
        }
        payload.Position = 0;
        var node = new CPlugTreeGenerator();
        var chunk = new CPlugTreeGenerator.Chunk09051000();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(payload.Position).IsEqualTo(4L);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.Flags).IsEqualTo(flags);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..4], CollectionOrdering.Matching);
    }
}
