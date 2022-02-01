namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaClipBuilder
{
    public class TMSX : GameBuilder<CGameCtnMediaClipBuilder, CGameCtnMediaClip>
    {
        public TMSX(CGameCtnMediaClipBuilder baseBuilder, CGameCtnMediaClip node) : base(baseBuilder, node) { }

        public override CGameCtnMediaClip Build()
        {
            Node.CreateChunk<CGameCtnMediaClip.Chunk03079003>();
            return Node;
        }
    }
}