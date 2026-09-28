namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09039000</remarks>
[Class(0x09039000)]
public partial class CPlugAudioEnvironment : CPlugAudio, IClass
{
    [Hexadecimal] public static new uint Id => 0x09039000;




    private EEAXPreset eAXPreset;
    [AppliedWithChunk<Chunk09039000>]
    public EEAXPreset EAXPreset { get => eAXPreset; set => eAXPreset = value; }

    private float sizeFactor;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float SizeFactor { get => sizeFactor; set => sizeFactor = value; }

    private float diffusion;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float Diffusion { get => diffusion; set => diffusion = value; }

    private float room;
    [AppliedWithChunk<Chunk09039000>]
    public float Room { get => room; set => room = value; }

    private float roomHFRatio;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float RoomHFRatio { get => roomHFRatio; set => roomHFRatio = value; }

    private float roomLFRatio;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float RoomLFRatio { get => roomLFRatio; set => roomLFRatio = value; }

    private float decayTime;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float DecayTime { get => decayTime; set => decayTime = value; }

    private float decayHFRatio;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float DecayHFRatio { get => decayHFRatio; set => decayHFRatio = value; }

    private float decayLFRatio;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float DecayLFRatio { get => decayLFRatio; set => decayLFRatio = value; }

    private float reflections;
    [AppliedWithChunk<Chunk09039000>]
    public float Reflections { get => reflections; set => reflections = value; }

    private float reflectionsDelay;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float ReflectionsDelay { get => reflectionsDelay; set => reflectionsDelay = value; }

    private float reverb;
    [AppliedWithChunk<Chunk09039000>]
    public float Reverb { get => reverb; set => reverb = value; }

    private float reverbDelay;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float ReverbDelay { get => reverbDelay; set => reverbDelay = value; }

    private float echoTime;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float EchoTime { get => echoTime; set => echoTime = value; }

    private float echoDepth;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float EchoDepth { get => echoDepth; set => echoDepth = value; }

    private float modulationTime;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float ModulationTime { get => modulationTime; set => modulationTime = value; }

    private float modulationDepth;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float ModulationDepth { get => modulationDepth; set => modulationDepth = value; }

    private float rolloffFactor;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float RolloffFactor { get => rolloffFactor; set => rolloffFactor = value; }

    private float airAbsorbtionHF;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float AirAbsorbtionHF { get => airAbsorbtionHF; set => airAbsorbtionHF = value; }

    private float hFReference;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float HFReference { get => hFReference; set => hFReference = value; }

    private float lFReference;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public float LFReference { get => lFReference; set => lFReference = value; }

    private bool decayTimeScale;
    [AppliedWithChunk<Chunk09039000>]
    public bool DecayTimeScale { get => decayTimeScale; set => decayTimeScale = value; }

    private bool reflectionsScale;
    [AppliedWithChunk<Chunk09039000>]
    public bool ReflectionsScale { get => reflectionsScale; set => reflectionsScale = value; }

    private bool reflectionsDelayScale;
    [AppliedWithChunk<Chunk09039000>]
    public bool ReflectionsDelayScale { get => reflectionsDelayScale; set => reflectionsDelayScale = value; }

    private bool reverbScale;
    [AppliedWithChunk<Chunk09039000>]
    public bool ReverbScale { get => reverbScale; set => reverbScale = value; }

    private bool reverbDelayScale;
    [AppliedWithChunk<Chunk09039000>]
    public bool ReverbDelayScale { get => reverbDelayScale; set => reverbDelayScale = value; }

    private bool echoTimeScale;
    [AppliedWithChunk<Chunk09039000>]
    public bool EchoTimeScale { get => echoTimeScale; set => echoTimeScale = value; }

    private bool modulationTimeScale;
    [AppliedWithChunk<Chunk09039000>]
    public bool ModulationTimeScale { get => modulationTimeScale; set => modulationTimeScale = value; }

    private bool decayHFLimitScale;
    [AppliedWithChunk<Chunk09039000>]
    [AppliedWithChunk<Chunk09039002>]
    public bool DecayHFLimitScale { get => decayHFLimitScale; set => decayHFLimitScale = value; }

    private float dopplerFactor;
    [AppliedWithChunk<Chunk09039001>]
    public float DopplerFactor { get => dopplerFactor; set => dopplerFactor = value; }

    private float gain;
    [AppliedWithChunk<Chunk09039002>]
    public float Gain { get => gain; set => gain = value; }

    private float reflectionsGain;
    [AppliedWithChunk<Chunk09039002>]
    public float ReflectionsGain { get => reflectionsGain; set => reflectionsGain = value; }

    private float lateReverbGain;
    [AppliedWithChunk<Chunk09039002>]
    public float LateReverbGain { get => lateReverbGain; set => lateReverbGain = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugAudioEnvironment"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugAudioEnvironment() { }


    /// <summary>
    /// CPlugAudioEnvironment 0x000 chunk
    /// </summary>
    [Chunk(0x09039000)]
    public partial class Chunk09039000 : Chunk<CPlugAudioEnvironment>
    {
        /// <inheritdoc />
        public override uint Id => 0x09039000;


        public override void ReadWrite(CPlugAudioEnvironment n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EEAXPreset>(ref n.eAXPreset);
            rw.Single(ref n.sizeFactor);
            rw.Single(ref n.diffusion);
            rw.Single(ref n.room);
            rw.Single(ref n.roomHFRatio);
            rw.Single(ref n.roomLFRatio);
            rw.Single(ref n.decayTime);
            rw.Single(ref n.decayHFRatio);
            rw.Single(ref n.decayLFRatio);
            rw.Single(ref n.reflections);
            rw.Single(ref n.reflectionsDelay);
            rw.Single(ref n.reverb);
            rw.Single(ref n.reverbDelay);
            rw.Single(ref n.echoTime);
            rw.Single(ref n.echoDepth);
            rw.Single(ref n.modulationTime);
            rw.Single(ref n.modulationDepth);
            rw.Single(ref n.rolloffFactor);
            rw.Single(ref n.airAbsorbtionHF);
            rw.Single(ref n.hFReference);
            rw.Single(ref n.lFReference);
            rw.Boolean(ref n.decayTimeScale);
            rw.Boolean(ref n.reflectionsScale);
            rw.Boolean(ref n.reflectionsDelayScale);
            rw.Boolean(ref n.reverbScale);
            rw.Boolean(ref n.reverbDelayScale);
            rw.Boolean(ref n.echoTimeScale);
            rw.Boolean(ref n.modulationTimeScale);
            rw.Boolean(ref n.decayHFLimitScale);
        }
    }

    /// <summary>
    /// CPlugAudioEnvironment 0x001 chunk
    /// </summary>
    [Chunk(0x09039001)]
    public partial class Chunk09039001 : Chunk<CPlugAudioEnvironment>
    {
        /// <inheritdoc />
        public override uint Id => 0x09039001;


        public override void ReadWrite(CPlugAudioEnvironment n, GbxReaderWriter rw)
        {
            rw.Single(ref n.dopplerFactor);
        }
    }

    /// <summary>
    /// CPlugAudioEnvironment 0x002 chunk
    /// </summary>
    [Chunk(0x09039002)]
    public partial class Chunk09039002 : Chunk<CPlugAudioEnvironment>
    {
        /// <inheritdoc />
        public override uint Id => 0x09039002;


        public override void ReadWrite(CPlugAudioEnvironment n, GbxReaderWriter rw)
        {
            rw.Single(ref n.sizeFactor);
            rw.Single(ref n.diffusion);
            rw.Single(ref n.gain);
            rw.Single(ref n.roomHFRatio);
            rw.Single(ref n.roomLFRatio);
            rw.Single(ref n.decayTime);
            rw.Single(ref n.decayHFRatio);
            rw.Single(ref n.decayLFRatio);
            rw.Single(ref n.reflectionsGain);
            rw.Single(ref n.reflectionsDelay);
            rw.Single(ref n.lateReverbGain);
            rw.Single(ref n.reverbDelay);
            rw.Single(ref n.echoTime);
            rw.Single(ref n.echoDepth);
            rw.Single(ref n.modulationTime);
            rw.Single(ref n.modulationDepth);
            rw.Single(ref n.rolloffFactor);
            rw.Single(ref n.hFReference);
            rw.Single(ref n.lFReference);
            rw.Single(ref n.airAbsorbtionHF); // NOT REALLY SURE
            rw.Boolean(ref n.decayHFLimitScale);
        }
    }

    /// <summary>
    /// CPlugAudioEnvironment 0x004 chunk
    /// </summary>
    [Chunk(0x09039004)]
    public partial class Chunk09039004 : Chunk<CPlugAudioEnvironment>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09039004;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;

        public override void ReadWrite(CPlugAudioEnvironment n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            if (Version >= 1)
            {
                rw.Single(ref U02);
                if (Version >= 2)
                {
                    rw.Single(ref U03);
                    rw.Single(ref U04);
                    rw.Single(ref U05);
                    rw.Single(ref U06);
                    rw.Single(ref U07);
                    rw.Single(ref U08);
                    rw.Single(ref U09);
                    rw.Single(ref U10);
                    if (Version >= 3)
                    {
                        rw.Single(ref U11);
                        rw.Single(ref U12);
                        rw.Single(ref U13);
                        rw.Single(ref U14);
                    }
                }
            }
        }
    }



    public enum EEAXPreset
    {
        Generic,
        PaddedCell,
        Room,
        Bathroom,
        LivingRoom,
        Stoneroom,
        Auditorium,
        ConcertHall,
        Cave,
        Arena,
        Hangar,
        CarpetedHallway,
        Hallway,
        StoneCorridor,
        Alley,
        Forest,
        City,
        Mountains,
        Quarry,
        Plain,
        ParkingLot,
        Sewerpipe,
        Underwater,
        Drugged,
        Dizzy,
        Psychotic,
        Undefined,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09039000 => new Chunk09039000(),
        0x09039001 => new Chunk09039001(),
        0x09039002 => new Chunk09039002(),
        0x09039004 => new Chunk09039004(),
        _ => base.NewChunk(chunkId),
    };
}
