using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Tests.Unit.Engines.GameData;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVertexStreamTests
{
    public static IEnumerable<(int Type, int Size)> DeclarationTypes()
    {
        var sizes = new[] { 4, 8, 12, 16, 4, 4, 4, 8, 4, 4, 8, 4, 8, 4, 4, 4, 8 };
        for (var i = 0; i < sizes.Length; i++) yield return (i, sizes[i]);
    }

    [Test]
    [MethodDataSource(nameof(DeclarationTypes))]
    public async Task ReadWrite_CurrentChunk_PreservesEveryNativeDeclarationType(int type, int size)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        WriteHeader(writer, 1, 2);
        writer.Write(1);
        writer.Write(Flags(CPlugVertexStream.EPlugVDcl.PointSize, type, size));
        writer.Write(0u);
        writer.Write(false);
        var bytes = Enumerable.Range(0, size * 2).Select(i => (byte)(i * 17 + 3)).ToArray();
        writer.Write(bytes);
        payload.Finish();

        var node = new CPlugVertexStream();
        var chunk = new CPlugVertexStream.Chunk09056000();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.VertexCount).IsEqualTo(2);
        await Assert.That(node.Flags).IsEqualTo(3u);
        await Assert.That(node.DataDecls![0].Type).IsEqualTo((CPlugVertexStream.EPlugVDclType)type);
        await Assert.That(node.DataDecls[0].Stride).IsEqualTo(size);
        await Assert.That(node.DataDecls[0].Data!).IsEquivalentTo(bytes, CollectionOrdering.Matching);
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    public async Task ReadWrite_Float3_CompressionDependsOnSpaceAndStoredFlag(int space, bool compressed)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        WriteHeader(writer, 1, 1);
        writer.Write(2);
        writer.Write(Flags(CPlugVertexStream.EPlugVDcl.Position, 2, 12, space));
        writer.Write(0u);
        writer.Write(Flags(CPlugVertexStream.EPlugVDcl.Normal, 2, 12, space));
        writer.Write(0u);
        writer.Write(compressed);
        var packed = space == 1 && compressed;
        for (var i = 0; i < 2; i++)
        {
            if (packed) writer.Write(0xE007FDFFu); // Includes -512 and unused high bits.
            else writer.Write(new Vec3(1.5f + i, -2.5f, 3.5f));
        }
        payload.Finish();

        var node = new CPlugVertexStream();
        var chunk = new CPlugVertexStream.Chunk09056000();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        var expected = packed ? new Vec3(1, 1, -512 / 511f) : new Vec3(1.5f, -2.5f, 3.5f);
        await Assert.That(node.Positions![0]).IsEqualTo(expected);
        await Assert.That(node.Normals![0]).IsEqualTo(packed ? expected : new Vec3(2.5f, -2.5f, 3.5f));
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task ReadWrite_StridedBuffers_PreserveSharedDeclarationsAndGeometryEdits(int version)
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        WriteHeader(writer, version, 2);
        writer.Write(2);
        writer.Write(Flags(CPlugVertexStream.EPlugVDcl.Position, 2, 20) | (version < 0 ? 0x40000000u : 0));
        if (version >= 0) writer.Write(0u);
        writer.Write(new Vec3(1, 2, 3));
        writer.Write(new Vec2(0.25f, 0.5f));
        writer.Write(new Vec3(4, 5, 6));
        writer.Write(new Vec2(0.75f, 1));
        writer.Write(Flags(CPlugVertexStream.EPlugVDcl.TexCoord0, 1, 20));
        if (version >= 0) writer.Write(12u << 2);
        writer.Write((ushort)0);
        writer.Write((ushort)12);
        payload.Finish();

        var node = new CPlugVertexStream();
        var chunk = new CPlugVertexStream.Chunk09056000 { Version = version };
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Positions!).IsEquivalentTo(new[] { new Vec3(1, 2, 3), new Vec3(4, 5, 6) }, CollectionOrdering.Matching);
        await Assert.That(node.UVs[0]).IsEquivalentTo(new[] { new Vec2(0.25f, 0.5f), new Vec2(0.75f, 1) }, CollectionOrdering.Matching);
        await Assert.That(node.DataDecls![1].SharedDeclIndex).IsEqualTo((ushort)0);
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));

        node.Positions![1] = new Vec3(7, 8, 9);
        node.UVs[0][0] = new Vec2(0.125f, 0.375f);
        using var saved = new MemoryStream();
        using (var savedWriter = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(savedWriter)) chunk.ReadWrite(node, rw);
        saved.Position = 0;
        using var reader = new GbxReader(saved);
        using var readWrite = new GbxReaderWriter(reader);
        var restored = new CPlugVertexStream();
        new CPlugVertexStream.Chunk09056000 { Version = version }.ReadWrite(restored, readWrite);
        await Assert.That(restored.Positions![1]).IsEqualTo(new Vec3(7, 8, 9));
        await Assert.That(restored.UVs[0][0]).IsEqualTo(new Vec2(0.125f, 0.375f));
        await Assert.That(saved.Position).IsEqualTo(saved.Length);
    }

    [Test]
    public async Task ReadWrite_CurrentSharedDeclarations_HaveTheirOwnChannelPayload()
    {
        using var payload = new ObjectModelPayload();
        var writer = payload.Writer;
        WriteHeader(writer, 1, 1);
        writer.Write(2);
        writer.Write(Flags(CPlugVertexStream.EPlugVDcl.Color0, 4, 8));
        writer.Write(0u);
        writer.Write(Flags(CPlugVertexStream.EPlugVDcl.Color2, 4, 8));
        writer.Write(4u << 2);
        writer.Write((ushort)0);
        writer.Write((ushort)4);
        writer.Write(true);
        writer.Write(0x11223344);
        writer.Write(0x55667788);
        payload.Finish();
        var node = new CPlugVertexStream();
        var chunk = new CPlugVertexStream.Chunk09056000();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.Colors[0][0]).IsEqualTo(0x11223344);
        await Assert.That(node.Colors[2][0]).IsEqualTo(0x55667788);
        await Assert.That(node.Colors.Count).IsEqualTo(2);
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_EmptyStream_ConsumesTheCorrectHeader(int version)
    {
        using var payload = new ObjectModelPayload();
        WriteHeader(payload.Writer, version, 0);
        if (version < 0) payload.Writer.Write(0);
        payload.Finish();
        var node = new CPlugVertexStream();
        var chunk = new CPlugVertexStream.Chunk09056000 { Version = version };
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.VertexCount).IsEqualTo(0);
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_StreamModel_PreservesExternalReferenceAndOmitsData(int version)
    {
        using var payload = new ObjectModelPayload();
        payload.Writer.Write(version);
        payload.Writer.Write(27);
        payload.Writer.Write(0x12345678u);
        payload.NodeRef("Shared.VertexStream.Gbx");
        payload.Finish();
        var node = new CPlugVertexStream();
        var chunk = new CPlugVertexStream.Chunk09056000();
        payload.Read(rw => chunk.ReadWrite(node, rw));
        await Assert.That(node.References.StreamModel.File!.FilePath).IsEqualTo("Shared.VertexStream.Gbx");
        await Assert.That(node.DataDecls).IsNull();
        await payload.AssertRoundTrip(rw => chunk.ReadWrite(node, rw));
    }

    [Test]
    [Arguments(GameVersion.TMF)]
    [Arguments(GameVersion.TMSX)]
    [Arguments(GameVersion.TMNESWC)]
    [Arguments(GameVersion.VSK5)]
    public async Task ReadWrite_LegacyGameContext_ReadsAnAmbiguousOneVertexHeader(GameVersion game)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            WriteHeader(writer, -1, 1);
            writer.Write(0);
        }
        stream.Position = 0;
        using var reader = new GbxReader(stream, new GbxReadSettings { GameVersion = game });
        using var rw = new GbxReaderWriter(reader);
        var node = new CPlugVertexStream();
        var chunk = new CPlugVertexStream.Chunk09056000();
        chunk.ReadWrite(node, rw);
        await Assert.That(chunk.Version).IsEqualTo(-1);
        await Assert.That(node.VertexCount).IsEqualTo(1);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
        await Assert.That(new CPlugVertexStream.Chunk09056000(game).Version).IsEqualTo(-1);
    }

    [Test]
    public async Task Constructor_NewNode_UsesNativeFlagsAndWriterVersions()
    {
        var node = new CPlugVertexStream();
        await Assert.That(node.Flags).IsEqualTo(3u);
        await Assert.That(node.IsStatic).IsTrue();
        await Assert.That(node.DirtyVision).IsTrue();
        await Assert.That(node.CompressLocalFloat3).IsTrue();
        await Assert.That(new CPlugVertexStream.Chunk09056000(GameVersion.TMT).Version).IsEqualTo(0);
        await Assert.That(new CPlugVertexStream.Chunk09056000(GameVersion.MP4).Version).IsEqualTo(1);
        await Assert.That(new CPlugVertexStream.Chunk09056000(GameVersion.TM2020).Version).IsEqualTo(1);
    }

    [Test]
    public async Task ReadWrite_CombinedReadWrite_PreservesChannelBytes()
    {
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input))
        {
            WriteHeader(writer, 1, 1);
            writer.Write(1);
            writer.Write(Flags(CPlugVertexStream.EPlugVDcl.Normal, 2, 12, 1));
            writer.Write(0u);
            writer.Write(true);
            writer.Write(0xE007FDFFu);
        }
        input.Position = 0;
        using var output = new MemoryStream();
        using var reader = new GbxReader(input);
        using var outputWriter = new GbxWriter(output);
        using var rw = new GbxReaderWriter(reader, outputWriter);
        var node = new CPlugVertexStream();
        new CPlugVertexStream.Chunk09056000().ReadWrite(node, rw);
        await Assert.That(node.Normals![0]).IsEqualTo(new Vec3(1, 1, -512 / 511f));
        await Assert.That(output.ToArray()).IsEquivalentTo(input.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task DeepClone_Node_CopiesRawChannelBytesAndGeometryViews()
    {
        var node = new CPlugVertexStream
        {
            VertexCount = 1,
            Positions = [new Vec3(1, 2, 3)],
            DataDecls = [new CPlugVertexStream.DataDecl { Flags1 = Flags(CPlugVertexStream.EPlugVDcl.Position, 2, 12), Data = new byte[12] }]
        };
        node.UVs[0] = [new Vec2(0.25f, 0.5f)];
        node.Colors = new SortedDictionary<int, int[]>(Comparer<int>.Create((x, y) => y.CompareTo(x)))
        {
            [0] = [0x11223344],
            [2] = [0x55667788]
        };
        node.CreateChunk<CPlugVertexStream.Chunk09056000>().Version = 1;
#pragma warning disable GBXNET10001
        var clone = (CPlugVertexStream)node.DeepClone();
#pragma warning restore GBXNET10001
        clone.Positions![0] = new Vec3(4, 5, 6);
        clone.DataDecls![0].Data![0] = 7;
        clone.UVs[0][0] = new Vec2(0.75f, 1);
        clone.Colors[0][0] = 0;
        await Assert.That(node.Positions[0]).IsEqualTo(new Vec3(1, 2, 3));
        await Assert.That(node.DataDecls[0].Data![0]).IsEqualTo((byte)0);
        await Assert.That(node.UVs[0][0]).IsEqualTo(new Vec2(0.25f, 0.5f));
        await Assert.That(node.Colors[0][0]).IsEqualTo(0x11223344);
        await Assert.That(clone.Colors.Keys).IsEquivalentTo(new[] { 2, 0 }, CollectionOrdering.Matching);
        await Assert.That(clone.Chunks.Get<CPlugVertexStream.Chunk09056000>()!.Version).IsEqualTo(1);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task ReadWrite_SharedDeclaration_RejectsReferencesToItself(int version)
    {
        using var payload = new ObjectModelPayload();
        WriteHeader(payload.Writer, version, 1);
        payload.Writer.Write(1);
        payload.Writer.Write(Flags(CPlugVertexStream.EPlugVDcl.TexCoord0, 1, 12));
        payload.Writer.Write(4u << 2);
        payload.Writer.Write((ushort)0);
        payload.Writer.Write((ushort)4);
        payload.Finish();
        await Assert.That(() => payload.Read(rw => new CPlugVertexStream.Chunk09056000().ReadWrite(new CPlugVertexStream(), rw)))
            .Throws<InvalidDataException>();
    }

    private static void WriteHeader(GbxWriter writer, int version, int count)
    {
        if (version >= 0) writer.Write(version);
        writer.Write(count);
        writer.Write(3u);
        writer.Write(-1);
    }

    private static uint Flags(CPlugVertexStream.EPlugVDcl semantic, int type, int stride, int space = 0)
        => (uint)semantic | ((uint)type << 9) | ((uint)stride << 18) | ((uint)space << 28);
}
