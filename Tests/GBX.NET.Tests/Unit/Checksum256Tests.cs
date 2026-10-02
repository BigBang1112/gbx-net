namespace GBX.NET.Tests.Unit;

public class Checksum256Tests
{
    [Test]
    public async Task ReadsAndWritesThirtyTwoBytesInOrder()
    {
        byte[] source = Enumerable.Range(0, 36).Select(i => (byte)i).ToArray();
        var checksum = new Checksum256(source);
        source[0] = 99;
        byte[] destination = Enumerable.Repeat((byte)0xAA, 36).ToArray();

        checksum.WriteLittleEndian(destination);

        await Assert.That(destination[..32]).IsEquivalentTo(Enumerable.Range(0, 32).Select(i => (byte)i).ToArray(), CollectionOrdering.Matching);
        await Assert.That(destination[32..]).IsEquivalentTo(new byte[] { 0xAA, 0xAA, 0xAA, 0xAA }, CollectionOrdering.Matching);
        await Assert.That(checksum.GetBytes()).IsEquivalentTo(destination[..32], CollectionOrdering.Matching);
        await Assert.That(checksum.ToString()).IsEqualTo("000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F");
    }

    [Test]
    public void RejectsShortSourceAndDestination()
    {
        Assert.Throws<ArgumentException>(() => new Checksum256(new byte[31]));
        Assert.Throws<ArgumentException>(() => Checksum256.Zero.WriteLittleEndian(new byte[31]));
    }
}
