using GBX.NET.Components;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Components;

public class GbxHeaderBasicTests
{
    [Fact]
    public void Constructor_Parameters()
    {
        // Arrange & Act
        var basic = new GbxHeaderBasic(6, GbxFormat.Binary, GbxCompression.Uncompressed, GbxCompression.Compressed, GbxUnknownByte.R);
        
        // Assert
        Assert.Equal(expected: 6, actual: basic.Version);
        Assert.Equal(expected: GbxFormat.Binary, actual: basic.Format);
        Assert.Equal(expected: GbxCompression.Uncompressed, actual: basic.CompressionOfRefTable);
        Assert.Equal(expected: GbxCompression.Compressed, actual: basic.CompressionOfBody);
        Assert.Equal(expected: GbxUnknownByte.R, actual: basic.UnknownByte);
    }

    [Fact]
    public void Create_UsesDefaults()
    {
        // Arrange & Act
        var basic = GbxHeaderBasic.Create();

        // Assert
        Assert.Equal(expected: 6, actual: basic.Version);
        Assert.Equal(expected: GbxFormat.Binary, actual: basic.Format);
        Assert.Equal(expected: GbxCompression.Uncompressed, actual: basic.CompressionOfRefTable);
        Assert.Equal(expected: GbxCompression.Compressed, actual: basic.CompressionOfBody);
        Assert.Equal(expected: GbxUnknownByte.R, actual: basic.UnknownByte);
    }

    [Fact]
    public void Parse_Stream_ParsesCorrectly()
    {
        // Arrange
        using var ms = new MemoryStream([(byte)'G', (byte)'B', (byte)'X', 4, 0, (byte)'T', (byte)'C', (byte)'U', (byte)'E', 69]);

        // Act
        var basic = GbxHeaderBasic.Parse(ms);

        // Assert
        Assert.Equal(expected: 4, actual: basic.Version);
        Assert.Equal(expected: GbxFormat.Text, actual: basic.Format);
        Assert.Equal(expected: GbxCompression.Compressed, actual: basic.CompressionOfRefTable);
        Assert.Equal(expected: GbxCompression.Uncompressed, actual: basic.CompressionOfBody);
        Assert.Equal(expected: GbxUnknownByte.E, actual: basic.UnknownByte);
        Assert.Equal(expected: 9, actual: ms.Position);
    }

    [Fact]
    public void Parse_GbxReader_V4_ParsesCorrectly()
    {
        // Arrange
        using var ms = new MemoryStream([(byte)'G', (byte)'B', (byte)'X', 4, 0, (byte)'T', (byte)'C', (byte)'U', (byte)'E', 69]);
        using var reader = new GbxReader(ms);

        // Act
        var basic = GbxHeaderBasic.Parse(reader);

        // Assert
        Assert.Equal(expected: 4, actual: basic.Version);
        Assert.Equal(expected: GbxFormat.Text, actual: basic.Format);
        Assert.Equal(expected: GbxCompression.Compressed, actual: basic.CompressionOfRefTable);
        Assert.Equal(expected: GbxCompression.Uncompressed, actual: basic.CompressionOfBody);
        Assert.Equal(expected: GbxUnknownByte.E, actual: basic.UnknownByte);
    }

    [Fact]
    public void Parse_GbxReader_WrongGbxMagic_Throws()
    {
        // Arrange
        using var ms = new MemoryStream([(byte)'G', (byte)'B', 4, 0, (byte)'T', (byte)'C', (byte)'U', 69]);
        using var reader = new GbxReader(ms);

        // Act & Assert
        Assert.Throws<NotAGbxException>(() => GbxHeaderBasic.Parse(reader));
    }

    [Fact]
    public void Write_Successful()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var writer = new GbxWriter(ms);

        var basic = GbxHeaderBasic.Create(4, GbxFormat.Text, GbxCompression.Compressed, GbxCompression.Uncompressed, GbxUnknownByte.E);

        // Act
        var result = basic.Write(writer);

        // Assert
        Assert.Equal(expected: [(byte)'G', (byte)'B', (byte)'X', 4, 0, (byte)'T', (byte)'C', (byte)'U', (byte)'E'], actual: ms.ToArray());
        Assert.True(result);
    }
}
