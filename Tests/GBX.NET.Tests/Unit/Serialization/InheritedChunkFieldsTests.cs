using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.TrackMania;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class InheritedChunkFieldsTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ReadWrite_DerivedTextChunks_UseBaseTextProperties(int kind)
    {
        var source = CreateControl(kind);
        source.ClipLength = 12.5f;
        source.TextMode = 7;
        if (kind is 1 or 3) source.MaxLine = 17;
        var restored = CreateControl(kind);
        IReadableWritableChunk[] chunks = kind switch
        {
            0 => [new CControlButton.Chunk07007009(), new CControlButton.Chunk0700700A()],
            1 => [new CControlEntry.Chunk07009008()],
            2 => [new CControlEnum.Chunk0700A009()],
            3 => [new CControlLabel.Chunk07006006()],
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };

        await RoundTrip(source, restored, chunks);

        await Assert.That(restored.ClipLength).IsEqualTo(source.ClipLength);
        await Assert.That(restored.TextMode).IsEqualTo(source.TextMode);
        await Assert.That(restored.MaxLine).IsEqualTo(source.MaxLine);

        static CControlText CreateControl(int kind) => kind switch
        {
            0 => new CControlButton(),
            1 => new CControlEntry(),
            2 => new CControlEnum(),
            3 => new CControlLabel(),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };
    }

    [Test]
    public async Task ReadWrite_TrackManiaControlCard_UsesBaseUseDelays()
    {
        CGameControlCard source = new CTrackManiaControlCard();
        source.UseDelays = true;
        var restored = new CTrackManiaControlCard();

        await RoundTrip(source, restored, new CTrackManiaControlCard.Chunk2408F000());

        await Assert.That(((CGameControlCard)restored).UseDelays).IsTrue();
        await Assert.That(restored.UseDelays).IsTrue();
    }

    private static async Task RoundTrip(IClass source, IClass restored, params IReadableWritableChunk[] chunks)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer))
        {
            foreach (var chunk in chunks) chunk.ReadWrite(source, rw);
            writer.Write(0x12345678);
        }
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            foreach (var chunk in chunks) chunk.ReadWrite(restored, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
            await Assert.That(payload.Position).IsEqualTo(payload.Length);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            foreach (var chunk in chunks) chunk.ReadWrite(restored, rw);
            writer.Write(0x12345678);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }
}
