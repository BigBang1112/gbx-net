namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F4000</remarks>
[Node(0x090F4000)]
public class CPlugGameSkin : CMwNod
{
    private string? relativeSkinDirectory;
    private string? parentPackDesc;
    private HeaderFid[]? headerFids;
    private Fid[]? fids;

    internal CPlugGameSkin()
    {

    }

    [NodeMember]
    [AppliedWithChunk<Chunk090F4000>]
    [AppliedWithChunk<Chunk090F4004>]
    public string? RelativeSkinDirectory { get => relativeSkinDirectory; set => relativeSkinDirectory = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090F4000>(sinceVersion: 1)]
    [AppliedWithChunk<Chunk090F4004>(sinceVersion: 1)]
    public string? ParentPackDesc { get => parentPackDesc; set => parentPackDesc = value; }
    
    [NodeMember]
    [AppliedWithChunk<Chunk090F4000>]
    public HeaderFid[]? HeaderFids { get => headerFids; set => headerFids = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090F4004>]
    public Fid[]? Fids { get => fids; set => fids = value; }

    /// <summary>
    /// CPlugGameSkin 0x000 header chunk
    /// </summary>
    [Chunk(0x090F4000)]
    public class Chunk090F4000 : HeaderChunk<CPlugGameSkin>, IVersionable
    {
        private int version;
        private CPlugGameSkin? n;

        public string? RelativeSkinDirectory;
        public string? ParentPackDesc;
        public HeaderFid[]? HeaderFids;
        public string? U03;
        public string? U04;
        public bool U05;
        public string? U06;
        public int U07 = 1;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugGameSkin n, GameBoxReaderWriter rw)
        {
            this.n = n;
            ReadWrite(rw);
        }

        public override void ReadWrite(GameBoxReaderWriter rw)
        {
            rw.Byte(ref version);

            if (n is null)
            {
                rw.String(ref RelativeSkinDirectory);
            }
            else
            {
                rw.String(ref n.relativeSkinDirectory);
            }

            if (version >= 1)
            {
                if (n is null)
                {
                    rw.String(ref ParentPackDesc); // weird idk
                }
                else
                {
                    rw.String(ref n.parentPackDesc); // weird idk
                }

                rw.String(ref U03);
            }

            var fidCount = n is null
                ? rw.Byte(HeaderFids?.Length ?? 0)
                : rw.Byte(n.headerFids?.Length ?? 0);

            if (n is null)
            {
                rw.ArrayArchive<HeaderFid>(ref HeaderFids, version, fidCount);
            }
            else
            {
                rw.ArrayArchive<HeaderFid>(ref n.headerFids, version, fidCount);
            }

            if (version >= 4)
            {
                rw.String(ref U04); // packdesc?

                if (version >= 5)
                {
                    rw.Boolean(ref U05); // something dialog?

                    if (version >= 6)
                    {
                        rw.String(ref U06);

                        if (version >= 7)
                        {
                            rw.Int32(ref U07);
                        }
                    }
                }
            }
        }
    }

    #region 0x003 chunk

    /// <summary>
    /// CPlugGameSkin 0x003 chunk
    /// </summary>
    [Chunk(0x090F4003)]
    public class Chunk090F4003 : Chunk<CPlugGameSkin>
    {
        public string? U01;
        public string? U02;

        public override void ReadWrite(CPlugGameSkin n, GameBoxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.String(ref U02);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CPlugGameSkin 0x004 chunk
    /// </summary>
    [Chunk(0x090F4004)]
    public class Chunk090F4004 : Chunk<CPlugGameSkin>
    {
        private int version;

        public string? U03;
        public Fid[]? Fids;
        public string? U04;
        public bool U05;
        public string? U06;
        public int U07 = 1;

        public int Version { get => version; set => version = value; }
        
        public override void ReadWrite(CPlugGameSkin n, GameBoxReaderWriter rw)
        {
            rw.Byte(ref version);
            rw.String(ref n.relativeSkinDirectory);

            if (version >= 1)
            {
                rw.String(ref n.parentPackDesc); // weird idk
                rw.String(ref U03);
            }

            var fidCount = rw.Byte(n.fids?.Length ?? 0);

            rw.ArrayArchive<Fid>(ref n.fids, version, fidCount);

            if (version >= 4)
            {
                rw.String(ref U04); // packdesc?

                if (version >= 5)
                {
                    rw.Boolean(ref U05); // something dialog?

                    if (version >= 6)
                    {
                        rw.String(ref U06);

                        if (version >= 7)
                        {
                            rw.Int32(ref U07);
                        }
                    }
                }
            }
        }
    }

    #endregion

    public class HeaderFid : IReadableWritable
    {
        private uint classId;
        private string name = "";
        private string directory = "";
        private bool u01;

        public uint ClassId { get => classId; set => classId = value; }
        public string Name { get => name; set => name = value; }
        public string Directory { get => directory; set => directory = value; }
        public bool U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.UInt32(ref classId);
            rw.String(ref name!);
            rw.String(ref directory!);

            if (version >= 2)
            {
                rw.Boolean(ref u01);
            }
        }

        public override string ToString()
        {
            return $"[{ClassId:X8}] {Name} ({Directory})";
        }
    }

    public class Fid : IReadableWritable
    {
        private uint classId;
        private string name = "";
        private CMwNod? node;
        private string directory = "";
        private bool u01;

        public uint ClassId { get => classId; set => classId = value; }
        public string Name { get => name; set => name = value; }
        public CMwNod? Node { get => node; set => node = value; }
        public string Directory { get => directory; set => directory = value; }
        public bool U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.UInt32(ref classId);
            rw.String(ref name!);

            if (version >= 3)
            {
                if (rw.Boolean(node is not null))
                {
                    rw.NodeRef(ref node!);
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
