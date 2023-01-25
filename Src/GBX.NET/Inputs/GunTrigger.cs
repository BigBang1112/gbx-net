namespace GBX.NET.Inputs;

public readonly record struct GunTrigger(TimeInt32 Time, bool Pressed) : IInputState
{

}
