namespace GBX.NET.Inputs;

public readonly partial record struct ActionSlot(TimeInt32 Time, byte Slot, bool Pressed) : IInputState
{
    
}