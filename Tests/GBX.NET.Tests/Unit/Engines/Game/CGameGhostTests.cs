using GBX.NET.Engines.Game;
using GBX.NET.Extensions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameGhostTests
{
    [Test]
    [Arguments(false, 0, 16)]
    [Arguments(true, 0, 16)]
    [Arguments(true, 1, 0)]
    [Arguments(true, 2, 0)]
    public async Task SampleData_ChunkReadAndWrite_PreservesChunkVersion(bool versionedChunk, int archiveVersion, int stateVersion)
    {
        var payload = CreateEmptyArchive(archiveVersion, stateVersion);
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            if (versionedChunk)
            {
                writer.Write(archiveVersion);
            }
            WriteCompressedData(writer, payload);
        }
        input.Position = 0;
        using var reader = new GbxReader(input);
        var ghost = new CGameGhost();
        CGameGhost.Chunk0303F005 chunk = versionedChunk
            ? new CGameGhost.Chunk0303F006 { Version = archiveVersion }
            : new CGameGhost.Chunk0303F005();

        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(ghost, rw);
        }
        var data = ghost.SampleData;

        if (chunk is CGameGhost.Chunk0303F006 versioned)
        {
            await Assert.That(versioned.Version).IsEqualTo(archiveVersion);
        }
        await Assert.That(data.Samples).IsEmpty();
        await Assert.That(ghost.CompressedData!.Parsed).IsTrue();
        await Assert.That(ghost.CompressedData.Exception).IsNull();
        await Assert.That(ReferenceEquals(data, ghost.SampleData)).IsTrue();

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        {
            using var rw = new GbxReaderWriter(writer);
            chunk.ReadWrite(ghost, rw);
        }
        output.Position = 0;
        using var outputReader = new GbxReader(output);
        if (versionedChunk)
        {
            await Assert.That(outputReader.ReadInt32()).IsEqualTo(archiveVersion);
        }
        using var decompressed = outputReader.ReadZlibData().OpenDecompressedReader();
        await Assert.That(decompressed.ReadToEnd()).IsEquivalentTo(payload, CollectionOrdering.Matching);
        await Assert.That(output.Position).IsEqualTo(output.Length);
    }

    [Test]
    public async Task SampleData_FailedRead_DoesNotCachePartialDataAndCanRetry()
    {
        var payload = CreateEmptyArchive(1, 0);
        byte[] invalidPayload = [.. payload, 1, 0, 0, 0];
        var ghost = new CGameGhost();
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            writer.Write(1);
            WriteCompressedData(writer, invalidPayload);
        }
        input.Position = 0;
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            new CGameGhost.Chunk0303F006().ReadWrite(ghost, rw);
        }

        var compressedData = (await Assert.That(ghost.CompressedData).IsTypeOf<ZlibData>())!;
        var first = Assert.Throws<InvalidDataException>(() => _ = ghost.SampleData);
        await Assert.That(compressedData.Parsed).IsFalse();
        await Assert.That(ReferenceEquals(first, compressedData.Exception)).IsTrue();

        var second = Assert.Throws<InvalidDataException>(() => _ = ghost.SampleData);
        await Assert.That(compressedData.Parsed).IsFalse();
        await Assert.That(ReferenceEquals(second, compressedData.Exception)).IsTrue();

        // A successful retry on the same compressed-data object must clear the stored failure.
        var validCompressedData = Compress(payload);
        await Assert.That(validCompressedData.Data.Length <= compressedData.Data.Length).IsTrue();
        validCompressedData.Data.CopyTo(compressedData.Data, 0);
        var parsed = ghost.SampleData;

        await Assert.That(parsed.Samples).IsEmpty();
        await Assert.That(compressedData.Parsed).IsTrue();
        await Assert.That(compressedData.Exception).IsNull();
        await Assert.That(ReferenceEquals(parsed, ghost.SampleData)).IsTrue();
    }

    private static byte[] CreateEmptyArchive(int archiveVersion, int stateVersion)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.Write(0u);
        writer.Write(true);
        writer.Write(0);
        writer.Write(0);
        writer.Write(stateVersion);
        writer.WriteData([]);
        writer.Write(0);
        if (archiveVersion != 0)
        {
            writer.Write(0);
        }
        return stream.ToArray();
    }

    private static ZlibData Compress(byte[] payload)
    {
        using var input = new MemoryStream(payload);
        using var output = new MemoryStream();
        Gbx.ZLib.Compress(input, output);
        return new ZlibData(payload.Length, output.ToArray(), exception: null);
    }

    private static void WriteCompressedData(GbxWriter writer, byte[] payload)
    {
        var compressed = Compress(payload);
        writer.Write(compressed.UncompressedSize);
        writer.WriteData(compressed.Data);
    }
}
