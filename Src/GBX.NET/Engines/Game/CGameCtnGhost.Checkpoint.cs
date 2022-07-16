namespace GBX.NET.Engines.Game;

public partial class CGameCtnGhost
{
    /// <summary>
    /// Checkpoint timestamp driven by the ghost.
    /// </summary>
    /// <param name="Time">Time of the checkpoint.</param>
    /// <param name="StuntsScore">Amount of stunt points when reaching this checkpoint. This is very often 0 in TM2 replay, and not present at all in very old TM versions.</param>
    /// <param name="Speed">The speed the vehicle had when crossing the checkpoint. Only present in the oldest TM versions.</param>
    public readonly record struct Checkpoint(TimeInt32? Time, int StuntsScore = 0, float? Speed = null)
    {
        public override string ToString()
        {
            return $"{Time} ({(Speed.HasValue ? $"{Speed}km/h, " : "")}{StuntsScore} pts.)";
        }
    }
}
