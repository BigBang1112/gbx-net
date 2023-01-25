namespace GBX.NET.Inputs;

public readonly record struct Strafe(TimeInt32 Time, EStrafe Pressed) : IInput
{
    
}