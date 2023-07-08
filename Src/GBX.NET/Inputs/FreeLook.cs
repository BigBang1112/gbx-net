namespace GBX.NET.Inputs;

public readonly partial record struct FreeLook(TimeInt32 Time, bool Pressed) : IInputState
{

}