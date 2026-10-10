using GBX.NET.Serialization;
using System.Net;
using System.Net.Sockets;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class IPAddressSerializationTests
{
    [Test]
    [Arguments(0u, "0.0.0.0")]
    [Arguments(0x7F000001u, "127.0.0.1")]
    [Arguments(0xC0A80101u, "192.168.1.1")]
    [Arguments(0x01020304u, "1.2.3.4")]
    [Arguments(uint.MaxValue, "255.255.255.255")]
    public async Task ReadsAndWritesIPv4UInt32(uint value, string text)
    {
        var expectedBytes = new[] { (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24) };
        using var input = new MemoryStream(expectedBytes);
        using var reader = new GbxReader(input);
        var address = reader.ReadIPv4();

        await Assert.That(address.ToString()).IsEqualTo(text);
        await Assert.That(address.AddressFamily).IsEqualTo(AddressFamily.InterNetwork);
        await Assert.That(input.Position).IsEqualTo(4L);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        writer.WriteIPv4(IPAddress.Parse(text));
        await Assert.That(output.ToArray().SequenceEqual(expectedBytes)).IsTrue();
    }

    [Test]
    public async Task CombinedSerializationReadsBeforeWriting()
    {
        byte[] bytes = [1, 0, 0, 127];
        using var input = new MemoryStream(bytes);
        using var output = new MemoryStream();
        using var reader = new GbxReader(input);
        using var writer = new GbxWriter(output);
        using var rw = new GbxReaderWriter(reader, writer);
        IPAddress? address = IPAddress.Broadcast;
        rw.IPv4(ref address);

        await Assert.That(address).IsEqualTo(IPAddress.Loopback);
        await Assert.That(output.ToArray().SequenceEqual(bytes)).IsTrue();
    }

    [Test]
    public async Task NullWritesZeroAndIPv6IsRejectedBeforeWriting()
    {
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        writer.WriteIPv4(null);
        await Assert.That(output.ToArray()).IsEquivalentTo(new byte[4]);

        Assert.Throws<ArgumentException>(() => writer.WriteIPv4(IPAddress.IPv6Loopback));
        Assert.Throws<ArgumentException>(() => writer.WriteIPv4(IPAddress.Parse("::ffff:127.0.0.1")));
        await Assert.That(output.Length).IsEqualTo(4L);
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
