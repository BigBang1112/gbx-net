namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallengeParameters
{
    /// <summary>
    /// Recalculates the default gold, silver, and bronze race times from <see cref="AuthorTime"/>.
    /// </summary>
    /// <remarks>
    /// The times are 106%, 120%, and 150% of the author time, advanced to the next whole second
    /// even when the scaled time already falls on a whole second. An unset author time clears all three times.
    /// </remarks>
    public void RecalculateMedalTimes()
    {
        (GoldTime, SilverTime, BronzeTime) = CalculateMedalTimes(AuthorTime);
    }

    internal static (TimeInt32? Gold, TimeInt32? Silver, TimeInt32? Bronze) CalculateMedalTimes(TimeInt32? authorTime)
    {
        var milliseconds = authorTime?.TotalMilliseconds ?? -1;
        if (milliseconds == -1)
        {
            return default;
        }

        return (CalculateMedalTime(milliseconds, 106), CalculateMedalTime(milliseconds, 120), CalculateMedalTime(milliseconds, 150));

        static TimeInt32 CalculateMedalTime(int milliseconds, uint percentage)
        {
            var scaledMilliseconds = unchecked((uint)milliseconds * percentage) / 100;
            return new TimeInt32((int)((scaledMilliseconds / 1000 + 1) * 1000));
        }
    }
}
