namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaBlockTriangles2DBuilder
{
    public class TMUF : GameBuilder<CGameCtnMediaBlockTriangles2DBuilder, CGameCtnMediaBlockTriangles2D>
    {
        public IList<CGameCtnMediaBlockTriangles.Key>? Keys { get; set; }
        
        public TMUF(CGameCtnMediaBlockTriangles2DBuilder baseBuilder, CGameCtnMediaBlockTriangles2D node) : base(baseBuilder, node) { }

        public TMUF WithKeys(Func<CGameCtnMediaBlockTriangles2D, IList<CGameCtnMediaBlockTriangles.Key>> keys)
        {
            Keys = keys(Node);
            return this;
        }

        public override CGameCtnMediaBlockTriangles2D Build()
        {
            Node.Keys = Keys ?? new List<CGameCtnMediaBlockTriangles.Key>();
            return Node;
        }
    }
}