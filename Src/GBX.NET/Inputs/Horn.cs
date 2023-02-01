namespace GBX.NET.Inputs;

public readonly record struct Horn(TimeInt32 Time, bool Pressed) : IInputState
{

}