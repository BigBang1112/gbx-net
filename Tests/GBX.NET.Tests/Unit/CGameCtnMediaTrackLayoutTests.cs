using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using System.Text;
using TmEssentials;

namespace GBX.NET.Tests.Unit;

public class CGameCtnMediaTrackLayoutTests
{
    [Test]
    [Arguments(0, -1)]
    [Arguments(1, -1)]
    [Arguments(1, 17)]
    public async Task NameAndBlocksKeepReferenceOrderAndLegacyFraming(int offset, int discardedWord)
    {
        var payload = Payload(w =>
        {
            String(w, "Track é");
            w.Write(10); // Deprecated list version.
            w.Write(3);
            w.Write(1); // First block reference.
            w.Write(0x030A6000u); // Music-effect block.
            w.Write(0x030A6000u); // Chunk 000.
            w.Write(0); // No keys.
            w.Write(0xFACADE01u);
            w.Write(-1); // Null block.
            w.Write(1); // First block again.
            if (offset == 1)
            {
                w.Write(discardedWord);
            }
        });
        var track = new CGameCtnMediaTrack();
        Chunk<CGameCtnMediaTrack> chunk = offset == 0
            ? new CGameCtnMediaTrack.Chunk03078000()
            : new CGameCtnMediaTrack.Chunk03078001();

        await Assert.That(track.Name).IsEqualTo("");
        await Assert.That(track.Blocks).IsEmpty();
        await RoundTrip(payload, rw => chunk.ReadWrite(track, rw));
        await Assert.That(track.Name).IsEqualTo("Track é");
        await Assert.That(track.Blocks.Count).IsEqualTo(3);
        await Assert.That(track.Blocks[0]).IsTypeOf<CGameCtnMediaBlockMusicEffect>();
        await Assert.That(track.Blocks[1]).IsNull();
        await Assert.That(track.Blocks[2]).IsSameReferenceAs(track.Blocks[0]);
        if (chunk is CGameCtnMediaTrack.Chunk03078001 legacy)
        {
            await Assert.That(legacy.U01).IsEqualTo(discardedWord);
        }
    }

    [Test]
    [Arguments(2, false)]
    [Arguments(2, true)]
    [Arguments(3, false)]
    [Arguments(3, true)]
    [Arguments(4, false)]
    [Arguments(4, true)]
    public async Task LegacyParametersUseFourByteBooleans(int offset, bool enabled)
    {
        var payload = Payload(w =>
        {
            w.Write(enabled ? 1 : 0);
            if (offset == 4)
            {
                w.Write(enabled ? 0 : 1);
            }
        });
        var track = new CGameCtnMediaTrack();
        Chunk<CGameCtnMediaTrack> chunk = offset switch
        {
            2 => new CGameCtnMediaTrack.Chunk03078002(),
            3 => new CGameCtnMediaTrack.Chunk03078003(),
            4 => new CGameCtnMediaTrack.Chunk03078004(),
            _ => throw new ArgumentOutOfRangeException(nameof(offset))
        };

        await RoundTrip(payload, rw => chunk.ReadWrite(track, rw));
        await Assert.That(track.IsKeepPlaying).IsEqualTo(offset != 3 && enabled);
        await Assert.That(track.IsReadOnly).IsEqualTo(offset == 3 ? enabled : offset == 4 && !enabled);
        await Assert.That(track.IsCycling).IsFalse();
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    [Arguments(2, true)]
    public async Task CyclingParametersKeepVersionBranchesAndNullSentinels(int version, bool hasTimes)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(1);
            w.Write(0);
            w.Write(1);
            if (version != 0)
            {
                w.Write(hasTimes ? 0.25f : -1f);
                w.Write(hasTimes ? 2.75f : -1f);
            }
        });
        var track = new CGameCtnMediaTrack();
        var chunk = new CGameCtnMediaTrack.Chunk03078005();

        await Assert.That(chunk.Version).IsEqualTo(1);
        await RoundTrip(payload, rw => chunk.ReadWrite(track, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(track.IsKeepPlaying).IsTrue();
        await Assert.That(track.IsReadOnly).IsFalse();
        await Assert.That(track.IsCycling).IsTrue();
        await Assert.That(track.RepeatingSegmentStart).IsEqualTo(hasTimes ? new TimeSingle(0.25f) : (TimeSingle?)null);
        await Assert.That(track.RepeatingSegmentEnd).IsEqualTo(hasTimes ? new TimeSingle(2.75f) : (TimeSingle?)null);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacyKeepPlayingIsForcedForMusicEffectsOnlyOnRead(bool savedValue)
    {
        var track = new CGameCtnMediaTrack { Blocks = [new CGameCtnMediaBlockMusicEffect()] };
        var chunk = new CGameCtnMediaTrack.Chunk03078002();
        using var input = new MemoryStream(Payload(w => w.Write(savedValue ? 1 : 0)));
        using var reader = new GbxReader(input);
        using var readWrite = new GbxReaderWriter(reader);
        chunk.ReadWrite(track, readWrite);
        await Assert.That(track.IsKeepPlaying).IsTrue();

        track.IsKeepPlaying = false;
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        chunk.ReadWrite(track, writeRead);
        await Assert.That(track.IsKeepPlaying).IsFalse();
        await Assert.That(output.ToArray()).IsEquivalentTo(new byte[4], CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(4)]
    [Arguments(5)]
    public async Task LaterParametersDoNotApplyTheLegacyMusicFixup(int offset)
    {
        var track = new CGameCtnMediaTrack { Blocks = [new CGameCtnMediaBlockMusicEffect()] };
        Chunk<CGameCtnMediaTrack> chunk = offset == 4
            ? new CGameCtnMediaTrack.Chunk03078004()
            : new CGameCtnMediaTrack.Chunk03078005();
        var payload = Payload(w =>
        {
            if (offset == 5)
            {
                w.Write(1);
            }
            w.Write(0);
            w.Write(0);
            if (offset == 5)
            {
                w.Write(0);
                w.Write(-1f);
                w.Write(-1f);
            }
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(track, rw));
        await Assert.That(track.IsKeepPlaying).IsFalse();
    }

    [Test]
    [Arguments(0x0307B000u, 0)]
    [Arguments(0x0307B000u, 1)]
    [Arguments(0x0307B000u, 2)]
    [Arguments(0x0307B000u, 3)]
    [Arguments(0x24062000u, 0)]
    [Arguments(0x24062000u, 1)]
    [Arguments(0x24062000u, 2)]
    [Arguments(0x24062000u, 3)]
    public async Task GhostTrackAliasesKeepTheirPayloadAndConvertTheTrack(uint chunkId, int ghostKind)
    {
        var payload = Payload(w =>
        {
            w.Write(chunkId);
            if (ghostKind == 0)
            {
                w.Write(-1);
            }
            else
            {
                w.Write(1);
                w.Write(0x03092000u);
                var nickname = Payload(metadata =>
                {
                    metadata.Write(0); // Skin descriptor count.
                    String(metadata, "Player");
                    String(metadata, "");
                });
                w.Write(0x03092017u);
                w.Write(0x534B4950u);
                w.Write(nickname.Length);
                w.Write(nickname);
                if (ghostKind >= 2)
                {
                    w.Write(0x0303F003u);
                    w.Write(0); // Empty vehicle-state data, kept lazy.
                    w.Write(2); // Sample offsets.
                    w.Write(0);
                    w.Write(0);
                    w.Write(ghostKind == 2 ? 1 : 2); // State-time count.
                    w.Write(500);
                    if (ghostKind == 3)
                    {
                        w.Write(1500);
                    }
                    w.Write(ghostKind == 2 ? 1 : 0); // Fixed time step.
                    w.Write(100); // Sample period in milliseconds.
                    w.Write(2); // Vehicle-state version.
                }
                w.Write(0xFACADE01u);
            }
            w.Write(0xFACADE01u);
        });
        var track = new CGameCtnMediaTrack();

        await RoundTrip(payload, rw =>
        {
            if (rw.Reader is not null)
            {
                track.Read(rw);
            }
            else
            {
                track.Write(rw);
            }
        });
        await Assert.That(track.Chunks.Single().Id).IsEqualTo(chunkId);
        await Assert.That(track.IsReadOnly).IsTrue();
        await Assert.That(track.IsKeepPlaying).IsFalse();
        await Assert.That(track.Blocks.Count).IsEqualTo(1);
        var block = (CGameCtnMediaBlockGhost)track.Blocks[0];
        await Assert.That(block.Start).IsEqualTo(TimeSingle.Zero);
        await Assert.That(block.End).IsEqualTo(new TimeSingle(ghostKind == 2 ? 0.7f : 1f));
        await Assert.That(track.Name).IsEqualTo(ghostKind == 0 ? "" : "Ghost:Player");
        if (ghostKind == 0)
        {
            await Assert.That(block.GhostModel).IsNull();
        }
        else if (ghostKind >= 2)
        {
            await Assert.That(block.GhostModel!.RawData!.Parsed).IsFalse();
        }
    }

    [Test]
    public async Task NativeDefaultsAndCloningKeepCyclingAndBlockIdentity()
    {
        var track = new CGameCtnMediaTrack();
        await Assert.That(track.IsKeepPlaying).IsFalse();
        await Assert.That(track.IsReadOnly).IsFalse();
        await Assert.That(track.IsCycling).IsFalse();
        await Assert.That(track.RepeatingSegmentStart).IsNull();
        await Assert.That(track.RepeatingSegmentEnd).IsNull();
        var block = new CGameCtnMediaBlockMusicEffect();
        track.Blocks = [block, block];
        track.RepeatingSegmentStart = new TimeSingle(0.25f);
        track.RepeatingSegmentEnd = new TimeSingle(2.75f);
#pragma warning disable GBXNET10001
        var clone = (CGameCtnMediaTrack)track.DeepClone();
#pragma warning restore GBXNET10001

        await Assert.That(clone.Blocks[0]).IsNotSameReferenceAs(block);
        await Assert.That(clone.Blocks[1]).IsSameReferenceAs(clone.Blocks[0]);
        await Assert.That(clone.RepeatingSegmentStart).IsEqualTo(track.RepeatingSegmentStart);
        await Assert.That(clone.RepeatingSegmentEnd).IsEqualTo(track.RepeatingSegmentEnd);
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
