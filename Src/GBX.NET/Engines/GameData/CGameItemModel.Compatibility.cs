namespace GBX.NET.Engines.GameData;

public partial class CGameItemModel
{
    [Obsolete("Use ItemType instead.")]
    public EItemType ItemTypeE { get => ItemType; set => ItemType = value; }

    [Obsolete("Use NadeoSkinsFids instead.")]
    public NodeRefList<CMwNod?>? NadeoSkinFids { get => NadeoSkinsFids; set => NadeoSkinsFids = value; }

    [Obsolete("Use IconRef instead.")]
    public new string? IconFid { get => IconRef; set => IconRef = value; }
}
