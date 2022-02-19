namespace GBX.NET.Engines.Game;

public partial class CGameCtnGhost
{
    /// <summary>
    /// Checkpoint timestamp driven by the ghost.
    /// </summary>
    /// <param name="Time">Time of the checkpoint.</param>
    /// <param name="StuntsScore">Amount of stunt points when reaching this checkpoint. This is very often 0 in TM2 replay.</param>
    public readonly record struct Checkpoint(TimeInt32? Time, int StuntsScore = 0)
    {
        public override string ToString()
        {
            return $"{Time} ({StuntsScore})";
        }
    }
}
