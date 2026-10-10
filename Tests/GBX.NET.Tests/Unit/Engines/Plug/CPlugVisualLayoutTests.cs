using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVisualLayoutTests
{
    private static readonly BoxAligned Bounds = new(1, 2, 3, 4, 5, 6);

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task NamedFields_AfterRead_PreserveEditsThroughBothApis(bool legacy)
    {
        using var payload = new MemoryStream();
        using (var w = new GbxWriter(payload))
        {
            w.Write(0x09006007u);
            w.Write(false);
            w.Write(0x09006009u);
            w.Write(1f);
            w.Write(0xFACADE01u);
        }
        var node = await RoundTrip(payload);
        if (legacy)
        {
#pragma warning disable CS0618
            node.GetChunk<CPlugVisual.Chunk09006007>()!.U01 = true;
            node.GetChunk<CPlugVisual.Chunk09006009>()!.U01 = 2.5f;
#pragma warning restore CS0618
        }
        else
        {
            node.IsInverse = true;
            node.NPatchTessLevel = 2.5f;
        }
        using var saved = new MemoryStream();
        using (var w = new GbxWriter(saved)) w.WriteNode(node);
        saved.Position = 0;
        using var reader = new GbxReader(saved);
        var restored = reader.ReadNode<CPlugVisual>()!;
        await Assert.That(restored.IsInverse).IsTrue();
        await Assert.That(restored.NPatchTessLevel).IsEqualTo(2.5f);
    }

    [Test]
    public async Task NamedChunks_PreserveNativeFields()
    {
        using var payload = new MemoryStream();
        using (var w = new GbxWriter(payload))
        {
            w.Write(0x09006001u);
            w.WriteIdAsString("visual");
            w.Write(0x09006004u);
            w.Write(-1);
            w.Write(0x09006005u);
            w.Write(1);
            w.Write(new Int3(3, 6, 9));
            w.Write(0x09006006u);
            w.Write(false);
            w.Write(0x09006007u);
            w.Write(true);
            w.Write(0x09006009u);
            w.Write(2.5f);
            w.Write(0x0900600Bu);
            w.Write(1);
            w.Write(12);
            w.Write(34);
            w.Write(Bounds);
            w.Write(0xFACADE01u);
        }
        var node = await RoundTrip(payload);
        await Assert.That(node.VisualId).IsEqualTo("visual");
        await Assert.That(node.FuncVisual).IsNull();
        await Assert.That(node.SubVisuals[0]).IsEqualTo(new Int3(3, 6, 9));
        await Assert.That(node.HasVertexNormals).IsFalse();
        await Assert.That(node.IsInverse).IsTrue();
        await Assert.That(node.NPatchTessLevel).IsEqualTo(2.5f);
        await Assert.That(node.Splits[0].IndexOffset).IsEqualTo(12);
        await Assert.That(node.Splits[0].VertexOffset).IsEqualTo(34);
        await Assert.That(node.Splits[0].BoundingBox).IsEqualTo(Bounds);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 1)]
    [Arguments(2, 2)]
    [Arguments(3, 0)]
    [Arguments(3, 1)]
    [Arguments(3, 2)]
    public async Task TexCoords_UseInterleavedNativeDimensions(int version, int kind)
    {
        using var payload = new MemoryStream();
        using (var w = new GbxWriter(payload))
        {
            w.Write(version);
            if (version >= 3)
            {
                w.Write(2);
                w.Write(256 | kind);
            }
            for (var i = 0; i < 2; i++)
            {
                w.Write(1f + i * 4);
                w.Write(2f + i * 4);
                if (kind >= 1) w.Write(3f + i * 4);
                if (kind >= 2) w.Write(4f + i * 4);
            }
            w.Write(0xFACADE01u);
        }
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var set = CPlugVisual.TexCoordSet.Read(reader, 2);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(0xFACADE01u);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(set.TexCoords[1].UV).IsEqualTo(new Vec2(5, 6));
        await Assert.That(set.TexCoords[1].Z).IsEqualTo(kind >= 1 ? 7f : default(float?));
        await Assert.That(set.TexCoords[1].W).IsEqualTo(kind >= 2 ? 8f : default(float?));
        using var saved = new MemoryStream();
        using (var w = new GbxWriter(saved))
        {
            set.Write(w);
            w.Write(0xFACADE01u);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0x09006003u, 0, 4, true, true)]
    [Arguments(0x09006008u, 0, 1, true, true)]
    [Arguments(0x0900600Au, 0, 2, true, true)]
    [Arguments(0x0900600Cu, 0, 3, true, true)]
    [Arguments(0x0900600Du, 0, 4, true, true)]
    [Arguments(0x0900600Eu, 0, 1, true, true)]
    [Arguments(0x0900600Fu, 0, 2, true, true)]
    [Arguments(0x0900600Fu, 1, 3, true, true)]
    [Arguments(0x0900600Fu, 2, 4, true, true)]
    [Arguments(0x0900600Fu, 3, 1, true, true)]
    [Arguments(0x0900600Fu, 3, 1, false, false)]
    [Arguments(0x0900600Fu, 4, 2, true, true)]
    [Arguments(0x0900600Fu, 5, 3, false, true)]
    [Arguments(0x0900600Fu, 6, 4, true, false)]
    public async Task Geometry_PreservesVersionedSkinAndTrailingData(uint chunkId, int version, int skinCount, bool weights, bool names)
    {
        var stride = skinCount == 1 ? 1 : skinCount * 4;
        var vertexWeights = Enumerable.Range(1, stride * 2).Select(x => (byte)x).ToArray();
        using var payload = new MemoryStream();
        using (var w = new GbxWriter(payload))
        {
            w.Write(chunkId);
            if (chunkId == 0x0900600F) w.Write(version);
            if (chunkId >= 0x0900600D) w.Write(0x3F8 | skinCount);
            else
            {
                w.Write(true);
                w.Write(true);
            }
            w.Write(1); // Texture-coordinate set count.
            if (chunkId < 0x0900600D) w.Write(skinCount);
            w.Write(2); // Vertex count.
            if (chunkId >= 0x0900600A) w.Write(0); // Vertex-stream count.
            if (chunkId == 0x09006003)
            {
                foreach (var value in new float[] { 1, 2, 4, 5 }) w.Write(value);
            }
            else
            {
                w.Write(1); // XYZ texture coordinates in the legacy kind format.
                foreach (var value in new float[] { 1, 2, 3, 4, 5, 6 }) w.Write(value);
            }

            if (chunkId >= 0x0900600C)
            {
                w.Write(true);
                if (version >= 4) w.Write(unchecked((int)0x80000003));
                else w.Write(true);
                if (version >= 3)
                {
                    w.Write(weights);
                    w.Write(names);
                }
            }
            if (weights) w.Write(vertexWeights);
            if (chunkId >= 0x0900600C)
            {
                w.Write(2); // Bone count.
                var legacy = chunkId < 0x0900600F || version == 0;
                if (legacy)
                {
                    w.Write(Iso4.Identity);
                    w.Write(Iso4.Identity with { TX = 10 });
                    if (chunkId == 0x0900600C)
                    {
                        w.WriteIdAsString("old0");
                        w.WriteIdAsString("old1");
                    }
                }
                if (names)
                {
                    w.WriteIdAsString("bone0");
                    if (legacy) w.Write(Bounds);
                    w.WriteIdAsString("bone1");
                    if (legacy) w.Write(Bounds);
                }
                if (chunkId == 0x0900600F && version != 1)
                {
                    w.Write(2);
                    w.Write(10);
                    w.Write(20);
                }
            }
            if (chunkId < 0x0900600D) w.Write(true); // Vertex colors.
            if (chunkId < 0x0900600C)
            {
                w.Write(1);
                w.Write(Iso4.Identity);
            }
            else w.Write(Bounds);
            if (chunkId >= 0x0900600E)
            {
                w.Write(1);
                w.Write(3); // Mapper index.
                w.Write(new Vec2(0.5f, 0.25f));
                w.Write(new Vec2(0.125f, 0.75f));
            }
            if (chunkId == 0x0900600F && version >= 5)
            {
                w.Write(2);
                w.Write((ushort)0);
                w.Write(ushort.MaxValue);
            }
            if (chunkId == 0x0900600F && version >= 6)
            {
                w.Write(0); // Packed-data version.
                w.Write(7); // Includes the four-byte size header.
                w.Write(new byte[] { 12, 34, 56 });
            }
            w.Write(0xFACADE01u);
        }
        var node = await RoundTrip(payload);
        await Assert.That(node.VertexCount).IsEqualTo(2);
        await Assert.That(node.SkinIndexCount).IsEqualTo(skinCount);
        await Assert.That(node.IsGeometryStatic).IsTrue();
        await Assert.That(node.IsIndexationStatic).IsTrue();
        await Assert.That(node.HasVertexColors).IsTrue();
        await Assert.That(node.TexCoords[0].TexCoords[1].Z).IsEqualTo(chunkId == 0x09006003 ? default(float?) : 6f);
        if (weights) await Assert.That(node.SkinData!.VertexWeights).IsEquivalentTo(vertexWeights, CollectionOrdering.Matching);
        if (chunkId >= 0x0900600C)
        {
            await Assert.That(node.SkinData!.BoneCount).IsEqualTo(2);
            await Assert.That(node.SkinData.Bones!.Length).IsEqualTo(names ? 2 : 0);
            await Assert.That(node.BoundingBox).IsEqualTo(Bounds);
        }
        if (chunkId >= 0x0900600E)
        {
            await Assert.That(node.BitmapElemToPacks[0].MapperIndex).IsEqualTo(3);
            await Assert.That(node.BitmapElemToPacks[0].Scale).IsEqualTo(new Vec2(0.5f, 0.25f));
            await Assert.That(node.BitmapElemToPacks[0].Translation).IsEqualTo(new Vec2(0.125f, 0.75f));
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Morphs_PreserveCountsBeforeTheVertexStream(bool empty)
    {
        using var payload = new MemoryStream();
        using (var w = new GbxWriter(payload))
        {
            w.Write(0x09006010u);
            w.Write(0);
            w.Write(empty ? 0 : 1);
            if (!empty)
            {
                w.Write(2);
                w.Write(0);
                w.Write(7);
                w.Write(2); // Bone IDs follow the vertex-stream reference.
                w.WriteNodeRef(new CPlugVertexStream());
                w.WriteIdAsString("morph0");
                w.WriteIdAsString("morph1");
            }
            w.Write(0xFACADE01u);
        }
        var node = await RoundTrip(payload);
        await Assert.That(node.Morphs.Length).IsEqualTo(empty ? 0 : 1);
        if (!empty)
        {
            await Assert.That(node.Morphs[0].Indices).IsEquivalentTo(new[] { 0, 7 }, CollectionOrdering.Matching);
            await Assert.That(node.Morphs[0].VertexStream).IsNotNull();
            await Assert.That(node.Morphs[0].Bones).IsEquivalentTo(new[] { "morph0", "morph1" }, CollectionOrdering.Matching);
        }
    }

    [Test]
    public async Task Defaults_MatchTheNativeVisualConstructors()
    {
        var node = new CPlugVisual();
        await Assert.That(node.Flags).IsEqualTo(0x804F0);
        await Assert.That(node.BoundingBox).IsEqualTo(new BoxAligned(0, 0, 0, -1, -1, -1));
        await Assert.That(new CPlugVisual2D().HasVertexNormals).IsFalse();
        await Assert.That(new CPlugVisual.Chunk0900600F(GameVersion.TMT).Version).IsEqualTo(4);
        await Assert.That(new CPlugVisual.Chunk0900600F(GameVersion.MP4).Version).IsEqualTo(5);
        await Assert.That(new CPlugVisual.Chunk0900600F(GameVersion.TM2020).Version).IsEqualTo(6);
    }

    private static async Task<CPlugVisual> RoundTrip(MemoryStream payload)
    {
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugVisual>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
        return node;
    }
}
