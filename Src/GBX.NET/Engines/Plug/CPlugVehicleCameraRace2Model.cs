namespace GBX.NET.Engines.Plug;

public partial class CPlugVehicleCameraRace2Model
{
    private PlugCurve? lookAtFactorFromUpSpeedRatio;
    private PlugCurve? yawFromSpeed;
    private PlugCurve? rollFromSpeed;

    public CPlugVehicleCameraRace2Model()
    {
        Shake = new CPlugCamShakeModel();
        LookAtFactorFromUpSpeedRatio = new PlugCurve
        {
            Count = 7,
            Values = [new(0, 0), new(1, 0), new(5, -0.05f), new(10, -0.1f), new(20, -0.3f), new(40, -0.45f), new(60, -0.6f)]
        };
        YawFromSpeed = new PlugCurve
        {
            Count = 6,
            Values = [new(-10, 2), new(0, 0), new(50, 2), new(100, 4), new(200, 8), new(400, 14)]
        };
        RollFromSpeed = new PlugCurve
        {
            Count = 6,
            Values = [new(-10, 1), new(0, 0), new(50, 1), new(100, 2), new(200, 2.5f), new(400, 3)]
        };
    }
}
