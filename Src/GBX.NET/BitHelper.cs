namespace GBX.NET;

/// <summary>
/// Provides utility methods for bit manipulation operations.
/// </summary>
internal static class BitHelper
{
    /// <summary>
    /// Gets the value of a specific bit in a 64-bit unsigned integer.
    /// </summary>
    /// <param name="flags">The value to examine.</param>
    /// <param name="bitPosition">The zero-based position of the bit to get (0-63).</param>
    /// <returns>True if the bit is set, false otherwise.</returns>
    public static bool GetBit(ulong flags, int bitPosition)
    {
        return (flags >> bitPosition & 1) != 0;
    }

    /// <summary>
    /// Gets the value of a specific bit in a 32-bit unsigned integer.
    /// </summary>
    /// <param name="flags">The value to examine.</param>
    /// <param name="bitPosition">The zero-based position of the bit to get (0-31).</param>
    /// <returns>True if the bit is set, false otherwise.</returns>
    public static bool GetBit(uint flags, int bitPosition)
    {
        return (flags >> bitPosition & 1) != 0;
    }

    /// <summary>
    /// Gets the value of a specific bit in a 16-bit unsigned integer.
    /// </summary>
    /// <param name="flags">The value to examine.</param>
    /// <param name="bitPosition">The zero-based position of the bit to get (0-15).</param>
    /// <returns>True if the bit is set, false otherwise.</returns>
    public static bool GetBit(ushort flags, int bitPosition)
    {
        return (flags >> bitPosition & 1) != 0;
    }

    /// <summary>
    /// Sets or clears a specific bit in a 64-bit unsigned integer.
    /// </summary>
    /// <param name="value">The value to modify.</param>
    /// <param name="bitPosition">The zero-based position of the bit to set (0-63).</param>
    /// <param name="enable">True to set the bit, false to clear it.</param>
    /// <returns>The modified value.</returns>
    public static ulong SetBit(ulong value, int bitPosition, bool enable)
    {
        var mask = 1ul << bitPosition;
        return enable ? (value | mask) : (value & ~mask);
    }

    /// <summary>
    /// Sets or clears a specific bit in a 32-bit unsigned integer.
    /// </summary>
    /// <param name="value">The value to modify.</param>
    /// <param name="bitPosition">The zero-based position of the bit to set (0-31).</param>
    /// <param name="enable">True to set the bit, false to clear it.</param>
    public static uint SetBit(uint value, int bitPosition, bool enable)
    {
        var mask = 1u << bitPosition;
        return enable ? (value | mask) : (value & ~mask);
    }

    /// <summary>
    /// Sets or clears a specific bit in a 16-bit unsigned integer.
    /// </summary>
    /// <param name="value">The value to modify.</param>
    /// <param name="bitPosition">The zero-based position of the bit to set (0-15).</param>
    /// <param name="enable">True to set the bit, false to clear it.</param>
    public static ushort SetBit(ushort value, int bitPosition, bool enable)
    {
        var mask = (ushort)(1 << bitPosition);
        return enable ? (ushort)(value | mask) : (ushort)(value & ~mask);
    }

    /// <summary>
    /// Gets a range of bits from a 64-bit unsigned integer.
    /// </summary>
    /// <param name="value">The value to examine.</param>
    /// <param name="startBit">The zero-based starting position of the bit range.</param>
    /// <param name="bitCount">The number of bits in the range.</param>
    /// <returns>The value of the specified bit range.</returns>
    public static int GetBitRange(ulong value, int startBit, int bitCount)
    {
        var mask = (1UL << bitCount) - 1;
        return (int)(value >> startBit & mask);
    }

    /// <summary>
    /// Gets a range of bits from a 32-bit unsigned integer.
    /// </summary>
    /// <param name="value">The value to examine.</param>
    /// <param name="startBit">The zero-based starting position of the bit range.</param>
    /// <param name="bitCount">The number of bits in the range.</param>
    /// <returns>The value of the specified bit range.</returns>
    public static int GetBitRange(uint value, int startBit, int bitCount)
    {
        var mask = (1U << bitCount) - 1;
        return (int)(value >> startBit & mask);
    }

    /// <summary>
    /// Gets a range of bits from a 16-bit unsigned integer.
    /// </summary>
    /// <param name="value">The value to examine.</param>
    /// <param name="startBit">The zero-based starting position of the bit range.</param>
    /// <param name="bitCount">The number of bits in the range.</param>
    /// <returns>The value of the specified bit range.</returns>
    public static int GetBitRange(ushort value, int startBit, int bitCount)
    {
        var mask = (1U << bitCount) - 1;
        return (int)(value >> startBit & mask);
    }

    /// <summary>
    /// Sets a range of bits in a 64-bit unsigned integer.
    /// </summary>
    /// <param name="value">The value to modify.</param>
    /// <param name="startBit">The zero-based starting position of the bit range.</param>
    /// <param name="bitCount">The number of bits in the range.</param>
    /// <param name="newValue">The new value for the bit range.</param>
    /// <returns>The modified value.</returns>
    public static ulong SetBitRange(ulong value, int startBit, int bitCount, int newValue)
    {
        var mask = ((1UL << bitCount) - 1) << startBit;
        return (value & ~mask) | (((ulong)newValue & ((1UL << bitCount) - 1)) << startBit);
    }

    /// <summary>
    /// Sets a range of bits in a 32-bit unsigned integer.
    /// </summary>
    /// <param name="value">The value to modify.</param>
    /// <param name="startBit">The zero-based starting position of the bit range.</param>
    /// <param name="bitCount">The number of bits in the range.</param>
    /// <param name="newValue">The new value for the bit range.</param>
    /// <returns>The modified value.</returns>
    public static uint SetBitRange(uint value, int startBit, int bitCount, int newValue)
    {
        var mask = ((1U << bitCount) - 1) << startBit;
        return (value & ~mask) | (((uint)newValue & ((1U << bitCount) - 1)) << startBit);
    }

    /// <summary>
    /// Sets a range of bits in a 16-bit unsigned integer.
    /// </summary>
    /// <param name="value">The value to modify.</param>
    /// <param name="startBit">The zero-based starting position of the bit range.</param>
    /// <param name="bitCount">The number of bits in the range.</param>
    /// <param name="newValue">The new value for the bit range.</param>
    /// <returns>The modified value.</returns>
    public static ushort SetBitRange(ushort value, int startBit, int bitCount, int newValue)
    {
        var mask = (ushort)(((1U << bitCount) - 1) << startBit);
        return (ushort)((value & ~mask) | (((ushort)newValue & (ushort)((1U << bitCount) - 1)) << startBit));
    }
}
