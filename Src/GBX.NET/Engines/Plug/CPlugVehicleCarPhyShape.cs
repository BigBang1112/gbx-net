namespace GBX.NET.Engines.Plug;

public partial class CPlugVehicleCarPhyShape
{
    [Obsolete("Use CarVsCarShape_Box instead.")]
    public BoxAligned CarVsCarShapeBox { get => CarVsCarShape_Box; set => CarVsCarShape_Box = value; }
}
