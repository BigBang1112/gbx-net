namespace GBX.NET.Inputs;

public readonly partial record struct Action(TimeInt32 Time, bool Pressed) : IInputState
{
    
}