namespace GBX.NET.Engines.Plug;

public partial class CPlugMaterialColorTargetTable
{
    [Obsolete("Use LegacyClassicColors instead.")]
    public int[]? LegacyColors1 { get => LegacyClassicColors; set => LegacyClassicColors = value!; }

    [Obsolete("Use LegacyClassicColorBlindColors instead.")]
    public int[]? LegacyColors2 { get => LegacyClassicColorBlindColors; set => LegacyClassicColorBlindColors = value!; }

    [Obsolete("Use LegacyStuntColors instead.")]
    public int[]? LegacyColors3 { get => LegacyStuntColors; set => LegacyStuntColors = value!; }

    [Obsolete("Use LegacyStuntColorBlindColors instead.")]
    public int[]? LegacyColors4 { get => LegacyStuntColorBlindColors; set => LegacyStuntColorBlindColors = value!; }

    [Obsolete("Use ColorTargets instead.")]
    public ColorPair[]? Colors { get => ColorTargets; set => ColorTargets = value; }

    public partial class ColorPair
    {
        [Obsolete("Use Colors instead.")]
        public int[]? Colors1 { get => Colors; set => Colors = value!; }

        [Obsolete("Use ColorBlindColors instead.")]
        public int[]? Colors2 { get => ColorBlindColors; set => ColorBlindColors = value!; }
    }
}
