namespace GBX.NET.Inputs;

public readonly partial record struct GiveUp(TimeInt32 Time, bool Pressed) : IInputState
{
    
}