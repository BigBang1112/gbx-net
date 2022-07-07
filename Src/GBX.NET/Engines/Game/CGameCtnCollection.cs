namespace GBX.NET.Engines.Game;

[Node(0x03033000)]
[NodeExtension("TMElementColl")]
[NodeExtension("TMCollection")]
[NodeExtension("Collection")]
public class CGameCtnCollection : CMwNod, INodeHeader
{
    private string? collection;
    private bool needUnlock;
    private string? iconEnv;
    private string? iconCollection;
    private int sortIndex;
    private string? defaultZone;
    private Ident? vehicle;
    private string? mapFid;
    private Vec2? mapCoordElem;
    private Vec2? mapCoordIcon;
    private string? loadScreen;
    private Vec2? mapCoordDesc;
    private string? longDesc;
    private string? displayName;
    private bool? isEditable;
    private float squareSize;
    private float squareHeight;
    private float cameraMinHeight;
    private float shadowSoftSizeInWorld;
    private float colorVertexMin;
    private float colorVertexMax;
    private CPlugBitmap? iconFid;
    private int? iconFidIndex;
    private CPlugBitmap? loadScreenFid;
    private int? loadScreenFidIndex;
    private CGameCtnDecoration? defaultDecoration;
    private int? defaultDecorationIndex;
    private CGameCtnZone?[]? completeZoneList;

    public ChunkSet HeaderChunks { get; } = new();

    [NodeMember]
    public string? Collection { get => collection; set => collection = value; }

    [NodeMember]
    public bool NeedUnlock { get => needUnlock; set => needUnlock = value; }

    [NodeMember]
    public string? IconEnv { get => iconEnv; set => iconEnv = value; }

    [NodeMember]
    public string? IconCollection { get => iconCollection; set => iconCollection = value; }

    [NodeMember(ExactlyNamed = true)]
    public int SortIndex { get => sortIndex; set => sortIndex = value; }

    [NodeMember]
    public string? DefaultZone { get => defaultZone; set => defaultZone = value; }

    [NodeMember]
    public Ident? Vehicle { get => vehicle; set => vehicle = value; }

    [NodeMember]
    public string? MapFid { get => mapFid; set => mapFid = value; }

    [NodeMember]
    public Vec2? MapCoordElem { get => mapCoordElem; set => mapCoordElem = value; }

    [NodeMember]
    public Vec2? MapCoordIcon { get => mapCoordIcon; set => mapCoordIcon = value; }

    [NodeMember]
    public string? LoadScreen { get => loadScreen; set => loadScreen = value; }

    [NodeMember]
    public Vec2? MapCoordDesc { get => mapCoordDesc; set => mapCoordDesc = value; }

    [NodeMember]
    public string? LongDesc { get => longDesc; set => longDesc = value; }

    [NodeMember(ExactlyNamed = true)]
    public string? DisplayName { get => displayName; set => displayName = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool? IsEditable { get => isEditable; set => isEditable = value; }

    [NodeMember(ExactlyNamed = true)]
    public float SquareSize { get => squareSize; set => squareSize = value; }

    [NodeMember(ExactlyNamed = true)]
    public float SquareHeight { get => squareHeight; set => squareHeight = value; }

    public byte CollectionID { get; set; }
    public byte CollectionPackMask { get; set; }
    public string? CollectionIcon { get; set; }
    public string? BlockInfoFlat { get; set; }
    public string? LoadingScreen { get; set; }

    [NodeMember(ExactlyNamed = true)]
    public float CameraMinHeight { get => cameraMinHeight; set => cameraMinHeight = value; }

    [NodeMember(ExactlyNamed = true)]
    public float ShadowSoftSizeInWorld { get => shadowSoftSizeInWorld; set => shadowSoftSizeInWorld = value; }

    [NodeMember(ExactlyNamed = true)]
    public float ColorVertexMin { get => colorVertexMin; set => colorVertexMin = value; }

    [NodeMember(ExactlyNamed = true)]
    public float ColorVertexMax { get => colorVertexMax; set => colorVertexMax = value; }

    [NodeMember(ExactlyNamed = true)]
    public CPlugBitmap? IconFid
    {
        get => iconFid = GetNodeFromRefTable(iconFid, iconFidIndex) as CPlugBitmap;
        set => iconFid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CPlugBitmap? LoadScreenFid
    {
        get => loadScreenFid = GetNodeFromRefTable(loadScreenFid, loadScreenFidIndex) as CPlugBitmap;
        set => loadScreenFid = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnDecoration? DefaultDecoration
    {
        get => defaultDecoration = GetNodeFromRefTable(defaultDecoration, defaultDecorationIndex) as CGameCtnDecoration;
        set => defaultDecoration = value;
    }

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnZone?[]? CompleteZoneList { get => completeZoneList; set => completeZoneList = value; }

    #region Constructors

    protected CGameCtnCollection()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 header chunk

    [Chunk(0x03033000)]
    public class Chunk03033000 : HeaderChunk<CGameCtnCollection>
    {
        public string? U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    #endregion

    #region 0x001 header chunk (desc)

    [Chunk(0x03033001, "desc")]
    public class Chunk03033001H : HeaderChunk<CGameCtnCollection>, IVersionable
    {
        private int version;

        public Vec2? U01;
        public Vec2? U02;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Byte(ref version);
            rw.Id(ref n.collection);
            rw.Boolean(ref n.needUnlock);

            if (version >= 1)
            {
                rw.String(ref n.iconEnv);
                rw.String(ref n.iconCollection);

                if (version >= 2)
                {
                    rw.Int32(ref n.sortIndex);

                    if (version >= 3)
                    {
                        rw.Id(ref n.defaultZone);

                        if (version >= 4)
                        {
                            rw.Ident(ref n.vehicle);

                            if (version >= 5)
                            {
                                rw.String(ref n.mapFid);
                                rw.Vec2(ref U01);
                                rw.Vec2(ref U02);

                                if (version < 8)
                                {
                                    rw.Vec2(ref n.mapCoordElem);

                                    if (version >= 6)
                                    {
                                        rw.Vec2(ref n.mapCoordIcon);
                                    }
                                }

                                if (version >= 7)
                                {
                                    rw.String(ref n.loadScreen);

                                    if (version >= 8)
                                    {
                                        rw.Vec2(ref n.mapCoordElem);
                                        rw.Vec2(ref n.mapCoordIcon);
                                        rw.Vec2(ref n.mapCoordDesc);
                                        rw.String(ref n.longDesc);

                                        if (version >= 9)
                                        {
                                            rw.String(ref n.displayName);

                                            if (version >= 10)
                                            {
                                                rw.Boolean(ref n.isEditable);
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

    #endregion

    #region 0x008 chunk

    /// <summary>
    /// CGameCtnCollection 0x008 chunk
    /// </summary>
    [Chunk(0x03033008)]
    public class Chunk03033008 : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecoration>(ref n.defaultDecoration, ref n.defaultDecorationIndex);
        }
    }

    #endregion

    #region 0x009 chunk

    [Chunk(0x03033009)]
    public class Chunk03033009 : Chunk<CGameCtnCollection>
    {
        private int listVersion;

        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.collection);
            rw.Int32(ref listVersion);
            rw.ArrayNode<CGameCtnZone>(ref n.completeZoneList);
            rw.Int32(ref U01);
            rw.Boolean(ref n.needUnlock);
            rw.Single(ref n.squareSize);
            rw.Single(ref n.squareHeight);
            rw.Ident(ref n.vehicle);
        }
    }

    #endregion

    #region 0x00B chunk

    /// <summary>
    /// CGameCtnCollection 0x00B chunk
    /// </summary>
    [Chunk(0x0303300B)]
    public class Chunk0303300B : Chunk<CGameCtnCollection>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x00C chunk

    [Chunk(0x0303300C)]
    public class Chunk0303300C : Chunk<CGameCtnCollection>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isEditable);
            rw.Int32(ref U01); // This one just disappeared later in the code xd
        }
    }

    #endregion

    #region 0x00D chunk

    [Chunk(0x0303300D)]
    public class Chunk0303300D : Chunk<CGameCtnCollection>
    {
        public bool U01;
        public int U02;
        public bool U03;
        public int U04;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);

            if (U01)
            {
                rw.NodeRef<CPlugBitmap>(ref n.iconFid, ref n.iconFidIndex); // IconFid
            }

            rw.Boolean(ref U03);

            if (U03)
            {
                rw.Int32(ref U04);
            }
        }
    }

    #endregion

    #region 0x00E chunk

    [Chunk(0x0303300E)]
    public class Chunk0303300E : Chunk<CGameCtnCollection>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x011 chunk (not reviewed)

    [Chunk(0x03033011)]
    public class Chunk03033011 : Chunk<CGameCtnCollection>
    {
        public int U01;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // CSysFidNodRefBase
        }
    }

    #endregion

    #region 0x013 chunk

    /// <summary>
    /// CGameCtnCollection 0x013 chunk
    /// </summary>
    [Chunk(0x03033013)]
    public class Chunk03033013 : Chunk<CGameCtnCollection>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref n.cameraMinHeight);
        }
    }

    #endregion

    #region 0x016 chunk

    /// <summary>
    /// CGameCtnCollection 0x016 chunk
    /// </summary>
    [Chunk(0x03033016)]
    public class Chunk03033016 : Chunk<CGameCtnCollection>
    {
        public int U01;
        public bool U02;
        public bool U03;
        public bool U04;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03); // BackgroundShadow?
            rw.Single(ref n.shadowSoftSizeInWorld);
            rw.Boolean(ref U04); // VertexLighting?
            rw.Single(ref n.colorVertexMin);
            rw.Single(ref n.colorVertexMax);
        }
    }

    #endregion

    #region 0x018 chunk

    /// <summary>
    /// CGameCtnCollection 0x018 chunk
    /// </summary>
    [Chunk(0x03033018)]
    public class Chunk03033018 : Chunk<CGameCtnCollection>
    {
        public int U01;
        public Rect U02;
        public Vec2 U03;
        public Vec2 U04;

        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Rect(ref U02);
            rw.Vec2(ref U03);
            rw.Vec2(ref U04);
        }
    }

    #endregion

    #region 0x019 chunk

    [Chunk(0x03033019)]
    public class Chunk03033019 : Chunk<CGameCtnCollection>
    {
        public override void ReadWrite(CGameCtnCollection n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref n.loadScreenFid, ref n.loadScreenFidIndex);
        }
    }

    #endregion

    #endregion
}
