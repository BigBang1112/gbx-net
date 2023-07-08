namespace GBX.NET.Inputs;

public readonly partial record struct MouseAccu(TimeInt32 Time, ushort X, ushort Y) : IInput
{

}