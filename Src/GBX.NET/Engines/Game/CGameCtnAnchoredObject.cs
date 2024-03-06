namespace GBX.NET.Engines.Game;

public partial class CGameCtnAnchoredObject
{
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

    public override string ToString() => $"CGameCtnAnchoredObject: {itemModel}";
}
