using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugSurfaceGeomLayoutTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWrite_BoundingBox_PreservesGeometryAndEncryptionKey(bool encrypted)
    {
        BoxAligned bounds = (4, 5, 6, 17, 18, 19);
        using var payload = new MemoryStream();
        long prefixLength;
        using (var writer = new GbxWriter(payload))
        {
            writer.WriteIdAsString("LegacySurface");
            writer.Write(bounds);
            prefixLength = payload.Position;
            writer.Write(0); // GmSurfSphere.
            writer.Write(2.5f);
            writer.Write((short)CPlugSurface.MaterialId.Concrete);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var initializer = new RecordingEncryptionInitializer(payload);
        var node = new CPlugSurfaceGeom();
        await Assert.That(node.BoundingBox).IsEqualTo(new BoxAligned(0, 0, 0, -1, -1, -1));
        var chunk = new CPlugSurfaceGeom.Chunk0900F004();
        using (var reader = new GbxReader(payload, new() { EncryptionInitializer = encrypted ? initializer : null }))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(payload.Position).IsEqualTo((long)expected.Length);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(chunk.U01).IsEqualTo("LegacySurface");
        await Assert.That(node.BoundingBox).IsEqualTo(bounds);
        var sphere = (await Assert.That(node.Surf).IsTypeOf<CPlugSurface.Sphere>())!;
        await Assert.That(sphere.Size).IsEqualTo(2.5f);
        await Assert.That(node.SurfaceId).IsEqualTo((CPlugSurface.MaterialId?)CPlugSurface.MaterialId.Concrete);
        await Assert.That(initializer.Calls).IsEqualTo(encrypted ? 1 : 0);
        if (encrypted)
        {
            await Assert.That(initializer.Data).IsEquivalentTo(BitConverter.GetBytes(-13f), CollectionOrdering.Matching);
            await Assert.That(initializer.Position).IsEqualTo(prefixLength);
        }

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    private sealed class RecordingEncryptionInitializer(Stream stream) : IEncryptionInitializer
    {
        public int Calls { get; private set; }
        public long Position { get; private set; }
        public byte[] Data { get; private set; } = [];

        public void Initialize(byte[] data, uint offset, uint count)
        {
            Calls++;
            Position = stream.Position;
            Data = data.AsSpan((int)offset, (int)count).ToArray();
        }
    }
}
