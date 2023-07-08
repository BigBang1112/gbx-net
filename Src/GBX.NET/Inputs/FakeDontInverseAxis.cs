namespace GBX.NET.Inputs;

public readonly partial record struct FakeDontInverseAxis(TimeInt32 Time, bool Pressed) : IInputState
{
    
}