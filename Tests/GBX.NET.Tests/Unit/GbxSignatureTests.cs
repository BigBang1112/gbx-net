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
    private const uint ChunkId = 0xBB111200;

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
        if (result.Body.Signature is null)
        {
            throw new InvalidDataException("Expected signature metadata.");
        }
        var signature = result.Body.Signature;
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
        await Assert.That(BinaryPrimitives.ReadInt32LittleEndian(body.AsSpan(prefix.Length + 12))).IsEqualTo(1);
        await Assert.That(signature.ByteCount).IsEqualTo((long)body.Length - 36);
        await Assert.That(signature.Hash).IsEqualTo(new Checksum256(SHA256.HashData(body.AsSpan(0, (int)signature.ByteCount))));
        await Assert.That(BinaryPrimitives.ReadUInt32LittleEndian(body.AsSpan(body.Length - 4))).IsEqualTo(0xFACADE01u);

        using var fragmented = new FragmentedReadStream(signed);
        var parsed = await Gbx.ParseAsync<CGameCtnMediaClip>(fragmented);
        await Assert.That(((IGbx)parsed).Body.Signature).IsEqualTo(signature);
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
    public async Task Parse_ExposesSignatureMetadata(GbxCompression compression, bool async, bool fragmented)
    {
        var file = Save(new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = compression });
        using var expectedInput = new MemoryStream(file);
        var expected = Gbx.Parse(expectedInput);
        using Stream input = fragmented ? new FragmentedReadStream(file) : new MemoryStream(file);

        var gbx = async ? await Gbx.ParseAsync(input) : Gbx.Parse(input);

        await Assert.That(gbx.Body.Signature).IsNotNull();
        await Assert.That(gbx.Body.Signature).IsEqualTo(expected.Body.Signature);
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

        await Assert.That(gbx.Body.Signature).IsNotNull();
        await Assert.That(input.ReadByte()).IsEqualTo(42);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Parse_SkippedChunk_ReadsSignatureMetadataWithoutValidating(bool fragmented, bool tampered)
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

        await Assert.That(gbx.Body.Signature).IsNotNull();
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

        await Assert.That(gbx.Body.Signature).IsNull();
        await Assert.That(input.Position).IsEqualTo(position);
        await Assert.That(position < input.Length).IsTrue();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed, false)]
    [Arguments(GbxCompression.Uncompressed, true)]
    [Arguments(GbxCompression.Compressed, false)]
    [Arguments(GbxCompression.Compressed, true)]
    public async Task Parse_RetainedRawBody_HasNoSignatureMetadataAndPreservesBytes(GbxCompression compression, bool headerOnly)
    {
        var file = CreateFile(CreateSignedBody(), compression);
        using var input = new FragmentedReadStream(file);
        var gbx = headerOnly ? Gbx.ParseHeader(input, new() { ReadRawBody = true })
            : await Gbx.ParseAsync(input, new() { ReadRawBody = true });

        await Assert.That(gbx.Node).IsNull();
        await Assert.That(gbx.Body.Signature).IsNull();
        await Assert.That(Save(gbx)).IsEquivalentTo(file, CollectionOrdering.Matching);
    }

    [Test]
    public async Task Parse_MismatchedSignature_ReadsMetadataWithoutValidating()
    {
        var file = Save(new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = GbxCompression.Uncompressed });
        file[^5] ^= 1; // Last byte of the digest, before the root terminator.
        using var input = new MemoryStream(file);

        var gbx = Gbx.Parse<CGameCtnMediaClip>(input);

        await Assert.That(gbx.Body.Signature).IsNotNull();
        await Assert.That(gbx.Body.Exception).IsNull();
    }

    [Test]
    [Arguments(12, 2, GbxSignatureStatus.Unsupported)]
    [Arguments(16, -1, GbxSignatureStatus.Malformed)]
    [Arguments(16, int.MaxValue, GbxSignatureStatus.Malformed)]
    [Arguments(20, 0xFF, GbxSignatureStatus.Malformed)]
    public async Task ValidateSignature_InvalidSignatureMetadata_ReportsStatus(int offset, int value, GbxSignatureStatus expected)
    {
        var file = Save(new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = GbxCompression.Uncompressed });
        var body = GetBody(file);
        BinaryPrimitives.WriteInt32LittleEndian(file.AsSpan(file.Length - body.Length + offset), value);
        using var input = new MemoryStream(file);

        var result = Gbx.ValidateSignature(input);

        await Assert.That(result.Status).IsEqualTo(expected);
        await Assert.That(result.Signature).IsNull();
        await Assert.That(result.IsValid).IsFalse();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed)]
    [Arguments(GbxCompression.Compressed)]
    public async Task Parse_UnsignedArchive_HasNoSignatureMetadata(GbxCompression compression)
    {
        using var input = new MemoryStream(Save(new Gbx<CPlugStaticObjectModel>(new()) { BodyCompression = compression }));

        var gbx = Gbx.Parse(input);

        await Assert.That(gbx.Body.Signature).IsNull();
    }

    [Test]
    public async Task DeepClone_PreservesSignatureFromOriginalRead()
    {
        using var input = new MemoryStream(Save(new Gbx<CGameCtnMediaClip>(new())));
        var gbx = Gbx.Parse<CGameCtnMediaClip>(input);

#pragma warning disable GBXNET10001
        var clone = gbx.DeepClone();
#pragma warning restore GBXNET10001

        await Assert.That(gbx.Body.Signature).IsNotNull();
        await Assert.That(clone.Body.Signature).IsEqualTo(gbx.Body.Signature);
        await Assert.That(clone.Body).IsNotSameReferenceAs(gbx.Body);
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
    public async Task ValidateSignature_ValidBody_ReturnsVerifiedMetadata(GbxCompression compression, bool async, bool fragmented)
    {
        var file = Save(new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = compression });
        using var expectedInput = new MemoryStream(file);
        var expected = Gbx.Parse(expectedInput).Body.Signature;
        using Stream input = fragmented ? new FragmentedReadStream(file) : new MemoryStream(file);

        var result = async ? await Gbx.ValidateSignatureAsync(input) : Gbx.ValidateSignature(input);

        await Assert.That(result.Status).IsEqualTo(GbxSignatureStatus.Valid);
        await Assert.That(result.IsValid).IsTrue();
        await Assert.That(result.Signature).IsEqualTo(expected);
        await Assert.That(input.CanRead).IsTrue();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed, false)]
    [Arguments(GbxCompression.Uncompressed, true)]
    [Arguments(GbxCompression.Compressed, false)]
    [Arguments(GbxCompression.Compressed, true)]
    public async Task ValidateSignature_UnknownClass_VerifiesWithoutParsingNode(GbxCompression compression, bool async)
    {
        using var input = new FragmentedReadStream(CreateFile(CreateSignedBody(), compression));

        var result = async ? await Gbx.ValidateSignatureAsync(input) : Gbx.ValidateSignature(input);

        await Assert.That(result.Status).IsEqualTo(GbxSignatureStatus.Valid);
        await Assert.That(result.IsValid).IsTrue();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed, false)]
    [Arguments(GbxCompression.Uncompressed, true)]
    [Arguments(GbxCompression.Compressed, false)]
    [Arguments(GbxCompression.Compressed, true)]
    public async Task ValidateSignature_TamperedBody_ReportsMismatch(GbxCompression compression, bool async)
    {
        var body = CreateSignedBody();
        body[0] ^= 1;
        using var input = new FragmentedReadStream(CreateFile(body, compression));

        var result = async ? await Gbx.ValidateSignatureAsync(input) : Gbx.ValidateSignature(input);

        await Assert.That(result.Status).IsEqualTo(GbxSignatureStatus.Mismatched);
        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Signature).IsNull();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed, false)]
    [Arguments(GbxCompression.Uncompressed, true)]
    [Arguments(GbxCompression.Compressed, false)]
    [Arguments(GbxCompression.Compressed, true)]
    public async Task ValidateSignature_UnsignedBody_ReportsMissing(GbxCompression compression, bool async)
    {
        using var input = new MemoryStream(Save(new Gbx<CPlugStaticObjectModel>(new()) { BodyCompression = compression }));

        var result = async ? await Gbx.ValidateSignatureAsync(input) : Gbx.ValidateSignature(input);

        await Assert.That(result.Status).IsEqualTo(GbxSignatureStatus.Missing);
        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Signature).IsNull();
    }

    [Test]
    public async Task ValidateSignatureAsync_Cancelled_Throws()
    {
        using var input = new MemoryStream(Save(new Gbx<CGameCtnMediaClip>(new())));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.That(async () =>
        {
            await Gbx.ValidateSignatureAsync(input, cancellationToken: cancellation.Token);
        }).Throws<OperationCanceledException>();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ValidateSignature_FilePath_ReturnsVerifiedMetadata(bool async)
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllBytes(path, Save(new Gbx<CGameCtnMediaClip>(new())));

            var result = async ? await Gbx.ValidateSignatureAsync(path) : Gbx.ValidateSignature(path);

            await Assert.That(result.Status).IsEqualTo(GbxSignatureStatus.Valid);
            await Assert.That(result.IsValid).IsTrue();
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    public async Task SignatureResult_ValidStatusWithoutMetadata_Throws()
    {
        await Assert.That(() => new GbxSignatureValidationResult(GbxSignatureStatus.Valid)).Throws<ArgumentException>();
    }

    [Test]
    public async Task WriteUncompressed_TextBody_DoesNotAppendBinarySignature()
    {
        using var body = new MemoryStream();
        using var writer = new GbxWriter(body);
        writer.WriteFormat(GbxFormat.Text);

        new GbxBody().WriteUncompressed(new CGameCtnMediaClip(), writer);

        await Assert.That(body.ToArray().AsSpan().IndexOf(BitConverter.GetBytes(ChunkId))).IsEqualTo(-1);
    }

    [Test]
    public async Task Write_UnchunkedBody_LeavesBodyUnchanged()
    {
        using var body = new MemoryStream();
        body.Write(new byte[64]);
        using var writer = new GbxWriter(body);

        await Assert.That(GbxSignatureChunk.Write(writer)).IsFalse();
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

        await Assert.That(gbx.Body.Signature).IsNull();
        await Assert.That(copied).IsEquivalentTo(file, CollectionOrdering.Matching);

        using var copiedInput = new MemoryStream(copied);
        await Assert.That(Gbx.Parse(copiedInput).Body.Signature is not null).IsEqualTo(signed);
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
        await Assert.That(Gbx.Parse(signed).Body.Signature).IsNotNull();
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
        GbxSignatureChunk.Write(writer);
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
