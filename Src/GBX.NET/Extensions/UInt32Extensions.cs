namespace GBX.NET.Extensions;

internal static class UInt32Extensions
{
    public static int ToSteerValue(this uint data)
    {
        var dir = (data >> 16) & 0xFF;
        var val = (int)(data & 0xFFFF);

        return dir switch
        {
            0xFF => ushort.MaxValue + 1 - val,
            1 => -ushort.MaxValue - 1,
            _ => val * -1 * (int)(dir + 1)
        };
    }
    
    public static int ToGasValue(this uint data)
    {
        return -ToSteerValue(data);
    }
}
