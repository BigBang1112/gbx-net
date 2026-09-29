using System.IO;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Tests.Integration;

[Category("Integration")]
public class GmSurfTests
{
    public static IEnumerable<(int SurfId, string? SurfType, string Suffix)> Fixtures() =>
    [
        (-1, null, ""),
        (0, "Sphere", ""),
        (1, "Ellipsoid", ""),
        (6, "Box", ""),
        (7, "Mesh", ""),
        (8, "VCylinder", ""),
        (9, "MultiSphere", ""),
        (10, "ConvexPolyhedron", ""),
        (11, "Capsule", ""),
        (12, "Circle", ""),
        (13, "Compound", ""),
        (14, "SphereLocated", ""),
        (15, "CompoundInstance", ""),
        (16, "Cylinder", ""),
        (17, "SphericalShell", ""),
        (18, "Voxel", ""),
        (19, "Diggable", ""),
        (10, "ConvexPolyhedron", "Procedural"),
        (8, "VCylinder", "Version0"),
        (14, "SphereLocated", "Version0"),
        (16, "Cylinder", "Version0"),
        (9, "MultiSphere", "Primary"),
        (9, "MultiSphere", "Fallback")
    ];

    [Test]
    [MethodDataSource(nameof(Fixtures))]
    public async Task NativeFixtureRoundTripsWithoutChangingArchive(int surfId, string? surfType, string suffix = "")
    {
        var path = TestFiles.Gbx("CPlugSurface", $"GmSurfFixture{surfId}{suffix}.Shape.Gbx");
        using var expected = new MemoryStream();
        Gbx.Decompress(path, expected);
        expected.Position = 0;

        var gbx = Gbx.Parse(path);
        gbx.BodyCompression = GbxCompression.Uncompressed;
        var surface = (await Assert.That(gbx.Node).IsTypeOf<CPlugSurface>())!;
        await Assert.That(surface.Surf?.GetType().Name).IsEqualTo(surfType);
        await Assert.That(surface.SurfVersion).IsEqualTo(suffix == "Version0" ? 0 : 2);
        if (surface.Surf is not null)
        {
            await Assert.That(surface.Surf.GameplayMainDir).IsEqualTo(suffix == "Version0" ? (Vec3?)null : new Vec3(0, 0, 1));
        }
        if (surface.Surf is CPlugSurface.MultiSphere multiSphere)
        {
            var sphereEntry = await Assert.That(multiSphere.Spheres).HasSingleItem();
            await Assert.That(sphereEntry.Radius).IsEqualTo(1f);
            await Assert.That(sphereEntry.Center).IsEqualTo(new Vec3(0, 0, 0));
            await Assert.That(multiSphere.SurfaceIndex).IsEqualTo((short)0);
        }
        if (suffix == "Primary")
        {
            await Assert.That(surface.Chunks.Get<CPlugSurface.Chunk0900C003>()!.U04).IsEquivalentTo(new ushort[] { 0x0100, 0x0201 }, CollectionOrdering.Matching);
        }
        if (suffix == "Fallback")
        {
            await Assert.That(surface.Materials.Length).IsEqualTo(2);
            await Assert.That(surface.Chunks.Get<CPlugSurface.Chunk0900C003>()!.U02).IsEquivalentTo(new ushort[] { 0, 1 }, CollectionOrdering.Matching);
        }
        if (surface.Surf is CPlugSurface.ConvexPolyhedron polyhedron && suffix != "Procedural")
        {
            await Assert.That(polyhedron.Version).IsEqualTo(0);
            await Assert.That(polyhedron.Representation).IsEqualTo(0);
            await Assert.That(polyhedron.Vertices).IsEquivalentTo(new Vec3[] { new(0, 0, 0), new(1, 0, 0), new(0, 1, 0), new(0, 0, 1) }, CollectionOrdering.Matching);
            await Assert.That(polyhedron.Indices).IsEquivalentTo(new uint[] { 0, 2, 1, 0, 1, 3, 0, 3, 2, 1, 2, 3 }, CollectionOrdering.Matching);
            await Assert.That(polyhedron.Faces.Length).IsEqualTo(4);
            await Assert.That(polyhedron.Faces[3]).IsEqualTo(new CPlugSurface.ConvexPolyhedron.Face(9, 3));
            await Assert.That(polyhedron.SurfaceIndex).IsEqualTo((short)0);
        }
        if (surface.Surf is CPlugSurface.ConvexPolyhedron procedural && suffix == "Procedural")
        {
            await Assert.That(procedural.Representation).IsEqualTo(1);
            await Assert.That(procedural.Parameters).IsEqualTo(new Vec3(1, .5f, 2));
        }
        if (surface.Surf is CPlugSurface.SphereLocated sphere)
        {
            await Assert.That(sphere.Center).IsEqualTo(new Vec3(0, 0, 0));
            await Assert.That(sphere.Radius).IsEqualTo(1f);
            await Assert.That(sphere.SurfaceIndex).IsEqualTo((short)0);
        }
        if (surface.Surf is CPlugSurface.Capsule capsule)
        {
            await Assert.That(capsule.SphereCenter).IsEqualTo(new Vec3(0, 0, 0));
            await Assert.That(capsule.Dir).IsEqualTo(new Vec3(0, 1, 0));
            await Assert.That(capsule.Length).IsEqualTo(1f);
            await Assert.That(capsule.SurfaceIndex).IsEqualTo((short)0);
        }
        if (surface.Surf is CPlugSurface.SphericalShell shell)
        {
            await Assert.That(shell.InnerRadius).IsEqualTo(.5f);
            await Assert.That(shell.OuterRadius).IsEqualTo(1f);
            await Assert.That(shell.SkipInToOut).IsFalse();
            await Assert.That(shell.SurfaceIndex).IsEqualTo((short)0);
        }

        using var saved = new MemoryStream();
        gbx.Save(saved);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected.ToArray(), CollectionOrdering.Matching);
        saved.Position = 0;
        var reparsed = Gbx.Parse(saved);
        await Assert.That(((CPlugSurface)reparsed.Node!).Surf?.GetType().Name).IsEqualTo(surfType);
    }

    [Test]
    public async Task EveryContributedFixtureHasARoundTripCase()
    {
        var expected = Fixtures().Select(x => $"GmSurfFixture{x.SurfId}{x.Suffix}.Shape.Gbx").OrderBy(x => x);
        var actual = Directory.GetFiles(TestFiles.Gbx("CPlugSurface"), "*.Shape.Gbx")
            .Select(x => Path.GetFileName(x)!).OrderBy(x => x);
        await Assert.That(actual).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(11u)]
    [Arguments(uint.MaxValue)]
    public async Task RejectsMultiSphereCountsAboveNativeLimit(uint count)
    {
        using var archive = new MemoryStream();
        Gbx.Decompress(TestFiles.Gbx("CPlugSurface", "GmSurfFixture9.Shape.Gbx"), archive);
        // Bespoke v6 header (25), C003 header/version (12), surf ID (4).
        archive.Position = 41;
        using (var writer = new BinaryWriter(archive, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(count);
        }
        archive.Position = 0;
        var error = Assert.Throws<InvalidDataException>(() => Gbx.Parse(archive));
        await Assert.That(error.Message).Contains("at most 10");
    }

    [Test]
    [Arguments(0)]
    [Arguments(10)]
    [Arguments(11)]
    public async Task SavesOnlyMultiSphereCountsWithinNativeLimit(int count)
    {
        var gbx = Gbx.Parse<CPlugSurface>(TestFiles.Gbx("CPlugSurface", "GmSurfFixture9.Shape.Gbx"));
        var surf = (await Assert.That(gbx.Node.Surf).IsTypeOf<CPlugSurface.MultiSphere>())!;
        surf.Spheres = Enumerable.Repeat(new CPlugSurface.MultiSphere.LocatedSphere(1, new Vec3(0, 0, 0)), count).ToArray();
        using var saved = new MemoryStream();
        if (count > 10)
        {
            Assert.Throws<InvalidDataException>(() => gbx.Save(saved));
            return;
        }
        gbx.Save(saved);
        saved.Position = 0;
        var reparsed = Gbx.Parse<CPlugSurface>(saved);
        await Assert.That(((CPlugSurface.MultiSphere)reparsed.Node.Surf!).Spheres.Length).IsEqualTo(count);
    }

    [Test]
    public async Task ChangingChunkVersionDoesNotDropFallbackMaterialIds()
    {
        var gbx = Gbx.Parse<CPlugSurface>(TestFiles.Gbx("CPlugSurface", "GmSurfFixture9Primary.Shape.Gbx"));
        var chunk = gbx.Node.Chunks.Get<CPlugSurface.Chunk0900C003>()!;
        chunk.Version = 3;
        chunk.U02 = [0, 1];
        gbx.Node.Materials = [
            new() { SurfaceId = CPlugSurface.MaterialId.Concrete },
            new() { SurfaceId = CPlugSurface.MaterialId.Pavement }
        ];

        using var saved = new MemoryStream();
        gbx.Save(saved);
        saved.Position = 0;
        var reparsed = Gbx.Parse<CPlugSurface>(saved);
        await Assert.That(reparsed.Node.Chunks.Get<CPlugSurface.Chunk0900C003>()!.U02).IsEquivalentTo(new ushort[] { 0, 1 }, CollectionOrdering.Matching);
        await Assert.That(reparsed.Node.Materials.Length).IsEqualTo(2);
    }
}
