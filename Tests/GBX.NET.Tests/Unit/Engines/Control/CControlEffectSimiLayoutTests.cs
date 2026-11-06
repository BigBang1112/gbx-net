using GBX.NET.Engines.Control;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Control;

[Category("Unit")]
public class CControlEffectSimiLayoutTests
{
    [Test]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    public async Task ReadWrite_Key_ColorBlendAndRgbFollowDepthFromVersion3(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new BinaryWriter(payload, Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(2f); // Time.
            writer.Write(3f); // Position X.
            writer.Write(4f); // Position Y.
            writer.Write(0.25f); // Rotation.
            writer.Write(5f); // Scale X.
            writer.Write(6f); // Scale Y.
            writer.Write(0.75f); // Opacity.
            writer.Write(0.5f); // Depth.
            if (version >= 3)
            {
                writer.Write(0.625f); // Color blend.
                writer.Write(0.125f); // Red.
                writer.Write(0.25f); // Green.
                writer.Write(0.875f); // Blue.
            }
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var key = new CControlEffectSimi.Key();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            key.ReadWrite(rw, version);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }

        await Assert.That(key.Opacity).IsEqualTo(0.75f);
        await Assert.That(key.Depth).IsEqualTo(0.5f);
        await Assert.That(key.ColorBlend).IsEqualTo(version >= 3 ? 0.625f : 0f);
        await Assert.That(key.Color).IsEqualTo(version >= 3 ? new Vec3(0.125f, 0.25f, 0.875f) : Vec3.Zero);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) key.ReadWrite(rw, version);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }
}
