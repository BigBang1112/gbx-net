namespace GBX.NET.Inputs;

public readonly record struct Use(TimeInt32 Time, byte Num, bool Pressed) : IInputState
{

}