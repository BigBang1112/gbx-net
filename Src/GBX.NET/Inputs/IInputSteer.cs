namespace GBX.NET.Inputs;

public interface IInputSteer : IInput
{
    float NormalizedValue { get; }
    float GetValue();
}
