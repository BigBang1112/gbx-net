namespace GBX.NET.Inputs;

public readonly record struct Jump(TimeInt32 Time, bool Pressed) : IInputState
{

}
