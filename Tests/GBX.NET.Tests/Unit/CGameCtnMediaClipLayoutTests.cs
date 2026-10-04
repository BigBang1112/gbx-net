using GBX.NET.Engines.Game;
using GBX.NET.Managers;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Text;

namespace GBX.NET.Tests.Unit;

public class CGameCtnMediaClipLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(5)]
    public async Task LegacyTracksPreserveOrderAndRepeatedReferences(int chunkOffset)
    {
        var payload = Payload(w =>
        {
            Tracks(w);
            if (chunkOffset != 0)
            {
                String(w, "Intro é");
            }
            if (chunkOffset == 2)
            {
                w.Write(1); // Discarded legacy bool32.
            }
        });
        var clip = new CGameCtnMediaClip();
        Chunk<CGameCtnMediaClip> chunk = chunkOffset switch
        {
            0 => new CGameCtnMediaClip.Chunk03079000(),
            1 => new CGameCtnMediaClip.Chunk03079001(),
            2 => new CGameCtnMediaClip.Chunk03079002(),
            3 => new CGameCtnMediaClip.Chunk03079003(),
            5 => new CGameCtnMediaClip.Chunk03079005(),
            _ => throw new ArgumentOutOfRangeException(nameof(chunkOffset))
        };

        await RoundTrip(payload, rw => chunk.ReadWrite(clip, rw));
        await AssertTracks(clip);
        await Assert.That(clip.Name).IsEqualTo(chunkOffset == 0 ? null : "Intro é");
        if (chunk is CGameCtnMediaClip.Chunk03079002 legacy)
        {
            await Assert.That(legacy.U01).IsTrue();
        }
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(0, 7)]
    [Arguments(1, 1)]
    [Arguments(1, 2)]
    [Arguments(1, 4)]
    [Arguments(1, 7)]
    public async Task ModernClipKeepsBooleanOrderAndSharedFields(int version, int booleans)
    {
        const string script = "// é\nSmShieldSizeScale = 0.5;";
        var payload = Payload(w =>
        {
            w.Write(version);
            Tracks(w);
            String(w, "In-game");
            w.Write(booleans & 1);
            w.Write((booleans >> 1) & 1);
            w.Write((booleans >> 2) & 1);
            String(w, script);
            w.Write(0.375f);
            w.Write(-1);
        });
        var clip = new CGameCtnMediaClip();
        var chunk = new CGameCtnMediaClip.Chunk0307900D();

        await Assert.That(chunk.Version).IsEqualTo(1);
        await Assert.That(clip.StereoSepMax).IsEqualTo(0.2f);
        await Assert.That(clip.ConfigScript).IsEqualTo(string.Empty);
        await Assert.That(clip.LocalPlayerClipEntIndex).IsEqualTo(-1);
        await RoundTrip(payload, rw => chunk.ReadWrite(clip, rw));
        await AssertTracks(clip);
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(clip.Name).IsEqualTo("In-game");
        await Assert.That(clip.StopWhenLeave).IsEqualTo((booleans & 1) != 0);
        await Assert.That(clip.IsScriptEvent).IsEqualTo((booleans & 2) != 0);
        await Assert.That(clip.StopWhenRespawn).IsEqualTo((booleans & 4) != 0);
        await Assert.That(clip.ConfigScript).IsEqualTo(script);
        await Assert.That(clip.StereoSepMax).IsEqualTo(0.375f);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacySettingsUseTheClipProperties(bool isScriptEvent)
    {
        var payload = Payload(w =>
        {
            w.Write(0.625f);
            String(w, "// Clip script é");
            w.Write(isScriptEvent ? 1 : 0);
        });
        var clip = new CGameCtnMediaClip();
        var stereo = new CGameCtnMediaClip.Chunk03079008();
        var script = new CGameCtnMediaClip.Chunk03079009();
        var scriptEvent = new CGameCtnMediaClip.Chunk0307900B();

        await RoundTrip(payload, rw =>
        {
            stereo.ReadWrite(clip, rw);
            script.ReadWrite(clip, rw);
            scriptEvent.ReadWrite(clip, rw);
        });
        await Assert.That(clip.StereoSepMax).IsEqualTo(0.625f);
        await Assert.That(clip.ConfigScript).IsEqualTo("// Clip script é");
        await Assert.That(clip.IsScriptEvent).IsEqualTo(isScriptEvent);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacyIdUsesLookbackStrings(bool hasId)
    {
        var payload = Payload(w =>
        {
            w.Write(3); // Lookback-string version.
            w.Write(hasId ? 0x40000000u : uint.MaxValue);
            if (hasId)
            {
                String(w, "LegacyClip");
            }
        });
        var clip = new CGameCtnMediaClip();
        var chunk = new CGameCtnMediaClip.Chunk03079006();

        await RoundTrip(payload, rw => chunk.ReadWrite(clip, rw));
        await Assert.That(chunk.U01).IsEqualTo(hasId ? "LegacyClip" : string.Empty);
    }

    [Test]
    [Arguments(0u)]
    [Arguments(0x0A002000u)]
    [Arguments(0x0A003000u)]
    public async Task LegacySceneAcceptsTheSceneBaseClass(uint sceneClassId)
    {
        var payload = Payload(w =>
        {
            w.Write(sceneClassId == 0 ? -1 : 1);
            if (sceneClassId != 0)
            {
                w.Write(sceneClassId);
                w.Write(0xFACADE01u); // End of scene node.
            }
        });
        var clip = new CGameCtnMediaClip();
        var chunk = new CGameCtnMediaClip.Chunk03079004();

        await RoundTrip(payload, rw => chunk.ReadWrite(clip, rw));
        await Assert.That(clip.Scene is null ? 0u : ClassManager.GetId(clip.Scene.GetType()) ?? 0).IsEqualTo(sceneClassId);
    }

    [Test]
    [Arguments(0, 1)]
    [Arguments(1, 0)]
    [Arguments(1, 1)]
    [Arguments(1, 2)]
    [Arguments(1, 3)]
    [Arguments(1, int.MinValue + 2)]
    [Arguments(1, int.MinValue + 3)]
    public async Task TriggerFlagsUseBitZeroAndPreserveOtherBits(int version, int flags)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(flags);
        });
        var clip = new CGameCtnMediaClip();
        var chunk = new CGameCtnMediaClip.Chunk0307900E();

        await Assert.That(chunk.Version).IsEqualTo(1);
        await Assert.That(clip.TriggersBeforeRaceStart).IsFalse();
        await RoundTrip(payload, rw => chunk.ReadWrite(clip, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(clip.Flags).IsEqualTo(flags);
        await Assert.That(clip.TriggersBeforeRaceStart).IsEqualTo((flags & 1) != 0);

        clip.TriggersBeforeRaceStart = true;
        await Assert.That(clip.Flags).IsEqualTo(flags | 1);
        clip.TriggersBeforeRaceStart = false;
        await Assert.That(clip.Flags).IsEqualTo(flags & ~1);
    }

    private static void Tracks(BinaryWriter writer)
    {
        writer.Write(10); // Deprecated-list version.
        writer.Write(3); // Two distinct tracks and a repeated reference.
        for (var i = 1; i <= 2; i++)
        {
            writer.Write(i);
            writer.Write(0x03078000u);
            writer.Write(0x03078001u);
            String(writer, i == 1 ? "B" : "A");
            writer.Write(10); // Deprecated block-list version.
            writer.Write(0); // No blocks.
            writer.Write(-1); // Legacy track value.
            writer.Write(0xFACADE01u);
        }
        writer.Write(1); // First track again.
    }

    private static async Task AssertTracks(CGameCtnMediaClip clip)
    {
        await Assert.That(clip.Tracks.Count).IsEqualTo(3);
        await Assert.That(clip.Tracks[0].Name).IsEqualTo("B");
        await Assert.That(clip.Tracks[1].Name).IsEqualTo("A");
        await Assert.That(clip.Tracks[2]).IsSameReferenceAs(clip.Tracks[0]);
    }

    private static void String(BinaryWriter writer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes.Length);
        writer.Write(bytes);
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
