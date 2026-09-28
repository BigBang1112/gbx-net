namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0901A000</remarks>
[Class(0x0901A000)]
public partial class CPlugSound : CPlugAudio, IClass
{
    [Hexadecimal] public static new uint Id => 0x0901A000;




    private CMwNod? plugFile;
    /// <summary>
    /// CPlugFileSnd
    /// </summary>
    [AppliedWithChunk<Chunk0901A000>]
    public CMwNod? PlugFile { get => plugFileFile?.GetNode(ref plugFile) ?? plugFile; set => plugFile = value; }
    private Components.GbxRefTableFile? plugFileFile;
    public Components.GbxRefTableFile? PlugFileFile { get => plugFileFile; set => plugFileFile = value; }
    public CMwNod? GetPlugFile(GbxReadSettings settings = default, bool exceptions = false) => plugFileFile?.GetNode(ref plugFile, settings, exceptions) ?? plugFile;

    private EMode mode;
    [AppliedWithChunk<Chunk0901A009>]
    [AppliedWithChunk<Chunk0901A00D>]
    [AppliedWithChunk<Chunk0901A00E>]
    public EMode Mode { get => mode; set => mode = value; }

    private float volume;
    [AppliedWithChunk<Chunk0901A009>]
    [AppliedWithChunk<Chunk0901A00D>]
    public float Volume { get => volume; set => volume = value; }

    private bool isLooping;
    [AppliedWithChunk<Chunk0901A009>]
    [AppliedWithChunk<Chunk0901A00D>]
    [AppliedWithChunk<Chunk0901A00E>]
    public bool IsLooping { get => isLooping; set => isLooping = value; }

    private bool isContinuous;
    [AppliedWithChunk<Chunk0901A009>]
    [AppliedWithChunk<Chunk0901A00D>]
    [AppliedWithChunk<Chunk0901A00E>]
    public bool IsContinuous { get => isContinuous; set => isContinuous = value; }

    private float priority;
    [AppliedWithChunk<Chunk0901A009>]
    [AppliedWithChunk<Chunk0901A00D>]
    [AppliedWithChunk<Chunk0901A00E>]
    public float Priority { get => priority; set => priority = value; }

    private ESoundKind soundKind;
    [AppliedWithChunk<Chunk0901A00B>]
    [AppliedWithChunk<Chunk0901A011>]
    public ESoundKind SoundKind { get => soundKind; set => soundKind = value; }

    private int insideConeAngle;
    [AppliedWithChunk<Chunk0901A00B>]
    [AppliedWithChunk<Chunk0901A011>]
    public int InsideConeAngle { get => insideConeAngle; set => insideConeAngle = value; }

    private int outsideConeAngle;
    [AppliedWithChunk<Chunk0901A00B>]
    [AppliedWithChunk<Chunk0901A011>]
    public int OutsideConeAngle { get => outsideConeAngle; set => outsideConeAngle = value; }

    private float coneOutsideAttenuation;
    [AppliedWithChunk<Chunk0901A00B>]
    [AppliedWithChunk<Chunk0901A011>]
    public float ConeOutsideAttenuation { get => coneOutsideAttenuation; set => coneOutsideAttenuation = value; }

    private float coneOutsideAttenuationHF;
    [AppliedWithChunk<Chunk0901A00B>]
    public float ConeOutsideAttenuationHF { get => coneOutsideAttenuationHF; set => coneOutsideAttenuationHF = value; }

    private float refDistance;
    [AppliedWithChunk<Chunk0901A00C>]
    [AppliedWithChunk<Chunk0901A00F>]
    public float RefDistance { get => refDistance; set => refDistance = value; }

    private float maxDistanceOmni;
    [AppliedWithChunk<Chunk0901A00C>]
    public float MaxDistanceOmni { get => maxDistanceOmni; set => maxDistanceOmni = value; }

    private bool enableDoppler;
    [AppliedWithChunk<Chunk0901A00C>]
    [AppliedWithChunk<Chunk0901A00F>]
    public bool EnableDoppler { get => enableDoppler; set => enableDoppler = value; }

    private float volumeAttenuationDirect;
    [AppliedWithChunk<Chunk0901A00C>]
    public float VolumeAttenuationDirect { get => volumeAttenuationDirect; set => volumeAttenuationDirect = value; }

    private float volumeAttenuationDirectHF;
    [AppliedWithChunk<Chunk0901A00C>]
    public float VolumeAttenuationDirectHF { get => volumeAttenuationDirectHF; set => volumeAttenuationDirectHF = value; }

    private float volumeAttenuationRoom;
    [AppliedWithChunk<Chunk0901A00C>]
    public float VolumeAttenuationRoom { get => volumeAttenuationRoom; set => volumeAttenuationRoom = value; }

    private float volumeAttenuationRoomHF;
    [AppliedWithChunk<Chunk0901A00C>]
    public float VolumeAttenuationRoomHF { get => volumeAttenuationRoomHF; set => volumeAttenuationRoomHF = value; }

    private float dopplerFactor;
    [AppliedWithChunk<Chunk0901A00C>]
    [AppliedWithChunk<Chunk0901A00F>]
    public float DopplerFactor { get => dopplerFactor; set => dopplerFactor = value; }

    private float rolloffFactor;
    [AppliedWithChunk<Chunk0901A00C>]
    [AppliedWithChunk<Chunk0901A00F>]
    public float RolloffFactor { get => rolloffFactor; set => rolloffFactor = value; }

    private float roomRolloffFactor;
    [AppliedWithChunk<Chunk0901A00C>]
    [AppliedWithChunk<Chunk0901A00F>]
    public float RoomRolloffFactor { get => roomRolloffFactor; set => roomRolloffFactor = value; }

    private float airAbsorptionFactor;
    [AppliedWithChunk<Chunk0901A00C>]
    [AppliedWithChunk<Chunk0901A00F>]
    public float AirAbsorptionFactor { get => airAbsorptionFactor; set => airAbsorptionFactor = value; }

    private bool useLowPassFilter;
    [AppliedWithChunk<Chunk0901A00E>]
    public bool UseLowPassFilter { get => useLowPassFilter; set => useLowPassFilter = value; }

    private int maxDuplicates;
    [AppliedWithChunk<Chunk0901A00E>]
    public int MaxDuplicates { get => maxDuplicates; set => maxDuplicates = value; }

    private int balanceGroup;
    [AppliedWithChunk<Chunk0901A00E>]
    public int BalanceGroup { get => balanceGroup; set => balanceGroup = value; }

    private int duplicatesIntervalMin;
    [AppliedWithChunk<Chunk0901A00E>]
    public int DuplicatesIntervalMin { get => duplicatesIntervalMin; set => duplicatesIntervalMin = value; }

    private float fadeStopDuration;
    [AppliedWithChunk<Chunk0901A00E>]
    public float FadeStopDuration { get => fadeStopDuration; set => fadeStopDuration = value; }

    private float pitch;
    [AppliedWithChunk<Chunk0901A00E>]
    public float Pitch { get => pitch; set => pitch = value; }

    private float fadePlayDuration;
    [AppliedWithChunk<Chunk0901A00E>]
    public float FadePlayDuration { get => fadePlayDuration; set => fadePlayDuration = value; }

    private string? groupDuplicate;
    [AppliedWithChunk<Chunk0901A00E>]
    public string? GroupDuplicate { get => groupDuplicate; set => groupDuplicate = value; }

    private float maxDistance;
    [AppliedWithChunk<Chunk0901A00F>]
    public float MaxDistance { get => maxDistance; set => maxDistance = value; }

    private CFuncKeysReal? volumeFromDistance;
    [AppliedWithChunk<Chunk0901A00F>]
    public CFuncKeysReal? VolumeFromDistance { get => volumeFromDistance; set => volumeFromDistance = value; }

    private int roomFxSend;
    [AppliedWithChunk<Chunk0901A00F>]
    public int RoomFxSend { get => roomFxSend; set => roomFxSend = value; }

    private int pitchFromDistMode;
    [AppliedWithChunk<Chunk0901A00F>]
    public int PitchFromDistMode { get => pitchFromDistMode; set => pitchFromDistMode = value; }

    private int ignoreSourceProperties;
    [AppliedWithChunk<Chunk0901A00F>]
    public int IgnoreSourceProperties { get => ignoreSourceProperties; set => ignoreSourceProperties = value; }

    private CFuncKeysReal? pitchFromDistance;
    [AppliedWithChunk<Chunk0901A00F>]
    public CFuncKeysReal? PitchFromDistance { get => pitchFromDistance; set => pitchFromDistance = value; }

    private CFuncKeysReal? volumeFormSpeedKmh;
    [AppliedWithChunk<Chunk0901A00F>]
    public CFuncKeysReal? VolumeFormSpeedKmh { get => volumeFormSpeedKmh; set => volumeFormSpeedKmh = value; }

    private float radius;
    [AppliedWithChunk<Chunk0901A00F>]
    public float Radius { get => radius; set => radius = value; }

    private float panAngleDeg;
    [AppliedWithChunk<Chunk0901A00F>]
    public float PanAngleDeg { get => panAngleDeg; set => panAngleDeg = value; }

    private CPlugSound[]? oldSubSources;
    [AppliedWithChunk<Chunk0901A010>]
    public CPlugSound[]? OldSubSources { get => oldSubSources; set => oldSubSources = value; }

    private CPlugSound? backingSound;
    [AppliedWithChunk<Chunk0901A012>]
    public CPlugSound? BackingSound { get => backingSoundFile?.GetNode(ref backingSound) ?? backingSound; set => backingSound = value; }
    private Components.GbxRefTableFile? backingSoundFile;
    public Components.GbxRefTableFile? BackingSoundFile { get => backingSoundFile; set => backingSoundFile = value; }
    public CPlugSound? GetBackingSound(GbxReadSettings settings = default, bool exceptions = false) => backingSoundFile?.GetNode(ref backingSound, settings, exceptions) ?? backingSound;

    private CPlugSound? focusedSound;
    [AppliedWithChunk<Chunk0901A013>]
    public CPlugSound? FocusedSound { get => focusedSoundFile?.GetNode(ref focusedSound) ?? focusedSound; set => focusedSound = value; }
    private Components.GbxRefTableFile? focusedSoundFile;
    public Components.GbxRefTableFile? FocusedSoundFile { get => focusedSoundFile; set => focusedSoundFile = value; }
    public CPlugSound? GetFocusedSound(GbxReadSettings settings = default, bool exceptions = false) => focusedSoundFile?.GetNode(ref focusedSound, settings, exceptions) ?? focusedSound;

    /// <summary>
    /// Creates a new instance of <see cref="CPlugSound"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugSound() { }


    /// <summary>
    /// CPlugSound 0x000 chunk
    /// </summary>
    [Chunk(0x0901A000)]
    public partial class Chunk0901A000 : Chunk<CPlugSound>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901A000;


        public override void ReadWrite(CPlugSound n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref n.plugFile, ref n.plugFileFile); // CPlugFileSnd
        }
    }

    /// <summary>
    /// CPlugSound 0x002 chunk
    /// </summary>
    [Chunk(0x0901A002)]
    public partial class Chunk0901A002 : Chunk<CPlugSound>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901A002;

        public string? U01;

        public override void ReadWrite(CPlugSound n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    /// <summary>
    /// CPlugSound 0x009 chunk
    /// </summary>
    [Chunk(0x0901A009)]
    public partial class Chunk0901A009 : Chunk<CPlugSound>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901A009;


        public override void ReadWrite(CPlugSound n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EMode>(ref n.mode);
            rw.Single(ref n.volume);
            rw.Boolean(ref n.isLooping);
            rw.Boolean(ref n.isContinuous);
            rw.Single(ref n.priority);
        }
    }

    /// <summary>
    /// CPlugSound 0x00B chunk
    /// </summary>
    [Chunk(0x0901A00B)]
    public partial class Chunk0901A00B : Chunk<CPlugSound>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901A00B;


        public override void ReadWrite(CPlugSound n, GbxReaderWriter rw)
        {
            rw.EnumInt32<ESoundKind>(ref n.soundKind);
            rw.Int32(ref n.insideConeAngle);
            rw.Int32(ref n.outsideConeAngle);
            rw.Single(ref n.coneOutsideAttenuation);
            rw.Single(ref n.coneOutsideAttenuationHF);
        }
    }

    /// <summary>
    /// CPlugSound 0x00C chunk
    /// </summary>
    [Chunk(0x0901A00C)]
    public partial class Chunk0901A00C : Chunk<CPlugSound>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901A00C;


        public override void ReadWrite(CPlugSound n, GbxReaderWriter rw)
        {
            rw.Single(ref n.refDistance);
            rw.Single(ref n.maxDistanceOmni);
            rw.Boolean(ref n.enableDoppler);
            rw.Single(ref n.volumeAttenuationDirect);
            rw.Single(ref n.volumeAttenuationDirectHF);
            rw.Single(ref n.volumeAttenuationRoom);
            rw.Single(ref n.volumeAttenuationRoomHF);
            rw.Single(ref n.dopplerFactor);
            rw.Single(ref n.rolloffFactor);
            rw.Single(ref n.roomRolloffFactor);
            rw.Single(ref n.airAbsorptionFactor);
        }
    }

    /// <summary>
    /// CPlugSound 0x00D chunk
    /// </summary>
    [Chunk(0x0901A00D)]
    public partial class Chunk0901A00D : Chunk<CPlugSound>
    {
        /// <inheritdoc />
        public override uint Id => 0x0901A00D;


        public override void ReadWrite(CPlugSound n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EMode>(ref n.mode);
            rw.Single(ref n.volume);
            rw.Boolean(ref n.isLooping);
            rw.Boolean(ref n.isContinuous);
            rw.Single(ref n.priority);
        }
    }

    /// <summary>
    /// CPlugSound 0x00E chunk
    /// </summary>
    [Chunk(0x0901A00E)]
    public partial class Chunk0901A00E : Chunk<CPlugSound>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0901A00E;

        public int Version { get; set; }

        public float U01;

        public override void ReadWrite(CPlugSound n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.EnumInt32<EMode>(ref n.mode);
            rw.Single(ref U01);
            rw.Boolean(ref n.isLooping);
            rw.Boolean(ref n.isContinuous);
            rw.Single(ref n.priority);
            if (Version >= 1)
            {
                rw.Boolean(ref n.useLowPassFilter);
                if (Version >= 2)
                {
                    rw.Int32(ref n.maxDuplicates);
                    if (Version >= 3)
                    {
                        rw.Int32(ref n.balanceGroup);
                        if (Version >= 4)
                        {
                            rw.Int32(ref n.duplicatesIntervalMin);
                            if (Version >= 5)
                            {
                                rw.Single(ref n.fadeStopDuration);
                                if (Version >= 6)
                                {
                                    rw.Single(ref n.pitch);
                                    if (Version >= 7)
                                    {
                                        rw.Single(ref n.fadePlayDuration);
                                        if (Version >= 9)
                                        {
                                            rw.Id(ref n.groupDuplicate);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugSound 0x00F chunk
    /// </summary>
    [Chunk(0x0901A00F)]
    public partial class Chunk0901A00F : Chunk<CPlugSound>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0901A00F;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;

        public override void ReadWrite(CPlugSound n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref n.refDistance);
            rw.Single(ref n.maxDistance);
            rw.Boolean(ref n.enableDoppler);
            rw.Single(ref n.dopplerFactor);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            if (Version >= 1)
            {
                rw.Single(ref n.rolloffFactor);
                if (Version >= 2)
                {
                    rw.NodeRef<CFuncKeysReal>(ref n.volumeFromDistance);
                    if (Version >= 3)
                    {
                        rw.Int32(ref n.roomFxSend);
                        if (Version >= 4)
                        {
                            rw.Single(ref n.roomRolloffFactor);
                            rw.Single(ref n.airAbsorptionFactor);
                            if (Version >= 7)
                            {
                                rw.Int32(ref n.pitchFromDistMode);
                                rw.Int32(ref n.ignoreSourceProperties);
                                if (Version >= 8)
                                {
                                    rw.NodeRef<CFuncKeysReal>(ref n.pitchFromDistance);
                                    if (Version >= 9)
                                    {
                                        rw.NodeRef<CFuncKeysReal>(ref n.volumeFormSpeedKmh);
                                        if (Version >= 10)
                                        {
                                            rw.Single(ref n.radius);
                                            if (Version >= 11)
                                            {
                                                rw.Single(ref n.panAngleDeg);
                                                if (Version >= 12)
                                                {
                                                    rw.Single(ref U07);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugSound 0x010 chunk
    /// </summary>
    [Chunk(0x0901A010)]
    public partial class Chunk0901A010 : Chunk<CPlugSound>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0901A010;

        public int Version { get; set; }


        public override void ReadWrite(CPlugSound n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayNodeRef_deprec<CPlugSound>(ref n.oldSubSources!);
        }
    }

    /// <summary>
    /// CPlugSound 0x011 chunk
    /// </summary>
    [Chunk(0x0901A011)]
    public partial class Chunk0901A011 : Chunk<CPlugSound>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0901A011;

        public int Version { get; set; }


        public override void ReadWrite(CPlugSound n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.EnumInt32<ESoundKind>(ref n.soundKind);
            rw.Int32(ref n.insideConeAngle);
            rw.Int32(ref n.outsideConeAngle);
            rw.Single(ref n.coneOutsideAttenuation);
        }
    }

    /// <summary>
    /// CPlugSound 0x012 chunk
    /// </summary>
    [Chunk(0x0901A012)]
    public partial class Chunk0901A012 : Chunk<CPlugSound>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0901A012;

        public int Version { get; set; }


        public override void ReadWrite(CPlugSound n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugSound>(ref n.backingSound, ref n.backingSoundFile);
        }
    }

    /// <summary>
    /// CPlugSound 0x013 chunk
    /// </summary>
    [Chunk(0x0901A013)]
    public partial class Chunk0901A013 : Chunk<CPlugSound>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0901A013;

        public int Version { get; set; }


        public override void ReadWrite(CPlugSound n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugSound>(ref n.focusedSound, ref n.focusedSoundFile);
        }
    }



    public enum EMode
    {
        Direct,
        Direct_w__attenuation,
        Spatialised,
        Spatialised_Omni,
        ForceHard3d,
    }

    public enum ESoundKind
    {
        Point,
        Directional,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0901A000 => new Chunk0901A000(),
        0x0901A002 => new Chunk0901A002(),
        0x0901A009 => new Chunk0901A009(),
        0x0901A00B => new Chunk0901A00B(),
        0x0901A00C => new Chunk0901A00C(),
        0x0901A00D => new Chunk0901A00D(),
        0x0901A00E => new Chunk0901A00E(),
        0x0901A00F => new Chunk0901A00F(),
        0x0901A010 => new Chunk0901A010(),
        0x0901A011 => new Chunk0901A011(),
        0x0901A012 => new Chunk0901A012(),
        0x0901A013 => new Chunk0901A013(),
        _ => base.NewChunk(chunkId),
    };
}
