namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaClipBuilder
{
    public class TMSX : GameBuilder<ICGameCtnMediaClipBuilder, CGameCtnMediaClip>
    {
        public TMSX(ICGameCtnMediaClipBuilder baseBuilder, CGameCtnMediaClip node) : base(baseBuilder, node) { }

        public override CGameCtnMediaClip Build()
        {
            Node.CreateChunk<CGameCtnMediaClip.Chunk03079003>();
            return Node;
        }
    }
}