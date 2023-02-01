namespace GBX.NET.Inputs;

public readonly record struct Fly(TimeInt32 Time, bool Pressed) : IInputState
{

}