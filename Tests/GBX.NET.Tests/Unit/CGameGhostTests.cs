using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
using GBX.NET.Extensions;
using GBX.NET.Serialization;
using System.IO.Compression;

namespace GBX.NET.Tests.Unit;

[CollectionDefinition(nameof(CGameGhostTests), DisableParallelization = true)]
public class GhostCompressionCollection;

[Collection(nameof(CGameGhostTests))]
public class CGameGhostTests : IDisposable
{
    private readonly IZLib? originalZLib;

    public CGameGhostTests()
    {
        try
        {
            originalZLib = Gbx.ZLib;
        }
        catch (ZLibNotDefinedException)
        {
        }

        Gbx.ZLib = new TestZLib();
    }

    public void Dispose() => Gbx.ZLib = originalZLib!;

    [Theory]
    [InlineData(false, 0, 16)]
    [InlineData(true, 0, 16)]
    [InlineData(true, 1, 0)]
    [InlineData(true, 2, 0)]
    public void SampleData_ChunkReadAndWrite_PreservesChunkVersion(bool versionedChunk, int archiveVersion, int stateVersion)
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
            Assert.Equal(archiveVersion, versioned.Version);
        }
        Assert.Empty(data.Samples);
        Assert.True(ghost.CompressedData!.Parsed);
        Assert.Null(ghost.CompressedData.Exception);
        Assert.Same(data, ghost.SampleData);

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
            Assert.Equal(archiveVersion, outputReader.ReadInt32());
        }
        using var decompressed = outputReader.ReadZlibData().OpenDecompressedReader();
        Assert.Equal(payload, decompressed.ReadToEnd());
        Assert.Equal(output.Length, output.Position);
    }

    [Fact]
    public void SampleData_FailedRead_DoesNotCachePartialDataAndCanRetry()
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

        var compressedData = Assert.IsType<ZlibData>(ghost.CompressedData);
        var first = Assert.Throws<InvalidDataException>(() => ghost.SampleData);
        Assert.False(compressedData.Parsed);
        Assert.Same(first, compressedData.Exception);

        var second = Assert.Throws<InvalidDataException>(() => ghost.SampleData);
        Assert.False(compressedData.Parsed);
        Assert.Same(second, compressedData.Exception);

        // A successful retry on the same compressed-data object must clear the stored failure.
        var validCompressedData = Compress(payload);
        Assert.True(validCompressedData.Data.Length <= compressedData.Data.Length);
        validCompressedData.Data.CopyTo(compressedData.Data, 0);
        var parsed = ghost.SampleData;

        Assert.Empty(parsed.Samples);
        Assert.True(compressedData.Parsed);
        Assert.Null(compressedData.Exception);
        Assert.Same(parsed, ghost.SampleData);
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

    private sealed class TestZLib : IZLib
    {
        public void Compress(Stream input, Stream output)
        {
            using var stream = new ZLibStream(output, CompressionLevel.Optimal, leaveOpen: true);
            input.CopyTo(stream);
        }

        public void Decompress(Stream input, Stream output)
        {
            using var stream = Decompress(input);
            stream.CopyTo(output);
        }

        public Stream Decompress(Stream input) => new ZLibStream(input, CompressionMode.Decompress, leaveOpen: true);
    }
}
