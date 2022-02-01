namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaClipBuilder
{
    public class TM2020 : GameBuilder<CGameCtnMediaClipBuilder, CGameCtnMediaClip>
    {
        public int? LocalPlayerClipEntIndex { get; set; }
        public bool StopWhenRespawn { get; set; }
        public bool StopWhenLeave { get; set; }

        public TM2020(CGameCtnMediaClipBuilder baseBuilder, CGameCtnMediaClip node) : base(baseBuilder, node) { }

        public TM2020 WithLocalPlayerClipEntIndex(int localPlayerClipEntIndex)
        {
            LocalPlayerClipEntIndex = localPlayerClipEntIndex;
            return this;
        }

        public TM2020 StopsWhenRespawn()
        {
            StopWhenRespawn = true;
            return this;
        }

        public TM2020 StopsWhenLeave()
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