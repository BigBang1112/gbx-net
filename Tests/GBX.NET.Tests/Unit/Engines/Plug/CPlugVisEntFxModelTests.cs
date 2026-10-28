using GBX.NET.Engines.Function;
using GBX.NET.Engines.Graphic;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Tests.Unit.Engines.GameData;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVisEntFxModelTests
{
    [Test]
    public async Task ReadWrite_LegacyChunk_PreservesEveryExternalReferenceAndRgb()
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(0);
        WriteParticles(payload);
        payload.NodeRef("Teleport.Func.Gbx");
        payload.NodeRef("Highlight.Func.Gbx");
        payload.Writer.Write(new Vec3(0.2f, 0.4f, 0.6f));
        payload.NodeRef("Tactical.Light.Gbx");
        payload.Finish();

        var model = new CPlugVisEntFxModel();
        var chunk = new CPlugVisEntFxModel.Chunk09115000();
        payload.Read(rw => chunk.ReadWrite(model, rw));
        await Assert.That(chunk.Version).IsEqualTo(0);
        await AssertParticles(model);
        await Assert.That(model.EliminatedTeleportIntens!.References.Curve.File!.FilePath).IsEqualTo("Teleport.Func.Gbx");
        await Assert.That(model.EliminatedHighlight!.References.Curve.File!.FilePath).IsEqualTo("Highlight.Func.Gbx");
        await Assert.That(model.EliminatedHighlightRgb).IsEqualTo(new Vec3(0.2f, 0.4f, 0.6f));
        await Assert.That(model.References.TacticalLight.File!.FilePath).IsEqualTo("Tactical.Light.Gbx");
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(model, rw));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ReadWrite_CurrentChunk_PreservesInlineCurveVersionsAndNativeOrder(int curveVersion)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(1);
        WriteParticles(payload);
        WriteInlineCurve(writer, curveVersion, 0);
        WriteInlineCurve(writer, curveVersion, 1);
        writer.Write(new Vec3(0.2f, 0.4f, 0.6f));
        payload.NodeRef("Tactical.Light.Gbx");
        payload.Finish();

        var model = new CPlugVisEntFxModel();
        var chunk = new CPlugVisEntFxModel.Chunk09115000();
        payload.Read(rw => chunk.ReadWrite(model, rw));
        await Assert.That(chunk.Version).IsEqualTo(1);
        await AssertParticles(model);
        var curves = new[] { model.EliminatedTeleportIntens!, model.EliminatedHighlight! };
        for (var i = 0; i < curves.Length; i++)
        {
            await Assert.That(curves[i].Version).IsEqualTo(curveVersion);
            await Assert.That(curves[i].Count).IsEqualTo(i + 1);
            // Curve versions 0 and 2 retain the constructor's interpolation flags.
            await Assert.That(curves[i].Flags).IsEqualTo((ushort)(curveVersion >= 3 ? 0xFEDC + i : curveVersion == 1 ? 1 : i == 0 ? 2 : 0));
            if (curveVersion == 1) await Assert.That(curves[i].FlagsV1).IsEqualTo((byte)(0xA5 + i));
            if (curveVersion == 2) await Assert.That(curves[i].Capacity).IsEqualTo(0x12345678 + i);
            await Assert.That(curves[i].Values![i]).IsEqualTo(new Vec2(i + 1.5f, i + 2.5f));
        }
        await Assert.That(model.EliminatedHighlightRgb).IsEqualTo(new Vec3(0.2f, 0.4f, 0.6f));
        await Assert.That(model.References.TacticalLight.File!.FilePath).IsEqualTo("Tactical.Light.Gbx");
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(model, rw));
    }

    [Test]
    [Arguments(CFuncKeysReal.ERealInterp.None, 1)]
    [Arguments(CFuncKeysReal.ERealInterp.Linear, 0)]
    [Arguments(CFuncKeysReal.ERealInterp.Hermite, 0x10)]
    [Arguments(CFuncKeysReal.ERealInterp.SmoothStep, 2)]
    public async Task ReadWrite_LegacyCurve_PreservesNodeAndConvertsValuesAndInterpolation(CFuncKeysReal.ERealInterp interpolation, int flags)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(1);
        writer.Write(CFuncKeysReal.Id);
        writer.Write(0x05002001u);
        writer.Write(2);
        writer.Write(1.5f);
        writer.Write(3.5f);
        writer.Write(0x0501A002u);
        writer.Write(2);
        writer.Write(2.5f);
        writer.Write(4.5f);
        writer.Write((int)interpolation);
        writer.Write(true);
        writer.Write(true);
        writer.Write(5.5f);
        writer.Write(6.5f);
        writer.Write(0xFACADE01u);
        payload.Finish();

        var keys = new PlugCurve();
        payload.Read(rw => keys.ReadWrite(rw, version: -1));
        await Assert.That(keys.Curve!.RealInterp).IsEqualTo(interpolation);
        await Assert.That(keys.Curve.ForcedTangentMinX).IsEqualTo(5.5f);
        await Assert.That(keys.Curve.ForcedTangentMaxX).IsEqualTo(6.5f);
        await Assert.That(keys.Flags).IsEqualTo((ushort)(flags | 0x20));
        await Assert.That(keys.Values!).IsEquivalentTo(new[] { new Vec2(1.5f, 2.5f), new Vec2(3.5f, 4.5f) }, CollectionOrdering.Matching);
        await Assert.That(keys.Count).IsEqualTo(2);
        await payload.AssertRoundTrip(rw => keys.ReadWrite(rw, version: -1));

        using var current = new MemoryStream();
        using (var currentWriter = new GbxWriter(current)) keys.Write(currentWriter, v: 1);
        current.Position = 0;
        using var reader = new GbxReader(current);
        var restored = reader.ReadReadable<PlugCurve>(version: 1);
        await Assert.That(restored.Flags).IsEqualTo(keys.Flags);
        await Assert.That(restored.Values!).IsEquivalentTo(keys.Values!, CollectionOrdering.Matching);
        await Assert.That(current.Position).IsEqualTo(current.Length);
    }

    [Test]
    public async Task ReadWrite_LegacyCurve_AcceptsCurrentSimpleCurveNode()
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(1);
        writer.Write(CPlugCurveSimpleNod.Id);
        writer.Write(0x09185000u);
        writer.Write(0);
        WriteInlineCurve(writer, 3, 1);
        writer.Write(0xFACADE01u);
        payload.Finish();

        var keys = new PlugCurve();
        payload.Read(rw => keys.ReadWrite(rw, version: -1));
        await Assert.That(keys.Curve).IsTypeOf<CPlugCurveSimpleNod>();
        await Assert.That(keys.Flags).IsEqualTo((ushort)0xFEDD);
        await Assert.That(keys.Count).IsEqualTo(2);
        await Assert.That(keys.Values![1]).IsEqualTo(new Vec2(2.5f, 3.5f));
        await payload.AssertRoundTrip(rw => keys.ReadWrite(rw, version: -1));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task Constructor_NewNode_PreservesNativeDefaultsWhenWrittenAsNode(int version)
    {
        var model = new CPlugVisEntFxModel();
        model.CreateChunk<CPlugVisEntFxModel.Chunk09115000>().Version = version;
        using var payload = new ObjectModelPayload();
        payload.Writer.WriteNodeRef(model);
        payload.Finish();
        CPlugVisEntFxModel? restored = null;
        payload.Read(rw => restored = rw.Reader!.ReadNodeRef<CPlugVisEntFxModel>());
        await Assert.That(restored!.Chunks.Get<CPlugVisEntFxModel.Chunk09115000>()!.Version).IsEqualTo(version);
        await Assert.That(restored.EliminatedHighlightRgb).IsEqualTo(new Vec3(1, 0.8f, 0.4f));
        await Assert.That(restored.EliminatedTeleportIntens!.Flags).IsEqualTo((ushort)2);
        await Assert.That(restored.EliminatedTeleportIntens.Values!).IsEquivalentTo(new[] { new Vec2(0, 1), new Vec2(1, 1), new Vec2(2.7f, 0) }, CollectionOrdering.Matching);
        await Assert.That(restored.EliminatedHighlight!.Values!).IsEquivalentTo(new[] { new Vec2(0, 0), new Vec2(0.005f, 6), new Vec2(0.05f, 6), new Vec2(0.12f, 1), new Vec2(0.3f, 0.25f), new Vec2(2.5f, 0) }, CollectionOrdering.Matching);
        await Assert.That(restored.TacticalLight!.Radius).IsEqualTo(10f);
        await Assert.That(restored.TacticalLight.RadiusSpecular).IsEqualTo(10f);
        await Assert.That(restored.TacticalLight.Flags).IsEqualTo(0x10u);
        await Assert.That(restored.TacticalLight.Color).IsEqualTo(Vec3.One);
        await Assert.That(restored.TacticalLight.Intensity).IsEqualTo(1f);
        await Assert.That(((GxLight)restored.TacticalLight).Flags).IsEqualTo((GxLight.EFlags)0x4D);
        await payload.AssertRoundTrip(rw => rw.Writer!.WriteNodeRef(restored));
    }

    [Test]
    public async Task Constructor_ChunkVersions_MatchVerifiedWriters()
    {
        await Assert.That(new CPlugVisEntFxModel.Chunk09115000(GameVersion.MP4).Version).IsEqualTo(0);
        await Assert.That(new CPlugVisEntFxModel.Chunk09115000(GameVersion.TM2020).Version).IsEqualTo(1);
    }

    private static void WriteParticles(ObjectModelPayload payload)
    {
        payload.NodeRef("DeactivatedShot.Particle.Gbx");
        payload.NodeRef("Deactivated.Particle.Gbx");
        payload.NodeRef("TeleportUnspawn.Particle.Gbx");
        payload.NodeRef("TeleportSpawn.Particle.Gbx");
    }

    private static async Task AssertParticles(CPlugVisEntFxModel model)
    {
        await Assert.That(model.References.Part_DeactivatedShot.File!.FilePath).IsEqualTo("DeactivatedShot.Particle.Gbx");
        await Assert.That(model.References.Part_Deactivated.File!.FilePath).IsEqualTo("Deactivated.Particle.Gbx");
        await Assert.That(model.References.Part_TeleportUnspawn.File!.FilePath).IsEqualTo("TeleportUnspawn.Particle.Gbx");
        await Assert.That(model.References.Part_TeleportSpawn.File!.FilePath).IsEqualTo("TeleportSpawn.Particle.Gbx");
    }

    private static void WriteInlineCurve(GbxWriter writer, int version, int index)
    {
        writer.Write(version);
        writer.Write(index + 1);
        if (version == 1) writer.Write((byte)(0xA5 + index));
        if (version == 2) writer.Write(0x12345678 + index);
        if (version >= 3) writer.Write((ushort)(0xFEDC + index));
        for (var i = 0; i <= index; i++) writer.Write(new Vec2(i + 1.5f, i + 2.5f));
    }
}
