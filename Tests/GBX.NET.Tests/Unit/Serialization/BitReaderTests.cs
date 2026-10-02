using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

public class BitReaderTests
{
    [Test]
    public async Task ReadsLeastSignificantBitsFirstAcrossByteBoundaries()
    {
        var reader = new BitReader([0b11010110, 0b01101001]);

        await Assert.That(reader.ReadBit()).IsFalse();
        await Assert.That(reader.Read2Bit()).IsEqualTo((byte)3);
        await Assert.That(reader.ReadNumber(7)).IsEqualTo(58UL);
        await Assert.That(reader.Position).IsEqualTo(10);
        await Assert.That(reader.Length).IsEqualTo(16);
        await Assert.That(reader.ReadNumber(6)).IsEqualTo(26UL);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(15)]
    [Arguments(16)]
    public async Task ReadToEndRepacksRemainingBitsAndAdvancesToTheEnd(int offset)
    {
        var reader = new BitReader([0xA5, 0xC3]);
        reader.ReadNumber(offset);
        var count = (16 - offset + 7) / 8;
        var expected = BitConverter.GetBytes((ushort)(0xC3A5 >> offset))[..count];

        await Assert.That(reader.ReadToEnd()).IsEquivalentTo(expected, CollectionOrdering.Matching);
        await Assert.That(reader.Position).IsEqualTo(16);
        await Assert.That(reader.ReadToEnd()).IsEmpty();
    }

    [Test]
    public async Task ReadsSignedValuesUsingTwosComplementAndLittleEndianOrder()
    {
        var reader = new BitReader([0xFE, 0x00, 0x80, 0x78, 0x56, 0x34, 0x12]);

        await Assert.That(reader.ReadSByte()).IsEqualTo((sbyte)-2);
        await Assert.That(reader.ReadInt16()).IsEqualTo(short.MinValue);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
    }

    [Test]
    public async Task ZeroBitReadDoesNotConsumeInput()
    {
        var reader = new BitReader([]);
        await Assert.That(reader.ReadNumber(0)).IsEqualTo(0UL);
        await Assert.That(reader.Position).IsEqualTo(0);
        await Assert.That(reader.ReadToEnd()).IsEmpty();
    }
}
