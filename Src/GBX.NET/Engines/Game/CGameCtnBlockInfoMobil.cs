namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03122000</remarks>
[Node(0x03122000)]
public class CGameCtnBlockInfoMobil : CMwNod
{
    private CGameCtnSolidDecals?[]? solidDecals;
    private CGameCtnBlockInfoMobilLink[]? dynaLinks;
    private CPlugSolid? solidFid;
    private GameBoxRefTable.File? solidFidFile;
    private CPlugPrefab? prefabFid;
    private GameBoxRefTable.File? prefabFidFile;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03122002))]
    public CGameCtnSolidDecals?[]? SolidDecals { get => solidDecals; set => solidDecals = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03122003), sinceVersion: 2)]
    public CPlugSolid? SolidFid
    {
        get => solidFid = GetNodeFromRefTable(solidFid, solidFidFile) as CPlugSolid;
        set => solidFid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03122003), sinceVersion: 3)]
    public CPlugPrefab? PrefabFid
    {
        get => prefabFid = GetNodeFromRefTable(prefabFid, prefabFidFile) as CPlugPrefab;
        set => prefabFid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03122004))]
    public CGameCtnBlockInfoMobilLink[]? DynaLinks { get => dynaLinks; set => dynaLinks = value; }

    protected CGameCtnBlockInfoMobil()
    {

    }

    /// <summary>
    /// CGameCtnBlockInfoMobil 0x002 chunk
    /// </summary>
    [Chunk(0x03122002)]
    public class Chunk03122002 : Chunk<CGameCtnBlockInfoMobil>
    {
        private int listVersion = 10;

        public int U01;

        public override void ReadWrite(CGameCtnBlockInfoMobil n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref listVersion);
            rw.ArrayNode<CGameCtnSolidDecals>(ref n.solidDecals);
            rw.Int32(ref U01);
        }
    }


    [Chunk(0x03122003)]
    public class Chunk03122003 : Chunk<CGameCtnBlockInfoMobil>, IVersionable
    {
        private int version;

        public bool? U01;
        public CMwNod? U02;
        public int U03;
        public byte? U04;
        public float? U05;
        public float? U06;
        public float? U07;
        public float? U08;
        public float? U09;
        public float? U10;
        public CMwNod? U11;
        public bool U12;
        public CMwNod? U13;
        private GameBoxRefTable.File? U13file;
        public CMwNod? U14;
        public int? U15;
        public int? U16;
        public int[]? U17;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoMobil n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.Int32(ref version);

            if (version < 2)
            {
                rw.Boolean(ref U01);

                throw new ChunkVersionNotSupportedException(version);
            }

            rw.Int32(ref U03);

            if (version >= 1)
            {
                rw.Byte(ref U04);

                if (U04 != 0)
                {
                    rw.Single(ref U05);
                    rw.Single(ref U06);
                    rw.Single(ref U07);
                    rw.Single(ref U08);
                    rw.Single(ref U09);
                    rw.Single(ref U10);
                }

                if (version >= 2)
                {
                    rw.NodeRef<CPlugSolid>(ref n.solidFid, ref n.solidFidFile);

                    if (version >= 14)
                    {
                        rw.NodeRef(ref U14);
                    }

                    if (version >= 3)
                    {
                        rw.NodeRef<CPlugPrefab>(ref n.prefabFid, ref n.prefabFidFile);

                        if (version >= 4)
                        {
                            rw.Boolean(ref U12);

                            if (version >= 6)
                            {
                                rw.NodeRef(ref U13, ref U13file);

                                if (version >= 7)
                                {
                                    rw.Int32(ref U15); // ?? probably noderef

                                    if (version >= 9)
                                    {
                                        rw.Int32(ref U16); // ??

                                        if (version >= 16)
                                        {
                                            throw new ChunkVersionNotSupportedException(version);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    [Chunk(0x03122004)]
    public class Chunk03122004 : Chunk<CGameCtnBlockInfoMobil>, IVersionable
    {
        private int version;
        private int listVersion;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoMobil n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref listVersion);

            rw.Array(ref n.dynaLinks, r =>
            {
                var u01 = r.ReadInt32();
                var u02 = r.ReadInt32();
                var u03 = r.ReadInt32();
                var socketId = r.ReadId();
                var model = r.ReadNodeRef<CGameObjectModel>();

                var u04 = default(int?);

                if (u03 == 0) // May still not be perfect
                {
                    u04 = r.ReadInt32();
                }

                return new CGameCtnBlockInfoMobilLink(socketId, model)
                {
                    U01 = u01,
                    U02 = u02,
                    U03 = u03,
                    U04 = u04
                };
            },
            (x, w) =>
            {
                w.Write(x.U01);
                w.Write(x.U02);
                w.Write(x.U03);
                w.WriteId(x.SocketId);
                w.Write(x.Model);

                if (x.U03 == 0) // Still not perfect
                {
                    w.Write(x.U04.GetValueOrDefault());
                }
            });
        }
    }
}
