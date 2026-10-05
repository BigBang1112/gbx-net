using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Text;

namespace GBX.NET.Tests.Unit;

public class CGameCtnMediaShootParamsLayoutTests
{
    [Test]
    [Arguments(0, false)]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    public async Task LegacyChunksConvertBooleanSettings(int chunkOffset, bool enabled)
    {
        var payload = Payload(w =>
        {
            Common(w);
            w.Write(enabled ? 1 : 0); // Legacy motion blur.
            w.Write(1); // Soft shadows.
            w.Write(0); // Ambient occlusion.
            w.Write(1); // Audio stream.
            w.Write(enabled ? (chunkOffset == 0 ? 1 : 2) : 0);
            if (chunkOffset == 0)
            {
                w.Write(0.25f); // Discarded native legacy floats.
                w.Write(-0.75f);
            }
        });
        var node = new CGameCtnMediaShootParams();
        Chunk<CGameCtnMediaShootParams> chunk = chunkOffset == 0
            ? new CGameCtnMediaShootParams.Chunk03060000()
            : new CGameCtnMediaShootParams.Chunk03060001();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.VideoFps).IsEqualTo(60);
        await Assert.That(node.SizeX).IsEqualTo(1280);
        await Assert.That(node.SizeY).IsEqualTo(720);
        await Assert.That(node.Hq).IsTrue();
        await Assert.That(node.HqSampleCountPerAxe).IsEqualTo(7);
        await Assert.That(node.MotionBlur).IsEqualTo(enabled
            ? CGameCtnMediaShootParams.EMotionBlur.Full
            : CGameCtnMediaShootParams.EMotionBlur.None);
        await Assert.That(node.Stereo3d).IsEqualTo(enabled
            ? CGameCtnMediaShootParams.EStereo3d.LeftNRight
            : CGameCtnMediaShootParams.EStereo3d.None);
        await Assert.That(node.HqSoftShadows).IsTrue();
        await Assert.That(node.HqAmbientOcc).IsFalse();
        await Assert.That(node.IsAudioStream).IsTrue();
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(0, 1)]
    [Arguments(1, 0)]
    [Arguments(1, 2)]
    [Arguments(1, 3)]
    [Arguments(1, 43)]
    public async Task CurrentChunkKeepsCaptureAndEncoderSettings(int version, int motionBlur)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            Common(w);
            w.Write(9); // DOF samples per axis.
            w.Write(motionBlur);
            w.Write(1); // DOF.
            w.Write(0); // Car reflections.
            w.Write(1); // Reflection subsampling.
            w.Write(0); // Reflection ray casting.
            w.Write(1); // Soft shadows.
            w.Write(0); // Ambient occlusion.
            w.Write(1); // Audio stream.
            w.Write(1); // Red-cyan stereo.
            w.Write(2); // JPG screenshots.
            w.Write(0); // AVI video.
            w.Write(1); // 3D HUD.
            w.Write(3); // High automatic bitrate preset.
            w.Write(0); // Independent video encoder version.
            w.Write(17); // Preserve an unknown codec enum value.
            w.Write(2); // CQ mode.
            w.Write(12345); // Bitrate in kbps.
            w.Write(23); // CQ quantizer.
            w.Write(0); // Independent audio encoder version.
            w.Write(0.875f); // Vorbis VBR quality.
        });
        var node = new CGameCtnMediaShootParams();
        var chunk = new CGameCtnMediaShootParams.Chunk03060002();

        await Assert.That(chunk.Version).IsEqualTo(1);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.TM2020);
        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That((int)node.MotionBlur).IsEqualTo(motionBlur);
        await Assert.That(node.DofSampleCount).IsEqualTo(9);
        await Assert.That(node.HqDOF).IsTrue();
        await Assert.That(node.HqCarReflects).IsFalse();
        await Assert.That(node.ReflectSubSample).IsTrue();
        await Assert.That(node.ReflectRayCast).IsFalse();
        await Assert.That(node.HqSoftShadows).IsTrue();
        await Assert.That(node.HqAmbientOcc).IsFalse();
        await Assert.That(node.IsAudioStream).IsTrue();
        await Assert.That(node.Stereo3d).IsEqualTo(CGameCtnMediaShootParams.EStereo3d.RedNCyan);
        await Assert.That(node.ExtScreen).IsEqualTo(CGameCtnMediaShootParams.EExtScreen.Jpg);
        await Assert.That(node.ExtVideo).IsEqualTo(CGameCtnMediaShootParams.EExtVideo.Avi);
        await Assert.That(node.Hud3d).IsTrue();
        await Assert.That(node.WebmVideoAutoBitrate).IsEqualTo(CGameCtnMediaShootParams.EQualityPreset.High);
        await Assert.That(node.VideoEncoding!.Version).IsEqualTo(0);
        await Assert.That((int)node.VideoEncoding.Codec).IsEqualTo(17);
        await Assert.That(node.VideoEncoding.Mode).IsEqualTo(CGameCtnMediaShootParams.EVideoMode.Cq);
        await Assert.That(node.VideoEncoding.Bitrate).IsEqualTo(12345);
        await Assert.That(node.VideoEncoding.CQLevel).IsEqualTo(23);
        await Assert.That(node.AudioEncoding!.Version).IsEqualTo(0);
        await Assert.That(node.AudioEncoding.VbrQuality).IsEqualTo(0.875f);
    }

    [Test]
    public async Task VideoDefaultsAndEncoderAliasesMatchNativeSettings()
    {
        var node = new CGameCtnMediaShootParams();

        await Assert.That(node.VideoFps).IsEqualTo(30);
        await Assert.That(node.SizeX).IsEqualTo(1920);
        await Assert.That(node.SizeY).IsEqualTo(1080);
        await Assert.That(node.Hq).IsTrue();
        await Assert.That(node.HqSampleCountPerAxe).IsEqualTo(1);
        await Assert.That(node.DofSampleCount).IsEqualTo(1);
        await Assert.That(node.HqCarReflects).IsTrue();
        await Assert.That(node.IsAudioStream).IsTrue();
        await Assert.That((int)node.ExtScreen).IsEqualTo(3);
        await Assert.That(node.ExtVideo).IsEqualTo(CGameCtnMediaShootParams.EExtVideo.Webm);
        await Assert.That(node.WebmVideoAutoBitrate).IsEqualTo(CGameCtnMediaShootParams.EQualityPreset.Medium);
        var video = node.VideoEncoding!;
        var audio = node.AudioEncoding!;
        await Assert.That(video.Codec).IsEqualTo(CGameCtnMediaShootParams.EVideoCodec.Vp8);
        await Assert.That(video.Mode).IsEqualTo(CGameCtnMediaShootParams.EVideoMode.Cq);
        await Assert.That(video.Bitrate).IsEqualTo(4096);
        await Assert.That(video.CQLevel).IsEqualTo(10);
        await Assert.That(audio.VbrQuality).IsEqualTo(0.1f);

#pragma warning disable CS0618
        video.U01 = 17;
        video.U02 = 43;
        video.U03 = 8192;
        video.U04 = 5;
        audio.U01 = 0.5f;
        await Assert.That((int)video.Codec).IsEqualTo(17);
        await Assert.That((int)video.Mode).IsEqualTo(43);
        await Assert.That(video.Bitrate).IsEqualTo(8192);
        await Assert.That(video.CQLevel).IsEqualTo(5);
        await Assert.That(audio.VbrQuality).IsEqualTo(0.5f);
        video.Codec = CGameCtnMediaShootParams.EVideoCodec.Vp9;
        video.Mode = CGameCtnMediaShootParams.EVideoMode.Vbr;
        video.Bitrate = 16384;
        video.CQLevel = 30;
        audio.VbrQuality = -0.1f;
        await Assert.That(video.U01).IsEqualTo(1);
        await Assert.That(video.U02).IsEqualTo(0);
        await Assert.That(video.U03).IsEqualTo(16384);
        await Assert.That(video.U04).IsEqualTo(30);
        await Assert.That(audio.U01).IsEqualTo(-0.1f);
#pragma warning restore CS0618
    }

    [Test]
    [Arguments(0)]
    [Arguments(7)]
    public async Task EncoderVersionsAreStoredIndependently(int version)
    {
        var videoPayload = Payload(w =>
        {
            w.Write(version);
            w.Write(17);
            w.Write(43);
            w.Write(8192);
            w.Write(30);
        });
        var audioPayload = Payload(w =>
        {
            w.Write(version);
            w.Write(-0.1f);
        });
        var video = new CGameCtnMediaShootParams.VideoEnc();
        var audio = new CGameCtnMediaShootParams.AudioEnc();

        await RoundTrip(videoPayload, rw => video.ReadWrite(rw, 1));
        await RoundTrip(audioPayload, rw => audio.ReadWrite(rw, 1));
        await Assert.That(video.Version).IsEqualTo(version);
        await Assert.That(audio.Version).IsEqualTo(version);
        await Assert.That((int)video.Mode).IsEqualTo(43);
        await Assert.That(audio.VbrQuality).IsEqualTo(-0.1f);
    }

    [Test]
    public async Task CloningKeepsModernModesAndCopiesEncoders()
    {
        var node = new CGameCtnMediaShootParams
        {
            MotionBlur = CGameCtnMediaShootParams.EMotionBlur.Quarter,
            Stereo3d = CGameCtnMediaShootParams.EStereo3d.RedNCyan,
            VideoEncoding = new() { Version = 7, Bitrate = 8192 },
            AudioEncoding = new() { Version = 3, VbrQuality = 0.5f }
        };
#pragma warning disable GBXNET10001
        var clone = (CGameCtnMediaShootParams)node.DeepClone();
#pragma warning restore GBXNET10001

        await Assert.That(clone.MotionBlur).IsEqualTo(node.MotionBlur);
        await Assert.That(clone.Stereo3d).IsEqualTo(node.Stereo3d);
        await Assert.That(clone.VideoEncoding).IsNotSameReferenceAs(node.VideoEncoding);
        await Assert.That(clone.AudioEncoding).IsNotSameReferenceAs(node.AudioEncoding);
        await Assert.That(clone.VideoEncoding!.Version).IsEqualTo(7);
        await Assert.That(clone.VideoEncoding.Bitrate).IsEqualTo(8192);
        await Assert.That(clone.AudioEncoding!.Version).IsEqualTo(3);
        clone.VideoEncoding.Bitrate = 1234;
        clone.AudioEncoding.VbrQuality = 0.75f;
        await Assert.That(node.VideoEncoding!.Bitrate).IsEqualTo(8192);
        await Assert.That(node.AudioEncoding!.VbrQuality).IsEqualTo(0.5f);
    }

    private static void Common(BinaryWriter writer)
    {
        writer.Write(60);
        writer.Write(1280);
        writer.Write(720);
        writer.Write(1); // Four-byte boolean.
        writer.Write(7);
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        const uint followingWord = 0xDEADBEEF;
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffixWriter = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffixWriter.Write(followingWord);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var readWrite = new GbxReaderWriter(reader);
        serialize(readWrite);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(followingWord);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        serialize(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
