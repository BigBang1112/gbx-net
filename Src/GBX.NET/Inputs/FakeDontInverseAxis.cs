namespace GBX.NET.Inputs;

public readonly record struct FakeDontInverseAxis(TimeInt32 Time, bool Pressed) : IInputState
{

}