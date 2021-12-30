namespace GBX.NET.Engines.Game;

/// <summary>
/// Podium info (0x03168000)
/// </summary>
[Node(0x03168000)]
public class CGamePodiumInfo : CMwNod
{
    private int[] mediaClipFids;

    public int[] MediaClipFids
    {
        get => mediaClipFids;
        set => mediaClipFids = value;
    }

    protected CGamePodiumInfo()
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
