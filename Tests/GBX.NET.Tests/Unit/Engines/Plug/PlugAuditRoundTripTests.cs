using GBX.NET.Engines.Control;
using GBX.NET.Engines.MetaNotPersistent;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class PlugAuditRoundTripTests
{
    [Test]
    public async Task ReadWrite_SoundMapping_WritesLegacyFlagAndModernEnum()
    {
        var node = new CPlugSoundMulti();
        var legacy = new CPlugSoundMulti.Chunk09064001();
        var modern = new CPlugSoundMulti.Chunk09064003();
        using var original = new MemoryStream();
        using (var writer = new GbxWriter(original))
        {
            writer.WriteListNodeRef_deprec<CMwNod>(null);
            writer.Write(false);
            writer.Write(2);
            writer.Write(false);
            writer.Write(0);
            writer.Write(-1);
            writer.Write((int)CPlugSoundMulti.ESoundInputMapping.Scale);
        }

        await VerifyPayloadAsync(original, rw =>
        {
            legacy.ReadWrite(node, rw);
            modern.ReadWrite(node, rw);
        });
        await Assert.That(node.InputMapping).IsEqualTo(CPlugSoundMulti.ESoundInputMapping.Scale);
    }

    [Test]
    public async Task ReadWrite_VariantTags_PreservesDuplicateKeysAndOrder()
    {
        var variant = new NPlugItem_SVariant();
        var tags = new KeyValuePair<string, string>[]
        {
            new("MatModifier", "Glass"), new("Variant", "Day"), new("MatModifier", "Metal")
        };
        using var original = new MemoryStream();
        using (var writer = new GbxWriter(original))
        {
            writer.Write(tags.Length);
            foreach (var tag in tags)
            {
                writer.Write(tag.Key);
                writer.Write(tag.Value);
            }
            writer.Write(-1);
            writer.Write(true);
        }

        await VerifyPayloadAsync(original, rw => variant.ReadWrite(rw, v: 1));
        await Assert.That(variant.Tags).IsEquivalentTo(tags, CollectionOrdering.Matching);
        await Assert.That(variant.HiddenInManualCycle).IsTrue();
    }

    [Test]
    public async Task ReadWrite_BitmapSubTexture_PreservesNumericIdAndDigitName()
    {
        var subTexture = new CPlugBitmapAtlas.SubTex();
        using var original = new MemoryStream();
        using (var writer = new GbxWriter(original))
        {
            writer.Write(new Id(0));
            writer.Write(new Id("1"));
            for (var i = 0; i < 6; i++) writer.Write(i);
        }

        await VerifyPayloadAsync(original, rw => subTexture.ReadWrite(rw, 0));
        await Assert.That(subTexture.U01).IsEqualTo("0");
        await Assert.That(subTexture.U02).IsEqualTo("1");
    }

    [Test]
    public async Task ReadWrite_IconIndex_PreservesNumericIdsAndDigitNames()
    {
        var node = new CPlugIconIndex();
        var chunk = new CPlugIconIndex.Chunk09093003();
        using var original = new MemoryStream();
        using (var writer = new GbxWriter(original))
        {
            writer.Write(new Id(0));
            for (var i = 0; i < 8; i++) writer.Write(i);
            writer.Write(0.25f);
            writer.Write(0.5f);
            foreach (var id in new Id[] { new(0), new("0"), new(1), new("1"), new(), new("Icon") })
            {
                writer.Write(id);
            }
        }

        await VerifyPayloadAsync(original, rw => chunk.ReadWrite(node, rw));
        await Assert.That(((CPlugIconIndex.Chunk09093000)chunk).U01).IsEqualTo("0");
        await Assert.That(chunk.U01!).IsEquivalentTo(new[] { "0", "0", "1", "1", "", "Icon" }, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_ControlQuad_PreservesNumericIconId(bool versioned)
    {
        var node = new CControlQuad();
        Chunk<CControlQuad> chunk = versioned ? new CControlQuad.Chunk0701B007() : new CControlQuad.Chunk0701B006();
        using var original = new MemoryStream();
        using (var writer = new GbxWriter(original))
        {
            if (versioned) writer.Write(0);
            writer.Write(new Id(1));
            if (versioned)
            {
                writer.Write(new Vec3(1, 1, 1));
                writer.Write(1f);
            }
        }

        await VerifyPayloadAsync(original, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.IconId).IsEqualTo("1");
    }

    private static async Task VerifyPayloadAsync(MemoryStream original, Action<GbxReaderWriter> serialize)
    {
        original.Position = 0;
        using (var reader = new GbxReader(original))
        using (var rw = new GbxReaderWriter(reader)) serialize(rw);
        await Assert.That(original.Position).IsEqualTo(original.Length);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) serialize(rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(original.ToArray(), CollectionOrdering.Matching);
    }
}
