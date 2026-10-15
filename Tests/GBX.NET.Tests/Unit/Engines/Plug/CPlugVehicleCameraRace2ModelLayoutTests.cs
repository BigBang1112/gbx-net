using GBX.NET.Engines.Function;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVehicleCameraRace2ModelLayoutTests
{
    [Test]
    [Arguments(0, 3)]
    [Arguments(1, 3)]
    [Arguments(2, 3)]
    [Arguments(3, 3)]
    [Arguments(4, 0)]
    [Arguments(4, 1)]
    [Arguments(4, 2)]
    [Arguments(4, 3)]
    [Arguments(4, 4)]
    public async Task ReadWrite_VersionedCameraSettings_PreserveTheirWireLayout(int version, int curveVersion)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.WriteIdAsString("Camera");
            for (var i = 0; i < 5; i++) writer.Write(100.25f + i);
            for (var i = 0; i < (version >= 3 ? 4 : version >= 2 ? 2 : 0); i++)
            {
                writer.Write(800.5f + i);
                writer.Write(900 + i);
                writer.Write(1000 + i);
            }
            if (version >= 4)
            {
                WriteSettings(writer);
                for (var i = 0; i < 3; i++)
                {
                    writer.Write(curveVersion);
                    writer.Write(2);
                    if (curveVersion == 1) writer.Write((byte)1);
                    if (curveVersion == 2) writer.Write(7);
                    if (curveVersion >= 3) writer.Write((ushort)0x12);
                    writer.Write(new Vec2(i + 1, i + 2));
                    writer.Write(new Vec2(i + 3, i + 4));
                }
                for (var i = 0; i < 4; i++) writer.Write(700.25f + i);
            }
            writer.Write(0x12345678);
        }
        var length = (int)payload.Length - 4;
        payload.Position = 0;
        var node = new CPlugVehicleCameraRace2Model();
        var chunk = new CPlugVehicleCameraRace2Model.Chunk090F6002();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(payload.Position).IsEqualTo(length);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        }
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.MinDist).IsEqualTo(100.25f);
        await Assert.That(node.DefaultFov).IsEqualTo(104.25f);
        if (version >= 4)
        {
            await AssertSettings(node);
            await Assert.That(node.RollFromSpeed!.Values).IsEquivalentTo(
                new[] { new Vec2(3, 4), new Vec2(5, 6) }, CollectionOrdering.Matching);
            await Assert.That(node.LookAtFactorFromUpSpeedRatio!.Count).IsEqualTo(2);
            await Assert.That(node.DeltaLookAtFactorDamperKa).IsEqualTo(703.25f);
        }

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..length], CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_LegacySettings_PreserveCurveReferences(bool hasCurve)
    {
        var curve = new CFuncKeysReal
        {
            Xs = [1, 2],
            Ys = [3, 4],
            RealInterp = CFuncKeysReal.ERealInterp.Hermite,
            ForceTangentMaxX = true
        };
        curve.CreateChunk<CFuncKeys.Chunk05002001>();
        curve.CreateChunk<CFuncKeysReal.Chunk0501A002>();
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteSettings(writer);
            for (var i = 0; i < 3; i++) writer.WriteNodeRef(hasCurve ? curve : null);
            for (var i = 0; i < 4; i++) writer.Write(700.25f + i);
        }
        payload.Position = 0;
        var node = new CPlugVehicleCameraRace2Model();
        var chunk = new CPlugVehicleCameraRace2Model.Chunk090F6000();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await AssertSettings(node);
        if (hasCurve)
        {
            await Assert.That(node.YawFromSpeed!.Curve).IsSameReferenceAs(node.RollFromSpeed!.Curve);
            await Assert.That(node.LookAtFactorFromUpSpeedRatio!.Values).IsEquivalentTo(
                new[] { new Vec2(1, 3), new Vec2(2, 4) }, CollectionOrdering.Matching);
            await Assert.That(node.RollFromSpeed.Flags).IsEqualTo((ushort)0x30);
        }
        else
        {
            await Assert.That(node.RollFromSpeed!.Count).IsEqualTo(0);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);

        // The current camera format uses the same settings with inline curves.
        var currentChunk = new CPlugVehicleCameraRace2Model.Chunk090F6002(GameVersion.TM2020);
        using var current = new MemoryStream();
        using (var writer = new GbxWriter(current))
        using (var rw = new GbxReaderWriter(writer)) currentChunk.ReadWrite(node, rw);
        current.Position = 0;
        var restored = new CPlugVehicleCameraRace2Model();
        using (var reader = new GbxReader(current))
        using (var rw = new GbxReaderWriter(reader)) currentChunk.ReadWrite(restored, rw);
        await Assert.That(current.Position).IsEqualTo(current.Length);
        await Assert.That(restored.RollFromSpeed!.Values).IsEquivalentTo(node.RollFromSpeed!.Values!, CollectionOrdering.Matching);
        await Assert.That(restored.RollFromSpeed.Flags).IsEqualTo(node.RollFromSpeed.Flags);
        await AssertSettings(restored);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_RollInputChunk_PreservesItsBoolean(bool value)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x090F6001u);
            writer.Write(value);
            writer.Write(0xFACADE01u);
        }
        payload.Position = 0;
        CPlugVehicleCameraRace2Model node;
        using (var reader = new GbxReader(payload)) node = reader.ReadNode<CPlugVehicleCameraRace2Model>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.IsRollFromInput).IsEqualTo(value);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeCameraSettings()
    {
        var node = new CPlugVehicleCameraRace2Model();
        await Assert.That(node.MinDist).IsEqualTo(0.1f);
        await Assert.That(node.DefaultFov).IsEqualTo(75f);
        await Assert.That(node.InputBrakeDistDelta).IsEqualTo(-0.7f);
        await Assert.That(node.InputLeftSteerYawTimeUp).IsEqualTo(2500);
        await Assert.That(node.StateFlyingDurationBeforeFlyingMode).IsEqualTo(200);
        await Assert.That(node.IsRollFromInput).IsTrue();
        await Assert.That(node.RollFromSpeed!.Values![4]).IsEqualTo(new Vec2(200, 2.5f));
        await Assert.That(node.YawFromSpeed!.Values![5]).IsEqualTo(new Vec2(400, 14));
        await Assert.That(node.LookAtFactorFromUpSpeedRatio!.Values![6]).IsEqualTo(new Vec2(60, -0.6f));
        await Assert.That(new CPlugVehicleCameraRace2Model.Chunk090F6002(GameVersion.TMT).Version).IsEqualTo(1);
        await Assert.That(new CPlugVehicleCameraRace2Model.Chunk090F6002(GameVersion.MP4).Version).IsEqualTo(1);
        await Assert.That(new CPlugVehicleCameraRace2Model.Chunk090F6002(GameVersion.TM2020).Version).IsEqualTo(4);
    }

    private static async Task AssertSettings(CPlugVehicleCameraRace2Model node)
    {
        await Assert.That(node.InputGasDistDelta).IsEqualTo(0.25f);
        await Assert.That(node.InputGasDistTimeUp).IsEqualTo(-2);
        await Assert.That(node.InputGasDistTimeDown).IsEqualTo(-3);
        await Assert.That(node.InputRightSteerRollTimeDown).IsEqualTo(-39);
        await Assert.That(node.InputLeftSteerYawTimeUp).IsEqualTo(-41);
        await Assert.That(node.StateFlyingDurationBeforeFlyingMode).IsEqualTo(-48);
        await Assert.That(node.StateFlyingDurationBeforeCameraMove).IsEqualTo(-49);
    }

    private static void WriteSettings(GbxWriter writer)
    {
        int[] integerIndices = [1, 2, 4, 5, 7, 8, 10, 11, 13, 14, 16, 17, 19, 20, 22, 23, 25, 26, 28, 29, 31, 32, 34, 35, 37, 38, 40, 41, 43, 44, 47, 48, 49, 50, 51, 52];
        for (var i = 0; i < 55; i++)
        {
            if (Array.IndexOf(integerIndices, i) >= 0) writer.Write(-i - 1);
            else writer.Write(i + 0.25f);
        }
    }
}
