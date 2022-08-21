using System.Runtime.Serialization;

namespace GBX.NET.Engines.Game;

/// <summary>
/// Block placed on a map.
/// </summary>
/// <remarks>ID: 0x03057000</remarks>
[Node(0x03057000)]
public class CGameCtnBlock : CMwNod, INodeDependant<CGameCtnChallenge>
{
    #region Constants

    private const int isGroundBit = 12;
    private const int isSkinnableBit = 15;
    private const int isWaypointBit = 20;
    private const int isGhostBit = 28;
    private const int isFreeBit = 29;

    #endregion

    #region Fields

    private Ident blockModel;
    private Direction direction;
    private Int3 coord;
    private int flags;
    private CGameCtnBlockSkin? skin;
    private CGameWaypointSpecialProperty? waypoint;
    private DifficultyColor? color;

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

    /// <summary>
    /// Used skin on the block.
    /// </summary>
    [NodeMember]
    public CGameCtnBlockSkin? Skin
    {
        get => skin;
        set
        {
            if (Flags == -1)
                return;

            if (value is null)
            {
                Flags &= ~(1 << isSkinnableBit);
                skin = null;
                return;
            }

            Flags |= 1 << isSkinnableBit;
            skin = value;
        }
    }

    /// <summary>
    /// Additional block parameters.
    /// </summary>
    [NodeMember]
    public CGameWaypointSpecialProperty? WaypointSpecialProperty
    {
        get => waypoint;
        set
        {
            if (Flags == -1)
                return;

            if (value is null)
            {
                Flags &= ~(1 << isWaypointBit);
                waypoint = null;

                return;
            }

            Flags |= 1 << isWaypointBit;
            waypoint = value;
        }
    }

    [NodeMember]
    public bool IsGhost
    {
        get => IsGhostBlock(Flags);
        set
        {
            if (value) Flags |= 1 << isGhostBit;
            else Flags &= ~(1 << isGhostBit);
        }
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
    /// If the block should use the ground variant. Taken from flags.
    /// </summary>
    [NodeMember]
    public bool IsGround // ground: bit 12
    {
        get => IsGroundBlock(Flags);
        set
        {
            if (value) Flags |= 1 << isGroundBit;
            else Flags &= ~(1 << isGroundBit);
        }
    }

    /// <summary>
    /// Determines the hill ground variant in TM®. Taken from flags.
    /// </summary>
    [NodeMember]
    public bool Bit21
    {
        get => Flags > -1 && (Flags & (1 << 21)) != 0;
        set
        {
            if (value) Flags |= 1 << 21;
            else Flags &= ~(1 << 21);
        }
    }

    /// <summary>
    /// Taken from flags.
    /// </summary>
    [NodeMember]
    public bool Bit17
    {
        get => Flags > -1 && (Flags & (1 << 17)) != 0;
        set
        {
            if (value) Flags |= 1 << 17;
            else Flags &= ~(1 << 17);
        }
    }

    /// <summary>
    /// If the block is considered as clip. Taken from flags.
    /// </summary>
    [NodeMember]
    public bool IsClip => Flags > -1 && ((Flags >> 6) & 63) == 63;

    /// <summary>
    /// Variant of the block. Taken from flags.
    /// </summary>
    [NodeMember]
    public byte? Variant
    {
        get => Flags > -1 ? (byte?)(Flags & 15) : null;
        set
        {
            if (value.HasValue)
                Flags = (int)(Flags & 0xFFFFFFF0) + (value.Value & 15);
        }
    }

    /// <summary>
    /// Color of the block. Available since TM® Royal update.
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

    CGameCtnChallenge? INodeDependant<CGameCtnChallenge>.DependingNode { get; set; }

    #endregion

    #region Static properties

    /// <summary>
    /// A type of block that seperates other blocks in ManiaPlanet. The game can sometimes crash if it isn't provided in the map file, especially in ManiaPlanet (not Trackmania®). One theory is that this block determines what blocks should be undone by Undo.
    /// </summary>
    [IgnoreDataMember]
    public static CGameCtnBlock Unassigned1 => new("Unassigned1", Direction.East, (-1, -1, -1), -1);

    #endregion

    #region Constructors

    protected CGameCtnBlock()
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

    internal static bool IsGhostBlock_WhenDefined(int flags) => (flags & (1 << isGhostBit)) != 0;
    internal static bool IsFreeBlock_WhenDefined(int flags) => (flags & (1 << isFreeBit)) != 0;
    internal static bool IsGroundBlock_WhenDefined(int flags) => (flags & (1 << isGroundBit)) != 0;
    internal static bool IsWaypointBlock_WhenDefined(int flags) => (flags & (1 << isWaypointBit)) != 0;
    internal static bool IsSkinnableBlock_WhenDefined(int flags) => (flags & (1 << isSkinnableBit)) != 0;

    internal static bool IsGhostBlock(int flags) => flags > -1 && IsGhostBlock_WhenDefined(flags);
    internal static bool IsFreeBlock(int flags) => flags > -1 && IsFreeBlock_WhenDefined(flags);
    internal static bool IsGroundBlock(int flags) => flags > -1 && IsGroundBlock_WhenDefined(flags);
    internal static bool IsWaypointBlock(int flags) => flags > -1 && IsWaypointBlock_WhenDefined(flags);
    internal static bool IsSkinnableBlock(int flags) => flags > -1 && IsSkinnableBlock_WhenDefined(flags);

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
