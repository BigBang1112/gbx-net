namespace GBX.NET.Engines.Plug;

public partial class CPlugGameSkin
{
    public partial class Fid : IReadableWritable
    {
        private uint classId;
        private string name = "";
        private CMwNod? node;
        private string directory = "";
        private bool u01;

        public uint ClassId { get => classId; set => classId = value; }
        public string Name { get => name; set => name = value; }

        public CMwNod? Node { get => nodeFile?.GetNode(ref node) ?? node; set => node = value; }
        private Components.GbxRefTableFile? nodeFile;
        public Components.GbxRefTableFile? NodeFile { get => nodeFile; set => nodeFile = value; }
        public CMwNod? GetNode(GbxReadSettings settings = default) => nodeFile?.GetNode(ref node, settings) ?? node;

        public string Directory { get => directory; set => directory = value; }
        public bool U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GbxReaderWriter rw, int version = 0)
        {
            rw.UInt32(ref classId);
            rw.String(ref name!);

            if (version >= 3)
            {
                if (rw.Boolean(node is not null))
                {
                    rw.NodeRef(ref node, ref nodeFile);
                }
            }
            else
            {
                rw.String(ref directory!);
            }

            if (version >= 2)
            {
                rw.Boolean(ref u01);
            }
        }

        public override string ToString()
        {
            return $"[{ClassId:X8}] {Name} ({Node})";
        }
    }
}
