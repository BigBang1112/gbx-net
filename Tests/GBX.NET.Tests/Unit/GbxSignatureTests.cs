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
        var result = Gbx.ReadSignature(input);
        await Assert.That(result.Status).IsEqualTo(GbxSignatureStatus.Valid);
        if (!result.IsValid)
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
        using var unsignedInput = new MemoryStream(CreateUnknownFile(unsignedBody.ToArray(), compression));
        await Assert.That(Gbx.ReadSignature(unsignedInput).Status).IsEqualTo(GbxSignatureStatus.Missing);
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed)]
    [Arguments(GbxCompression.Compressed)]
    public async Task ReadSignature_UnknownRootAndPartialReads_MatchesSyncAndAsync(GbxCompression compression)
    {
        using var body = new MemoryStream();
        using (var writer = new GbxWriter(body))
        {
            writer.Write(new byte[] { 0xFF, 0xFE, 0xFD }); // No known node layout.
            writer.Write(0xFACADE01u);
        }
        GbxSignatureChunk.TryAppend(body);
        var file = CreateUnknownFile(body.ToArray(), compression);
        using var syncInput = new FragmentedReadStream(file);
        using var asyncInput = new FragmentedReadStream(file);

        var syncResult = Gbx.ReadSignature(syncInput);
        var asyncResult = await Gbx.ReadSignatureAsync(asyncInput);

        await Assert.That(syncResult.Status).IsEqualTo(GbxSignatureStatus.Valid);
        await Assert.That(asyncResult).IsEqualTo(syncResult);
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
        var expected = Gbx.ReadSignature(expectedInput);
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
    [Arguments(false)]
    [Arguments(true)]
    public async Task Parse_SkippedChunk_StillVerifiesCoveredBytes(bool fragmented)
    {
        const uint skippedId = 0x030790FE;
        var clip = new CGameCtnMediaClip();
        clip.Chunks.Add(new SkippableChunk(skippedId) { Data = [1, 2, 3, 4] });
        var file = Save(new Gbx<CGameCtnMediaClip>(clip) { BodyCompression = GbxCompression.Uncompressed });
        using Stream input = fragmented ? new FragmentedReadStream(file) : new MemoryStream(file);

        var gbx = Gbx.Parse(input, new() { SkipChunkIds = [skippedId] });

        await Assert.That(gbx.SignatureStatus).IsEqualTo(GbxSignatureStatus.Valid);
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
        await Assert.That(gbx.SignatureStatus).IsNull();
        await Assert.That(input.Position).IsEqualTo(position);
        await Assert.That(position < input.Length).IsTrue();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed, false)]
    [Arguments(GbxCompression.Uncompressed, true)]
    [Arguments(GbxCompression.Compressed, false)]
    [Arguments(GbxCompression.Compressed, true)]
    public async Task Parse_RetainedRawBody_ExposesSignatureForUnknownRoot(GbxCompression compression, bool headerOnly)
    {
        var file = CreateUnknownFile(CreateSignedBody(), compression);
        using var input = new FragmentedReadStream(file);
        var gbx = headerOnly ? Gbx.ParseHeader(input, new() { ReadRawBody = true })
            : await Gbx.ParseAsync(input, new() { ReadRawBody = true });

        await Assert.That(gbx.Node).IsNull();
        await Assert.That(gbx.SignatureStatus).IsEqualTo(GbxSignatureStatus.Valid);
        if (!gbx.HasSignature)
        {
            throw new InvalidDataException("Expected a verified signature on the Gbx object.");
        }
        await Assert.That(gbx.Signature.CommitHash).IsEqualTo(SimplestGit.CommitHash);
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
    public async Task HasSignature_InterfaceGuard_ProvidesNonNullMetadataWithoutAddingAChunk()
    {
        using var input = new MemoryStream(Save(new Gbx<CGameCtnMediaClip>(new())));
        IGbx gbx = Gbx.Parse(input);

        if (!gbx.HasSignature)
        {
            throw new InvalidDataException("Expected a verified signature.");
        }

        await Assert.That(gbx.Signature.CommitHash).IsEqualTo(SimplestGit.CommitHash);
        var node = gbx.Node ?? throw new InvalidDataException("Expected a parsed node.");
        await Assert.That(node.Chunks.Contains(ChunkId)).IsFalse();
    }

    [Test]
    public async Task SignatureResult_ValidStatusWithoutMetadata_Throws()
    {
        await Assert.That(() => new GbxSignatureReadResult(GbxSignatureStatus.Valid)).Throws<ArgumentException>();
    }

    [Test]
    [Arguments(0)] // Node payload.
    [Arguments(27)] // Source metadata.
    public async Task ReadSignature_Tampering_ReportsMismatched(int offset)
    {
        var body = CreateSignedBody();
        body[offset] ^= 1;
        using var input = new MemoryStream(CreateUnknownFile(body, GbxCompression.Uncompressed));

        var result = Gbx.ReadSignature(input);

        await Assert.That(result.Status).IsEqualTo(GbxSignatureStatus.Mismatched);
        await Assert.That(result.Signature).IsNull();
    }

    [Test]
    [Arguments(16, 2, GbxSignatureStatus.Unsupported)]
    [Arguments(8, 1, GbxSignatureStatus.Missing)] // Broken framing cannot identify a final signature.
    [Arguments(12, 0, GbxSignatureStatus.Malformed)]
    [Arguments(20, -1, GbxSignatureStatus.Malformed)]
    [Arguments(20, int.MaxValue, GbxSignatureStatus.Malformed)]
    public async Task ReadSignature_InvalidMetadata_ReportsStatus(int offset, int value, GbxSignatureStatus expected)
    {
        var body = CreateSignedBody();
        BinaryPrimitives.WriteInt32LittleEndian(body.AsSpan(3 + offset), value);
        using var input = new MemoryStream(CreateUnknownFile(body, GbxCompression.Uncompressed));

        var result = await Gbx.ReadSignatureAsync(input);

        await Assert.That(result.Status).IsEqualTo(expected);
        await Assert.That(result.Signature).IsNull();
    }

    [Test]
    public async Task ReadSignature_IncidentalChunkId_ReportsMissing()
    {
        using var body = new MemoryStream();
        using var writer = new GbxWriter(body);
        writer.Write(ChunkId);
        writer.Write(new byte[80]);
        writer.Write(0xFACADE01u);
        using var input = new MemoryStream(CreateUnknownFile(body.ToArray(), GbxCompression.Uncompressed));

        await Assert.That(Gbx.ReadSignature(input).Status).IsEqualTo(GbxSignatureStatus.Missing);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadSignature_StreamOwnership_HonorsCloseStream(bool closeStream)
    {
        using var input = new FragmentedReadStream(CreateUnknownFile(CreateSignedBody(), GbxCompression.Compressed));

        var result = await Gbx.ReadSignatureAsync(input, new() { CloseStream = closeStream });

        await Assert.That(result.Status).IsEqualTo(GbxSignatureStatus.Valid);
        await Assert.That(input.CanRead).IsEqualTo(!closeStream);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadSignature_BodyExceedsLimit_Throws(bool async)
    {
        var file = CreateUnknownFile(CreateSignedBody(), GbxCompression.Uncompressed);
        using var input = new FragmentedReadStream(file);
        var settings = new GbxReadSettings { MaxUncompressedBodySize = 16 };

        if (async)
        {
            await Assert.That(async () => await Gbx.ReadSignatureAsync(input, settings)).Throws<LengthLimitException>();
        }
        else
        {
            await Assert.That(() => Gbx.ReadSignature(input, settings)).Throws<LengthLimitException>();
        }
    }

    [Test]
    public async Task ReadSignature_MetadataExceedsLimit_ReportsMalformed()
    {
        using var input = new MemoryStream(CreateUnknownFile(CreateSignedBody(), GbxCompression.Uncompressed));

        var result = Gbx.ReadSignature(input, new() { MaxSkippableChunkSize = 16 });

        await Assert.That(result.Status).IsEqualTo(GbxSignatureStatus.Malformed);
    }

    [Test]
    public async Task ReadSignature_InvalidUtf8_ReportsMalformed()
    {
        var body = CreateSignedBody();
        body[27] = 0xFF;
        using var input = new MemoryStream(CreateUnknownFile(body, GbxCompression.Uncompressed));

        await Assert.That(Gbx.ReadSignature(input).Status).IsEqualTo(GbxSignatureStatus.Malformed);
    }

    [Test]
    public async Task ReadSignature_EmbeddedCompressedFile_ConsumesOnlyItsBody()
    {
        var file = CreateUnknownFile(CreateSignedBody(), GbxCompression.Compressed);
        using var input = new MemoryStream();
        input.Write(new byte[7]);
        input.Write(file);
        input.WriteByte(42);
        input.Position = 7;

        var result = await Gbx.ReadSignatureAsync(input);

        await Assert.That(result.Status).IsEqualTo(GbxSignatureStatus.Valid);
        await Assert.That(input.ReadByte()).IsEqualTo(42);
    }

    [Test]
    public async Task ReadSignature_FilePath_SyncAndAsyncAgreeAndReleaseFile()
    {
        var filePath = Path.Combine(Path.GetTempPath(), $"gbx-signature-{Guid.NewGuid():N}.Gbx");
        try
        {
            await File.WriteAllBytesAsync(filePath, CreateUnknownFile(CreateSignedBody(), GbxCompression.Compressed));

            var syncResult = Gbx.ReadSignature(filePath);
            var asyncResult = await Gbx.ReadSignatureAsync(filePath);

            await Assert.That(syncResult.Status).IsEqualTo(GbxSignatureStatus.Valid);
            await Assert.That(asyncResult).IsEqualTo(syncResult);
            using var exclusive = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
        }
        finally
        {
            File.Delete(filePath);
        }
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
    public async Task TryAppend_UnchunkedBody_LeavesBodyUnchanged()
    {
        using var body = new MemoryStream();
        body.Write(new byte[64]);

        await Assert.That(GbxSignatureChunk.TryAppend(body)).IsFalse();
        await Assert.That(body.ToArray()).IsEquivalentTo(new byte[64], CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed)]
    [Arguments(GbxCompression.Compressed)]
    public async Task Save_RawBody_PreservesSignatureAndBytes(GbxCompression compression)
    {
        var file = Save(new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = compression });
        using var input = new MemoryStream(file);
        var gbx = Gbx.ParseHeader(input, new() { ReadRawBody = true });

        var copied = Save(gbx);

        await Assert.That(copied).IsEquivalentTo(file, CollectionOrdering.Matching);
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
        await Assert.That(Gbx.ReadSignature(signed).Status).IsEqualTo(GbxSignatureStatus.Valid);
    }

    [Test]
    public async Task ReadSignatureAsync_PreCanceled_DoesNotReadAndHonorsOwnership()
    {
        using var input = new MemoryStream(CreateUnknownFile(CreateSignedBody(), GbxCompression.Uncompressed));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.That(async () => await Gbx.ReadSignatureAsync(input, cancellationToken: cancellation.Token))
            .Throws<OperationCanceledException>();
        await Assert.That(input.Position).IsEqualTo(0L);
        await Assert.That(input.CanRead).IsTrue();
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
        return gbx.Body.DecodeData(gbx.Body.RawData.ToArray());
    }

    private static byte[] CreateSignedBody()
    {
        using var body = new MemoryStream();
        using var writer = new GbxWriter(body);
        writer.Write(new byte[] { 0xFF, 0xFE, 0xFD });
        writer.Write(0xFACADE01u);
        GbxSignatureChunk.TryAppend(body);
        return body.ToArray();
    }

    private static byte[] CreateUnknownFile(byte[] body, GbxCompression compression)
    {
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        GbxHeaderBasic.Create(compressionOfBody: compression).Write(writer);
        writer.Write(0x12345000u);
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
