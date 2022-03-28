using GBX.NET.Attributes;
using Xunit;

namespace GBX.NET.Tests.Unit.Attributes;

public class ChunkAttributeTests
{
    // test GetClassPart, should truncate last 3 digits
    [Fact]
    public void GetClassPart_ShouldTruncateLast3Digits()
    {
        // arrange
        var chunkAttribute = new ChunkAttribute(0x0304301F);

        // act
        var result = chunkAttribute.GetClassPart();

        // assert
        Assert.Equal<uint>(expected: 0x03043000, actual: result);
    }

    // test GetChunkPart
    [Fact]
    public void GetChunkPart_ShouldReturnLast3Digits()
    {
        // arrange
        var chunkAttribute = new ChunkAttribute(0x0304301F);

        // act
        var result = chunkAttribute.GetChunkPart();

        // assert
        Assert.Equal<uint>(expected: 0x01F, actual: result);
    }
}
