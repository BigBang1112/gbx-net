namespace GBX.NET.Engines.Control;

public partial class CControlBase
{
    public bool HasHmsItem { get; set; }

    // The stack bit is a class capability preserved from the native constructor,
    // rather than one of the flags restored from the serialized mask.
    internal bool HasControlStack => this is Engines.Game.CGameControlCard
        or Engines.Game.CGamePlaygroundControlScores or Engines.Game.CGamePlaygroundControlMessages
        or CControlPager or CControlList or Engines.TrackMania.CTrackManiaControlCheckPointList
        or CControlTrackManiaScores or CControlTrackManiaBigScores or Engines.TrackMania.CTrackManiaMatchSettingsControlGrid
        || this is not (CControlContainer or CControlLabel or CControlQuad or CControlOverlay);
}
