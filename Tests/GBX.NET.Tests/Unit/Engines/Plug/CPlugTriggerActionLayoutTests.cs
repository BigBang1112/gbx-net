using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugTriggerActionLayoutTests
{
    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    public async Task ReadWrite_Archive_PreservesNativeVersionBranches(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (version >= 5) writer.Write(-123);
            if (version >= 4)
            {
                writer.Write(0.5f);
                writer.Write(new Vec3(1, 2, 3));
            }
            if (version >= 3) writer.Write(-456);
            if (version >= 2) writer.Write(-1);
            if (version >= 1)
            {
                writer.Write(5); // CapsuleObject.
                writer.Write(3000);
            }
            if (version >= 0)
            {
                writer.Write(0.25f);
                writer.Write(new Vec3(4, 5, 6));
                writer.Write(1.5f);
                writer.Write(2.5f);
                writer.WriteIdAsString("Trigger");
            }
            writer.Write(0x11223344);
        }

        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugTriggerAction { U01 = 99 };
        using (var reader = new GbxReader(payload))
        {
            node.Read(reader, version);
            await Assert.That(payload.Position).IsEqualTo((long)expected.Length);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }

        await Assert.That(node.U01).IsEqualTo(version >= 5 ? -123 : 1);
        await Assert.That(node.EndYaw).IsEqualTo(version >= 4 ? 0.5f : 0f);
        await Assert.That(node.EndPos).IsEqualTo(version >= 4 ? new Vec3(1, 2, 3) : default);
        await Assert.That(node.U06).IsEqualTo(version >= 3 ? -456 : 0);
        await Assert.That(node.ActionIndex).IsEqualTo(version >= 2 ? -1 : 0);
        await Assert.That(node.Type).IsEqualTo(version >= 1 ? CPlugTriggerAction.EType.CapsuleObject : CPlugTriggerAction.EType.Capsule);
        await Assert.That(node.U09).IsEqualTo(version >= 1 ? 3000 : 0);
        await Assert.That(node.StartYaw).IsEqualTo(version >= 0 ? 0.25f : 0f);
        await Assert.That(node.StartPos).IsEqualTo(version >= 0 ? new Vec3(4, 5, 6) : default);
        await Assert.That(node.Radius).IsEqualTo(version >= 0 ? 1.5f : 1f);
        await Assert.That(node.Height).IsEqualTo(version >= 0 ? 2.5f : 1f);
        await Assert.That(node.U16).IsEqualTo(version >= 0 ? "Trigger" : null);

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output)) node.Write(writer, version);
        await Assert.That(output.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(4)]
    public async Task Write_LegacyArchive_PreservesVersionFiveValue(int version)
    {
        var node = new CPlugTriggerAction { U01 = 99 };
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output)) node.Write(writer, version);
        await Assert.That(node.U01).IsEqualTo(99);
    }

    [Test]
    public async Task ReadWrite_UnknownTriggerType_RoundTrips()
    {
        var node = new CPlugTriggerAction { Type = (CPlugTriggerAction.EType)104 };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload)) node.Write(writer, 1);
        payload.Position = 0;
        var restored = new CPlugTriggerAction();
        using (var reader = new GbxReader(payload)) restored.Read(reader, 1);
        await Assert.That((int)restored.Type).IsEqualTo(104);
    }
}
