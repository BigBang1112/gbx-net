namespace GBX.NET.Inputs;

public readonly record struct HornTM2020(TimeInt32 Time, bool Enabled) : IInputState
{

}