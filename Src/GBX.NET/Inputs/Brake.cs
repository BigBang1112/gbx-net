namespace GBX.NET.Inputs;

public readonly partial record struct Brake(TimeInt32 Time, bool Pressed) : IInputState
{
    
}