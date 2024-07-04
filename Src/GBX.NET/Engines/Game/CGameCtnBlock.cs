using GBX.NET.Interfaces.Game;

namespace GBX.NET.Engines.Game;

[ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
public partial class CGameCtnBlock : IGameCtnBlockTM10, IGameCtnBlockTMSX, IGameCtnBlockTMF, IGameCtnBlockMP4, IGameCtnBlockTM2020
{
    private const int GroundBit = 12;
    private const int ClipBit = 13;
    private const int SkinnableBit = 15;
    private const int WaypointBit = 20;
    private const int GhostBit = 28;
    private const int FreeBit = 29;

    private const int VariantMax = 63;
    private const int SubVariantOffset = 6;
    private const int SubVariantMax = 63;

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

    private int flags;
    /// <summary>
    /// Flags of the block. If the blocks version is 0, this value can be presented as <see cref="short"/>.
    /// </summary>
    public int Flags { get => flags; set => flags = value; }

    [Obsolete]
    public bool HasFlags => flags != -1;

    /// <summary>
    /// Variant of the block. Taken from flags. Value range of 0-63.
    /// </summary>
    public byte? Variant
    {
        get => (byte)(flags & VariantMax);
        set => flags = (flags & ~VariantMax) | (value ?? 0);
    }

    /// <summary>
    /// Subvariant of the block. Taken from flags. Value range of 0-63. Always 63 when <see cref="IsClip"/> is true.
    /// </summary>
    public byte? SubVariant
    {
        get => (byte?)((flags >> SubVariantOffset) & SubVariantMax);
        set => flags = (flags & ~(SubVariantMax << SubVariantOffset)) | ((value ?? 0) << SubVariantOffset);
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
    /// If the block is considered as clip. Taken from flags.
    /// </summary>
    public bool IsClip
    {
        get => IsFlagBitSet(ClipBit);
        set => SetFlagBit(ClipBit, value);
    }

    private CGameCtnBlockSkin? skin;
    /// <summary>
    /// Used skin on the block.
    /// </summary>
    public CGameCtnBlockSkin? Skin
    {
        get => skin;
        set
        {
            if (value is null && string.IsNullOrEmpty(Author)) // it may be needed to have this complex set on Author prop too
            {
                flags &= ~(1 << SkinnableBit);
            }

            flags |= 1 << SkinnableBit;
            skin = value;
        }
    }

    /// <summary>
    /// Taken from flags.
    /// </summary>
    public bool Bit17
    {
        get => IsFlagBitSet(17);
        set => SetFlagBit(17, value);
    }

    private CGameWaypointSpecialProperty? waypointSpecialProperty;
    /// <summary>
    /// Additional block parameters.
    /// </summary>
    public CGameWaypointSpecialProperty? WaypointSpecialProperty
    {
        get => waypointSpecialProperty;
        set => SetFlagBitAndObject(WaypointBit, ref waypointSpecialProperty, value);
    }

    /// <summary>
    /// Determines the hill ground variant in TM®. Taken from flags.
    /// </summary>
    public bool Bit21
    {
        get => IsFlagBitSet(21);
        set => SetFlagBit(21, value);
    }

    public bool IsGhost
    {
        get => IsFlagBitSet(GhostBit);
        set => SetFlagBit(GhostBit, value);
    }

    /// <summary>
    /// If this block is a free block. Feature available since TM2020.
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
    public Vec3? PitchYawRoll { get; set; }

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

    byte IGameCtnBlockTM10.Variant { get => Variant.GetValueOrDefault(); set => Variant = value; }
    byte IGameCtnBlockTMSX.Variant { get => Variant.GetValueOrDefault(); set => Variant = value; }
    byte IGameCtnBlockTMSX.SubVariant { get => SubVariant.GetValueOrDefault(); set => SubVariant = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnBlock"/>. Adding chunks is not needed (only when creating blocks for TM1.0 from 2003).
    /// </summary>
    public CGameCtnBlock()
    {
        
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

    [ArchiveGenerationOptions( StructureKind = StructureKind.SeparateReadAndWrite )]
    public partial class SSquareCardEventIds;
}
