using GBX.NET.Engines.Function;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using static GBX.NET.Engines.Plug.CPlugAudioBalance;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugAudioBalanceTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    public async Task ReadWrite_Mixer_PreservesVersionedPayload(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(14);
            for (var i = 0; i < 14; i++)
            {
                WriteLine(writer, version, 1 + i * 0.1f);
                WriteLine(writer, version, 3 + i * 0.1f);
            }
            if (version >= 1) writer.Write(0.4f);
            if (version >= 2) writer.Write(0.6f);
            if (version >= 6) writer.Write(0.8f);
            if (version >= 7)
            {
                writer.Write(0.1f);
                writer.Write(0.2f);
                writer.Write(0.3f);
                writer.Write(0.4f);
                writer.WriteNodeRef<CFuncKeysReal>(null);
                writer.WriteNodeRef<CFuncKeysReal>(null);
            }
            if (version >= 8)
            {
                writer.WriteNodeRef<CFuncKeysReal>(null);
                writer.WriteNodeRef<CFuncKeysReal>(null);
            }
            if (version >= 9) writer.WriteNodeRef<CFuncKeysReal>(null);
            if (version >= 10)
            {
                writer.Write(1.1f);
                writer.Write(1.2f);
                writer.Write(1.3f);
            }
            if (version >= 11) writer.Write(0.9f);
            if (version >= 12) writer.Write(0.7f);
            writer.Write(Sentinel);
        }

        var node = new CPlugAudioBalance();
        var chunk = new Chunk09034000();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Groups!.Length).IsEqualTo(14);
        await Assert.That(node.Groups[0].Others!.Volume).IsEqualTo(1f);
        await Assert.That(node.Groups[0].Focused!.Volume).IsEqualTo(3f);
        await Assert.That(node.Groups[0].Others!.Pitch).IsEqualTo(1.01f);
        await Assert.That(node.Groups[0].Others!.DopplerFactor).IsEqualTo(1.02f);
        await Assert.That(node.Groups[0].Others!.RolloffFactor).IsEqualTo(version >= 3 ? 1.03f : 1f);
        await Assert.That(node.Groups[0].Others!.DistanceFactor).IsEqualTo(version >= 6 ? 1.04f : 1f);
        await Assert.That(node.SceneVolume).IsEqualTo(version >= 1 ? 0.4f : 1f);
        await Assert.That(node.RolloffFactor).IsEqualTo(version >= 2 ? 0.6f : 1f);
        await Assert.That(node.RefDistanceFactor).IsEqualTo(version >= 6 ? 0.8f : 1f);
        await Assert.That(node.MaxDistFade_Ratio_Focused).IsEqualTo(version >= 7 ? 0.1f : 1f);
        await Assert.That(node.MaxDistFade_Rolloff_Focused).IsEqualTo(version >= 7 ? 0.2f : 1f);
        await Assert.That(node.MaxDistFade_Ratio_Others).IsEqualTo(version >= 7 ? 0.3f : 1f);
        await Assert.That(node.MaxDistFade_Rolloff_Others).IsEqualTo(version >= 7 ? 0.4f : 1f);
        await Assert.That(node.SceneLfeSend).IsEqualTo(version >= 10 ? 1.1f : 0f);
        await Assert.That(node.MusicLfeSend).IsEqualTo(version >= 10 ? 1.2f : 0f);
        await Assert.That(node.GlobalLfeSend).IsEqualTo(version >= 10 ? 1.3f : 0f);
        await Assert.That(node.AutoRadiusFactor).IsEqualTo(version >= 11 ? 0.9f : 0.15f);
        await Assert.That(node.UiVolume).IsEqualTo(version >= 12 ? 0.7f : 1f);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_Transitions_ConvertsLegacyMinimumDuration(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(0.25f);
            writer.Write(0.5f);
            writer.Write(0.75f);
            writer.Write(true);
            writer.Write(Sentinel);
        }

        var node = new CPlugAudioBalance();
        var chunk = new Chunk09034001();
        await Verify(payload.ToArray(), rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.AttackDuration).IsEqualTo(0.25f);
        await Assert.That(node.MinimumDuration).IsEqualTo(version == 0 ? 0.75f : 0.5f);
        await Assert.That(node.ReleaseDuration).IsEqualTo(0.75f);
        await Assert.That(node.IsAbsolute).IsTrue();
    }

    [Test]
    [Arguments(0)]
    [Arguments(3)]
    [Arguments(13)]
    public async Task Read_LegacyGroups_InheritsAmbianceForEnvironment(int count)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0);
            writer.Write(count);
            for (var i = 0; i < count; i++)
            {
                WriteLine(writer, 0, 1 + i * 0.1f);
                WriteLine(writer, 0, 3 + i * 0.1f);
            }
            writer.Write(Sentinel);
        }

        var node = new CPlugAudioBalance();
        var chunk = new Chunk09034000();
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Groups!.Length).IsEqualTo(14);
        var ambiance = node.Groups[(int)EAudioBalanceGroup.Ambiance - 1];
        var environment = node.Groups[(int)EAudioBalanceGroup.Environment - 1];
        await Assert.That(environment.Others!.Volume).IsEqualTo(count == 0 ? 1f : 1.2f);
        await Assert.That(environment.Focused!.Volume).IsEqualTo(count == 0 ? 1f : 3.2f);
        ambiance.Others!.Volume = 7;
        await Assert.That(environment.Others.Volume).IsEqualTo(count == 0 ? 1f : 1.2f);

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        output.Position = 0;
        var restored = new CPlugAudioBalance();
        using var outputReader = new GbxReader(output);
        await Assert.That(outputReader.ReadInt32()).IsEqualTo(0);
        await Assert.That(outputReader.ReadInt32()).IsEqualTo(14);
        output.Position = 0;
        using (var rw = new GbxReaderWriter(outputReader)) chunk.ReadWrite(restored, rw);
        await Assert.That(output.Position).IsEqualTo(output.Length);
        await GbxAssert.AreDeeplyEqual(node, restored);
    }

    [Test]
    public async Task Constructor_UsesNativeDefaults()
    {
        var node = new CPlugAudioBalance();
        await Assert.That(node.AttackDuration).IsEqualTo(0.3f);
        await Assert.That(node.MinimumDuration).IsEqualTo(0.5f);
        await Assert.That(node.ReleaseDuration).IsEqualTo(0.7f);
        await Assert.That(node.Groups!.Length).IsEqualTo(14);
        await Assert.That(node.Groups[(int)EAudioBalanceGroup.BackingDirect - 1].Others!.Volume).IsEqualTo(0f);
        await Assert.That(node.Groups[(int)EAudioBalanceGroup.OtherPlayers - 1].Focused!.Volume).IsEqualTo(0f);
        await Assert.That(node.Groups[(int)EAudioBalanceGroup.Music - 1].Focused!.Volume).IsEqualTo(1f);
        await Assert.That(node.Groups[(int)EAudioBalanceGroup.Music - 1].Others!.Volume).IsEqualTo(1f);
    }

    private static void WriteLine(GbxWriter writer, int version, float volume)
    {
        writer.Write(volume);
        writer.Write(volume + 0.01f);
        writer.Write(volume + 0.02f);
        if (version >= 3) writer.Write(volume + 0.03f);
        if (version >= 6) writer.Write(volume + 0.04f);
    }

    private static async Task Verify(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream(payload);
        using var reader = new GbxReader(input);
        using (var rw = new GbxReaderWriter(reader)) serialize(rw);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(input.Position).IsEqualTo(input.Length);
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using (var rw = new GbxReaderWriter(writer)) serialize(rw);
        writer.Write(Sentinel);
        await Assert.That(output.ToArray().SequenceEqual(payload)).IsTrue();
    }
}
