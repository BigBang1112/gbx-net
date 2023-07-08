namespace GBX.NET.Inputs;

public readonly partial record struct Respawn(TimeInt32 Time, bool Pressed) : IInputState
{
    
}