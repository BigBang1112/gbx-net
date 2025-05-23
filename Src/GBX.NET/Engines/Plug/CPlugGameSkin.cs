namespace GBX.NET.Engines.Plug;

public partial class CPlugGameSkin
{
    [AppliedWithChunk<Chunk090F4001>]
    public Fid[]? CustomizableFids2 { get; set; }

    public partial class Chunk090F4000
    {
        public override void Read(CPlugGameSkin n, GbxReader r)
        {
            n.CustomizableFids = new Fid[r.ReadInt32()];
            r.ReadInt32(); // CustomizableNames count thats not used in loop
            for (var i = 0; i < n.CustomizableFids.Length; i++)
            {
                n.CustomizableFids[i] = new Fid
                {
                    Node = r.ReadNodeRef<CMwNod>(out var nodeFile),
                    Name = r.ReadString(),
                    NodeFile = nodeFile,
                };
            }
        }

        public override void Write(CPlugGameSkin n, GbxWriter w)
        {
            var count = n.CustomizableFids?.Length ?? 0;
            w.Write(count);
            w.Write(count); // CustomizableNames count that is not used in loop

            if (n.CustomizableFids is null)
            {
                return;
            }

            for (var i = 0; i < count; i++)
            {
                var fid = n.CustomizableFids[i];
                w.WriteNodeRef(fid.Node, fid.NodeFile);
                w.Write(fid.Name);
            }
        }
    }

    public partial class Chunk090F4001
    {
        public override void Read(CPlugGameSkin n, GbxReader r)
        {
            n.CustomizableFids2 = new Fid[r.ReadInt32()];
            r.ReadInt32(); // CustomizableNames count thats not used in loop
            for (var i = 0; i < n.CustomizableFids2.Length; i++)
            {
                n.CustomizableFids2[i] = new Fid
                {
                    Node = r.ReadNodeRef<CMwNod>(out var nodeFile),
                    Name = r.ReadString(),
                    NodeFile = nodeFile,
                };
            }
        }

        public override void Write(CPlugGameSkin n, GbxWriter w)
        {
            var count = n.CustomizableFids2?.Length ?? 0;
            w.Write(count);
            w.Write(count); // CustomizableNames count that is not used in loop

            if (n.CustomizableFids2 is null)
            {
                return;
            }

            for (var i = 0; i < count; i++)
            {
                var fid = n.CustomizableFids2[i];
                w.WriteNodeRef(fid.Node, fid.NodeFile);
                w.Write(fid.Name);
            }
        }
    }

    public partial class Fid : IReadableWritable
    {
        private uint classId;
        private string name = "";
        private CMwNod? node;
        private string directory = "";
        private bool mipMaps;

        public uint ClassId { get => classId; set => classId = value; }
        public string Name { get => name; set => name = value; }

        public CMwNod? Node { get => nodeFile?.GetNode(ref node) ?? node; set => node = value; }
        private Components.GbxRefTableFile? nodeFile;
        public Components.GbxRefTableFile? NodeFile { get => nodeFile; set => nodeFile = value; }
        public CMwNod? GetNode(GbxReadSettings settings = default, bool exceptions = false) => nodeFile?.GetNode(ref node, settings, exceptions) ?? node;

        public string Directory { get => directory; set => directory = value; }
        public bool MipMaps { get => mipMaps; set => mipMaps = value; }

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
                rw.Boolean(ref mipMaps);
            }
        }

        public override string ToString()
        {
            return $"[0x{ClassId:X8}] {Name} ({Node?.ToString() ?? Directory})";
        }
    }

    public partial class HeaderFid
    {
        public override string ToString()
        {
            return $"[0x{ClassId:X8}] {Name} ({Directory})";
        }
    }
}
