namespace GBX.NET.Engines.Game;

[Node(0x03038000)]
[NodeExtension("TMDecoration")]
[NodeExtension("Decoration")]
public class CGameCtnDecoration : CGameCtnCollector
{
    private CGameCtnDecorationSize? decoSize;
    private CGameCtnDecorationAudio? decoAudio;
    private CGameCtnDecorationMood? decoMood;

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnDecorationSize? DecoSize { get => decoSize; set => decoSize = value; }
    
    [NodeMember(ExactlyNamed = true)]
    internal CGameCtnDecorationAudio? DecoAudio { get => decoAudio; set => decoAudio = value; }
    
    [NodeMember(ExactlyNamed = true)]
    public CGameCtnDecorationMood? DecoMood { get => decoMood; set => decoMood = value; }

    #region Constructors

    protected CGameCtnDecoration()
    {

    }

    #endregion

    #region Chunks

    #region 0x011 chunk

    [Chunk(0x03038011)]
    public class Chunk03038011 : Chunk<CGameCtnDecoration>
    {
        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationSize>(ref n.decoSize);
        }
    }

    #endregion

    #region 0x012 chunk

    /// <summary>
    /// CGameCtnDecoration 0x012 chunk
    /// </summary>
    [Chunk(0x03038012)]
    public class Chunk03038012 : Chunk<CGameCtnDecoration>
    {
        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationAudio>(ref n.decoAudio);
        }
    }

    #endregion

    #region 0x013 chunk

    [Chunk(0x03038013)]
    public class Chunk03038013 : Chunk<CGameCtnDecoration>
    {
        public int U01;

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationMood>(ref n.decoMood);
        }
    }

    #endregion

    #endregion
}
