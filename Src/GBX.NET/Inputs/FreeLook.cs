namespace GBX.NET.Inputs;

public readonly record struct FreeLook(TimeInt32 Time, bool Pressed) : IInputState
{

}