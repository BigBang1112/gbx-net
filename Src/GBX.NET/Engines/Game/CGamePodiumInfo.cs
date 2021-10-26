using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Game
{
    [Node(0x03168000)]
    public sealed class CGamePodiumInfo : CMwNod
    {
        private int[] mediaClipFids;

        public int[] MediaClipFids
        {
            get => mediaClipFids;
            set => mediaClipFids = value;
        }

        private CGamePodiumInfo()
        {
            mediaClipFids = null!;
        }

        [Chunk(0x03168000)]
        public class Chunk03168000 : Chunk<CGamePodiumInfo>
        {
            public int U01;

            public override void ReadWrite(CGamePodiumInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref U01);
                rw.Array(ref n.mediaClipFids!);
            }
        }
    }
}
