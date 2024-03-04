namespace GBX.NET.Serialization;

internal sealed class GbxReaderLimiter
{
    private int limiterPos;
    private int limiterLimit;

    public int Remaining => limiterLimit - limiterPos;

    internal void Limit(int size)
    {
        limiterPos = 0;

        if (size == 0)
        {
            limiterLimit = 0;
            return;
        }

        limiterLimit = size;
    }

    internal void Unlimit() => Limit(0);

    internal void ThrowIfLimitExceeded(int expectedLengthToRead)
    {
        limiterPos += expectedLengthToRead;

        if (limiterLimit != 0 && limiterPos > limiterLimit)
        {
            throw new Exception("Limit exceeded (expected length to read: " + expectedLengthToRead + ", remaining: " + Remaining + ").");
        }
    }
}
