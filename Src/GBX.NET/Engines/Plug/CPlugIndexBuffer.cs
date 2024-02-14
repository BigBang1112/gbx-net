namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09057000</remarks>
[Node(0x09057000)]
public class CPlugIndexBuffer : CPlug
{
    private int flags;
    private int[] indices;

    [NodeMember]
    [AppliedWithChunk<Chunk09057000>]
    public int Flags { get => flags; set => flags = value; }
    
    [NodeMember]
    [AppliedWithChunk<Chunk09057000>]
    public int[] Indices { get => indices; set => indices = value; }

    internal CPlugIndexBuffer()
    {
        indices = [];
    }

    /// <summary>
    /// CPlugIndexBuffer 0x000 chunk
    /// </summary>
    [Chunk(0x09057000)]
    public class Chunk09057000 : Chunk<CPlugIndexBuffer>
    {
        public override void Read(CPlugIndexBuffer n, GameBoxReader r)
        {
            n.flags = r.ReadInt32();
            n.indices = new int[r.ReadInt32()];

            for (var i = 0; i < n.indices.Length; i++)
            {
                n.indices[i] = r.ReadUInt16();
            }
        }

        public override void Write(CPlugIndexBuffer n, GameBoxWriter w)
        {
            w.Write(n.flags);
            w.Write(n.indices.Length);

            for (var i = 0; i < n.indices.Length; i++)
            {
                w.Write((ushort)n.indices[i]);
            }
        }
    }

    #region 0x001 chunk

    /// <summary>
    /// CPlugIndexBuffer 0x001 chunk
    /// </summary>
    [Chunk(0x09057001)]
    public class Chunk09057001 : Chunk<CPlugIndexBuffer>
    {
        public override void Read(CPlugIndexBuffer n, GameBoxReader r)
        {
            n.flags = r.ReadInt32();
            n.indices = new int[r.ReadInt32()];

            var curVal = 0;

            for (var i = 0; i < n.indices.Length; i++)
            {
                curVal += r.ReadInt16();
                n.indices[i] = curVal;
            }
        }

        public override void Write(CPlugIndexBuffer n, GameBoxWriter w)
        {
            w.Write(n.flags);
            w.Write(n.indices.Length);

            var curVal = 0;

            for (var i = 0; i < n.indices.Length; i++)
            {
                w.Write((short)(n.indices[i] - curVal));
                curVal = n.indices[i];
            }
        }
    }

    #endregion
}