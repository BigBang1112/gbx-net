namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09189000</remarks>
[Node(0x09189000)]
public class CPlugMediaClipList : CMwNod
{
    private ExternalNode<CMwNod>[] mediaClipFids = Array.Empty<ExternalNode<CMwNod>>();

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09189000>]
    public ExternalNode<CMwNod>[] MediaClipFids { get => mediaClipFids; set => mediaClipFids = value; }

    internal CPlugMediaClipList()
    {

    }

    /// <summary>
    /// CPlugMediaClipList 0x000 chunk
    /// </summary>
    [Chunk(0x09189000)]
    public class Chunk09189000 : Chunk<CPlugMediaClipList>
    {
        public int U01;

        public override void ReadWrite(CPlugMediaClipList n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.ArrayNode<CMwNod>(ref n.mediaClipFids!);
        }
    }
}
