using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVisual2DLayoutTests
{
    [Test]
    public async Task LegacyComponents_WriteTheSameNativeRecord()
    {
#pragma warning disable CS0618
        var vertex = new CPlugVisual2D.Vertex2D
        {
            U01 = 1, U02 = 2, U03 = 3, U04 = 4,
            U05 = 5, U06 = 6, U07 = 7, U08 = 8
        };
#pragma warning restore CS0618

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) vertex.Write(writer);
        using var expected = new MemoryStream();
        using (var writer = new GbxWriter(expected))
        {
            for (var value = 1; value <= 8; value++) writer.Write((float)value);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Vertices2D_EmbeddedChunk_PreservesNativeRecords(bool empty, bool quads)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x0904A000u);
            writer.Write(empty ? 0 : 2);
            if (!empty)
            {
                // Native GxVertex2 records contain position XY, normal XY, and color RGBA.
                foreach (var value in new float[] { 1, 2, -3, 4, 0.125f, 0.25f, 0.5f, 1,
                    -5, 6, 7, -8, 0.75f, 0.5f, 0.25f, 0 }) writer.Write(value);
            }
            writer.Write(0xFACADE01u);
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        CPlugVisual2D node = quads
            ? reader.ReadNode<CPlugVisualQuads2D>()!
            : reader.ReadNode<CPlugVisual2D>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Vertices2D.Length).IsEqualTo(empty ? 0 : 2);
        if (!empty)
        {
            await Assert.That(node.Vertices2D[0].Position).IsEqualTo(new Vec2(1, 2));
            await Assert.That(node.Vertices2D[0].Normal).IsEqualTo(new Vec2(-3, 4));
            await Assert.That(node.Vertices2D[0].Color).IsEqualTo(new Color(0.125f, 0.25f, 0.5f, 1));
            await Assert.That(node.Vertices2D[1].Position).IsEqualTo(new Vec2(-5, 6));
            await Assert.That(node.Vertices2D[1].Normal).IsEqualTo(new Vec2(7, -8));
            await Assert.That(node.Vertices2D[1].Color).IsEqualTo(new Color(0.75f, 0.5f, 0.25f, 0));
        }

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }
}
