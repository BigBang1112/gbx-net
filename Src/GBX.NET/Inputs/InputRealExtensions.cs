namespace GBX.NET.Inputs;

public static class InputRealExtensions
{
    public static float GetValue(this IInputReal input)
    {
        return input.Value / 65536f;
    }
}
