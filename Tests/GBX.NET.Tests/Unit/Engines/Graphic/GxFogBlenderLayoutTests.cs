using GBX.NET.Engines.Graphic;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Graphic;

[Category("Unit")]
public class GxFogBlenderLayoutTests
{
    [Test]
    [Arguments(0, false, 0)]
    [Arguments(3, true, 0)]
    [Arguments(3, false, 43)]
    public async Task NativeKeyListPreservesSettingsAndSharedFogReferences(int keyCount, bool enabled, int legacyMode)
    {
        var fog = new GxFog { DepthMax = 1234, Color = new Vec3(0.25f, 0.5f, 0.75f) };
        fog.CreateChunk<GxFog.Chunk04004000>();
        fog.CreateChunk<GxFog.Chunk04004001>();
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            writer.Write(enabled ? 1 : 0);
            writer.Write(legacyMode);
            writer.Write(keyCount);
            for (var i = 0; i < keyCount; i++)
            {
                writer.Write((i + 1) * 0.25f);
                writer.WriteNodeRef(i == 1 ? null : fog);
            }
            writer.Write(0xDEADBEEFu);
        }

        var node = new GxFogBlender();
        var chunk = new GxFogBlender.Chunk04008000();
        await Assert.That(node.Enabled).IsTrue();
        await Assert.That(node.LegacyMode).IsEqualTo(1);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020);

        input.Position = 0;
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }

        await Assert.That(node.Enabled).IsEqualTo(enabled);
        await Assert.That(node.LegacyMode).IsEqualTo(legacyMode);
        await Assert.That(node.Keys).IsNotNull();
        await Assert.That(node.Keys!.Count).IsEqualTo(keyCount);
        if (keyCount != 0)
        {
            await Assert.That(node.Keys[0].Time.TotalSeconds).IsEqualTo(0.25f);
            await Assert.That(node.Keys[1].Time.TotalSeconds).IsEqualTo(0.5f);
            await Assert.That(node.Keys[2].Time.TotalSeconds).IsEqualTo(0.75f);
            await Assert.That(node.Keys[1].Fog).IsNull();
            await Assert.That(node.Keys[0].Fog).IsNotNull();
            await Assert.That(node.Keys[2].Fog).IsSameReferenceAs(node.Keys[0].Fog);
            await Assert.That(node.Keys[0].Fog!.DepthMax).IsEqualTo(1234f);
            await Assert.That(node.Keys[0].Fog!.Color).IsEqualTo(fog.Color);
        }

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(node, rw);
            writer.Write(0xDEADBEEFu);
        }
        await Assert.That(output.ToArray()).IsEquivalentTo(input.ToArray(), CollectionOrdering.Matching);
    }
}
