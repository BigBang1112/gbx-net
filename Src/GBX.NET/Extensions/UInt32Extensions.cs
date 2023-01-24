namespace GBX.NET.Extensions;

internal static class UInt32Extensions
{
    public static int ToInputValue(this uint data)
    {
        var dir = data >> 24;
        var val = (int)(data & 0xFFFF);

        if (dir == 0xFF)
        {
            return val;
        }

        return val * -1 * (int)(dir + 1);
    }
}
