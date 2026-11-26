using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnBlockTests
{
    [Test]
    public async Task Name_WhenChanged_UpdatesBlockModelAndPreservesCollectionAndAuthor()
    {
        var block = new CGameCtnBlock { BlockModel = new Ident("Old", "Stadium", "Maker") };

        block.Name = "New";

        await Assert.That(block.BlockModel).IsEqualTo(new Ident("New", "Stadium", "Maker"));
        await Assert.That(block.Name).IsEqualTo("New");
    }

    [Test]
    public async Task VariantAndSubVariant_WhenChanged_PreserveOtherFlags()
    {
        var block = new CGameCtnBlock { Flags = (1 << 12) | (1 << 28) };

        block.Variant = 63;
        block.SubVariant = 42;

        await Assert.That(block.Variant).IsEqualTo((byte)63);
        await Assert.That(block.SubVariant).IsEqualTo((byte)42);
        await Assert.That(block.Flags).IsEqualTo((1 << 12) | (1 << 28) | (42 << 6) | 63);

        block.Variant = 7;
        await Assert.That(block.SubVariant).IsEqualTo((byte)42);
        await Assert.That(block.IsGhost).IsTrue();
    }

    [Test]
    public async Task PackedIndices_WhenOutOfRange_DoNotOverwriteAdjacentBits()
    {
        const int preservedFlags = unchecked((int)0xD0001000);
        var block = new CGameCtnBlock { Flags = preservedFlags };

        block.Variant = 255;
        block.SubVariant = 128;
        block.BlockInfoVariantIndex = 255;

        await Assert.That(block.Variant).IsEqualTo((byte)63);
        await Assert.That(block.SubVariant).IsEqualTo((byte)0);
        await Assert.That(block.BlockInfoVariantIndex).IsEqualTo((byte)127);
        await Assert.That(block.Flags).IsEqualTo(preservedFlags | 63 | (127 << 21));

        block.BlockInfoVariantIndex = 42;
        block.Bit21 = true;
        await Assert.That(block.BlockInfoVariantIndex).IsEqualTo((byte)43);
        block.Bit21 = false;
        await Assert.That(block.BlockInfoVariantIndex).IsEqualTo((byte)42);
        await Assert.That(block.Flags).IsEqualTo(preservedFlags | 63 | (42 << 21));
    }

    [Test]
    public async Task IsEditable_SharesLegacyIsClipFlag()
    {
        var block = new CGameCtnBlock { IsEditable = true, SubVariant = 7 };

        await Assert.That(block.IsClip).IsTrue();
        await Assert.That(block.Flags).IsEqualTo((1 << 13) | (7 << 6));

        block.IsClip = false;
        await Assert.That(block.IsEditable).IsFalse();
        await Assert.That(block.SubVariant).IsEqualTo((byte)7);
    }

    [Test]
    [Arguments(null, false)]
    [Arguments("", false)]
    [Arguments("Maker", true)]
    public async Task Skin_WhenRemoved_PreservesOnlyAuthorPresence(string? author, bool expectedPresence)
    {
        var block = new CGameCtnBlock { Author = author, Skin = new CGameCtnBlockSkin(), IsGhost = true };

        block.Skin = null;

        await Assert.That((block.Flags & (1 << 15)) != 0).IsEqualTo(expectedPresence);
        await Assert.That(block.IsGhost).IsTrue();
    }

    [Test]
    public async Task PhysicsAndCardEvents_WhenAssignedAndCleared_UpdatePresenceBits()
    {
        var block = new CGameCtnBlock { IsGhost = true };
        block.PhyCharSpecialProperty = new CPlugCharPhySpecialProperty();
        block.SquareCardEventIds = [new()];

        await Assert.That(block.Flags).IsEqualTo((1 << 28) | (1 << 19) | (1 << 18));

        block.PhyCharSpecialProperty = null;
        block.SquareCardEventIds = [];

        await Assert.That(block.Flags).IsEqualTo(1 << 28);
    }

    [Test]
    public async Task BlockArchive_VersionZero_DoesNotSignExtendSkinFlag()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.WriteIdAsString("Block");
            writer.Write((byte)Direction.North);
            writer.Write(new Byte3(1, 2, 3));
            writer.Write((ushort)0x8001);
            writer.WriteIdAsString("Maker");
            writer.Write(-1); // Null skin node reference.
            writer.Write(0x12345678);
        }

        var block = new CGameCtnBlock();
        using var input = new MemoryStream(payload.ToArray());
        using var reader = new GbxReader(input);
        block.Read(reader, v: 0);

        await Assert.That(block.Flags).IsEqualTo(0x8001);
        await Assert.That(block.IsGhost).IsFalse();
        await Assert.That(block.BlockInfoVariantIndex).IsEqualTo((byte)0);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        {
            block.Write(writer, v: 0);
            writer.Write(0x12345678);
        }
        await Assert.That(output.ToArray().SequenceEqual(payload.ToArray())).IsTrue();
    }

    [Test]
    public async Task BlockArchive_VersionZeroWrite_DoesNotTruncateInMemoryFlags()
    {
        const int flags = unchecked((int)0xC0000001);
        var block = new CGameCtnBlock { Name = "Block", Flags = flags };
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);

        block.Write(writer, v: 0);

        await Assert.That(block.Flags).IsEqualTo(flags);
    }

    [Test]
    [Arguments(1)]
    [Arguments(5)]
    [Arguments(6)]
    public async Task BlockArchive_PreservesPackedIndicesAndUnknownUpperBits(int version)
    {
        const int flags = unchecked((int)0xF8000000) | (42 << 21) | (1 << 16) | (7 << 6) | 3;
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.WriteIdAsString("Block");
            writer.Write((byte)Direction.East);
            writer.Write(new Byte3(4, 5, 6));
            writer.Write(flags);
            writer.Write(0x12345678);
        }

        var block = new CGameCtnBlock();
        using var input = new MemoryStream(payload.ToArray());
        using var reader = new GbxReader(input);
        block.Read(reader, version);
        await Assert.That(block.Flags).IsEqualTo(flags);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        {
            block.Write(writer, version);
            writer.Write(0x12345678);
        }
        await Assert.That(output.ToArray().SequenceEqual(payload.ToArray())).IsTrue();
    }

    [Test]
    public async Task LegacyChunk_ClampsTwelveBitMobilVariant()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(new Ident("Block", "Stadium", "Maker"));
            writer.Write((byte)Direction.North);
            writer.Write(new Byte3(1, 2, 3));
            writer.Write(7 | (64 << 12) | (1 << 24));
        }

        var block = new CGameCtnBlock();
        using var input = new MemoryStream(payload.ToArray());
        using var reader = new GbxReader(input);
        new CGameCtnBlock.Chunk03057002().Read(block, reader);

        await Assert.That(block.Variant).IsEqualTo((byte)7);
        await Assert.That(block.SubVariant).IsEqualTo((byte)63);
        await Assert.That(block.IsGround).IsTrue();
        await Assert.That(block.IsEditable).IsFalse();
        await Assert.That(input.Position).IsEqualTo(input.Length);
    }

    [Test]
    public async Task FlagProperties_WhenChanged_SetAndClearIndependently()
    {
        var block = new CGameCtnBlock
        {
            IsGround = true,
            IsClip = true,
            IsPillar = true,
            IsReplacement = true,
            IsGhost = true,
            IsFree = true
        };

        await Assert.That(block.Flags).IsEqualTo((1 << 12) | (1 << 13) | (1 << 14) | (1 << 16) | (1 << 28) | (1 << 29));

        block.IsClip = false;
        block.IsGhost = false;

        await Assert.That(block.IsGround).IsTrue();
        await Assert.That(block.IsPillar).IsTrue();
        await Assert.That(block.IsReplacement).IsTrue();
        await Assert.That(block.IsFree).IsTrue();
        await Assert.That(block.Flags).IsEqualTo((1 << 12) | (1 << 14) | (1 << 16) | (1 << 29));
    }

    [Test]
    public async Task WaypointAndDecal_WhenChanged_UpdateFlags()
    {
        var block = new CGameCtnBlock();
        block.WaypointSpecialProperty = new CGameWaypointSpecialProperty();
        block.DecalId = "decal";

        await Assert.That(block.Flags & (1 << 20)).IsEqualTo(1 << 20);
        await Assert.That(block.Flags & (1 << 17)).IsEqualTo(1 << 17);

        block.WaypointSpecialProperty = null;
        block.DecalId = null;

        await Assert.That(block.Flags & ((1 << 20) | (1 << 17))).IsEqualTo(0);
    }
}
