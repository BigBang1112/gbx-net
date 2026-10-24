using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;
using GBX.NET.Tests.Mocks;
using SimplestGitSourceGenerator;
using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class GbxSignatureTests
{
    [Test]
    [Arguments(GbxCompression.Uncompressed, false)]
    [Arguments(GbxCompression.Uncompressed, true)]
    [Arguments(GbxCompression.Compressed, false)]
    [Arguments(GbxCompression.Compressed, true)]
    public async Task TryReadSignature_SavedFile_ReturnsVerifiedMetadata(GbxCompression compression, bool fragmented)
    {
        using var saved = new MemoryStream();
        var beforeSave = DateTimeOffset.UtcNow;
        new Gbx<CGameCtnMediaClip>(new()) { BodyCompression = compression }.Save(saved);
        var afterSave = DateTimeOffset.UtcNow;
        using Stream input = fragmented
            ? new FragmentedReadStream(saved.ToArray())
            : new MemoryStream(saved.ToArray());

        var success = Gbx.TryReadSignature(input, out var signature, out var status);

        await Assert.That(success).IsTrue();
        await Assert.That(status).IsEqualTo(GbxSignatureStatus.Valid);
        await Assert.That(signature).IsNotNull();
        await Assert.That(signature!.Version).IsEqualTo(1);
        await Assert.That(signature.CommitHash).IsEqualTo(SimplestGit.CommitHash);
        await Assert.That(signature.Tag).IsEqualTo(SimplestGit.Tag);
        await Assert.That(signature.Timestamp.ToUnixTimeMilliseconds() >= beforeSave.ToUnixTimeMilliseconds()
            && signature.Timestamp.ToUnixTimeMilliseconds() <= afterSave.ToUnixTimeMilliseconds()).IsTrue();
        await Assert.That(signature.ByteCount).IsEqualTo(44L + Encoding.UTF8.GetByteCount(SimplestGit.CommitHash)
            + Encoding.UTF8.GetByteCount(SimplestGit.Tag));
        await Assert.That(signature.Hash).IsNotEqualTo(Checksum256.Zero);
        await Assert.That(input.CanRead).IsTrue();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed, "", "")]
    [Arguments(GbxCompression.Compressed, "commit", "v1-é-雪")]
    public async Task TryReadSignature_UnknownClassAndOpaqueBody_ReadsMetadataWithoutParsingNode(GbxCompression compression, string commit, string tag)
    {
        var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(1760000000123);
        var body = CreateBody(commit, tag, timestamp);
        using var input = new MemoryStream(CreateFile(body, compression));

        var success = Gbx.TryReadSignature(input, out var signature);

        await Assert.That(success).IsTrue();
        await Assert.That(signature!.CommitHash).IsEqualTo(commit);
        await Assert.That(signature.Tag).IsEqualTo(tag);
        await Assert.That(signature.Timestamp).IsEqualTo(timestamp);
        await Assert.That(signature.ByteCount).IsEqualTo((long)(body.Length - 36));
        await Assert.That(signature.Hash).IsEqualTo(new Checksum256(SHA256.HashData(body.AsSpan(0, body.Length - 36))));
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed)]
    [Arguments(GbxCompression.Compressed)]
    public async Task TryReadSignature_UnsignedBody_ReturnsFalseAndNull(GbxCompression compression)
    {
        using var input = new FragmentedReadStream(CreateFile(BitConverter.GetBytes(0xFACADE01u), compression));

        var success = Gbx.TryReadSignature(input, out var signature, out var status);

        await Assert.That(success).IsFalse();
        await Assert.That(status).IsEqualTo(GbxSignatureStatus.Missing);
        await Assert.That(signature).IsNull();
    }

    [Test]
    [Arguments("chunkId", GbxSignatureStatus.Missing)]
    [Arguments("skip", GbxSignatureStatus.Malformed)]
    [Arguments("size", GbxSignatureStatus.Malformed)]
    [Arguments("magic", GbxSignatureStatus.Malformed)]
    [Arguments("version", GbxSignatureStatus.Unsupported)]
    [Arguments("negativeStringLength", GbxSignatureStatus.Malformed)]
    [Arguments("oversizedStringLength", GbxSignatureStatus.Malformed)]
    [Arguments("invalidUtf8", GbxSignatureStatus.Malformed)]
    [Arguments("tagLength", GbxSignatureStatus.Malformed)]
    [Arguments("timestamp", GbxSignatureStatus.Malformed)]
    [Arguments("byteCount", GbxSignatureStatus.Malformed)]
    [Arguments("facade", GbxSignatureStatus.Malformed)]
    public async Task TryReadSignature_DamagedMetadataWithMatchingHash_ReportsStatus(string field, GbxSignatureStatus expectedStatus)
    {
        var body = CreateBody("commit", "tag", DateTimeOffset.UnixEpoch);
        // The body starts with five opaque bytes, followed by the validation chunk.
        const int chunkStart = 5;
        switch (field)
        {
            case "chunkId": body[chunkStart] ^= 1; break;
            case "skip": body[chunkStart + 4] ^= 1; break;
            case "size": body[chunkStart + 8] ^= 1; break;
            case "magic": body[chunkStart + 12] ^= 1; break;
            case "version": BinaryPrimitives.WriteInt32LittleEndian(body.AsSpan(chunkStart + 16), 2); break;
            case "negativeStringLength": BinaryPrimitives.WriteInt32LittleEndian(body.AsSpan(chunkStart + 20), -1); break;
            case "oversizedStringLength": BinaryPrimitives.WriteInt32LittleEndian(body.AsSpan(chunkStart + 20), int.MaxValue); break;
            case "invalidUtf8": body[chunkStart + 24] = 0xFF; break;
            case "tagLength": BinaryPrimitives.WriteInt32LittleEndian(body.AsSpan(chunkStart + 30), 4); break;
            case "timestamp": BinaryPrimitives.WriteInt64LittleEndian(body.AsSpan(body.Length - 52), long.MaxValue); break;
            case "byteCount": BinaryPrimitives.WriteInt64LittleEndian(body.AsSpan(body.Length - 44), -1); break;
            case "facade": body[^1] ^= 1; break;
        }
        SHA256.HashData(body.AsSpan(0, body.Length - 36)).CopyTo(body, body.Length - 36);
        using var input = new MemoryStream(CreateFile(body, GbxCompression.Uncompressed));

        var success = Gbx.TryReadSignature(input, out var signature, out var status);

        await Assert.That(success).IsFalse();
        await Assert.That(status).IsEqualTo(expectedStatus);
        await Assert.That(signature).IsNull();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed, false)]
    [Arguments(GbxCompression.Uncompressed, true)]
    [Arguments(GbxCompression.Compressed, false)]
    [Arguments(GbxCompression.Compressed, true)]
    public async Task TryReadSignature_TamperedBodyOrDigest_ReturnsFalseAndNull(GbxCompression compression, bool digest)
    {
        var body = CreateBody("commit", "tag", DateTimeOffset.UnixEpoch);
        body[digest ? body.Length - 36 : 0] ^= 1;
        using var input = new MemoryStream(CreateFile(body, compression));

        var success = Gbx.TryReadSignature(input, out var signature, out var status);

        await Assert.That(success).IsFalse();
        await Assert.That(status).IsEqualTo(GbxSignatureStatus.Mismatched);
        await Assert.That(signature).IsNull();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(35)]
    [Arguments(52)]
    public async Task TryReadSignature_TruncatedOrUnrelatedData_ReturnsFalseAndNull(int length)
    {
        var file = CreateFile(CreateBody("commit", "tag", DateTimeOffset.UnixEpoch), GbxCompression.Uncompressed);
        using var input = new FragmentedReadStream(file[..length]);

        var success = Gbx.TryReadSignature(input, out var signature, out var status);

        await Assert.That(success).IsFalse();
        await Assert.That(status).IsEqualTo(GbxSignatureStatus.Malformed);
        await Assert.That(signature).IsNull();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task TryReadSignature_CloseStreamSetting_HonorsStreamOwnership(bool closeStream)
    {
        using var input = new MemoryStream(CreateFile(CreateBody("commit", "tag", DateTimeOffset.UnixEpoch), GbxCompression.Uncompressed));

        var success = Gbx.TryReadSignature(input, out _, new() { CloseStream = closeStream });

        await Assert.That(success).IsTrue();
        await Assert.That(input.CanRead).IsEqualTo(!closeStream);
    }

    [Test]
    public async Task TryReadSignature_StreamWithPrefix_ReadsFromCurrentPosition()
    {
        var file = CreateFile(CreateBody("commit", "tag", DateTimeOffset.UnixEpoch), GbxCompression.Uncompressed);
        using var input = new MemoryStream();
        input.Write(new byte[17]);
        input.Write(file);
        input.Position = 17;

        var success = Gbx.TryReadSignature(input, out var signature);

        await Assert.That(success).IsTrue();
        await Assert.That(signature!.CommitHash).IsEqualTo("commit");
    }

    [Test]
    public async Task TryReadSignature_FilePath_ReadsMetadataAndReleasesFile()
    {
        var path = Path.Combine(Path.GetTempPath(), $"gbx-signature-{Guid.NewGuid():N}.Gbx");
        try
        {
            File.WriteAllBytes(path, CreateFile(CreateBody("commit", "tag", DateTimeOffset.UnixEpoch), GbxCompression.Compressed));

            var success = Gbx.TryReadSignature(path, out var signature, out var status);

            await Assert.That(success).IsTrue();
            await Assert.That(status).IsEqualTo(GbxSignatureStatus.Valid);
            await Assert.That(signature!.CommitHash).IsEqualTo("commit");
            var asyncResult = await Gbx.ReadSignatureAsync(path);
            await Assert.That(asyncResult).IsEqualTo(Gbx.ReadSignature(path));
            await Assert.That(asyncResult.Signature).IsEqualTo(signature);
            using var exclusive = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task TryReadSignature_UncompressedBodyExceedsLimit_ThrowsLengthLimitException(bool fragmented)
    {
        var file = CreateFile(CreateBody("commit", "tag", DateTimeOffset.UnixEpoch), GbxCompression.Uncompressed);
        using Stream input = fragmented ? new FragmentedReadStream(file) : new MemoryStream(file);

        await Assert.That(() => Gbx.TryReadSignature(input, out _, new() { MaxUncompressedBodySize = 16 }))
            .Throws<LengthLimitException>();
    }

    [Test]
    public async Task TryReadSignature_MetadataExceedsLimit_ReturnsFalseAndNull()
    {
        using var input = new MemoryStream(CreateFile(CreateBody("commit", "tag", DateTimeOffset.UnixEpoch), GbxCompression.Uncompressed));

        var success = Gbx.TryReadSignature(input, out var signature, out var status, new() { MaxSkippableChunkSize = 64 });

        await Assert.That(success).IsFalse();
        await Assert.That(status).IsEqualTo(GbxSignatureStatus.Malformed);
        await Assert.That(signature).IsNull();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed)]
    [Arguments(GbxCompression.Compressed)]
    public async Task TryReadSignature_UnknownVersionWithDifferentLayout_ReportsUnsupported(GbxCompression compression)
    {
        using var body = new MemoryStream();
        using (var writer = new GbxWriter(body))
        {
            writer.Write(0x01112000u);
            writer.Write(0x534B4950u);
            writer.Write(8);
            writer.Write(0x4E584247u);
            writer.Write(2);
            writer.Write(0xFACADE01u);
        }
        using var input = new FragmentedReadStream(CreateFile(body.ToArray(), compression));

        var success = Gbx.TryReadSignature(input, out var signature, out var status);

        await Assert.That(success).IsFalse();
        await Assert.That(status).IsEqualTo(GbxSignatureStatus.Unsupported);
        await Assert.That(signature).IsNull();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task TryReadSignature_MissingSignature_HonorsStreamOwnership(bool closeStream)
    {
        using var input = new MemoryStream(CreateFile(BitConverter.GetBytes(0xFACADE01u), GbxCompression.Uncompressed));

        var success = Gbx.TryReadSignature(input, out var signature, out var status, new() { CloseStream = closeStream });

        await Assert.That(success).IsFalse();
        await Assert.That(status).IsEqualTo(GbxSignatureStatus.Missing);
        await Assert.That(signature).IsNull();
        await Assert.That(input.CanRead).IsEqualTo(!closeStream);
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed, GbxSignatureStatus.Valid)]
    [Arguments(GbxCompression.Uncompressed, GbxSignatureStatus.Missing)]
    [Arguments(GbxCompression.Uncompressed, GbxSignatureStatus.Malformed)]
    [Arguments(GbxCompression.Uncompressed, GbxSignatureStatus.Unsupported)]
    [Arguments(GbxCompression.Uncompressed, GbxSignatureStatus.Mismatched)]
    [Arguments(GbxCompression.Compressed, GbxSignatureStatus.Valid)]
    [Arguments(GbxCompression.Compressed, GbxSignatureStatus.Missing)]
    [Arguments(GbxCompression.Compressed, GbxSignatureStatus.Malformed)]
    [Arguments(GbxCompression.Compressed, GbxSignatureStatus.Unsupported)]
    [Arguments(GbxCompression.Compressed, GbxSignatureStatus.Mismatched)]
    public async Task ReadSignatureAsync_PartialReads_MatchesGeneratedSyncResult(GbxCompression compression, GbxSignatureStatus expectedStatus)
    {
        var body = CreateBody("commit", "tag", DateTimeOffset.UnixEpoch);
        switch (expectedStatus)
        {
            case GbxSignatureStatus.Missing: body = BitConverter.GetBytes(0xFACADE01u); break;
            case GbxSignatureStatus.Malformed: body[5 + 8] ^= 1; break;
            case GbxSignatureStatus.Unsupported: BinaryPrimitives.WriteInt32LittleEndian(body.AsSpan(5 + 16), 2); break;
            case GbxSignatureStatus.Mismatched: body[0] ^= 1; break;
        }
        var file = CreateFile(body, compression);
        using var input = new FragmentedReadStream(file);
        using var syncInput = new FragmentedReadStream(file);

        var result = await Gbx.ReadSignatureAsync(input);
        var syncResult = Gbx.ReadSignature(syncInput);

        await Assert.That(result.Status).IsEqualTo(expectedStatus);
        await Assert.That(result).IsEqualTo(syncResult);
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed)]
    [Arguments(GbxCompression.Compressed)]
    public async Task ReadSignatureAsync_Body_UsesAsyncReadsAndPassesCancellationToken(GbxCompression compression)
    {
        using var cancellation = new CancellationTokenSource();
        using var input = new AsyncReadTrackingStream(CreateFile(CreateBody("commit", "tag", DateTimeOffset.UnixEpoch), compression));

        var result = await Gbx.ReadSignatureAsync(input, cancellationToken: cancellation.Token);

        await Assert.That(result.Status).IsEqualTo(GbxSignatureStatus.Valid);
        await Assert.That(input.AsyncReadCount).IsGreaterThan(0);
        await Assert.That(input.LastCancellationToken).IsEqualTo(cancellation.Token);
        await Assert.That(input.CanRead).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadSignatureAsync_PreCanceled_ThrowsWithoutReadingAndHonorsStreamOwnership(bool closeStream)
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        using var input = new AsyncReadTrackingStream(CreateFile(CreateBody("commit", "tag", DateTimeOffset.UnixEpoch), GbxCompression.Uncompressed));

        await Assert.That(async () => await Gbx.ReadSignatureAsync(input, new() { CloseStream = closeStream }, cancellation.Token))
            .Throws<OperationCanceledException>();

        await Assert.That(input.AsyncReadCount).IsEqualTo(0);
        await Assert.That(input.CanRead).IsEqualTo(!closeStream);
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed)]
    [Arguments(GbxCompression.Compressed)]
    public async Task ReadSignatureAsync_CanceledDuringBodyRead_ThrowsInsteadOfReturningSignatureStatus(GbxCompression compression)
    {
        using var cancellation = new CancellationTokenSource();
        using var input = new AsyncReadTrackingStream(CreateFile(CreateBody("commit", "tag", DateTimeOffset.UnixEpoch), compression), cancellation);

        await Assert.That(async () => await Gbx.ReadSignatureAsync(input, cancellationToken: cancellation.Token))
            .Throws<OperationCanceledException>();

        await Assert.That(input.AsyncReadCount).IsGreaterThan(0);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadSignatureAsync_CloseStreamSetting_HonorsStreamOwnership(bool closeStream)
    {
        using var input = new FragmentedReadStream(CreateFile(CreateBody("commit", "tag", DateTimeOffset.UnixEpoch), GbxCompression.Uncompressed));

        var result = await Gbx.ReadSignatureAsync(input, new() { CloseStream = closeStream });

        await Assert.That(result.Status).IsEqualTo(GbxSignatureStatus.Valid);
        await Assert.That(input.CanRead).IsEqualTo(!closeStream);
    }

    [Test]
    public async Task ReadSignatureAsync_UncompressedBodyExceedsLimit_ThrowsLengthLimitException()
    {
        using var input = new FragmentedReadStream(CreateFile(CreateBody("commit", "tag", DateTimeOffset.UnixEpoch), GbxCompression.Uncompressed));

        await Assert.That(async () => await Gbx.ReadSignatureAsync(input, new() { MaxUncompressedBodySize = 16 }))
            .Throws<LengthLimitException>();
    }

    [Test]
    public async Task ReadSignatureAsync_PreCanceledFilePath_ThrowsBeforeOpeningFile()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var missingPath = Path.Combine(Path.GetTempPath(), $"gbx-signature-{Guid.NewGuid():N}.Gbx");

        await Assert.That(async () => await Gbx.ReadSignatureAsync(missingPath, cancellationToken: cancellation.Token))
            .Throws<OperationCanceledException>();
    }

    private static byte[] CreateBody(string commit, string tag, DateTimeOffset timestamp)
    {
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        writer.Write(new byte[] { 0xFF, 0xFE, 0xFD, 0xFC, 0xFB });
        writer.Write(0x01112000u);
        writer.Write(0x534B4950u);
        writer.Write(64 + Encoding.UTF8.GetByteCount(commit) + Encoding.UTF8.GetByteCount(tag));
        writer.Write(0x4E584247u);
        writer.Write(1);
        writer.Write(commit);
        writer.Write(tag);
        writer.Write(timestamp.ToUnixTimeMilliseconds());
        writer.Write(output.Position + 8);
        writer.Write(SHA256.HashData(output.ToArray()));
        writer.Write(0xFACADE01u);
        return output.ToArray();
    }

    private static byte[] CreateFile(byte[] body, GbxCompression compression)
    {
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        GbxHeaderBasic.Create(compressionOfBody: compression).Write(writer);
        writer.Write(0x12345000u); // Unknown class, with no user data or external references.
        writer.Write(0);
        writer.Write(1);
        writer.Write(0);
        if (compression == GbxCompression.Compressed)
        {
            var compressed = Gbx.LZO!.Compress(body);
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
