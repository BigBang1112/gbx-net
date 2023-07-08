namespace GBX.NET.Inputs;

public readonly partial record struct Accelerate(TimeInt32 Time, bool Pressed) : IInputState
{

}