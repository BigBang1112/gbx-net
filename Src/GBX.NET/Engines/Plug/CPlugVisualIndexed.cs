
namespace GBX.NET.Engines.Plug;

public partial class CPlugVisualIndexed
{
    public CPlugIndexBuffer? IndexBuffer { get; set; }

    public partial class Chunk0906A000
    {
        public override void Read(CPlugVisualIndexed n, GbxReader r)
        {
            var indices = new int[r.ReadInt32()];

            for (var i = 0; i < indices.Length; i++)
            {
                indices[i] = r.ReadUInt16();
            }

            n.IndexBuffer = new CPlugIndexBuffer { Indices = indices };
        }

        public override void Write(CPlugVisualIndexed n, GbxWriter w)
        {
            if (n.IndexBuffer is null)
            {
                w.Write(0);
                return;
            }

            w.Write(n.IndexBuffer.Indices.Length);

            for (var i = 0; i < n.IndexBuffer.Indices.Length; i++)
            {
                w.Write((ushort)n.IndexBuffer.Indices[i]);
            }
        }
    }

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
