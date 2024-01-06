namespace GBX.NET.Inputs;

public readonly partial record struct Gas(TimeInt32 Time, int Value) : IInputReal
{
    public float NormalizedValue => this.GetValue();
}