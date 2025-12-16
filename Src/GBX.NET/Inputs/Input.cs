namespace GBX.NET.Inputs;

internal static class Input
{
    public static IInput Parse(TimeInt32 time, string name, uint data) => name switch
    {
        "_FakeDontInverseAxis" => new FakeDontInverseAxis(time, data != 0),
        "_FakeFinishLine" => new FakeFinishLine(time, data),
        "_FakeIsRaceRunning" => new FakeIsRaceRunning(time, data),
        "Accelerate" => new Accelerate(time, data != 0),
        "AccelerateReal" => new AccelerateReal(time, data.ToGasValue()),
        "Brake" => new Brake(time, data != 0),
        "BrakeReal" => new BrakeReal(time, data.ToGasValue()),
        "Gas" => new Gas(time, data.ToGasValue()),
        "Horn" => new Horn(time, data != 0),
        "Respawn" => new Respawn(time, data != 0),
        "Steer" => new Steer(time, data.ToSteerValue()),
        "SteerLeft" => new SteerLeft(time, data != 0),
        "SteerRight" => new SteerRight(time, data != 0),
        _ => new UnknownInput(time, name, data)
    };
}
