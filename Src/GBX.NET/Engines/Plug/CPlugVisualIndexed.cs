
namespace GBX.NET.Engines.Plug;

public partial class CPlugVisualIndexed
{
    public CPlugIndexBuffer? IndexBuffer { get; set; }

    public partial class Chunk0906A001
    {
        public override void Read(CPlugVisualIndexed n, GbxReader r)
        {
            if (r.ReadBoolean())
            {
                n.IndexBuffer = r.ReadNode<CPlugIndexBuffer>();
            }
        }

        public override void Write(CPlugVisualIndexed n, GbxWriter w)
        {
            w.Write(n.IndexBuffer is not null);
            w.WriteNode(n.IndexBuffer);
        }
    }
}
