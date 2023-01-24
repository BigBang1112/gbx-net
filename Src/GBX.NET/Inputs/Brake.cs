namespace GBX.NET.Inputs;

public readonly record struct Brake(TimeInt32 Time, bool Pressed) : IInputState
{
    
}