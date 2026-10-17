using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Tests.Unit.Engines.GameData;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVehicleVisModelTests
{
    [Test]
    [Arguments(0, 0, "")]
    [Arguments(1, 0, "")]
    [Arguments(2, 0, "")]
    [Arguments(3, 0, "")]
    [Arguments(4, 0, "")]
    [Arguments(5, 1, "")]
    [Arguments(6, 1, "")]
    [Arguments(7, 2, "")]
    [Arguments(8, 2, "Vehicle.Mesh.Gbx")]
    [Arguments(9, 1, "")]
    [Arguments(12, 2, "")]
    [Arguments(13, 2, "")]
    [Arguments(14, 2, "")]
    [Arguments(15, 2, "")]
    [Arguments(16, 2, "")]
    [Arguments(17, 2, "")]
    [Arguments(18, 2, "")]
    public async Task ReadWrite_Model_ReadsNativeGeometryAndVersionBranches(int version, int surfVersion, string meshPath)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(version);
        writer.Write(-1); // Shared
        var legacyFile = payload.NodeRef("LegacyBody.Solid.Gbx");
        writer.NodeDict.Add(legacyFile, 1);
        writer.Write(-1);
        writer.Write(-1);
        if (version >= 5) writer.Write(surfVersion);

        var horn = new CPlugLocatedSound();
        for (var i = 0; i < 11; i++) writer.WriteNodeRef(i is 0 or 2 ? horn : null);
        if (version >= 2) writer.Write(-1);
        if (version >= 6) writer.Write(-1);
        if (version >= 8) writer.Write(-1);
        if (version >= 13) WriteNullRefs(writer, 2);
        if (version >= 14) writer.Write(-1);
        if (version >= 15) writer.Write(-1);
        if (version >= 16) WriteNullRefs(writer, 2);
        if (version >= 1)
        {
            writer.Write(-1.25f);
            writer.Write(0.75f);
        }
        if (version >= 9)
        {
            writer.Write(25f);
            writer.Write(1.5f);
        }
        if (version >= 3)
        {
            writer.Write(-1); // Mesh
            writer.Write(false);
            for (var i = 0; i < 4; i++) WriteDamagePart(writer, i, surfVersion);
            for (var i = 0; i < 5; i++)
            {
                writer.WriteArray<ushort>([(ushort)i, (ushort)(i + 1), (ushort)(i + 2)]);
                writer.WriteArray<Vec3>([new(i, 2, 3)]);
                writer.WriteIdAsString($"Glass{i}");
                writer.WriteIdAsString($"Part{i}");
            }
            for (var i = 0; i < 9; i++) writer.Write(new Vec3(i, i + 1, i + 2));
        }
        if (version >= 7)
        {
            writer.Write(meshPath);
            if (meshPath.Length == 0) writer.Write(-1);
        }
        if (version is >= 12 and <= 17) writer.WriteNodeRef(new CPlugVFXFile());
        if (version >= 17) writer.WriteNodeRef(new CPlugFxSystem());
        payload.Finish();

        var node = new CPlugVehicleVisModel();
        var chunk = new CPlugVehicleVisModel.Chunk090E7000();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(chunk.References.U01.File!.FilePath).IsEqualTo("LegacyBody.Solid.Gbx");
        await Assert.That(node.Sounds.Horn).IsNotNull();
        await Assert.That(ReferenceEquals(node.Sounds.Horn, node.Sounds.Engine)).IsTrue();
        await Assert.That(node.Sounds.WheelSurfaceLeft).IsNull();
        if (version >= 3)
        {
            await Assert.That(node.GeomModel.DamageParts!.Length).IsEqualTo(4);
            for (var i = 0; i < 4; i++)
            {
                var part = node.GeomModel.DamageParts[i];
                var sphere = (CPlugSurface.Sphere)part.Surf!;
                await Assert.That(part.Dyna.LinearMass).IsEqualTo(i + 1f);
                await Assert.That(part.Dyna.CenterOfMass).IsEqualTo(new Vec3(i, 2, 3));
                await Assert.That(sphere.Size).IsEqualTo(i + 0.5f);
                if (surfVersion >= 1) await Assert.That(sphere.SurfaceIndex).IsEqualTo((short)(i + 10));
                if (surfVersion >= 2) await Assert.That(sphere.GameplayMainDir).IsEqualTo(new Vec3(i, 1, 0));
            }
            await Assert.That(node.GeomModel.Glass![4].GlassId).IsEqualTo("Glass4");
            await Assert.That(node.GeomModel.Glass[4].RigidPartId).IsEqualTo("Part4");
            await Assert.That(node.GeomModel.Glass[4].Indices).IsEquivalentTo(new ushort[] { 4, 5, 6 }, CollectionOrdering.Matching);
            await Assert.That(node.GeomModel.U02![8]).IsEqualTo(new Vec3(8, 9, 10));
        }
        if (version >= 7) await Assert.That(node.GeomModel.MeshRefPath).IsEqualTo(meshPath);
        if (version >= 17) await Assert.That(node.FxSystem).IsNotNull();
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    public async Task ReadWrite_Impact_ConvertsLegacyThresholdsWithoutMutatingOnWrite(int version)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(version);
        var thresholds = version < 5 ? new[] { 32f, 6f, 40f, 6f } : [115.2f, 21.6f, 144f, 21.6f];
        foreach (var value in thresholds) writer.Write(value);
        if (version is >= 1 and <= 3) writer.Write(-1);
        if (version is >= 2 and <= 3) writer.Write(-1);
        if (version >= 3) writer.Write(-1);
        if (version >= 4)
        {
            writer.Write(new Vec3(1, 2, 3));
            writer.Write(new Vec3(4, 5, 6));
        }
        payload.Finish();

        var node = new CPlugVehicleVisModel();
        var chunk = new CPlugVehicleVisModel.Chunk090E7002();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.SoundWheelImpact_Hard_ThresholdKmh).IsEqualTo(115.2f);
        await Assert.That(node.SoundBodyImpact_Hard_ThresholdKmh).IsEqualTo(144f);
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.SoundWheelImpact_Hard_ThresholdKmh).IsEqualTo(115.2f);
        if (version >= 4) await Assert.That(node.SoundImpactWheelBackPos).IsEqualTo(new Vec3(4, 5, 6));

        node.SoundWheelImpact_Hard_ThresholdKmh = 72;
        using var edited = new MemoryStream();
        using (var editedWriter = new GbxWriter(edited))
        using (var rw = new GbxReaderWriter(editedWriter)) chunk.ReadWrite(node, rw);
        edited.Position = 0;
        using var editedReader = new GbxReader(edited);
        await Assert.That(editedReader.ReadInt32()).IsEqualTo(version);
        await Assert.That(editedReader.ReadSingle()).IsEqualTo(version < 5 ? 20f : 72f);
        await Assert.That(node.SoundWheelImpact_Hard_ThresholdKmh).IsEqualTo(72f);
    }

    [Test]
    [Arguments(0, 1)]
    [Arguments(1, 2)]
    [Arguments(2, 3)]
    [Arguments(3, 4)]
    [Arguments(4, 7)]
    [Arguments(5, 9)]
    [Arguments(6, 11)]
    [Arguments(7, 13)]
    [Arguments(8, 15)]
    [Arguments(9, 17)]
    [Arguments(10, 20)]
    public async Task ReadWrite_Events_ReadsNativeCountsAndLegacyWheelJump(int version, int count)
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(version);
        for (var i = 0; i < count; i++) payload.Writer.WriteNodeRef(i == 3 ? new CPlugLocatedSound() : null);
        payload.Finish();

        var node = new CPlugVehicleVisModel();
        var chunk = new CPlugVehicleVisModel.Chunk090E7001();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        if (version == 3)
        {
            await Assert.That(node.SoundEvents.WheelLand).IsNotNull();
            await Assert.That(ReferenceEquals(node.SoundEvents.WheelLand, node.SoundEvents.WheelJump)).IsTrue();
        }
        if (version >= 4) await Assert.That(node.SoundEvents.WheelJump).IsNull();
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    private static void WriteNullRefs(GbxWriter writer, int count)
    {
        for (var i = 0; i < count; i++) writer.Write(-1);
    }

    private static void WriteDamagePart(GbxWriter writer, int index, int surfVersion)
    {
        writer.Write(-1); // Mesh
        writer.Write(0); // CPlugDynaModel archive version
        writer.Write(index + 1f);
        writer.Write(0.3f);
        writer.Write(new Vec3(index, 2, 3));
        for (var i = 0; i < 9; i++) writer.Write(i % 4 == 0 ? 1f : 0f);
        writer.Write(10000f);
        writer.Write(0); // Sphere
        writer.Write(index + 0.5f);
        if (surfVersion >= 1) writer.Write((short)(index + 10));
        if (surfVersion >= 2) writer.Write(new Vec3(index, 1, 0));
    }
}
