namespace GBX.NET.Inputs;

public readonly record struct SteerLeft(TimeInt32 Time, bool Pressed) : IInputState
{
    
}