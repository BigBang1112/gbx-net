namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaClipBuilder
{
    public class TMUF : GameBuilder<CGameCtnMediaClipBuilder, CGameCtnMediaClip>
    {
        public int? LocalPlayerClipEntIndex { get; set; }

        public TMUF(CGameCtnMediaClipBuilder baseBuilder, CGameCtnMediaClip node) : base(baseBuilder, node) { }

        public TMUF WithLocalPlayerClipEntIndex(int localPlayerClipEntIndex)
        {
            LocalPlayerClipEntIndex = localPlayerClipEntIndex;
            return this;
        }

        public override CGameCtnMediaClip Build()
        {
            Node.LocalPlayerClipEntIndex = LocalPlayerClipEntIndex ?? -1;
            Node.CreateChunk<CGameCtnMediaClip.Chunk03079004>();
            Node.CreateChunk<CGameCtnMediaClip.Chunk03079005>();
            Node.CreateChunk<CGameCtnMediaClip.Chunk03079007>();
            return Node;
        }
    }
}