using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugCurveSimpleNodLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ReadWrite_Curve_PreservesVersionedPayload(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0); // Chunk version, independent of the inline curve version.
            writer.Write(version);
            writer.Write(2);
            if (version == 1) writer.Write((byte)0xFE);
            if (version == 2) writer.Write(23);
            if (version >= 3) writer.Write((ushort)0xFEDC);
            writer.Write(new Vec2(1.5f, 2.5f));
            writer.Write(new Vec2(3.5f, 4.5f));
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugCurveSimpleNod();
        var chunk = new CPlugCurveSimpleNod.Chunk09185000();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(chunk.Version).IsEqualTo(0);
        await Assert.That(node.Curve.Version).IsEqualTo(version);
        await Assert.That(node.Curve.Count).IsEqualTo(2);
        await Assert.That(node.Curve.Flags).IsEqualTo((ushort)(version == 1 ? 1 : version >= 3 ? 0xFEDC : 0));
        await Assert.That(node.Curve.Capacity).IsEqualTo(version == 2 ? 23 : 7);
        await Assert.That(node.Curve.Values!).IsEquivalentTo(new Vec2[] { new(1.5f, 2.5f), new(3.5f, 4.5f) }, CollectionOrdering.Matching);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    public async Task DeepClone_Node_CopiesTheSharedCurveAndControlPoints()
    {
        var node = new CPlugCurveSimpleNod
        {
            Curve = new PlugCurve { Flags = 0xFEDC, Values = [new(1, 2)] }
        };
        node.CreateChunk<CPlugCurveSimpleNod.Chunk09185000>();
        PlugCurve legacy = node;
        var model = new CPlugVisEntFxModel { EliminatedTeleportIntens = legacy, EliminatedHighlight = legacy };
#pragma warning disable GBXNET10001
        var clone = (CPlugVisEntFxModel)model.DeepClone();
#pragma warning restore GBXNET10001
        var clonedNode = (CPlugCurveSimpleNod)clone.EliminatedHighlight!.Curve!;
        await Assert.That(clone.EliminatedHighlight).IsSameReferenceAs(clone.EliminatedTeleportIntens);
        await Assert.That(clonedNode.Curve).IsNotSameReferenceAs(node.Curve);
        clonedNode.Curve.Values![0] = new Vec2(3, 4);
        await Assert.That(node.Curve.Values![0]).IsEqualTo(new Vec2(1, 2));
        await Assert.That(legacy.Values![0]).IsEqualTo(new Vec2(1, 2));
    }
}
