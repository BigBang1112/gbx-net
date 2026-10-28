using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugSurfaceArchiveTests
{
    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 0)]
    [Arguments(2, 0)]
    [Arguments(2, 1)]
    [Arguments(2, 2)]
    public async Task ReadWrite_C003Box_PreservesMaterialIndexBeforeTheMaterialTable(int version, int surfVersion)
    {
        BoxAligned bounds = (-1, -2, -3, 4, 5, 6);
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            if (version >= 2) writer.Write(surfVersion);
            writer.Write(6); // Box.
            writer.Write(bounds);
            writer.Write((short)0); // Primitive material index.
            if (surfVersion >= 2) writer.Write(new Vec3(1, 0, 0));
            writer.Write(1); // Materials.
            writer.Write(false); // Built-in material.
            writer.Write((short)CPlugSurface.MaterialId.Grass);
            writer.Write(0); // Legacy material data.
            if (version >= 1) writer.Write(-1); // Skeleton.
            writer.Write(0x11223344);
        }

        payload.Position = 0;
        var node = new CPlugSurface();
        var chunk = new CPlugSurface.Chunk0900C003();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }

        var box = (CPlugSurface.Box)node.Surf!;
        await Assert.That(box.Transform).IsEqualTo(bounds);
        await Assert.That(box.SurfaceIndex).IsEqualTo((short)0);
        await Assert.That(node.Materials.Single().SurfaceId).IsEqualTo((CPlugSurface.MaterialId?)CPlugSurface.MaterialId.Grass);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..^4], CollectionOrdering.Matching);
    }
}
