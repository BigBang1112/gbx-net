using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugAnimFileLayoutTests
{
    [Test]
    public async Task LegacyGraphState_ReadWrite_PreservesInlineClipAndTransition()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.WriteIdAsString("State");
            writer.Write(0); // Clip node type, followed by its inline payload.
            writer.WriteIdAsString("Idle");
            writer.Write(2); // Clip flags.
            writer.Write(-1); // No root motion node.
            writer.Write(1);
            writer.Write(100);
            writer.Write(3);
            writer.Write(0); // No selection conditions.
            writer.WriteIdAsString("");
            writer.Write(false);
            writer.Write(4);
            writer.Write(360f);
            writer.Write(0.25f);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var state = new CPlugAnimFile.LegacyGraphState();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            state.ReadWrite(rw, 12);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(state.Node!.ClipName).IsEqualTo("Idle");
        await Assert.That(state.Node.Flags).IsEqualTo(2);
        await Assert.That(state.RootMotionNode!.Type).IsEqualTo(-1);
        await Assert.That(state.TransitionDuration).IsEqualTo(100);
        await Assert.That(state.RootYawOffset).IsEqualTo(0.25f);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) state.ReadWrite(rw, 12);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }
}
