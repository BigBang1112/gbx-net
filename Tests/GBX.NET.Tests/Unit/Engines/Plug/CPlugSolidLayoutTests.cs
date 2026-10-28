using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;
using GBX.NET.Tests.Unit.Engines.GameData;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugSolidLayoutTests
{
    [Test]
    [Arguments(13, false, false, false)]
    [Arguments(13, true, false, false)]
    [Arguments(13, false, true, false)]
    [Arguments(13, true, true, false)]
    [Arguments(17, false, false, false)]
    [Arguments(17, true, false, false)]
    [Arguments(17, false, true, false)]
    [Arguments(17, true, true, false)]
    [Arguments(17, false, true, true)]
    [Arguments(17, true, true, true)]
    public async Task ReadWrite_ModelAndTree_ReadsConditionalModelReferences(int offset, bool useModel, bool hasModel, bool external)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(useModel);
        writer.Write(hasModel);
        if (hasModel)
        {
            if (offset == 17) writer.Write(external);
            if (external) payload.NodeRef("Model.Solid.Gbx");
            else writer.WriteNodeRef(new CPlugSolid());
        }
        else
        {
            writer.WriteNodeRef(new CPlugTree());
        }
        payload.Finish();

        var node = new CPlugSolid();
        Chunk<CPlugSolid> chunk = offset == 13 ? new CPlugSolid.Chunk0900500D() : new CPlugSolid.Chunk09005011();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.UseModel).IsEqualTo(useModel);
        if (offset == 17 && hasModel) await Assert.That(node.ModelIsExternal).IsEqualTo((bool?)external);
        if (hasModel)
        {
            await Assert.That(node.Tree).IsNull();
            if (external) await Assert.That(node.References.Model.File!.FilePath).IsEqualTo("Model.Solid.Gbx");
            else await Assert.That(node.Model).IsNotNull();
        }
        else
        {
            await Assert.That(node.Tree).IsNotNull();
            await Assert.That(node.Model).IsNull();
        }
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
        if (external) await Assert.That(node.References.Model.State).IsEqualTo(NodeRefState.Unresolved);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    public async Task ReadWrite_AudioInstances_PreservesModelsTransformsAndVersionedTail(int version)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(version);
        writer.Write(10); // Deprecated sound array version.
        writer.Write(1);
        payload.NodeRef("Sound.Sound.Gbx");
        writer.Write(10); // Deprecated emitter array version.
        writer.Write(1);
        payload.NodeRef("Emitter.Gbx");
        for (var i = 0; i < 2; i++)
        {
            writer.Write(1);
            writer.Write(0);
            writer.Write(Iso4.Identity with { TX = i + 1 });
        }
        if (version >= 1) writer.Write(23);
        if (version >= 2)
        {
            writer.Write(1);
            writer.WriteIdAsString("Animation");
            writer.Write(1);
            writer.Write(Iso4.Identity with { TY = 7 });
        }
        if (version >= 3) writer.Write("Development name");
        if (version >= 4) writer.Write(31);
        if (version >= 5) payload.NodeRef("Path.Gbx");
        payload.Finish();

        var node = new CPlugSolid();
        var chunk = new CPlugSolid.Chunk09005019();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(node.Sounds!.References[0].File!.FilePath).IsEqualTo("Sound.Sound.Gbx");
        await Assert.That(node.ParticleEmitterModels!.References[0].File!.FilePath).IsEqualTo("Emitter.Gbx");
        await Assert.That(node.SoundInstances![0].Loc.TX).IsEqualTo(1f);
        await Assert.That(node.ParticleInstances![0].Loc.TX).IsEqualTo(2f);
        if (version >= 5) await Assert.That(node.References.Path.File!.FilePath).IsEqualTo("Path.Gbx");
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 0)]
    [Arguments(2, 0)]
    [Arguments(3, 0)]
    [Arguments(4, 0)]
    [Arguments(4, 1)]
    [Arguments(4, 2)]
    public async Task ReadWrite_LodNormalMaps_PreservesTgaAndInlineImageArchives(int version, int archiveVersion)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(version);
        if (version <= 2)
        {
            var bytes = version < 2 ? new byte[] { 1, 2, 3, 4, 5 } : [];
            writer.Write(bytes.Length);
            writer.Write(bytes);
            if (version == 1)
            {
                writer.Write(true);
                WriteImage(writer, new Int3(2, 2, 1), 1u | (3u << 2) | (1u << 15), 12);
            }
        }
        if (version >= 4)
        {
            writer.Write(archiveVersion);
            writer.WriteNodeRef(new CPlugVisualIndexedTriangles());
            writer.Write(false); // Normal image is present.
            WriteImage(writer, new Int3(5, 3, 1), 1u | (1u << 5), 32); // DXT blocks, full mip chain.
            if (archiveVersion >= 1)
            {
                writer.Write(false); // Diffuse image is present.
                WriteImage(writer, new Int3(2, 2, 1), 1u | (4u << 2) | (1u << 15), 16);
            }
        }
        payload.Finish();

        var node = new CPlugSolid();
        var chunk = new CPlugSolid.Chunk0900501A();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(version);
        if (version == 1) await Assert.That(node.VeryLQDiffuse!.Pixels!.Length).IsEqualTo(12);
        if (version >= 4)
        {
            await Assert.That(node.SolidLodNormalMap!.Version).IsEqualTo(archiveVersion);
            await Assert.That(node.SolidLodNormalMap.Normal!.Data!.Pixels!.Length).IsEqualTo(32);
        }
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(4, 2, 2, 2u | (4u << 2) | (2u << 15), 72)]
    [Arguments(2, 2, 1, 3u | (4u << 2) | (1u << 15), 96)]
    [Arguments(2, 2, 3, 1u | (4u << 2) | (1u << 15) | (1u << 27), 48)]
    [Arguments(3, 2, 1, 1u | (15u << 5) | (1u << 15), 24)]
    public async Task ReadWrite_ImageData_ConsumesMipLevelsCubeFacesAndArrayLayers(int width, int height, int depth, uint flags, int byteCount)
    {
        using var payload = new ObjectModelPayload();
        WriteImage(payload.Writer, new Int3(width, height, depth), flags, byteCount);
        payload.Finish();
        var image = new CPlugSolid.FileImageData();
        payload.Read(rw => image.ReadWrite(rw));
        await Assert.That(image.Pixels!.Length).IsEqualTo(byteCount);
        await payload.AssertRoundTrip(rw => image.ReadWrite(rw));
    }

    [Test]
    public async Task ReadWrite_PhysicalFlags_ConsumesTheLegacyFloatBeforeTheFinalInteger()
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        for (var i = 0; i < 6; i++) writer.Write(i % 2 == 0);
        writer.Write(3.25f);
        writer.Write(37);
        payload.Finish();
        var node = new CPlugSolid();
        var chunk = new CPlugSolid.Chunk0900500B();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.U07).IsEqualTo(3.25f);
        await Assert.That(chunk.U08).IsEqualTo(37);
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    private static void WriteImage(GbxWriter writer, Int3 size, uint flags, int byteCount)
    {
        writer.Write(size);
        writer.Write(flags);
        for (var i = 0; i < byteCount; i++) writer.Write((byte)i);
    }

    [Test]
    [Arguments(6)]
    [Arguments(14)]
    public async Task ReadWrite_PhysicalParameters_ReadsMassCenterOfMassAndInverseInertia(int offset)
    {
        using var payload = new ObjectModelPayload();
        var count = offset == 6 ? 15 : 16;
        for (var i = 1; i <= count; i++) payload.Writer.Write((float)i);
        payload.Finish();
        var node = new CPlugSolid();
        Chunk<CPlugSolid> chunk = offset == 6 ? new CPlugSolid.Chunk09005006() : new CPlugSolid.Chunk0900500E();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        if (offset == 6)
        {
            await Assert.That(node.Mass).IsEqualTo(3f);
            await Assert.That(node.CenterOfMass).IsEqualTo(new Vec3(4, 5, 6));
            await Assert.That(node.InverseInertiaMatrix).IsEqualTo(new Mat3(7, 8, 9, 10, 11, 12, 13, 14, 15));
        }
        else
        {
            await Assert.That(node.Mass).IsEqualTo(1f);
            await Assert.That(node.CenterOfMass).IsEqualTo(new Vec3(2, 3, 4));
            await Assert.That(node.InverseInertiaMatrix).IsEqualTo(new Mat3(5, 6, 7, 8, 9, 10, 11, 12, 13));
            await Assert.That(node.LinearFluidFriction).IsEqualTo(14f);
            await Assert.That(node.AngularFluidFriction).IsEqualTo(15f);
            await Assert.That(node.MaxDistPerStep).IsEqualTo(16f);
        }
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task ReadWrite_TileCountU_PreservesLegacyByteWidth()
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write((byte)4);
        payload.Finish();
        var node = new CPlugSolid();
        var chunk = new CPlugSolid.Chunk09005012();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.TileCountU).IsEqualTo((byte)4);
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    public async Task ReadWrite_NullLodImages_ConsumesOnlyTheNullFlags()
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(4);
        writer.Write(2);
        writer.Write(-1);
        writer.Write(true);
        writer.Write(true);
        payload.Finish();
        var node = new CPlugSolid();
        var chunk = new CPlugSolid.Chunk0900501A();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.SolidLodNormalMap!.Normal!.Data).IsNull();
        await Assert.That(node.SolidLodNormalMap.Diffuse!.Data).IsNull();
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }
}
