namespace GBX.NET.Engines.Control;

public partial class CControlContainer
{
    internal bool HasFrameChildLocations => this is CControlFrame;
}
