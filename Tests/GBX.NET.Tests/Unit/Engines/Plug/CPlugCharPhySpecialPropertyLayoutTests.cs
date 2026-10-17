using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugCharPhySpecialPropertyLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ReadWrite_Versions_PreserveNativeFieldOrderAndLegacyValues(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteSettings(writer, version);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugCharPhySpecialProperty();
        var chunk = new CPlugCharPhySpecialProperty.Chunk090F2000();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.SpecialType).IsEqualTo((CPlugCharPhySpecialProperty.ESpecialType)(version % 4));
        await Assert.That(node.JumpPadFixedVel).IsTrue();
        await Assert.That(node.SpeedUpDuration.TotalMilliseconds).IsEqualTo(1234);
        await Assert.That(node.SpeedUpCoef).IsEqualTo(2.25f);
        await Assert.That(node.TurboDuration.TotalMilliseconds).IsEqualTo(2345);
        await Assert.That(node.TurboVal).IsEqualTo(4.5f);
        await Assert.That(node.FreeWheelingDuration.TotalMilliseconds).IsEqualTo(version >= 1 ? 3456 : 3000);
        await Assert.That(node.JumpPadDestinationOffset).IsEqualTo(version >= 2 ? new Vec3(12, 34, 56) : new Vec3(0, 5, 0));
        await Assert.That(node.TrajectoryCurve).IsEqualTo(version >= 2 ? 0.75f : 0.5f);
        await Assert.That(node.JumpPadSize).IsEqualTo(version >= 3 ? 3 : 0);
        await Assert.That(node.JumpPadCenterOnPad).IsEqualTo(version < 4);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false, 0)]
    [Arguments(false, 1)]
    [Arguments(false, 2)]
    [Arguments(false, 3)]
    [Arguments(false, 4)]
    [Arguments(true, 0)]
    [Arguments(true, 1)]
    [Arguments(true, 2)]
    [Arguments(true, 3)]
    [Arguments(true, 4)]
    public async Task ReadNode_NodeBodies_SupportCurrentAndLegacyChunkIds(bool legacyIds, int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(legacyIds ? 0x0A071000u : CPlugCharPhySpecialProperty.Id);
            WriteSettings(writer, version);
            writer.Write(0xFACADE01u);
        }
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugCharPhySpecialProperty>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Chunks.Count).IsEqualTo(1);
        await Assert.That(node.SpeedUpCoef).IsEqualTo(2.25f);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)
        {
            ClassIdRemapMode = legacyIds ? ClassIdRemapMode.Id2006 : ClassIdRemapMode.Latest
        }) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstruction()
    {
        var node = new CPlugCharPhySpecialProperty();
        await Assert.That(node.SpecialType).IsEqualTo(CPlugCharPhySpecialProperty.ESpecialType.JumpPad);
        await Assert.That(node.JumpPadDestinationOffset).IsEqualTo(new Vec3(0, 5, 0));
        await Assert.That(node.TrajectoryCurve).IsEqualTo(0.5f);
        await Assert.That(node.JumpPadFixedVel).IsFalse();
        await Assert.That(node.JumpPadCenterOnPad).IsTrue();
        await Assert.That(node.SpeedUpDuration.TotalMilliseconds).IsEqualTo(5000);
        await Assert.That(node.SpeedUpCoef).IsEqualTo(3f);
        await Assert.That(node.TurboDuration.TotalMilliseconds).IsEqualTo(3000);
        await Assert.That(node.TurboVal).IsEqualTo(3f);
        await Assert.That(node.FreeWheelingDuration.TotalMilliseconds).IsEqualTo(3000);
        await Assert.That(node.JumpPadSize).IsEqualTo(0);
    }

    private static void WriteSettings(GbxWriter writer, int version)
    {
        writer.Write(version);
        writer.Write(version % 4);
        if (version <= 1)
        {
            writer.Write(1.25f);
            writer.Write(-2.5f);
            writer.Write(3.75f);
        }
        writer.Write(true);
        writer.Write(1234);
        writer.Write(2.25f);
        writer.Write(2345);
        writer.Write(4.5f);
        if (version >= 1) writer.Write(3456);
        if (version >= 2)
        {
            writer.Write(new Vec3(12, 34, 56));
            writer.Write(0.75f);
        }
        if (version >= 3) writer.Write(3);
        if (version >= 4) writer.Write(false);
    }
}
