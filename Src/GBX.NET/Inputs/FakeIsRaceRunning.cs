namespace GBX.NET.Inputs;

public readonly record struct FakeIsRaceRunning(TimeInt32 Time, bool Pressed) : IInputState
{

}