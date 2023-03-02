namespace GBX.NET.Inputs;

public readonly record struct SteerOld(TimeInt32 Time, float Value) : IInputSteer
{
    public float NormalizedValue => GetValue();

    public float GetValue()
    {
        return Value;
    }
}