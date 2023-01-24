namespace GBX.NET.Inputs;

public readonly record struct SteerRight(TimeInt32 Time, bool Pressed) : IInputState
{
    
}