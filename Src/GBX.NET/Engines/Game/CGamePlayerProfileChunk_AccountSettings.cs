namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_AccountSettings
{
    public partial class SPlayerTagsConfig
    {
        [Obsolete("Use Version instead.")]
        public int U01 { get => Version; set => Version = value; }

        [Obsolete("Use TagDisplayList instead.")]
        public int[]? U02 { get => TagDisplayList; set => TagDisplayList = value; }
    }

    public bool LoginValidated
    {
        get => BitHelper.GetBit(flags, 0);
        set => flags = BitHelper.SetBit(flags, 0, value);
    }

    public bool RememberOnlinePassword
    {
        get => BitHelper.GetBit(flags, 1);
        set => flags = BitHelper.SetBit(flags, 1, value);
    }

    public bool AutoConnect
    {
        get => BitHelper.GetBit(flags, 2);
        set => flags = BitHelper.SetBit(flags, 2, value);
    }

    public bool AskForAccountConversion
    {
        get => BitHelper.GetBit(flags, 3);
        set => flags = BitHelper.SetBit(flags, 3, value);
    }

    public bool UnlockAllCheats
    {
        get => BitHelper.GetBit(flags2, 0);
        set => flags2 = BitHelper.SetBit(flags2, 0, value);
    }

    public bool FriendsCheat
    {
        get => BitHelper.GetBit(flags2, 1);
        set => flags2 = BitHelper.SetBit(flags2, 1, value);
    }
}
