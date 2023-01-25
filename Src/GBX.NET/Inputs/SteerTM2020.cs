namespace GBX.NET.Inputs;

public readonly record struct SteerTM2020(TimeInt32 Time, sbyte Value) : IInput
{
    public float GetValue()
    {
        return Value / 127f;
    }
}