namespace GBX.NET.Inputs;

public readonly partial record struct FakeFinishLine(TimeInt32 Time, bool Pressed) : IInputState
{
    
}