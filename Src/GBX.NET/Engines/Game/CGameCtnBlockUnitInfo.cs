namespace GBX.NET.Engines.Game;

[Node(0x03036000)]
public class CGameCtnBlockUnitInfo : CMwNod
{
    #region Fields

    private int placePylons;
    private int acceptPylons;
    private Int3 relativeOffset;
    private ExternalNode<CGameCtnBlockInfoClip>[]? clips;
    private bool underground;
    private string? terrainModifierId;
    private CGameCtnBlockInfoClip? bottomClip;
    private GameBoxRefTable.File? bottomClipFile;
    private CGameCtnBlockInfoClip? topClip;
    private GameBoxRefTable.File? topClipFile;
    private Direction? bottomClipDir;
    private Direction? topClipDir;
    private ExternalNode<CGameCtnBlockInfoClip>[]? clipsNorth;
    private ExternalNode<CGameCtnBlockInfoClip>[]? clipsEast;
    private ExternalNode<CGameCtnBlockInfoClip>[]? clipsSouth;
    private ExternalNode<CGameCtnBlockInfoClip>[]? clipsWest;
    private ExternalNode<CGameCtnBlockInfoClip>[]? clipsTop;
    private ExternalNode<CGameCtnBlockInfoClip>[]? clipsBottom;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03036000>]
    public int PlacePylons { get => placePylons; set => placePylons = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03036004>]
    public int AcceptPylons { get => acceptPylons; set => acceptPylons = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03036000>]
    public Int3 RelativeOffset { get => relativeOffset; set => relativeOffset = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03036000>]
    public ExternalNode<CGameCtnBlockInfoClip>[]? Clips
    {
        get => clips = GetNodesFromRefTable(clips);
        set => clips = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03036002>]
    public bool Underground
    {
        get
        {
            DiscoverChunk<Chunk03036002>();
            return underground;
        }
        set
        {
            DiscoverChunk<Chunk03036002>();
            underground = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03036005>]
    public string? TerrainModifierId { get => terrainModifierId; set => terrainModifierId = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03036008>]
    [AppliedWithChunk<Chunk0303600B>]
    public CGameCtnBlockInfoClip? BottomClip
    {
        get => bottomClip = GetNodeFromRefTable(bottomClip, bottomClipFile) as CGameCtnBlockInfoClip;
        set => bottomClip = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03036008>]
    [AppliedWithChunk<Chunk0303600B>]
    public CGameCtnBlockInfoClip? TopClip
    {
        get => topClip = GetNodeFromRefTable(topClip, topClipFile) as CGameCtnBlockInfoClip;
        set => topClip = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303600B>]
    public Direction? BottomClipDir { get => bottomClipDir; set => bottomClipDir = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0303600B>]
    public Direction? TopClipDir { get => topClipDir; set => topClipDir = value; }

    [NodeMember(ExactName = "Clips_North")]
    [AppliedWithChunk<Chunk0303600C>]
    public ExternalNode<CGameCtnBlockInfoClip>[]? ClipsNorth { get => clipsNorth; set => clipsNorth = value; }
    
    [NodeMember(ExactName = "Clips_East")]
    [AppliedWithChunk<Chunk0303600C>]
    public ExternalNode<CGameCtnBlockInfoClip>[]? ClipsEast { get => clipsEast; set => clipsEast = value; }
    
    [NodeMember(ExactName = "Clips_South")]
    [AppliedWithChunk<Chunk0303600C>]
    public ExternalNode<CGameCtnBlockInfoClip>[]? ClipsSouth { get => clipsSouth; set => clipsSouth = value; }
    
    [NodeMember(ExactName = "Clips_West")]
    [AppliedWithChunk<Chunk0303600C>]
    public ExternalNode<CGameCtnBlockInfoClip>[]? ClipsWest { get => clipsWest; set => clipsWest = value; }
    
    [NodeMember(ExactName = "Clips_Top")]
    [AppliedWithChunk<Chunk0303600C>]
    public ExternalNode<CGameCtnBlockInfoClip>[]? ClipsTop { get => clipsTop; set => clipsTop = value; }
    
    [NodeMember(ExactName = "Clips_Bottom")]
    [AppliedWithChunk<Chunk0303600C>]
    public ExternalNode<CGameCtnBlockInfoClip>[]? ClipsBottom { get => clipsBottom; set => clipsBottom = value; }

    #endregion

    #region Constructors

    internal CGameCtnBlockUnitInfo()
    {

    }

    #endregion

    #region Methods

    public override string ToString() => $"{base.ToString()} {{ {relativeOffset} }}";

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x000 chunk
    /// </summary>
    [Chunk(0x03036000)]
    public class Chunk03036000 : Chunk<CGameCtnBlockUnitInfo>
    {
        public bool U01;
        public bool U02;

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.placePylons);
            rw.Boolean(ref U01); // AcceptPylons?
            rw.Boolean(ref U02);
            rw.Int3(ref n.relativeOffset);
            rw.ArrayNode<CGameCtnBlockInfoClip>(ref n.clips);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x001 chunk
    /// </summary>
    [Chunk(0x03036001)]
    public class Chunk03036001 : Chunk<CGameCtnBlockUnitInfo>
    {
        public string? U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01); // Desert, Grass
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    #endregion

    #region 0x002 chunk (Underground)

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x002 chunk (Underground)
    /// </summary>
    [Chunk(0x03036002, "Underground")]
    public class Chunk03036002 : SkippableChunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.underground);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x003 chunk
    /// </summary>
    [Chunk(0x03036003)]
    public class Chunk03036003 : Chunk<CGameCtnBlockUnitInfo>
    {
        public CMwNod? U01;
        public GameBoxRefTable.File? U01file;
        public string? U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01, ref U01file); // pillar stuff?
            rw.Id(ref U02); // TerrainModifierId?
            rw.Int32(ref U03);
            rw.Int32(ref U04);
        }
    }

    #endregion

    #region 0x004 chunk (AcceptPylons)

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x004 chunk (AcceptPylons)
    /// </summary>
    [Chunk(0x03036004, "AcceptPylons")]
    public class Chunk03036004 : Chunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.acceptPylons);
        }
    }

    #endregion

    #region 0x005 chunk (TerrainModifierId)

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x005 chunk (TerrainModifierId)
    /// </summary>
    [Chunk(0x03036005, "TerrainModifierId")]
    public class Chunk03036005 : Chunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.terrainModifierId);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x006 chunk
    /// </summary>
    [Chunk(0x03036006)]
    public class Chunk03036006 : Chunk<CGameCtnBlockUnitInfo>
    {
        public bool U01;
        public (bool, int) U02;

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);

            for (var i = 0; i < 4; i++)
            {
                rw.Boolean(ref U02.Item1);
                rw.Int32(ref U02.Item2);
            }
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x007 chunk
    /// </summary>
    [Chunk(0x03036007)]
    public class Chunk03036007 : Chunk<CGameCtnBlockUnitInfo>
    {
        public ExternalNode<CMwNod>[]? U01;

        public override void Read(CGameCtnBlockUnitInfo n, GameBoxReader r)
        {
            U01 = new ExternalNode<CMwNod>[4]; // or pylons?

            for (var i = 0; i < U01.Length; i++)
            {
                var node = r.ReadNodeRef<CMwNod>(out GameBoxRefTable.File? file);
                U01[i] = new(node, file);
            }
        }

        public override void Write(CGameCtnBlockUnitInfo n, GameBoxWriter w)
        {
            for (var i = 0; i < (n.clips?.Length ?? 0); i++) // or pylons?
            {
                if (U01 is null || i >= U01.Length)
                {
                    w.Write(-1);
                }
                else 
                {
                    var extNode = U01[i];
                    
                    if (extNode.File is not null)
                    {
                        w.Write(extNode.File.NodeIndex + 1);
                    }
                    else
                    {
                        w.Write(extNode.Node);
                    }
                }
            }
        }
    }

    #endregion

    #region 0x008 chunk (bottom+top clip)

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x008 chunk (bottom+top clip)
    /// </summary>
    [Chunk(0x03036008, "bottom+top clip")]
    public class Chunk03036008 : Chunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnBlockInfoClip>(ref n.bottomClip);
            rw.NodeRef<CGameCtnBlockInfoClip>(ref n.topClip);
        }
    }

    #endregion

    #region 0x009 chunk

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x009 chunk
    /// </summary>
    [Chunk(0x03036009)]
    public class Chunk03036009 : Chunk<CGameCtnBlockUnitInfo>, IVersionable
    {
        private int version;

        public CMwNod? U01;
        private GameBoxRefTable.File? U01file;
        public int U02;
        public int? U03;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.NodeRef(ref U01, ref U01file); // pillar stuff?
            rw.Int32(ref U02);

            if (version == 0)
            {
                rw.Int32(ref U03);
            }

            if (version == 2)
            {
                rw.Byte(ref U03);
            }
        }
    }

    #endregion

    #region 0x00A chunk

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x00A chunk
    /// </summary>
    [Chunk(0x0303600A)]
    public class Chunk0303600A : Chunk<CGameCtnBlockUnitInfo>, IVersionable
    {
        private int version;

        public CMwNod? U01;
        private GameBoxRefTable.File? U01file;
        public int U02;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef(ref U01, ref U01file); // pillar stuff?
            rw.Int32(ref U02);
        }
    }

    #endregion

    #region 0x00B chunk (bottom+top clip with dir)

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x00B chunk (bottom+top clip with dir)
    /// </summary>
    [Chunk(0x0303600B, "bottom+top clip with dir")]
    public class Chunk0303600B : Chunk<CGameCtnBlockUnitInfo>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef<CGameCtnBlockInfoClip>(ref n.bottomClip, ref n.bottomClipFile);
            rw.NodeRef<CGameCtnBlockInfoClip>(ref n.topClip, ref n.topClipFile);
            rw.EnumInt32<Direction>(ref n.bottomClipDir);
            rw.EnumInt32<Direction>(ref n.topClipDir);
        }
    }

    #endregion

    #region 0x00C chunk

    /// <summary>
    /// CGameCtnBlockUnitInfo 0x00C chunk
    /// </summary>
    [Chunk(0x0303600C)]
    public class Chunk0303600C : Chunk<CGameCtnBlockUnitInfo>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }
        
        public short? U01;
        public short? U02;
        public int? U03;
        public int? U04;

        public override void Read(CGameCtnBlockUnitInfo n, GameBoxReader r)
        {
            var version = r.ReadInt32();

            if (version == 0)
            {
                //rw.Int16();
                throw new ChunkVersionNotSupportedException(version);
            }

            var clipCountBits = r.ReadInt32();
            
            var clipCountNorth = clipCountBits & 7;
            var clipCountEast = clipCountBits >> 3 & 7;
            var clipCountSouth = clipCountBits >> 6 & 7;
            var clipCountWest = clipCountBits >> 9 & 7;
            var clipCountTop = clipCountBits >> 12 & 7;
            var clipCountBottom = clipCountBits >> 15 & 7;
            
            n.clipsNorth = r.ReadExternalNodeArray<CGameCtnBlockInfoClip>(clipCountNorth);
            n.clipsEast = r.ReadExternalNodeArray<CGameCtnBlockInfoClip>(clipCountEast);
            n.clipsSouth = r.ReadExternalNodeArray<CGameCtnBlockInfoClip>(clipCountSouth);
            n.clipsWest = r.ReadExternalNodeArray<CGameCtnBlockInfoClip>(clipCountWest);
            n.clipsTop = r.ReadExternalNodeArray<CGameCtnBlockInfoClip>(clipCountTop);
            n.clipsBottom = r.ReadExternalNodeArray<CGameCtnBlockInfoClip>(clipCountBottom);

            if (version >= 2)
            {
                U01 = r.ReadInt16();
                U02 = r.ReadInt16();
            }
            else
            {
                U03 = r.ReadInt32();
                U04 = r.ReadInt32();
            }
        }
    }

    #endregion

    #endregion
}
