namespace GBX.NET.Inputs;

public readonly record struct Walk(TimeInt32 Time, EWalk Pressed) : IInput
{
    
}