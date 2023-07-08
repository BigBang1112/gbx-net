namespace GBX.NET.Inputs;

public readonly partial record struct Jump(TimeInt32 Time, bool Pressed) : IInputState
{

}
