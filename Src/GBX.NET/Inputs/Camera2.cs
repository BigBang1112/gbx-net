namespace GBX.NET.Inputs;

public readonly record struct Camera2(TimeInt32 Time, bool Pressed) : IInputState
{

}
