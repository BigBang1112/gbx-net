namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03122000</remarks>
[Node(0x03122000)]
public class CGameCtnBlockInfoMobil : CMwNod
{
    private bool hasGeomTransformation;
    private Vec3? geomTranslation;
    private Vec3? geomRotation;
    private CGameCtnSolidDecals?[]? solidDecals;
    private CGameCtnBlockInfoMobilLink[]? dynaLinks;
    private CPlugSolid? solidFid;
    private GameBoxRefTable.File? solidFidFile;
    private CPlugPrefab? prefabFid;
    private GameBoxRefTable.File? prefabFidFile;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03122003), sinceVersion: 1)]
    public bool HasGeomTransformation { get => hasGeomTransformation; set => hasGeomTransformation = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03122003), sinceVersion: 1)]
    public Vec3? GeomTranslation { get => geomTranslation; set => geomTranslation = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03122003), sinceVersion: 1)]
    public Vec3? GeomRotation { get => geomRotation; set => geomRotation = value; }

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

    internal CGameCtnBlockInfoMobil()
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
        public Vec3? U05;
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
        public int? U17;
        public int? U18;
        public int? U19;
        public byte? U20;
        public int? U21;
        public int? U22;
        public int? U23;
        public int? U24;
        public int? U25;
        public int? U26;
        public CMwNod?[]? U27;
        public int? U28;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoMobil n, GameBoxReaderWriter rw)
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
                rw.Boolean(ref n.hasGeomTransformation);

                if (n.hasGeomTransformation)
                {
                    rw.Vec3(ref n.geomTranslation);
                    rw.Vec3(ref n.geomRotation);
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
                                            rw.Int32(ref U17); // node ref (file likely)
                                            
                                            if (version >= 17)
                                            {
                                                rw.Int32(ref U18); // node ref

                                                if (version >= 18)
                                                {
                                                    rw.Int32(ref U19); // node ref (file likely)

                                                    rw.Byte(ref U20);
                                                    rw.Int32(ref U21);
                                                    rw.Int32(ref U22);
                                                    rw.Int32(ref U23);
                                                    rw.Int32(ref U24);
                                                    rw.Int32(ref U25);
                                                    rw.Int32(ref U26);
                                                    rw.ArrayNode<CMwNod>(ref U27);
                                                    rw.Int32(ref U28);

                                                    if (version >= 24)
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
            rw.ArrayNode<CGameCtnBlockInfoMobilLink>(ref n.dynaLinks!);
        }
    }
}
