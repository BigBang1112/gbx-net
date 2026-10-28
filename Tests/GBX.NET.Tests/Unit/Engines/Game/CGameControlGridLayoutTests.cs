using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameControlGridLayoutTests
{
    [Test]
    [Arguments(-1)]
    [Arguments(-2)]
    public async Task ReadWrite_LocalDataPointer_PreservesNullAndInlineReferenceMarkers(int index)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(index);
            if (index == -2) writer.Write(-1);
            writer.Write(true);
            writer.Write(false);
            writer.Write(0x11223344);
        }
        payload.Position = 0;
        var node = new CGameControlGrid();
        var chunk = new CGameControlGrid.Chunk030B9005();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.BaseLocalDataPointer.Index).IsEqualTo(index);
        await Assert.That(node.HaveLocalData).IsTrue();
        await Assert.That(node.HideLocalDataIfNone).IsFalse();
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..^4], CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_LocalDataPointer_ReusesSharedControlModelInstances()
    {
        var instance = new CControlContainer.ModelInstance
        {
            Model = new CControlBase(),
            Data = new CControlContainer.InstanceData { Version = 2, Name = "Instance" }
        };
        var first = new CGameControlGrid { HaveLocalData = true };
        var second = new CGameControlGrid { HideLocalDataIfNone = false };
        first.BaseLocalDataPointer.Instance = instance;
        second.BaseLocalDataPointer.Instance = instance;
        var chunk = new CGameControlGrid.Chunk030B9005();
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(first, rw);
            chunk.ReadWrite(second, rw);
            writer.Write(0x11223344);
        }
        payload.Position = 0;
        var restoredFirst = new CGameControlGrid();
        var restoredSecond = new CGameControlGrid();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(restoredFirst, rw);
            chunk.ReadWrite(restoredSecond, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(restoredFirst.BaseLocalDataPointer.Instance).IsNotNull();
        await Assert.That(restoredSecond.BaseLocalDataPointer.Instance).IsSameReferenceAs(restoredFirst.BaseLocalDataPointer.Instance);
        await Assert.That(restoredFirst.BaseLocalDataPointer.Instance!.Data!.Name).IsEqualTo("Instance");
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(restoredFirst, rw);
            chunk.ReadWrite(restoredSecond, rw);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..^4], CollectionOrdering.Matching);
    }
}
