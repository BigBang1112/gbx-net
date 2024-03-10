namespace GBX.NET.Engines.Plug;

public partial class CPlugIndexBuffer
{
    public int Flags { get; set; }
    public int[] Indices { get; set; } = [];

    public partial class Chunk09057000
    {
        public override void Read(CPlugIndexBuffer n, GbxReader r)
        {
            n.Flags = r.ReadInt32();
            n.Indices = new int[r.ReadInt32()];

            for (var i = 0; i < n.Indices.Length; i++)
            {
                n.Indices[i] = r.ReadUInt16();
            }
        }

        public override void Write(CPlugIndexBuffer n, GbxWriter w)
        {
            w.Write(n.Flags);
            w.Write(n.Indices.Length);

            for (var i = 0; i < n.Indices.Length; i++)
            {
                w.Write((ushort)n.Indices[i]);
            }
        }
    }

    public partial class Chunk09057001
    {
        public override void Read(CPlugIndexBuffer n, GbxReader r)
        {
            n.Flags = r.ReadInt32();
            n.Indices = new int[r.ReadInt32()];

            var curVal = 0;

            for (var i = 0; i < n.Indices.Length; i++)
            {
                curVal += r.ReadInt16();
                n.Indices[i] = curVal;
            }
        }

        public override void Write(CPlugIndexBuffer n, GbxWriter w)
        {
            w.Write(n.Flags);
            w.Write(n.Indices.Length);

            var curVal = 0;

            for (var i = 0; i < n.Indices.Length; i++)
            {
                w.Write((short)(n.Indices[i] - curVal));
                curVal = n.Indices[i];
            }
        }
    }
}
