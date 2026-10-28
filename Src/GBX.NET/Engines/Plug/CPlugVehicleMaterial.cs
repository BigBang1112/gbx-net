namespace GBX.NET.Engines.Plug;

public partial class CPlugVehicleMaterial
{
    [Obsolete("Use M6_MaterialHeightDetail_Scale instead.")]
    public Vec2 MaterialHeightDetailScale { get => M6_MaterialHeightDetail_Scale; set => M6_MaterialHeightDetail_Scale = value; }
}
