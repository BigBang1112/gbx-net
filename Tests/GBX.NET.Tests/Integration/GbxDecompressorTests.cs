using System.IO;
using Xunit;

namespace GBX.NET.Tests.Integration;

public class GbxDecompressorTests
{
    [Fact]
    public void CompressDecompressed_ThenDecompress_DataIsEqual()
    {
        // Arrange
        var expectedDecompressed = File.ReadAllBytes("TestData/GbxDecompressor/BayDecompressed.TMCollection.Gbx");
        using var expectedDecompressedStream = new MemoryStream(expectedDecompressed);
        using var actualCompressedStream = new MemoryStream();
        using var actualDecompressedStreamAfterwards = new MemoryStream();

        // Act
        GbxCompressor.Compress(expectedDecompressedStream, actualCompressedStream);
        actualCompressedStream.Seek(0, SeekOrigin.Begin);
        GbxCompressor.Decompress(actualCompressedStream, actualDecompressedStreamAfterwards);

        // Assert
        Assert.Equal(expectedDecompressed, actualDecompressedStreamAfterwards.ToArray());
    }
}
