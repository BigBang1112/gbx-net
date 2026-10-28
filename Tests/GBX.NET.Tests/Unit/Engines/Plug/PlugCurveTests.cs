using GBX.NET.Engines.Function;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Tests.Unit.Engines.GameData;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class PlugCurveTests
{
    [Test]
    [Arguments(-8)]
    [Arguments(-1)]
    public async Task ReadWrite_NegativeVersions_PreserveLegacyReferences(int version)
    {
        using var payload = new ObjectModelPayload();
        payload.NodeRef("Legacy.Func.Gbx");
        payload.Finish();
        var curve = new PlugCurve();
        payload.Read(rw => curve.ReadWrite(rw, version));
        await Assert.That(curve.References.Curve.File!.FilePath).IsEqualTo("Legacy.Func.Gbx");
        await payload.AssertRoundTrip(rw => curve.ReadWrite(rw, version));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(8)]
    public async Task ReadWrite_NonNegativeVersions_UseInlineStorage(int version)
    {
        using var payload = new ObjectModelPayload();
        WriteCurve(payload, true, "Unused.Func.Gbx");
        payload.Finish();
        var curve = new PlugCurve();
        payload.Read(rw => curve.ReadWrite(rw, version));
        await Assert.That(curve.Version).IsEqualTo(4);
        await Assert.That(curve.Count).IsEqualTo(2);
        await Assert.That(curve.Flags).IsEqualTo((ushort)0xFEDC);
        await payload.AssertRoundTrip(rw => curve.ReadWrite(rw, version));
    }

    [Test]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    [Arguments(14)]
    public async Task ReadWrite_VehicleMaterial_SwitchesFromLegacyToInlineAtVersion12(int version)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(version);
        WriteFloats(writer, 4);
        writer.Write((byte)0);
        WriteFloats(writer, 9); // Versions 7, 1, 2 and 3 add scalar fields.
        writer.Write(false);
        writer.Write(Vec2.Zero);
        WriteFloats(writer, version <= 12 ? 4 : 3);
        WriteCurve(payload, version >= 12, "First.Func.Gbx");
        if (version >= 10) WriteCurve(payload, version >= 12, "Second.Func.Gbx");
        if (version >= 11) writer.Write(25.5f);
        if (version >= 13)
        {
            WriteCurve(payload, true, "Third.Func.Gbx");
            WriteCurve(payload, true, "Fourth.Func.Gbx");
            WriteFloats(writer, 3);
        }
        if (version >= 14)
        {
            WriteCurve(payload, true, "Fifth.Func.Gbx");
            writer.Write(32);
        }
        payload.Finish();

        var model = new CPlugVehicleMaterial();
        var chunk = new CPlugVehicleMaterial.Chunk090F1010();
        payload.Read(rw => chunk.ReadWrite(model, rw));
        if (version < 12)
        {
            await Assert.That(chunk.U23!.References.Curve.File!.FilePath).IsEqualTo("First.Func.Gbx");
        }
        else
        {
            await Assert.That(chunk.U23!.Version).IsEqualTo(4);
            await Assert.That(chunk.U23.Flags).IsEqualTo((ushort)0xFEDC);
            await Assert.That(chunk.U23.Values![0]).IsEqualTo(new Vec2(1.5f, 2.5f));
        }
        if (version >= 11) await Assert.That(chunk.U25).IsEqualTo(25.5f);
        if (version >= 14) await Assert.That(chunk.U32).IsEqualTo(32);
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(model, rw));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ReadWrite_VehicleTuning_SwitchesFromLegacyToInlineAtVersion2(int version)
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(version);
        payload.Writer.Write(123);
        WriteCurve(payload, version >= 2, "Tuning.Func.Gbx");
        if (version >= 1) payload.Writer.Write(45.5f);
        payload.Finish();

        var model = new CPlugVehicleCarPhyTuning();
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED0AE();
        payload.Read(rw => chunk.ReadWrite(model, rw));
        if (version < 2)
        {
            await Assert.That(chunk.U02!.References.Curve.File!.FilePath).IsEqualTo("Tuning.Func.Gbx");
        }
        else
        {
            await Assert.That(chunk.U02!.Flags).IsEqualTo((ushort)0xFEDC);
            await Assert.That(chunk.U02.Values!.Length).IsEqualTo(2);
        }
        await Assert.That(chunk.U01).IsEqualTo(123);
        if (version >= 1) await Assert.That(chunk.U03).IsEqualTo(45.5f);
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(model, rw));
    }

    [Test]
    public async Task ReadWrite_VehicleTuning_UnversionedCurveFieldUsesInlineStorage()
    {
        var curve = new PlugCurve { Flags = 0xFEDC, Count = 99, Values = [new(1.5f, 2.5f)] };
        var model = new CPlugVehicleCarPhyTuning { MaxSideFriction = curve };
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED0AF();
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        {
            chunk.ReadWrite(model, new GbxReaderWriter(writer));
        }
        saved.Position = 0;
        using var reader = new GbxReader(saved);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0); // Chunk version
        var restored = reader.ReadReadable<PlugCurve>();
        await Assert.That(restored.Version).IsEqualTo(3);
        await Assert.That(restored.Count).IsEqualTo(1);
        await Assert.That(restored.Flags).IsEqualTo((ushort)0xFEDC);
        await Assert.That(restored.Values![0]).IsEqualTo(new Vec2(1.5f, 2.5f));
    }

    [Test]
    public async Task ReadWrite_Water_LegacyWriteCreatesSimpleCurveNodesFromControlPoints()
    {
        var model = new CPlugDynaWaterModel();
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) model.Write(writer, v: 0);
        saved.Position = 0;
        using var reader = new GbxReader(saved);
        var restored = reader.ReadReadable<CPlugDynaWaterModel>(version: 0);
        await Assert.That(restored.U04!.Curve).IsTypeOf<CPlugCurveSimpleNod>();
        await Assert.That(restored.U04.Values!).IsEquivalentTo(model.U04!.Values!, CollectionOrdering.Matching);
        await Assert.That(restored.U05!.Values!).IsEquivalentTo(model.U05!.Values!, CollectionOrdering.Matching);
        await Assert.That(restored.U06!.Values!).IsEquivalentTo(model.U06!.Values!, CollectionOrdering.Matching);
        await Assert.That(restored.U08!.Values!).IsEquivalentTo(model.U08!.Values!, CollectionOrdering.Matching);
        await Assert.That(saved.Position).IsEqualTo(saved.Length);
    }

    [Test]
    public async Task DeepClone_Node_CopiesControlPointsAndLegacyNodesWhilePreservingSharedReferences()
    {
        var curve = new PlugCurve
        {
            Values = [new(1, 2)],
            Curve = new CFuncKeysReal { Xs = [1], Ys = [2] }
        };
        var model = new CPlugVisEntFxModel { EliminatedTeleportIntens = curve, EliminatedHighlight = curve };
#pragma warning disable GBXNET10001
        var clone = (CPlugVisEntFxModel)model.DeepClone();
#pragma warning restore GBXNET10001
        await Assert.That(clone.EliminatedHighlight).IsSameReferenceAs(clone.EliminatedTeleportIntens);
        await Assert.That(clone.EliminatedHighlight).IsNotSameReferenceAs(curve);
        await Assert.That(clone.EliminatedHighlight!.Curve).IsNotSameReferenceAs(curve.Curve);
        clone.EliminatedHighlight.Values![0] = new Vec2(3, 4);
        clone.EliminatedHighlight.Curve!.Xs![0] = 5;
        await Assert.That(curve.Values![0]).IsEqualTo(new Vec2(1, 2));
        await Assert.That(curve.Curve!.Xs![0]).IsEqualTo(1f);
    }

    private static void WriteCurve(ObjectModelPayload payload, bool inline, string filePath)
    {
        if (!inline)
        {
            payload.NodeRef(filePath);
            return;
        }
        payload.Writer.Write(4);
        payload.Writer.Write(2);
        payload.Writer.Write((ushort)0xFEDC);
        payload.Writer.Write(new Vec2(1.5f, 2.5f));
        payload.Writer.Write(new Vec2(3.5f, 4.5f));
    }

    private static void WriteFloats(GbxWriter writer, int count)
    {
        for (var i = 0; i < count; i++) writer.Write(i + 0.5f);
    }
}
