using GBX.NET.Engines.Game;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnMediaBlockTrianglesTests
{
    [Test]
    public async Task TrianglesRejectIndicesOutsideTheVertexArray()
    {
        var block = new CGameCtnMediaBlockTriangles { Vertices = new Vec4[3] };
        block.Triangles = [new Int3(0, 1, 2)];

        Assert.Throws<Exception>(() => block.Triangles = [new Int3(0, 1, 3)]);

        await Assert.That(block.Triangles).IsEquivalentTo(new[] { new Int3(0, 1, 2) }, CollectionOrdering.Matching);
    }

    [Test]
    public async Task ShrinkingVerticesResizesKeyPositionsAndRemovesInvalidTriangles()
    {
        var block = new CGameCtnMediaBlockTriangles { Vertices = new Vec4[4] };
        var key = new CGameCtnMediaBlockTriangles.Key(block);
        block.Keys.Add(key);
        block.Triangles = [new Int3(0, 1, 2), new Int3(1, 2, 3)];

        block.Vertices = new Vec4[3];

        await Assert.That(key.Positions.Length).IsEqualTo(3);
        await Assert.That(block.Triangles).IsEquivalentTo(new[] { new Int3(0, 1, 2) }, CollectionOrdering.Matching);
    }

    [Test]
    public async Task ShrinkingVerticesWithoutKeysStillRemovesInvalidTriangles()
    {
        var block = new CGameCtnMediaBlockTriangles { Vertices = new Vec4[4] };
        block.Triangles = [new Int3(0, 1, 2), new Int3(1, 2, 3)];

        block.Vertices = new Vec4[3];

        await Assert.That(block.Triangles).IsEquivalentTo(new[] { new Int3(0, 1, 2) }, CollectionOrdering.Matching);
    }

    [Test]
    public async Task ResizingOneKeysPositionsResizesVerticesAndOtherKeys()
    {
        var block = new CGameCtnMediaBlockTriangles { Vertices = new Vec4[3] };
        var first = new CGameCtnMediaBlockTriangles.Key(block);
        var second = new CGameCtnMediaBlockTriangles.Key(block);
        block.Keys.AddRange([first, second]);

        first.Positions = new Vec3[5];

        await Assert.That(block.Vertices.Length).IsEqualTo(5);
        await Assert.That(first.Positions.Length).IsEqualTo(5);
        await Assert.That(second.Positions.Length).IsEqualTo(5);
    }
}
