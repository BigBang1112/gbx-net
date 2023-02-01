namespace GBX.NET.Inputs;

public readonly record struct Vertical(TimeInt32 Time, byte Pressed) : IInput
{
    
}