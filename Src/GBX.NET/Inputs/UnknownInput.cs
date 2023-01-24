namespace GBX.NET.Inputs;

public readonly record struct UnknownInput(TimeInt32 Time, string Name, uint Data) : IInput
{

}
