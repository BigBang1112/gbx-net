namespace GBX.NET.Inputs;

public readonly record struct Action(TimeInt32 Time, bool Pressed) : IInputState
{

}