namespace GBX.NET.Inputs;

public readonly partial record struct BrakeReal(TimeInt32 Time, int Value) : IInputReal
{
    public float NormalizedValue => InputRealExtensions.GetValue(this);
}