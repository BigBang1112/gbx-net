namespace GBX.NET.Engines.Control;

public partial class CControlContainer
{
    internal bool HasFrameChildLocations => this is CControlFrame;
    internal bool HasGridChildLocations => this is CControlGrid;
    internal bool HasMenuFrameChildVersion => this is Engines.Game.CGameMenuFrame;
}
