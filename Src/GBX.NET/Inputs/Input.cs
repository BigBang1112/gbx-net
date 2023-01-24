namespace GBX.NET.Inputs;

internal static class Input
{
    public static IInput Parse(TimeInt32 time, string name, uint data) => name switch
    {
        "_FakeDontInverseAxis" => new FakeDontInverseAxis(time, data != 0),
        "_FakeFinishLine" => new FakeFinishLine(time, data != 0),
        "_FakeIsRaceRunning" => new FakeIsRaceRunning(time, data != 0),
        "Accelerate" => new Accelerate(time, data != 0),
        "AccelerateReal" => new AccelerateReal(time, data.ToInputValue()),
        "Brake" => new Brake(time, data != 0),
        "BrakeReal" => new BrakeReal(time, data.ToInputValue()),
        "Gas" => new Gas(time, data.ToInputValue()),
        "Horn" => new Horn(time),
        "Respawn" => new Respawn(time),
        "Steer" => new Steer(time, data.ToInputValue()),
        "SteerLeft" => new SteerLeft(time, data != 0),
        "SteerRight" => new SteerRight(time, data != 0),
        _ => new UnknownInput(time, name, data)
    };
}
