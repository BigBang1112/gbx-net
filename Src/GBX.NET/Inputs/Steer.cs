namespace GBX.NET.Inputs;

public readonly record struct Steer(TimeInt32 Time, int Value) : IInputReal, IInputSteer
{
    public float GetValue()
    {
        return InputRealExtensions.GetValue(this);
    }
}