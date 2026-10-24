using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Control;

[Category("Unit")]
public class CControlBaseLayoutTests
{
    [Test]
    [Arguments(typeof(CControlButton), true)]
    [Arguments(typeof(CControlQuad), false)]
    [Arguments(typeof(CControlFrame), false)]
    [Arguments(typeof(CGameControlCardGeneric), true)]
    [Arguments(typeof(CGamePlaygroundControlScores), true)]
    [Arguments(typeof(CGamePlaygroundControlMessages), true)]
    public async Task ReadWrite_LegacyControl_UsesStackCapabilityOfRuntimeClass(Type controlType, bool hasStack)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0U);
            writer.WriteNodeRef<CControlStyle>(null);
            writer.WriteNodeRef<CMwNod>(null);
            if (hasStack)
            {
                writer.WriteNodeRef<CMwNod>(null);
                writer.Write("Stack");
            }
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = (CControlBase)Activator.CreateInstance(controlType)!;
        var chunk = new CControlBase.Chunk0700100C();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.StackText).IsEqualTo(hasStack ? "Stack" : null);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }
}
