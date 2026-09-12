using GBX.NET.Engines.Plug;
using GBX.NET.LZO;

namespace GBX.NET.Tests.Integration;

public class GmSurfTests
{
    public static TheoryData<int, string?, string> Fixtures => new()
    {
        { -1, null, "" },
        { 0, "Sphere", "" },
        { 1, "Ellipsoid", "" },
        { 6, "Box", "" },
        { 7, "Mesh", "" },
        { 8, "VCylinder", "" },
        { 9, "MultiSphere", "" },
        { 10, "ConvexPolyhedron", "" },
        { 11, "Capsule", "" },
        { 12, "Circle", "" },
        { 13, "Compound", "" },
        { 14, "SphereLocated", "" },
        { 15, "CompoundInstance", "" },
        { 16, "Cylinder", "" },
        { 17, "SphericalShell", "" },
        { 18, "Voxel", "" },
        { 19, "Diggable", "" },
        { 10, "ConvexPolyhedron", "Procedural" },
        { 8, "VCylinder", "Version0" },
        { 14, "SphereLocated", "Version0" },
        { 16, "Cylinder", "Version0" },
        { 9, "MultiSphere", "Primary" },
        { 9, "MultiSphere", "Fallback" }
    };

    [Theory]
    [MemberData(nameof(Fixtures))]
    public void NativeFixtureRoundTripsWithoutChangingArchive(int surfId, string? surfType, string suffix = "")
    {
        Gbx.LZO = new Lzo();
        var path = Path.Combine("Files", "Gbx", "CPlugSurface", $"GmSurfFixture{surfId}{suffix}.Shape.Gbx");
        using var expected = new MemoryStream();
        Gbx.Decompress(path, expected);
        expected.Position = 0;

        var gbx = Gbx.Parse(path);
        gbx.BodyCompression = GbxCompression.Uncompressed;
        var surface = Assert.IsType<CPlugSurface>(gbx.Node);
        Assert.Equal(surfType, surface.Surf?.GetType().Name);
        Assert.Equal(suffix == "Version0" ? 0 : 2, surface.SurfVersion);
        if (surface.Surf is not null)
        {
            Assert.Equal(suffix == "Version0" ? (Vec3?)null : new Vec3(0, 0, 1), surface.Surf.GameplayMainDir);
        }
        if (surface.Surf is CPlugSurface.MultiSphere multiSphere)
        {
            var sphereEntry = Assert.Single(multiSphere.Spheres);
            Assert.Equal(1f, sphereEntry.Radius);
            Assert.Equal(new Vec3(0, 0, 0), sphereEntry.Center);
            Assert.Equal(0, multiSphere.SurfaceIndex);
        }
        if (suffix == "Primary")
        {
            Assert.Equal(new ushort[] { 0x0100, 0x0201 }, surface.Chunks.Get<CPlugSurface.Chunk0900C003>()!.U04);
        }
        if (suffix == "Fallback")
        {
            Assert.Equal(2, surface.Materials.Length);
            Assert.Equal(new ushort[] { 0, 1 }, surface.Chunks.Get<CPlugSurface.Chunk0900C003>()!.U02);
        }
        if (surface.Surf is CPlugSurface.ConvexPolyhedron polyhedron && suffix != "Procedural")
        {
            Assert.Equal(0, polyhedron.Version);
            Assert.Equal(0, polyhedron.Representation);
            Assert.Equal(new Vec3[] { new(0, 0, 0), new(1, 0, 0), new(0, 1, 0), new(0, 0, 1) }, polyhedron.Vertices);
            Assert.Equal(new uint[] { 0, 2, 1, 0, 1, 3, 0, 3, 2, 1, 2, 3 }, polyhedron.Indices);
            Assert.Equal(4, polyhedron.Faces.Length);
            Assert.Equal(new CPlugSurface.ConvexPolyhedron.Face(9, 3), polyhedron.Faces[3]);
            Assert.Equal(0, polyhedron.SurfaceIndex);
        }
        if (surface.Surf is CPlugSurface.ConvexPolyhedron procedural && suffix == "Procedural")
        {
            Assert.Equal(1, procedural.Representation);
            Assert.Equal(new Vec3(1, .5f, 2), procedural.Parameters);
        }
        if (surface.Surf is CPlugSurface.SphereLocated sphere)
        {
            Assert.Equal(new Vec3(0, 0, 0), sphere.Center);
            Assert.Equal(1f, sphere.Radius);
            Assert.Equal(0, sphere.SurfaceIndex);
        }
        if (surface.Surf is CPlugSurface.Capsule capsule)
        {
            Assert.Equal(new Vec3(0, 0, 0), capsule.SphereCenter);
            Assert.Equal(new Vec3(0, 1, 0), capsule.Dir);
            Assert.Equal(1f, capsule.Length);
            Assert.Equal(0, capsule.SurfaceIndex);
        }
        if (surface.Surf is CPlugSurface.SphericalShell shell)
        {
            Assert.Equal(.5f, shell.InnerRadius);
            Assert.Equal(1f, shell.OuterRadius);
            Assert.False(shell.SkipInToOut);
            Assert.Equal(0, shell.SurfaceIndex);
        }

        using var saved = new MemoryStream();
        gbx.Save(saved);
        Assert.Equal(expected.ToArray(), saved.ToArray());
        saved.Position = 0;
        var reparsed = Gbx.Parse(saved);
        Assert.Equal(surfType, Assert.IsType<CPlugSurface>(reparsed.Node).Surf?.GetType().Name);
    }

    [Fact]
    public void EveryContributedFixtureHasARoundTripCase()
    {
        var expected = Fixtures.Select(x => $"GmSurfFixture{x[0]}{x[2]}.Shape.Gbx").OrderBy(x => x);
        var actual = Directory.GetFiles(Path.Combine("Files", "Gbx", "CPlugSurface"), "*.Shape.Gbx")
            .Select(Path.GetFileName).OrderBy(x => x);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(11u)]
    [InlineData(uint.MaxValue)]
    public void RejectsMultiSphereCountsAboveNativeLimit(uint count)
    {
        Gbx.LZO = new Lzo();
        using var archive = new MemoryStream();
        Gbx.Decompress(Path.Combine("Files", "Gbx", "CPlugSurface", "GmSurfFixture9.Shape.Gbx"), archive);
        // Bespoke v6 header (25), C003 header/version (12), surf ID (4).
        archive.Position = 41;
        using (var writer = new BinaryWriter(archive, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(count);
        }
        archive.Position = 0;
        var error = Assert.Throws<InvalidDataException>(() => Gbx.Parse(archive));
        Assert.Contains("at most 10", error.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(11)]
    public void SavesOnlyMultiSphereCountsWithinNativeLimit(int count)
    {
        Gbx.LZO = new Lzo();
        var gbx = Gbx.Parse<CPlugSurface>(Path.Combine("Files", "Gbx", "CPlugSurface", "GmSurfFixture9.Shape.Gbx"));
        var surf = Assert.IsType<CPlugSurface.MultiSphere>(gbx.Node.Surf);
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
        Assert.Equal(count, Assert.IsType<CPlugSurface.MultiSphere>(reparsed.Node.Surf).Spheres.Length);
    }

    [Fact]
    public void ChangingChunkVersionDoesNotDropFallbackMaterialIds()
    {
        Gbx.LZO = new Lzo();
        var gbx = Gbx.Parse<CPlugSurface>(Path.Combine("Files", "Gbx", "CPlugSurface", "GmSurfFixture9Primary.Shape.Gbx"));
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
        Assert.Equal(new ushort[] { 0, 1 }, reparsed.Node.Chunks.Get<CPlugSurface.Chunk0900C003>()!.U02);
        Assert.Equal(2, reparsed.Node.Materials.Length);
    }
}
