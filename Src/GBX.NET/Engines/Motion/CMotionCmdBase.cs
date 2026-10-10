namespace GBX.NET.Engines.Motion;

public partial class CMotionCmdBase
{
    private uint LegacyPeriodInMilliseconds => unchecked((uint)(LegacyPeriod / LegacySpeed));

    private float LegacyNormalizedPhase => LegacyPhase / (float)LegacyPeriod;
}
