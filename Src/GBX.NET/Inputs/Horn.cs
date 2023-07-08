namespace GBX.NET.Inputs;

public readonly partial record struct Horn(TimeInt32 Time, bool Pressed) : IInputState
{

}