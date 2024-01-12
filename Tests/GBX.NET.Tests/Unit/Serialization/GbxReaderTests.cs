﻿using GBX.NET.Exceptions;
using GBX.NET.Serialization;
using GBX.NET.Tests.Mocks;

namespace GBX.NET.Tests.Unit.Serialization;

public class GbxReaderTests
{
    [Fact]
    public void Contructor_Input_BaseStreamIsInput()
    {
        // Arrange
        using var ms = new MemoryStream();

        // Act
        using var r = new GbxReader(ms);

        // Assert
        Assert.Same(expected: ms, actual: r.BaseStream);
    }

    [Fact]
    public void Contructor_InputLeaveOpen_BaseStreamIsInput()
    {
        // Arrange
        using var ms = new MemoryStream();

        // Act
        using var r = new GbxReader(ms, leaveOpen: true);

        // Assert
        Assert.Same(expected: ms, actual: r.BaseStream);
    }

    [Fact]
    public void ReadGbxMagic_HasCorrectMagic()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([(byte)'G', (byte)'B', (byte)'X', 69]);
        ms.Position = 0;

        // Act
        var result = r.ReadGbxMagic();

        // Assert
        Assert.True(result, "GBX magic is invalid.");
        Assert.Equal(expected: 3, actual: ms.Position);
    }

    [Fact]
    public void ReadGbxMagic_HasIncorrectMagic()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([(byte)'G', (byte)'X', (byte)'Z', 69]);
        ms.Position = 0;

        // Act
        var result = r.ReadGbxMagic();

        // Assert
        Assert.False(result, "GBX magic is valid but it shouldn't be.");
        Assert.Equal(expected: 2, actual: ms.Position);
    }

    [Fact]
    public void ReadGbxMagic_MissingData_ThrowsEndOfStream()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([(byte)'G', (byte)'B']);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<EndOfStreamException>(() => r.ReadGbxMagic());
    }

    [Fact]
    public void ReadBoolean_Int32_IsCleanTrue()
    {
        Gbx.StrictBooleans = true;

        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([1, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadBoolean();

        // Assert
        Assert.True(value);
        Assert.Equal(expected: 4, actual: ms.Position);
    }

    [Fact]
    public void ReadBoolean_Int32_IsCleanFalse()
    {
        Gbx.StrictBooleans = true;

        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([0, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadBoolean();

        // Assert
        Assert.False(value);
        Assert.Equal(expected: 4, actual: ms.Position);
    }

    [Fact]
    public void ReadBoolean_Int32_IsDirtyThrows()
    {
        Gbx.StrictBooleans = true;

        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([2, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<BooleanOutOfRangeException>(() => r.ReadBoolean());
    }

    [Fact]
    public void ReadBoolean_Int32_AsByteFalse()
    {
        Gbx.StrictBooleans = true;

        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([0, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadBoolean(asByte: false);

        // Assert
        Assert.False(value);
        Assert.Equal(expected: 4, actual: ms.Position);
    }

    [Fact]
    public void ReadBoolean_Byte_IsCleanTrue()
    {
        Gbx.StrictBooleans = true;

        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.WriteByte(1);
        ms.WriteByte(69);
        ms.Position = 0;

        // Act
        var value = r.ReadBoolean(asByte: true);

        // Assert
        Assert.True(value);
        Assert.Equal(expected: 1, actual: ms.Position);
    }

    [Fact]
    public void ReadBoolean_Byte_IsCleanFalse()
    {
        Gbx.StrictBooleans = true;

        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.WriteByte(0);
        ms.WriteByte(69);
        ms.Position = 0;

        // Act
        var value = r.ReadBoolean(asByte: true);

        // Assert
        Assert.False(value);
        Assert.Equal(expected: 1, actual: ms.Position);
    }

    [Fact]
    public void ReadBoolean_Byte_IsDirtyThrows()
    {
        Gbx.StrictBooleans = true;

        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.WriteByte(2);
        ms.WriteByte(69);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<BooleanOutOfRangeException>(() => r.ReadBoolean(asByte: true));
    }

    [Fact]
    public void ReadString_Int32()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([4, 0, 0, 0, (byte)'T', (byte)'e', (byte)'s', (byte)'t', 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadString();

        // Assert
        Assert.Equal(expected: "Test", actual: value);
        Assert.Equal(expected: 8, actual: ms.Position);
    }

    [Fact]
    public void ReadString_Int32_Empty()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([0, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadString();

        // Assert
        Assert.Equal(expected: string.Empty, actual: value);
        Assert.Equal(expected: 4, actual: ms.Position);
    }

    [Fact]
    public void ReadString_Int32_NegativeLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([255, 255, 255, 255, 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => r.ReadString());
    }

    [Fact]
    public void ReadString_Int32_TooLong_ThrowsLengthLimitException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([1, 0, 0, 16, 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => r.ReadString());
    }

    [Fact]
    public void ReadString_StringLengthPrefix_Byte()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([4, (byte)'T', (byte)'e', (byte)'s', (byte)'t', 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadString(StringLengthPrefix.Byte);

        // Assert
        Assert.Equal(expected: "Test", actual: value);
        Assert.Equal(expected: 5, actual: ms.Position);
    }

    [Fact]
    public void ReadString_StringLengthPrefix_Int32()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([4, 0, 0, 0, (byte)'T', (byte)'e', (byte)'s', (byte)'t', 69]);
        ms.Position = 0;

        // Act
        var value = r.ReadString(StringLengthPrefix.Int32);

        // Assert
        Assert.Equal(expected: "Test", actual: value);
        Assert.Equal(expected: 8, actual: ms.Position);
    }

    [Fact]
    public void ReadString_StringLengthPrefix_Unknown_ThrowsArgumentException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([4, 0, 0, 0, (byte)'T', (byte)'e', (byte)'s', (byte)'t', 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => r.ReadString((StringLengthPrefix)69));
    }

    [Fact]
    public void ReadString_StringLengthPrefix_NegativeLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([255, 255, 255, 255, 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => r.ReadString(StringLengthPrefix.Int32));
    }

    [Fact]
    public void ReadString_StringLengthPrefix_TooLong_ThrowsLengthLimitException()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([1, 0, 0, 16, 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => r.ReadString(StringLengthPrefix.Int32));
    }

    [Fact]
    public void ReadData_NoCount_TooLong_Throws()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([1, 0, 0, 16, 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => r.ReadData());
    }

    [Fact]
    public void ReadBytes_WithCount_TooLong_ThrowsWithLength()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => r.ReadBytes(GbxReader.MaxDataSize + 1));
    }

    [Fact]
    public void ReadData_NoCount_ReadsBytes()
    {
        // Arrange
        using var ms = new MemoryStream([3, 0, 0, 0, 1, 2, 3, 69]);
        using var r = new GbxReader(ms);

        // Act
        var bytes = r.ReadData();

        // Assert
        Assert.Equal(expected: [1, 2, 3], actual: bytes);
    }

    [Fact]
    public void ReadBytes_WithCount_ReadsBytes()
    {
        // Arrange
        using var ms = new MemoryStream([1, 2, 3, 69]);
        using var r = new GbxReader(ms);

        // Act
        var bytes = r.ReadBytes(3);

        // Assert
        Assert.Equal(expected: [1, 2, 3], actual: bytes);
    }

    [Fact]
    public void ReadId_VersionNotSupported_Throws()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([2, 0, 0, 0, 0, 0, 0, 64, 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => r.ReadId());
        Assert.Equal(expected: 4, actual: ms.Position);
    }

    [Fact]
    public void ReadId_HasIdString_ReturnsIdWithString()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3, 0, 0, 0, 0, 0, 0, 64, 3, 0, 0, 0, (byte)'H', (byte)'i', (byte)'!', 69]);
        ms.Position = 0;

        // Act
        var id = r.ReadId();

        // Assert
        Assert.Equal(expected: 3, actual: r.IdVersion);
        Assert.Equal(expected: "Hi!", actual: id.String);
        Assert.Null(id.Index);
        Assert.NotNull(r.IdDict);
        Assert.Single(r.IdDict);
        Assert.True(r.IdDict.TryGetValue(0x40000001, out var val) && val == "Hi!",
            @"IdList does not contain (0x40000001, ""Hi!"")");
        Assert.Equal(expected: 15, actual: ms.Position);
    }

    [Fact]
    public void ReadId_HasIdCollection_ReturnsIdWithCorrectNumber()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3, 0, 0, 0, 4, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act
        var id = r.ReadId();

        // Assert
        Assert.Equal(expected: 3, actual: r.IdVersion);
        Assert.Equal(expected: 4, actual: id.Index);
        Assert.Null(id.String);
        Assert.NotNull(r.IdDict);
        Assert.Equal(expected: 8, actual: ms.Position);
    }

    [Fact]
    public void ReadId_HasReusedIdString_ReturnsSameIdWithString()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3, 0, 0, 0, 0, 0, 0, 64, 3, 0, 0, 0, (byte)'H', (byte)'i', (byte)'!', 1, 0, 0, 64, 69]);
        ms.Position = 0;

        var id = r.ReadId();

        // Act
        var reusedId = r.ReadId();

        // Assert
        Assert.Equal(expected: 3, actual: r.IdVersion);
        Assert.Equal(expected: id, actual: reusedId);
        Assert.Null(reusedId.Index);
        Assert.NotNull(r.IdDict);
        Assert.Single(r.IdDict);
        Assert.True(r.IdDict.TryGetValue(0x40000001, out var val) && val == "Hi!",
            @"IdList does not contain (0x40000001, ""Hi!"")");
        Assert.Equal(expected: 19, actual: ms.Position);
    }

    [Fact]
    public void ReadId_Has2IdStrings_ReturnsIdsWithString()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([
            3,
            0,
            0,
            0,
            0,
            0,
            0,
            64,
            3,
            0,
            0,
            0,
            (byte)'H',
            (byte)'i',
            (byte)'!',
            0,
            0,
            0,
            64,
            2,
            0,
            0,
            0,
            (byte)'T',
            (byte)'M',
            69
        ]);
        ms.Position = 0;

        // Act
        var id = r.ReadId();
        var anotherId = r.ReadId();

        // Assert
        Assert.Equal(expected: 3, actual: r.IdVersion);
        Assert.Equal(expected: "Hi!", actual: id.String);
        Assert.Equal(expected: "TM", actual: anotherId.String);
        Assert.Null(id.Index);
        Assert.Null(anotherId.Index);
        Assert.NotNull(r.IdDict);
        Assert.Equal(expected: 2, actual: r.IdDict.Count);
        Assert.True(r.IdDict.TryGetValue(0x40000001, out var val) && val == "Hi!",
            @"IdList does not contain (0x40000001, ""Hi!"")");
        Assert.True(r.IdDict.TryGetValue(0x40000002, out var val2) && val2 == "TM",
            @"IdList does not contain (0x40000002, ""TM"")");
        Assert.Equal(expected: 25, actual: ms.Position);
    }

    [Fact]
    public void ReadIdAsString_HasIdString_ReturnsIdWithString()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3, 0, 0, 0, 0, 0, 0, 64, 3, 0, 0, 0, (byte)'H', (byte)'i', (byte)'!', 69]);
        ms.Position = 0;

        // Act
        var str = r.ReadIdAsString();

        // Assert
        Assert.Equal(expected: 3, actual: r.IdVersion);
        Assert.Equal(expected: "Hi!", actual: str);
        Assert.NotNull(r.IdDict);
        Assert.Single(r.IdDict);
        Assert.True(r.IdDict.TryGetValue(0x40000001, out var val) && val == "Hi!",
            @"IdList does not contain (0x40000001, ""Hi!"")");
        Assert.Equal(expected: 15, actual: ms.Position);
    }

    [Fact]
    public void ReadIdAsString_HasIdCollection_Throws()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3, 0, 0, 0, 4, 0, 0, 0, 69]);
        ms.Position = 0;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => r.ReadIdAsString());
        Assert.Equal(expected: 3, actual: r.IdVersion);
        Assert.NotNull(r.IdDict);
        Assert.Equal(expected: 8, actual: ms.Position);
    }

    [Fact]
    public void ReadIdAsString_HasReusedIdString_ReturnsSameIdWithString()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3, 0, 0, 0, 0, 0, 0, 64, 3, 0, 0, 0, (byte)'H', (byte)'i', (byte)'!', 1, 0, 0, 64, 69]);
        ms.Position = 0;

        var id = r.ReadIdAsString();

        // Act
        var reusedId = r.ReadIdAsString();

        // Assert
        Assert.Equal(expected: 3, actual: r.IdVersion);
        Assert.Equal(expected: id, actual: reusedId);
        Assert.NotNull(r.IdDict);
        Assert.Single(r.IdDict);
        Assert.True(r.IdDict.TryGetValue(0x40000001, out var val) && val == "Hi!",
            @"IdList does not contain (0x40000001, ""Hi!"")");
        Assert.Equal(expected: 19, actual: ms.Position);
    }

    [Fact]
    public void ReadIdent_Reads3Ids()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        ms.Write([3,
            0,
            0,
            0,
            0,
            0,
            0,
            64,
            3,
            0,
            0,
            0,
            (byte)'H',
            (byte)'i',
            (byte)'!',
            4,
            0,
            0,
            0,
            1,
            0,
            0,
            64,
            69]);
        ms.Position = 0;

        // Act
        var ident = r.ReadIdent();

        // Assert
        Assert.Equal(expected: 3, actual: r.IdVersion);
        Assert.Equal(expected: ("Hi!", new(4), "Hi!"), actual: ident);
        Assert.NotNull(r.IdDict);
        Assert.Single(r.IdDict);
        Assert.True(r.IdDict.TryGetValue(0x40000001, out var val) && val == "Hi!",
            @"IdList does not contain (0x40000001, ""Hi!"")");
        Assert.Equal(expected: 23, actual: ms.Position);
    }

    [Fact]
    public void SkipData_WhenBaseStreamCanSeek_ShouldSeekCorrectly()
    {
        // Arrange
        using var ms = new MemoryStream([1, 2, 3, 4, 5, 6, 7]);
        using var r = new GbxReader(ms);

        ms.Position = 2;

        // Act
        r.SkipData(5);

        // Assert
        Assert.Equal(7, ms.Position);
    }

    [Fact]
    public void SkipData_WhenBaseStreamCannotSeek_ShouldSkipCorrectly()
    {
        // Arrange
        using var stream = new NonSeekableStream([1, 2, 3, 4, 5, 6, 7], position: 2);
        using var r = new GbxReader(stream);

        // Act
        r.SkipData(5);

        // Assert
        Assert.Equal(7, stream.Position);
    }

    [Fact]
    public void SkipData_WhenBaseStreamCanSeekAndNotEnoughBytesToRead_Throws()
    {
        // Arrange
        using var ms = new NonSeekableStream([1, 2, 3]);
        using var r = new GbxReader(ms);

        // Act & Assert
        Assert.Throws<EndOfStreamException>(() => r.SkipData(5));
    }

    [Fact]
    public void SkipData_WhenBaseStreamCannotSeekAndNotEnoughBytesToRead_Throws()
    {
        // Arrange
        using var ms = new MemoryStream([1, 2, 3]);
        using var r = new GbxReader(ms);

        // Act & Assert
        Assert.Throws<EndOfStreamException>(() => r.SkipData(5));
    }

    [Fact]
    public void IdVersion_InitiallyNull()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        // Act & Assert
        Assert.Null(r.IdVersion);
    }

    [Fact]
    public void IdVersion_SetAndGetWithoutEncapsulation()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        // Act
        r.IdVersion = 3;
        var actual = r.IdVersion;

        // Assert
        Assert.Equal(expected: 3, actual);
    }

    [Fact]
    public void IdVersion_SetAndGetWithEncapsulation()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);
        using var encapsulation = new Encapsulation(r);

        // Act
        r.IdVersion = 3;
        var actual = r.Encapsulation?.IdVersion;

        // Assert
        Assert.Equal(expected: 3, actual);
    }

    [Fact]
    public void IdDict_InitiallyNotNull()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var r = new GbxReader(ms);

        // Act & Assert
        Assert.NotNull(r.IdDict);
    }
}