using GBX.NET.Engines.Function;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVehicleCarPhyTuningLayoutTests
{
    [Test]
    public async Task ReadWrite_GameplayModel_KeepsItsFloatingPointSteeringTorque()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(6);
            writer.Write(1.5f);
        }
        var node = await RoundTrip(payload, new CPlugVehicleCarPhyTuning.Chunk090ED010());
        await Assert.That(node.GameplayModel).IsEqualTo(6);
        await Assert.That(node.SteerGroundTorque).IsEqualTo(1.5f);
    }

    [Test]
    [Arguments(6)]
    [Arguments(7)]
    public async Task ReadWrite_AirControl_UsesInlineCurvesFromVersionSeven(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(120);
            writer.Write(2.5f);
            WriteCurve(writer, version >= 7);
            writer.Write(true);
            writer.Write(3f);
            writer.Write(4f);
            writer.Write(5f);
            writer.Write(10);
            writer.Write(11);
            WriteCurve(writer, version >= 7);
            WriteCurve(writer, version >= 7);
            WriteCurve(writer, version >= 7);
            writer.Write(12);
            writer.Write(13);
            writer.Write(14);
            WriteCurve(writer, version >= 7);
            writer.Write(15);
            WriteCurve(writer, version >= 7);
        }
        var node = await RoundTrip(payload, new CPlugVehicleCarPhyTuning.Chunk090ED095());
        await Assert.That(node.AirControlDuration).IsEqualTo(120);
        await Assert.That(node.AirControlMaxAngularSpeedY).IsEqualTo(2.5f);
        if (version >= 7)
        {
            await Assert.That(node.AirControlZCoefFromAngularSpeed!.Version).IsEqualTo(2);
            await Assert.That(node.AirControlZCoefFromAngularSpeed.Capacity).IsEqualTo(11);
        }
    }

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ReadWrite_CurveGroup_UsesInlineCurvesFromVersionTwo(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            WriteCurve(writer, version >= 2);
            WriteCurve(writer, version >= 2);
            writer.Write(1.25f);
            writer.Write(2.25f);
            WriteCurve(writer, version >= 2);
            WriteCurve(writer, version >= 2);
            for (var i = 0; i < 5; i++) writer.Write(3.25f + i);
            WriteCurve(writer, version >= 2);
        }
        await RoundTrip(payload, new CPlugVehicleCarPhyTuning.Chunk090ED0A1());
    }

    [Test]
    [Arguments(4)]
    [Arguments(5)]
    public async Task ReadWrite_JumpHook_PreservesVectorAndVersionedCurve(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            for (var i = 0; i < 6; i++) writer.Write(i + 0.25f);
            for (var i = 0; i < 3; i++) writer.Write(100 + i);
            for (var i = 0; i < 18; i++) writer.Write(i + 1.5f);
            writer.Write(new Vec3(31, 32, 33));
            WriteCurve(writer, version >= 5);
        }
        var node = await RoundTrip(payload, new CPlugVehicleCarPhyTuning.Chunk090ED09B());
        await Assert.That(node.HookPos).IsEqualTo((Vec3?)new Vec3(31, 32, 33));
        await Assert.That(node.BigCarScale).IsEqualTo((float?)18.5f);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ReadWrite_RetiredCurveGroup_PreservesItsVersionZeroReferences(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            for (var i = 0; i < (version < 2 ? 8 : 2); i++) writer.Write(i + 100);
            for (var i = 0; i < 6; i++) writer.Write(i + 0.5f);
            for (var i = 0; i < (version < 2 ? 11 : 2); i++) WriteCurve(writer, version >= 1);
        }
        await RoundTrip(payload, new CPlugVehicleCarPhyTuning.Chunk090ED0A5());
    }

    [Test]
    [Arguments(12)]
    [Arguments(13)]
    public async Task ReadWrite_ObsoleteCurves_EndAfterVersionTwelve(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            for (var i = 0; i < 3; i++) WriteCurve(writer, inline: true);
            for (var i = 0; i < 3; i++) writer.Write(i + 100);
            writer.Write(1.5f);
            WriteCurve(writer, inline: true);
            WriteCurve(writer, inline: true);
            writer.Write(2.5f);
            WriteCurve(writer, inline: true);
            writer.Write(3.5f);
            WriteCurve(writer, inline: true);
            WriteCurve(writer, inline: true);
            writer.Write(101);
            writer.Write(102);
            if (version == 12)
            {
                WriteCurve(writer, inline: true);
                WriteCurve(writer, inline: true);
            }
            for (var i = 0; i < 4; i++) WriteCurve(writer, inline: true);
            writer.Write(103);
            writer.Write(104);
        }
        await RoundTrip(payload, new CPlugVehicleCarPhyTuning.Chunk090ED0A6());
    }

    [Test]
    public async Task ReadWrite_VersionThirtyFive_PreservesFloatInsteadOfNormalizingItToBoolean()
    {
        var node = new CPlugVehicleCarPhyTuning();
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED08B { Version = 35, U23 = 1500.5f };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(BitConverter.ToSingle(payload.ToArray(), (int)payload.Length - 4)).IsEqualTo(1500.5f);
        var restoredChunk = new CPlugVehicleCarPhyTuning.Chunk090ED08B();
        await RoundTrip(payload, restoredChunk);
        await Assert.That(restoredChunk.U23).IsEqualTo(1500.5f);
    }

    [Test]
    public async Task ReadWrite_LegacyAccelerationReferenceAndInlinePoints_ShareOneProperty()
    {
        var node = new CPlugVehicleCarPhyTuning();
        var legacyChunk = new CPlugVehicleCarPhyTuning.Chunk090ED024();
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer)) legacyChunk.ReadWrite(node, rw);
        var restored = await RoundTrip(payload, legacyChunk);
        await Assert.That(restored.AccelCurve!.Curve).IsNotNull();
        await Assert.That(restored.AccelCurve.Values).IsEquivalentTo(node.AccelCurve!.Values!, CollectionOrdering.Matching);
        var inlineChunk = new CPlugVehicleCarPhyTuning.Chunk090ED0AF();
        using var inline = new MemoryStream();
        using (var writer = new GbxWriter(inline))
        using (var rw = new GbxReaderWriter(writer)) inlineChunk.ReadWrite(restored, rw);
        var inlineRestored = await RoundTrip(inline, inlineChunk);
        await Assert.That(inlineRestored.AccelCurve!.Values).IsEquivalentTo(restored.AccelCurve.Values!, CollectionOrdering.Matching);
    }

    private static void WriteCurve(GbxWriter writer, bool inline)
    {
        if (!inline)
        {
            writer.WriteNodeRef<CFuncKeysReal>(null);
            return;
        }
        writer.Write(2);
        writer.Write(2);
        writer.Write(11);
        writer.Write(new Vec2(1.5f, 2.5f));
        writer.Write(new Vec2(3.5f, 4.5f));
    }

    private static async Task<CPlugVehicleCarPhyTuning> RoundTrip(MemoryStream payload, Chunk<CPlugVehicleCarPhyTuning> chunk)
    {
        payload.Position = 0;
        var node = new CPlugVehicleCarPhyTuning();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
        return node;
    }
}
