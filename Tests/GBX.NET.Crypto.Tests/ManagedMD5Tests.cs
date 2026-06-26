using System.Text;
using ReferenceMD5 = System.Security.Cryptography.MD5;

namespace GBX.NET.Crypto.Tests;

public class ManagedMD5Tests
{
    public static IEnumerable<object[]> LengthCases()
    {
        // Cover empty input, sub-block, exact block, block boundaries and multi-block inputs.
        foreach (var length in new[] { 0, 1, 2, 3, 16, 55, 56, 57, 63, 64, 65, 119, 120, 127, 128, 129, 255, 256, 1000 })
        {
            yield return new object[] { length };
        }
    }

    [Theory]
    [MemberData(nameof(LengthCases))]
    public void Compute_MatchesReferenceMD5_ForVariousLengths(int length)
    {
        var data = new byte[length];
        new Random(length).NextBytes(data);

        var expected = ReferenceMD5.HashData(data);
        var actual = ManagedMD5.Compute(data);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("abc")]
    [InlineData("message digest")]
    [InlineData("abcdefghijklmnopqrstuvwxyz")]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")]
    [InlineData("12345678901234567890123456789012345678901234567890123456789012345678901234567890")]
    public void Compute_MatchesReferenceMD5_ForKnownStrings(string text)
    {
        var data = Encoding.ASCII.GetBytes(text);

        var expected = ReferenceMD5.HashData(data);
        var actual = ManagedMD5.Compute(data);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Compute_EmptyInput_MatchesKnownDigest()
    {
        // RFC 1321 test vector: MD5("") = d41d8cd98f00b204e9800998ecf8427e
        var actual = ManagedMD5.Compute(ReadOnlySpan<byte>.Empty);

        Assert.Equal("D41D8CD98F00B204E9800998ECF8427E", Convert.ToHexString(actual));
    }

    [Fact]
    public void Compute_Abc_MatchesKnownDigest()
    {
        // RFC 1321 test vector: MD5("abc") = 900150983cd24fb0d6963f7d28e17f72
        var actual = ManagedMD5.Compute(Encoding.ASCII.GetBytes("abc"));

        Assert.Equal("900150983CD24FB0D6963F7D28E17F72", Convert.ToHexString(actual));
    }

    [Fact]
    public void Compute_IntoDestination_MatchesReferenceMD5()
    {
        var data = new byte[300];
        new Random(42).NextBytes(data);

        var expected = ReferenceMD5.HashData(data);

        Span<byte> destination = stackalloc byte[16];
        var written = ManagedMD5.Compute(data, destination);

        Assert.Equal(16, written);
        Assert.True(destination.SequenceEqual(expected));
    }

    [Fact]
    public void Compute_IntoTooSmallDestination_Throws()
    {
        var data = new byte[8];

        Assert.Throws<ArgumentException>(() =>
        {
            var destination = new byte[15];
            ManagedMD5.Compute(data, destination);
        });
    }

    [Fact]
    public void Compute_MatchesReferenceMD5_AcrossManyRandomInputs()
    {
        var random = new Random(12345);

        for (var i = 0; i < 500; i++)
        {
            var data = new byte[random.Next(0, 300)];
            random.NextBytes(data);

            var expected = ReferenceMD5.HashData(data);
            var actual = ManagedMD5.Compute(data);

            Assert.Equal(expected, actual);
        }
    }
}
