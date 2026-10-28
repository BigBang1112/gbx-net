using GBX.NET.Engines.Script;
using static GBX.NET.Engines.Script.CScriptTraitsMetadata;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnAnchoredObject
{
    private PackDesc? packDescValue;
    public partial PackDesc? PackDesc
    {
        get => packDescValue;
        set
        {
            packDescValue = value;
            Flags = (short)(value is null ? Flags & ~4 : Flags | 4);
        }
    }

    public bool IsFlying
    {
        get => (Flags & 1) != 0;
        set => Flags = (short)(value ? Flags | 1 : Flags & ~1);
    }

    public bool IsAutoPiloted
    {
        get => (Flags & 2) != 0;
        set => Flags = (short)(value ? Flags | 2 : Flags & ~2);
    }

    public partial class Chunk03101003
    {
        public override void ReadWrite(CGameCtnAnchoredObject n, GbxReaderWriter rw)
        {
            var metadata = rw.Reader is null ? n.WaypointSpecialProperty?.ScriptMetadata ?? new CScriptTraitsMetadata() : null;
            rw.Encapsulated(rw => rw.Node(ref metadata));

            if (rw.Reader is not null && metadata?.Traits.Count > 0 && n.WaypointSpecialProperty?.ScriptMetadata is null)
            {
                var waypoint = n.WaypointSpecialProperty ??= new CGameWaypointSpecialProperty();
                waypoint.ScriptMetadata = metadata;
                if (waypoint.Chunks.Get<CGameWaypointSpecialProperty.Chunk2E009001>() is null)
                {
                    waypoint.CreateChunk<CGameWaypointSpecialProperty.Chunk2E009001>();
                }
            }
        }
    }

    public partial class ScriptParameter
    {
        public ScriptTrait? Value { get; set; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            if (v > 4)
            {
                throw new ChunkVersionNotSupportedException(v);
            }

            rw.String(ref name);

            if (v < 0)
            {
                var legacyType = rw.Int32(rw.Reader is null ? Value?.Type.Type switch
                {
                    EScriptType.Boolean => 0,
                    EScriptType.Integer => 1,
                    EScriptType.Real => 2,
                    EScriptType.Text => 3,
                    _ => throw new NotSupportedException("Legacy item parameters support only Boolean, Integer, Real, and Text.")
                } : 0);

                var type = new ScriptType(legacyType switch
                {
                    0 => EScriptType.Boolean,
                    1 => EScriptType.Integer,
                    2 => EScriptType.Real,
                    3 => EScriptType.Text,
                    _ => throw new NotSupportedException($"Legacy item parameter type {legacyType} is not supported.")
                });

                if (rw.Reader is not null) Value = ReadContents(rw.Reader, type, noContent: false, version: 0);
                if (rw.Writer is not null) WriteContents(rw.Writer, Value!, version: 0);
                return;
            }

            if (rw.Reader is not null)
            {
                var type = ReadType(rw.Reader, v);
                Value = ReadContents(rw.Reader, type, noContent: false, v);
            }

            if (rw.Writer is not null)
            {
                if (Value is null)
                {
                    throw new InvalidOperationException("An item script parameter must have a value.");
                }
                
                WriteType(rw.Writer, Value.Type, v);
                WriteContents(rw.Writer, Value, v);
            }
        }
    }

    /// <summary>
    /// Block that tells when that block gets deleted, this item is deleted with it. Works for TM2020 only.
    /// </summary>
    public CGameCtnBlock? SnappedOnBlock { get; set; }

    /// <summary>
    /// Item that tells when that item gets deleted, this item is deleted with it. Works for TM2020 only but is more modern.
    /// </summary>
    public CGameCtnAnchoredObject? SnappedOnItem { get; set; }

    /// <summary>
    /// Item that tells when that item gets deleted, this item is deleted with it. Works for ManiaPlanet, used to work in the past in TM2020 but now it likely doesn't.
    /// </summary>
    public CGameCtnAnchoredObject? PlacedOnItem { get; set; }

    /// <summary>
    /// Group number that groups items that get deleted together. Works for TM2020 only.
    /// </summary>
    public int? SnappedOnGroup { get; set; }

    /// <summary>
    /// [MapElemColor] Color of the item. Available since TM2020 Royal update.
    /// </summary>
    public DifficultyColor Color { get; set; }

    /// <summary>
    /// Phase of the animation. Available since TM2020 Royal update.
    /// </summary>
    public EPhaseOffset AnimPhaseOffset { get; set; }

    /// <summary>
    /// The second layer of skin. Available since TM2020.
    /// </summary>
    public PackDesc? ForegroundPackDesc { get; set; }

    /// <summary>
    /// [MapElemLmQuality] Lightmap quality setting of the item. Available since TM2020.
    /// </summary>
    public LightmapQuality LightmapQuality { get; set; }

    /// <summary>
    /// Reference to the macroblock that placed this item. In macroblock mode, this item is then part of a selection group. Available since TM2020.
    /// </summary>
    public MacroblockInstance? MacroblockReference { get; set; }

    [Obsolete("Use the correctly-named YawPitchRoll instead. This property doesn't swap Pitch and Yaw and behaves like YawPitchRoll, and it will be removed in 2.3.0.")]
    public Vec3 PitchYawRoll
    {
        get => YawPitchRoll;
        set => YawPitchRoll = value;
    }

    public override string ToString() => $"CGameCtnAnchoredObject: {itemModel}";
}
