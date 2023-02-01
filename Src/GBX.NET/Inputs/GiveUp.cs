namespace GBX.NET.Inputs;

public readonly record struct GiveUp(TimeInt32 Time, bool Pressed) : IInputState
{
    
}