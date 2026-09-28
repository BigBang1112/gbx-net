namespace GBX.NET.Engines.Hms;

/// <remarks>ID: 0x0600D000</remarks>
[Class(0x0600D000)]
public partial class CHmsSoundSource : CHmsPoc, IClass
{
    [Hexadecimal] public static new uint Id => 0x0600D000;




    private CPlugSound? plugSound;
    [AppliedWithChunk<Chunk0600D000>]
    [AppliedWithChunk<Chunk0600D002>]
    [AppliedWithChunk<Chunk0600D005>]
    public CPlugSound? PlugSound { get => plugSoundFile?.GetNode(ref plugSound) ?? plugSound; set => plugSound = value; }
    private Components.GbxRefTableFile? plugSoundFile;
    public Components.GbxRefTableFile? PlugSoundFile { get => plugSoundFile; set => plugSoundFile = value; }
    public CPlugSound? GetPlugSound(GbxReadSettings settings = default, bool exceptions = false) => plugSoundFile?.GetNode(ref plugSound, settings, exceptions) ?? plugSound;

    private float volume;
    [AppliedWithChunk<Chunk0600D001>]
    [AppliedWithChunk<Chunk0600D003>]
    public float Volume { get => volume; set => volume = value; }

    private float pitch;
    [AppliedWithChunk<Chunk0600D001>]
    [AppliedWithChunk<Chunk0600D003>]
    public float Pitch { get => pitch; set => pitch = value; }

    private float priorityAdjustement;
    [AppliedWithChunk<Chunk0600D002>]
    [AppliedWithChunk<Chunk0600D005>]
    public float PriorityAdjustement { get => priorityAdjustement; set => priorityAdjustement = value; }

    private bool useLowQuality;
    [AppliedWithChunk<Chunk0600D002>]
    [AppliedWithChunk<Chunk0600D005>]
    public bool UseLowQuality { get => useLowQuality; set => useLowQuality = value; }

    private float rpmOrSpeed;
    [AppliedWithChunk<Chunk0600D003>]
    public float RpmOrSpeed { get => rpmOrSpeed; set => rpmOrSpeed = value; }

    private Vec3 volumicSize;
    [AppliedWithChunk<Chunk0600D004>]
    public Vec3 VolumicSize { get => volumicSize; set => volumicSize = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CHmsSoundSource"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CHmsSoundSource() { }


    /// <summary>
    /// CHmsSoundSource 0x000 chunk
    /// </summary>
    [Chunk(0x0600D000)]
    public partial class Chunk0600D000 : Chunk<CHmsSoundSource>
    {
        /// <inheritdoc />
        public override uint Id => 0x0600D000;


        public override void ReadWrite(CHmsSoundSource n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugSound>(ref n.plugSound, ref n.plugSoundFile);
        }
    }

    /// <summary>
    /// CHmsSoundSource 0x001 chunk
    /// </summary>
    [Chunk(0x0600D001)]
    public partial class Chunk0600D001 : Chunk<CHmsSoundSource>
    {
        /// <inheritdoc />
        public override uint Id => 0x0600D001;


        public override void ReadWrite(CHmsSoundSource n, GbxReaderWriter rw)
        {
            rw.Single(ref n.volume);
            rw.Single(ref n.pitch);
        }
    }

    /// <summary>
    /// CHmsSoundSource 0x002 chunk
    /// </summary>
    [Chunk(0x0600D002)]
    public partial class Chunk0600D002 : Chunk<CHmsSoundSource>
    {
        /// <inheritdoc />
        public override uint Id => 0x0600D002;


        public override void ReadWrite(CHmsSoundSource n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugSound>(ref n.plugSound, ref n.plugSoundFile);
            rw.Single(ref n.priorityAdjustement);
            rw.Boolean(ref n.useLowQuality);
        }
    }

    /// <summary>
    /// CHmsSoundSource 0x003 chunk (volume, pitch, speed)
    /// </summary>
    [Chunk(0x0600D003, "volume, pitch, speed")]
    public partial class Chunk0600D003 : Chunk<CHmsSoundSource>
    {
        /// <inheritdoc />
        public override uint Id => 0x0600D003;


        public override void ReadWrite(CHmsSoundSource n, GbxReaderWriter rw)
        {
            rw.Single(ref n.volume);
            rw.Single(ref n.pitch);
            rw.Single(ref n.rpmOrSpeed);
        }
    }

    /// <summary>
    /// CHmsSoundSource 0x004 chunk (VolumicSize)
    /// </summary>
    [Chunk(0x0600D004, "VolumicSize")]
    public partial class Chunk0600D004 : Chunk<CHmsSoundSource>
    {
        /// <inheritdoc />
        public override uint Id => 0x0600D004;


        public override void ReadWrite(CHmsSoundSource n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.volumicSize);
        }
    }

    /// <summary>
    /// CHmsSoundSource 0x005 chunk
    /// </summary>
    [Chunk(0x0600D005)]
    public partial class Chunk0600D005 : Chunk<CHmsSoundSource>
    {
        /// <inheritdoc />
        public override uint Id => 0x0600D005;


        public override void ReadWrite(CHmsSoundSource n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugSound>(ref n.plugSound, ref n.plugSoundFile);
            rw.Single(ref n.priorityAdjustement);
            rw.Boolean(ref n.useLowQuality);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0600D000 => new Chunk0600D000(),
        0x0600D001 => new Chunk0600D001(),
        0x0600D002 => new Chunk0600D002(),
        0x0600D003 => new Chunk0600D003(),
        0x0600D004 => new Chunk0600D004(),
        0x0600D005 => new Chunk0600D005(),
        _ => base.NewChunk(chunkId),
    };
}
