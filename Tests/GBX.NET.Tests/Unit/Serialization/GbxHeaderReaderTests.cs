using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Serialization;

public class GbxHeaderReaderTests
{
    [Test]
    public async Task ReadUserData_EmptyUserData_ReturnsFalse()
    {
        // Arrange
        using var ms = new MemoryStream(BitConverter.GetBytes(0).ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r);

        // Act
        var result = parser.ReadUserData(node: null, unknownHeader: null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task ReadUserData_UserDataWithZeroHeaderChunks_ReturnsFalse()
    {
        // Arrange
        using var ms = new MemoryStream(
                    BitConverter.GetBytes(4)
            .Concat(BitConverter.GetBytes(0))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r);

        // Act
        var result = parser.ReadUserData(node: null, unknownHeader: null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public void ReadUserData_UnknownNodeAndNoHeaderObject_Throws()
    {
        // Arrange
        using var ms = new MemoryStream(
                    BitConverter.GetBytes(16)
            .Concat(BitConverter.GetBytes(1))
            .Concat(BitConverter.GetBytes(0x03043069))
            .Concat(BitConverter.GetBytes(4))
            .Concat(BitConverter.GetBytes(6))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r);

        // Act & Assert
        Assert.Throws<Exception>(() => parser.ReadUserData(node: null, unknownHeader: null));
    }

    [Test]
    public async Task ReadUserData_UnknownNode_ReadsAndAddsUnknownHeaderChunk()
    {
        // Arrange
        using var ms = new MemoryStream(
                    BitConverter.GetBytes(16)
            .Concat(BitConverter.GetBytes(1))
            .Concat(BitConverter.GetBytes(0x03043069))
            .Concat(BitConverter.GetBytes(4))
            .Concat(BitConverter.GetBytes(6))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r);

        var unknownHeader = new GbxHeaderUnknown(GbxHeaderBasic.Default, 0x03043000);

        // Act
        var result = parser.ReadUserData(node: null, unknownHeader);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(unknownHeader.UserData).HasSingleItem();
        await Assert.That(unknownHeader.UserData.First().IsHeavy).IsFalse().Because("Header chunk is heavy but should not be.");
        await Assert.That(unknownHeader.UserData.First().Id).IsEqualTo((uint)0x03043069);
        await Assert.That(((HeaderChunk)unknownHeader.UserData.First()).Data).IsEquivalentTo((byte[])[6, 0, 0, 0], CollectionOrdering.Matching);
        await Assert.That(ms.Position).IsEqualTo(20);
    }

    [Test]
    public async Task ReadUserData_KnownNode_ReadsAndAddsUnknownHeaderChunk()
    {
        // Arrange
        using var ms = new MemoryStream(
                    BitConverter.GetBytes(16)
            .Concat(BitConverter.GetBytes(1))
            .Concat(BitConverter.GetBytes(0x03043069))
            .Concat(BitConverter.GetBytes(4))
            .Concat(BitConverter.GetBytes(6))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r);

        var node = new CGameCtnChallenge();

        // Act
        var result = parser.ReadUserData(node, unknownHeader: null);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(node!.Chunks).HasSingleItem();
        await Assert.That(node.Chunks.First()).IsTypeOf<HeaderChunk>();
        await Assert.That(((HeaderChunk)node.Chunks.First()).IsHeavy).IsFalse().Because("Header chunk is heavy but should not be.");
        await Assert.That(node.Chunks.First().Id).IsEqualTo((uint)0x03043069);
        await Assert.That(((HeaderChunk)node.Chunks.First()).Data).IsEquivalentTo((byte[])[6, 0, 0, 0], CollectionOrdering.Matching);
        await Assert.That(ms.Position).IsEqualTo(20);
    }

    [Test]
    public async Task ReadUserData_KnownNode_CreatesAndReadsKnownHeaderChunk()
    {
        // Arrange
        using var ms = new MemoryStream(
                    BitConverter.GetBytes(16)
            .Concat(BitConverter.GetBytes(1))
            .Concat(BitConverter.GetBytes(0x03043004))
            .Concat(BitConverter.GetBytes(4))
            .Concat(BitConverter.GetBytes(6))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r);

        var node = new CGameCtnChallenge();

        // Act
        var result = parser.ReadUserData(node, unknownHeader: null);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(node!.Chunks).HasSingleItem();
        await Assert.That(node.Chunks.First()).IsTypeOf<CGameCtnChallenge.HeaderChunk03043004>();
        await Assert.That(((CGameCtnChallenge.HeaderChunk03043004)node.Chunks.First()).IsHeavy).IsFalse().Because("Header chunk is heavy but should not be.");
        await Assert.That(((CGameCtnChallenge.HeaderChunk03043004)node.Chunks.First()).Version).IsEqualTo(6);
        await Assert.That(ms.Position).IsEqualTo(20);
    }

    [Test]
    public async Task Parse_KnownNode_Version6_ParsesCorrectly()
    {
        // Arrange
        using var ms = new MemoryStream(new byte[] {
            (byte)'G',
            (byte)'B',
            (byte)'X',
            6, 0,
            (byte)'B',
            (byte)'U',
            (byte)'C',
            (byte)'R' }
            .Concat(BitConverter.GetBytes(0x03043000))
            .Concat(BitConverter.GetBytes(16))
            .Concat(BitConverter.GetBytes(1))
            .Concat(BitConverter.GetBytes(0x03043004))
            .Concat(BitConverter.GetBytes(4))
            .Concat(BitConverter.GetBytes(6))
            .Concat(BitConverter.GetBytes(69))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r);

        // Act
        var header = parser.Parse(out var node);

        // Assert
        await Assert.That(header.Basic.Version).IsEqualTo((ushort)6);
        await Assert.That(header.Basic.Format).IsEqualTo(GbxFormat.Binary);
        await Assert.That(header.Basic.CompressionOfRefTable).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(header.Basic.CompressionOfBody).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(header.Basic.Mode).IsEqualTo(GbxMode.Release);
        await Assert.That(header.ClassId).IsEqualTo((uint)0x03043000);
        await Assert.That(header).IsTypeOf<GbxHeader<CGameCtnChallenge>>();
        await Assert.That((object?)node).IsTypeOf<CGameCtnChallenge>();
        await Assert.That(node!.Chunks).HasSingleItem();
        await Assert.That(header.NumNodes).IsEqualTo(69);
    }

    [Test]
    public async Task Parse_KnownNode_VersionLowerThan6_ParsesCorrectly()
    {
        // Arrange
        using var ms = new MemoryStream(new byte[] {
            (byte)'G',
            (byte)'B',
            (byte)'X',
            4, 0,
            (byte)'B',
            (byte)'U',
            (byte)'C',
            (byte)'R' }
            .Concat(BitConverter.GetBytes(0x03043000))
            .Concat(BitConverter.GetBytes(69))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r);

        // Act
        var header = parser.Parse(out var node);

        // Assert
        await Assert.That(header.Basic.Version).IsEqualTo((ushort)4);
        await Assert.That(header.Basic.Format).IsEqualTo(GbxFormat.Binary);
        await Assert.That(header.Basic.CompressionOfRefTable).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(header.Basic.CompressionOfBody).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(header.Basic.Mode).IsEqualTo(GbxMode.Release);
        await Assert.That(header.ClassId).IsEqualTo((uint)0x03043000);
        await Assert.That(header).IsTypeOf<GbxHeader<CGameCtnChallenge>>();
        await Assert.That((object?)node).IsTypeOf<CGameCtnChallenge>();
        await Assert.That(node!.Chunks).IsEmpty();
        await Assert.That(header.NumNodes).IsEqualTo(69);
    }

    [Test]
    public async Task Parse_UnknownNode_Version6_ParsesCorrectly()
    {
        // Arrange
        using var ms = new MemoryStream(new byte[] {
            (byte)'G',
            (byte)'B',
            (byte)'X',
            6, 0,
            (byte)'B',
            (byte)'U',
            (byte)'C',
            (byte)'R' }
            .Concat(BitConverter.GetBytes(0x03999000))
            .Concat(BitConverter.GetBytes(16))
            .Concat(BitConverter.GetBytes(1))
            .Concat(BitConverter.GetBytes(0x03999004))
            .Concat(BitConverter.GetBytes(4))
            .Concat(BitConverter.GetBytes(6))
            .Concat(BitConverter.GetBytes(69))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r);

        // Act
        var header = parser.Parse(out var node);

        // Assert
        await Assert.That(header.Basic.Version).IsEqualTo((ushort)6);
        await Assert.That(header.Basic.Format).IsEqualTo(GbxFormat.Binary);
        await Assert.That(header.Basic.CompressionOfRefTable).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(header.Basic.CompressionOfBody).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(header.Basic.Mode).IsEqualTo(GbxMode.Release);
        await Assert.That(header.ClassId).IsEqualTo((uint)0x03999000);
        await Assert.That(header).IsTypeOf<GbxHeaderUnknown>();
        await Assert.That(((GbxHeaderUnknown)header).UserData).HasSingleItem();
        await Assert.That((object?)node).IsNull();
        await Assert.That(header.NumNodes).IsEqualTo(69);
    }

    [Test]
    public async Task Parse_UnknownNode_VersionLowerThan6_ParsesCorrectly()
    {
        // Arrange
        using var ms = new MemoryStream(new byte[] {
            (byte)'G',
            (byte)'B',
            (byte)'X',
            4, 0,
            (byte)'B',
            (byte)'U',
            (byte)'C',
            (byte)'R' }
            .Concat(BitConverter.GetBytes(0x03999000))
            .Concat(BitConverter.GetBytes(69))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r);

        // Act
        var header = parser.Parse(out var node);

        // Assert
        await Assert.That(header.Basic.Version).IsEqualTo((ushort)4);
        await Assert.That(header.Basic.Format).IsEqualTo(GbxFormat.Binary);
        await Assert.That(header.Basic.CompressionOfRefTable).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(header.Basic.CompressionOfBody).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(header.Basic.Mode).IsEqualTo(GbxMode.Release);
        await Assert.That(header.ClassId).IsEqualTo((uint)0x03999000);
        await Assert.That(header).IsTypeOf<GbxHeaderUnknown>();
        await Assert.That(((GbxHeaderUnknown)header).UserData).IsEmpty();
        await Assert.That((object?)node).IsNull();
        await Assert.That(header.NumNodes).IsEqualTo(69);
    }
}
