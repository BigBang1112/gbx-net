using GBX.NET.Engines.Function;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVehicleCameraRace3ModelLayoutTests
{
    [Test]
    [Arguments(-1, 0)]
    [Arguments(0, 0)]
    [Arguments(1, 0)]
    [Arguments(2, 0)]
    [Arguments(3, 0)]
    [Arguments(4, 0)]
    [Arguments(5, 0)]
    [Arguments(6, 0)]
    [Arguments(7, 0)]
    [Arguments(8, 0)]
    [Arguments(8, 1)]
    [Arguments(8, 2)]
    [Arguments(8, 3)]
    [Arguments(8, 4)]
    public async Task ReadWrite_CameraSettings_PreserveVersionedWireLayout(int version, int curveVersion)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (version >= 0) writer.Write(version);
            WriteSettings(writer);
            for (var i = 0; i < (version >= 2 ? 4 : 3); i++)
            {
                if (version < 8)
                {
                    writer.WriteNodeRef<CFuncKeysReal>(null);
                }
                else
                {
                    writer.Write(curveVersion);
                    writer.Write(2);
                    if (curveVersion == 1) writer.Write((byte)1);
                    if (curveVersion == 2) writer.Write(7);
                    if (curveVersion >= 3) writer.Write((ushort)0x32);
                    writer.Write(new Vec2(i + 1, i + 2));
                    writer.Write(new Vec2(i + 3, i + 4));
                }
            }
            if (version >= 3) writer.Write(701f);
            if (version >= 5)
            {
                writer.Write(702f);
                writer.Write(703f);
                writer.Write(704f);
            }
            for (var i = 0; i < (version >= 7 ? 6 : version >= 6 ? 3 : 0); i++)
            {
                writer.Write(i + 800.5f);
                writer.Write(900 + i);
                writer.Write(1000 + i);
            }
            writer.Write(0x12345678);
        }

        var length = (int)payload.Length - 4;
        payload.Position = 0;
        var node = new CPlugVehicleCameraRace3Model();
        Chunk<CPlugVehicleCameraRace3Model> chunk = version < 0
            ? new CPlugVehicleCameraRace3Model.Chunk090EF000()
            : new CPlugVehicleCameraRace3Model.Chunk090EF005();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(payload.Position).IsEqualTo(length);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        }
        await Assert.That(node.SlerpSpeed).IsEqualTo(0.25f);
        await Assert.That(node.FlyDurationBeforeFlyingBehavior).IsEqualTo(version < 0 ? 100 : -4);
        await Assert.That(node.SlerpTargetPosNormalBehaviorTimeUp).IsEqualTo(-10);
        await Assert.That(node.SlerpTargetPosNormalBehaviorTimeDown).IsEqualTo(-11);
        await Assert.That(node.EventBurningRadiusTimeDown).IsEqualTo(-65);
        if (version >= 3) await Assert.That(node.VehiclePitchCoef).IsEqualTo(701f);
        if (version >= 5) await Assert.That(node.AirSpawn_VelY1).IsEqualTo(704f);
        if (version >= 8)
        {
            await Assert.That(node.SlerpSpeedModulationFromSpeed!.Values).IsEquivalentTo(
                new[] { new Vec2(1, 2), new Vec2(3, 4) }, CollectionOrdering.Matching);
            await Assert.That(node.DeltaRadiusFromSpeedKmh!.Values).IsEquivalentTo(
                new[] { new Vec2(4, 5), new Vec2(6, 7) }, CollectionOrdering.Matching);
        }

        if (version < 0)
        {
            payload.Position = 12;
            using var writer = new GbxWriter(payload);
            writer.Write(100);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..length], CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_LegacyCurveReferences_BecomeInlineValues()
    {
        var curve = new CFuncKeysReal
        {
            Xs = [1, 2],
            Ys = [3, 4],
            RealInterp = CFuncKeysReal.ERealInterp.Hermite
        };
        curve.CreateChunk<CFuncKeys.Chunk05002001>();
        curve.CreateChunk<CFuncKeysReal.Chunk0501A002>();
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(5);
            WriteSettings(writer);
            for (var i = 0; i < 4; i++) writer.WriteNodeRef(curve);
            for (var i = 0; i < 4; i++) writer.Write(700f + i);
        }
        payload.Position = 0;
        var node = new CPlugVehicleCameraRace3Model();
        var chunk = new CPlugVehicleCameraRace3Model.Chunk090EF005();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.SlerpSpeedModulationFromSpeed!.Curve).IsSameReferenceAs(node.DeltaRadiusFromSpeedKmh!.Curve);
        await Assert.That(node.SlerpSpeedModulationFromSpeed.Values).IsEquivalentTo(
            new[] { new Vec2(1, 3), new Vec2(2, 4) }, CollectionOrdering.Matching);
        await Assert.That(node.SlerpSpeedModulationFromSpeed.Flags).IsEqualTo((ushort)0x10);

        chunk.Version = 8;
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        saved.Position = 0;
        var restored = new CPlugVehicleCameraRace3Model();
        using (var reader = new GbxReader(saved))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(restored, rw);
        await Assert.That(saved.Position).IsEqualTo(saved.Length);
        await Assert.That(restored.DeltaRadiusFromSpeedKmh!.Values).IsEquivalentTo(
            new[] { new Vec2(1, 3), new Vec2(2, 4) }, CollectionOrdering.Matching);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeCameraSettings()
    {
        var node = new CPlugVehicleCameraRace3Model();
        await Assert.That(node.Up).IsEqualTo(2f);
        await Assert.That(node.DefaultFov).IsEqualTo(90f);
        await Assert.That(node.InputGasFarTimeUp).IsEqualTo(2000);
        await Assert.That(node.EventTurboFarDelta).IsEqualTo(-0.6f);
        await Assert.That(node.SlerpSpeedModulationFromSpeed!.Values!.Length).IsEqualTo(6);
        await Assert.That(node.DeltaRadiusFromSpeedKmh!.Values![0]).IsEqualTo(new Vec2(0, 0));
        await Assert.That(new CPlugVehicleCameraRace3Model.Chunk090EF005(GameVersion.MP4).Version).IsEqualTo(5);
        await Assert.That(new CPlugVehicleCameraRace3Model.Chunk090EF005(GameVersion.TM2020).Version).IsEqualTo(8);
    }

    [Test]
    public async Task ReadWrite_LookAtAndOrientationChunks_ExposeNamedSettings()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x090EF001u);
            writer.Write(2.5f);
            writer.Write(0x090EF002u);
            writer.Write(0.75f);
            writer.Write(95f);
            writer.Write(0x090EF003u);
            writer.Write(true);
            writer.Write(false);
            writer.Write(0x090EF004u);
            writer.WriteIdAsString("Camera");
            writer.Write(0xFACADE01u);
        }
        payload.Position = 0;
        CPlugVehicleCameraRace3Model node;
        using (var reader = new GbxReader(payload)) node = reader.ReadNode<CPlugVehicleCameraRace3Model>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.LookAtFactor).IsEqualTo(0.75f);
        await Assert.That(node.DefaultFov).IsEqualTo(95f);
        await Assert.That(node.FollowWheelsPlane).IsTrue();
        await Assert.That(node.ForceUpV010).IsFalse();

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    private static void WriteSettings(GbxWriter writer)
    {
        int[] integerIndices = [3, 4, 5, 9, 10, 12, 13, 15, 16, 18, 19, 22, 23, 27, 28, 30, 31, 34, 35, 42, 43, 45, 46, 48, 49, 51, 52, 54, 55, 57, 58, 60, 61, 63, 64];
        for (var i = 0; i < 65; i++)
        {
            if (Array.IndexOf(integerIndices, i) >= 0) writer.Write(-i - 1);
            else writer.Write(i + 0.25f);
        }
    }
}
