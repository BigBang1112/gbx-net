namespace GBX.NET.Engines.Game;

/// <summary>
/// Item placed on a map (0x03101000)
/// </summary>
[Node(0x03101000)]
public class CGameCtnAnchoredObject : CMwNod, INodeDependant<CGameCtnChallenge>
{
    #region Fields

    private Ident itemModel;
    private Vec3 pitchYawRoll;
    private Byte3 blockUnitCoord;
    private string anchorTreeId = string.Empty;
    private Vec3 absolutePositionInMap;
    private CGameWaypointSpecialProperty? waypointSpecialProperty;
    private short flags;
    private float scale = 1;
    private Vec3 pivotPosition;
    private FileRef? skin;
    private DifficultyColor? color;

    #endregion

    #region Properties

    /// <summary>
    /// Name of the item with collection and author
    /// </summary>
    [NodeMember]
    public Ident ItemModel
    {
        get => itemModel;
        set => itemModel = value;
    }

    /// <summary>
    /// Pitch, yaw and roll of the item in radians.
    /// </summary>
    [NodeMember]
    public Vec3 PitchYawRoll
    {
        get => pitchYawRoll;
        set => pitchYawRoll = value;
    }

    /// <summary>
    /// Block coordinates that the item is approximately located in. It doesn't have to be provided most of the time.
    /// </summary>
    [NodeMember]
    public Byte3 BlockUnitCoord
    {
        get => blockUnitCoord;
        set => blockUnitCoord = value;
    }

    [NodeMember]
    public string AnchorTreeId
    {
        get => anchorTreeId;
        set => anchorTreeId = value;
    }

    /// <summary>
    /// The X, Y and Z position in the real world space of the item.
    /// </summary>
    [NodeMember]
    public Vec3 AbsolutePositionInMap
    {
        get => absolutePositionInMap;
        set => absolutePositionInMap = value;
    }

    /// <summary>
    /// If the item is a waypoint, contains inner waypoint info, otherwise null.
    /// </summary>
    [NodeMember]
    public CGameWaypointSpecialProperty? WaypointSpecialProperty
    {
        get => waypointSpecialProperty;
        set => waypointSpecialProperty = value;
    }

    /// <summary>
    /// Flags of the item.
    /// </summary>
    [NodeMember]
    public short Flags
    {
        get => flags;
        set => flags = value;
    }

    /// <summary>
    /// Scale of the item.
    /// </summary>
    [NodeMember]
    public float Scale
    {
        get => scale;
        set => scale = value;
    }

    /// <summary>
    /// Pivot position of the item. Useful for making rotations around a different point than center.
    /// </summary>
    [NodeMember]
    public Vec3 PivotPosition
    {
        get => pivotPosition;
        set => pivotPosition = value;
    }

    /// <summary>
    /// Variant index of the item. Taken from flags.
    /// </summary>
    [NodeMember]
    public int Variant
    {
        get => (flags >> 8) & 15;
        set => flags = (short)((flags & 0xF0FF) | ((value & 15) << 8));
    }

    /// <summary>
    /// Color of the item. Available since TM® Royal update.
    /// </summary>
    [NodeMember]
    public DifficultyColor? Color
    {
        get
        {
            ((INodeDependant<CGameCtnChallenge>)this).DependingNode?.DiscoverChunk<CGameCtnChallenge.Chunk03043062>();

            return color;
        }
        set => color = value;
    }

    [NodeMember]
    public FileRef? Skin
    {
        get => skin;
        set => skin = value;
    }

    CGameCtnChallenge? INodeDependant<CGameCtnChallenge>.DependingNode { get; set; }

    #endregion

    #region Constructors

    protected CGameCtnAnchoredObject()
    {
        itemModel = null!;
    }

    internal CGameCtnAnchoredObject(Ident itemModel, Vec3 absolutePositionInMap, Vec3 pitchYawRoll,
        Vec3 pivotPosition = default, int variant = 0, float scale = 1, Byte3 blockUnitCoord = default)
    {
        this.itemModel = itemModel;
        this.absolutePositionInMap = absolutePositionInMap;
        this.pitchYawRoll = pitchYawRoll;
        this.pivotPosition = pivotPosition;
        Variant = variant;
        this.scale = scale;
        this.blockUnitCoord = blockUnitCoord;
    }

    #endregion

    #region Methods

    public override string ToString()
    {
        return $"{base.ToString()} {{ {ItemModel} }}";
    }

    #endregion

    #region Chunks

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnAnchoredObject 0x002 chunk
    /// </summary>
    [Chunk(0x03101002)]
    public class Chunk03101002 : Chunk<CGameCtnAnchoredObject>, IVersionable
    {
        public Vec3 U01;
        public Vec3 U02;

        private int version = 7;

        /// <summary>
        /// Version of the chunk. For the lastst TM2 version, version 7 the latest, in TM®, the latest known version is 8.
        /// </summary>
        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnAnchoredObject n, GameBoxReaderWriter rw, ILogger? logger)
        {
            rw.Int32(ref version);
            rw.Ident(ref n.itemModel!);
            rw.Vec3(ref n.pitchYawRoll);
            rw.Byte3(ref n.blockUnitCoord);
            rw.Id(ref n.anchorTreeId!);
            rw.Vec3(ref n.absolutePositionInMap);

            if (rw.Mode == GameBoxReaderWriterMode.Read)
                n.waypointSpecialProperty = Parse<CGameWaypointSpecialProperty>(rw.Reader!, classId: null, progress: null, logger);
            else if (rw.Mode == GameBoxReaderWriterMode.Write)
            {
                if (n.waypointSpecialProperty is null)
                    rw.Writer!.Write(-1);
                else
                {
                    rw.Writer!.Write(0x2E009000);
                    n.waypointSpecialProperty.Write(rw.Writer, logger);
                }
            }

            if (version >= 4)
            {
                rw.Int16(ref n.flags);

                if (version >= 5)
                {
                    rw.Vec3(ref n.pivotPosition);

                    if (version >= 6)
                    {
                        rw.Single(ref n.scale);

                        if (version >= 8) // TM 2020
                        {
                            if ((n.flags & 0x4) == 0x4)
                            {
                                rw.FileRef(ref n.skin);
                            }

                            rw.Vec3(ref U01);
                            rw.Vec3(ref U02);
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region 0x004 skippable chunk

    /// <summary>
    /// CGameCtnAnchoredObject 0x004 skippable chunk
    /// </summary>
    [Chunk(0x03101004)]
    public class Chunk03101004 : SkippableChunk<CGameCtnAnchoredObject>
    {
        public int U01 = 0;
        public int U02 = -1;

        public override void ReadWrite(CGameCtnAnchoredObject n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    #endregion

    #region 0x005 skippable chunk

    /// <summary>
    /// CGameCtnAnchoredObject 0x005 skippable chunk
    /// </summary>
    [Chunk(0x03101005), IgnoreChunk]
    public class Chunk03101005 : SkippableChunk<CGameCtnAnchoredObject>
    {

    }

    #endregion

    #endregion
}
