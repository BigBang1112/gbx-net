using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using GBX.NET.Tests.Mocks;
using SimplestGitSourceGenerator;
using System.Buffers.Binary;
using System.Security.Cryptography;

namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class GbxSignatureTests
{
    private const uint ChunkId = 0x01112000;

    [Test]
    [Arguments(GbxCompression.Uncompressed)]
    [Arguments(GbxCompression.Compressed)]
    public async Task Save_AlwaysSigns_HashesBodyAndMetadataOnceAtRoot(GbxCompression compression)
    {
        var track = new CGameCtnMediaTrack { Name = "Nested track", Blocks = [] };
        track.CreateChunk<CGameCtnMediaTrack.Chunk03078001>();
        var clip = new CGameCtnMediaClip { Name = "Signed clip", Tracks = [track] };
        clip.CreateChunk<CGameCtnMediaClip.Chunk0307900D>();
        var gbx = new Gbx<CGameCtnMediaClip>(clip) { BodyCompression = compression };
        using var unsignedBody = new MemoryStream();
        using (var writer = new GbxWriter(unsignedBody))
        using (var rw = new GbxReaderWriter(writer))
        {
            writer.NodeDict[clip] = 0;
            clip.ReadWrite(rw);
        }
        var before = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var signed = Save(gbx);

        var after = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        using var input = new MemoryStream(signed);
        var result = Gbx.Parse(input);
        await Assert.That(result.SignatureStatus).IsEqualTo(GbxSignatureStatus.Valid);
        if (result.Signature is null)
        {
            throw new InvalidDataException("Expected a verified signature.");
        }
        var signature = result.Signature;
        await Assert.That(signature.Version).IsEqualTo(1);
        await Assert.That(signature.CommitHash).IsEqualTo(SimplestGit.CommitHash);
        await Assert.That(signature.Tag).IsEqualTo(SimplestGit.Tag);
        await Assert.That(signature.Timestamp.ToUnixTimeMilliseconds() >= before
            && signature.Timestamp.ToUnixTimeMilliseconds() <= after).IsTrue();

        var body = GetBody(signed);
        var prefix = unsignedBody.ToArray()[..^4];
        await Assert.That(body[..prefix.Length]).IsEquivalentTo(prefix, CollectionOrdering.Matching);
        await Assert.That(BinaryPrimitives.ReadUInt32LittleEndian(body.AsSpan(prefix.Length))).IsEqualTo(ChunkId);
        await Assert.That(BinaryPrimitives.ReadUInt32LittleEndian(body.AsSpan(prefix.Length + 4))).IsEqualTo(0x534B4950u);
        await Assert.That(BinaryPrimitives.ReadInt32LittleEndian(body.AsSpan(prefix.Length + 8))).IsEqualTo(body.Length - prefix.Length - 16);
        await Assert.That(BinaryPrimitives.ReadUInt32LittleEndian(body.AsSpan(prefix.Length + 12))).IsEqualTo(0x4E584247u);
        await Assert.That(signature.ByteCount).IsEqualTo((long)body.Length - 36);
        await Assert.That(signature.Hash).IsEqualTo(new Checksum256(SHA256.HashData(body.AsSpan(0, (int)signature.ByteCount))));
        await Assert.That(BinaryPrimitives.ReadUInt32LittleEndian(body.AsSpan(body.Length - 4))).IsEqualTo(0xFACADE01u);

        using var fragmented = new FragmentedReadStream(signed);
        var parsed = await Gbx.ParseAsync<CGameCtnMediaClip>(fragmented);
        await Assert.That(parsed.Signature).IsEqualTo(signature);
        await Assert.That(((IGbx)parsed).SignatureStatus).IsEqualTo(GbxSignatureStatus.Valid);
        await Assert.That(parsed.Node.Tracks![0].Name).IsEqualTo(track.Name);
        await Assert.That(parsed.Node.Chunks.Contains(ChunkId)).IsFalse();
        await Assert.That(parsed.Node.Tracks[0].Chunks.Contains(ChunkId)).IsFalse();
        await GbxAssert.HaveEqualSavedBytes(signed, Save(parsed));
        await Assert.That(clip.Chunks.Contains(ChunkId)).IsFalse();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed, false, false)]
    [Arguments(GbxCompression.Uncompressed, false, true)]
    [Arguments(GbxCompression.Uncompressed, true, false)]
    [Arguments(GbxCompression.Uncompressed, true, true)]
    [Arguments(GbxCompression.Compressed, false, false)]
    [Arguments(GbxCompression.Compressed, false, true)]
    [Arguments(GbxCompression.Compressed, true, false)]
    [Arguments(GbxCompression.Compressed, true, true)]
    public async Task Parse_ExposesVerifiedSignature(GbxCompression compression, bool async, bool fragmented)
    {
        var file = Save(new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = compression });
        using var expectedInput = new MemoryStream(file);
        var expected = Gbx.Parse(expectedInput);
        using Stream input = fragmented ? new FragmentedReadStream(file) : new MemoryStream(file);

        var gbx = async ? await Gbx.ParseAsync(input) : Gbx.Parse(input);

        await Assert.That(gbx.Signature).IsEqualTo(expected.Signature);
        await Assert.That(gbx.SignatureStatus).IsEqualTo(GbxSignatureStatus.Valid);
        await Assert.That(gbx.Body.RawData.IsEmpty).IsTrue();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed)]
    [Arguments(GbxCompression.Compressed)]
    public async Task Parse_EmbeddedFile_LeavesFollowingDataUnread(GbxCompression compression)
    {
        var file = Save(new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = compression });
        using var input = new MemoryStream();
        input.Write(new byte[7]);
        input.Write(file);
        input.WriteByte(42);
        input.Position = 7;

        var gbx = await Gbx.ParseAsync(input);

        await Assert.That(gbx.SignatureStatus).IsEqualTo(GbxSignatureStatus.Valid);
        await Assert.That(input.ReadByte()).IsEqualTo(42);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Parse_SkippedChunk_StillVerifiesCoveredBytes(bool fragmented, bool tampered)
    {
        const uint skippedId = 0x030790FE;
        var clip = new CGameCtnMediaClip();
        clip.Chunks.Add(new SkippableChunk(skippedId) { Data = [1, 2, 3, 4] });
        var file = Save(new Gbx<CGameCtnMediaClip>(clip) { BodyCompression = GbxCompression.Uncompressed });
        if (tampered)
        {
            file[file.Length - GetBody(file).Length + 12] ^= 1;
        }
        using Stream input = fragmented ? new FragmentedReadStream(file) : new MemoryStream(file);

        var gbx = Gbx.Parse(input, new() { SkipChunkIds = [skippedId] });

        await Assert.That(gbx.SignatureStatus).IsEqualTo(tampered ? GbxSignatureStatus.Mismatched : GbxSignatureStatus.Valid);
        await Assert.That(gbx.Node!.Chunks.Contains(skippedId)).IsFalse();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed)]
    [Arguments(GbxCompression.Compressed)]
    public async Task ParseHeader_UnreadBody_HasNoSignatureAndDoesNotAdvanceOnAccess(GbxCompression compression)
    {
        var file = Save(new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = compression });
        using var input = new MemoryStream(file);
        var gbx = Gbx.ParseHeader(input);
        var position = input.Position;

        await Assert.That(gbx.Signature).IsNull();
        await Assert.That(gbx.SignatureStatus).IsEqualTo(GbxSignatureStatus.Unknown);
        await Assert.That(input.Position).IsEqualTo(position);
        await Assert.That(position < input.Length).IsTrue();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed, false)]
    [Arguments(GbxCompression.Uncompressed, true)]
    [Arguments(GbxCompression.Compressed, false)]
    [Arguments(GbxCompression.Compressed, true)]
    public async Task Parse_RetainedRawBody_ReportsUnknownAndPreservesBytes(GbxCompression compression, bool headerOnly)
    {
        var file = CreateFile(CreateSignedBody(), compression);
        using var input = new FragmentedReadStream(file);
        var gbx = headerOnly ? Gbx.ParseHeader(input, new() { ReadRawBody = true })
            : await Gbx.ParseAsync(input, new() { ReadRawBody = true });

        await Assert.That(gbx.Node).IsNull();
        await Assert.That(gbx.SignatureStatus).IsEqualTo(GbxSignatureStatus.Unknown);
        await Assert.That(gbx.Signature).IsNull();
        await Assert.That(Save(gbx)).IsEquivalentTo(file, CollectionOrdering.Matching);
    }

    [Test]
    public async Task Parse_MismatchedSignature_ReportsStatusWithoutRejectingNode()
    {
        var file = Save(new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = GbxCompression.Uncompressed });
        file[^5] ^= 1; // Last byte of the digest, before the root terminator.
        using var input = new MemoryStream(file);

        var gbx = Gbx.Parse<CGameCtnMediaClip>(input);

        await Assert.That(gbx.Signature).IsNull();
        await Assert.That(gbx.SignatureStatus).IsEqualTo(GbxSignatureStatus.Mismatched);
        await Assert.That(gbx.Body.Exception).IsNull();
    }

    [Test]
    [Arguments(12, 0, GbxSignatureStatus.Malformed)]
    [Arguments(16, 2, GbxSignatureStatus.Unsupported)]
    [Arguments(20, -1, GbxSignatureStatus.Malformed)]
    [Arguments(20, int.MaxValue, GbxSignatureStatus.Malformed)]
    [Arguments(24, 0xFF, GbxSignatureStatus.Malformed)]
    public async Task Parse_InvalidSignatureMetadata_ReportsStatus(int offset, int value, GbxSignatureStatus expected)
    {
        var file = Save(new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = GbxCompression.Uncompressed });
        var body = GetBody(file);
        BinaryPrimitives.WriteInt32LittleEndian(file.AsSpan(file.Length - body.Length + offset), value);
        using var input = new MemoryStream(file);

        var gbx = Gbx.Parse(input);

        await Assert.That(gbx.SignatureStatus).IsEqualTo(expected);
        await Assert.That(gbx.Signature).IsNull();
        await Assert.That(gbx.Body.Exception).IsNull();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed)]
    [Arguments(GbxCompression.Compressed)]
    public async Task Parse_UnsignedArchive_ReportsMissingSignature(GbxCompression compression)
    {
        using var input = new MemoryStream(Save(new Gbx<CPlugStaticObjectModel>(new()) { BodyCompression = compression }));

        var gbx = Gbx.Parse(input);

        await Assert.That(gbx.Signature).IsNull();
        await Assert.That(gbx.SignatureStatus).IsEqualTo(GbxSignatureStatus.Missing);
    }

    [Test]
    public async Task DeepClone_PreservesSignatureFromOriginalRead()
    {
        using var input = new MemoryStream(Save(new Gbx<CGameCtnMediaClip>(new())));
        var gbx = Gbx.Parse<CGameCtnMediaClip>(input);

#pragma warning disable GBXNET10001
        var clone = gbx.DeepClone();
#pragma warning restore GBXNET10001

        await Assert.That(clone.Signature).IsEqualTo(gbx.Signature);
        await Assert.That(clone.SignatureStatus).IsEqualTo(GbxSignatureStatus.Valid);
        await Assert.That(clone.Body).IsNotSameReferenceAs(gbx.Body);
    }

    [Test]
    public async Task SignatureResult_ValidStatusWithoutMetadata_Throws()
    {
        await Assert.That(() => new GbxSignatureReadResult(GbxSignatureStatus.Valid)).Throws<ArgumentException>();
    }

    [Test]
    public async Task Save_TextBody_DoesNotAppendBinarySignature()
    {
        var gbx = new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = GbxCompression.Uncompressed };
        gbx.Header.Basic = gbx.Header.Basic with { Format = GbxFormat.Text };

        using var saved = new MemoryStream(Save(gbx));
        await Assert.That(GbxHeaderBasic.Parse(saved).Format).IsEqualTo(GbxFormat.Text);
        await Assert.That(saved.ToArray().AsSpan().IndexOf(BitConverter.GetBytes(ChunkId))).IsEqualTo(-1);
    }

    [Test]
    public async Task Write_UnchunkedBody_LeavesBodyUnchanged()
    {
        using var body = new MemoryStream();
        body.Write(new byte[64]);

        await Assert.That(GbxSignatureChunk.Write(body)).IsFalse();
        await Assert.That(body.ToArray()).IsEquivalentTo(new byte[64], CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed, false)]
    [Arguments(GbxCompression.Uncompressed, true)]
    [Arguments(GbxCompression.Compressed, false)]
    [Arguments(GbxCompression.Compressed, true)]
    public async Task Save_RawBody_PreservesBytesWithoutAddingSignature(GbxCompression compression, bool signed)
    {
        var file = signed
            ? Save(new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = compression })
            : CreateFile(BitConverter.GetBytes(0xFACADE01u), compression, 0x03079000u);
        using var input = new MemoryStream(file);
        var gbx = Gbx.ParseHeader(input, new() { ReadRawBody = true });

        var copied = Save(gbx);

        await Assert.That(gbx.SignatureStatus).IsEqualTo(GbxSignatureStatus.Unknown);
        await Assert.That(gbx.Signature).IsNull();
        await Assert.That(copied).IsEquivalentTo(file, CollectionOrdering.Matching);

        using var copiedInput = new MemoryStream(copied);
        await Assert.That(Gbx.Parse(copiedInput).SignatureStatus).IsEqualTo(signed ? GbxSignatureStatus.Valid : GbxSignatureStatus.Missing);
    }

    [Test]
    public async Task Save_StaleAttachedSignature_IsDiscardedWithoutMutatingNode()
    {
        var clip = new CGameCtnMediaClip();
        var gbx = new Gbx<CGameCtnMediaClip>(clip) { BodyCompression = GbxCompression.Uncompressed };
        var original = Save(gbx);
        var stale = new SkippableChunk(ChunkId) { Data = [1, 2, 3] };
        clip.Chunks.Add(stale);

        await GbxAssert.HaveEqualSavedBytes(original, Save(gbx));
        await Assert.That(clip.Chunks.Get(ChunkId)).IsSameReferenceAs(stale);
        using var signed = new MemoryStream(Save(gbx));
        await Assert.That(Gbx.Parse(signed).SignatureStatus).IsEqualTo(GbxSignatureStatus.Valid);
    }

    private static byte[] Save(Gbx gbx)
    {
        using var output = new MemoryStream();
        gbx.Save(output);
        return output.ToArray();
    }

    private static byte[] GetBody(byte[] file)
    {
        using var input = new MemoryStream(file);
        var gbx = Gbx.ParseHeader(input, new() { ReadRawBody = true });
        var data = gbx.Body.RawData.ToArray();
        if (gbx.Body.CompressedSize is null)
        {
            return data;
        }

        var body = new byte[gbx.Body.UncompressedSize];
        (Gbx.LZO ?? throw new LzoNotDefinedException()).Decompress(data, body);
        return body;
    }

    private static byte[] CreateSignedBody()
    {
        using var body = new MemoryStream();
        using var writer = new GbxWriter(body);
        writer.Write(new byte[] { 0xFF, 0xFE, 0xFD });
        writer.Write(0xFACADE01u);
        GbxSignatureChunk.Write(body);
        return body.ToArray();
    }

    private static byte[] CreateFile(byte[] body, GbxCompression compression, uint classId = 0x12345000)
    {
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        GbxHeaderBasic.Create(compressionOfBody: compression).Write(writer);
        writer.Write(classId);
        writer.Write(0); // User data size.
        writer.Write(1); // Root node.
        writer.Write(0); // External nodes.
        if (compression == GbxCompression.Compressed)
        {
            var compressed = (Gbx.LZO ?? throw new LzoNotDefinedException()).Compress(body);
            writer.Write(body.Length);
            writer.Write(compressed.Length);
            writer.Write(compressed);
        }
        else
        {
            writer.Write(body);
        }
        return output.ToArray();
    }
}
