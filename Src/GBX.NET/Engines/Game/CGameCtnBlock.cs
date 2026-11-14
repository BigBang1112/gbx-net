using GBX.NET.Interfaces.Game;

namespace GBX.NET.Engines.Game;

[ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
public partial class CGameCtnBlock : IGameCtnBlockTM10, IGameCtnBlockTMSX, IGameCtnBlockTMF, IGameCtnBlockMP4, IGameCtnBlockTM2020
{
    private const int GroundBit = 12;
    private const int EditableBit = 13;
    private const int PillarBit = 14;
    private const int SkinnableBit = 15;
    private const int ReplacementBit = 16;
    private const int DecalBit = 17;
    private const int SquareCardEventsBit = 18;
    private const int PhyCharBit = 19;
    private const int WaypointBit = 20;
    private const int GhostBit = 28;
    private const int FreeBit = 29;

    private const int VariantMax = 63;
    private const int SubVariantOffset = 6;
    private const int SubVariantMax = 63;
    private const int BlockInfoVariantOffset = 21;
    private const int BlockInfoVariantMax = 127;

    private string? name;
    /// <summary>
    /// Name of the block.
    /// </summary>
    public string Name
    {
        get => name ?? blockModel.Id;
        set => name = value;
    }

    private Ident blockModel = Ident.Empty;
    public Ident BlockModel
    {
        get
        {
            if (name is null)
            {
                return blockModel;
            }

            if (blockModel.Id == name)
            {
                return blockModel;
            }

            return blockModel = blockModel with { Id = name };
        }
        set
        {
            blockModel = value;
            name = value.Id;
        }
    }

    private Int3 coord;
    /// <summary>
    /// Position of the block on the map in block coordination.
    /// </summary>
    public Int3 Coord { get => coord; set => coord = value; }

	/// <summary>
	/// Facing direction of the block.
	/// </summary>
	public Direction Direction { get; set; }

	private int flags;
    /// <summary>
    /// Flags of the block. If the blocks version is 0, this value can be presented as <see cref="short"/>.
    /// </summary>
    public int Flags { get => flags; set => flags = value; }

    /// <summary>
    /// Variant of the block. Taken from flags. Value range of 0-63.
    /// </summary>
    public byte Variant
    {
        get => (byte)(flags & VariantMax);
        set => flags = (flags & ~VariantMax) | (value & VariantMax);
    }

    /// <summary>
    /// Subvariant of the block. Taken from flags. Value range of 0-63. Always 63 when <see cref="IsClip"/> is true.
    /// </summary>
    public byte SubVariant
    {
        get => (byte)((flags >> SubVariantOffset) & SubVariantMax);
        set => flags = (flags & ~(SubVariantMax << SubVariantOffset)) | ((value & SubVariantMax) << SubVariantOffset);
    }

    /// <summary>
    /// Ground or air block info variant index. Range 0-127. Supported by ManiaPlanet, Turbo, and TM2020.
    /// </summary>
    /// <remarks><see cref="IsGround"/> selects ground or air. Index 0 selects the primary variant, and higher indices select additional variants at index minus one.</remarks>
    public byte BlockInfoVariantIndex
    {
        get => (byte)((flags >> BlockInfoVariantOffset) & BlockInfoVariantMax);
        set => flags = (flags & ~(BlockInfoVariantMax << BlockInfoVariantOffset)) | ((value & BlockInfoVariantMax) << BlockInfoVariantOffset);
    }

    /// <summary>
    /// If the block should use the ground variant. Taken from flags.
    /// </summary>
    public bool IsGround
    {
        get => IsFlagBitSet(GroundBit);
        set => SetFlagBit(GroundBit, value);
    }

    /// <summary>
    /// Whether the block is editable in the puzzle or simple editor. It can also identify a clip block.
    /// </summary>
    /// <remarks>Native <c>Editable</c> in older games and <c>IsEditableInPuzzleOrSimpleEditor</c> in TM2020.</remarks>
    public bool IsEditable
    {
        get => IsFlagBitSet(EditableBit);
        set => SetFlagBit(EditableBit, value);
    }

    /// <summary>
    /// Compatibility alias for <see cref="IsEditable"/>.
    /// </summary>
    [Obsolete("Use IsEditable instead")]
    public bool IsClip
    {
        get => IsEditable;
        set => IsEditable = value;
    }

    /// <summary>
    /// Cached pillar classification from the block model.
    /// </summary>
    public bool IsPillar
    {
        get => IsFlagBitSet(PillarBit);
        set => SetFlagBit(PillarBit, value);
    }

    /// <summary>
    /// Used skin on the block.
    /// </summary>
    public CGameCtnBlockSkin? Skin
    {
        get => skinRef?.Resolve();
        set
        {
            SetFlagBit(SkinnableBit, value is not null || !string.IsNullOrEmpty(Author));
            NodeRef.SetEmbedded(ref skinRef, value);
        }
    }

    /// <summary>
    /// Cached replacement classification from the selected block-info variant.
    /// </summary>
    public bool IsReplacement
    {
        get => IsFlagBitSet(ReplacementBit);
        set => SetFlagBit(ReplacementBit, value);
    }

    /// <summary>
    /// Character physics.
    /// </summary>
    [AppliedWithArchive(2)]
    public CPlugCharPhySpecialProperty? PhyCharSpecialProperty
    {
        get => phyCharSpecialPropertyRef?.Resolve();
        set
        {
            SetFlagBit(PhyCharBit, value is not null);
            NodeRef.SetEmbedded(ref phyCharSpecialPropertyRef, value);
        }
    }

    private SSquareCardEventIds[]? squareCardEventIds;
    /// <summary>
    /// Card events associated with block units.
    /// </summary>
    [AppliedWithArchive(2)]
    public SSquareCardEventIds[]? SquareCardEventIds
    {
        get => squareCardEventIds;
        set
        {
            SetFlagBit(SquareCardEventsBit, value is { Length: > 0 });
            squareCardEventIds = value;
        }
    }

    /// <summary>
    /// Waypoint parameters.
    /// </summary>
    public CGameWaypointSpecialProperty? WaypointSpecialProperty
    {
        get => waypointSpecialPropertyRef?.Resolve();
        set
        {
            SetFlagBit(WaypointBit, value is not null);
            NodeRef.SetEmbedded(ref waypointSpecialPropertyRef, value);
        }
    }

    /// <summary>
    /// The low bit of <see cref="BlockInfoVariantIndex"/>. Retained for compatibility.
    /// </summary>
    [Obsolete("Use BlockInfoVariantIndex instead.")]
    public bool Bit21
    {
        get => IsFlagBitSet(21);
        set => SetFlagBit(21, value);
    }

    /// <summary>
    /// Whether the block uses ghost placement. Supported by ManiaPlanet, Turbo, and TM2020.
    /// </summary>
    public bool IsGhost
    {
        get => IsFlagBitSet(GhostBit);
        set => SetFlagBit(GhostBit, value);
    }

    /// <summary>
    /// Whether this block uses free placement with an absolute position and rotation. Available since TM2020.
    /// </summary>
    public bool IsFree
    {
        get => IsFlagBitSet(FreeBit);
        set => SetFlagBit(FreeBit, value);
    }

    /// <summary>
    /// Absolute position of the block in the map. Used only in TM2020 in Free block mode.
    /// </summary>
    public Vec3? AbsolutePositionInMap { get; set; }

    /// <summary>
    /// Rotation of the block. Used only in TM2020 in Free block mode.
    /// </summary>
    public Vec3? YawPitchRoll { get; set; }

    /// <summary>
    /// [MapElemColor] Color of the block. Available since TM2020 Royal update.
    /// </summary>
    public DifficultyColor Color { get; set; }

    /// <summary>
    /// [MapElemLmQuality] Lightmap quality setting of the block. Available since TM2020.
    /// </summary>
    public LightmapQuality LightmapQuality { get; set; }

    /// <summary>
    /// Reference to the macroblock that placed this block. In macroblock mode, this block is then part of a selection group. Available since TM2020.
    /// </summary>
    public MacroblockInstance? MacroblockReference { get; set; }

    private string? decalId;
    public string? DecalId
    {
        get => decalId;
        set => SetFlagBitAndObject(DecalBit, ref decalId, value);
    }

    public TMUnlimiter? TMUnlimiterData { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnBlock"/>. Adding chunks is not needed (only when creating blocks for TM1.0 from 2003).
    /// </summary>
    public CGameCtnBlock()
    {

    }

    public partial class Chunk03057002
    {
        public override void Read(CGameCtnBlock n, GbxReader r)
        {
            n.BlockModel = r.ReadIdent();
            n.Direction = (Direction)r.ReadByte();
            n.Coord = r.ReadByte3();

            var flags = r.ReadInt32();
            if ((flags & (1 << 27)) != 0)
            {
                r.ReadNodeRef(ref n.skinRef);
            }
            n.Flags = 0;
            n.Variant = (byte)(flags & 0xFFF);
            n.SubVariant = (byte)Math.Min((flags >> 12) & 0xFFF, SubVariantMax);
            n.Flags |= (flags >> 12) & 0xF000;
        }

        public override void Write(CGameCtnBlock n, GbxWriter w)
        {
            w.Write(n.BlockModel);
            w.Write((byte)n.Direction);
            w.Write((Byte3)n.Coord);

            var flags =
                (n.Variant & 0xFFF) |
                ((n.SubVariant & 0xFFF) << 12) |
                ((n.Flags & 0xF000) << 12);

            // in case of private value desync
            if ((n.skinRef?.Kind ?? NodeRefKind.None) != NodeRefKind.None)
            {
                flags |= 1 << 27;
            }

            w.Write(flags);
            if ((n.skinRef?.Kind ?? NodeRefKind.None) != NodeRefKind.None)
            {
                w.WriteNodeRef(n.skinRef);
            }
        }
    }

    public override string ToString()
    {
        return $"{nameof(CGameCtnBlock)}: {Name} {coord}";
    }

    private bool IsFlagBitSet(int bit) => (flags & (1 << bit)) != 0;

    private void SetFlagBit(int bit, bool value)
    {
        if (value)
        {
            flags |= 1 << bit;
        }
        else
        {
            flags &= ~(1 << bit);
        }
    }

    private void SetFlagBitAndObject<T>(int bit, ref T? obj, T? value)
    {
        if (value is null)
        {
            flags &= ~(1 << bit);
            obj = default;
            return;
        }

        flags |= 1 << bit;
        obj = value;
    }

    [ChunkGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class Chunk03057002;

    public partial class SSquareCardEventIds
    {
        [Obsolete("Use Version instead.")]
        public int U01 { get => Version; set => Version = value; }

        [Obsolete("Use BlockUnitIndex instead.")]
        public int U02 { get => BlockUnitIndex; set => BlockUnitIndex = value; }

        [Obsolete("Use CardEventIds instead.")]
        public Ident[]? U03 { get => CardEventIds; set => CardEventIds = value; }
    }
}
