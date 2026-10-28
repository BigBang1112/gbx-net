namespace GBX.NET.Engines.Plug;

public partial class CPlugVehicleCameraRace3Model
{
    private PlugCurve? slerpSpeedModulationFromSpeed;
    private PlugCurve? lookAtFactorFromUpSpeedRatio;
    private PlugCurve? flyingLookDownFactorFromSpeedRatio;

    public CPlugVehicleCameraRace3Model()
    {
        Shake = new CPlugCamShakeModel();
        SlerpSpeedModulationFromSpeed = new PlugCurve
        {
            Count = 6,
            Values = [new(-100, 1.5f), new(-50, 1.2f), new(0, 1), new(250, 1.5f), new(500, 2), new(700, 3)]
        };
        LookAtFactorFromUpSpeedRatio = new PlugCurve
        {
            Count = 7,
            Values = [new(0, 0), new(1, 0), new(5, -0.3f), new(10, -0.35f), new(20, -0.4f), new(40, -0.45f), new(60, -0.5f)]
        };
        FlyingLookDownFactorFromSpeedRatio = new PlugCurve
        {
            Count = 9,
            Values = [new(0, 0), new(1, 0), new(1.1f, 0.03f), new(1.3f, 0.1f), new(2, 0.3f), new(10, 0.35f), new(20, 0.4f), new(40, 0.5f), new(60, 0.6f)]
        };
        DeltaRadiusFromSpeedKmh = new PlugCurve { Count = 1, Values = [new(0, 0)] };
    }
}
