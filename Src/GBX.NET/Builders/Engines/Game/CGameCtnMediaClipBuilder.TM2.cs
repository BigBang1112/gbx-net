namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaClipBuilder
{
    public class TM2 : GameBuilder<CGameCtnMediaClipBuilder, CGameCtnMediaClip>
    {
        public int? LocalPlayerClipEntIndex { get; set; }
        public bool StopWhenRespawn { get; set; }
        public bool StopWhenLeave { get; set; }

        public TM2(CGameCtnMediaClipBuilder baseBuilder, CGameCtnMediaClip node) : base(baseBuilder, node) { }

        public TM2 WithLocalPlayerClipEntIndex(int localPlayerClipEntIndex)
        {
            LocalPlayerClipEntIndex = localPlayerClipEntIndex;
            return this;
        }

        public TM2 StopsWhenRespawn()
        {
            StopWhenRespawn = true;
            return this;
        }

        public TM2 StopsWhenLeave()
        {
            StopWhenLeave = true;
            return this;
        }

        public override CGameCtnMediaClip Build()
        {
            Node.LocalPlayerClipEntIndex = LocalPlayerClipEntIndex ?? -1;
            Node.StopWhenRespawn = StopWhenRespawn;
            Node.StopWhenLeave = StopWhenLeave;
            Node.CreateChunk<CGameCtnMediaClip.Chunk0307900D>();
            return Node;
        }
    }
}