using GBX.NET.Serialization;
using System.Net;
using System.Net.Sockets;

namespace GBX.NET.Tests.Unit.Serialization;

public class IPAddressSerializationTests
{
    [Test]
    [Arguments(0u, "0.0.0.0")]
    [Arguments(0x0100007Fu, "127.0.0.1")]
    [Arguments(0x0101A8C0u, "192.168.1.1")]
    [Arguments(uint.MaxValue, "255.255.255.255")]
    public async Task ReadsAndWritesIPv4UInt32(uint value, string text)
    {
        var expectedBytes = new[] { (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24) };
        using var input = new MemoryStream(expectedBytes);
        using var reader = new GbxReader(input);
        var address = reader.ReadIPAddress();

        await Assert.That(address.ToString()).IsEqualTo(text);
        await Assert.That(address.AddressFamily).IsEqualTo(AddressFamily.InterNetwork);
        await Assert.That(input.Position).IsEqualTo(4L);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        writer.Write(IPAddress.Parse(text));
        await Assert.That(output.ToArray().SequenceEqual(expectedBytes)).IsTrue();
    }

    [Test]
    public async Task CombinedSerializationReadsBeforeWriting()
    {
        byte[] bytes = [127, 0, 0, 1];
        using var input = new MemoryStream(bytes);
        using var output = new MemoryStream();
        using var reader = new GbxReader(input);
        using var writer = new GbxWriter(output);
        using var rw = new GbxReaderWriter(reader, writer);
        IPAddress? address = IPAddress.Broadcast;
        rw.IPAddress(ref address);

        await Assert.That(address).IsEqualTo(IPAddress.Loopback);
        await Assert.That(output.ToArray().SequenceEqual(bytes)).IsTrue();
    }

    [Test]
    public async Task NullWritesZeroAndIPv6IsRejectedBeforeWriting()
    {
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        writer.Write((IPAddress?)null);
        await Assert.That(output.ToArray()).IsEquivalentTo(new byte[4]);

        Assert.Throws<ArgumentException>(() => writer.Write(IPAddress.IPv6Loopback));
        Assert.Throws<ArgumentException>(() => writer.Write(IPAddress.Parse("::ffff:127.0.0.1")));
        await Assert.That(output.Length).IsEqualTo(4L);
    }

    [Test]
    public async Task CollectionsRoundTripWithFixedLengthsAndPrefixes()
    {
        IPAddress[] addresses = [IPAddress.Loopback, IPAddress.Parse("192.168.1.1"), IPAddress.Broadcast];
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer))
        {
            rw.ArrayIPAddress(addresses);
            rw.ArrayIPAddress(addresses, 3);
            rw.ArrayIPAddress_deprec(addresses);
            rw.ListIPAddress(addresses.ToList());
            rw.ListIPAddress(addresses.ToList(), 3);
            rw.ListIPAddress_deprec(addresses.ToList());
            rw.JaggedArrayIPAddress([addresses, [], [IPAddress.Any]]);
        }

        using var input = new MemoryStream(output.ToArray());
        using var reader = new GbxReader(input);
        using var readWrite = new GbxReaderWriter(reader);
        await Assert.That(readWrite.ArrayIPAddress()!.SequenceEqual(addresses)).IsTrue();
        await Assert.That(readWrite.ArrayIPAddress(null, 3)!.SequenceEqual(addresses)).IsTrue();
        await Assert.That(readWrite.ArrayIPAddress_deprec()!.SequenceEqual(addresses)).IsTrue();
        await Assert.That(readWrite.ListIPAddress()!.SequenceEqual(addresses)).IsTrue();
        await Assert.That(readWrite.ListIPAddress(null, 3)!.SequenceEqual(addresses)).IsTrue();
        await Assert.That(readWrite.ListIPAddress_deprec()!.SequenceEqual(addresses)).IsTrue();
        var rows = readWrite.JaggedArrayIPAddress(null)!;
        await Assert.That(rows.Length).IsEqualTo(3);
        await Assert.That(rows[0].SequenceEqual(addresses)).IsTrue();
        await Assert.That(rows[1]).IsEmpty();
        await Assert.That(rows[2].SequenceEqual(new[] { IPAddress.Any })).IsTrue();
        await Assert.That(input.Position).IsEqualTo(input.Length);
    }

    [Test]
    public async Task DeepCloneCopiesAddressesAndPreservesSharedReferences()
    {
        var address = IPAddress.Parse("192.168.1.1");
        var context = new DeepCloneContext();
        var clone = context.Clone(address);
        await Assert.That(clone).IsEqualTo(address);
        await Assert.That(ReferenceEquals(clone, address)).IsFalse();
        await Assert.That(ReferenceEquals(context.Clone(address), clone)).IsTrue();
    }
}
