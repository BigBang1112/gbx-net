namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_AccountSettings
{
    /// <summary>
    /// Selects the native online-save payload, which omits local account data.
    /// This context is supplied by the caller and is not stored in the payload.
    /// </summary>
    public bool IsOnlineSaveArchivation { get; set; }

    [Obsolete("Use OnlineValidationCode instead.")]
    public string? OnlineValidationKey { get => OnlineValidationCode; set => OnlineValidationCode = value; }

    [Obsolete("Use EncryptedKeyHexa instead. The serialized value is Blowfish ciphertext.")]
    public string? RSAPublicKey { get => EncryptedKeyHexa; set => EncryptedKeyHexa = value; }

    [Obsolete("Use LegacyBuddyArchiveVersion instead.")]
    public int U03 { get => LegacyBuddyArchiveVersion; set => LegacyBuddyArchiveVersion = value; }

    [Obsolete("Use ReceivedMessagesSystemTime instead.")]
    public ulong U04 { get => ReceivedMessagesSystemTime; set => ReceivedMessagesSystemTime = value; }

    [Obsolete("Use BuddyArchiveVersion instead.")]
    public int U07 { get => BuddyArchiveVersion; set => BuddyArchiveVersion = value; }

    public partial class SPlayerTagsConfig
    {
        [Obsolete("Use Version instead.")]
        public int U01 { get => Version; set => Version = value; }

        [Obsolete("Use TagDisplayList instead.")]
        public int[]? U02 { get => TagDisplayList; set => TagDisplayList = value; }
    }

    public partial class YoutubeUpload
    {
        [Obsolete("Use FileName instead.")]
        public string? U01 { get => FileName; set => FileName = value; }

        [Obsolete("Use UploadUrl instead.")]
        public string? U02 { get => UploadUrl; set => UploadUrl = value; }
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

    public bool UnlockAllCheat
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
