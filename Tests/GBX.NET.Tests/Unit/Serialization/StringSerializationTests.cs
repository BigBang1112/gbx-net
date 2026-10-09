using GBX.NET.Exceptions;
using GBX.NET.Serialization;
using GBX.NET.Tests.Mocks;
using System.Text;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class StringSerializationTests
{
    public static IEnumerable<(string? Value, StringLengthPrefix Prefix)> Strings()
    {
        foreach (var value in new[] { null, "", "Gbx", "Příliš žluťoučký 🏁", new string('a', 127), new string('a', 128), new string('a', 255) })
        {
            yield return (value, StringLengthPrefix.Byte);
            yield return (value, StringLengthPrefix.Int32);
        }
    }

    [Test]
    [MethodDataSource(nameof(Strings))]
    public async Task Utf8LengthCountsBytesAndSupportsPartialReads(string? value, StringLengthPrefix prefix)
    {
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        {
            writer.Write(value, prefix);
            writer.Write((byte)99);
        }

        var bytes = output.ToArray();
        var byteCount = Encoding.UTF8.GetByteCount(value ?? "");
        var prefixLength = prefix == StringLengthPrefix.Byte ? 1 : 4;
        await Assert.That(bytes.Length).IsEqualTo(prefixLength + byteCount + 1);
        await Assert.That(prefix == StringLengthPrefix.Byte ? bytes[0] : BitConverter.ToInt32(bytes)).IsEqualTo(byteCount);

        using var input = new FragmentedReadStream(bytes);
        using var reader = new GbxReader(input);
        await Assert.That(reader.ReadString(prefix)).IsEqualTo(value ?? "");
        await Assert.That(reader.ReadByte()).IsEqualTo((byte)99);
    }

    [Test]
    [Arguments("a", 256)]
    [Arguments("é", 128)]
    public async Task BytePrefixRejectsOversizedUtf8BeforeWriting(string character, int count)
    {
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        var value = string.Concat(Enumerable.Repeat(character, count));

        Assert.Throws<LengthLimitException>(() => writer.Write(value, StringLengthPrefix.Byte));
        await Assert.That(output.Length).IsEqualTo(0L);
    }

    [Test]
    public void TruncatedStringThrowsInsteadOfReturningPartialText()
    {
        using var input = new FragmentedReadStream([5, 0, 0, 0, (byte)'a']);
        using var reader = new GbxReader(input);
        Assert.Throws<EndOfStreamException>(() => reader.ReadString());
    }
}
