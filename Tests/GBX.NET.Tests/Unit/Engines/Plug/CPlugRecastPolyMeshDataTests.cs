using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugRecastPolyMeshDataTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    [Arguments(0, false, false)]
    [Arguments(1, false, false)]
    [Arguments(2, false, false)]
    [Arguments(3, false, false)]
    [Arguments(3, false, true)]
    [Arguments(4, false, false)]
    [Arguments(4, false, true)]
    [Arguments(4, true, false)]
    [Arguments(4, true, true)]
    [Arguments(5, false, false)]
    [Arguments(5, true, false)]
    public async Task ReadWrite_NativeVersions_PreserveMeshAndLayersAndDiscardLegacyMeshes(
        int version, bool hasLayers, bool hasLegacyMeshes)
    {
        var payload = NativePayload(version, hasLayers, hasLegacyMeshes);
        var node = new CPlugRecastPolyMeshData();
        var chunk = new CPlugRecastPolyMeshData.Chunk09150000();
        using (var input = new MemoryStream(payload))
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }
        await Assert.That(chunk.Version).IsEqualTo(version);
        await Assert.That(chunk.U02!.SequenceEqual(new[] { 12345, 67890 })).IsTrue();
        if (hasLayers)
        {
            await Assert.That(node.HeightfieldLayers?.Length).IsEqualTo(1);
            var layer = node.HeightfieldLayers![0];
            await Assert.That(layer.BoundsMin).IsEqualTo(new Vec3(1, 2, 3));
            await Assert.That(layer.BoundsMax).IsEqualTo(new Vec3(4, 5, 6));
            await Assert.That(layer.CellSize).IsEqualTo(7f);
            await Assert.That(layer.CellHeight).IsEqualTo(8f);
            await Assert.That(layer.Width).IsEqualTo(2);
            await Assert.That(layer.Height).IsEqualTo(1);
            await Assert.That(layer.Heights!.SequenceEqual(new byte[] { 1, 2 })).IsTrue();
            await Assert.That(layer.Areas!.SequenceEqual(new byte[] { 3, 4 })).IsTrue();
            await Assert.That(layer.Connections!.SequenceEqual(new byte[] { 5, 6 })).IsTrue();
        }
        else
        {
            await Assert.That(node.HeightfieldLayers).IsNull();
        }

        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer))
        {
            chunk.ReadWrite(node, rw);
            writer.Write(Sentinel);
        }
        await Assert.That(output.ToArray().SequenceEqual(NativePayload(version, hasLayers, false))).IsTrue();
    }

    private static byte[] NativePayload(int version, bool hasLayers, bool hasLegacyMeshes)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        writer.Write(version);
        if (version >= 2) writer.Write(42);
        if (version is 3 or 4)
        {
            writer.Write(hasLegacyMeshes ? 1 : 0);
            if (hasLegacyMeshes)
            {
                writer.Write(1); // Vertex count.
                writer.Write(1); // Polygon count.
                writer.Write(2); // Polygon capacity.
                writer.Write(3); // Vertices per polygon.
                writer.Write(new byte[40]); // Bounds, cell sizes, border and edge error.
                writer.Write(new byte[6 + 24 + 4 + 4 + 2]); // Vertices, polygons, regions, flags and areas.
            }
            writer.Write(hasLegacyMeshes ? 1 : 0);
            if (hasLegacyMeshes)
            {
                writer.Write(1); // Mesh count.
                writer.Write(2); // Vertex count.
                writer.Write(3); // Triangle count.
                writer.Write(new byte[16 + 24 + 12]);
            }
        }
        writer.Write(2); // Four-byte mesh data element count.
        writer.Write(12345);
        writer.Write(67890);
        if (version >= 4)
        {
            writer.Write(hasLayers ? 1 : 0);
            if (hasLayers)
            {
                writer.Write(1); // Layer count.
                for (var i = 1; i <= 8; i++) writer.Write((float)i);
                writer.Write(2); // Width.
                writer.Write(1); // Height.
                for (var i = 9; i <= 14; i++) writer.Write(i);
                writer.Write(new byte[] { 1, 2, 3, 4, 5, 6 });
            }
        }
        writer.Write(Sentinel);
        return stream.ToArray();
    }
}
