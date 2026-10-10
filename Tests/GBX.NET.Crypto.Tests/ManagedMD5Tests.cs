using System.Text;
using TUnit.Assertions.Enums;
using ReferenceMD5 = System.Security.Cryptography.MD5;

namespace GBX.NET.Crypto.Tests;

public class ManagedMD5Tests
{
    public static IEnumerable<int> LengthCases()
    {
        // Cover empty input, sub-block, exact block, block boundaries and multi-block inputs.
        foreach (var length in new[] { 0, 1, 2, 3, 16, 55, 56, 57, 63, 64, 65, 119, 120, 127, 128, 129, 255, 256, 1000 })
        {
            yield return length;
        }
    }

    [Test]
    [MethodDataSource(nameof(LengthCases))]
    public async Task Compute_MatchesReferenceMD5_ForVariousLengths(int length)
    {
        var data = new byte[length];
        new Random(length).NextBytes(data);

        var expected = ReferenceMD5.HashData(data);
        var actual = ManagedMD5.Compute(data);

        await Assert.That(actual).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments("")]
    [Arguments("a")]
    [Arguments("abc")]
    [Arguments("message digest")]
    [Arguments("abcdefghijklmnopqrstuvwxyz")]
    [Arguments("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")]
    [Arguments("12345678901234567890123456789012345678901234567890123456789012345678901234567890")]
    public async Task Compute_MatchesReferenceMD5_ForKnownStrings(string text)
    {
        var data = Encoding.ASCII.GetBytes(text);

        var expected = ReferenceMD5.HashData(data);
        var actual = ManagedMD5.Compute(data);

        await Assert.That(actual).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    public async Task Compute_EmptyInput_MatchesKnownDigest()
    {
        // RFC 1321 test vector: MD5("") = d41d8cd98f00b204e9800998ecf8427e
        var actual = ManagedMD5.Compute(ReadOnlySpan<byte>.Empty);

        await Assert.That(Convert.ToHexString(actual)).IsEqualTo("D41D8CD98F00B204E9800998ECF8427E");
    }

    [Test]
    public async Task Compute_Abc_MatchesKnownDigest()
    {
        // RFC 1321 test vector: MD5("abc") = 900150983cd24fb0d6963f7d28e17f72
        var actual = ManagedMD5.Compute(Encoding.ASCII.GetBytes("abc"));

        await Assert.That(Convert.ToHexString(actual)).IsEqualTo("900150983CD24FB0D6963F7D28E17F72");
    }

    [Test]
    public async Task Compute_IntoDestination_MatchesReferenceMD5()
    {
        var data = new byte[300];
        new Random(42).NextBytes(data);

        var expected = ReferenceMD5.HashData(data);

        Span<byte> destination = stackalloc byte[16];
        var written = ManagedMD5.Compute(data, destination);

        var matches = destination.SequenceEqual(expected);

        await Assert.That(written).IsEqualTo(16);
        await Assert.That(matches).IsTrue();
    }

    [Test]
    public void Compute_IntoTooSmallDestination_Throws()
    {
        var data = new byte[8];

        Assert.Throws<ArgumentException>(() =>
        {
            var destination = new byte[15];
            ManagedMD5.Compute(data, destination);
        });
    }

    [Test]
    public async Task Compute_MatchesReferenceMD5_AcrossManyRandomInputs()
    {
        var random = new Random(12345);

        for (var i = 0; i < 500; i++)
        {
            var data = new byte[random.Next(0, 300)];
            random.NextBytes(data);

            var expected = ReferenceMD5.HashData(data);
            var actual = ManagedMD5.Compute(data);

            await Assert.That(actual).IsEquivalentTo(expected, CollectionOrdering.Matching);
        }
    }
}
