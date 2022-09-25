using System.Runtime.Serialization;

namespace GBX.NET.Engines.Game;

/// <summary>
/// Block placed on a map.
/// </summary>
/// <remarks>ID: 0x03057000</remarks>
[Node(0x03057000)]
public class CGameCtnBlock : CMwNod
{
    #region Constants

    private const int isGroundBit = 12;
    private const int isClipBit = 13;
    private const int isSkinnableBit = 15;
    private const int isWaypointBit = 20;
    private const int isGhostBit = 28;
    private const int isFreeBit = 29;

    private const int variantMax = 63;
    private const int subVariantOffset = 6;
    private const int subVariantMax = 63;

    #endregion

    #region Fields

    private Ident blockModel;
    private Direction direction;
    private Int3 coord;
    private int flags;
    private CGameCtnBlockSkin? skin;
    private CGameWaypointSpecialProperty? waypoint;

    #endregion

    #region Properties

    /// <summary>
    /// Name of the block.
    /// </summary>
    [NodeMember]
    public string Name
    {
        get => blockModel.Id;
        set
        {
            if (blockModel is null)
            {
                blockModel = new Ident(value);
            }
            else
            {
                blockModel = new Ident(value, blockModel.Collection, blockModel.Author);
            }
        }
    }

    [NodeMember]
    public Ident BlockModel { get => blockModel; set => blockModel = value; }

    /// <summary>
    /// Facing direction of the block.
    /// </summary>
    [NodeMember]
    public Direction Direction { get => direction; set => direction = value; }

    /// <summary>
    /// Position of the block on the map in block coordination. This value get's explicitly converted to <see cref="Byte3"/> in the serialized form. Values below 0 or above 255 should be avoided.
    /// </summary>
    [NodeMember]
    public Int3 Coord { get => coord; set => coord = value; }

    /// <summary>
    /// Flags of the block. If the chunk version is null, this value can be presented as <see cref="short"/>.
    /// </summary>
    [NodeMember]
    public int Flags { get => flags; set => flags = value; }

    /// <summary>
    /// Author of the block, usually of a custom one made in Mesh Modeller.
    /// </summary>
    [NodeMember]
    public string? Author { get; set; } // TODO: Check behaviour

    public bool HasFlags => HasValidFlags(Flags);

    /// <summary>
    /// Variant of the block. Taken from flags. Value range of 0-63.
    /// </summary>
    [NodeMember]
    public byte? Variant
    {
        get => HasFlags ? (byte)(Flags & variantMax) : null;
        set
        {
            if (HasFlags)
            {
                Flags = (Flags & ~variantMax) | (value ?? 0);
            }
        }
    }

    /// <summary>
    /// Subvariant of the block. Taken from flags. Value range of 0-63. Always 63 when <see cref="IsClip"/> is true.
    /// </summary>
    [NodeMember]
    public byte? SubVariant
    {
        get => HasFlags ? (byte?)((Flags >> subVariantOffset) & subVariantMax) : null;
        set
        {
            if (HasFlags)
            {
                Flags = (Flags & ~(subVariantMax << subVariantOffset)) | ((value ?? 0) << subVariantOffset);
            }
        }
    }

    /// <summary>
    /// If the block should use the ground variant. Taken from flags.
    /// </summary>
    [NodeMember]
    public bool IsGround
    {
        get => IsGroundBlock(Flags);
        set => SetFlagBit(isGroundBit, value);
    }

    /// <summary>
    /// If the block is considered as clip. Taken from flags.
    /// </summary>
    [NodeMember]
    public bool IsClip
    {
        get => IsClipBlock(Flags);
        set => SetFlagBit(isClipBit, value);
    }

    /// <summary>
    /// Used skin on the block.
    /// </summary>
    [NodeMember]
    public CGameCtnBlockSkin? Skin
    {
        get => skin;
        set => SetFlagBitAndObject(isSkinnableBit, ref skin, value);
    }

    /// <summary>
    /// Taken from flags.
    /// </summary>
    [NodeMember]
    public bool Bit17
    {
        get => HasFlags && IsFlagBitSet(17);
        set => SetFlagBit(17, value);
    }

    /// <summary>
    /// Additional block parameters.
    /// </summary>
    [NodeMember]
    public CGameWaypointSpecialProperty? WaypointSpecialProperty
    {
        get => waypoint;
        set => SetFlagBitAndObject(isWaypointBit, ref waypoint, value);
    }

    /// <summary>
    /// Determines the hill ground variant in TM®. Taken from flags.
    /// </summary>
    [NodeMember]
    public bool Bit21
    {
        get => HasFlags && IsFlagBitSet(21);
        set => SetFlagBit(21, value);
    }

    [NodeMember]
    public bool IsGhost
    {
        get => IsGhostBlock(Flags);
        set => SetFlagBit(isGhostBit, value);
    }

    /// <summary>
    /// If this block is a free block. Feature available since TM®. Set this property first before modifying free transformation.
    /// </summary>
    [NodeMember]
    public bool IsFree
    {
        get => IsFreeBlock(Flags);
    }

    /// <summary>
    /// Color of the block. Available since TM2020 Royal update.
    /// </summary>
    [NodeMember(ExactName = "MapElemColor")]
    public DifficultyColor? Color { get; set; }

    /// <summary>
    /// Lightmap quality setting of the block. Available since TM2020.
    /// </summary>
    [NodeMember(ExactName = "MapElemLmQuality")]
    public LightmapQuality? LightmapQuality { get; set; }

    /// <summary>
    /// Reference to the macroblock that placed this block. In macroblock mode, this block is then part of a selection group. Available since TM2020.
    /// </summary>
    [NodeMember]
    public MacroblockInstance? MacroblockReference { get; set; }

    #endregion

    #region Static properties

    /// <summary>
    /// A type of block that seperates other blocks in ManiaPlanet. The game can sometimes crash if it isn't provided in the map file, especially in ManiaPlanet (not Trackmania®). One theory is that this block determines what blocks should be undone by Undo.
    /// </summary>
    [IgnoreDataMember]
    public static CGameCtnBlock Unassigned1 => new("Unassigned1", Direction.East, (0, 0, 0), -1);

    #endregion

    #region Constructors

    internal CGameCtnBlock()
    {
        blockModel = Ident.Empty;
    }

    public CGameCtnBlock(Ident blockModel, Direction direction, Int3 coord, int flags)
    {
        this.blockModel = blockModel;
        this.direction = direction;
        this.coord = coord;
        this.flags = flags;
    }

    public CGameCtnBlock(string name,
                         Direction direction,
                         Int3 coord,
                         int flags = 0,
                         string? author = null,
                         CGameCtnBlockSkin? skin = null,
                         CGameWaypointSpecialProperty? waypoint = null)
    {
        blockModel = new Ident(name);
        this.direction = direction;
        this.coord = coord;
        this.flags = flags;
        Author = author;
        this.skin = skin;
        this.waypoint = waypoint;
    }

    #endregion

    #region Methods

    public override string ToString() => $"{base.ToString()} {{ {Name} {Coord} }}";
    
    private void SetFlagBit(int bit, bool value)
    {
        if (value) Flags |= 1 << bit;
        else Flags &= ~(1 << bit);
    }

    private void SetFlagBitAndObject<T>(int bit, ref T? obj, T? value)
    {
        if (!HasFlags)
        {
            return;
        }

        if (value is null)
        {
            Flags &= ~(1 << bit);
            obj = default;
            return;
        }

        Flags |= 1 << bit;
        obj = value;
    }
    
    public bool IsFlagBitSet(int bit) => IsFlagBitSet(Flags, bit);

    private static bool HasValidFlags(int flags) => flags != -1;
    private static bool IsFlagBitSet(int flags, int bit) => (flags & (1 << bit)) != 0;

    internal static bool IsGroundBlock_WhenDefined(int flags) => IsFlagBitSet(flags, isGroundBit);
    internal static bool IsClipBlock_WhenDefined(int flags) => IsFlagBitSet(flags, isClipBit);
    internal static bool IsSkinnableBlock_WhenDefined(int flags) => IsFlagBitSet(flags, isSkinnableBit);
    internal static bool IsWaypointBlock_WhenDefined(int flags) => IsFlagBitSet(flags, isWaypointBit);
    internal static bool IsGhostBlock_WhenDefined(int flags) => IsFlagBitSet(flags, isGhostBit);
    internal static bool IsFreeBlock_WhenDefined(int flags) => IsFlagBitSet(flags, isFreeBit);

    internal static bool IsGroundBlock(int flags) => HasValidFlags(flags) && IsGroundBlock_WhenDefined(flags);
    internal static bool IsClipBlock(int flags) => HasValidFlags(flags) && IsClipBlock_WhenDefined(flags);
    internal static bool IsSkinnableBlock(int flags) => HasValidFlags(flags) && IsSkinnableBlock_WhenDefined(flags);
    internal static bool IsWaypointBlock(int flags) => HasValidFlags(flags) && IsWaypointBlock_WhenDefined(flags);
    internal static bool IsGhostBlock(int flags) => HasValidFlags(flags) && IsGhostBlock_WhenDefined(flags);
    internal static bool IsFreeBlock(int flags) => HasValidFlags(flags) && IsFreeBlock_WhenDefined(flags);

    #endregion

    #region Chunks

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnBlock 0x002 chunk
    /// </summary>
    [Chunk(0x03057002)]
    public class Chunk03057002 : Chunk<CGameCtnBlock>
    {
        public override void ReadWrite(CGameCtnBlock n, GameBoxReaderWriter rw)
        {
            rw.Ident(ref n.blockModel!);
            rw.EnumByte<Direction>(ref n.direction);
            rw.Byte3(ref n.coord);
            rw.Int32(ref n.flags);
        }
    }

    #endregion

    #endregion
}
