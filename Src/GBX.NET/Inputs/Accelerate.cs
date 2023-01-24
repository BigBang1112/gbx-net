namespace GBX.NET.Inputs;

public readonly record struct Accelerate(TimeInt32 Time, bool Enabled) : IInputState
{
    
}