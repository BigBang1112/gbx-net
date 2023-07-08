namespace GBX.NET.Inputs;

public readonly partial record struct GunTrigger(TimeInt32 Time, bool Pressed) : IInputState
{

}
