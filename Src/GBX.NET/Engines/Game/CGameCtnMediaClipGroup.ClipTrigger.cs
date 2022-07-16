namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaClipGroup
{
    public record ClipTrigger(CGameCtnMediaClip Clip, Trigger Trigger)
    {
        public override string ToString()
        {
            return $"{Clip} with trigger";
        }
    }

    public class Trigger
    {
        public Int3[] Coords { get; set; } = Array.Empty<Int3>();
        public int U01 { get; set; }
        public int U02 { get; set; }
        public int U03 { get; set; }
        public int U04 { get; set; }
        public ECondition Condition { get; set; }
        public float ConditionValue { get; set; }
    }
}
