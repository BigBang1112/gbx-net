namespace GBX.NET.Engines.Plug;

[Node(0x9057000)]
public class CPlugIndexBuffer : CPlug
{
    private int flags;
    private ushort[]? indices;

    public ushort[]? Indices { get => indices; set => indices = value; }

    protected CPlugIndexBuffer()
    {
        indices = null!;
    }

    [Chunk(0x9057000)]
    public class Chunk09057000 : Chunk<CPlugIndexBuffer>
    {
        public override void ReadWrite(CPlugIndexBuffer n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.flags);
            rw.Array(ref n.indices);
        }
    }
}