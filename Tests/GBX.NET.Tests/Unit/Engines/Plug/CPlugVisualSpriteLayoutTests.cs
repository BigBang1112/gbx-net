using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVisualSpriteLayoutTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacyRenderModePreservesOtherSpriteFlags(bool sorting)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(unchecked((int)0xFFFFFFF9)); // Only the low three bits are the render mode.
            if (sorting) writer.Write(true);
        }

        var node = new CPlugVisualSprite { SpriteFlags = 0x80000068, Flags = 123 };
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            if (sorting) new CPlugVisualSprite.Chunk09010001().ReadWrite(node, rw);
            else new CPlugVisualSprite.Chunk09010000().ReadWrite(node, rw);
        }

        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.SpriteFlags).IsEqualTo(sorting ? 0x80000079u : 0x80000069u);
        await Assert.That(node.RenderMode).IsEqualTo(CPlugVisualSprite.ERenderMode.RotatedQuad);
        await Assert.That(node.RadiusInScreen).IsTrue();
        await Assert.That(node.UseGlobalDir).IsTrue();
        await Assert.That(node.UseTextureAtlas).IsTrue();
        await Assert.That(node.Flags).IsEqualTo(123);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            if (sorting) new CPlugVisualSprite.Chunk09010001().ReadWrite(node, rw);
            else new CPlugVisualSprite.Chunk09010000().ReadWrite(node, rw);
        }
        saved.Position = 0;
        using (var reader = new GbxReader(saved))
        {
            await Assert.That(reader.ReadInt32()).IsEqualTo(1);
            if (sorting) await Assert.That(reader.ReadBoolean()).IsTrue();
        }
        await Assert.That(saved.Position).IsEqualTo(saved.Length);

        node.SortBackToFront = false;
        node.RenderMode = CPlugVisualSprite.ERenderMode.Quad;
        await Assert.That(node.SpriteFlags).IsEqualTo(0x80000068u);
    }

    [Test]
    public async Task LegacySpriteChunksPreserveUnsignedFlagsAndAtlasCounts()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x09010002u);
            writer.Write(0xF1234567u);
            writer.Write(0x09010005u);
            writer.Write(0x80000069u);
            foreach (var value in new[] { 1.25f, -2.5f, 3.75f, -0.25f, 0.75f }) writer.Write(value);
            writer.Write(0x09010006u);
            writer.Write((ushort)40000);
            writer.Write((ushort)65000);
            writer.Write(0xFACADE01u);
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugVisualSprite>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.GetChunk<CPlugVisualSprite.Chunk09010002>()).IsNotNull();
        await Assert.That(node.SpriteFlags).IsEqualTo(0x80000069u);
        await Assert.That(node.GlobalDirection).IsEqualTo(new Vec3(1.25f, -2.5f, 3.75f));
        await Assert.That(node.PivotPoint).IsEqualTo(new Vec2(-0.25f, 0.75f));
        await Assert.That(node.AtlasGridCountU).IsEqualTo((ushort)40000);
        await Assert.That(node.AtlasGridCountV).IsEqualTo((ushort)65000);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        using var expected = new MemoryStream(payload.ToArray());
        expected.Position = 4;
        using (var writer = new GbxWriter(expected)) writer.Write(node.SpriteFlags);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(2)]
    public async Task AtlasRectanglesPreserveCountAndCoordinateOrder(int count)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(count);
            for (var i = 0; i < count; i++)
                foreach (var value in new[] { i + 0.125f, i + 0.25f, i + 0.5f, i + 0.75f }) writer.Write(value);
        }

        var node = new CPlugVisualSprite();
        await Assert.That(node.IsFlagBitSet(8)).IsTrue();
        await Assert.That(node.GlobalDirection).IsEqualTo(new Vec3(0, 1, 0));
        await Assert.That(node.AtlasGridCountU).IsEqualTo((ushort)1);
        await Assert.That(node.AtlasGridCountV).IsEqualTo((ushort)1);
        await Assert.That(node.AtlasTexCoords!.Single()).IsEqualTo(new Rect(0, 0, 1, 1));

        var chunk = new CPlugVisualSprite.Chunk09010009();
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.AtlasTexCoords!.Length).IsEqualTo(count);
        for (var i = 0; i < count; i++)
            await Assert.That(node.AtlasTexCoords[i]).IsEqualTo(new Rect(i + 0.125f, i + 0.25f, i + 0.5f, i + 0.75f));

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task SpriteParametersAreEmbeddedWithoutAClassIdOrReferenceIndex()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x090AC000u);
            writer.Write(0x12345678u);
            foreach (var value in new[] { 1f, 2f, 3f, 0.25f, 0.75f, 0.5f }) writer.Write(value);
            writer.Write(0xFACADE01u);
        }

        var node = new CPlugVisualSprite();
        var chunk = new CPlugVisualSprite.Chunk09010008();
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.SpriteParam!.GlobalDirection).IsEqualTo(new Vec3(1, 2, 3));
        await Assert.That(node.SpriteParam.PivotPoint).IsEqualTo(new Vec2(0.25f, 0.75f));
        await Assert.That(node.SpriteParam.GlobalDirTiltFactor).IsEqualTo(0.5f);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }
}
