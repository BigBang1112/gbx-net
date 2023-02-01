namespace GBX.NET.Inputs;

/// <summary>
/// Special input case. <paramref name="Pressed"/> indicates state of the menu, not the key press.
/// </summary>
/// <param name="Time"></param>
/// <param name="Pressed"></param>
public readonly record struct Menu(TimeInt32 Time, bool Pressed) : IInputState
{

}