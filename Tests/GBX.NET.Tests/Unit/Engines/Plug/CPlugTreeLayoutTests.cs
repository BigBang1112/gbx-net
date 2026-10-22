using GBX.NET.Components;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugTreeLayoutTests
{
    [Test]
    public async Task GetAllChildren_MissingMipTree_ContinuesToOtherLevelsAndChildren()
    {
        var descendant = new CPlugTree();
        var sibling = new CPlugTree();
        var mip = new CPlugTreeVisualMip
        {
            Levels = [new(10, null), new(20, new CPlugTree { Children = [descendant] })]
        };
        var root = new CPlugTree { Children = [mip, sibling] };

        await Assert.That(root.GetAllChildren(includeVisualMipLevels: true).ToArray())
            .IsEquivalentTo(new CPlugTree[] { mip, descendant, sibling }, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task GetAllChildrenWithLocation_MissingMipTree_PreservesRegularChildren(bool emptyLevels)
    {
        var child = new CPlugTree { Location = Iso4.Identity with { TX = 2 } };
        var mip = new CPlugTreeVisualMip
        {
            Children = [child],
            Location = Iso4.Identity with { TY = 3 },
            Levels = emptyLevels ? [] : [new(10, null)]
        };

        var children = mip.GetAllChildrenWithLocation().ToArray();

        await Assert.That(children).HasSingleItem();
        await Assert.That(children[0].Tree).IsSameReferenceAs(child);
        await Assert.That(children[0].Location).IsEqualTo(Iso4.Identity with { TX = 2, TY = 3 });
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_ModelReference_ControlsModelIdAndPropertyPayload(bool hasModel)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x0904F00Du);
            writer.Write(new Id(123));
            writer.WriteNodeRef(hasModel ? new CPlugSolid() : null);
            if (hasModel) writer.Write(new Id(7));
            writer.Write(0x0904F016u);
            if (!hasModel)
            {
                for (var i = 0; i < 4; i++) writer.Write(-1);
            }
            writer.Write(0x0904F01Au);
            writer.Write(0x80008u);
            writer.Write(0xFACADE01u);
        }

        payload.Position = 0;
        CPlugTree node;
        using (var reader = new GbxReader(payload)) node = reader.ReadNode<CPlugTree>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.PlugId).IsEqualTo(new Id(123));
        await Assert.That(node.HasModel).IsEqualTo(hasModel);
        await Assert.That(node.ModelPlugId).IsEqualTo(hasModel ? "7" : null);
        await Assert.That(node.IsCustomBBox).IsTrue();
        await Assert.That(node.Flags).IsEqualTo(0x8A008UL);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        payload.Position = payload.Length - 8;
        using (var writer = new GbxWriter(payload)) writer.Write(0x8A008u);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments("")]
    [Arguments("Tree")]
    [Arguments("123")]
    public async Task ReadWrite_StringPlugIds_PreserveTheirRepresentation(string value)
    {
        var chunk = new CPlugTree.Chunk0904F00D();
        var expected = new Id(value);
        var node = new CPlugTree { PlugId = expected };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);

        payload.Position = 0;
        var restored = new CPlugTree();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(restored, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(restored.PlugId).IsEqualTo(expected);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(restored, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_LegacyParameterIds_DoNotConsumeTheFollowingChunk()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x0904F00Cu);
            writer.Write(2);
            writer.WriteIdAsString("Shader");
            writer.WriteIdAsString("Surface");
            writer.Write(0x0904F01Au);
            writer.Write(8u);
            writer.Write(0xFACADE01u);
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugTree>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.NodParameterIds).IsEquivalentTo(new[] { "Shader", "Surface" }, CollectionOrdering.Matching);
        await Assert.That(node.Flags).IsEqualTo(0xA008UL);
    }

    [Test]
    [Arguments(0x00E, 3)]
    [Arguments(0x012, 4)]
    public async Task ReadWrite_LegacyPropertyChunks_PreserveTheirReferenceOrder(int offset, int count)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x0904F000u + (uint)offset);
            writer.WriteNodeRef(new CPlugVisual());
            writer.WriteNodeRef(new CPlugShader());
            writer.WriteNodeRef(new CPlugSurface());
            if (count >= 4) writer.WriteNodeRef(new CPlugTreeGenerator());
            writer.Write(0xFACADE01u);
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugTree>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Visual).IsNotNull();
        await Assert.That(node.Surface).IsTypeOf<CPlugSurface>();
        await Assert.That(node.Generator is not null).IsEqualTo(count >= 4);
        await Assert.That(node.Shader).IsTypeOf<CPlugShader>();
    }

    [Test]
    [Arguments(0x010, false, 0x1FE812UL)]
    [Arguments(0x013, false, 0x1F6812UL)]
    [Arguments(0x015, false, 0x1F2812UL)]
    [Arguments(0x018, true, 0x1F2812UL)]
    [Arguments(0x019, false, 0xFF1F2812UL)]
    [Arguments(0x01A, false, 0xFF1F2012UL)]
    public async Task ReadWrite_FlagUpgrades_ApplyOnlyOnRead(int offset, bool wide, ulong expected)
    {
        var chunk = FlagChunk(offset);
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (wide) writer.Write(0x12345678FF1F0012UL);
            else writer.Write(0xFF1F0012u);
        }

        var node = new CPlugTree();
        payload.Position = 0;
        using (var reader = new GbxReader(payload) { IsRelease = false })
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Flags).IsEqualTo(expected);

        node.Flags = 0x12345678FF1F0012UL;
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(node.Flags).IsEqualTo(wide ? 0x12345678FF1F0012UL : 0xFF1F0012UL);
        await Assert.That(saved.Length).IsEqualTo(wide ? 8L : 4L);
        saved.Position = 0;
        using var savedReader = new GbxReader(saved);
        if (wide) await Assert.That(savedReader.ReadUInt64()).IsEqualTo(0x12345678FF1F0012UL);
        else await Assert.That(savedReader.ReadUInt32()).IsEqualTo(0xFF1F0012u);
    }

    [Test]
    public async Task ReadWrite_Location_UsesTwelveFloatsAndReleaseSetsRooted()
    {
        var location = new Iso4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(4u);
            writer.Write(location);
        }

        var node = new CPlugTree();
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) new CPlugTree.Chunk0904F01A().ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(52L);
        await Assert.That(node.Location).IsEqualTo((Iso4?)location);
        await Assert.That(node.Flags).IsEqualTo(0xA004UL);
    }

    [Test]
    public async Task Constructor_DefaultsAndExternalModelPresence_MatchNativeState()
    {
        var node = new CPlugTree();
        await Assert.That(node.Flags).IsEqualTo(0x1E80AUL);
        await Assert.That(node.Location).IsEqualTo((Iso4?)Iso4.Identity);
        foreach (var game in new[] { GameVersion.TM10, GameVersion.TMPU, GameVersion.TMSX })
            await Assert.That(new CPlugTree(game).Flags).IsEqualTo(0x1E00AUL);

        node.Flags |= 1UL << 60;
        node.IsCustomBBox = true;
        node.IsCustomBBox = false;
        await Assert.That(node.Flags).IsEqualTo((1UL << 60) | 0x1E80AUL);

        node.References.ModelSolid.Descriptor = new GbxRefTableFile(new GbxRefTable(), 0, true, "Model.Solid.Gbx");
        await Assert.That(node.HasModel).IsTrue();
    }

    private static Chunk<CPlugTree> FlagChunk(int offset) => offset switch
    {
        0x010 => new CPlugTree.Chunk0904F010(),
        0x013 => new CPlugTree.Chunk0904F013(),
        0x015 => new CPlugTree.Chunk0904F015(),
        0x018 => new CPlugTree.Chunk0904F018(),
        0x019 => new CPlugTree.Chunk0904F019(),
        0x01A => new CPlugTree.Chunk0904F01A(),
        _ => throw new ArgumentOutOfRangeException(nameof(offset))
    };
}
