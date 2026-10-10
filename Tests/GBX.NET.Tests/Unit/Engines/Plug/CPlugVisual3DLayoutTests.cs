using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVisual3DLayoutTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacyChunks_PreservePositionNormalColorAndTangents(bool bitangents)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0x09006008u);
            writer.Write(false); // Geometry static.
            writer.Write(false); // Indexation static.
            writer.Write(0); // Texture coordinate sets.
            writer.Write(0); // Skin indices per vertex.
            writer.Write(1); // Vertices.
            writer.Write(true); // Vertex colors.
            writer.Write(0); // Legacy bone transforms.
            writer.Write(bitangents ? 0x0902C003u : 0x0902C001u);
            writer.Write(new Vec3(1, 2, 3));
            writer.Write(new Vec3(-1, 0, 0));
            writer.Write(new Vec4(0.125f, 0.25f, 0.5f, 1));
            writer.Write(1);
            writer.Write(new Vec3(0, 1, 0));
            if (bitangents)
            {
                writer.Write(1);
                writer.Write(new Vec3(0, 0, 1));
            }
            writer.Write(0x0902C002u);
            writer.Write(-1); // BlendShapes.
            writer.Write(0xFACADE01u);
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugVisual3D>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Vertices[0].Color).IsEqualTo(new Vec4(0.125f, 0.25f, 0.5f, 1));
        await Assert.That(node.Tangents![0]).IsEqualTo(new Vec3(0, 1, 0));
        if (bitangents) await Assert.That(node.BiTangents![0]).IsEqualTo(new Vec3(0, 0, 1));
        await Assert.That(node.BlendShapes).IsNull();
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task PackedVertices_PreserveNativeNormalAndBgraColor(bool sprite, bool compressed)
    {
        const int packedNormal = unchecked((int)0xC00FFFFF); // X=-1, Y=-1, Z=0, unused high bits set.
        const int packedColor = unchecked((int)0x80402010);
        CPlugVisual3D node = sprite ? new CPlugVisualSprite() : new CPlugVisual3D();
        node.VertexCount = 1;
        node.Flags = (1 << 22) | (1 << 7) | (1 << 8) | (compressed ? (1 << 20) | (1 << 21) : 0);
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(new Vec3(1, 2, 3));
            if (compressed && !sprite) writer.Write(packedNormal);
            else writer.Write(new Vec3(2, 0.5f, 1.5f));
            if (compressed) writer.Write(packedColor);
            else writer.Write(new Vec4(0.25f, 0.5f, 0.75f, 1));
            writer.Write(1); // Tangent count.
            if (compressed) writer.Write(0x12345678);
            else writer.Write(new Vec3(1, 0, 0));
            writer.Write(0); // Bitangent count.
        }
        payload.Position = 0;
        var chunk = new CPlugVisual3D.Chunk0902C004();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(chunk.TangentData!.Length).IsEqualTo(compressed ? 4 : 12);
        await Assert.That(node.Vertices[0].Normal).IsEqualTo(compressed && !sprite
            ? new Vec3(-1f / 511, -1f / 511, 0) : new Vec3(2, 0.5f, 1.5f));
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task OptionalVertexElements_RespectPresenceFlagsAndNativeDefaults(bool normals, bool colors)
    {
        var node = new CPlugVisual3D { VertexCount = 1, Flags = (1 << 22) | (normals ? 1 << 7 : 0) | (colors ? 1 << 8 : 0) };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(new Vec3(1, 2, 3));
            if (normals) writer.Write(new Vec3(0, 0, 1));
            if (colors) writer.Write(new Vec4(0, 0.5f, 1, 0.25f));
            writer.Write(0);
            writer.Write(0);
        }
        payload.Position = 0;
        var chunk = new CPlugVisual3D.Chunk0902C004();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Vertices[0].Normal).IsEqualTo(normals ? new Vec3(0, 0, 1) : new Vec3(0, 1, 0));
        await Assert.That(node.Vertices[0].Color).IsEqualTo(colors ? new Vec4(0, 0.5f, 1, 0.25f) : new Vec4(1, 1, 1, 1));
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task CompressedColors_UseNativeRounding()
    {
        var vertex = new CPlugVisual3D.Vertex(new Vec3(1, 2, 3), Color: new Vec4(0.5f, 0.25f, 0.75f, 0.1f));
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream)) vertex.Write(writer, false, true, false, true, false);
        stream.Position = 12;
        using var reader = new GbxReader(stream);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0x1A8040BFu);
    }

    [Test]
    public async Task CompressedNormals_UseSignedTenBitComponentsAndTruncation()
    {
        var vertex = new CPlugVisual3D.Vertex(new Vec3(1, 2, 3), new Vec3(-0.5f, 1, 0));
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream)) vertex.Write(writer, true, false, true, false, false);
        stream.Position = 12;
        using var reader = new GbxReader(stream);
        await Assert.That(reader.ReadInt32()).IsEqualTo((-255 & 1023) | (511 << 10));
    }

    [Test]
    public async Task LegacyBlendShapeReference_PreservesClearingAfterRead()
    {
        var node = new CPlugVisual3D { BlendShapes = new CPlugVisual() };
        var chunk = new CPlugVisual3D.Chunk0902C002();
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        stream.Position = 0;
        using (var reader = new GbxReader(stream))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(node.BlendShapes).IsNotNull();
#pragma warning disable CS0618
        chunk.U01 = null;
#pragma warning restore CS0618
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(node.BlendShapes).IsNull();
        await Assert.That(saved.ToArray()).IsEquivalentTo(BitConverter.GetBytes(-1), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task VertexStreams_OmitClassicVerticesButKeepTangentData(bool compressed)
    {
        var node = new CPlugVisual3D { VertexCount = 1, Flags = compressed ? 1 << 20 : 0,
            VertexStreams = [new CPlugVertexStream()] };
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(1);
            if (compressed) writer.Write(123);
            else writer.Write(new Vec3(1, 0, 0));
            writer.Write(0);
        }
        stream.Position = 0;
        var chunk = new CPlugVisual3D.Chunk0902C004();
        using (var reader = new GbxReader(stream))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
        await Assert.That(node.Vertices.Length).IsEqualTo(0);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(stream.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(2)]
    public void InvalidTangentCounts_AreRejected(int count)
    {
        var node = new CPlugVisual3D { VertexCount = 1, VertexStreams = [new CPlugVertexStream()] };
        using var stream = new MemoryStream(BitConverter.GetBytes(count));
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        Assert.Throws<InvalidDataException>(() => new CPlugVisual3D.Chunk0902C004().ReadWrite(node, rw));
    }

    [Test]
    public void InvalidTangentLength_IsRejectedOnWrite()
    {
        var node = new CPlugVisual3D { VertexCount = 1, VertexStreams = [new CPlugVertexStream()] };
        var chunk = new CPlugVisual3D.Chunk0902C004 { TangentCount = 1, TangentData = new byte[4] };
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        using var rw = new GbxReaderWriter(writer);
        Assert.Throws<InvalidDataException>(() => chunk.ReadWrite(node, rw));
    }

    [Test]
    public void TruncatedTangentData_IsRejected()
    {
        var node = new CPlugVisual3D { VertexCount = 1, Flags = 1 << 20, VertexStreams = [new CPlugVertexStream()] };
        using var stream = new MemoryStream([1, 0, 0, 0, 12, 34]);
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);
        Assert.Throws<EndOfStreamException>(() => new CPlugVisual3D.Chunk0902C004().ReadWrite(node, rw));
    }

    [Test]
    public void InvalidVertexCount_IsRejectedOnWrite()
    {
        var node = new CPlugVisual3D { VertexCount = 1 };
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        using var rw = new GbxReaderWriter(writer);
        Assert.Throws<InvalidDataException>(() => new CPlugVisual3D.Chunk0902C001().ReadWrite(node, rw));
    }

    [Test]
    public async Task LegacyColorAliases_MapToRgba()
    {
#pragma warning disable CS0618
        var vertex = new CPlugVisual3D.Vertex { U02 = new Vec3(0.25f, 0.5f, 0.75f), U03 = 1 };
        await Assert.That(vertex.Color).IsEqualTo(new Vec4(0.25f, 0.5f, 0.75f, 1));
        await Assert.That((vertex with { Color = new Vec4(1, 0, 0, 0.5f) }).U02).IsEqualTo(new Vec3(1, 0, 0));
#pragma warning restore CS0618
    }
}
