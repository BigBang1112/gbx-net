namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockTriangles3DBuilder
{
    public class TMUF : GameBuilder<CGameCtnMediaBlockTriangles3DBuilder, CGameCtnMediaBlockTriangles3D>
    {
        public IList<CGameCtnMediaBlockTriangles.Key>? Keys { get; set; }
        
        public TMUF(CGameCtnMediaBlockTriangles3DBuilder baseBuilder, CGameCtnMediaBlockTriangles3D node) : base(baseBuilder, node) { }

        public TMUF WithKeys(Func<CGameCtnMediaBlockTriangles3D, IList<CGameCtnMediaBlockTriangles.Key>> keys)
        {
            Keys = keys(Node);
            return this;
        }

        public override CGameCtnMediaBlockTriangles3D Build()
        {
            Node.Keys = Keys ?? new List<CGameCtnMediaBlockTriangles.Key>();
            return Node;
        }
    }
}