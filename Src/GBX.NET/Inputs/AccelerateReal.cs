namespace GBX.NET.Inputs;

public readonly record struct AccelerateReal(TimeInt32 Time, int Value) : IInputReal
{
    public float NormalizedValue => InputRealExtensions.GetValue(this);
}