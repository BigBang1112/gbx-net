using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Tests.Unit.Engines.GameData;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugDynaWaterModelTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ReadWrite_Chunk_PreservesNativeOrderAndVersionBranches(int version)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(version);
        WriteBody(writer, version);
        if (version >= 2) writer.Write(-1);
        payload.Finish();

        var node = new CPlugDynaWaterModel();
        var chunk = new CPlugDynaWaterModel.Chunk0915F000();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        var model = node;
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(model.Gravity).IsEqualTo(1.5f);
        await Assert.That(model.U02).IsEqualTo(2.5f);
        await Assert.That(model.U03).IsEqualTo(3.5f);
        await Assert.That(model.U07).IsEqualTo(7.5f);
        await Assert.That(model.U09).IsEqualTo(9.5f);
        await Assert.That(model.Surface).IsNull();
        var curves = new[] { model.U04!, model.U05!, model.U06!, model.U08! };
        for (var i = 0; i < curves.Length; i++)
        {
            await Assert.That(curves[i].Count).IsEqualTo(version == 0 ? 0 : 2);
            if (version > 0)
            {
                await Assert.That(curves[i].Flags).IsEqualTo((ushort)(0x8000 + i));
                await Assert.That(curves[i].Values!).IsEquivalentTo(new[] { new Vec2(i, i + 10), new Vec2(i + 1, i + 20) }, CollectionOrdering.Matching);
            }
            else
            {
                await Assert.That(curves[i].Values!).IsEmpty();
            }
        }
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ReadWrite_InlineCurve_PreservesEveryFlagWidthAndPointCount(int keyVersion)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(keyVersion);
        writer.Write(2);
        if (keyVersion == 1) writer.Write((byte)0xA5);
        if (keyVersion == 2) writer.Write(0x12345678);
        if (keyVersion >= 3) writer.Write((ushort)0xFEDC);
        writer.Write(new Vec2(1.5f, 2.5f));
        writer.Write(new Vec2(3.5f, 4.5f));
        payload.Finish();

        var keys = new PlugCurve();
        payload.Read(rw => keys.ReadWrite(rw, version: 1));
        await Assert.That(keys.Version).IsEqualTo(keyVersion);
        await Assert.That(keys.Count).IsEqualTo(2);
        await Assert.That(keys.Flags).IsEqualTo((ushort)(keyVersion >= 3 ? 0xFEDC : keyVersion == 1 ? 1 : 0));
        if (keyVersion == 1) await Assert.That(keys.FlagsV1).IsEqualTo((byte)0xA5);
        if (keyVersion == 2) await Assert.That(keys.Capacity).IsEqualTo(0x12345678);
        await Assert.That(keys.Values!).IsEquivalentTo(new[] { new Vec2(1.5f, 2.5f), new Vec2(3.5f, 4.5f) }, CollectionOrdering.Matching);
        await payload.AssertRoundTrip(rw => keys.ReadWrite(rw, version: 1));
    }

    [Test]
    public async Task ReadWrite_Version2_ReadsInlineSurfaceWithGameplayDirection()
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(2);
        WriteBody(writer, 2);
        writer.Write(0); // Sphere
        writer.Write(12.5f);
        writer.Write((short)23);
        writer.Write(new Vec3(1, 2, 3));
        payload.Finish();

        var node = new CPlugDynaWaterModel();
        var chunk = new CPlugDynaWaterModel.Chunk0915F000();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        var sphere = (CPlugSurface.Sphere)node.Surface!.Surf!;
        await Assert.That(node.Surface.SurfVersion).IsEqualTo(2);
        await Assert.That(sphere.Size).IsEqualTo(12.5f);
        await Assert.That(sphere.SurfaceIndex).IsEqualTo((short)23);
        await Assert.That(sphere.GameplayMainDir).IsEqualTo(new Vec3(1, 2, 3));
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(3)]
    [Arguments(4)]
    public async Task ReadWrite_CurrentSurface_PreservesExternalReference(int version)
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(version);
        WriteBody(payload.Writer, version);
        payload.NodeRef("Water.Shape.Gbx");
        payload.Finish();
        var node = new CPlugDynaWaterModel();
        var chunk = new CPlugDynaWaterModel.Chunk0915F000();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.References.Surface.File!.FilePath).IsEqualTo("Water.Shape.Gbx");
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ReadWrite_InlineArchive_HasNoAdditionalVersionPrefix(int version)
    {
        using var payload = new ObjectModelPayload();
        WriteBody(payload.Writer, version);
        if (version >= 2) payload.Writer.Write(-1);
        payload.Finish();
        var model = new CPlugDynaWaterModel();
        payload.Read(rw => model.ReadWrite(rw, version));
        await Assert.That(model.Gravity).IsEqualTo(1.5f);
        await Assert.That(model.U09).IsEqualTo(9.5f);
        await payload.AssertRoundTrip(rw => model.ReadWrite(rw, version));
    }

    [Test]
    public async Task ReadWrite_LegacyCurve_ConvertsReferencedPointsAndPreservesNode()
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        writer.Write(1); // Node index
        writer.Write(CPlugCurveSimpleNod.Id);
        writer.Write(0x09185000u);
        writer.Write(0); // Chunk version
        writer.Write(3); // Curve version
        writer.Write(2);
        writer.Write((ushort)0xFEDC);
        writer.Write(new Vec2(1.5f, 2.5f));
        writer.Write(new Vec2(3.5f, 4.5f));
        writer.Write(0xFACADE01u);
        payload.Finish();

        var keys = new PlugCurve();
        payload.Read(rw => keys.ReadWrite(rw, version: -1));
        await Assert.That(keys.Curve).IsNotNull();
        await Assert.That(keys.Flags).IsEqualTo((ushort)0xFEDC);
        await Assert.That(keys.Count).IsEqualTo(2);
        await Assert.That(keys.Values!).IsEquivalentTo(new[] { new Vec2(1.5f, 2.5f), new Vec2(3.5f, 4.5f) }, CollectionOrdering.Matching);
        await payload.AssertRoundTrip(rw => keys.ReadWrite(rw, version: -1));
    }

    [Test]
    public async Task Constructor_NewNode_UsesNativeWaterDefaults()
    {
        var model = new CPlugDynaWaterModel();
        await Assert.That(model.Gravity).IsEqualTo(0f);
        await Assert.That(model.U02).IsEqualTo(10f);
        await Assert.That(model.U03).IsEqualTo(1f);
        await Assert.That(model.U07).IsEqualTo(1f);
        await Assert.That(model.U09).IsEqualTo(1f);
        await Assert.That(model.U04!.Values!).IsEquivalentTo(new[] { new Vec2(0, 1), new Vec2(1, 500) }, CollectionOrdering.Matching);
        await Assert.That(model.U05!.Values!).IsEquivalentTo(new[] { new Vec2(0, 0.85f) }, CollectionOrdering.Matching);
        await Assert.That(model.U06!.Values!).IsEquivalentTo(new[] { new Vec2(0, 0), new Vec2(1, 6000) }, CollectionOrdering.Matching);
        await Assert.That(model.U08!.Values!).IsEquivalentTo(new[] { new Vec2(0, 0), new Vec2(1, 1200) }, CollectionOrdering.Matching);
        await Assert.That(model.U04.Version).IsEqualTo(3);
        await Assert.That(model.U04.Capacity).IsEqualTo(7);
        await Assert.That(new CPlugDynaWaterModel.Chunk0915F000(GameVersion.TM2020).Version).IsEqualTo(3);
    }

    [Test]
    public async Task ReadWrite_InlineSurface_CanBeWrittenAsCurrentNodeReference()
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(2);
        WriteBody(payload.Writer, 2);
        payload.Writer.Write(0); // Sphere
        payload.Writer.Write(12.5f);
        payload.Writer.Write((short)23);
        payload.Writer.Write(new Vec3(1, 2, 3));
        payload.Finish();
        var model = new CPlugDynaWaterModel();
        var chunk = new CPlugDynaWaterModel.Chunk0915F000();
        payload.Read(rw => chunk.ReadWrite(model, rw));
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) model.Write(writer, v: 3);
        saved.Position = 0;
        using var reader = new GbxReader(saved);
        var restored = reader.ReadReadable<CPlugDynaWaterModel>(version: 3);
        var sphere = (CPlugSurface.Sphere)restored.Surface!.Surf!;
        await Assert.That(sphere.Size).IsEqualTo(12.5f);
        await Assert.That(sphere.SurfaceIndex).IsEqualTo((short)23);
        await Assert.That(sphere.GameplayMainDir).IsEqualTo(new Vec3(1, 2, 3));
        await Assert.That(saved.Position).IsEqualTo(saved.Length);
    }

    [Test]
    public async Task DeepClone_Node_CopiesCurveArraysAndChunks()
    {
        var node = new CPlugDynaWaterModel();
        var chunk = node.CreateChunk<CPlugDynaWaterModel.Chunk0915F000>();
        chunk.Version = 3;
#pragma warning disable GBXNET10001
        var clone = (CPlugDynaWaterModel)node.DeepClone();
#pragma warning restore GBXNET10001
        await Assert.That(clone).IsNotSameReferenceAs(node);
        await Assert.That(clone.U04).IsNotSameReferenceAs(node.U04);
        clone.U04!.Values![0] = new Vec2(2, 3);
        await Assert.That(node.U04!.Values![0]).IsEqualTo(new Vec2(0, 1));
        var clonedChunk = clone.Chunks.Get<CPlugDynaWaterModel.Chunk0915F000>()!;
        await Assert.That(clonedChunk).IsNotSameReferenceAs(chunk);
        await Assert.That(clonedChunk.Version).IsEqualTo(3);
        clonedChunk.Version = 4;
        await Assert.That(chunk.Version).IsEqualTo(3);
        clone.Gravity = 4.5f;
        await Assert.That(clone.Gravity).IsEqualTo(4.5f);
        await Assert.That(node.Gravity).IsEqualTo(0f);
    }

    [Test]
    public async Task ReadWrite_NodeReference_PreservesChunkHeaderAndInlineParameters()
    {
        var node = new CPlugDynaWaterModel { Gravity = 7.5f };
        node.CreateChunk<CPlugDynaWaterModel.Chunk0915F000>().Version = 3;
        using var payload = new ObjectModelPayload();
        payload.Writer.WriteNodeRef(node);
        payload.Finish();
        CPlugDynaWaterModel? restored = null;
        payload.Read(rw => restored = rw.Reader!.ReadNodeRef<CPlugDynaWaterModel>());
        await Assert.That(restored!.Chunks.Get<CPlugDynaWaterModel.Chunk0915F000>()!.Version).IsEqualTo(3);
        await Assert.That(restored.Gravity).IsEqualTo(7.5f);
        await Assert.That(restored.U02).IsEqualTo(10f);
        await Assert.That(restored.U04!.Values!).IsEquivalentTo(node.U04!.Values!, CollectionOrdering.Matching);
        await payload.AssertRoundTrip(rw => rw.Writer!.WriteNodeRef(restored));
    }

    private static void WriteBody(GbxWriter writer, int version)
    {
        writer.Write(1.5f);
        writer.Write(2.5f);
        writer.Write(3.5f);
        WriteCurve(writer, version, 0);
        WriteCurve(writer, version, 1);
        WriteCurve(writer, version, 2);
        writer.Write(7.5f);
        WriteCurve(writer, version, 3);
        writer.Write(9.5f);
    }

    private static void WriteCurve(GbxWriter writer, int version, int index)
    {
        if (version == 0)
        {
            writer.Write(-1);
            return;
        }
        writer.Write(3);
        writer.Write(2);
        writer.Write((ushort)(0x8000 + index));
        writer.Write(new Vec2(index, index + 10));
        writer.Write(new Vec2(index + 1, index + 20));
    }
}

