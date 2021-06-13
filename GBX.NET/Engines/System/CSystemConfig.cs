using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.System
{
    [Node(0x0B005000)]
    public class CSystemConfig : CMwNod
    {
        #region Fields

        private string desiredLanguageId;
        private bool advertising_DisabledByUser;
        private float advertising_TunningCoef;
        private bool inputsAlternateMethod;
        private bool inputsFreezeUnusedAxes;
        private bool inputsEnableRumble;
        private bool inputsCaptureKeyboard;
        private bool gameProfileEnableMulti;
        private string gameProfileName;
        private bool isIgnorePlayerSkins;
        private bool isSkipRollingDemo;
        private bool audioEnabled;
        private float audioSoundVolume;
        private float audioMusicVolume;
        private bool audioAllowEFX;
        private bool audioDisableDoppler;
        private int audioGlobalQuality;
        private string audioDevice_Oal;
        private bool audioAllowHRTF;
        private int audioDontMuteWhenApplicationUnfocused;
        private bool audioSoundHdr;
        private bool fileTransferEnableAvatarDownload;
        private bool fileTransferEnableAvatarUpload;
        private bool fileTransferEnableAvatarLocators;
        private bool fileTransferEnableMapDownload;
        private bool fileTransferEnableMapUpload;
        private bool fileTransferEnableMapLocators;
        private bool fileTransferEnableMapModDownload;
        private bool fileTransferEnableMapModUpload;
        private bool fileTransferEnableMapModLocators;
        private bool fileTransferEnableMapSkinDownload;
        private bool fileTransferEnableMapSkinUpload;
        private bool fileTransferEnableMapSkinLocators;
        private bool fileTransferEnableTagDownload;
        private bool fileTransferEnableTagUpload;
        private bool fileTransferEnableTagLocators;
        private bool fileTransferEnableVehicleSkinDownload;
        private bool fileTransferEnableVehicleSkinUpload;
        private bool fileTransferEnableVehicleSkinLocators;
        private bool fileTransferEnableUnknownTypeDownload;
        private bool fileTransferEnableUnknownTypeUpload;
        private bool fileTransferEnableUnknownTypeLocators;
        private bool networkTestInternetConnection;
        private string networkLastUsedMSAddress;
        private string networkLastUsedMSPath;
        private bool networkUseProxy;
        private int networkServerPort;
        private int networkP2PServerPort;
        private int networkClientPort;
        private int networkServerBroadcastLength;
        private bool networkForceUseLocalAddress;
        private string networkForceServerAddress;
        private int networkDownload;
        private int networkUpload;
        private bool networkUseNatUPnP;
        private int tmCarQuality;
        private int playerShadow;
        private int tmOpponents;
        private int tmMaxOpponents;
        private int tmBackgroundQuality;

        #endregion

        #region Properties

        public string DesiredLanguageId
        {
            get
            {
                DiscoverChunk<Chunk0B005008>();
                return desiredLanguageId;
            }
            set
            {
                DiscoverChunk<Chunk0B005008>();
                desiredLanguageId = value;
            }
        }

        public bool Advertising_DisabledByUser
        {
            get
            {
                DiscoverChunk<Chunk0B00502B>();
                return advertising_DisabledByUser;
            }
            set
            {
                DiscoverChunk<Chunk0B00502B>();
                advertising_DisabledByUser = value;
            }
        }

        public float Advertising_TunningCoef
        {
            get
            {
                DiscoverChunk<Chunk0B00502B>();
                return advertising_TunningCoef;
            }
            set
            {
                DiscoverChunk<Chunk0B00502B>();
                advertising_TunningCoef = value;
            }
        }

        public bool InputsAlternateMethod
        {
            get
            {
                DiscoverChunk<Chunk0B005045>();
                return inputsAlternateMethod;
            }
            set
            {
                DiscoverChunk<Chunk0B005045>();
                inputsAlternateMethod = value;
            }
        }

        public bool InputsFreezeUnusedAxes
        {
            get
            {
                DiscoverChunk<Chunk0B005045>();
                return inputsFreezeUnusedAxes;
            }
            set
            {
                DiscoverChunk<Chunk0B005045>();
                inputsFreezeUnusedAxes = value;
            }
        }

        public bool InputsEnableRumble
        {
            get
            {
                DiscoverChunk<Chunk0B005045>();
                return inputsEnableRumble;
            }
            set
            {
                DiscoverChunk<Chunk0B005045>();
                inputsEnableRumble = value;
            }
        }

        public bool InputsCaptureKeyboard
        {
            get
            {
                DiscoverChunk<Chunk0B005045>();
                return inputsCaptureKeyboard;
            }
            set
            {
                DiscoverChunk<Chunk0B005045>();
                inputsCaptureKeyboard = value;
            }
        }

        public bool GameProfileEnableMulti
        {
            get
            {
                DiscoverChunk<Chunk0B005048>();
                return gameProfileEnableMulti;
            }
            set
            {
                DiscoverChunk<Chunk0B005048>();
                gameProfileEnableMulti = value;
            }
        }

        public string GameProfileName
        {
            get
            {
                DiscoverChunk<Chunk0B005048>();
                return gameProfileName;
            }
            set
            {
                DiscoverChunk<Chunk0B005048>();
                gameProfileName = value;
            }
        }

        public bool IsIgnorePlayerSkins
        {
            get
            {
                DiscoverChunks<Chunk0B00503D, Chunk0B00504A>();
                return isIgnorePlayerSkins;
            }
            set
            {
                DiscoverChunks<Chunk0B00503D, Chunk0B00504A>();
                isIgnorePlayerSkins = value;
            }
        }

        public bool IsSkipRollingDemo
        {
            get
            {
                DiscoverChunks<Chunk0B00503D, Chunk0B00504A>();
                return isSkipRollingDemo;
            }
            set
            {
                DiscoverChunks<Chunk0B00503D, Chunk0B00504A>();
                isSkipRollingDemo = value;
            }
        }

        public bool AudioEnabled
        {
            get
            {
                DiscoverChunk<Chunk0B00504F>();
                return audioEnabled;
            }
            set
            {
                DiscoverChunk<Chunk0B00504F>();
                audioEnabled = value;
            }
        }

        public float AudioSoundVolume
        {
            get
            {
                DiscoverChunk<Chunk0B00504F>();
                return audioSoundVolume;
            }
            set
            {
                DiscoverChunk<Chunk0B00504F>();
                audioSoundVolume = value;
            }
        }

        public float AudioMusicVolume
        {
            get
            {
                DiscoverChunk<Chunk0B00504F>();
                return audioMusicVolume;
            }
            set
            {
                DiscoverChunk<Chunk0B00504F>();
                audioMusicVolume = value;
            }
        }

        public bool AudioAllowEFX
        {
            get
            {
                DiscoverChunk<Chunk0B00504F>();
                return audioAllowEFX;
            }
            set
            {
                DiscoverChunk<Chunk0B00504F>();
                audioAllowEFX = value;
            }
        }

        public bool AudioDisableDoppler
        {
            get
            {
                DiscoverChunk<Chunk0B00504F>();
                return audioDisableDoppler;
            }
            set
            {
                DiscoverChunk<Chunk0B00504F>();
                audioDisableDoppler = value;
            }
        }

        public int AudioGlobalQuality
        {
            get
            {
                DiscoverChunk<Chunk0B00504F>();
                return audioGlobalQuality;
            }
            set
            {
                DiscoverChunk<Chunk0B00504F>();
                audioGlobalQuality = value;
            }
        }

        public string AudioDevice_Oal
        {
            get
            {
                DiscoverChunk<Chunk0B00504F>();
                return audioDevice_Oal;
            }
            set
            {
                DiscoverChunk<Chunk0B00504F>();
                audioDevice_Oal = value;
            }
        }

        public bool AudioAllowHRTF
        {
            get
            {
                DiscoverChunk<Chunk0B005056>();
                return audioAllowHRTF;
            }
            set
            {
                DiscoverChunk<Chunk0B005056>();
                audioAllowHRTF = value;
            }
        }

        public int AudioDontMuteWhenApplicationUnfocused
        {
            get
            {
                DiscoverChunk<Chunk0B005056>();
                return audioDontMuteWhenApplicationUnfocused;
            }
            set
            {
                DiscoverChunk<Chunk0B005056>();
                audioDontMuteWhenApplicationUnfocused = value;
            }
        }

        public bool AudioSoundHdr
        {
            get
            {
                DiscoverChunk<Chunk0B005056>();
                return audioSoundHdr;
            }
            set
            {
                DiscoverChunk<Chunk0B005056>();
                audioSoundHdr = value;
            }
        }

        public bool FileTransferEnableAvatarDownload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableAvatarDownload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableAvatarDownload = value;
            }
        }

        public bool FileTransferEnableAvatarUpload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableAvatarUpload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableAvatarUpload = value;
            }
        }

        public bool FileTransferEnableAvatarLocators
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableAvatarLocators;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableAvatarLocators = value;
            }
        }

        public bool FileTransferEnableMapDownload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableMapDownload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableMapDownload = value;
            }
        }

        public bool FileTransferEnableMapUpload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableMapUpload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableMapUpload = value;
            }
        }

        public bool FileTransferEnableMapLocators
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableMapLocators;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableMapLocators = value;
            }
        }

        public bool FileTransferEnableMapModDownload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableMapModDownload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableMapModDownload = value;
            }
        }

        public bool FileTransferEnableMapModUpload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableMapModUpload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableMapModUpload = value;
            }
        }

        public bool FileTransferEnableMapModLocators
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableMapModLocators;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableMapModLocators = value;
            }
        }

        public bool FileTransferEnableMapSkinDownload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableMapSkinDownload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableMapSkinDownload = value;
            }
        }

        public bool FileTransferEnableMapSkinUpload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableMapSkinUpload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableMapSkinUpload = value;
            }
        }

        public bool FileTransferEnableMapSkinLocators
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableMapSkinLocators;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableMapSkinLocators = value;
            }
        }

        public bool FileTransferEnableTagDownload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableTagDownload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableTagDownload = value;
            }
        }

        public bool FileTransferEnableTagUpload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableTagUpload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableTagUpload = value;
            }
        }

        public bool FileTransferEnableTagLocators
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableTagLocators;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableTagLocators = value;
            }
        }

        public bool FileTransferEnableVehicleSkinDownload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableVehicleSkinDownload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableVehicleSkinDownload = value;
            }
        }

        public bool FileTransferEnableVehicleSkinUpload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableVehicleSkinUpload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableVehicleSkinUpload = value;
            }
        }

        public bool FileTransferEnableVehicleSkinLocators
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableVehicleSkinLocators;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableVehicleSkinLocators = value;
            }
        }

        public bool FileTransferEnableUnknownTypeDownload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableUnknownTypeDownload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableUnknownTypeDownload = value;
            }
        }

        public bool FileTransferEnableUnknownTypeUpload
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableUnknownTypeUpload;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableUnknownTypeUpload = value;
            }
        }

        public bool FileTransferEnableUnknownTypeLocators
        {
            get
            {
                DiscoverChunk<Chunk0B005059>();
                return fileTransferEnableUnknownTypeLocators;
            }
            set
            {
                DiscoverChunk<Chunk0B005059>();
                fileTransferEnableUnknownTypeLocators = value;
            }
        }

        public bool NetworkTestInternetConnection
        {
            get
            {
                DiscoverChunks<Chunk0B005035, Chunk0B005043, Chunk0B005044>();
                return networkTestInternetConnection;
            }
            set
            {
                DiscoverChunks<Chunk0B005035, Chunk0B005043, Chunk0B005044>();
                networkTestInternetConnection = value;
            }
        }

        public string NetworkLastUsedMSAddress
        {
            get
            {
                DiscoverChunks<Chunk0B005043, Chunk0B005044>();
                return networkLastUsedMSAddress;
            }
            set
            {
                DiscoverChunks<Chunk0B005043, Chunk0B005044>();
                networkLastUsedMSAddress = value;
            }
        }

        public string NetworkLastUsedMSPath
        {
            get
            {
                DiscoverChunks<Chunk0B005043, Chunk0B005044>();
                return networkLastUsedMSPath;
            }
            set
            {
                DiscoverChunks<Chunk0B005043, Chunk0B005044>();
                networkLastUsedMSPath = value;
            }
        }

        public bool NetworkUseProxy
        {
            get
            {
                DiscoverChunk<Chunk0B005039>();
                return networkUseProxy;
            }
            set
            {
                DiscoverChunk<Chunk0B005039>();
                networkUseProxy = value;
            }
        }

        public int NetworkServerPort
        {
            get
            {
                DiscoverChunk<Chunk0B005039>();
                return networkServerPort;
            }
            set
            {
                DiscoverChunk<Chunk0B005039>();
                networkServerPort = value;
            }
        }

        public int NetworkP2PServerPort
        {
            get
            {
                DiscoverChunk<Chunk0B005039>();
                return networkP2PServerPort;
            }
            set
            {
                DiscoverChunk<Chunk0B005039>();
                networkP2PServerPort = value;
            }
        }

        public int NetworkClientPort
        {
            get
            {
                DiscoverChunk<Chunk0B005039>();
                return networkClientPort;
            }
            set
            {
                DiscoverChunk<Chunk0B005039>();
                networkClientPort = value;
            }
        }

        public int NetworkServerBroadcastLength
        {
            get
            {
                DiscoverChunk<Chunk0B005039>();
                return networkServerBroadcastLength;
            }
            set
            {
                DiscoverChunk<Chunk0B005039>();
                networkServerBroadcastLength = value;
            }
        }

        public bool NetworkForceUseLocalAddress
        {
            get
            {
                DiscoverChunk<Chunk0B005039>();
                return networkForceUseLocalAddress;
            }
            set
            {
                DiscoverChunk<Chunk0B005039>();
                networkForceUseLocalAddress = value;
            }
        }

        public string NetworkForceServerAddress
        {
            get
            {
                DiscoverChunk<Chunk0B005039>();
                return networkForceServerAddress;
            }
            set
            {
                DiscoverChunk<Chunk0B005039>();
                networkForceServerAddress = value;
            }
        }

        public int NetworkDownload
        {
            get
            {
                DiscoverChunk<Chunk0B005039>();
                return networkDownload;
            }
            set
            {
                DiscoverChunk<Chunk0B005039>();
                networkDownload = value;
            }
        }

        public int NetworkUpload
        {
            get
            {
                DiscoverChunk<Chunk0B005039>();
                return networkUpload;
            }
            set
            {
                DiscoverChunk<Chunk0B005039>();
                networkUpload = value;
            }
        }

        public bool NetworkUseNatUPnP
        {
            get
            {
                DiscoverChunk<Chunk0B005039>();
                return networkUseNatUPnP;
            }
            set
            {
                DiscoverChunk<Chunk0B005039>();
                networkUseNatUPnP = value;
            }
        }

        public int TmCarQuality
        {
            get
            {
                DiscoverChunks<Chunk0B005034, Chunk0B005052>();
                return tmCarQuality;
            }
            set
            {
                DiscoverChunks<Chunk0B005034, Chunk0B005052>();
                tmCarQuality = value;
            }
        }

        public int PlayerShadow
        {
            get
            {
                DiscoverChunk<Chunk0B005052>();
                return playerShadow;
            }
            set
            {
                DiscoverChunk<Chunk0B005052>();
                playerShadow = value;
            }
        }

        public int TmOpponents
        {
            get
            {
                DiscoverChunks<Chunk0B005034, Chunk0B005052>();
                return tmOpponents;
            }
            set
            {
                DiscoverChunks<Chunk0B005034, Chunk0B005052>();
                tmOpponents = value;
            }
        }

        public int TmMaxOpponents
        {
            get
            {
                DiscoverChunks<Chunk0B005034, Chunk0B005052>();
                return tmMaxOpponents;
            }
            set
            {
                DiscoverChunks<Chunk0B005034, Chunk0B005052>();
                tmMaxOpponents = value;
            }
        }

        public int TmBackgroundQuality
        {
            get
            {
                DiscoverChunk<Chunk0B005034>();
                return tmBackgroundQuality;
            }
            set
            {
                DiscoverChunk<Chunk0B005034>();
                tmBackgroundQuality = value;
            }
        }

        #endregion

        #region Chunks

        [Chunk(0x0B005008)]
        public class Chunk0B005008 : SkippableChunk<CSystemConfig>
        {
            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.String(ref n.desiredLanguageId);
            }
        }

        [Chunk(0x0B00502B)]
        public class Chunk0B00502B : SkippableChunk<CSystemConfig>
        {
            private int u01;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
                rw.Boolean(ref n.advertising_DisabledByUser);
                rw.Single(ref n.advertising_TunningCoef);
            }
        }

        [Chunk(0x0B005034)]
        public class Chunk0B005034 : SkippableChunk<CSystemConfig>
        {
            private int u01;
            private int u02;
            private bool u03;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public bool U03
            {
                get => u03;
                set => u03 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref n.tmCarQuality);
                rw.Int32(ref u01);
                rw.Int32(ref u02);
                rw.Int32(ref n.tmOpponents);
                rw.Int32(ref n.tmMaxOpponents);
                rw.Boolean(ref u03);
                rw.Int32(ref n.tmBackgroundQuality);
            }
        }

        [Chunk(0x0B005035)]
        public class Chunk0B005035 : SkippableChunk<CSystemConfig>
        {
            private bool u01;

            public bool U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref u01);
                rw.Boolean(ref n.networkTestInternetConnection);
            }
        }

        [Chunk(0x0B005039)]
        public class Chunk0B005039 : SkippableChunk<CSystemConfig>
        {
            private string u01;
            private string u02;

            public string U01
            {
                get => u01;
                set => u01 = value;
            }

            public string U02
            {
                get => u02;
                set => u02 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref n.networkUseProxy);
                rw.String(ref u01);
                rw.String(ref u02);
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

        [Chunk(0x0B00503D)]
        public class Chunk0B00503D : SkippableChunk<CSystemConfig>
        {
            private int u01;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref n.isIgnorePlayerSkins);
                rw.Boolean(ref n.isSkipRollingDemo);
                rw.Int32(ref u01);
            }
        }

        [Chunk(0x0B005041)]
        public class Chunk0B005041 : SkippableChunk<CSystemConfig>
        {
            private string u01;

            public string U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.String(ref u01);
            }
        }

        [Chunk(0x0B005043)]
        public class Chunk0B005043 : SkippableChunk<CSystemConfig>
        {
            private bool u01;

            public bool U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref u01);
                rw.Boolean(ref n.networkTestInternetConnection);
                rw.String(ref n.networkLastUsedMSAddress);
                rw.String(ref n.networkLastUsedMSPath);
            }
        }

        [Chunk(0x0B005044)]
        public class Chunk0B005044 : SkippableChunk<CSystemConfig>
        {
            private int[] u01;

            public int[] U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref n.networkTestInternetConnection);
                rw.String(ref n.networkLastUsedMSAddress);
                rw.String(ref n.networkLastUsedMSPath);
                rw.Array(ref u01);
            }
        }

        [Chunk(0x0B005045)]
        public class Chunk0B005045 : SkippableChunk<CSystemConfig>
        {
            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref n.inputsAlternateMethod);
                rw.Boolean(ref n.inputsFreezeUnusedAxes);
                rw.Boolean(ref n.inputsEnableRumble);
                rw.Boolean(ref n.inputsCaptureKeyboard);
            }
        }

        [Chunk(0x0B005047)]
        public class Chunk0B005047 : SkippableChunk<CSystemConfig>
        {
            private bool u01;

            public bool U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref u01);
            }
        }

        [Chunk(0x0B005048)]
        public class Chunk0B005048 : SkippableChunk<CSystemConfig>
        {
            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref n.gameProfileEnableMulti);
                rw.String(ref n.gameProfileName);
            }
        }

        [Chunk(0x0B00504A)]
        public class Chunk0B00504A : SkippableChunk<CSystemConfig>
        {
            private int u01;
            private int u02;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref n.isIgnorePlayerSkins);
                rw.Boolean(ref n.isSkipRollingDemo);
                rw.Int32(ref u01);
                rw.Int32(ref u02);
            }
        }

        [Chunk(0x0B00504F)]
        public class Chunk0B00504F : SkippableChunk<CSystemConfig>
        {
            private int u01;
            private int u02;
            private int u03;
            private bool u04;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public int U03
            {
                get => u03;
                set => u03 = value;
            }

            public bool U04
            {
                get => u04;
                set => u04 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref n.audioEnabled);
                rw.Single(ref n.audioSoundVolume);
                rw.Single(ref n.audioMusicVolume);
                rw.Int32(ref u01);
                rw.Int32(ref u02);
                rw.Int32(ref u03);
                rw.Boolean(ref n.audioAllowEFX);
                rw.Boolean(ref n.audioDisableDoppler);
                rw.Boolean(ref u04);
                rw.Int32(ref n.audioGlobalQuality);
                rw.String(ref n.audioDevice_Oal);
            }
        }

        [Chunk(0x0B005050)]
        public class Chunk0B005050 : SkippableChunk<CSystemConfig>
        {
            private bool u01;

            public bool U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref u01);
            }
        }

        [Chunk(0x0B005051)]
        public class Chunk0B005051 : SkippableChunk<CSystemConfig>
        {
            private string u01;

            public string U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.String(ref u01);
            }
        }

        [Chunk(0x0B005052)]
        public class Chunk0B005052 : SkippableChunk<CSystemConfig>
        {
            private int u01;
            private int u02;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref n.tmCarQuality);
                rw.Int32(ref u01);
                rw.Int32(ref n.playerShadow);
                rw.Int32(ref u02);
                rw.Int32(ref n.tmOpponents);
                rw.Int32(ref n.tmMaxOpponents);
            }
        }

        [Chunk(0x0B005053)]
        public class Chunk0B005053 : SkippableChunk<CSystemConfig>
        {
            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref n.audioSoundHdr);
            }
        }

        [Chunk(0x0B005056)]
        public class Chunk0B005056 : SkippableChunk<CSystemConfig>
        {
            private bool u01;
            private int u02;
            private bool u03;

            public bool U01
            {
                get => u01;
                set => u01 = value;
            }

            public int U02
            {
                get => u02;
                set => u02 = value;
            }

            public bool U03
            {
                get => u03;
                set => u03 = value;
            }

            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref u01);
                rw.Boolean(ref n.audioAllowHRTF);
                rw.Int32(ref n.audioDontMuteWhenApplicationUnfocused);
                rw.Int32(ref u02);
                rw.Boolean(ref n.audioSoundHdr);
                rw.Boolean(ref u03);
            }
        }

        [Chunk(0x0B005059)]
        public class Chunk0B005059 : SkippableChunk<CSystemConfig>
        {
            public override void ReadWrite(CSystemConfig n, GameBoxReaderWriter rw)
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

        #endregion
    }
}
