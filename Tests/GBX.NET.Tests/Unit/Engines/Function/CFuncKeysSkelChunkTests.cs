using GBX.NET.Engines.Function;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Function;

[Category("Unit")]
public class CFuncKeysSkelChunkTests
{
    [Test]
    public async Task Chunk05006001_RoundTripsFixedLengthLocationRows()
    {
        var original = new CFuncKeysSkel
        {
            Skel = new CFuncSkel { Bones = ["A", "B"] },
            Locations =
            [
                [
                    new() { Rotation = new Quat(1, 2, 3, 4), Position = new Vec3(5, 6, 7) },
                    new() { Rotation = new Quat(8, 9, 10, 11), Position = new Vec3(12, 13, 14) }
                ],
                [
                    new() { Rotation = new Quat(15, 16, 17, 18), Position = new Vec3(19, 20, 21) },
                    new() { Rotation = new Quat(22, 23, 24, 25), Position = new Vec3(26, 27, 28) }
                ]
            ]
        };

        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        using (var rw = new GbxReaderWriter(writer))
        {
            new CFuncKeysSkel.Chunk05006001().ReadWrite(original, rw);
        }

        await Assert.That(stream.Length).IsEqualTo(4 + 4 * (4 + 3) * sizeof(float));

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        using var readerWriter = new GbxReaderWriter(reader);
        var restored = new CFuncKeysSkel { Skel = new CFuncSkel { Bones = ["A", "B"] } };
        new CFuncKeysSkel.Chunk05006001().ReadWrite(restored, readerWriter);

        await Assert.That(restored.Locations).Count().IsEqualTo(2);
        await Assert.That(restored.Locations![0]).Count().IsEqualTo(2);
        await Assert.That(restored.Locations[1]).Count().IsEqualTo(2);
        await Assert.That(restored.Locations[0][0].Rotation).IsEqualTo(original.Locations[0][0].Rotation);
        await Assert.That(restored.Locations[0][1].Position).IsEqualTo(original.Locations[0][1].Position);
        await Assert.That(restored.Locations[1][0].Rotation).IsEqualTo(original.Locations[1][0].Rotation);
        await Assert.That(restored.Locations[1][1].Position).IsEqualTo(original.Locations[1][1].Position);
    }

    [Test]
    public void Chunk05006001_RejectsRowsWithWrongBoneCount()
    {
        var node = new CFuncKeysSkel
        {
            Skel = new CFuncSkel { Bones = ["A", "B"] },
            Locations = [[new CFuncKeysSkel.Loc()]]
        };

        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        using var rw = new GbxReaderWriter(writer);

        Assert.Throws<InvalidOperationException>(() => new CFuncKeysSkel.Chunk05006001().ReadWrite(node, rw));
    }

    [Test]
    public async Task Chunk05006001_WritesEmptyLocationsWithoutSkeleton()
    {
        var node = new CFuncKeysSkel();

        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        using (var rw = new GbxReaderWriter(writer))
        {
            new CFuncKeysSkel.Chunk05006001().ReadWrite(node, rw);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0);
        await Assert.That(new CFuncSkel().BonesCount).IsEqualTo(0);
    }
}
