namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockTriangles2DBuilder : Builder
{
    public Vec4[] Vertices { get; }
    public Int3[]? Triangles { get; set; }
    
    /// <param name="vertices">Array of vertex colors.</param>
    public CGameCtnMediaBlockTriangles2DBuilder(Vec4[] vertices)
    {
        Vertices = vertices;
    }

    public CGameCtnMediaBlockTriangles2DBuilder WithTriangles(Int3[] triangles)
    {
        Triangles = triangles;
        return this;
    }

    public TMUF ForTMUF() => new(this, NewNode());

    internal CGameCtnMediaBlockTriangles2D NewNode()
    {
        var node = new CGameCtnMediaBlockTriangles2D
        {
            Vertices = Vertices,
            Triangles = Triangles ?? Array.Empty<Int3>()
        };
        
        node.CreateChunk<CGameCtnMediaBlockTriangles.Chunk03029001>();
        
        return node;
    }
}