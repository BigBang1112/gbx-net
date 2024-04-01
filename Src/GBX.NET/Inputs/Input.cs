namespace GBX.NET.Inputs;

internal static class Input
{
    public static IInput Parse(TimeInt32 time, string name, uint data) => name switch
    {
        "_FakeDontInverseAxis" => new FakeDontInverseAxis(time, data != 0),
        "_FakeFinishLine" => new FakeFinishLine(time, data != 0),
        "_FakeIsRaceRunning" => new FakeIsRaceRunning(time, data != 0),
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

    public static string GetName(IInput input) => input switch
    {
        FakeDontInverseAxis => "_FakeDontInverseAxis",
        FakeFinishLine => "_FakeFinishLine",
        FakeIsRaceRunning => "_FakeIsRaceRunning",
        Accelerate => nameof(Accelerate),
        AccelerateReal => nameof(AccelerateReal),
        Brake => nameof(Brake),
        BrakeReal => nameof(BrakeReal),
        Gas => nameof(Gas),
        Horn => nameof(Horn),
        Respawn => nameof(Respawn),
        Steer => nameof(Steer),
        SteerLeft => nameof(SteerLeft),
        SteerRight => nameof(SteerRight),
        _ => throw new ArgumentException($"Unknown input type: {input.GetType()}.", nameof(input))
    };

    public static uint GetData(IInput input) => input switch
    {
        IInputState state => (uint)(state.Pressed ? 128 : 0),
        AccelerateReal accelerateReal => accelerateReal.Value.FromGasValue(),
        BrakeReal brakeReal => brakeReal.Value.FromGasValue(),
        Steer steer => steer.Value.FromSteerValue(),
        _ => 0
    };

    private static int ToSteerValue(this uint data)
    {
        var dir = (data >> 16) & 0xFF;
        var val = (int)(data & 0xFFFF);

        return dir switch
        {
            0xFF => ushort.MaxValue + 1 - val,
            1 => -ushort.MaxValue - 1,
            _ => val * -1 * (int)(dir + 1)
        };
    }

    private static uint FromSteerValue(this int value)
    {
        if (value > 0)
        {
            return (uint)(0xFF0000 | (ushort.MaxValue + 1 - value));
        }

        if (value < 0)
        {
            return (uint)(0x010000 | (value & 0xFFFF));
        }

        return 0;
    }

    private static uint FromGasValue(this int value)
    {
        return FromSteerValue(-value);
    }

    private static int ToGasValue(this uint data)
    {
        return -ToSteerValue(data);
    }
}
