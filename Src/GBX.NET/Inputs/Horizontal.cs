namespace GBX.NET.Inputs;

public readonly record struct Horizontal(TimeInt32 Time, byte Pressed) : IInput
{
    
}