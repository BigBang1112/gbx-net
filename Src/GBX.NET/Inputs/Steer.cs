namespace GBX.NET.Inputs;

public readonly partial record struct Steer(TimeInt32 Time, int Value) : IInputReal, IInputSteer
{
    public float NormalizedValue => GetValue();

    public float GetValue()
    {
        return InputRealExtensions.GetValue(this);
    }
}