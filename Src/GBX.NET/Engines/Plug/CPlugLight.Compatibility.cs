using GBX.NET.Engines.Graphic;

namespace GBX.NET.Engines.Plug;

public partial class CPlugLight
{
    [Obsolete("Use GxLightModel instead.")]
    public GxLight? Light { get => GxLightModel; set => GxLightModel = value; }
}
