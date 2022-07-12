using System.IO;
using Xunit;

namespace GBX.NET.Tests.Unit;

public class GbxDecompressorTests
{
    [Fact]
    public void DecompressDecompressed_DataIsEqual()
    {
        // Arrange
        var expectedDecompressed = File.ReadAllBytes("TestData/GbxDecompressor/BayDecompressed.TMCollection.Gbx");
        using var expectedDecompressedStream = new MemoryStream(expectedDecompressed);
        using var actualDecompressedStream = new MemoryStream();

        // Act
        GbxCompressor.Decompress(expectedDecompressedStream, actualDecompressedStream);

        // Assert
        Assert.Equal(expectedDecompressedStream.ToArray(), actualDecompressedStream.ToArray());
    }
}
