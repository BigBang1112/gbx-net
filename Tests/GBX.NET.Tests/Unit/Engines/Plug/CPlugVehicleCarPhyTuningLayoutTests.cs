using GBX.NET.Components;
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
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    public async Task ReadWrite_AirControl_PreservesVersionedFieldsAndCurveFormats(int version)
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
            if (version >= 1) writer.Write(10);
            if (version >= 2)
            {
                writer.Write(true);
                WriteCurve(writer, version >= 7);
                WriteCurve(writer, version >= 7);
            }
            if (version >= 3) WriteCurve(writer, version >= 7);
            if (version >= 4) writer.Write(true);
            if (version >= 5)
            {
                writer.Write(true);
                writer.Write(14);
                WriteCurve(writer, version >= 7);
                writer.Write(15.5f);
            }
            if (version >= 6) WriteCurve(writer, version >= 7);
        }
        var node = await RoundTrip(payload, new CPlugVehicleCarPhyTuning.Chunk090ED095());
        await Assert.That(node.AirControlDuration).IsEqualTo(120);
        await Assert.That(node.AirControlMaxAngularSpeedY).IsEqualTo(2.5f);
        if (version >= 2) await Assert.That(node.AirControlXYEnabled).IsTrue();
        if (version >= 4) await Assert.That(node.AirControlXEnabled).IsTrue();
        await Assert.That(node.AirControlXOrientationLimitEnabled).IsTrue();
        await Assert.That(node.AirControlXOrientationThresholdDeg).IsEqualTo((float?)(version >= 5 ? 15.5f : 5f));
        if (version >= 7)
        {
            await Assert.That(node.AirControlZCoefFromAngularSpeed!.Version).IsEqualTo(2);
            await Assert.That(node.AirControlZCoefFromAngularSpeed.Capacity).IsEqualTo(11);
            await Assert.That(node.AirControlXAccelFromAngularSpeed!.Values).IsEquivalentTo(new Vec2[] { new(1.5f, 2.5f), new(3.5f, 4.5f) }, CollectionOrdering.Matching);
            await Assert.That(node.AirControlYAccelFromAngularSpeed!.Capacity).IsEqualTo(11);
            await Assert.That(node.AirControlXLimitTorqueFromAngularSpeed!.Capacity).IsEqualTo(11);
            await Assert.That(node.AirControlXAccelCoefFromOrientation!.Capacity).IsEqualTo(11);
        }
    }

    [Test]
    [Arguments(0)]
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
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED0A1();
        await RoundTrip(payload, chunk);
        await Assert.That(chunk.U03).IsEqualTo(1.25f);
        await Assert.That(chunk.U04).IsEqualTo(2.25f);
        await Assert.That(chunk.U07).IsEqualTo(3.25f);
        await Assert.That(chunk.U08).IsEqualTo(4.25f);
        await Assert.That(chunk.U09).IsEqualTo(5.25f);
        await Assert.That(chunk.U10).IsEqualTo(6.25f);
        await Assert.That(chunk.U11).IsEqualTo(7.25f);
        foreach (var curve in new[] { chunk.U01, chunk.U02, chunk.U05, chunk.U06, chunk.U12 })
        {
            if (version < 2) await Assert.That(curve!.Values).IsEmpty();
            else
            {
                await Assert.That(curve!.Capacity).IsEqualTo(11);
                await Assert.That(curve.Values).IsEquivalentTo(new Vec2[] { new(1.5f, 2.5f), new(3.5f, 4.5f) }, CollectionOrdering.Matching);
            }
        }
    }

    [Test]
    public async Task Constructor_CurveGroup_UsesNativeScalarDefaults()
    {
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED0A1(GameVersion.TM2020);
        await Assert.That(chunk.Version).IsEqualTo(2);
        await Assert.That(chunk.U03).IsEqualTo(0.01f);
        await Assert.That(chunk.U04).IsEqualTo(0.01f);
        await Assert.That(chunk.U07).IsEqualTo(0.25f);
        await Assert.That(chunk.U08).IsEqualTo(90f);
        await Assert.That(chunk.U09).IsEqualTo(20f);
        await Assert.That(chunk.U10).IsEqualTo(45f);
        await Assert.That(chunk.U11).IsEqualTo(0.01f);
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
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    [Arguments(14)]
    [Arguments(15)]
    public async Task ReadWrite_ObsoleteCurves_EndAfterVersionTwelve(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            for (var i = 0; i < 3; i++) WriteCurve(writer, inline: version >= 11);
            for (var i = 0; i < 3; i++) writer.Write(i + 100);
            writer.Write(1.5f);
            WriteCurve(writer, inline: version >= 11);
            WriteCurve(writer, inline: version >= 11);
            writer.Write(2.5f);
            WriteCurve(writer, inline: version >= 11);
            writer.Write(3.5f);
            WriteCurve(writer, inline: version >= 11);
            WriteCurve(writer, inline: version >= 11);
            writer.Write(101);
            writer.Write(102);
            if (version <= 12)
            {
                WriteCurve(writer, inline: version >= 11);
                WriteCurve(writer, inline: version >= 11);
            }
            for (var i = 0; i < 4; i++) WriteCurve(writer, inline: version >= 11);
            if (version >= 12)
            {
                writer.Write(103);
                writer.Write(104);
            }
            if (version == 14) writer.Write(105);
            if (version >= 15) WriteCurve(writer, inline: true);
        }
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED0A6();
        await RoundTrip(payload, chunk);
        if (version >= 11) await Assert.That(chunk.U01!.Capacity).IsEqualTo(11);
        if (version >= 15) await Assert.That(chunk.U28!.Capacity).IsEqualTo(11);
        else await Assert.That(chunk.U28!.Values).IsEquivalentTo(new Vec2[] { new(0f, 1.07f), new(1f, 1.05f) }, CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_VersionThirtyFive_PreservesFloatInsteadOfNormalizingItToBoolean()
    {
        var node = new CPlugVehicleCarPhyTuning();
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED08B { Version = 35, U21 = 1500.5f };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(BitConverter.ToSingle(payload.ToArray(), (int)payload.Length - 4)).IsEqualTo(1500.5f);
        var restoredChunk = new CPlugVehicleCarPhyTuning.Chunk090ED08B();
        await RoundTrip(payload, restoredChunk);
        await Assert.That(restoredChunk.U21).IsEqualTo(1500.5f);
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

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    [Arguments(14)]
    [Arguments(15)]
    [Arguments(16)]
    [Arguments(17)]
    [Arguments(18)]
    [Arguments(19)]
    [Arguments(20)]
    [Arguments(21)]
    public async Task ReadWrite_Helico_PreservesVersionedFieldsAndLegacyUnits(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            if (version <= 17) writer.Write(3501f);
            else
            {
                for (var i = 0; i < 3; i++) WriteCurve(writer, version >= 21);
            }
            writer.Write(-4100f);
            writer.Write(-6100f);
            for (var i = 0; i < 3; i++) writer.WriteNodeRef<CFuncKeysReal>(null);
            WriteCurve(writer, version >= 21);
            WriteCurve(writer, version >= 21);
            if (version >= 1)
            {
                WriteCurve(writer, version >= 21);
                WriteCurve(writer, version >= 21);
            }
            if (version >= 2) writer.Write(2.25f);
            if (version == 3) writer.Write(15.25f);
            if (version >= 4) writer.WriteNodeRef<CFuncKeysReal>(null);
            if (version >= 5)
            {
                writer.Write(3.25f);
                WriteCurve(writer, version >= 21);
                writer.Write(31.25f);
                writer.Write(version < 14 ? 0.0625f : 625f);
                writer.Write(version < 14 ? 10.5f : 1050f);
            }
            if (version >= 6) writer.Write(201.25f);
            if (version >= 7)
            {
                writer.Write(1);
                WriteCurve(writer, version >= 21);
            }
            if (version >= 8) WriteCurve(writer, version >= 21);
            if (version >= 9) writer.Write(version < 14 ? 0.03125f : 312.5f);
            if (version >= 10) writer.Write(1.25f);
            if (version >= 11)
            {
                WriteCurve(writer, version >= 21);
                WriteCurve(writer, version >= 21);
            }
            if (version >= 12) WriteCurve(writer, version >= 21);
            if (version >= 13) WriteCurve(writer, version >= 21);
            if (version >= 15) writer.Write(0.25f);
            if (version >= 16) writer.Write(41.25f);
            if (version >= 19) WriteCurve(writer, version >= 21);
            if (version >= 20) writer.Write(0.75f);
        }
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED09C();
        var node = await RoundTrip(payload, chunk);
        await Assert.That(node.HelicoFrictionCoef).IsEqualTo((float?)-4100f);
        await Assert.That(node.HelicoFrictionTorqueCoef).IsEqualTo((float?)-6100f);
        if (version <= 17) await Assert.That(chunk.U01).IsEqualTo((float?)3501f);
        if (version >= 5)
        {
            await Assert.That(node.HelicoBladeRotAccelDeg).IsEqualTo((float?)(version < 14 ? 0.0625f : 625f));
            await Assert.That(node.HelicoBladeMaxSpeedDeg).IsEqualTo((float?)(version < 14 ? 10.5f : 1050f));
        }
        if (version >= 7) await Assert.That(node.HelicoVersion).IsEqualTo((CPlugVehicleCarPhyTuning.EVehicleHelicoVersion?)CPlugVehicleCarPhyTuning.EVehicleHelicoVersion.V2);
        if (version >= 9) await Assert.That(node.HelicoBladeRotDecelDeg).IsEqualTo((float?)(version < 14 ? 0.03125f : 312.5f));
        if (version >= 15) await Assert.That(node.CenterOfMassZ_NoWheel).IsEqualTo((float?)0.25f);
        if (version >= 21)
        {
            await Assert.That(node.HelicoRollTorqueFromRollDeg!.Capacity).IsEqualTo(11);
            await Assert.That(node.HelicoGroundAccelCoefFromGroundDist!.Values).IsEquivalentTo(new Vec2[] { new(1.5f, 2.5f), new(3.5f, 4.5f) }, CollectionOrdering.Matching);
        }
    }

    [Test]
    [Arguments(16)]
    [Arguments(17)]
    public async Task ReadWrite_Helico_KeepsLegacyCurveAnglesAsStored(int version)
    {
        var node = new CPlugVehicleCarPhyTuning
        {
            HelicoRollTorqueFromRollDeg = new PlugCurve { Values = [new(90f, 4000f)] }
        };
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED09C { Version = version };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(node.HelicoRollTorqueFromRollDeg.Values![0].X).IsEqualTo(90f);
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        {
            reader.ReadInt32();
            reader.ReadSingle();
            reader.ReadSingle();
            reader.ReadSingle();
            for (var i = 0; i < 3; i++) reader.ReadNodeRef<CFuncKeysReal>();
            var wireCurve = reader.ReadNodeRef<CFuncKeysReal>();
            await Assert.That(wireCurve!.Xs![0]).IsEqualTo(90f);
        }
        payload.Position = 0;
        var restored = new CPlugVehicleCarPhyTuning();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) new CPlugVehicleCarPhyTuning.Chunk090ED09C().ReadWrite(restored, rw);
        await Assert.That(restored.HelicoRollTorqueFromRollDeg!.Values![0].X).IsEqualTo(90f);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    [Arguments(14)]
    [Arguments(15)]
    [Arguments(16)]
    [Arguments(17)]
    [Arguments(18)]
    [Arguments(19)]
    [Arguments(20)]
    [Arguments(21)]
    public async Task ReadWrite_Boat_PreservesNativeVersionedLayout(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            if (version == 0)
            {
                writer.Write(true);
                writer.Write(1.25f);
                writer.Write(2.25f);
                writer.Write(51.25f);
                writer.Write(1.75f);
                writer.Write(false);
                writer.Write(3.25f);
                writer.Write(0.000002f);
            }
            if (version >= 1)
            {
                if (version < 20)
                {
                    WriteCurve(writer, version >= 21);
                    writer.Write(1.75f);
                    WriteCurve(writer, version >= 21);
                    writer.Write(2.75f);
                    writer.Write(0.000002f);
                }
                writer.Write(51.25f);
                writer.Write(3.25f);
                WriteCurve(writer, version >= 21);
                WriteCurve(writer, version >= 21);
            }
            if (version >= 2 && version < 20) WriteCurve(writer, version >= 21);
            if (version >= 3)
            {
                WriteCurve(writer, version >= 21);
                WriteCurve(writer, version >= 21);
            }
            if (version >= 4)
            {
                writer.Write(1.5f);
                writer.Write(0.005f);
            }
            if (version >= 5)
            {
                writer.Write(31.25f);
                for (var i = 0; i < 3; i++) WriteCurve(writer, version >= 21);
                if (version < 20) WriteCurve(writer, version >= 21);
            }
            if (version >= 6) WriteCurve(writer, version >= 21);
            if (version >= 8)
            {
                writer.Write(1.25f);
                WriteCurve(writer, version >= 21);
            }
            if (version >= 9)
            {
                writer.Write(2.25f);
                writer.Write(3.25f);
                writer.Write(4.25f);
            }
            if (version >= 10) WriteCurve(writer, version >= 21);
            if (version >= 11) writer.Write(5.25f);
            if (version >= 12) WriteCurve(writer, version >= 21);
            if (version >= 13) WriteCurve(writer, version >= 21);
            if (version >= 14) WriteCurve(writer, version >= 21);
            if (version >= 15)
            {
                writer.Write(11.25f);
                writer.Write(2345);
            }
            if (version >= 16) writer.Write(6.25f);
            if (version >= 17) writer.Write(0.75f);
            if (version >= 18) WriteCurve(writer, version >= 21);
            if (version >= 19) writer.Write(3);
            if (version >= 20) writer.WriteNodeRef<CPlugDynaWaterModel>(null);
        }
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED09D();
        var node = await RoundTrip(payload, chunk);
        await Assert.That(node.BoatReferenceSpeed).IsEqualTo(51.25f);
        await Assert.That(node.BoatCharacteristicLength).IsEqualTo(3.25f);
        if (version < 20)
        {
            await Assert.That(node.WaterViscosity).IsEqualTo(0.000002f);
            await Assert.That(node.WaterPressureSuctionDragFalloffPower).IsEqualTo(version == 0 ? 1.75f : 2.75f);
        }
        if (version >= 4) await Assert.That(node.BoatSteeringSmoothingRatio).IsEqualTo((float?)0.005f);
        if (version >= 15) await Assert.That(node.BoatPlanningCooldownDuration).IsEqualTo((int?)2345);
        if (version >= 17) await Assert.That(chunk.U05).IsEqualTo((float?)0.75f);
        if (version >= 19) await Assert.That(node.BoatIsCarrier).IsEqualTo((int?)3);
        if (version >= 21)
        {
            await Assert.That(node.BoatAccelFromSpeedKmh!.Capacity).IsEqualTo(11);
            await Assert.That(node.BoatPlanningCoefBuildingRateFromPlanningCoefValue!.Values).IsEquivalentTo(new Vec2[] { new(1.5f, 2.5f), new(3.5f, 4.5f) }, CollectionOrdering.Matching);
        }
    }

    [Test]
    [Arguments(6, false)]
    [Arguments(6, true)]
    [Arguments(7, false)]
    [Arguments(7, true)]
    public async Task ReadWrite_Boat_KeepsLegacySpeedCurvesAsStored(int version, bool cached)
    {
        PlugCurve CreateCurve()
        {
            var curve = new PlugCurve { Values = [new(36f, 123f)] };
            if (cached) _ = (CFuncKeysReal?)curve;
            return curve;
        }
        var node = new CPlugVehicleCarPhyTuning
        {
            BoatAccelFromSpeedKmh = CreateCurve(),
            BoatReverseAccelFromSpeedKmh = CreateCurve(),
            BoatWaterSteeringFromSpeedKmh = CreateCurve(),
            BoatRigthingTorqueRatioFromSpeedKmh = CreateCurve(),
            BoatWheelTimeToMaxAngleFromSpeedZKmh = CreateCurve(),
            BoatWheelInverseTimeToZeroFromSpeedZKmh = CreateCurve(),
            BoatEngineSteeringFromSpeedKmh = CreateCurve()
        };
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED09D { Version = version };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        var curves = new[]
        {
            node.BoatAccelFromSpeedKmh, node.BoatWaterSteeringFromSpeedKmh,
            node.BoatRigthingTorqueRatioFromSpeedKmh, node.BoatWheelTimeToMaxAngleFromSpeedZKmh,
            node.BoatWheelInverseTimeToZeroFromSpeedZKmh, node.BoatReverseAccelFromSpeedKmh
        };
        foreach (var curve in curves)
        {
            await Assert.That(curve!.Values![0].X).IsEqualTo(36f);
            if (cached) await Assert.That(curve.Curve!.Xs![0]).IsEqualTo(36f);
        }
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        {
            reader.ReadInt32();
            reader.ReadNodeRef<CFuncKeysReal>();
            reader.ReadSingle();
            reader.ReadNodeRef<CFuncKeysReal>();
            for (var i = 0; i < 4; i++) reader.ReadSingle();
            var wireCurves = new List<CFuncKeysReal?> { reader.ReadNodeRef<CFuncKeysReal>(), reader.ReadNodeRef<CFuncKeysReal>() };
            reader.ReadNodeRef<CFuncKeysReal>();
            wireCurves.Add(reader.ReadNodeRef<CFuncKeysReal>());
            reader.ReadNodeRef<CFuncKeysReal>();
            for (var i = 0; i < 3; i++) reader.ReadSingle();
            for (var i = 0; i < 3; i++) wireCurves.Add(reader.ReadNodeRef<CFuncKeysReal>());
            foreach (var curve in wireCurves)
            {
                await Assert.That(curve!.Xs![0]).IsEqualTo(36f);
            }
            reader.ReadNodeRef<CFuncKeysReal>();
            var engineSteering = reader.ReadNodeRef<CFuncKeysReal>();
            await Assert.That(engineSteering!.Xs![0]).IsEqualTo(36f);
            await Assert.That(payload.Position).IsEqualTo(payload.Length);
        }
        var restored = await RoundTrip(payload, new CPlugVehicleCarPhyTuning.Chunk090ED09D());
        await Assert.That(restored.BoatAccelFromSpeedKmh!.Values![0].X).IsEqualTo(36f);
        await Assert.That(restored.BoatReverseAccelFromSpeedKmh!.Values![0].X).IsEqualTo(36f);
        await Assert.That(restored.BoatWaterSteeringFromSpeedKmh!.Values![0].X).IsEqualTo(36f);
        await Assert.That(restored.BoatRigthingTorqueRatioFromSpeedKmh!.Values![0].X).IsEqualTo(36f);
        await Assert.That(restored.BoatWheelTimeToMaxAngleFromSpeedZKmh!.Values![0].X).IsEqualTo(36f);
        await Assert.That(restored.BoatWheelInverseTimeToZeroFromSpeedZKmh!.Values![0].X).IsEqualTo(36f);
    }

    [Test]
    [Arguments(20, false)]
    [Arguments(20, true)]
    [Arguments(21, false)]
    [Arguments(21, true)]
    public async Task ReadWrite_Boat_PreservesExternalWaterModelWithoutResolvingIt(int version, bool resource)
    {
        var node = new CPlugVehicleCarPhyTuning();
        node.CreateChunk<CPlugVehicleCarPhyTuning.Chunk090ED09D>().Version = version;
        var table = new GbxRefTable();
        node.References.DynaWaterModel.Descriptor = resource
            ? new GbxRefTableResource(table, 4, true, 42)
            : new GbxRefTableFile(table, 0, true, "Water.DynaWaterModel.Gbx");
        using var stream = new MemoryStream();
        node.ToGbx(GbxHeaderBasic.Create(compressionOfBody: GbxCompression.Uncompressed)).Save(stream);

        for (var pass = 0; pass < 2; pass++)
        {
            stream.Position = 0;
            var parsed = Gbx.Parse(stream);
            var restored = (CPlugVehicleCarPhyTuning)parsed.Node!;
            var reference = restored.References.DynaWaterModel;
            await Assert.That(reference.Kind).IsEqualTo(resource ? NodeRefKind.ExternalResource : NodeRefKind.ExternalFile);
            await Assert.That(reference.State).IsEqualTo(NodeRefState.Unresolved);
            await Assert.That(reference.CachedNode).IsNull();
            if (resource) await Assert.That(reference.Resource?.ResourceIndex).IsEqualTo(42);
            else await Assert.That(reference.File?.FilePath).IsEqualTo("Water.DynaWaterModel.Gbx");
            stream.SetLength(0);
            parsed.Save(stream);
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    public async Task ReadWrite_Truck_PreservesNativeVersionedLayout(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(6.25f);
            writer.Write(0.75f);
            writer.Write(46.25f);
            if (version >= 1) writer.Write(1.25f);
            if (version >= 2)
            {
                writer.Write(1234);
                writer.Write(0.4f);
            }
            if (version >= 3) writer.Write(1.5f);
            if (version >= 4)
            {
                writer.Write(0.6f);
                WriteCurve(writer, version >= 8);
                if (version < 7) writer.Write(2.75f);
                else WriteCurve(writer, version >= 8);
            }
            if (version >= 5) writer.Write(3.25f);
            if (version >= 6)
            {
                writer.Write(0.8f);
                writer.Write(1.1f);
            }
        }
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED09E();
        var node = await RoundTrip(payload, chunk);
        await Assert.That(node.TruckLinkThresholdAngle).IsEqualTo(6.25f);
        await Assert.That(node.TruckLinkStiffness).IsEqualTo(0.75f);
        await Assert.That(node.TruckAxisMaxAngle).IsEqualTo(46.25f);
        if (version >= 1) await Assert.That(node.TruckInertiaZCoef).IsEqualTo((float?)1.25f);
        if (version >= 2)
        {
            await Assert.That(node.TruckAccelPenaltyDuration).IsEqualTo((int?)1234);
            await Assert.That(node.TruckLinkPosCorrectionRate).IsEqualTo((float?)0.4f);
        }
        if (version >= 3) await Assert.That(node.TruckLinkPosVelDamping).IsEqualTo((float?)1.5f);
        if (version >= 4)
        {
            await Assert.That(node.TruckLinkTrailerAngularCorrectionRatio).IsEqualTo((float?)0.6f);
            if (version < 7)
            {
                await Assert.That(node.TruckLinkHackHeaderPositiveRollMultiplier).IsEqualTo((float?)2.75f);
            }
        }
        if (version >= 5) await Assert.That(node.TruckLinkRollCenterOfMassY).IsEqualTo((float?)3.25f);
        if (version >= 6)
        {
            await Assert.That(chunk.U01).IsEqualTo((float?)0.8f);
            await Assert.That(chunk.U02).IsEqualTo((float?)1.1f);
        }
        if (version >= 8)
        {
            await Assert.That(node.TruckLinkRollMultiplierFromZSpeedKmh!.Capacity).IsEqualTo(11);
            await Assert.That(node.TruckLinkHackHeaderPositiveRollMultiplierCurve!.Values).IsEquivalentTo(new Vec2[] { new(1.5f, 2.5f), new(3.5f, 4.5f) }, CollectionOrdering.Matching);
        }
    }

    [Test]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    public async Task ReadWrite_Truck_SelectsThePositiveRollScalarOrCurveForItsVersion(int version)
    {
        Vec2[] points = [new(0f, 2.75f), new(100f, 3.75f)];
        var node = new CPlugVehicleCarPhyTuning
        {
            TruckLinkHackHeaderPositiveRollMultiplier = 4.75f,
            TruckLinkHackHeaderPositiveRollMultiplierCurve = new PlugCurve { Values = (Vec2[])points.Clone() }
        };
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED09E { Version = version };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(node.TruckLinkHackHeaderPositiveRollMultiplierCurve.Values).IsEquivalentTo(points, CollectionOrdering.Matching);
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        {
            reader.ReadInt32();
            reader.ReadBytes(32);
            new PlugCurve().Read(reader, version - 8);
            if (version < 7) await Assert.That(reader.ReadSingle()).IsEqualTo(4.75f);
            else
            {
                var wireCurve = new PlugCurve();
                wireCurve.Read(reader, version - 8);
                await Assert.That(wireCurve.Values).IsEquivalentTo(points, CollectionOrdering.Matching);
            }
            for (var i = 0; i < 3; i++) reader.ReadSingle();
            await Assert.That(payload.Position).IsEqualTo(payload.Length);
        }
        var restored = await RoundTrip(payload, new CPlugVehicleCarPhyTuning.Chunk090ED09E());
        if (version < 7) await Assert.That(restored.TruckLinkHackHeaderPositiveRollMultiplier).IsEqualTo((float?)4.75f);
        else await Assert.That(restored.TruckLinkHackHeaderPositiveRollMultiplierCurve!.Values).IsEquivalentTo(points, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    [Arguments(14)]
    public async Task ReadWrite_Plane_PreservesNativeVersionedLayoutAndDiscardedValues(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            WriteCurve(writer, version >= 14);
            writer.Write(2.25f);
            writer.Write(0.75f);
            writer.Write(0.8f);
            if (version >= 1) writer.Write(1.25f);
            if (version >= 2) WriteCurve(writer, version >= 14);
            if (version >= 3) WriteCurve(writer, version >= 14);
            if (version >= 4)
            {
                writer.Write(3.25f);
                writer.Write(5.25f);
                for (var i = 0; i < 4; i++) writer.Write(40.25f + i);
            }
            if (version >= 5) writer.Write(44.25f);
            if (version >= 6) WriteCurve(writer, version >= 14);
            if (version >= 7)
            {
                writer.Write(0.6f);
                WriteCurve(writer, version >= 14);
                WriteCurve(writer, version >= 14);
                writer.Write(18.25f);
            }
            if (version >= 8)
            {
                writer.Write(-6.25f);
                writer.Write(1.5f);
                writer.Write(1.75f);
                writer.Write(1.25f);
            }
            if (version >= 9)
            {
                writer.Write(new Vec3(1.25f, 2.25f, 3.25f));
                writer.Write(new Vec3(-1f, -2f, -3f));
            }
            if (version >= 10) WriteCurve(writer, version >= 14);
            if (version >= 11) WriteCurve(writer, version >= 14);
            if (version >= 12) writer.Write(true);
            if (version >= 13)
            {
                writer.Write(2600);
                writer.Write(10100);
                writer.Write(4600);
                writer.Write(17600);
                writer.Write(9);
            }
        }
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED09F();
        var node = await RoundTrip(payload, chunk);
        await Assert.That(node.PlaneLiftPower).IsEqualTo(2.25f);
        await Assert.That(node.PlaneLanding_Contact_Coef).IsEqualTo(0.75f);
        await Assert.That(node.PlaneLanding_End_Coef).IsEqualTo(0.8f);
        if (version >= 1) await Assert.That(node.PlaneAngularFriction).IsEqualTo((float?)1.25f);
        if (version >= 4)
        {
            await Assert.That(node.PlaneLanding_Rebounds_Coef).IsEqualTo((float?)3.25f);
            await Assert.That(node.PlaneLanding_Impacts_Coef).IsEqualTo((float?)5.25f);
            await Assert.That(chunk.U01).IsEqualTo((float?)40.25f);
            await Assert.That(chunk.U02).IsEqualTo((float?)41.25f);
            await Assert.That(chunk.U03).IsEqualTo((float?)42.25f);
            await Assert.That(chunk.U04).IsEqualTo((float?)43.25f);
        }
        if (version >= 5) await Assert.That(chunk.U05).IsEqualTo((float?)44.25f);
        if (version >= 7)
        {
            await Assert.That(node.PlaneFrontWheelDamperCoef).IsEqualTo((float?)0.6f);
            await Assert.That(node.PlaneStallAngleMaxDeg).IsEqualTo((float?)18.25f);
        }
        await Assert.That(node.PlaneStallAngleMinDeg).IsEqualTo((float?)(version >= 8 ? -6.25f : -5f));
        if (version >= 8)
        {
            await Assert.That(node.PlaneEngineTime01s).IsEqualTo((float?)1.5f);
            await Assert.That(node.PlaneEngineTime10s).IsEqualTo((float?)1.75f);
            await Assert.That(node.PlanePitchGroundControlMultiplier).IsEqualTo((float?)1.25f);
        }
        await Assert.That(node.PlaneDragCoef).IsEqualTo((Vec3?)(version >= 9 ? new Vec3(1.25f, 2.25f, 3.25f) : new Vec3(1f, 1f, 1f)));
        await Assert.That(node.PlaneXDragApplicationPosOffset).IsEqualTo((Vec3?)(version >= 9 ? new Vec3(-1f, -2f, -3f) : default(Vec3)));
        if (version >= 12) await Assert.That(node.PlaneEngineAuto).IsTrue();
        await Assert.That(node.PlaneLanding_Contact_BestTime).IsEqualTo((int?)(version >= 13 ? 2600 : 2500));
        await Assert.That(node.PlaneLanding_Contact_WorseTime).IsEqualTo((int?)(version >= 13 ? 10100 : 10000));
        await Assert.That(node.PlaneLanding_End_BestTime).IsEqualTo((int?)(version >= 13 ? 4600 : 4500));
        await Assert.That(node.PlaneLanding_End_WorseTime).IsEqualTo((int?)(version >= 13 ? 17600 : 17500));
        await Assert.That(node.PlaneLanding_Rebounds_MaxCount).IsEqualTo((int?)(version >= 13 ? 9 : 8));
        if (version >= 14)
        {
            await Assert.That(node.PlaneThrustFromFrontSpeedKmh!.Capacity).IsEqualTo(11);
            await Assert.That(node.PlaneLiftCoefFromStallAngleRatio!.Values).IsEquivalentTo(new Vec2[] { new(1.5f, 2.5f), new(3.5f, 4.5f) }, CollectionOrdering.Matching);
        }
    }

    [Test]
    [Arguments(13)]
    [Arguments(14)]
    public async Task ReadWrite_Plane_PreservesAllEightCurvesAcrossTheInlineFormatBoundary(int version)
    {
        Vec2[] points = [new(-10f, 0.5f), new(90f, 2.5f)];
        PlugCurve CreateCurve() => new() { Values = (Vec2[])points.Clone(), Version = 2, Capacity = 11 };
        var node = new CPlugVehicleCarPhyTuning
        {
            PlaneThrustFromFrontSpeedKmh = CreateCurve(),
            PlaneLiftCoefFromStallAngleRatio = CreateCurve(),
            PlanePitchControlFromSpeedKmh = CreateCurve(),
            PlaneRollControlFromSpeedKmh = CreateCurve(),
            PlaneDragMultiplierFromGroundDist = CreateCurve(),
            PlaneLiftMultiplierFromGroundDist = CreateCurve(),
            PlaneYawGroundControlFromFrontSpeedKmh = CreateCurve(),
            PlaneYawControlFromFrontSpeedKmh = CreateCurve()
        };
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED09F { Version = version };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        var restored = await RoundTrip(payload, new CPlugVehicleCarPhyTuning.Chunk090ED09F());
        var curves = new[]
        {
            restored.PlaneThrustFromFrontSpeedKmh, restored.PlaneLiftCoefFromStallAngleRatio,
            restored.PlanePitchControlFromSpeedKmh, restored.PlaneRollControlFromSpeedKmh,
            restored.PlaneDragMultiplierFromGroundDist, restored.PlaneLiftMultiplierFromGroundDist,
            restored.PlaneYawGroundControlFromFrontSpeedKmh, restored.PlaneYawControlFromFrontSpeedKmh
        };
        foreach (var curve in curves)
        {
            await Assert.That(curve!.Values).IsEquivalentTo(points, CollectionOrdering.Matching);
            if (version < 14) await Assert.That(curve.References.Curve.CachedNode).IsNotNull();
            else
            {
                await Assert.That(curve.References.Curve.CachedNode).IsNull();
                await Assert.That(curve.Version).IsEqualTo(2);
                await Assert.That(curve.Capacity).IsEqualTo(11);
            }
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ReadWrite_Bumper_PreservesVersionedImpulsesAndUnknownVersionTwoFields(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            if (version >= 1)
            {
                writer.Write(-0.025f);
                writer.Write(30f);
                writer.Write(22f);
                writer.Write(80f);
                writer.Write(24f);
            }
            if (version >= 2)
            {
                writer.Write(21f);
                writer.Write(6f);
                writer.Write(0.1f);
                writer.Write(4f);
                writer.Write(1.5f);
            }
        }
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED0A0();
        var node = await RoundTrip(payload, chunk);
        await Assert.That(node.BumperImpulsePositionOffset).IsEqualTo(version >= 1 ? -0.025f : -0.015f);
        await Assert.That(node.BumperGroundNormalImpulse).IsEqualTo(version >= 1 ? 30f : 25f);
        await Assert.That(node.BumperSurfaceNormalImpulse).IsEqualTo(version >= 1 ? 22f : 20f);
        await Assert.That(node.Bumper2GroundNormalImpulse).IsEqualTo(version >= 1 ? 80f : 75f);
        await Assert.That(node.Bumper2SurfaceNormalImpulse).IsEqualTo(version >= 1 ? 24f : 20f);
        await Assert.That(chunk.U01).IsEqualTo(version >= 2 ? 21f : 20f);
        await Assert.That(chunk.U02).IsEqualTo(version >= 2 ? 6f : 5f);
        await Assert.That(chunk.U03).IsEqualTo(version >= 2 ? 0.1f : 0.075f);
        await Assert.That(chunk.U04).IsEqualTo(version >= 2 ? 4f : 3f);
        await Assert.That(chunk.U05).IsEqualTo(version >= 2 ? 1.5f : 1f);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    public async Task ReadWrite_Chunk0A3_PreservesVersionedDataAndOmittedDefaults(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(1.25f);
            writer.Write(2.25f);
            writer.Write(101);
            writer.Write(102);
            if (version >= 1) writer.Write(3.25f);
            if (version >= 2)
            {
                for (var i = 0; i < 4; i++) writer.Write(4.25f + i);
            }
            if (version >= 3) writer.Write(8.25f);
            if (version >= 4)
            {
                writer.WriteNodeRef<CFuncKeysReal>(null);
                writer.Write(9.25f);
                for (var i = 0; i < 3; i++) writer.Write(103 + i);
            }
            if (version >= 5)
            {
                writer.Write(106);
                writer.WriteNodeRef<CFuncKeysReal>(null);
                writer.WriteNodeRef<CFuncKeysReal>(null);
                WriteCurve(writer, version >= 13);
                WriteCurve(writer, version >= 13);
            }
            if (version >= 6) writer.Write(107);
            if (version >= 7) writer.Write(108);
            if (version >= 8) writer.Write(109);
            if (version >= 9)
            {
                writer.Write(10.25f);
                writer.Write(110);
            }
            if (version >= 10) writer.Write(111);
            if (version >= 11) writer.Write(11.25f);
            if (version >= 12)
            {
                WriteCurve(writer, version >= 13);
                WriteCurve(writer, version >= 13);
            }
        }
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED0A3();
        await RoundTrip(payload, chunk);
        await Assert.That(chunk.U05).IsEqualTo(version >= 1 ? 3.25f : 0.45f);
        await Assert.That(chunk.U21).IsEqualTo(version >= 6 ? 107 : 6000);
        await Assert.That(chunk.U25).IsEqualTo(version >= 9 ? 110 : 10000);
        await Assert.That(chunk.U27).IsEqualTo(version >= 11 ? 11.25f : 0.7f);
        if (version < 12)
        {
            await Assert.That(chunk.U28!.Values).IsEquivalentTo(new Vec2[] { new(0f, 0f), new(0.05f, 0.6f), new(0.2f, 0.85f), new(1f, 1f) }, CollectionOrdering.Matching);
        }
        if (version >= 13)
        {
            await Assert.That(chunk.U19!.Capacity).IsEqualTo(11);
            await Assert.That(chunk.U29!.Values).IsEquivalentTo(new Vec2[] { new(1.5f, 2.5f), new(3.5f, 4.5f) }, CollectionOrdering.Matching);
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    [Arguments(14)]
    [Arguments(15)]
    [Arguments(16)]
    [Arguments(17)]
    [Arguments(18)]
    public async Task ReadWrite_Chunk0A4_PreservesSlowMotionAndCompatibilityFields(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(0.6f);
            if (version >= 1) writer.Write(0.2f);
            if (version >= 2) writer.Write(12345);
            if (version >= 3)
            {
                writer.Write(101);
                WriteCurve(writer, version >= 14);
                writer.Write(1.25f);
                writer.Write(2.25f);
            }
            if (version >= 4)
            {
                writer.Write(3.25f);
                writer.Write(102);
                writer.Write(4.25f);
            }
            if (version >= 5) WriteCurve(writer, version >= 14);
            if (version >= 6) WriteCurve(writer, version >= 14);
            if (version >= 7)
            {
                WriteCurve(writer, version >= 14);
                writer.Write(103);
            }
            if (version >= 8)
            {
                WriteCurve(writer, version >= 14);
                WriteCurve(writer, version >= 14);
            }
            if (version >= 9) writer.Write(104);
            if (version >= 10) writer.Write(105);
            if (version >= 11 && version <= 16)
            {
                writer.Write(106);
                writer.Write(107);
                writer.Write(108);
            }
            if (version >= 12) WriteCurve(writer, version >= 14);
            if (version >= 13 && version <= 16) writer.Write(109);
            if (version >= 15) writer.Write(110);
            if (version >= 16) writer.Write(5.25f);
            if (version >= 18) writer.Write(111);
        }
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED0A4();
        var node = await RoundTrip(payload, chunk);
        await Assert.That(node.SlowMotionTimeScaleCoef).IsEqualTo(0.6f);
        await Assert.That(node.SlowMotionTimeScaleMin).IsEqualTo(version >= 1 ? 0.2f : 0.11111111f);
        await Assert.That(node.SlowMotionDuration).IsEqualTo(version >= 2 ? 12345 : 10000);
        await Assert.That(chunk.U21).IsEqualTo(version >= 15 ? 110 : 17);
        await Assert.That(chunk.U22).IsEqualTo(version >= 16 ? 5.25f : 1f);
        await Assert.That(chunk.U23).IsEqualTo(version >= 18 ? 111 : 1);
        if (version >= 14)
        {
            await Assert.That(chunk.U02!.Capacity).IsEqualTo(11);
            await Assert.That(chunk.U19!.Values).IsEquivalentTo(new Vec2[] { new(1.5f, 2.5f), new(3.5f, 4.5f) }, CollectionOrdering.Matching);
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    [Arguments(14)]
    [Arguments(15)]
    [Arguments(16)]
    [Arguments(17)]
    [Arguments(18)]
    [Arguments(19)]
    public async Task ReadWrite_ReactorBoost_PreservesVersionedFieldsAndCurveEncodings(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(1);
            writer.Write(101);
            writer.Write(102);
            for (var i = 0; i < 4; i++) writer.Write(1.25f + i);
            WriteCurve(writer, version >= 19);
            WriteCurve(writer, version >= 19);
            if (version >= 1) writer.Write(103);
            if (version >= 2)
            {
                writer.Write(104);
                WriteCurve(writer, version >= 19);
            }
            if (version >= 3) writer.Write(105);
            if (version >= 4)
            {
                writer.Write(5.25f);
                writer.Write(6.25f);
                writer.Write(106);
                writer.Write(107);
            }
            if (version >= 5)
            {
                writer.Write(108);
                writer.Write(109);
            }
            if (version >= 6) writer.WriteNodeRef<CFuncKeysReal>(null);
            if (version >= 7) writer.Write(110);
            if (version >= 8) WriteCurve(writer, version >= 19);
            if (version >= 9)
            {
                WriteCurve(writer, version >= 19);
                WriteCurve(writer, version >= 19);
                writer.Write(111);
            }
            if (version >= 10)
            {
                writer.Write(7.25f);
                WriteCurve(writer, version >= 19);
            }
            if (version >= 11) WriteCurve(writer, version >= 19);
            if (version >= 12)
            {
                writer.Write(8.25f);
                writer.Write(9.25f);
            }
            if (version >= 13) WriteCurve(writer, version >= 19);
            if (version >= 14)
            {
                WriteCurve(writer, version >= 19);
                WriteCurve(writer, version >= 19);
            }
            if (version >= 15) writer.Write(112);
            if (version >= 16) WriteCurve(writer, version >= 19);
            if (version >= 17) WriteCurve(writer, version >= 19);
            if (version >= 18)
            {
                writer.Write(10.25f);
                writer.Write(11.25f);
            }
        }
        var chunk = new CPlugVehicleCarPhyTuning.Chunk090ED0A7();
        var node = await RoundTrip(payload, chunk);
        await Assert.That(node.ReactorBoostEnabled).IsEqualTo(1);
        await Assert.That(node.ReactorBoostDuration).IsEqualTo(102);
        await Assert.That(chunk.U08).IsEqualTo(version >= 1 ? 103 : 30);
        await Assert.That(chunk.U23).IsEqualTo(version >= 9 ? 111 : 500);
        await Assert.That(chunk.U32).IsEqualTo(version >= 15 ? 112 : 1000);
        await Assert.That(chunk.U35).IsEqualTo(version >= 18 ? 10.25f : 0.35f);
        if (version >= 19)
        {
            await Assert.That(chunk.U06!.Capacity).IsEqualTo(11);
            await Assert.That(chunk.U34!.Values).IsEquivalentTo(new Vec2[] { new(1.5f, 2.5f), new(3.5f, 4.5f) }, CollectionOrdering.Matching);
        }
        if (version < 16)
        {
            await Assert.That(chunk.U33!.Values).IsEquivalentTo(new Vec2[] { new(0f, 0f), new(100f, 15f), new(500f, 20f) }, CollectionOrdering.Matching);
        }
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
