namespace GBX.NET.Inputs;

public readonly partial record struct FakeFinishLine(TimeInt32 Time, uint Data) : IInputState
{
    public bool Pressed => Data != 0;

    public FakeFinishLine(TimeInt32 Time, bool Pressed) : this(Time, Pressed ? 1u : 0u) { }
}