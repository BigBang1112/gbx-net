namespace GBX.NET.Engines.Game;

/// <summary>
/// Item placed on a map.
/// </summary>
/// <remarks>ID: 0x03101000</remarks>
[Node(0x03101000)]
public class CGameCtnAnchoredObject : CMwNod
{
    #region Enums

    public enum EPhaseOffset
    {
        None,
        One8th,
        Two8th,
        Three8th,
        Four8th,
        Five8th,
        Six8th,
        Seven8th
    }

    #endregion

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
    private FileRef? packDesc;

    #endregion

    #region Properties

    /// <summary>
    /// Name of the item with collection and author
    /// </summary>
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03101002))]
    public Ident ItemModel { get => itemModel; set => itemModel = value; }

    /// <summary>
    /// Pitch, yaw and roll of the item in radians.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03101002))]
    public Vec3 PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }

    /// <summary>
    /// Block coordinates that the item is approximately located in. It doesn't have to be provided most of the time.
    /// </summary>
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03101002))]
    public Byte3 BlockUnitCoord { get => blockUnitCoord; set => blockUnitCoord = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03101002))]
    public string AnchorTreeId { get => anchorTreeId; set => anchorTreeId = value; }

    /// <summary>
    /// The X, Y and Z position in the real world space of the item.
    /// </summary>
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03101002))]
    public Vec3 AbsolutePositionInMap { get => absolutePositionInMap; set => absolutePositionInMap = value; }

    /// <summary>
    /// If the item is a waypoint, contains inner waypoint info, otherwise null.
    /// </summary>
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03101002))]
    public CGameWaypointSpecialProperty? WaypointSpecialProperty { get => waypointSpecialProperty; set => waypointSpecialProperty = value; }

    /// <summary>
    /// Flags of the item.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03101002), sinceVersion: 4)]
    public short Flags { get => flags; set => flags = value; }

    /// <summary>
    /// Variant index of the item. Taken from flags.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03101002), sinceVersion: 4)]
    public int Variant
    {
        get => (flags >> 8) & 15;
        set => flags = (short)((flags & 0xF0FF) | ((value & 15) << 8));
    }

    /// <summary>
    /// Pivot position of the item. Useful for making rotations around a different point than center.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03101002), sinceVersion: 5)]
    public Vec3 PivotPosition { get => pivotPosition; set => pivotPosition = value; }

    /// <summary>
    /// Scale of the item. This value currently doesn't have any effect.
    /// </summary>
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03101002), sinceVersion: 6)]
    public float Scale { get => scale; set => scale = value; }

    /// <summary>
    /// Skin used on the item.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03101002), sinceVersion: 7)]
    public FileRef? PackDesc { get => packDesc; set => packDesc = value; }

    /// <summary>
    /// Block that tells when that block gets deleted, this item is deleted with it. Works for TM2020 only.
    /// </summary>
    [NodeMember]
    public CGameCtnBlock? SnappedOnBlock { get; set; }

    /// <summary>
    /// Item that tells when that item gets deleted, this item is deleted with it. Works for TM2020 only but is more modern.
    /// </summary>
    [NodeMember]
    public CGameCtnAnchoredObject? SnappedOnItem { get; set; }

    /// <summary>
    /// Item that tells when that item gets deleted, this item is deleted with it. Works for ManiaPlanet, used to work in the past in TM2020 but now it likely doesn't.
    /// </summary>
    [NodeMember]
    public CGameCtnAnchoredObject? PlacedOnItem { get; set; }

    /// <summary>
    /// Group number that groups items that get deleted together. Works for TM2020 only.
    /// </summary>
    [NodeMember]
    public int? SnappedOnGroup { get; set; }

    /// <summary>
    /// Color of the item. Available since TM2020 Royal update.
    /// </summary>
    [NodeMember(ExactName = "MapElemColor")]
    public DifficultyColor? Color { get; set; }

    /// <summary>
    /// Phase of the animation. Available since TM2020 Royal update.
    /// </summary>
    [NodeMember]
    public EPhaseOffset? AnimPhaseOffset { get; set; }

    /// <summary>
    /// The second layer of skin. Available since TM2020.
    /// </summary>
    [NodeMember]
    public FileRef? ForegroundPackDesc { get; set; }

    /// <summary>
    /// Lightmap quality setting of the item. Available since TM2020.
    /// </summary>
    [NodeMember(ExactName = "MapElemLmQuality")]
    public LightmapQuality? LightmapQuality { get; set; }

    /// <summary>
    /// Reference to the macroblock that placed this item. In macroblock mode, this item is then part of a selection group. Available since TM2020.
    /// </summary>
    [NodeMember]
    public MacroblockInstance? MacroblockReference { get; set; }

    #endregion

    #region Constructors

    internal CGameCtnAnchoredObject()
    {
        itemModel = Ident.Empty;
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
        private int version = 7;

        public Vec3 U01;
        public Vec3 U02;
        public int U03;

        /// <summary>
        /// Version of the chunk. For the lastst TM2 version, version 7 the latest, in TM®, the latest known version is 8.
        /// </summary>
        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnAnchoredObject n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Ident(ref n.itemModel!);
            rw.Vec3(ref n.pitchYawRoll);
            rw.Byte3(ref n.blockUnitCoord);
            rw.Id(ref n.anchorTreeId!);
            rw.Vec3(ref n.absolutePositionInMap);
            rw.NodeRef(ref n.waypointSpecialProperty);

            if (version < 5)
            {
                rw.Int32(ref U03);
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

                        if (version >= 7)
                        {
                            if ((n.flags & 4) == 4)
                            {
                                rw.FileRef(ref n.packDesc);
                            }

                            if (version >= 8) // TM 2020
                            {
                                rw.Vec3(ref U01);
                                rw.Vec3(ref U02);

                                if (version >= 9)
                                {
                                    throw new VersionNotSupportedException(version);
                                }
                            }
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
    public class Chunk03101004 : SkippableChunk<CGameCtnAnchoredObject>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public int U02 = -1;

        public override void ReadWrite(CGameCtnAnchoredObject n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
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
