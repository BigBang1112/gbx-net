using GBX.NET.Engines.Game;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class GbxWriterExternalArrayTests
{
    [Test]
    [Arguments(-1, 0)]
    [Arguments(-1, 2)]
    [Arguments(0, 2)]
    [Arguments(1, 3)]
    [Arguments(3, 1)]
    public async Task WriteArrayExternalNodeRef_WithFixedLengthWritesExactlyThatManyReferences(int count, int length)
    {
        var clips = count < 0 ? null : Enumerable.Range(0, count)
            .Select(_ => new External<CGameCtnBlockInfoClip>(null, null)).ToArray();
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.WriteArrayExternalNodeRef(clips, length);
            writer.Write(0x11223344);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        for (var i = 0; i < length; i++)
        {
            await Assert.That(reader.ReadInt32()).IsEqualTo(-1);
        }
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    public async Task WriteArrayExternalNodeRef_WithNegativeLengthThrowsBeforeWriting()
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        Assert.Throws<ArgumentOutOfRangeException>(() => writer.WriteArrayExternalNodeRef<CGameCtnBlockInfoClip>(null, -1));
        await Assert.That(stream.Length).IsEqualTo(0);
    }
}
