namespace GBX.NET.Engines.MwFoundations;

[Node(0x01026000)]
public class CMwRefBuffer : CMwNod
{
    private uint nodClassId;
    private bool useAddRefRelease;
    private CMwNod[] nods = Array.Empty<CMwNod>();
    private string? bufferId;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk01026000>]
    public uint NodClassId { get => nodClassId; set => nodClassId = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk01026000>]
    public bool UseAddRefRelease { get => useAddRefRelease; set => useAddRefRelease = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk01026000>]
    public CMwNod[] Nods { get => nods; set => nods = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk01026001>]
    public string? BufferId { get => bufferId; set => bufferId = value; }

    internal CMwRefBuffer()
	{

	}

    #region 0x000 chunk

    /// <summary>
    /// CMwRefBuffer 0x000 chunk
    /// </summary>
    [Chunk(0x01026000)]
    public class Chunk01026000 : Chunk<CMwRefBuffer>
    {
        public override void ReadWrite(CMwRefBuffer n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref n.nodClassId);
            rw.Boolean(ref n.useAddRefRelease);
            rw.Int32(10);
            rw.ArrayNode<CMwNod>(ref n.nods!);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CMwRefBuffer 0x001 chunk
    /// </summary>
    [Chunk(0x01026001)]
    public class Chunk01026001 : Chunk<CMwRefBuffer>
    {
        public override void ReadWrite(CMwRefBuffer n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.bufferId);
        }
    }

    #endregion
}
