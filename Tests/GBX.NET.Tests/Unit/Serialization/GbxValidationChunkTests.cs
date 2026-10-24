using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using GBX.NET.Tests.Mocks;
using SimplestGitSourceGenerator;
using System.Security.Cryptography;
using System.Text;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class GbxValidationChunkTests
{
    private const uint ChunkId = 0x01112000;

    [Test]
    [Arguments(GbxCompression.Uncompressed, false)]
    [Arguments(GbxCompression.Uncompressed, true)]
    [Arguments(GbxCompression.Compressed, false)]
    [Arguments(GbxCompression.Compressed, true)]
    public async Task Save_WritesOneFinalRootChunkWithMatchingBodyHash(GbxCompression compression, bool async)
    {
        var track = new CGameCtnMediaTrack { Name = "Nested track", Blocks = [] };
        track.CreateChunk<CGameCtnMediaTrack.Chunk03078001>();
        var clip = new CGameCtnMediaClip { Name = "Validation test", Tracks = [track] };
        clip.CreateChunk<CGameCtnMediaClip.Chunk0307900D>();
        using var originalBody = new MemoryStream();
        using (var writer = new GbxWriter(originalBody))
        using (var rw = new GbxReaderWriter(writer))
        {
            writer.NodeDict[clip] = 0;
            clip.ReadWrite(rw);
        }
        var expectedPrefix = originalBody.ToArray()[..^4];
        var saved = Save(clip, compression);
        var body = GetBody(saved);

        await Assert.That(body[..expectedPrefix.Length]).IsEquivalentTo(expectedPrefix, CollectionOrdering.Matching);
        using var input = new MemoryStream(body);
        input.Position = expectedPrefix.Length;
        using var reader = new GbxReader(input);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(ChunkId);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0x534B4950u);
        var size = reader.ReadInt32();
        var payloadStart = input.Position;
        await Assert.That(Encoding.ASCII.GetString(reader.ReadBytes(4))).IsEqualTo("GBXN");
        await Assert.That(reader.ReadInt32()).IsEqualTo(1);
        await Assert.That(reader.ReadInt64()).IsEqualTo((long)expectedPrefix.Length);
        await Assert.That(reader.ReadBytes(32)).IsEquivalentTo(SHA256.HashData(expectedPrefix), CollectionOrdering.Matching);
        await Assert.That(reader.ReadString()).IsEqualTo(SimplestGit.CommitHash);
        await Assert.That(reader.ReadString()).IsEqualTo(SimplestGit.Tag);
        await Assert.That(input.Position - payloadStart).IsEqualTo((long)size);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xFACADE01u);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var savedInput = new FragmentedReadStream(saved, maxReadSize: 7);
        var reparsed = async
            ? await Gbx.ParseAsync<CGameCtnMediaClip>(savedInput)
            : Gbx.Parse<CGameCtnMediaClip>(savedInput);
        await Assert.That(reparsed.Node.Chunks.Contains(ChunkId)).IsFalse();
        await Assert.That(reparsed.Node.Tracks![0].Chunks.Contains(ChunkId)).IsFalse();
        await Assert.That(reparsed.Node.Tracks[0].Name).IsEqualTo(track.Name);
        await Assert.That(Save(reparsed.Node, compression)).IsEquivalentTo(saved, CollectionOrdering.Matching);
    }

    [Test]
    public async Task Read_IgnoresUnrecognizedValidationPayloadAndContinuesWithFollowingChunks()
    {
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            writer.Write(ChunkId);
            writer.Write(0x534B4950u);
            writer.Write(3);
            writer.Write(new byte[] { 0xFF, 0xFE, 0xFD });
            writer.Write(0x03079006u);
            writer.WriteIdAsString("Still readable");
            writer.Write(0xFACADE01u);
        }
        using var fragmented = new FragmentedReadStream(input.ToArray(), maxReadSize: 1);
        using var reader = new GbxReader(fragmented);
        using var rw = new GbxReaderWriter(reader);
        var clip = new CGameCtnMediaClip();

        clip.ReadWrite(rw);

        await Assert.That(clip.Chunks.Contains(ChunkId)).IsFalse();
        await Assert.That(clip.Chunks.Contains(0x03079006)).IsTrue();
        await Assert.That(fragmented.ReadByte()).IsEqualTo(-1);
    }

    [Test]
    public async Task Save_DiscardsManuallyAttachedValidationChunkWithoutMutatingNode()
    {
        var clip = new CGameCtnMediaClip();
        var expected = Save(clip, GbxCompression.Uncompressed);
        var staleChunk = new SkippableChunk(ChunkId) { Data = [1, 2, 3] };
        clip.Chunks.Add(staleChunk);

        var actual = Save(clip, GbxCompression.Uncompressed);

        await Assert.That(actual).IsEquivalentTo(expected, CollectionOrdering.Matching);
        await Assert.That(clip.Chunks.Get(ChunkId)).IsSameReferenceAs(staleChunk);
    }

    [Test]
    public async Task Save_RawBodyWithoutValidation_PreservesOriginalBytes()
    {
        var clip = new CGameCtnMediaClip();
        var saved = Save(clip, GbxCompression.Uncompressed);
        var body = GetBody(saved);
        // The empty node has no preceding body bytes. Remove the implicit chunk for a legacy file.
        var legacy = saved[..^(body.Length - 4)];
        BitConverter.GetBytes(0xFACADE01u).CopyTo(legacy, legacy.Length - 4);
        using var input = new MemoryStream(legacy);
        var gbx = Gbx.ParseHeader<CGameCtnMediaClip>(input, new() { ReadRawBody = true });
        using var output = new MemoryStream();

        gbx.Save(output);

        await Assert.That(output.ToArray()).IsEquivalentTo(legacy, CollectionOrdering.Matching);
    }

    private static byte[] Save(CGameCtnMediaClip clip, GbxCompression compression)
    {
        var gbx = new Gbx<CGameCtnMediaClip>(clip) { BodyCompression = compression };
        using var output = new MemoryStream();
        gbx.Save(output);
        return output.ToArray();
    }

    private static byte[] GetBody(byte[] saved)
    {
        using var input = new MemoryStream(saved);
        var gbx = Gbx.ParseHeader<CGameCtnMediaClip>(input);
        if (gbx.BodyCompression == GbxCompression.Uncompressed)
        {
            return saved[(int)input.Position..];
        }
        using var reader = new BinaryReader(input);
        var body = new byte[gbx.Body.UncompressedSize];
        var compressed = reader.ReadBytes(gbx.Body.CompressedSize!.Value);
        Gbx.LZO!.Decompress(compressed, body);
        return body;
    }
}
