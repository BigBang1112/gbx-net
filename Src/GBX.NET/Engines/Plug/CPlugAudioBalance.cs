namespace GBX.NET.Engines.Plug;

public partial class CPlugAudioBalance
{
    private const int GroupCount = 14;

    private Group[] mixerGroups = CreateDefaultGroups();
    /// <summary>
    /// Mixer groups in <see cref="EAudioBalanceGroup"/> order, excluding <see cref="EAudioBalanceGroup.Auto"/>.
    /// </summary>
    public partial Group[]? Groups
    {
        get => mixerGroups;
        set
        {
            if (value?.Length > GroupCount)
            {
                throw new ArgumentException($"An audio balance supports at most {GroupCount} groups.", nameof(value));
            }

            if (value is { Length: GroupCount })
            {
                mixerGroups = value;
                return;
            }

            var groups = CreateDefaultGroups();
            if (value is not null)
            {
                Array.Copy(value, groups, value.Length);
            }
            groups[(int)EAudioBalanceGroup.Environment - 1] = new DeepCloneContext().Clone(groups[(int)EAudioBalanceGroup.Ambiance - 1])!;
            mixerGroups = groups;
        }
    }

    private static Group[] CreateDefaultGroups()
    {
        var groups = new Group[GroupCount];
        for (var i = 0; i < groups.Length; i++)
        {
            groups[i] = new Group();
        }
        groups[(int)EAudioBalanceGroup.BackingDirect - 1].Others!.Volume = 0;
        groups[(int)EAudioBalanceGroup.OtherPlayers - 1].Focused!.Volume = 0;
        return groups;
    }

    public partial class Group
    {
        public Group()
        {
            Others = new Line();
            Focused = new Line();
        }

        [Obsolete("Use Others instead.")]
        public Line? U01 { get => Others; set => Others = value; }

        [Obsolete("Use Focused instead.")]
        public Line? U02 { get => Focused; set => Focused = value; }
    }

    public partial class Line
    {
        [Obsolete("Use Volume instead.")]
        public float U01 { get => Volume; set => Volume = value; }

        [Obsolete("Use Pitch instead.")]
        public float U02 { get => Pitch; set => Pitch = value; }

        [Obsolete("Use DopplerFactor instead.")]
        public float U03 { get => DopplerFactor; set => DopplerFactor = value; }

        [Obsolete("Use RolloffFactor instead.")]
        public float U04 { get => RolloffFactor; set => RolloffFactor = value; }

        [Obsolete("Use DistanceFactor instead.")]
        public float U05 { get => DistanceFactor; set => DistanceFactor = value; }
    }
}
