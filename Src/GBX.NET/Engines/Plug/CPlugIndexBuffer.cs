namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09057000</remarks>
[Node(0x09057000)]
public class CPlugIndexBuffer : CPlug
{
    private int flags;
    private ushort[] indices;

    [NodeMember]
    [AppliedWithChunk<Chunk09057000>]
    public int Flags { get => flags; set => flags = value; }
    
    [NodeMember]
    [AppliedWithChunk<Chunk09057000>]
    public ushort[] Indices { get => indices; set => indices = value; }

    internal CPlugIndexBuffer()
    {
        indices = Array.Empty<ushort>();
    }

    /// <summary>
    /// CPlugIndexBuffer 0x000 chunk
    /// </summary>
    [Chunk(0x09057000)]
    public class Chunk09057000 : Chunk<CPlugIndexBuffer>
    {
        public override void ReadWrite(CPlugIndexBuffer n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.flags);
            rw.Array(ref n.indices!);
        }
    }

    #region 0x001 chunk

    /// <summary>
    /// CPlugIndexBuffer 0x001 chunk
    /// </summary>
    [Chunk(0x09057001)]
    public class Chunk09057001 : Chunk<CPlugIndexBuffer>
    {
        public override void ReadWrite(CPlugIndexBuffer n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.flags);
            rw.Array(ref n.indices!);
        }
    }

    #endregion
}