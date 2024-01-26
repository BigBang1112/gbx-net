using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

public class GbxHeaderReaderTests
{
    [Fact]
    public void ValidateUserDataNumbers_ZeroLength_ReturnsZeros()
    {
        // Arrange
        using var ms = new MemoryStream([0, 0, 0, 0, 69]);
        using var r = new GbxReader(ms);

        var settings = new GbxReadSettings(); // Assuming default settings
        var parser = new GbxHeaderReader(r, settings);

        // Act
        var result = parser.ValidateUserDataNumbers();

        // Assert
        Assert.Equal(0, result.Length);
        Assert.Equal(0, result.NumChunks);
        Assert.Equal(4, ms.Position);
    }

    [Fact]
    public void ValidateUserDataNumbers_ExceedsMaxSize_Throws()
    {
        // Arrange
        using var ms = new MemoryStream(
            BitConverter.GetBytes(GbxReader.MaxDataSize + 1)
                .Append((byte)69)
                .ToArray());
        using var r = new GbxReader(ms);

        var settings = new GbxReadSettings();
        var parser = new GbxHeaderReader(r, settings);

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => parser.ValidateUserDataNumbers());
        Assert.Equal(4, ms.Position);
    }

    [Fact]
    public void ValidateUserDataNumbers_NormalCase_ReturnsCorrectNumbers()
    {
        // Arrange
        var expectedLength = 100; // Example length
        var expectedNumChunks = 5; // Example number of chunks

        using var ms = new MemoryStream(
            BitConverter.GetBytes(expectedLength)
                .Concat(BitConverter.GetBytes(expectedNumChunks))
                .Append((byte)69)
                .ToArray());
        using var r = new GbxReader(ms);

        var settings = new GbxReadSettings();
        var parser = new GbxHeaderReader(r, settings);

        // Act
        var result = parser.ValidateUserDataNumbers();

        // Assert
        Assert.Equal(expectedLength, result.Length);
        Assert.Equal(expectedNumChunks, result.NumChunks);
        Assert.Equal(8, ms.Position);
    }

    [Fact]
    public void ValidateUserDataNumbers_SkipUserData_ReturnsUserDataInfoWithZeroChunks()
    {
        // Arrange
        var expectedLength = 100; // Example length

        using var ms = new MemoryStream(
            BitConverter.GetBytes(expectedLength)
                .Concat(new byte[expectedLength])
                .Append((byte)69)
                .ToArray());
        using var r = new GbxReader(ms);

        var settings = new GbxReadSettings { SkipUserData = true };
        var parser = new GbxHeaderReader(r, settings);

        // Act
        var result = parser.ValidateUserDataNumbers();

        // Assert
        Assert.Equal(expectedLength, result.Length);
        Assert.Equal(0, result.NumChunks);
        Assert.Equal(104, ms.Position);
    }

    [Fact]
    public void FillHeaderChunkInfo_CorrectlyFillsChunkInfo()
    {
        // Arrange
        var numChunks = 3;
        var userDataInfo = new UserDataNumbers(Length: 963, numChunks);

        using var ms = new MemoryStream(
                    BitConverter.GetBytes(0x03043003)
            .Concat(BitConverter.GetBytes(238))
            .Concat(BitConverter.GetBytes(0x03043004))
            .Concat(BitConverter.GetBytes(4))
            .Concat(BitConverter.GetBytes(0x03043005))
            .Concat(BitConverter.GetBytes(693 + 0x80000000))
                .Append((byte)69)
                .ToArray());
        using var reader = new GbxReader(ms);

        Span<HeaderChunkInfo> headerChunkDescs = stackalloc HeaderChunkInfo[numChunks];

        // Act
        GbxHeaderReader.FillHeaderChunkInfo(headerChunkDescs, reader, userDataInfo);

        // Assert
        Assert.Equal(expected: (uint)0x03043003, actual: headerChunkDescs[0].Id);
        Assert.Equal(expected: 238, actual: headerChunkDescs[0].Size);
        Assert.False(headerChunkDescs[0].IsHeavy);

        Assert.Equal(expected: (uint)0x03043004, actual: headerChunkDescs[1].Id);
        Assert.Equal(expected: 4, actual: headerChunkDescs[1].Size);
        Assert.False(headerChunkDescs[1].IsHeavy);

        Assert.Equal(expected: (uint)0x03043005, actual: headerChunkDescs[2].Id);
        Assert.Equal(expected: 693, actual: headerChunkDescs[2].Size);
        Assert.True(headerChunkDescs[2].IsHeavy);

        Assert.Equal(expected: 24, actual: ms.Position);
    }

    [Fact]
    public void FillHeaderChunkInfo_ExceedsMaxSize_Throws()
    {
        // Arrange
        var numChunks = 1;
        var userDataInfo = new UserDataNumbers(Length: 128, numChunks);

        using var ms = new MemoryStream(
                    BitConverter.GetBytes(0x03043003)
            .Concat(BitConverter.GetBytes(GbxReader.MaxDataSize + 1))
                .Append((byte)69)
                .ToArray());
        using var reader = new GbxReader(ms);

        var headerChunkDescs = new HeaderChunkInfo[numChunks];

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => GbxHeaderReader.FillHeaderChunkInfo(headerChunkDescs, reader, userDataInfo));
        Assert.Equal(expected: 8, actual: ms.Position);
    }

    [Theory]
    [InlineData(420, 24)]
    [InlineData(210, 8)]
    public void FillHeaderChunkInfo_TotalSizeMismatch_Throws(int length, int streamPos)
    {
        // Arrange
        var numChunks = 3;
        var userDataInfo = new UserDataNumbers(length, numChunks);

        using var ms = new MemoryStream(
                    BitConverter.GetBytes(0x03043003)
            .Concat(BitConverter.GetBytes(238))
            .Concat(BitConverter.GetBytes(0x03043004))
            .Concat(BitConverter.GetBytes(4))
            .Concat(BitConverter.GetBytes(0x03043005))
            .Concat(BitConverter.GetBytes(693 + 0x80000000))
                .Append((byte)69)
                .ToArray());
        using var reader = new GbxReader(ms);

        var headerChunkDescs = new HeaderChunkInfo[numChunks];

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => GbxHeaderReader.FillHeaderChunkInfo(headerChunkDescs, reader, userDataInfo));

        Assert.Equal(expected: streamPos, actual: ms.Position);
    }

    [Fact]
    public void ReadUserData_EmptyUserData_ReturnsFalse()
    {
        // Arrange
        using var ms = new MemoryStream(BitConverter.GetBytes(0).ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r, new());

        // Act
        var result = parser.ReadUserData(node: null, unknownHeader: null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ReadUserData_UserDataWithZeroHeaderChunks_ReturnsFalse()
    {
        // Arrange
        using var ms = new MemoryStream(
                    BitConverter.GetBytes(4)
            .Concat(BitConverter.GetBytes(0))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r, new());

        // Act
        var result = parser.ReadUserData(node: null, unknownHeader: null);

        // Assert
        Assert.False(result);
    }

    [Fact]
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

        var parser = new GbxHeaderReader(r, new());

        // Act & Assert
        Assert.Throws<Exception>(() => parser.ReadUserData(node: null, unknownHeader: null));
    }

    [Fact]
    public void ReadUserData_UnknownNode_ReadsAndAddsUnknownHeaderChunk()
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

        var parser = new GbxHeaderReader(r, new());

        var unknownHeader = new GbxHeaderUnknown(GbxHeaderBasic.Default, 0x03043000);

        // Act
        var result = parser.ReadUserData(node: null, unknownHeader);

        // Assert
        Assert.True(result);
        Assert.Single(unknownHeader.UserData);
        Assert.False(unknownHeader.UserData.First().IsHeavy, "Header chunk is heavy but should not be.");
        Assert.Equal(expected: (uint)0x03043069, actual: unknownHeader.UserData.First().Id);
        Assert.Equal(expected: [6, 0, 0, 0], actual: unknownHeader.UserData.First().Data);
        Assert.Equal(expected: 20, actual: ms.Position);
    }

    [Fact]
    public void ReadUserData_KnownNode_ReadsAndAddsUnknownHeaderChunk()
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

        var parser = new GbxHeaderReader(r, new());

        var node = new CGameCtnChallenge();

        // Act
        var result = parser.ReadUserData(node, unknownHeader: null);

        // Assert
        Assert.True(result);
        Assert.Single(node.Chunks);
        Assert.IsType<HeaderChunk>(node.Chunks.First());
        Assert.False(((HeaderChunk)node.Chunks.First()).IsHeavy, "Header chunk is heavy but should not be.");
        Assert.Equal(expected: (uint)0x03043069, actual: node.Chunks.First().Id);
        Assert.Equal(expected: [6, 0, 0, 0], actual: ((HeaderChunk)node.Chunks.First()).Data);
        Assert.Equal(expected: 20, actual: ms.Position);
    }

    [Fact]
    public void ReadUserData_KnownNode_CreatesAndReadsKnownHeaderChunk()
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

        var parser = new GbxHeaderReader(r, new());

        var node = new CGameCtnChallenge();

        // Act
        var result = parser.ReadUserData(node, unknownHeader: null);

        // Assert
        Assert.True(result);
        Assert.Single(node.Chunks);
        Assert.IsType<CGameCtnChallenge.HeaderChunk03043004>(node.Chunks.First());
        Assert.False(((CGameCtnChallenge.HeaderChunk03043004)node.Chunks.First()).IsHeavy, "Header chunk is heavy but should not be.");
        Assert.Equal(expected: 6, actual: ((CGameCtnChallenge.HeaderChunk03043004)node.Chunks.First()).Version);
        Assert.Equal(expected: 20, actual: ms.Position);
    }

    [Fact]
    public void ReadUserData_KnownNode_ChunkLarger_CreatesAndReadsKnownHeaderChunkAndSkipsData()
    {
        // Arrange
        using var ms = new MemoryStream(
                    BitConverter.GetBytes(20)
            .Concat(BitConverter.GetBytes(1))
            .Concat(BitConverter.GetBytes(0x03043004))
            .Concat(BitConverter.GetBytes(8))
            .Concat(BitConverter.GetBytes(6))
            .Concat(BitConverter.GetBytes(69))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r, new());

        var node = new CGameCtnChallenge();

        // Act
        var result = parser.ReadUserData(node, unknownHeader: null);

        // Assert
        Assert.True(result);
        Assert.Single(node.Chunks);
        Assert.IsType<CGameCtnChallenge.HeaderChunk03043004>(node.Chunks.First());
        Assert.False(((CGameCtnChallenge.HeaderChunk03043004)node.Chunks.First()).IsHeavy, "Header chunk is heavy but should not be.");
        Assert.Equal(expected: 6, actual: ((CGameCtnChallenge.HeaderChunk03043004)node.Chunks.First()).Version);
        Assert.Equal(expected: 24, actual: ms.Position);
    }

    [Fact]
    public void ReadUserDataOfT_EmptyUserData_ReturnsFalse()
    {
        // Arrange
        using var ms = new MemoryStream(BitConverter.GetBytes(0).ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r, new());

        // Act
        var result = parser.ReadUserData<CGameCtnChallenge>(new());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ReadUserDataOfT_UserDataWithZeroHeaderChunks_ReturnsFalse()
    {
        // Arrange
        using var ms = new MemoryStream(
                    BitConverter.GetBytes(4)
            .Concat(BitConverter.GetBytes(0))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r, new());

        // Act
        var result = parser.ReadUserData<CGameCtnChallenge>(new());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ReadUserDataOfT_ReadsAndAddsUnknownHeaderChunk()
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

        var parser = new GbxHeaderReader(r, new());

        var node = new CGameCtnChallenge();

        // Act
        var result = parser.ReadUserData(node);

        // Assert
        Assert.True(result);
        Assert.Single(node.Chunks);
        Assert.False(((HeaderChunk)node.Chunks.First()).IsHeavy, "Header chunk is heavy but should not be.");
        Assert.Equal(expected: (uint)0x03043069, actual: node.Chunks.First().Id);
        Assert.Equal(expected: [6, 0, 0, 0], actual: ((HeaderChunk)node.Chunks.First()).Data);
        Assert.Equal(expected: 20, actual: ms.Position);
    }

    [Fact]
    public void ReadUserDataOfT_CreatesAndReadsKnownHeaderChunk()
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

        var parser = new GbxHeaderReader(r, new());

        var node = new CGameCtnChallenge();

        // Act
        var result = parser.ReadUserData(node);

        // Assert
        Assert.True(result);
        Assert.Single(node.Chunks);
        Assert.IsType<CGameCtnChallenge.HeaderChunk03043004>(node.Chunks.First());
        Assert.False(((CGameCtnChallenge.HeaderChunk03043004)node.Chunks.First()).IsHeavy, "Header chunk is heavy but should not be.");
        Assert.Equal(expected: 6, actual: ((CGameCtnChallenge.HeaderChunk03043004)node.Chunks.First()).Version);
        Assert.Equal(expected: 20, actual: ms.Position);
    }

    [Fact]
    public void ReadUserDataOfT_ChunkLarger_CreatesAndReadsKnownHeaderChunkAndSkipsData()
    {
        // Arrange
        using var ms = new MemoryStream(
                    BitConverter.GetBytes(20)
            .Concat(BitConverter.GetBytes(1))
            .Concat(BitConverter.GetBytes(0x03043004))
            .Concat(BitConverter.GetBytes(8))
            .Concat(BitConverter.GetBytes(6))
            .Concat(BitConverter.GetBytes(69))
                .ToArray());
        using var r = new GbxReader(ms);

        var parser = new GbxHeaderReader(r, new());

        var node = new CGameCtnChallenge();

        // Act
        var result = parser.ReadUserData(node);

        // Assert
        Assert.True(result);
        Assert.Single(node.Chunks);
        Assert.IsType<CGameCtnChallenge.HeaderChunk03043004>(node.Chunks.First());
        Assert.False(((CGameCtnChallenge.HeaderChunk03043004)node.Chunks.First()).IsHeavy, "Header chunk is heavy but should not be.");
        Assert.Equal(expected: 6, actual: ((CGameCtnChallenge.HeaderChunk03043004)node.Chunks.First()).Version);
        Assert.Equal(expected: 24, actual: ms.Position);
    }
}
