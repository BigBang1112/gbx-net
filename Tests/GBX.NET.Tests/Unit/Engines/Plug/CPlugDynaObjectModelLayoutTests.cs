using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Tests.Unit.Engines.GameData;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugDynaObjectModelLayoutTests
{
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
    public async Task ReadWrite_VersionedArchive_PreservesNativeFieldOrderAndExternalReferences(int version)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(version);
        if (version >= 9) writer.Write(true);
        if (version >= 11) writer.Write(true);
        payload.NodeRef("Mesh.Solid.Gbx");
        payload.NodeRef("Dyna.Shape.Gbx");
        if (version <= 2) payload.NodeRef("Dyna.Model.Gbx");
        if (version >= 1) payload.NodeRef("Static.Shape.Gbx");
        if (version >= 2) writer.Write(123.5f);
        if (version >= 3) writer.Write(12.5f);
        if (version >= 5)
        {
            writer.Write(2.5f);
            writer.Write(3.5f);
        }
        if (version >= 4)
        {
            writer.Write(101);
            writer.Write(102);
            writer.Write((byte)23);
            writer.Write((byte)24);
            writer.Write(105);
            writer.Write(106);
        }
        if (version >= 6)
        {
            writer.Write((byte)27);
            writer.Write(108);
            writer.Write(109);
        }
        if (version >= 7) payload.NodeRef("Loc.Anim.Gbx");
        if (version >= 8) writer.Write(110);
        if (version >= 10) writer.Write(true);
        if (version == 12) writer.Write(111);
        if (version >= 13) payload.NodeRef("Water.Model.Gbx");
        payload.Finish();

        var node = new CPlugDynaObjectModel { LocAnimIsPhysical = true };
        payload.Read(rw => node.ReadWrite(rw));
        await Assert.That(node.Version).IsEqualTo(version);
        await Assert.That(node.IsStatic).IsEqualTo(version >= 9);
        await Assert.That(node.DynamizeOnSpawn).IsEqualTo(version >= 11);
        await Assert.That(node.References.Mesh.File!.FilePath).IsEqualTo("Mesh.Solid.Gbx");
        await Assert.That(node.References.DynaShape.File!.FilePath).IsEqualTo("Dyna.Shape.Gbx");
        await Assert.That(node.References.StaticShape.File!.FilePath).IsEqualTo(version == 0 ? "Dyna.Shape.Gbx" : "Static.Shape.Gbx");
        if (version <= 2) await Assert.That(node.References.DynaModel.File!.FilePath).IsEqualTo("Dyna.Model.Gbx");
        else await Assert.That(node.References.DynaModel.File).IsNull();
        await Assert.That(node.BreakSpeedKmh).IsEqualTo(version >= 2 ? 123.5f : 100f);
        await Assert.That(node.Mass).IsEqualTo(version >= 3 ? 12.5f : 10f);
        await Assert.That(node.LightAliveDurationScMin).IsEqualTo(version >= 5 ? 2.5f : 5f);
        await Assert.That(node.LightAliveDurationScMax).IsEqualTo(version >= 5 ? 3.5f : 7f);
        await Assert.That(node.U01).IsEqualTo(version >= 4 ? 101 : 1);
        await Assert.That(node.U02).IsEqualTo(version >= 4 ? 102 : 1);
        await Assert.That(node.U03).IsEqualTo((byte)(version >= 4 ? 23 : 4));
        await Assert.That(node.U04).IsEqualTo((byte)(version >= 4 ? 24 : 0));
        await Assert.That(node.U05).IsEqualTo(version >= 4 ? 105 : 1);
        await Assert.That(node.U06).IsEqualTo(version >= 4 ? 106 : 10);
        await Assert.That(node.U07).IsEqualTo((byte)(version >= 6 ? 27 : 0));
        await Assert.That(node.U08).IsEqualTo(version >= 6 ? 108 : 0);
        await Assert.That(node.U09).IsEqualTo(version >= 6 ? 109 : 0);
        if (version >= 7) await Assert.That(node.References.LocAnim.File!.FilePath).IsEqualTo("Loc.Anim.Gbx");
        else await Assert.That(node.References.LocAnim.File).IsNull();
        await Assert.That(node.U10).IsEqualTo(version >= 8 ? 110 : 0);
        await Assert.That(node.LocAnimIsPhysical).IsEqualTo(version >= 10);
        await Assert.That(node.U11).IsEqualTo(version == 12 ? 111 : 0);
        if (version >= 13) await Assert.That(node.References.WaterModel.File!.FilePath).IsEqualTo("Water.Model.Gbx");
        else await Assert.That(node.References.WaterModel.File).IsNull();
        await payload.AssertRoundTrip(rw => node.ReadWrite(rw));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ReadWrite_LegacyArchive_UsesReferencedPhysicsMass(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(-1); // No mesh.
            writer.Write(-1); // No dynamic shape.
            writer.WriteNodeRef(new CPlugDynaModel { LinearMass = 42.5f });
            if (version >= 1) writer.Write(-1); // No static shape.
            if (version >= 2) writer.Write(123.5f);
            writer.Write(0x12345678);
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadReadable<CPlugDynaObjectModel>();
        await Assert.That(node.DynaModel!.LinearMass).IsEqualTo(42.5f);
        await Assert.That(node.Mass).IsEqualTo(42.5f);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        {
            node.Write(writer);
            writer.Write(0x12345678);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_DefaultArchive_UsesLatestVersionAndNativeDefaults()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer))
        {
            rw.ReadableWritable(new CPlugDynaObjectModel());
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadReadable<CPlugDynaObjectModel>();
        await Assert.That(node.Version).IsEqualTo(13);
        await Assert.That(node.BreakSpeedKmh).IsEqualTo(100f);
        await Assert.That(node.Mass).IsEqualTo(10f);
        await Assert.That(node.LightAliveDurationScMin).IsEqualTo(5f);
        await Assert.That(node.LightAliveDurationScMax).IsEqualTo(7f);
        await Assert.That(payload.Length).IsEqualTo(83L);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
    }

    [Test]
    public async Task ReadWrite_VersionZero_ShapesShareTheOriginalCollisionReference()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0);
            writer.Write(-1);
            writer.WriteNodeRef(new CPlugSurface());
            writer.Write(-1);
            writer.Write(0x12345678);
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadReadable<CPlugDynaObjectModel>();
        await Assert.That(node.Shape).IsSameReferenceAs(node.DynaShape);
        await Assert.That(node.StaticShape).IsSameReferenceAs(node.DynaShape);
        await Assert.That(node.References.StaticShape.CachedNode).IsSameReferenceAs(node.DynaShape);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
    }
}

