namespace GBX.NET.Inputs;

public readonly record struct FakeFinishLine(TimeInt32 Time, bool Pressed) : IInputState
{
    
}