namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class Checksum128Tests
{
    [Test]
    public async Task ReadsAndWritesSixteenBytesInOrder()
    {
        byte[] source = Enumerable.Range(0, 20).Select(i => (byte)i).ToArray();
        var checksum = new Checksum128(source);
        source[0] = 99;
        byte[] destination = Enumerable.Repeat((byte)0xAA, 20).ToArray();

        checksum.WriteLittleEndian(destination);

        await Assert.That(destination[..16]).IsEquivalentTo(Enumerable.Range(0, 16).Select(i => (byte)i).ToArray(), CollectionOrdering.Matching);
        await Assert.That(destination[16..]).IsEquivalentTo(new byte[] { 0xAA, 0xAA, 0xAA, 0xAA }, CollectionOrdering.Matching);
        await Assert.That(checksum.GetBytes()).IsEquivalentTo(destination[..16], CollectionOrdering.Matching);
        await Assert.That(checksum.ToString()).IsEqualTo("000102030405060708090A0B0C0D0E0F");
    }

    [Test]
    public void RejectsShortSourceAndDestination()
    {
        Assert.Throws<ArgumentException>(() => new Checksum128(new byte[15]));
        Assert.Throws<ArgumentException>(() => Checksum128.Zero.WriteLittleEndian(new byte[15]));
    }
}
