namespace GBX.NET.Inputs;

public readonly record struct SteerTM2020(TimeInt32 Time, sbyte Value) : IInputSteer
{
    public float NormalizedValue => GetValue();

    public float GetValue()
    {
        return Value / 127f;
    }
}