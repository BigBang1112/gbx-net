namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030A7000</remarks>
[Class(0x030A7000)]
public partial class CGameCtnMediaBlockSound : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x030A7000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private PackDesc? sound;
    [AppliedWithChunk<Chunk030A7001>]
    [AppliedWithChunk<Chunk030A7004>]
    public PackDesc? Sound { get => sound; set => sound = value; }

    private List<Key>? keys;
    [AppliedWithChunk<Chunk030A7001>]
    [AppliedWithChunk<Chunk030A7004>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    private int playCount;
    [AppliedWithChunk<Chunk030A7002>]
    [AppliedWithChunk<Chunk030A7003>]
    public int PlayCount { get => playCount; set => playCount = value; }

    private bool isLooping;
    [AppliedWithChunk<Chunk030A7002>]
    [AppliedWithChunk<Chunk030A7003>]
    public bool IsLooping { get => isLooping; set => isLooping = value; }

    private bool isMusic;
    [AppliedWithChunk<Chunk030A7003>]
    public bool IsMusic { get => isMusic; set => isMusic = value; }

    private bool stopWithClip;
    [AppliedWithChunk<Chunk030A7003>]
    public bool StopWithClip { get => stopWithClip; set => stopWithClip = value; }

    private bool audioToSpeech;
    [AppliedWithChunk<Chunk030A7003>]
    public bool AudioToSpeech { get => audioToSpeech; set => audioToSpeech = value; }

    private int audioToSpeechTarget;
    [AppliedWithChunk<Chunk030A7003>]
    public int AudioToSpeechTarget { get => audioToSpeechTarget; set => audioToSpeechTarget = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockSound"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockSound() { }


    /// <summary>
    /// CGameCtnMediaBlockSound 0x001 chunk
    /// </summary>
    [Chunk(0x030A7001)]
    public partial class Chunk030A7001 : Chunk<CGameCtnMediaBlockSound>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A7001;


        public override void ReadWrite(CGameCtnMediaBlockSound n, GbxReaderWriter rw)
        {
            rw.PackDesc(ref n.sound);
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockSound 0x002 chunk
    /// </summary>
    [Chunk(0x030A7002)]
    public partial class Chunk030A7002 : Chunk<CGameCtnMediaBlockSound>
    {
        /// <inheritdoc />
        public override uint Id => 0x030A7002;


        public override void ReadWrite(CGameCtnMediaBlockSound n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.playCount);
            rw.Boolean(ref n.isLooping);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockSound 0x003 chunk
    /// </summary>
    [Chunk(0x030A7003)]
    public partial class Chunk030A7003 : Chunk<CGameCtnMediaBlockSound>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x030A7003;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockSound n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.playCount);
            rw.Boolean(ref n.isLooping);
            rw.Boolean(ref n.isMusic);
            if (Version >= 1)
            {
                rw.Boolean(ref n.stopWithClip);
                if (Version >= 2)
                {
                    rw.Boolean(ref n.audioToSpeech);
                    rw.Int32(ref n.audioToSpeechTarget);
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockSound 0x004 chunk
    /// </summary>
    [Chunk(0x030A7004)]
    public partial class Chunk030A7004 : Chunk<CGameCtnMediaBlockSound>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x030A7004;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockSound n, GbxReaderWriter rw)
        {
            rw.PackDesc(ref n.sound);
            rw.VersionInt32(this);
            rw.ListReadableWritable<Key>(ref n.keys!, version: Version);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float volume;
        public float Volume { get => volume; set => volume = value; }

        private float pan;
        public float Pan { get => pan; set => pan = value; }

        private Vec3 position;
        public Vec3 Position { get => position; set => position = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref volume);
            rw.Single(ref pan);
            if (v >= 1)
            {
                rw.Vec3(ref position);
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x030A7001 => new Chunk030A7001(),
        0x030A7002 => new Chunk030A7002(),
        0x030A7003 => new Chunk030A7003(),
        0x030A7004 => new Chunk030A7004(),
        _ => base.NewChunk(chunkId),
    };
}
