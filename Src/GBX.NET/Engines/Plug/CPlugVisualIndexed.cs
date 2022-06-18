namespace GBX.NET.Engines.Plug;

/// <summary>
/// Indexed visual (0x0906A000)
/// </summary>
/// <remarks>Handles indicies of a 3D mesh.</remarks>
[Node(0x0906A000)]
public abstract class CPlugVisualIndexed : CPlugVisual3D
{
    private CPlugIndexBuffer? indexBuffer;

    public ushort[]? Indices
    {
        get => indexBuffer?.Indices;
        set
        {
            if (indexBuffer is not null)
            {
                indexBuffer.Indices = value;
            }
        }
    }

    protected CPlugVisualIndexed()
    {
        
    }

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

    [Chunk(0x0906A001)]
    public class Chunk0906A001 : Chunk<CPlugVisualIndexed>
    {
        public bool U01;
        public int Flags;

        public override void Read(CPlugVisualIndexed n, GameBoxReader r, ILogger? logger)
        {
            U01 = r.ReadBoolean();

            if (U01)
            {
                n.indexBuffer = Parse<CPlugIndexBuffer>(r, 0x9057000, progress: null, logger);
            }
        }

        public override void Write(CPlugVisualIndexed n, GameBoxWriter w, ILogger? logger)
        {
            w.Write(U01);

            if (U01)
            {
                if (n.indexBuffer is null)
                {
                    w.Write(-1);
                    return;
                }

                n.indexBuffer.Write(w, logger);
            }
        }
    }
}
