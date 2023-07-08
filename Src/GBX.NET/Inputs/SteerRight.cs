namespace GBX.NET.Inputs;

public readonly partial record struct SteerRight(TimeInt32 Time, bool Pressed) : IInputState
{
    
}