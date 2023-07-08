namespace GBX.NET.Inputs;

public readonly partial record struct FakeIsRaceRunning(TimeInt32 Time, bool Pressed) : IInputState
{

}