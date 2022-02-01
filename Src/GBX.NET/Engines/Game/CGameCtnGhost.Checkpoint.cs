namespace GBX.NET.Engines.Game;

public partial class CGameCtnGhost
{
    /// <summary>
    /// Checkpoint timestamp driven by the ghost.
    /// </summary>
    public struct Checkpoint
    {
        /// <summary>
        /// Time of the checkpoint.
        /// </summary>
        public TimeSpan? Time { get; set; }

        /// <summary>
        /// Amount of stunt points when reaching this checkpoint. This is very often 0 in TM2 replay.
        /// </summary>
        public int StuntsScore { get; set; }

        public Checkpoint(TimeSpan? time, int stuntsScore)
        {
            Time = time;
            StuntsScore = stuntsScore;
        }

        public Checkpoint(TimeSpan? time) : this(time, 0)
        {

        }

        public override string ToString()
        {
            return $"{Time.ToTmString()} ({StuntsScore})";
        }
    }
}
