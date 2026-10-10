using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugCrystalLayoutTests
{
    [Test]
    [Arguments(25)]
    [Arguments(26)]
    [Arguments(28)]
    [Arguments(31)]
    [Arguments(32)]
    [Arguments(34)]
    [Arguments(35)]
    [Arguments(37)]
    public async Task EditableCrystalPreservesSparseHandlesAndMeshAttributes(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload)) WriteEditableCrystalPayload(writer, version);
        payload.Position = 0;
        var node = new CPlugCrystal();
        var crystal = new CPlugCrystal.Crystal();
        using (var reader = new GbxReader(payload)) crystal.Read(reader, node);

        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(crystal.Edges[0]).IsEqualTo(new Int2(0, 2));
        await Assert.That(crystal.VertexPool.Handles[1].IsFree).IsTrue();
        await Assert.That(crystal.Faces[0].Normal).IsEqualTo(new Vec3(0, 1, 0));
        await Assert.That(crystal.Faces[0].Vertices[2].TexCoord).IsEqualTo(new Vec2(2, 3));
        await Assert.That(crystal.EditableMesh.Faces[0].Vertices[0].Color).IsEqualTo(new Vec4(0.5f, 1, 2, 3));
        await Assert.That(crystal.FaceFlags[0]).IsEqualTo(13);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) crystal.Write(writer, node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(26, 0, 3)]
    [Arguments(27, 0, 3)]
    [Arguments(28, 0, 3)]
    [Arguments(29, 0, 3)]
    [Arguments(31, 0, 3)]
    [Arguments(33, 255, 3)]
    [Arguments(34, 65535, 255)]
    [Arguments(35, 0, 3)]
    [Arguments(36, 0, 3)]
    [Arguments(37, 0, 3)]
    public async Task EmbeddedCrystalPreservesNativeVersionBranches(int version, int maximumMaterialIndex, int positionCount)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteCrystalPayload(writer, version, maximumMaterialIndex, positionCount);
        }

        payload.Position = 0;
        var node = new CPlugCrystal();
        var crystal = new CPlugCrystal.Crystal();
        using (var reader = new GbxReader(payload)) crystal.Read(reader, node);

        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(crystal.Groups[0].IsInUse).IsTrue();
        await Assert.That(crystal.Groups[0].ParentIndex).IsEqualTo(-1);
        await Assert.That(crystal.Faces[0].Material).IsNull();
        await Assert.That(crystal.Faces[0].Vertices[2].Index).IsEqualTo(2);
        await Assert.That(crystal.Faces[0].Vertices[2].TexCoord).IsEqualTo(new Vec2(2, 3));
        await Assert.That(crystal.U04).IsEqualTo(0x12345678);
        if (version < 27) await Assert.That(crystal.Faces[0].TexCoordLayers.Length).IsEqualTo(2);
        if (version < 29) await Assert.That(crystal.FaceProperties[0]).IsEqualTo(11);
        if (version < 30) await Assert.That(crystal.FaceFlags[0]).IsEqualTo(13);
        if (version < 36) await Assert.That(crystal.U09[^1]).IsEqualTo(22);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) crystal.Write(writer, node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(3)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    [Arguments(8)]
    public async Task VoxelSpacePreservesDimensionsAndVoxelAttributes(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (version < 6) writer.Write((byte)8);
            else writer.Write(new Int3(8, 16, 32));
            writer.Write(0.5f);
            if (version >= 3) writer.Write(new Vec3(1, 2, 3));
            if (version >= 5) writer.Write(42);
            if (version >= 8) writer.Write(Iso4.Identity with { TX = 7 });
            writer.Write(1); // Models.
            writer.Write(12); // Model index.
            writer.Write(1); // Voxels.
            writer.Write(123);
            if (version >= 6) writer.Write(456);
            if (version >= 7) writer.Write(new byte[] { 1, 2, 3, 4 });
        }

        payload.Position = 0;
        var voxels = new CPlugCrystal.VoxelSpace();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) voxels.ReadWrite(rw, version);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(voxels.Models![0].ModelIndex).IsEqualTo(12);
        await Assert.That(voxels.Models[0].Voxels![0].Index).IsEqualTo(123);
        if (version >= 6) await Assert.That(voxels.Dimensions).IsEqualTo(new Int3(8, 16, 32));
        if (version >= 7) await Assert.That(voxels.Models[0].Voxels![0].U05).IsEqualTo((byte)4);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) voxels.ReadWrite(rw, version);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task DeformationLayerPreservesCachedCrystalAndTransforms()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0); // Chunk version.
            writer.Write(1); // Layers.
            writer.Write(12); // Deformation.
            writer.Write(2); // Base layer version.
            writer.Write(true);
            WriteCrystalPayload(writer, 37, 0, 3);
            writer.WriteIdAsString("Deformation");
            writer.Write("Deformation layer");
            writer.Write(true);
            writer.Write(0); // Modifier version.
            writer.Write(0); // Mask.
            writer.Write(0); // Deformation version.
            writer.Write(new BoxAligned(0, 0, 0, 1, 2, 3));
            writer.Write(Iso4.Identity);
            writer.Write(1);
            writer.Write(Iso4.Identity with { TX = 10, TY = 20, TZ = 30 });
        }

        payload.Position = 0;
        var node = new CPlugCrystal();
        var chunk = new CPlugCrystal.Chunk09003005();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        var layer = (CPlugCrystal.DeformationLayer)node.Layers.Single();
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(layer.CachedCrystal!.Version).IsEqualTo(37);
        await Assert.That(layer.LayerId).IsEqualTo("Deformation");
        await Assert.That(layer.Transforms![0].Translation).IsEqualTo(new Vec3(10, 20, 30));

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacyVoxelChunkPreservesEveryCell(bool hasVertexValues)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0);
            writer.Write(true);
            writer.Write((byte)2);
            writer.Write(0.5f);
            writer.Write(hasVertexValues);
            writer.Write(7);
            for (var i = 0; i < 8; i++)
            {
                writer.Write(i % 2 == 0);
                writer.Write(i);
                if (hasVertexValues) writer.Write(new Vec4(i, 2, 3, 4));
            }
        }

        payload.Position = 0;
        var node = new CPlugCrystal();
        var chunk = new CPlugCrystal.Chunk09003008();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.LegacyVoxels!.Voxels!.Length).IsEqualTo(8);
        await Assert.That(node.LegacyVoxels.Voxels[7].ModelIndex).IsEqualTo(7);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    public async Task MaterialChunkResolvesFacesReadBeforeTheMaterialTable(int version)
    {
        var part = new CPlugCrystal.Part();
        var face = new CPlugCrystal.Face([], part, null, null) { MaterialIndex = 0 };
        var node = new CPlugCrystal
        {
            Layers = [new CPlugCrystal.GeometryLayer { Crystal = new CPlugCrystal.Crystal { Faces = [face] } }]
        };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(1);
            writer.Write("TestMaterial");
            if (version == 1)
            {
                writer.Write(0);
                writer.Write(new Vec4(1, 2, 3, 4));
            }
        }

        payload.Position = 0;
        var chunk = new CPlugCrystal.Chunk09003003();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(face.Material!.MaterialName).IsEqualTo("TestMaterial");
        if (version == 1) await Assert.That(face.LegacyMaterialValues).IsEqualTo(new Vec4(1, 2, 3, 4));

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0, 8)]
    [Arguments(1, 8)]
    [Arguments(2, 12)]
    public async Task EmptyLightmapChunkPreservesVersionedIndexArray(int version, int payloadLength)
    {
        var node = new CPlugCrystal();
        var chunk = new CPlugCrystal.Chunk09003006 { Version = version };
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) chunk.Write(node, writer);
        await Assert.That(saved.Length).IsEqualTo((long)payloadLength);
        saved.Position = 0;
        using (var reader = new GbxReader(saved)) chunk.Read(node, reader);
        await Assert.That(saved.Position).IsEqualTo(saved.Length);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(0, 2)]
    [Arguments(0, 3)]
    [Arguments(1, 0)]
    [Arguments(1, 2)]
    [Arguments(1, 3)]
    [Arguments(2, 0)]
    [Arguments(2, 2)]
    [Arguments(2, 3)]
    public async Task LightmapChunkPreservesIndependentTablesAndIndexOrder(int version, int lightmapCount)
    {
        var face = new CPlugCrystal.Face(
            [new(0, default, default), new(1, default, default), new(2, default, default)],
            new CPlugCrystal.Part(), null, null);
        var node = new CPlugCrystal
        {
            Layers = [new CPlugCrystal.GeometryLayer { Crystal = new CPlugCrystal.Crystal { Faces = [face] } }]
        };
        var coordCount = version == 2 && lightmapCount > 0 ? 2 : lightmapCount;
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(coordCount);
            for (var i = 0; i < coordCount; i++)
            {
                if (version == 0) writer.Write(new Vec2(i / 2f, 1 - i / 2f));
                else
                {
                    writer.Write((ushort)(i == 0 ? 0 : i == 1 ? 32768 : ushort.MaxValue));
                    writer.Write((ushort)(i == 0 ? ushort.MaxValue : i == 1 ? 16384 : 0));
                }
            }
            if (version == 2)
            {
                // The first used coordinate is not first in the shared table.
                writer.WriteArrayOptimizedInt(Enumerable.Range(0, lightmapCount).Select(i => (i + 1) % 2).ToArray());
            }
        }
        payload.Position = 0;
        var chunk = new CPlugCrystal.Chunk09003006();
        using (var reader = new GbxReader(payload)) chunk.Read(node, reader);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(chunk.LightmapCoords.Length).IsEqualTo(coordCount);
        if (lightmapCount != face.Vertices.Length)
        {
            await Assert.That(face.Vertices.All(vertex => vertex.LightmapCoord == default)).IsTrue();
        }
        else
        {
            await Assert.That(face.Vertices[0].LightmapCoord).IsEqualTo(chunk.LightmapCoords[version == 2 ? 1 : 0]);
        }

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) chunk.Write(node, writer);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);

        if (lightmapCount == face.Vertices.Length)
        {
            face.Vertices[0] = face.Vertices[0] with { LightmapCoord = new Vec2(0.25f, 0.75f) };
            using var edited = new MemoryStream();
            using (var writer = new GbxWriter(edited)) chunk.Write(node, writer);
            face.Vertices[0] = face.Vertices[0] with { LightmapCoord = default };
            edited.Position = 0;
            using (var reader = new GbxReader(edited)) new CPlugCrystal.Chunk09003006().Read(node, reader);
            var expected = version == 0 ? new Vec2(0.25f, 0.75f) : new Vec2(16383 / 65535f, 49151 / 65535f);
            await Assert.That(face.Vertices[0].LightmapCoord).IsEqualTo(expected);
        }
    }

    private static void WriteCrystalPayload(GbxWriter writer, int version, int maximumMaterialIndex, int positionCount)
    {
        writer.Write(version);
        writer.Write(4); // Base visual level.
        writer.Write(0); // Visual levels.
        writer.Write(0); // Anchor infos.
        writer.Write(1); // Parts.
        if (version >= 31) writer.Write(0);
        writer.Write(true, asByte: version >= 35);
        writer.Write(-1); // Parent.
        writer.Write("part");
        writer.Write(-1); // Anchor.
        writer.Write(0); // Children.
        if (version < 29)
        {
            writer.Write(true);
            writer.Write(true);
        }
        writer.Write(true, asByte: version >= 35);
        if (version >= 33)
        {
            writer.Write(maximumMaterialIndex);
            writer.Write(0); // Maximum group index.
        }
        writer.Write(positionCount);
        for (var i = 0; i < positionCount; i++) writer.Write(new Vec3(i, 0, 0));
        writer.Write(version < 35 ? 1 : 3); // Total edges.
        if (version >= 35) writer.Write(1); // Loose edges.
        WriteIndex(writer, 0, version < 34 ? int.MaxValue : positionCount);
        WriteIndex(writer, 2, version < 34 ? int.MaxValue : positionCount);
        writer.Write(1); // Faces.
        if (version >= 37)
        {
            writer.Write(3); // Shared UVs.
            for (var i = 0; i < 3; i++) writer.Write(new Vec2(i, i + 1));
            writer.Write(3); // Shared UV indices.
            writer.Write(new byte[] { 0, 1, 2 });
        }
        if (version >= 35) writer.Write((byte)0); // Triangle.
        else writer.Write(3);
        for (var i = 0; i < 3; i++) WriteIndex(writer, i, version < 34 ? int.MaxValue : positionCount);
        if (version < 27)
        {
            writer.Write(2); // UV layers.
            for (var layer = 0; layer < 2; layer++)
            {
                for (var i = 0; i < 3; i++) writer.Write(new Vec2(i + layer * 10, i + 1));
            }
            writer.Write(new Vec3(0, 1, 0));
        }
        else if (version < 37)
        {
            for (var i = 0; i < 3; i++) writer.Write(new Vec2(i, i + 1));
        }
        WriteIndex(writer, -1, version < 33 ? int.MaxValue : maximumMaterialIndex);
        if (version < 28) writer.Write(new Vec4(1, 2, 3, 4));
        WriteIndex(writer, 0, version < 33 ? int.MaxValue : 0);
        if (version < 29)
        {
            writer.Write(11); // Face properties.
            writer.Write(12); // Legacy face value.
        }
        if (version < 30) writer.Write(13); // Face flags.
        if (version < 29)
        {
            for (var i = 0; i < positionCount; i++) writer.Write(i + 0.5f);
        }
        writer.Write(0x12345678);
        if (version < 32)
        {
            writer.Write(0); // Links.
            writer.Write(123);
            writer.Write("legacy");
        }
        if (version < 30)
        {
            writer.Write(1); // Smoothing groups.
            writer.Write(0.75f);
        }
        if (version < 36)
        {
            writer.Write(1); // Face selection count.
            writer.Write(version < 35 ? 1 : 3); // Edge selection count.
            writer.Write(positionCount); // Vertex selection count.
            writer.Write(21);
            for (var i = 0; i < (version < 35 ? 1 : 3); i++) writer.Write(22);
            for (var i = 0; i < positionCount; i++) writer.Write(23);
            writer.Write(24);
        }
    }

    private static void WriteEditableCrystalPayload(GbxWriter writer, int version)
    {
        writer.Write(version);
        writer.Write(4);
        writer.Write(0); // Visual levels.
        writer.Write(0); // Anchors.
        writer.Write(1); // Parts.
        if (version >= 31) writer.Write(0);
        writer.Write(true, asByte: version >= 35);
        writer.Write(-1);
        writer.Write("part");
        writer.Write(-1);
        writer.Write(0);
        if (version < 29)
        {
            writer.Write(false);
            writer.Write(false);
        }
        writer.Write(false, asByte: version >= 35);
        if (version >= 33)
        {
            writer.Write(0);
            writer.Write(0);
        }

        writer.Write(4); // Sparse vertex handles, used chain 2 -> 0 -> 3.
        WriteHandle(writer, false, 3, 3);
        WriteHandle(writer, true, 5, int.MaxValue);
        WriteHandle(writer, false, 7, 0);
        WriteHandle(writer, false, 9, int.MaxValue);
        writer.Write(1); // First free.
        writer.Write(2); // First used.
        for (var i = 0; i < 2; i++) // Edge and face pools.
        {
            writer.Write(1);
            WriteHandle(writer, false, 1, int.MaxValue);
            writer.Write(int.MaxValue);
            writer.Write(0);
        }
        writer.Write(new Int2((2 << 10) | 7, (3 << 10) | 9));
        if (version < 32)
        {
            writer.Write(1);
            writer.Write(0);
            writer.Write(3);
            if (version < 26)
            {
                writer.Write(4);
                writer.Write(-1); // No guide edge.
            }
            writer.Write(true);
            writer.Write(new Vec4(5, 6, 7, 8));
        }
        writer.Write(0); // Mesh version.
        writer.Write(3);
        for (var i = 0; i < 3; i++) writer.Write(new Vec3(i, 0, 0));
        writer.Write(1);
        writer.Write(new Vec3(0, 1, 0));
        writer.Write(1); // Mesh faces.
        writer.Write(3);
        for (var i = 0; i < 3; i++)
        {
            writer.Write(i);
            writer.Write(0); // Normal index.
            writer.Write(new Vec4(0.5f, 1, 2, 3));
        }
        writer.Write(2); // UV layers.
        for (var layer = 0; layer < 2; layer++)
        {
            writer.Write(layer + 4); // Preserve arbitrary channel IDs.
            for (var i = 0; i < 3; i++) writer.Write(new Vec2(i + layer * 10, i + 1));
        }
        writer.Write(2); // Triangle face type.
        if (version != 28) writer.Write(11);
        if (version < 28) writer.Write(12);
        writer.Write(13); // Face flags.
        writer.Write(0); // Group.
        if (version < 29)
        {
            for (var i = 0; i < 3; i++) writer.Write(i + 0.5f);
        }
        writer.Write(0x12345678);
        if (version < 32)
        {
            writer.Write(0); // Links.
            writer.Write(123);
            writer.Write("legacy");
        }
        if (version < 30) writer.WriteArray(new[] { 0.75f });
        if (version < 36)
        {
            writer.Write(1);
            writer.Write(1);
            writer.Write(3);
            writer.Write(21);
            writer.Write(22);
            for (var i = 0; i < 3; i++) writer.Write(23);
            writer.Write(24);
        }
        writer.Write(-1); // Material index.
        writer.Write(new Vec4(1, 2, 3, 4));
    }

    private static void WriteHandle(GbxWriter writer, bool isFree, int generation, int next)
    {
        writer.Write(isFree);
        writer.Write(generation);
        writer.Write(next);
    }

    private static void WriteIndex(GbxWriter writer, int value, int maximum)
    {
        if (maximum < 255) writer.Write((byte)value);
        else if (maximum < 65535) writer.Write((ushort)value);
        else writer.Write(value);
    }
}
