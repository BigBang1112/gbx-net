namespace GBX.NET.Inputs;

public interface IInputReal : IInput
{
    int Value { get; }
    float NormalizedValue { get; }
}
