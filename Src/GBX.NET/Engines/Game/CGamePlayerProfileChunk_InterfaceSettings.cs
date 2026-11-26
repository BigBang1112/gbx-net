namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_InterfaceSettings
{
    private const EInterfaceFlags DefaultInterfaceFlags = (EInterfaceFlags)6;

    [Flags]
    public enum EInterfaceFlags : byte
    {
        None = 0,
        ClickedOnShare = 1,
        EditorHelp = 4
    }

    [Obsolete("Use DynamicLoadScreenIndex instead.")]
    public int U01 { get => DynamicLoadScreenIndex; set => DynamicLoadScreenIndex = value; }

    [Obsolete("Use PainterCustomColors instead.")]
    public Vec3[]? U03 { get => PainterCustomColors; set => PainterCustomColors = value; }

    [Obsolete("Use InterfaceFlags instead.")]
    public byte U04 { get => (byte)InterfaceFlags; set => InterfaceFlags = (EInterfaceFlags)value; }

    public partial class SCampaignSettings
    {
        [Obsolete("Use CampaignId instead.")]
        public string? U02 { get => CampaignId; set => CampaignId = value; }

        [Obsolete("Use Unlocks instead.")]
        public int U03 { get => Unlocks; set => Unlocks = value; }
    }

    public partial class SSoloChallengesCurrentPage
    {
        [Obsolete("Use MapGroupIndex instead.")]
        public int U01 { get => MapGroupIndex; set => MapGroupIndex = value; }

        [Obsolete("Use PageIndex instead.")]
        public int U02 { get => PageIndex; set => PageIndex = value; }
    }
}
