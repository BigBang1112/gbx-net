namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0906A000</remarks>
[Node(0x0906A000)]
public abstract class CPlugVisualIndexed : CPlugVisual3D
{
    private int[]? indices;

    private CPlugIndexBuffer? indexBuffer;

    [NodeMember]
    public int[] Indices
    {
        get => indexBuffer?.Indices ?? indices ?? [];
        set
        {
            if (indexBuffer is not null) // not ideal for old games
            {
                indexBuffer.Indices = value;
            }
        }
    }

    [NodeMember]
    public CPlugIndexBuffer? IndexBuffer { get => indexBuffer; set => indexBuffer = value; }

    internal CPlugVisualIndexed()
    {
        
    }

    /// <summary>
    /// CPlugVisualIndexed 0x000 chunk
    /// </summary>
    [Chunk(0x0906A000)]
    public class Chunk0906A000 : Chunk<CPlugVisualIndexed>
    {
        public override void Read(CPlugVisualIndexed n, GameBoxReader r)
        {
            n.indices = new int[r.ReadInt32()];

            for (var i = 0; i < n.indices.Length; i++)
            {
                n.indices[i] = r.ReadUInt16();
            }
        }

        public override void Write(CPlugVisualIndexed n, GameBoxWriter w)
        {
            if (n.indices is null)
            {
                w.Write(0);
                return;
            }

            w.Write(n.indices.Length);

            for (var i = 0; i < n.indices.Length; i++)
            {
                w.Write((ushort)n.indices[i]);
            }
        }
    }

    /// <summary>
    /// CPlugVisualIndexed 0x001 chunk
    /// </summary>
    [Chunk(0x0906A001)]
    public class Chunk0906A001 : Chunk<CPlugVisualIndexed>
    {
        public bool U01;

        public override void Read(CPlugVisualIndexed n, GameBoxReader r)
        {
            U01 = r.ReadBoolean();

            if (U01)
            {
                n.indexBuffer = r.ReadNode<CPlugIndexBuffer>(0x09057000);
            }
        }

        public override void Write(CPlugVisualIndexed n, GameBoxWriter w)
        {
            w.Write(U01);

            if (U01)
            {
                if (n.indexBuffer is null)
                {
                    w.Write(-1);
                    return;
                }
                
                n.indexBuffer.Write(w);
            }
        }
    }
}
