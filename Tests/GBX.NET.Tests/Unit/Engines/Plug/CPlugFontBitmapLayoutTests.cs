using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugFontBitmapLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(2)]
    public async Task ReadWrite_KerningMap_OnlyNonemptyMapsHaveCapacity(int count)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(count);
            if (count > 0)
            {
                writer.Write(8);
                for (var index = 0; index < count; index++)
                {
                    writer.Write(0x00410056U + (uint)index);
                    writer.Write((short)(-2 - index));
                    writer.Write((short)(100 + index));
                }
            }
            writer.Write(0x11223344);
        }
        payload.Position = 0;
        var node = new CPlugFontBitmap();
        var chunk = new CPlugFontBitmap.Chunk0904E005();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.Kernings?.Length ?? 0).IsEqualTo(count);
        if (count > 0)
        {
            await Assert.That(node.KerningCapacity).IsEqualTo(8);
            await Assert.That(node.Kernings![1].Adjustment).IsEqualTo((short)-3);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..^4], CollectionOrdering.Matching);
    }
}
