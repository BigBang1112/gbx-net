using GBX.NET.Builders.Engines.Game;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - 2D triangles.
/// </summary>
/// <remarks>ID: 0x0304B000</remarks>
[Node(0x0304B000)]
[NodeExtension("GameCtnMediaBlockTriangles2D")]
public class CGameCtnMediaBlockTriangles2D : CGameCtnMediaBlockTriangles
{
    internal CGameCtnMediaBlockTriangles2D()
    {

    }

    /// <param name="vertices">Array of vertex colors.</param>
    public static CGameCtnMediaBlockTriangles2DBuilder Create(Vec4[] vertices) => new(vertices);
}
