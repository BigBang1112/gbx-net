namespace GBX.NET.Engines.Game;

[Node(0x03039000)]
public class CGameCtnDecorationAudio : CMwNod
{
    private IDictionary<string, CPlugSound?>? sounds;
    private IDictionary<string, CPlugSound?>? musics;
    private CPlugAudioEnvironment? audioEnvOutsideOpen;
    private CPlugAudioEnvironment? audioEnvOutsideEnclosed;

    protected CGameCtnDecorationAudio()
    {
        
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03039000))]
    public IDictionary<string, CPlugSound?>? Sounds { get => sounds; set => sounds = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03039000))]
    public IDictionary<string, CPlugSound?>? Musics { get => musics; set => musics = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03039001))]
    public CPlugAudioEnvironment? AudioEnvOutsideOpen { get => audioEnvOutsideOpen; set => audioEnvOutsideOpen = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk03039001))]
    public CPlugAudioEnvironment? AudioEnvOutsideEnclosed { get => audioEnvOutsideEnclosed; set => audioEnvOutsideEnclosed = value; }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnDecorationAudio 0x000 chunk
    /// </summary>
    [Chunk(0x03039000)]
    public class Chunk03039000 : Chunk<CGameCtnDecorationAudio>
    {
        public override void ReadWrite(CGameCtnDecorationAudio n, GameBoxReaderWriter rw)
        {
            rw.DictionaryNode(ref n.sounds, keyReaderWriter: (r => r.ReadId(), (x, w) => w.WriteId(x)));
            rw.DictionaryNode(ref n.musics, keyReaderWriter: (r => r.ReadId(), (x, w) => w.WriteId(x)));
        }

        public override async Task ReadWriteAsync(CGameCtnDecorationAudio n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            n.sounds = await rw.DictionaryNodeAsync(n.sounds, keyReaderWriter: (r => r.ReadId(), (x, w) => w.WriteId(x)), cancellationToken: cancellationToken);
            n.musics = await rw.DictionaryNodeAsync(n.musics, keyReaderWriter: (r => r.ReadId(), (x, w) => w.WriteId(x)), cancellationToken: cancellationToken);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnDecorationAudio 0x001 chunk
    /// </summary>
    [Chunk(0x03039001)]
    public class Chunk03039001 : Chunk<CGameCtnDecorationAudio>
    {
        public override void ReadWrite(CGameCtnDecorationAudio n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugAudioEnvironment>(ref n.audioEnvOutsideOpen);
            rw.NodeRef<CPlugAudioEnvironment>(ref n.audioEnvOutsideEnclosed);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnDecorationAudio 0x002 chunk
    /// </summary>
    [Chunk(0x03039002)]
    public class Chunk03039002 : Chunk<CGameCtnDecorationAudio>
    {
        public float U01;

        public override void ReadWrite(CGameCtnDecorationAudio n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01); // 1
        }
    }

    #endregion
}