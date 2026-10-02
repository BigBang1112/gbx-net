using GBX.NET.Components;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Components;

public class GbxHeaderBasicTests
{
    [Test]
    public async Task Constructor_Parameters()
    {
        // Arrange & Act
        var basic = new GbxHeaderBasic(6, GbxFormat.Binary, GbxCompression.Uncompressed, GbxCompression.Compressed, GbxUnknownByte.R);
        
        // Assert
        await Assert.That(basic.Version).IsEqualTo((ushort)6);
        await Assert.That(basic.Format).IsEqualTo(GbxFormat.Binary);
        await Assert.That(basic.CompressionOfRefTable).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(basic.CompressionOfBody).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(basic.UnknownByte).IsEqualTo(GbxUnknownByte.R);
    }

    [Test]
    public async Task Create_UsesDefaults()
    {
        // Arrange & Act
        var basic = GbxHeaderBasic.Create();

        // Assert
        await Assert.That(basic.Version).IsEqualTo((ushort)6);
        await Assert.That(basic.Format).IsEqualTo(GbxFormat.Binary);
        await Assert.That(basic.CompressionOfRefTable).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(basic.CompressionOfBody).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(basic.UnknownByte).IsEqualTo(GbxUnknownByte.R);
    }

    [Test]
    public async Task Parse_Stream_ParsesCorrectly()
    {
        // Arrange
        using var ms = new MemoryStream([(byte)'G', (byte)'B', (byte)'X', 4, 0, (byte)'T', (byte)'C', (byte)'U', (byte)'E', 69]);

        // Act
        var basic = GbxHeaderBasic.Parse(ms);

        // Assert
        await Assert.That(basic.Version).IsEqualTo((ushort)4);
        await Assert.That(basic.Format).IsEqualTo(GbxFormat.Text);
        await Assert.That(basic.CompressionOfRefTable).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(basic.CompressionOfBody).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(basic.UnknownByte).IsEqualTo(GbxUnknownByte.E);
        await Assert.That(ms.Position).IsEqualTo(9);
    }

    [Test]
    public async Task Parse_GbxReader_V4_ParsesCorrectly()
    {
        // Arrange
        using var ms = new MemoryStream([(byte)'G', (byte)'B', (byte)'X', 4, 0, (byte)'T', (byte)'C', (byte)'U', (byte)'E', 69]);
        using var reader = new GbxReader(ms);

        // Act
        var basic = GbxHeaderBasic.Parse(reader);

        // Assert
        await Assert.That(basic.Version).IsEqualTo((ushort)4);
        await Assert.That(basic.Format).IsEqualTo(GbxFormat.Text);
        await Assert.That(basic.CompressionOfRefTable).IsEqualTo(GbxCompression.Compressed);
        await Assert.That(basic.CompressionOfBody).IsEqualTo(GbxCompression.Uncompressed);
        await Assert.That(basic.UnknownByte).IsEqualTo(GbxUnknownByte.E);
    }

    [Test]
    public void Parse_GbxReader_WrongGbxMagic_Throws()
    {
        // Arrange
        using var ms = new MemoryStream([(byte)'G', (byte)'B', 4, 0, (byte)'T', (byte)'C', (byte)'U', 69]);
        using var reader = new GbxReader(ms);

        // Act & Assert
        Assert.Throws<NotAGbxException>(() => GbxHeaderBasic.Parse(reader));
    }

    [Test]
    public async Task Write_Successful()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var writer = new GbxWriter(ms);

        var basic = GbxHeaderBasic.Create(4, GbxFormat.Text, GbxCompression.Compressed, GbxCompression.Uncompressed, GbxUnknownByte.E);

        // Act
        var result = basic.Write(writer);

        // Assert
        await Assert.That(ms.ToArray()).IsEquivalentTo((byte[])[(byte)'G', (byte)'B', (byte)'X', 4, 0, (byte)'T', (byte)'C', (byte)'U', (byte)'E'], CollectionOrdering.Matching);
        await Assert.That(result).IsTrue();
    }
}
