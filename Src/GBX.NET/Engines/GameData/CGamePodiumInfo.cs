namespace GBX.NET.Engines.GameData;

/// <summary>
/// Podium info.
/// </summary>
/// <remarks>ID: 0x2E035000</remarks>
[Node(0x2E035000)]
[NodeExtension("GamePodiumInfo")]
public class CGamePodiumInfo : CMwNod
{
    private ExternalNode<CMwNod>[] mediaClipFids;

    [NodeMember(ExactlyNamed = true)]
    public ExternalNode<CMwNod>[] MediaClipFids { get => mediaClipFids; set => mediaClipFids = value; }

    protected CGamePodiumInfo()
    {
        mediaClipFids = Array.Empty<ExternalNode<CMwNod>>();
    }

    /// <summary>
    /// CGamePodiumInfo 0x000 chunk
    /// </summary>
    [Chunk(0x2E035000)]
    public class Chunk03168000 : Chunk<CGamePodiumInfo>
    {
        public int U01;

        public override void ReadWrite(CGamePodiumInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.ArrayNode<CMwNod>(ref n.mediaClipFids!);
        }
    }
}
