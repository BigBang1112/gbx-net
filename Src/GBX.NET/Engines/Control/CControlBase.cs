namespace GBX.NET.Engines.Control;

public partial class CControlBase
{
    public bool HasHmsItem { get; set; }

    // The stack bit is a class capability preserved from the native constructor,
    // rather than one of the flags restored from the serialized mask.
    internal bool HasControlStack => this is Engines.Game.CGameControlCard
        or Engines.Game.CGamePlaygroundControlScores or Engines.Game.CGamePlaygroundControlMessages
        or CControlPager or Engines.TrackMania.CTrackManiaControlCheckPointList
        || this is not (CControlContainer or CControlLabel or CControlQuad);
}
