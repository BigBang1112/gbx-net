namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0906A000</remarks>
[Node(0x0906A000)]
public abstract class CPlugVisualIndexed : CPlugVisual3D
{
    private CPlugIndexBuffer? indexBuffer;

    [NodeMember]
    public ushort[] Indices
    {
        get => indexBuffer?.Indices ?? Array.Empty<ushort>();
        set
        {
            if (indexBuffer is not null)
            {
                indexBuffer.Indices = value;
            }
        }
    }

    [NodeMember]
    public CPlugIndexBuffer? IndexBuffer { get => indexBuffer; set => indexBuffer = value; }

    protected CPlugVisualIndexed()
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
            var indices = r.ReadArray<ushort>();

            if (n.indexBuffer is not null)
            {
                n.indexBuffer.Indices = indices;
            }
        }

        public override void Write(CPlugVisualIndexed n, GameBoxWriter w)
        {
            if (n.indexBuffer is null)
            {
                w.Write(0);
            }
            else
            {
                w.WriteArray(n.indexBuffer.Indices);
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
        public int Flags;

        public override void Read(CPlugVisualIndexed n, GameBoxReader r)
        {
            U01 = r.ReadBoolean();

            if (U01)
            {
                n.indexBuffer = Parse<CPlugIndexBuffer>(r, 0x9057000, progress: null);
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
