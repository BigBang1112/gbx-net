namespace GBX.NET.Inputs;

public interface IInput
{
    TimeInt32 Time { get; }

    /// <summary>
    /// Recreates the input with a new timestamp and returns it.
    /// </summary>
    /// <param name="time">New time.</param>
    /// <returns>Recreated input.</returns>
    IInput WithTime(TimeInt32 time);
}
