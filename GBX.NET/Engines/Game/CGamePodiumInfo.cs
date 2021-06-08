using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Game
{
    [Node(0x03168000)]
    public class CGamePodiumInfo : CMwNod
    {
        public int[] MediaClipFids { get; set; }

        [Chunk(0x03168000)]
        public class Chunk03168000 : Chunk<CGamePodiumInfo>
        {
            public override void ReadWrite(CGamePodiumInfo n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                n.MediaClipFids = rw.Array(n.MediaClipFids);
            }
        }
    }
}
