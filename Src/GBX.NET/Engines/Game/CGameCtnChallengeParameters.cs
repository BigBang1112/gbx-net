namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallengeParameters
{
    private int[]? items;
    private CGameCtnGhost?[]? legacyValidateGhosts;

    [Obsolete("This legacy item list is ignored by newer games.")]
    public partial int[]? Items
    {
        get => items;
        set => items = value;
    }

    [Obsolete("This legacy validation ghost list is ignored by newer games.")]
    public partial CGameCtnGhost?[]? LegacyValidateGhosts
    {
        get => legacyValidateGhosts;
        set => legacyValidateGhosts = value;
    }
}
