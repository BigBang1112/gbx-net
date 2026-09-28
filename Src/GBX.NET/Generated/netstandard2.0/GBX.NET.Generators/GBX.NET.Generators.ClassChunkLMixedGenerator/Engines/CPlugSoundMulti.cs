namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09064000</remarks>
[Class(0x09064000)]
public partial class CPlugSoundMulti : CPlugSound, IClass
{
    [Hexadecimal] public static new uint Id => 0x09064000;




    private CMwNod? additionalSounds;
    [AppliedWithChunk<Chunk09064000>]
    public CMwNod? AdditionalSounds { get => additionalSoundsFile?.GetNode(ref additionalSounds) ?? additionalSounds; set => additionalSounds = value; }
    private Components.GbxRefTableFile? additionalSoundsFile;
    public Components.GbxRefTableFile? AdditionalSoundsFile { get => additionalSoundsFile; set => additionalSoundsFile = value; }
    public CMwNod? GetAdditionalSounds(GbxReadSettings settings = default, bool exceptions = false) => additionalSoundsFile?.GetNode(ref additionalSounds, settings, exceptions) ?? additionalSounds;

    private ESoundInputMapping inputMapping;
    [AppliedWithChunk<Chunk09064000>]
    [AppliedWithChunk<Chunk09064001>]
    [AppliedWithChunk<Chunk09064003>]
    public ESoundInputMapping InputMapping { get => inputMapping; set => inputMapping = value; }

    private External<CMwNod>[]? additionalSounds2;
    [AppliedWithChunk<Chunk09064001>]
    public External<CMwNod>[]? AdditionalSounds2 { get => additionalSounds2; set => additionalSounds2 = value; }

    private float pitchVariancePos;
    [AppliedWithChunk<Chunk09064002>]
    public float PitchVariancePos { get => pitchVariancePos; set => pitchVariancePos = value; }

    private float volumeVariance;
    [AppliedWithChunk<Chunk09064002>]
    public float VolumeVariance { get => volumeVariance; set => volumeVariance = value; }

    private bool avoidDuplicates;
    [AppliedWithChunk<Chunk09064002>]
    public bool AvoidDuplicates { get => avoidDuplicates; set => avoidDuplicates = value; }

    private bool alternateParity;
    [AppliedWithChunk<Chunk09064002>]
    public bool AlternateParity { get => alternateParity; set => alternateParity = value; }

    private float pitchVarianceNeg;
    [AppliedWithChunk<Chunk09064002>]
    public float PitchVarianceNeg { get => pitchVarianceNeg; set => pitchVarianceNeg = value; }

    private Vec3[]? preferedDistances;
    [AppliedWithChunk<Chunk09064003>]
    public Vec3[]? PreferedDistances { get => preferedDistances; set => preferedDistances = value; }

    private CFuncKeysReal? volumeFromInput;
    [AppliedWithChunk<Chunk09064003>]
    public CFuncKeysReal? VolumeFromInput { get => volumeFromInputFile?.GetNode(ref volumeFromInput) ?? volumeFromInput; set => volumeFromInput = value; }
    private Components.GbxRefTableFile? volumeFromInputFile;
    public Components.GbxRefTableFile? VolumeFromInputFile { get => volumeFromInputFile; set => volumeFromInputFile = value; }
    public CFuncKeysReal? GetVolumeFromInput(GbxReadSettings settings = default, bool exceptions = false) => volumeFromInputFile?.GetNode(ref volumeFromInput, settings, exceptions) ?? volumeFromInput;

    /// <summary>
    /// Creates a new instance of <see cref="CPlugSoundMulti"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugSoundMulti() { }


    /// <summary>
    /// CPlugSoundMulti 0x000 chunk
    /// </summary>
    [Chunk(0x09064000)]
    public partial class Chunk09064000 : Chunk<CPlugSoundMulti>
    {
        /// <inheritdoc />
        public override uint Id => 0x09064000;


        public override void ReadWrite(CPlugSoundMulti n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref n.additionalSounds, ref n.additionalSoundsFile);
            rw.EnumInt32<ESoundInputMapping>(ref n.inputMapping);
        }
    }

    /// <summary>
    /// CPlugSoundMulti 0x001 chunk
    /// </summary>
    [Chunk(0x09064001)]
    public partial class Chunk09064001 : Chunk<CPlugSoundMulti>
    {
        /// <inheritdoc />
        public override uint Id => 0x09064001;


        public override void ReadWrite(CPlugSoundMulti n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CMwNod>(ref n.additionalSounds2!);
            rw.EnumInt32<ESoundInputMapping>(ref n.inputMapping);
        }
    }

    /// <summary>
    /// CPlugSoundMulti 0x002 chunk
    /// </summary>
    [Chunk(0x09064002)]
    public partial class Chunk09064002 : Chunk<CPlugSoundMulti>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09064002;

        public int Version { get; set; }


        public override void ReadWrite(CPlugSoundMulti n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref n.pitchVariancePos);
            rw.Single(ref n.volumeVariance);
            if (Version >= 1)
            {
                rw.Boolean(ref n.avoidDuplicates);
                if (Version >= 2)
                {
                    rw.Boolean(ref n.alternateParity);
                    if (Version >= 3)
                    {
                        rw.Single(ref n.pitchVarianceNeg);
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugSoundMulti 0x003 chunk
    /// </summary>
    [Chunk(0x09064003)]
    public partial class Chunk09064003 : Chunk<CPlugSoundMulti>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09064003;

        public int Version { get; set; }

        /// <summary>
        /// is Distance from ESoundInputMapping
        /// </summary>
        public bool U01;

        public override void ReadWrite(CPlugSoundMulti n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref U01); // is Distance from ESoundInputMapping
            rw.Array<Vec3>(ref n.preferedDistances!);
            if (Version >= 1)
            {
                rw.NodeRef<CFuncKeysReal>(ref n.volumeFromInput, ref n.volumeFromInputFile);
                if (Version >= 2)
                {
                    rw.EnumInt32<ESoundInputMapping>(ref n.inputMapping);
                }
            }
        }
    }



    public enum ESoundInputMapping
    {
        Direct,
        ForceRandom,
        Distance,
        Scale,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09064000 => new Chunk09064000(),
        0x09064001 => new Chunk09064001(),
        0x09064002 => new Chunk09064002(),
        0x09064003 => new Chunk09064003(),
        _ => base.NewChunk(chunkId),
    };
}
