using GBX.NET.Exceptions;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Serialization;

public class GbxWriterTests
{
    [Test]
    public async Task Contructor_Input_BaseStreamIsOutput()
    {
        // Arrange
        using var ms = new MemoryStream();

        // Act
        using var w = new GbxWriter(ms);

        // Assert
        await Assert.That(w.BaseStream).IsSameReferenceAs(ms);
    }

    [Test]
    public async Task Contructor_InputLeaveOpen_BaseStreamIsOutput()
    {
        // Arrange
        using var ms = new MemoryStream();

        // Act
        using var w = new GbxWriter(ms);

        // Assert
        await Assert.That(w.BaseStream).IsSameReferenceAs(ms);
    }

    [Test]
    public async Task WriteGbxMagic_WritesCorrect()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.WriteGbxMagic();

        // Assert
        await Assert.That(ms.Position).IsEqualTo(3);

        var data = ms.ToArray();
        await Assert.That((char)data[0]).IsEqualTo('G');
        await Assert.That((char)data[1]).IsEqualTo('B');
        await Assert.That((char)data[2]).IsEqualTo('X');
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task WriteBoolean_WritesCorrect(bool value)
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write(value);

        // Assert
        await Assert.That(ms.Position).IsEqualTo(4);

        var data = ms.ToArray();
        await Assert.That(data).IsEquivalentTo((byte[])[value ? (byte)1 : (byte)0, 0, 0, 0], CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task WriteBooleanAsByteFalse_WritesCorrect(bool value)
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write(value, asByte: false);

        // Assert
        await Assert.That(ms.Position).IsEqualTo(4);

        var data = ms.ToArray();
        await Assert.That(data).IsEquivalentTo((byte[])[value ? (byte)1 : (byte)0, 0, 0, 0], CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task WriteBooleanAsByteTrue_WritesCorrect(bool value)
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write(value, asByte: true);

        // Assert
        await Assert.That(ms.Position).IsEqualTo(1);

        var data = ms.ToArray();
        await Assert.That(data).IsEquivalentTo((byte[])[value ? (byte)1 : (byte)0], CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    public async Task WriteString_BytePrefix_NullOrEmpty_WritesCorrect(string? value)
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write(value, StringLengthPrefix.Byte);

        // Assert
        await Assert.That(ms.Position).IsEqualTo(1);

        var data = ms.ToArray();
        await Assert.That(data).IsEquivalentTo((byte[])[0], CollectionOrdering.Matching);
    }

    [Test]
    public async Task WriteString_BytePrefix_NonEmpty_WritesCorrect()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write("Hi!", StringLengthPrefix.Byte);

        // Assert
        await Assert.That(ms.Position).IsEqualTo(4);

        var data = ms.ToArray();
        await Assert.That(data).IsEquivalentTo((byte[])[3, (byte)'H', (byte)'i', (byte)'!'], CollectionOrdering.Matching);
    }

    [Test]
    public async Task WriteString_BytePrefix_LengthOver255_Throws()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => w.Write(GetRandomString(256), StringLengthPrefix.Byte));
        await Assert.That(ms.Position).IsEqualTo(0);
    }

    [Test]
    public async Task WriteString_BytePrefix_ByteLengthOver255_Throws()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act & Assert
        Assert.Throws<LengthLimitException>(() => w.Write(GetRandomString(254) + "š", StringLengthPrefix.Byte));
        await Assert.That(ms.Position).IsEqualTo(0);
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    public async Task WriteString_Int32Prefix_NullOrEmpty_WritesCorrect(string? value)
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write(value);

        // Assert
        await Assert.That(ms.Position).IsEqualTo(4);

        var data = ms.ToArray();
        await Assert.That(data).IsEquivalentTo((byte[])[0, 0, 0, 0], CollectionOrdering.Matching);
    }

    [Test]
    public async Task WriteString_Int32Prefix_NonEmpty_WritesCorrect()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);

        // Act
        w.Write("Hi!", StringLengthPrefix.Int32);

        // Assert
        await Assert.That(ms.Position).IsEqualTo(7);

        var data = ms.ToArray();
        await Assert.That(data).IsEquivalentTo((byte[])[3, 0, 0, 0, (byte)'H', (byte)'i', (byte)'!'], CollectionOrdering.Matching);
    }

    [Test]
    public async Task WriteString_Int32Prefix_LargerString_WritesCorrect()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);
        var str = GetRandomString(128);
        var expected = BitConverter.GetBytes(str.Length).Concat(Encoding.UTF8.GetBytes(str)).ToArray();

        // Act
        w.Write(str, StringLengthPrefix.Int32);

        // Assert
        await Assert.That(ms.Position).IsEqualTo(132);

        var data = ms.ToArray();
        await Assert.That(data).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    public async Task WriteString_Int32Prefix_MuchLargerString_WritesCorrect()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var w = new GbxWriter(ms);
        var str = GetRandomString(ushort.MaxValue);
        var expected = BitConverter.GetBytes(str.Length).Concat(Encoding.UTF8.GetBytes(str)).ToArray();

        // Act
        w.Write(str, StringLengthPrefix.Int32);

        // Assert
        await Assert.That(ms.Position).IsEqualTo(ushort.MaxValue + 4);

        var data = ms.ToArray();
        await Assert.That(data).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    private static string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}
