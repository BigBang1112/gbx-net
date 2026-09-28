namespace GBX.NET.Engines.System;

/// <remarks>ID: 0x0B005000</remarks>
[Class(0x0B005000)]
public partial class CSystemConfig : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0B005000;




    private bool audioEnabled;
    [AppliedWithChunk<Chunk0B005004>]
    [AppliedWithChunk<Chunk0B005028>]
    [AppliedWithChunk<Chunk0B00504F>]
    public bool AudioEnabled { get => audioEnabled; set => audioEnabled = value; }

    private float audioSoundVolume;
    [AppliedWithChunk<Chunk0B005004>]
    [AppliedWithChunk<Chunk0B005028>]
    [AppliedWithChunk<Chunk0B00504F>]
    public float AudioSoundVolume { get => audioSoundVolume; set => audioSoundVolume = value; }

    private float audioMusicVolume;
    [AppliedWithChunk<Chunk0B005004>]
    [AppliedWithChunk<Chunk0B005028>]
    [AppliedWithChunk<Chunk0B00504F>]
    public float AudioMusicVolume { get => audioMusicVolume; set => audioMusicVolume = value; }

    private bool audioAllowEFX;
    [AppliedWithChunk<Chunk0B005004>]
    [AppliedWithChunk<Chunk0B005028>]
    [AppliedWithChunk<Chunk0B00504F>]
    public bool AudioAllowEFX { get => audioAllowEFX; set => audioAllowEFX = value; }

    private string? desiredLanguageId;
    [AppliedWithChunk<Chunk0B005008>]
    public string? DesiredLanguageId { get => desiredLanguageId; set => desiredLanguageId = value; }

    private bool isIgnorePlayerSkins;
    [AppliedWithChunk<Chunk0B00500C>]
    [AppliedWithChunk<Chunk0B00501D>]
    [AppliedWithChunk<Chunk0B00503D>]
    [AppliedWithChunk<Chunk0B00504A>]
    public bool IsIgnorePlayerSkins { get => isIgnorePlayerSkins; set => isIgnorePlayerSkins = value; }

    private bool networkUseProxy;
    [AppliedWithChunk<Chunk0B00500D>]
    [AppliedWithChunk<Chunk0B005031>]
    [AppliedWithChunk<Chunk0B005036>]
    [AppliedWithChunk<Chunk0B005039>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    [AppliedWithChunk<Chunk0B00505F>]
    public bool NetworkUseProxy { get => networkUseProxy; set => networkUseProxy = value; }

    private int networkServerPort;
    [AppliedWithChunk<Chunk0B00500D>]
    [AppliedWithChunk<Chunk0B005031>]
    [AppliedWithChunk<Chunk0B005036>]
    [AppliedWithChunk<Chunk0B005039>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public int NetworkServerPort { get => networkServerPort; set => networkServerPort = value; }

    private int networkClientPort;
    [AppliedWithChunk<Chunk0B00500D>]
    [AppliedWithChunk<Chunk0B005031>]
    [AppliedWithChunk<Chunk0B005036>]
    [AppliedWithChunk<Chunk0B005039>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public int NetworkClientPort { get => networkClientPort; set => networkClientPort = value; }

    private bool networkForceUseLocalAddress;
    [AppliedWithChunk<Chunk0B00500D>]
    [AppliedWithChunk<Chunk0B005031>]
    [AppliedWithChunk<Chunk0B005036>]
    [AppliedWithChunk<Chunk0B005039>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public bool NetworkForceUseLocalAddress { get => networkForceUseLocalAddress; set => networkForceUseLocalAddress = value; }

    private string? networkForceServerAddress;
    [AppliedWithChunk<Chunk0B00500D>]
    [AppliedWithChunk<Chunk0B005031>]
    [AppliedWithChunk<Chunk0B005036>]
    [AppliedWithChunk<Chunk0B005039>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public string? NetworkForceServerAddress { get => networkForceServerAddress; set => networkForceServerAddress = value; }

    private int tmCarQuality;
    [AppliedWithChunk<Chunk0B00500E>]
    [AppliedWithChunk<Chunk0B00502C>]
    [AppliedWithChunk<Chunk0B005034>]
    [AppliedWithChunk<Chunk0B005052>]
    public int TmCarQuality { get => tmCarQuality; set => tmCarQuality = value; }

    private int tmOpponents;
    [AppliedWithChunk<Chunk0B00500E>]
    [AppliedWithChunk<Chunk0B00502C>]
    [AppliedWithChunk<Chunk0B005034>]
    [AppliedWithChunk<Chunk0B005052>]
    public int TmOpponents { get => tmOpponents; set => tmOpponents = value; }

    private bool isSkipRollingDemo;
    [AppliedWithChunk<Chunk0B00501D>]
    [AppliedWithChunk<Chunk0B00503D>]
    [AppliedWithChunk<Chunk0B00504A>]
    public bool IsSkipRollingDemo { get => isSkipRollingDemo; set => isSkipRollingDemo = value; }

    private bool isSafeMode;
    [AppliedWithChunk<Chunk0B005020>]
    public bool IsSafeMode { get => isSafeMode; set => isSafeMode = value; }

    private CSystemConfigDisplay? display;
    [AppliedWithChunk<Chunk0B005020>]
    public CSystemConfigDisplay? Display { get => display; set => display = value; }

    private bool inputsAlternateMethod;
    [AppliedWithChunk<Chunk0B005022>]
    [AppliedWithChunk<Chunk0B005045>]
    public bool InputsAlternateMethod { get => inputsAlternateMethod; set => inputsAlternateMethod = value; }

    private bool inputsFreezeUnusedAxes;
    [AppliedWithChunk<Chunk0B005022>]
    [AppliedWithChunk<Chunk0B005045>]
    public bool InputsFreezeUnusedAxes { get => inputsFreezeUnusedAxes; set => inputsFreezeUnusedAxes = value; }

    private bool audioDisableDoppler;
    [AppliedWithChunk<Chunk0B005028>]
    [AppliedWithChunk<Chunk0B00504F>]
    public bool AudioDisableDoppler { get => audioDisableDoppler; set => audioDisableDoppler = value; }

    private int audioGlobalQuality;
    [AppliedWithChunk<Chunk0B005028>]
    [AppliedWithChunk<Chunk0B00504F>]
    public int AudioGlobalQuality { get => audioGlobalQuality; set => audioGlobalQuality = value; }

    private bool advertising_DisabledByUser;
    [AppliedWithChunk<Chunk0B00502B>]
    public bool Advertising_DisabledByUser { get => advertising_DisabledByUser; set => advertising_DisabledByUser = value; }

    private float advertising_TunningCoef;
    [AppliedWithChunk<Chunk0B00502B>]
    public float Advertising_TunningCoef { get => advertising_TunningCoef; set => advertising_TunningCoef = value; }

    private int tmMaxOpponents;
    [AppliedWithChunk<Chunk0B00502C>]
    [AppliedWithChunk<Chunk0B005034>]
    [AppliedWithChunk<Chunk0B005052>]
    public int TmMaxOpponents { get => tmMaxOpponents; set => tmMaxOpponents = value; }

    private bool fileTransferEnableDownload;
    [AppliedWithChunk<Chunk0B005030>]
    [AppliedWithChunk<Chunk0B005054>]
    public bool FileTransferEnableDownload { get => fileTransferEnableDownload; set => fileTransferEnableDownload = value; }

    private bool fileTransferEnableUpload;
    [AppliedWithChunk<Chunk0B005030>]
    [AppliedWithChunk<Chunk0B005054>]
    public bool FileTransferEnableUpload { get => fileTransferEnableUpload; set => fileTransferEnableUpload = value; }

    private bool enableLocators;
    [AppliedWithChunk<Chunk0B005030>]
    [AppliedWithChunk<Chunk0B005054>]
    public bool EnableLocators { get => enableLocators; set => enableLocators = value; }

    private bool autoUpdateFromLocator;
    [AppliedWithChunk<Chunk0B005030>]
    [AppliedWithChunk<Chunk0B005054>]
    public bool AutoUpdateFromLocator { get => autoUpdateFromLocator; set => autoUpdateFromLocator = value; }

    private bool autoUpdateFromLocatorAtInternetConnection;
    [AppliedWithChunk<Chunk0B005030>]
    [AppliedWithChunk<Chunk0B005054>]
    public bool AutoUpdateFromLocatorAtInternetConnection { get => autoUpdateFromLocatorAtInternetConnection; set => autoUpdateFromLocatorAtInternetConnection = value; }

    private string? autoUpdateLocatorDBUrl;
    [AppliedWithChunk<Chunk0B005030>]
    [AppliedWithChunk<Chunk0B005054>]
    public string? AutoUpdateLocatorDBUrl { get => autoUpdateLocatorDBUrl; set => autoUpdateLocatorDBUrl = value; }

    private string? blackListUrl;
    [AppliedWithChunk<Chunk0B005030>]
    [AppliedWithChunk<Chunk0B005054>]
    public string? BlackListUrl { get => blackListUrl; set => blackListUrl = value; }

    private bool enableCrashLogUpload;
    [AppliedWithChunk<Chunk0B005030>]
    [AppliedWithChunk<Chunk0B005054>]
    public bool EnableCrashLogUpload { get => enableCrashLogUpload; set => enableCrashLogUpload = value; }

    private bool networkUseNatUPnP;
    [AppliedWithChunk<Chunk0B005031>]
    [AppliedWithChunk<Chunk0B005036>]
    [AppliedWithChunk<Chunk0B005039>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public bool NetworkUseNatUPnP { get => networkUseNatUPnP; set => networkUseNatUPnP = value; }

    private int tmBackgroundQuality;
    [AppliedWithChunk<Chunk0B005034>]
    [AppliedWithChunk<Chunk0B005052>]
    public int TmBackgroundQuality { get => tmBackgroundQuality; set => tmBackgroundQuality = value; }

    private bool networkTestInternetConnection;
    [AppliedWithChunk<Chunk0B005035>]
    [AppliedWithChunk<Chunk0B005043>]
    [AppliedWithChunk<Chunk0B005044>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public bool NetworkTestInternetConnection { get => networkTestInternetConnection; set => networkTestInternetConnection = value; }

    private int networkP2PServerPort;
    [AppliedWithChunk<Chunk0B005036>]
    [AppliedWithChunk<Chunk0B005039>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public int NetworkP2PServerPort { get => networkP2PServerPort; set => networkP2PServerPort = value; }

    private int networkServerBroadcastLength;
    [AppliedWithChunk<Chunk0B005036>]
    [AppliedWithChunk<Chunk0B005039>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public int NetworkServerBroadcastLength { get => networkServerBroadcastLength; set => networkServerBroadcastLength = value; }

    private bool gameProfileEnableMulti;
    [AppliedWithChunk<Chunk0B005038>]
    [AppliedWithChunk<Chunk0B005048>]
    public bool GameProfileEnableMulti { get => gameProfileEnableMulti; set => gameProfileEnableMulti = value; }

    private string? gameProfileName;
    [AppliedWithChunk<Chunk0B005038>]
    [AppliedWithChunk<Chunk0B005048>]
    public string? GameProfileName { get => gameProfileName; set => gameProfileName = value; }

    private int networkDownload;
    [AppliedWithChunk<Chunk0B005039>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public int NetworkDownload { get => networkDownload; set => networkDownload = value; }

    private int networkUpload;
    [AppliedWithChunk<Chunk0B005039>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public int NetworkUpload { get => networkUpload; set => networkUpload = value; }

    private string? networkLastUsedMSAddress;
    [AppliedWithChunk<Chunk0B005043>]
    [AppliedWithChunk<Chunk0B005044>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public string? NetworkLastUsedMSAddress { get => networkLastUsedMSAddress; set => networkLastUsedMSAddress = value; }

    private string? networkLastUsedMSPath;
    [AppliedWithChunk<Chunk0B005043>]
    [AppliedWithChunk<Chunk0B005044>]
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public string? NetworkLastUsedMSPath { get => networkLastUsedMSPath; set => networkLastUsedMSPath = value; }

    private bool inputsEnableRumble;
    [AppliedWithChunk<Chunk0B005045>]
    public bool InputsEnableRumble { get => inputsEnableRumble; set => inputsEnableRumble = value; }

    private bool inputsCaptureKeyboard;
    [AppliedWithChunk<Chunk0B005045>]
    public bool InputsCaptureKeyboard { get => inputsCaptureKeyboard; set => inputsCaptureKeyboard = value; }

    private string? audioDevice_Oal;
    [AppliedWithChunk<Chunk0B00504F>]
    public string? AudioDevice_Oal { get => audioDevice_Oal; set => audioDevice_Oal = value; }

    private int playerShadow;
    [AppliedWithChunk<Chunk0B005052>]
    public int PlayerShadow { get => playerShadow; set => playerShadow = value; }

    private bool audioSoundHdr;
    [AppliedWithChunk<Chunk0B005053>]
    [AppliedWithChunk<Chunk0B005056>]
    public bool AudioSoundHdr { get => audioSoundHdr; set => audioSoundHdr = value; }

    private bool audioAllowHRTF;
    [AppliedWithChunk<Chunk0B005056>]
    public bool AudioAllowHRTF { get => audioAllowHRTF; set => audioAllowHRTF = value; }

    private int audioDontMuteWhenApplicationUnfocused;
    [AppliedWithChunk<Chunk0B005056>]
    public int AudioDontMuteWhenApplicationUnfocused { get => audioDontMuteWhenApplicationUnfocused; set => audioDontMuteWhenApplicationUnfocused = value; }

    private int networkSpeed;
    [AppliedWithChunk<Chunk0B005057>]
    [AppliedWithChunk<Chunk0B00505E>]
    public int NetworkSpeed { get => networkSpeed; set => networkSpeed = value; }

    private bool fileTransferEnableAvatarDownload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableAvatarDownload { get => fileTransferEnableAvatarDownload; set => fileTransferEnableAvatarDownload = value; }

    private bool fileTransferEnableAvatarUpload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableAvatarUpload { get => fileTransferEnableAvatarUpload; set => fileTransferEnableAvatarUpload = value; }

    private bool fileTransferEnableAvatarLocators;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableAvatarLocators { get => fileTransferEnableAvatarLocators; set => fileTransferEnableAvatarLocators = value; }

    private bool fileTransferEnableMapDownload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableMapDownload { get => fileTransferEnableMapDownload; set => fileTransferEnableMapDownload = value; }

    private bool fileTransferEnableMapUpload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableMapUpload { get => fileTransferEnableMapUpload; set => fileTransferEnableMapUpload = value; }

    private bool fileTransferEnableMapLocators;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableMapLocators { get => fileTransferEnableMapLocators; set => fileTransferEnableMapLocators = value; }

    private bool fileTransferEnableMapModDownload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableMapModDownload { get => fileTransferEnableMapModDownload; set => fileTransferEnableMapModDownload = value; }

    private bool fileTransferEnableMapModUpload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableMapModUpload { get => fileTransferEnableMapModUpload; set => fileTransferEnableMapModUpload = value; }

    private bool fileTransferEnableMapModLocators;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableMapModLocators { get => fileTransferEnableMapModLocators; set => fileTransferEnableMapModLocators = value; }

    private bool fileTransferEnableMapSkinDownload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableMapSkinDownload { get => fileTransferEnableMapSkinDownload; set => fileTransferEnableMapSkinDownload = value; }

    private bool fileTransferEnableMapSkinUpload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableMapSkinUpload { get => fileTransferEnableMapSkinUpload; set => fileTransferEnableMapSkinUpload = value; }

    private bool fileTransferEnableMapSkinLocators;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableMapSkinLocators { get => fileTransferEnableMapSkinLocators; set => fileTransferEnableMapSkinLocators = value; }

    private bool fileTransferEnableTagDownload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableTagDownload { get => fileTransferEnableTagDownload; set => fileTransferEnableTagDownload = value; }

    private bool fileTransferEnableTagUpload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableTagUpload { get => fileTransferEnableTagUpload; set => fileTransferEnableTagUpload = value; }

    private bool fileTransferEnableTagLocators;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableTagLocators { get => fileTransferEnableTagLocators; set => fileTransferEnableTagLocators = value; }

    private bool fileTransferEnableVehicleSkinDownload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableVehicleSkinDownload { get => fileTransferEnableVehicleSkinDownload; set => fileTransferEnableVehicleSkinDownload = value; }

    private bool fileTransferEnableVehicleSkinUpload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableVehicleSkinUpload { get => fileTransferEnableVehicleSkinUpload; set => fileTransferEnableVehicleSkinUpload = value; }

    private bool fileTransferEnableVehicleSkinLocators;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableVehicleSkinLocators { get => fileTransferEnableVehicleSkinLocators; set => fileTransferEnableVehicleSkinLocators = value; }

    private bool fileTransferEnableUnknownTypeDownload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableUnknownTypeDownload { get => fileTransferEnableUnknownTypeDownload; set => fileTransferEnableUnknownTypeDownload = value; }

    private bool fileTransferEnableUnknownTypeUpload;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableUnknownTypeUpload { get => fileTransferEnableUnknownTypeUpload; set => fileTransferEnableUnknownTypeUpload = value; }

    private bool fileTransferEnableUnknownTypeLocators;
    [AppliedWithChunk<Chunk0B005059>]
    public bool FileTransferEnableUnknownTypeLocators { get => fileTransferEnableUnknownTypeLocators; set => fileTransferEnableUnknownTypeLocators = value; }

    private bool disableReplayRecording;
    [AppliedWithChunk<Chunk0B00505A>]
    public bool DisableReplayRecording { get => disableReplayRecording; set => disableReplayRecording = value; }

    private string? antiCheatServerUrl;
    [AppliedWithChunk<Chunk0B00505B>]
    public string? AntiCheatServerUrl { get => antiCheatServerUrl; set => antiCheatServerUrl = value; }

    private int smMaxPlayerResimStepPerFrame;
    [AppliedWithChunk<Chunk0B00505C>]
    public int SmMaxPlayerResimStepPerFrame { get => smMaxPlayerResimStepPerFrame; set => smMaxPlayerResimStepPerFrame = value; }

    private string? networkProxyLogin;
    [AppliedWithChunk<Chunk0B00505E>]
    public string? NetworkProxyLogin { get => networkProxyLogin; set => networkProxyLogin = value; }

    private string? networkProxyPassword;
    [AppliedWithChunk<Chunk0B00505E>]
    public string? NetworkProxyPassword { get => networkProxyPassword; set => networkProxyPassword = value; }

    private string? networkProxyAddress;
    [AppliedWithChunk<Chunk0B00505F>]
    public string? NetworkProxyAddress { get => networkProxyAddress; set => networkProxyAddress = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CSystemConfig"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSystemConfig() { }


    /// <summary>
    /// CSystemConfig 0x002 skippable chunk
    /// </summary>
    [Chunk(0x0B005002)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU)]
    public partial class Chunk0B005002 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU;

        public string? U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfig 0x004 skippable chunk
    /// </summary>
    [Chunk(0x0B005004)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU)]
    public partial class Chunk0B005004 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU;

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.audioEnabled);
            rw.Single(ref n.audioSoundVolume);
            rw.Single(ref n.audioMusicVolume);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Boolean(ref n.audioAllowEFX);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
        }
    }

    /// <summary>
    /// CSystemConfig 0x005 skippable chunk
    /// </summary>
    [Chunk(0x0B005005)]
    public partial class Chunk0B005005 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005005;

        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    /// <summary>
    /// CSystemConfig 0x007 skippable chunk
    /// </summary>
    [Chunk(0x0B005007)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU)]
    public partial class Chunk0B005007 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005007;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU;

        public string? U01;
        public string? U02;
        public string? U03;
        public bool U04;
        public DateTime? U05;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.String(ref U02);
            rw.String(ref U03);
            rw.Boolean(ref U04);
            rw.FileTime(ref U05);
        }
    }

    /// <summary>
    /// CSystemConfig 0x008 skippable chunk
    /// </summary>
    [Chunk(0x0B005008)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005008 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005008;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.String(ref n.desiredLanguageId);
        }
    }

    /// <summary>
    /// CSystemConfig 0x009 skippable chunk
    /// </summary>
    [Chunk(0x0B005009)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005009 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005009;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public string? U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.String(ref U01);
            rw.String(ref U01);
            rw.String(ref U01);
            rw.String(ref U01);
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfig 0x00A skippable chunk
    /// </summary>
    [Chunk(0x0B00500A)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU)]
    public partial class Chunk0B00500A : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00500A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU;

        public Int2 U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;
        public bool U07;
        public bool U08;
        public int U09;
        public int U10;
        public int U11;
        public bool U12;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Int2(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Boolean(ref U07);
            rw.Boolean(ref U08);
            rw.Int32(ref U09);
            rw.Int32(ref U10);
            rw.Int32(ref U11);
            rw.Boolean(ref U12);
        }
    }

    /// <summary>
    /// CSystemConfig 0x00B skippable chunk
    /// </summary>
    [Chunk(0x0B00500B)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B00500B : Chunk0B005005
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00500B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU | GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public int U04;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Int32(ref U04);
        }
    }

    /// <summary>
    /// CSystemConfig 0x00C skippable chunk
    /// </summary>
    [Chunk(0x0B00500C)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU)]
    public partial class Chunk0B00500C : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00500C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU;


        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isIgnorePlayerSkins);
        }
    }

    /// <summary>
    /// CSystemConfig 0x00D skippable chunk
    /// </summary>
    [Chunk(0x0B00500D)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU)]
    public partial class Chunk0B00500D : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00500D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU;

        public string? U01;
        public string? U02;
        public string? U03;
        public string? U04;
        public string? U05;
        public int U06;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.String(ref U02);
            rw.Boolean(ref n.networkUseProxy);
            rw.String(ref U03);
            rw.String(ref U04);
            rw.String(ref U05);
            rw.Int32(ref n.networkServerPort);
            rw.Int32(ref n.networkClientPort);
            rw.Boolean(ref n.networkForceUseLocalAddress);
            rw.String(ref n.networkForceServerAddress);
            rw.Int32(ref U06);
        }
    }

    /// <summary>
    /// CSystemConfig 0x00E skippable chunk
    /// </summary>
    [Chunk(0x0B00500E)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU)]
    public partial class Chunk0B00500E : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00500E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU;

        public int U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.tmCarQuality);
            rw.Int32(ref U01);
            rw.Int32(ref n.tmOpponents);
        }
    }

    /// <summary>
    /// CSystemConfig 0x00F skippable chunk
    /// </summary>
    [Chunk(0x0B00500F)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMPU)]
    public partial class Chunk0B00500F : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00500F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMPU;

        public float U01;
        public float U02;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    /// <summary>
    /// CSystemConfig 0x012 skippable chunk
    /// </summary>
    [Chunk(0x0B005012)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk0B005012 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005012;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC;

        public string? U01;
        public string? U02;
        public string? U03;
        public bool U04;
        public int U05;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.String(ref U02);
            rw.String(ref U03);
            rw.Boolean(ref U04);
            rw.Int32(ref U05);
        }
    }

    /// <summary>
    /// CSystemConfig 0x01D skippable chunk
    /// </summary>
    [Chunk(0x0B00501D)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5)]
    public partial class Chunk0B00501D : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00501D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5;


        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isIgnorePlayerSkins);
            rw.Boolean(ref n.isSkipRollingDemo);
        }
    }

    /// <summary>
    /// CSystemConfig 0x020 skippable chunk
    /// </summary>
    [Chunk(0x0B005020)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005020 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005020;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isSafeMode);
            rw.Node<CSystemConfigDisplay>(ref n.display);
        }
    }

    /// <summary>
    /// CSystemConfig 0x022 skippable chunk
    /// </summary>
    [Chunk(0x0B005022)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5)]
    public partial class Chunk0B005022 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005022;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5;

        public float U01;
        public float U02;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Boolean(ref n.inputsAlternateMethod);
            rw.Boolean(ref n.inputsFreezeUnusedAxes);
        }
    }

    /// <summary>
    /// CSystemConfig 0x027 skippable chunk
    /// </summary>
    [Chunk(0x0B005027)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX)]
    public partial class Chunk0B005027 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005027;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX;

        public bool U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfig 0x028 skippable chunk
    /// </summary>
    [Chunk(0x0B005028)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5)]
    public partial class Chunk0B005028 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005028;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5;

        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.audioEnabled);
            rw.Single(ref n.audioSoundVolume);
            rw.Single(ref n.audioMusicVolume);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Boolean(ref n.audioAllowEFX);
            rw.Boolean(ref n.audioDisableDoppler);
            rw.Int32(ref n.audioGlobalQuality);
        }
    }

    /// <summary>
    /// CSystemConfig 0x02B skippable chunk
    /// </summary>
    [Chunk(0x0B00502B)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B00502B : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00502B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public int U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Boolean(ref n.advertising_DisabledByUser);
            rw.Single(ref n.advertising_TunningCoef);
        }
    }

    /// <summary>
    /// CSystemConfig 0x02C skippable chunk
    /// </summary>
    [Chunk(0x0B00502C)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX)]
    public partial class Chunk0B00502C : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00502C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX;

        public int U01;
        public int U02;
        public bool U03;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.tmCarQuality);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref n.tmOpponents);
            rw.Int32(ref n.tmMaxOpponents);
            rw.Boolean(ref U03);
        }
    }

    /// <summary>
    /// CSystemConfig 0x030 skippable chunk
    /// </summary>
    [Chunk(0x0B005030)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF)]
    public partial class Chunk0B005030 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005030;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF;

        public int U01;
        public bool U02;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.fileTransferEnableDownload);
            rw.Boolean(ref n.fileTransferEnableUpload);
            rw.Int32(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref n.enableLocators);
            rw.Boolean(ref n.autoUpdateFromLocator);
            rw.Boolean(ref n.autoUpdateFromLocatorAtInternetConnection);
            rw.String(ref n.autoUpdateLocatorDBUrl);
            rw.String(ref n.blackListUrl);
            rw.Boolean(ref n.enableCrashLogUpload);
        }
    }

    /// <summary>
    /// CSystemConfig 0x031 skippable chunk
    /// </summary>
    [Chunk(0x0B005031)]
    [ChunkGameVersion(GameVersion.TMO | GameVersion.TMSX)]
    public partial class Chunk0B005031 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005031;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMO | GameVersion.TMSX;

        public string? U01;
        public string? U02;
        public int U03;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.networkUseProxy);
            rw.String(ref U01);
            rw.String(ref U02);
            rw.Int32(ref n.networkServerPort);
            rw.Int32(ref n.networkClientPort);
            rw.Boolean(ref n.networkForceUseLocalAddress);
            rw.String(ref n.networkForceServerAddress);
            rw.Int32(ref U03);
            rw.Boolean(ref n.networkUseNatUPnP);
        }
    }

    /// <summary>
    /// CSystemConfig 0x034 skippable chunk
    /// </summary>
    [Chunk(0x0B005034)]
    [ChunkGameVersion(GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF)]
    public partial class Chunk0B005034 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005034;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF;

        public int U01;
        public int U02;
        public bool U03;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.tmCarQuality);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref n.tmOpponents);
            rw.Int32(ref n.tmMaxOpponents);
            rw.Boolean(ref U03);
            rw.Int32(ref n.tmBackgroundQuality);
        }
    }

    /// <summary>
    /// CSystemConfig 0x035 skippable chunk
    /// </summary>
    [Chunk(0x0B005035)]
    [ChunkGameVersion(GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5)]
    public partial class Chunk0B005035 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005035;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMNESWC | GameVersion.TMU | GameVersion.VSK5;

        public bool U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref n.networkTestInternetConnection);
        }
    }

    /// <summary>
    /// CSystemConfig 0x036 skippable chunk
    /// </summary>
    [Chunk(0x0B005036)]
    [ChunkGameVersion(GameVersion.TMNESWC)]
    public partial class Chunk0B005036 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005036;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMNESWC;

        public string? U01;
        public string? U02;
        public int U03;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.networkUseProxy);
            rw.String(ref U01);
            rw.String(ref U02);
            rw.Int32(ref n.networkServerPort);
            rw.Int32(ref n.networkP2PServerPort);
            rw.Int32(ref n.networkClientPort);
            rw.Int32(ref n.networkServerBroadcastLength);
            rw.Boolean(ref n.networkForceUseLocalAddress);
            rw.String(ref n.networkForceServerAddress);
            rw.Int32(ref U03);
            rw.Boolean(ref n.networkUseNatUPnP);
        }
    }

    /// <summary>
    /// CSystemConfig 0x037 skippable chunk
    /// </summary>
    [Chunk(0x0B005037)]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.VSK5)]
    public partial class Chunk0B005037 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005037;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.VSK5;

        public string? U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfig 0x038 skippable chunk
    /// </summary>
    [Chunk(0x0B005038)]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.VSK5)]
    public partial class Chunk0B005038 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005038;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.VSK5;


        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.gameProfileEnableMulti);
            rw.Id(ref n.gameProfileName);
        }
    }

    /// <summary>
    /// CSystemConfig 0x039 skippable chunk
    /// </summary>
    [Chunk(0x0B005039)]
    [ChunkGameVersion(GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF)]
    public partial class Chunk0B005039 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005039;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMU | GameVersion.VSK5 | GameVersion.TMF;

        public string? U01;
        public string? U02;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.networkUseProxy);
            rw.String(ref U01);
            rw.String(ref U02);
            rw.Int32(ref n.networkServerPort);
            rw.Int32(ref n.networkP2PServerPort);
            rw.Int32(ref n.networkClientPort);
            rw.Int32(ref n.networkServerBroadcastLength);
            rw.Boolean(ref n.networkForceUseLocalAddress);
            rw.String(ref n.networkForceServerAddress);
            rw.Int32(ref n.networkDownload);
            rw.Int32(ref n.networkUpload);
            rw.Boolean(ref n.networkUseNatUPnP);
        }
    }

    /// <summary>
    /// CSystemConfig 0x03A skippable chunk
    /// </summary>
    [Chunk(0x0B00503A)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B00503A : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00503A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public string? U01;
        public string? U02;
        public int U03;
        public DateTime? U04;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.String(ref U02);
            rw.Int32(ref U03);
            rw.FileTime(ref U04);
        }
    }

    /// <summary>
    /// CSystemConfig 0x03D skippable chunk
    /// </summary>
    [Chunk(0x0B00503D)]
    public partial class Chunk0B00503D : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00503D;

        public int U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isIgnorePlayerSkins);
            rw.Boolean(ref n.isSkipRollingDemo);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfig 0x03E skippable chunk
    /// </summary>
    [Chunk(0x0B00503E)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B00503E : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00503E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public DateTime? U01;
        public UInt128 U02;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.FileTime(ref U01);
            rw.UInt128(ref U02);
        }
    }

    /// <summary>
    /// CSystemConfig 0x041 skippable chunk
    /// </summary>
    [Chunk(0x0B005041)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk0B005041 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005041;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

        public string? U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfig 0x043 skippable chunk
    /// </summary>
    [Chunk(0x0B005043)]
    public partial class Chunk0B005043 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005043;

        public bool U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref n.networkTestInternetConnection);
            rw.String(ref n.networkLastUsedMSAddress);
            rw.String(ref n.networkLastUsedMSPath);
        }
    }

    /// <summary>
    /// CSystemConfig 0x044 skippable chunk
    /// </summary>
    [Chunk(0x0B005044)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk0B005044 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005044;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

        public int[]? U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.networkTestInternetConnection);
            rw.String(ref n.networkLastUsedMSAddress);
            rw.String(ref n.networkLastUsedMSPath);
            rw.Array<int>(ref U01!);
        }
    }

    /// <summary>
    /// CSystemConfig 0x045 skippable chunk
    /// </summary>
    [Chunk(0x0B005045)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005045 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005045;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.inputsAlternateMethod);
            rw.Boolean(ref n.inputsFreezeUnusedAxes);
            rw.Boolean(ref n.inputsEnableRumble);
            rw.Boolean(ref n.inputsCaptureKeyboard);
        }
    }

    /// <summary>
    /// CSystemConfig 0x047 skippable chunk
    /// </summary>
    [Chunk(0x0B005047)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005047 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005047;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public bool U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfig 0x048 skippable chunk
    /// </summary>
    [Chunk(0x0B005048)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005048 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005048;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.gameProfileEnableMulti);
            rw.String(ref n.gameProfileName);
        }
    }

    /// <summary>
    /// CSystemConfig 0x049 skippable chunk
    /// </summary>
    [Chunk(0x0B005049)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005049 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005049;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public DateTime? U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.FileTime(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfig 0x04A skippable chunk
    /// </summary>
    [Chunk(0x0B00504A)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B00504A : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00504A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isIgnorePlayerSkins);
            rw.Boolean(ref n.isSkipRollingDemo);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    /// <summary>
    /// CSystemConfig 0x04C skippable chunk
    /// </summary>
    [Chunk(0x0B00504C)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk0B00504C : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00504C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

        public bool U01;
        public bool U02;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
        }
    }

    /// <summary>
    /// CSystemConfig 0x04D skippable chunk
    /// </summary>
    [Chunk(0x0B00504D)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk0B00504D : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00504D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;

        public string? U01;
        public bool U02;
        public bool U03;
        public bool U04;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
        }
    }

    /// <summary>
    /// CSystemConfig 0x04E skippable chunk
    /// </summary>
    [Chunk(0x0B00504E)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3)]
    public partial class Chunk0B00504E : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00504E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3;

        public string[]? U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.ArrayString(ref U01!);
        }
    }

    /// <summary>
    /// CSystemConfig 0x04F skippable chunk
    /// </summary>
    [Chunk(0x0B00504F)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B00504F : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00504F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.MP3 | GameVersion.MP4;

        public int U01;
        public int U02;
        public int U03;
        public bool U04;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.audioEnabled);
            rw.Single(ref n.audioSoundVolume);
            rw.Single(ref n.audioMusicVolume);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Boolean(ref n.audioAllowEFX);
            rw.Boolean(ref n.audioDisableDoppler);
            rw.Boolean(ref U04);
            rw.Int32(ref n.audioGlobalQuality);
            rw.String(ref n.audioDevice_Oal);
        }
    }

    /// <summary>
    /// CSystemConfig 0x050 skippable chunk
    /// </summary>
    [Chunk(0x0B005050)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005050 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005050;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public bool U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfig 0x051 skippable chunk
    /// </summary>
    [Chunk(0x0B005051)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005051 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005051;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public string? U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfig 0x052 skippable chunk
    /// </summary>
    [Chunk(0x0B005052)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005052 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005052;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public int U01;
        public int U02;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.tmCarQuality);
            rw.Int32(ref U01);
            rw.Int32(ref n.playerShadow);
            rw.Int32(ref U02);
            rw.Int32(ref n.tmOpponents);
            rw.Int32(ref n.tmMaxOpponents);
            rw.Int32(ref n.tmBackgroundQuality);
        }
    }

    /// <summary>
    /// CSystemConfig 0x053 skippable chunk
    /// </summary>
    [Chunk(0x0B005053)]
    public partial class Chunk0B005053 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005053;


        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.audioSoundHdr);
        }
    }

    /// <summary>
    /// CSystemConfig 0x054 skippable chunk
    /// </summary>
    [Chunk(0x0B005054)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005054 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005054;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public ulong U01;
        public bool U02;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.fileTransferEnableDownload);
            rw.Boolean(ref n.fileTransferEnableUpload);
            rw.UInt64(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref n.enableLocators);
            rw.Boolean(ref n.autoUpdateFromLocator);
            rw.Boolean(ref n.autoUpdateFromLocatorAtInternetConnection);
            rw.String(ref n.autoUpdateLocatorDBUrl);
            rw.String(ref n.blackListUrl);
            rw.Boolean(ref n.enableCrashLogUpload);
        }
    }

    /// <summary>
    /// CSystemConfig 0x055 skippable chunk
    /// </summary>
    [Chunk(0x0B005055)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005055 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005055;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public string? U01;
        public bool U02;
        public bool U03;
        public bool U04;
        public bool U05;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref U05);
        }
    }

    /// <summary>
    /// CSystemConfig 0x056 skippable chunk
    /// </summary>
    [Chunk(0x0B005056)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005056 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005056;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public bool U01;
        public int U02;
        public bool U03;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref n.audioAllowHRTF);
            rw.Int32(ref n.audioDontMuteWhenApplicationUnfocused);
            rw.Int32(ref U02);
            rw.Boolean(ref n.audioSoundHdr);
            rw.Boolean(ref U03);
        }
    }

    /// <summary>
    /// CSystemConfig 0x057 skippable chunk
    /// </summary>
    [Chunk(0x0B005057)]
    public partial class Chunk0B005057 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005057;

        public string? U01;
        public string? U02;
        public int[]? U03;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.networkUseProxy);
            rw.String(ref U01);
            rw.String(ref U02);
            rw.Int32(ref n.networkServerPort);
            rw.Int32(ref n.networkP2PServerPort);
            rw.Int32(ref n.networkClientPort);
            rw.Int32(ref n.networkServerBroadcastLength);
            rw.Boolean(ref n.networkForceUseLocalAddress);
            rw.String(ref n.networkForceServerAddress);
            rw.Int32(ref n.networkDownload);
            rw.Int32(ref n.networkUpload);
            rw.Boolean(ref n.networkUseNatUPnP);
            rw.Boolean(ref n.networkTestInternetConnection);
            rw.String(ref n.networkLastUsedMSAddress);
            rw.String(ref n.networkLastUsedMSPath);
            rw.Array<int>(ref U03!);
            rw.Int32(ref n.networkSpeed);
        }
    }

    /// <summary>
    /// CSystemConfig 0x058 skippable chunk
    /// </summary>
    [Chunk(0x0B005058)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005058 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005058;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public int U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfig 0x059 skippable chunk
    /// </summary>
    [Chunk(0x0B005059)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B005059 : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B005059;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;


        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.fileTransferEnableAvatarDownload);
            rw.Boolean(ref n.fileTransferEnableAvatarUpload);
            rw.Boolean(ref n.fileTransferEnableAvatarLocators);
            rw.Boolean(ref n.fileTransferEnableMapDownload);
            rw.Boolean(ref n.fileTransferEnableMapUpload);
            rw.Boolean(ref n.fileTransferEnableMapLocators);
            rw.Boolean(ref n.fileTransferEnableMapModDownload);
            rw.Boolean(ref n.fileTransferEnableMapModUpload);
            rw.Boolean(ref n.fileTransferEnableMapModLocators);
            rw.Boolean(ref n.fileTransferEnableMapSkinDownload);
            rw.Boolean(ref n.fileTransferEnableMapSkinUpload);
            rw.Boolean(ref n.fileTransferEnableMapSkinLocators);
            rw.Boolean(ref n.fileTransferEnableTagDownload);
            rw.Boolean(ref n.fileTransferEnableTagUpload);
            rw.Boolean(ref n.fileTransferEnableTagLocators);
            rw.Boolean(ref n.fileTransferEnableVehicleSkinDownload);
            rw.Boolean(ref n.fileTransferEnableVehicleSkinUpload);
            rw.Boolean(ref n.fileTransferEnableVehicleSkinLocators);
            rw.Boolean(ref n.fileTransferEnableUnknownTypeDownload);
            rw.Boolean(ref n.fileTransferEnableUnknownTypeUpload);
            rw.Boolean(ref n.fileTransferEnableUnknownTypeLocators);
        }
    }

    /// <summary>
    /// CSystemConfig 0x05A skippable chunk
    /// </summary>
    [Chunk(0x0B00505A)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B00505A : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00505A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public bool U01;
        public bool U02;
        public bool U03;
        public int U04;
        public int U05;
        public int U06;
        public string? U07;
        public string? U08;
        public float U09;
        public float U10;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.disableReplayRecording);
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.String(ref U07);
            rw.String(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
        }
    }

    /// <summary>
    /// CSystemConfig 0x05B skippable chunk
    /// </summary>
    [Chunk(0x0B00505B)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B00505B : SkippableChunk<CSystemConfig>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00505B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public int Version { get; set; }


        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref n.antiCheatServerUrl);
        }
    }

    /// <summary>
    /// CSystemConfig 0x05C skippable chunk
    /// </summary>
    [Chunk(0x0B00505C)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B00505C : SkippableChunk<CSystemConfig>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00505C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public int Version { get; set; }


        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.smMaxPlayerResimStepPerFrame);
        }
    }

    /// <summary>
    /// CSystemConfig 0x05D skippable chunk
    /// </summary>
    [Chunk(0x0B00505D)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B00505D : SkippableChunk<CSystemConfig>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00505D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public int Version { get; set; }

        public string? U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CSystemConfig 0x05E skippable chunk
    /// </summary>
    [Chunk(0x0B00505E)]
    [ChunkGameVersion(GameVersion.MP3 | GameVersion.MP4)]
    public partial class Chunk0B00505E : SkippableChunk<CSystemConfig>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00505E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP3 | GameVersion.MP4;

        public int Version { get; set; }

        public int[]? U01;

        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref n.networkUseProxy);
            rw.String(ref n.networkProxyLogin);
            rw.String(ref n.networkProxyPassword);
            rw.Int32(ref n.networkServerPort);
            rw.Int32(ref n.networkP2PServerPort);
            rw.Int32(ref n.networkClientPort);
            rw.Int32(ref n.networkServerBroadcastLength);
            rw.Boolean(ref n.networkForceUseLocalAddress);
            rw.String(ref n.networkForceServerAddress);
            rw.Int32(ref n.networkDownload);
            rw.Int32(ref n.networkUpload);
            rw.Boolean(ref n.networkUseNatUPnP);
            rw.Boolean(ref n.networkTestInternetConnection);
            rw.String(ref n.networkLastUsedMSAddress);
            rw.String(ref n.networkLastUsedMSPath);
            rw.Array<int>(ref U01!);
            rw.Int32(ref n.networkSpeed);
        }
    }

    /// <summary>
    /// CSystemConfig 0x05F skippable chunk
    /// </summary>
    [Chunk(0x0B00505F)]
    [ChunkGameVersion(GameVersion.MP4)]
    public partial class Chunk0B00505F : SkippableChunk<CSystemConfig>
    {
        /// <inheritdoc />
        public override uint Id => 0x0B00505F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.MP4;


        public override void ReadWrite(CSystemConfig n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.networkUseProxy);
            rw.String(ref n.networkProxyAddress);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0B005002 => new Chunk0B005002(),
        0x0B005004 => new Chunk0B005004(),
        0x0B005005 => new Chunk0B005005(),
        0x0B005007 => new Chunk0B005007(),
        0x0B005008 => new Chunk0B005008(),
        0x0B005009 => new Chunk0B005009(),
        0x0B00500A => new Chunk0B00500A(),
        0x0B00500B => new Chunk0B00500B(),
        0x0B00500C => new Chunk0B00500C(),
        0x0B00500D => new Chunk0B00500D(),
        0x0B00500E => new Chunk0B00500E(),
        0x0B00500F => new Chunk0B00500F(),
        0x0B005012 => new Chunk0B005012(),
        0x0B00501D => new Chunk0B00501D(),
        0x0B005020 => new Chunk0B005020(),
        0x0B005022 => new Chunk0B005022(),
        0x0B005027 => new Chunk0B005027(),
        0x0B005028 => new Chunk0B005028(),
        0x0B00502B => new Chunk0B00502B(),
        0x0B00502C => new Chunk0B00502C(),
        0x0B005030 => new Chunk0B005030(),
        0x0B005031 => new Chunk0B005031(),
        0x0B005034 => new Chunk0B005034(),
        0x0B005035 => new Chunk0B005035(),
        0x0B005036 => new Chunk0B005036(),
        0x0B005037 => new Chunk0B005037(),
        0x0B005038 => new Chunk0B005038(),
        0x0B005039 => new Chunk0B005039(),
        0x0B00503A => new Chunk0B00503A(),
        0x0B00503D => new Chunk0B00503D(),
        0x0B00503E => new Chunk0B00503E(),
        0x0B005041 => new Chunk0B005041(),
        0x0B005043 => new Chunk0B005043(),
        0x0B005044 => new Chunk0B005044(),
        0x0B005045 => new Chunk0B005045(),
        0x0B005047 => new Chunk0B005047(),
        0x0B005048 => new Chunk0B005048(),
        0x0B005049 => new Chunk0B005049(),
        0x0B00504A => new Chunk0B00504A(),
        0x0B00504C => new Chunk0B00504C(),
        0x0B00504D => new Chunk0B00504D(),
        0x0B00504E => new Chunk0B00504E(),
        0x0B00504F => new Chunk0B00504F(),
        0x0B005050 => new Chunk0B005050(),
        0x0B005051 => new Chunk0B005051(),
        0x0B005052 => new Chunk0B005052(),
        0x0B005053 => new Chunk0B005053(),
        0x0B005054 => new Chunk0B005054(),
        0x0B005055 => new Chunk0B005055(),
        0x0B005056 => new Chunk0B005056(),
        0x0B005057 => new Chunk0B005057(),
        0x0B005058 => new Chunk0B005058(),
        0x0B005059 => new Chunk0B005059(),
        0x0B00505A => new Chunk0B00505A(),
        0x0B00505B => new Chunk0B00505B(),
        0x0B00505C => new Chunk0B00505C(),
        0x0B00505D => new Chunk0B00505D(),
        0x0B00505E => new Chunk0B00505E(),
        0x0B00505F => new Chunk0B00505F(),
        _ => base.NewChunk(chunkId),
    };
}
