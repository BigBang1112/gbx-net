using KellermanSoftware.CompareNetObjects;
using GBX.NET.Serialization;
using System.Buffers.Binary;
using System.Text;

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
        var expectedData = await ReadSavedData(expected);
        var actualData = await ReadSavedData(actual);
        await Assert.That(actualData.Header).IsEquivalentTo(expectedData.Header, CollectionOrdering.Matching);
        await Assert.That(actualData.Body).IsEquivalentTo(expectedData.Body, CollectionOrdering.Matching);

        if (expectedData.Signature is { } expectedSignature)
        {
            var actualSignature = actualData.Signature ?? throw new InvalidDataException("Expected a verified signature.");
            await Assert.That(actualSignature with
            {
                Timestamp = expectedSignature.Timestamp,
                Hash = expectedSignature.Hash
            }).IsEqualTo(expectedSignature);
        }
    }

    private static async Task<(byte[] Header, byte[] Body, GbxSignature? Signature)> ReadSavedData(byte[] file)
    {
        using var stream = new MemoryStream(file);
        var gbx = Gbx.ParseHeader(stream);
        var headerLength = (int)stream.Position;
        byte[] body;
        if (gbx.Body.CompressedSize is int compressedSize)
        {
            // Compression sizes can change with the save timestamp. Compare decompressed bytes instead.
            headerLength -= 8;
            using var reader = new BinaryReader(stream);
            var compressed = reader.ReadBytes(compressedSize);
            await Assert.That(compressed.Length).IsEqualTo(compressedSize);
            await Assert.That(stream.Position).IsEqualTo(stream.Length);
            body = new byte[gbx.Body.UncompressedSize];
            (Gbx.LZO ?? throw new InvalidOperationException("LZO is required to compare compressed bodies.")).Decompress(compressed, body);
        }
        else
        {
            body = file[headerLength..];
        }

        var result = GbxSignatureChunk.Validate(body, default);
        if (body.Length >= 4 && BinaryPrimitives.ReadUInt32LittleEndian(body.AsSpan(body.Length - 4)) == 0xFACADE01)
        {
            await Assert.That(result.Status is GbxSignatureStatus.Valid or GbxSignatureStatus.Missing).IsTrue();
        }

        if (result.Signature is { } signature)
        {
            // Version 1 has 37 fixed bytes before the digest, plus the two UTF-8 strings.
            var chunkStart = checked((int)signature.ByteCount - 37
                - Encoding.UTF8.GetByteCount(signature.CommitHash) - Encoding.UTF8.GetByteCount(signature.Tag));
            body = [.. body.AsSpan(0, chunkStart), .. body.AsSpan(body.Length - 4)];
        }

        return (file[..headerLength], body, result.Signature);
    }
}
