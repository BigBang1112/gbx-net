using GBX.NET.Exceptions;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Serialization;

public class GbxWriterTests
{
    [Fact]
    public void Contructor_Input_BaseStreamIsOutput()
    {
        // Arrange
        using var ms = new MemoryStream();

        // Act
        using var w = new GbxWriter(ms);

        // Assert
        Assert.Same(expected: ms, actual: w.BaseStream);
    }

    [Fact]
    public void Contructor_InputLeaveOpen_BaseStreamIsOutput()
    {
        // Arrange
        using var ms = new MemoryStream();

        // Act
        using var w = new GbxWriter(ms);

        // Assert
        Assert.Same(expected: ms, actual: w.BaseStream);
    }

    [Fact]
    public void WriteGbxMagic_WritesCorrect()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.WriteGbxMagic();

        // Assert
        Assert.Equal(expected: 3, actual: ms.Position);

        var data = ms.ToArray();
        Assert.Equal(expected: 'G', actual: (char)data[0]);
        Assert.Equal(expected: 'B', actual: (char)data[1]);
        Assert.Equal(expected: 'X', actual: (char)data[2]);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void WriteBoolean_WritesCorrect(bool value)
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write(value);

        // Assert
        Assert.Equal(expected: 4, actual: ms.Position);

        var data = ms.ToArray();
        Assert.Equal(expected: [value ? (byte)1 : (byte)0, 0, 0, 0], actual: data);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void WriteBooleanAsByteFalse_WritesCorrect(bool value)
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write(value, asByte: false);

        // Assert
        Assert.Equal(expected: 4, actual: ms.Position);

        var data = ms.ToArray();
        Assert.Equal(expected: [value ? (byte)1 : (byte)0, 0, 0, 0], actual: data);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void WriteBooleanAsByteTrue_WritesCorrect(bool value)
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write(value, asByte: true);

        // Assert
        Assert.Equal(expected: 1, actual: ms.Position);

        var data = ms.ToArray();
        Assert.Equal(expected: [value ? (byte)1 : (byte)0], actual: data);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WriteString_BytePrefix_NullOrEmpty_WritesCorrect(string? value)
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write(value, StringLengthPrefix.Byte);

        // Assert
        Assert.Equal(expected: 1, actual: ms.Position);

        var data = ms.ToArray();
        Assert.Equal(expected: [0], actual: data);
    }

    [Fact]
    public void WriteString_BytePrefix_NonEmpty_WritesCorrect()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write("Hi!", StringLengthPrefix.Byte);

        // Assert
        Assert.Equal(expected: 4, actual: ms.Position);

        var data = ms.ToArray();
        Assert.Equal(expected: [3, (byte)'H', (byte)'i', (byte)'!'], actual: data);
    }

    [Fact]
    public void WriteString_BytePrefix_LengthOver255_Throws()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => w.Write(GetRandomString(256), StringLengthPrefix.Byte));
        Assert.Equal(expected: 0, actual: ms.Position);
    }

    [Fact]
    public void WriteString_BytePrefix_ByteLengthOver255_Throws()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => w.Write(GetRandomString(254) + "š", StringLengthPrefix.Byte));
        Assert.Equal(expected: 0, actual: ms.Position);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WriteString_Int32Prefix_NullOrEmpty_WritesCorrect(string? value)
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write(value);

        // Assert
        Assert.Equal(expected: 4, actual: ms.Position);

        var data = ms.ToArray();
        Assert.Equal(expected: [0, 0, 0, 0], actual: data);
    }

    [Fact]
    public void WriteString_Int32Prefix_NonEmpty_WritesCorrect()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write("Hi!", StringLengthPrefix.Int32);

        // Assert
        Assert.Equal(expected: 7, actual: ms.Position);

        var data = ms.ToArray();
        Assert.Equal(expected: [3, 0, 0, 0, (byte)'H', (byte)'i', (byte)'!'], actual: data);
    }

    [Fact]
    public void WriteString_Int32Prefix_LargerString_WritesCorrect()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);
        var str = GetRandomString(128);
        var expected = BitConverter.GetBytes(str.Length).Concat(Encoding.UTF8.GetBytes(str)).ToArray();

        // Act
        w.Write(str, StringLengthPrefix.Int32);

        // Assert
        Assert.Equal(expected: 132, actual: ms.Position);

        var data = ms.ToArray();
        Assert.Equal(expected, actual: data);
    }

    [Fact]
    public void WriteString_Int32Prefix_MuchLargerString_WritesCorrect()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);
        var str = GetRandomString(ushort.MaxValue);
        var expected = BitConverter.GetBytes(str.Length).Concat(Encoding.UTF8.GetBytes(str)).ToArray();

        // Act
        w.Write(str, StringLengthPrefix.Int32);

        // Assert
        Assert.Equal(expected: ushort.MaxValue + 4, actual: ms.Position);

        var data = ms.ToArray();
        Assert.Equal(expected, actual: data);
    }

        [Fact]
    public void WriteArrayOptimizedInt_LengthAtOrOverUInt16Max_WritesEachElementOnce()
    {
        // Arrange: 65535 entries select the int (4-byte) encoding; before the fix the elements were written twice
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);
        var value = Enumerable.Range(0, ushort.MaxValue).ToArray();

        // Act
        w.WriteArrayOptimizedInt(value);

        // Assert: length prefix + one int per element
        Assert.Equal(expected: 4 + value.Length * 4, actual: ms.Position);

        var data = ms.ToArray();
        Assert.Equal(expected: value.Length, actual: BitConverter.ToInt32(data, 0));
        Assert.Equal(expected: 0, actual: BitConverter.ToInt32(data, 4));
        Assert.Equal(expected: 1, actual: BitConverter.ToInt32(data, 8));
        Assert.Equal(expected: value.Length - 1, actual: BitConverter.ToInt32(data, data.Length - 4));
    }

    [Fact]
    public void WriteArrayOptimizedInt_LengthAtOrOverUInt16Max_RoundTrips()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);
        var value = Enumerable.Range(0, 70_000).Select(i => i * 3).ToArray();

        // Act
        w.WriteArrayOptimizedInt(value);
        ms.Position = 0;
        using var r = new GbxReader(ms);
        var read = r.ReadArrayOptimizedInt();

        // Assert: the reader consumes exactly what the writer produced
        Assert.Equal(expected: value, actual: read);
        Assert.Equal(expected: ms.Length, actual: ms.Position);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(1000)]
    public void WriteArrayOptimizedInt_SmallerLengths_WriteEachElementOnce(int length)
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);
        var value = Enumerable.Range(0, length).ToArray();
        var elementSize = length >= byte.MaxValue ? 2 : 1;

        // Act
        w.WriteArrayOptimizedInt(value);

        // Assert
        Assert.Equal(expected: 4 + length * elementSize, actual: ms.Position);
    }
    

    private static string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}
