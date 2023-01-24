namespace GBX.NET.Inputs;

public interface IInputState : IInput
{
    bool Pressed { get; }
}
