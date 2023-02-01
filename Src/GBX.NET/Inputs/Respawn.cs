namespace GBX.NET.Inputs;

public readonly record struct Respawn(TimeInt32 Time, bool Pressed) : IInputState
{
    
}