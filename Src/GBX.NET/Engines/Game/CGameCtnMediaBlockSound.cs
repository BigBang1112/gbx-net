using GBX.NET.Builders.Engines.Game;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Sound.
/// </summary>
/// <remarks>ID: 0x030A7000</remarks>
[Node(0x030A7000)]
[NodeExtension("GameCtnMediaBlockSound")]
public partial class CGameCtnMediaBlockSound : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private FileRef sound;
    private bool isMusic;
    private bool isLooping;
    private int playCount = 1;
    private bool stopWithClip;
    private bool audioToSpeech;
    private int audioToSpeechTarget;
    private IList<Key> keys;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk030A7001))]
    [AppliedWithChunk(typeof(Chunk030A7004))]
    public FileRef Sound { get => sound; set => sound = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk030A7003))]
    public bool IsMusic { get => isMusic; set => isMusic = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk030A7002))]
    [AppliedWithChunk(typeof(Chunk030A7003))]
    public bool IsLooping { get => isLooping; set => isLooping = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk030A7002))]
    [AppliedWithChunk(typeof(Chunk030A7003))]
    public int PlayCount { get => playCount; set => playCount = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk030A7003), sinceVersion: 1)]
    public bool StopWithClip { get => stopWithClip; set => stopWithClip = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk030A7003), sinceVersion: 2)]
    public bool AudioToSpeech { get => audioToSpeech; set => audioToSpeech = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk030A7003), sinceVersion: 2)]
    public int AudioToSpeechTarget { get => audioToSpeechTarget; set => audioToSpeechTarget = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk030A7001))]
    [AppliedWithChunk(typeof(Chunk030A7004))]
    public IList<Key> Keys { get => keys; set => keys = value; }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockSound()
    {
        sound = FileRef.Default;
        keys = Array.Empty<Key>();
    }

    public static CGameCtnMediaBlockSoundBuilder Create() => new();

    #endregion

    #region Chunks

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockSound 0x001 chunk
    /// </summary>
    [Chunk(0x030A7001)]
    public class Chunk030A7001 : Chunk<CGameCtnMediaBlockSound>
    {
        public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
        {
            rw.FileRef(ref n.sound!);
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnMediaBlockSound 0x002 chunk
    /// </summary>
    [Chunk(0x030A7002)]
    public class Chunk030A7002 : Chunk<CGameCtnMediaBlockSound>
    {
        public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.playCount);
            rw.Boolean(ref n.isLooping);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnMediaBlockSound 0x003 chunk
    /// </summary>
    [Chunk(0x030A7003)]
    public class Chunk030A7003 : Chunk<CGameCtnMediaBlockSound>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref n.playCount);
            rw.Boolean(ref n.isLooping);
            rw.Boolean(ref n.isMusic);

            if (version >= 1) // ManiaPlanet 2-3?
            {
                rw.Boolean(ref n.stopWithClip);

                if (version >= 2) // ManiaPlanet 4
                {
                    rw.Boolean(ref n.audioToSpeech);
                    rw.Int32(ref n.audioToSpeechTarget);
                }
            }
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CGameCtnMediaBlockSound 0x004 chunk
    /// </summary>
    [Chunk(0x030A7004)]
    public class Chunk030A7004 : Chunk<CGameCtnMediaBlockSound>, IVersionable
    {
        private int version = 1;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
        {
            rw.FileRef(ref n.sound!);
            rw.Int32(ref version);
            rw.ListKey(ref n.keys!, version);
        }
    }

    #endregion

    #endregion
}
