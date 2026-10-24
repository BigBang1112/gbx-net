using KellermanSoftware.CompareNetObjects;
using System.Buffers.Binary;
using System.Security.Cryptography;

namespace GBX.NET.Tests.Infrastructure;

internal static class GbxAssert
{
    public static async Task AreDeeplyEqual(object? expected, object? actual)
    {
        var comparison = new CompareLogic(new ComparisonConfig
        {
            MaxDifferences = 20,
            IgnoreCollectionOrder = false
        }).Compare(expected, actual);

        await Assert.That(comparison.AreEqual).IsTrue().Because(comparison.DifferencesString);
    }

    public static Task HaveEqualSerializedData(Gbx expected, Gbx actual) => AreDeeplyEqual(
        new { expected.Header, expected.RefTable, expected.Node },
        new { actual.Header, actual.RefTable, actual.Node });

    public static async Task HaveEqualSavedBytes(byte[] expected, byte[] actual)
    {
        var (expectedCompression, expectedBytes) = await NormalizeValidationTimestamp(expected);
        var (actualCompression, actualBytes) = await NormalizeValidationTimestamp(actual);
        await Assert.That(actualCompression).IsEqualTo(expectedCompression);
        await Assert.That(actualBytes).IsEquivalentTo(expectedBytes, CollectionOrdering.Matching);
    }

    private static async Task<(GbxCompression Compression, byte[] Bytes)> NormalizeValidationTimestamp(byte[] saved)
    {
        using var input = new MemoryStream(saved);
        var compression = Gbx.ParseHeader(input).BodyCompression;
        input.Position = 0;
        using var output = new MemoryStream();
        Gbx.Decompress(input, output);
        output.Position = 0;
        Gbx.ParseHeader(output);
        var bodyStart = checked((int)output.Position);
        var bytes = output.ToArray();
        // The final fields are timestamp (8), byte count (8), SHA-256 (32), and FACADE (4).
        var hashStart = bytes.Length - 36;
        var timestampStart = hashStart - 16;
        var byteCount = BinaryPrimitives.ReadInt64LittleEndian(bytes.AsSpan(hashStart - 8, 8));
        await Assert.That(byteCount).IsEqualTo((long)(hashStart - bodyStart));
        await Assert.That(BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(bytes.Length - 4))).IsEqualTo(0xFACADE01u);
        await Assert.That(bytes[hashStart..^4])
            .IsEquivalentTo(SHA256.HashData(bytes.AsSpan(bodyStart, hashStart - bodyStart)), CollectionOrdering.Matching);

        // Only the timestamp and its dependent digest may differ between saves.
        Array.Clear(bytes, timestampStart, 8);
        Array.Clear(bytes, hashStart, 32);
        return (compression, bytes);
    }
}
