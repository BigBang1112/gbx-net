using GBX.NET.Builders.Engines.Game;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Sound (0x030A7000)
/// </summary>
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
    public FileRef Sound
    {
        get => sound;
        set => sound = value;
    }

    [NodeMember]
    public bool IsMusic
    {
        get => isMusic;
        set => isMusic = value;
    }

    [NodeMember]
    public bool IsLooping
    {
        get => isLooping;
        set => isLooping = value;
    }

    [NodeMember]
    public int PlayCount
    {
        get => playCount;
        set => playCount = value;
    }

    [NodeMember]
    public bool StopWithClip
    {
        get => stopWithClip;
        set => stopWithClip = value;
    }

    [NodeMember]
    public bool AudioToSpeech
    {
        get => audioToSpeech;
        set => audioToSpeech = value;
    }

    [NodeMember]
    public int AudioToSpeechTarget
    {
        get => audioToSpeechTarget;
        set => audioToSpeechTarget = value;
    }

    [NodeMember]
    public IList<Key> Keys
    {
        get => keys;
        set => keys = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockSound()
    {
        sound = null!;
        keys = null!;
    }

    public static CGameCtnMediaBlockSoundBuilder Create() => new();

    #endregion

    #region Chunks

    #region 0x001 chunk

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

    [Chunk(0x030A7003)]
    public class Chunk030A7003 : Chunk<CGameCtnMediaBlockSound>, IVersionable
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

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

    [Chunk(0x030A7004)]
    public class Chunk030A7004 : Chunk<CGameCtnMediaBlockSound>, IVersionable
    {
        private int version = 1;

        public int Version
        {
            get => version;
            set => version = value;
        }

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
