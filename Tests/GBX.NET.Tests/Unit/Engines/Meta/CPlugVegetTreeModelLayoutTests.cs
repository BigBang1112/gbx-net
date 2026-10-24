using GBX.NET.Engines.Meta;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Meta;

[Category("Unit")]
public class CPlugVegetTreeModelLayoutTests
{
    [Test]
    public async Task ReadWrite_Version21_KeepsNativeOrder()
    {
        var node = new CPlugVegetTreeModel();

        await RoundTrip(TreeModelPayload(21), rw => node.ReadWrite(rw));

        await Assert.That(node.Version).IsEqualTo(21);
        await Assert.That(node.U01).IsEqualTo(7);
        await Assert.That(node.Materials!.Length).IsEqualTo(1);
        await Assert.That(node.Materials[0].U09).IsEqualTo((byte)5);
        await Assert.That(node.LodModels![0].ShadedGeoms![0].ModelMaterialIndex).IsEqualTo((ushort)3);
        await Assert.That(node.LodModels[0].ShadedGeoms![0].U02).IsEqualTo((byte)1);
        await Assert.That(node.LodMaxDists).IsEquivalentTo(new[] { 80f }, CollectionOrdering.Matching);
        await Assert.That(node.LodDistAtFov90).IsEqualTo(120f);
        await Assert.That(node.PlaneComputeMode).IsEqualTo(CPlugVegetTreeModel.EPlaneComputeMode.HorizontalPlaneBottom);
        await Assert.That(node.AngleMax_RotXZ_Deg).IsEqualTo(10f);
        await Assert.That(node.ScaleVar01).IsEqualTo(0.3f);
        await Assert.That(node.EnableRandomRotationY).IsFalse();
        await Assert.That(node.Force_No_Collision).IsTrue();
        await Assert.That(node.PropagationAtlasGrid).IsEqualTo(new Int2(4, 2));
        await Assert.That(node.PropagationLeafSize).IsEqualTo(0.2f);
        await Assert.That(node.PropagationGroundGenPointCount).IsEqualTo(500);
        await Assert.That(node.PropagationLeafPerSecondCount).IsEqualTo(30);
        await Assert.That(node.PropagationConeHalfAngleDeg).IsEqualTo(40f);
        await Assert.That(node.PropagationEmissionSpawnRadius).IsEqualTo(1.5f);
        await Assert.That(node.PropagationEnable).IsTrue();
        await Assert.That(node.PropagationEmissionPositions).IsEquivalentTo(new[] { new Vec3(1, 2, 3) }, CollectionOrdering.Matching);
        await Assert.That(node.AdditionalData).IsNull();
        await Assert.That(node.Impostor_AllPlanesVisible).IsTrue();
        await Assert.That(node.ReceivesPSSM).IsFalse();
    }

    [Test]
    public async Task ReadWrite_Version20_KeepsReceivesPSSMEnabled()
    {
        var node = new CPlugVegetTreeModel();

        await RoundTrip(TreeModelPayload(20), rw => node.ReadWrite(rw));

        await Assert.That(node.Impostor_AllPlanesVisible).IsTrue();
        await Assert.That(node.ReceivesPSSM).IsTrue();
    }

    [Test]
    public async Task ReadWrite_Version14ShadedGeom_ReadsModelMaterialIndexAsInt32()
    {
        var geometry = new CPlugVegetTreeModel.Geometry();
        var payload = Payload(w =>
        {
            w.Write(3);
            w.Write(-1); // Visual
        });

        await RoundTrip(payload, rw => geometry.ReadWrite(rw, 14));

        await Assert.That(geometry.ModelMaterialIndex).IsEqualTo((ushort)3);
        await Assert.That(geometry.U02).IsEqualTo((byte)0);
    }

    [Test]
    public async Task ReadWrite_Version15ShadedGeom_ReadsModelMaterialIndexAsUInt16()
    {
        var geometry = new CPlugVegetTreeModel.Geometry();
        var payload = Payload(w =>
        {
            w.Write((ushort)3);
            w.Write(-1); // Visual
            w.Write((byte)1);
        });

        await RoundTrip(payload, rw => geometry.ReadWrite(rw, 15));

        await Assert.That(geometry.ModelMaterialIndex).IsEqualTo((ushort)3);
        await Assert.That(geometry.U02).IsEqualTo((byte)1);
    }

    [Test]
    public async Task Constructor_Defaults_MatchNativeConstructor()
    {
        var node = new CPlugVegetTreeModel();

        await Assert.That(node.U01).IsEqualTo(-1);
        await Assert.That(node.U02).IsEqualTo((byte)1);
        await Assert.That(node.LodDistAtFov90).IsEqualTo(150f);
        await Assert.That(node.PlaneComputeMode).IsEqualTo(CPlugVegetTreeModel.EPlaneComputeMode.Zero);
        await Assert.That(node.U05).IsEqualTo((byte)2);
        await Assert.That(node.AngleMax_RotXZ_Deg).IsEqualTo(1f);
        await Assert.That(node.ScaleVar01).IsEqualTo(0.1f);
        await Assert.That(node.EnableRandomRotationY).IsTrue();
        await Assert.That(node.Force_No_Collision).IsFalse();
        await Assert.That(node.PropagationAtlasGrid).IsEqualTo(new Int2(1, 1));
        await Assert.That(node.PropagationLeafSize).IsEqualTo(0.1f);
        await Assert.That(node.PropagationGroundGenPointCount).IsEqualTo(1000);
        await Assert.That(node.PropagationLeafPerSecondCount).IsEqualTo(20);
        await Assert.That(node.PropagationConeHalfAngleDeg).IsEqualTo(45f);
        await Assert.That(node.PropagationEmissionSpawnRadius).IsEqualTo(2f);
        await Assert.That(node.PropagationEnable).IsFalse();
        await Assert.That(node.Impostor_AllPlanesVisible).IsFalse();
    }

    [Test]
    public async Task AdditionalConstructor_Defaults_MatchNativeConstructor()
    {
        var additional = new CPlugVegetTreeModel.Additional();

        await Assert.That(additional.U02).IsEqualTo(-1f);
        await Assert.That(additional.U04).IsEqualTo(float.MaxValue);
        await Assert.That(additional.U05).IsEqualTo(float.MaxValue);
        await Assert.That(additional.U06).IsEqualTo(float.MinValue);
        await Assert.That(additional.U07).IsEqualTo(float.MinValue);
        await Assert.That(additional.U08).IsEqualTo(float.MaxValue);
        await Assert.That(additional.U09).IsEqualTo(float.MaxValue);
        await Assert.That(additional.U10).IsEqualTo(float.MinValue);
        await Assert.That(additional.U11).IsEqualTo(float.MinValue);
    }

    private static byte[] TreeModelPayload(int version) => Payload(w =>
    {
        w.Write(version);
        w.Write(7); // U01
        w.Write(1); // LOD model count
        w.Write(1); // LOD max distance count

        w.Write(1); // Materials
        for (var i = 0; i < 9; i++)
        {
            w.Write(-1); // Bitmaps, images, variation, subsurface and material
        }
        w.Write((byte)5);

        w.Write(0); // Texture names

        w.Write(1); // Shaded geoms
        w.Write((ushort)3);
        w.Write(-1); // Visual
        w.Write((byte)1);

        w.Write(80f);
        w.Write((byte)1);
        w.Write(120f);
        w.Write((byte)4); // HorizontalPlaneBottom
        w.Write((byte)2);
        w.Write(2UL);
        w.Write(10f);
        w.Write(0.3f);
        w.Write(0); // EnableRandomRotationY
        w.Write(1); // Surface version
        w.Write(-1); // No surface
        w.Write(1); // Force_No_Collision

        w.Write(-1); // Propagation base color
        w.Write(-1); // Propagation normal
        w.Write(4);
        w.Write(2);
        w.Write(0.2f);
        w.Write(500);
        w.Write(30);
        w.Write(40f);
        w.Write(1.5f);
        w.Write(1); // PropagationEnable
        w.Write(1);
        w.Write(1f);
        w.Write(2f);
        w.Write(3f);

        w.Write(0); // No additional data
        w.Write(1); // Impostor_AllPlanesVisible

        if (version >= 21)
        {
            w.Write(0); // ReceivesPSSM
        }
    });

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffixWriter = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffixWriter.Write(0xDEADBEEFu);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var readWrite = new GbxReaderWriter(reader);
        serialize(readWrite);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        serialize(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
