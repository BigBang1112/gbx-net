namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A01F000</remarks>
[Node(0x0A01F000)]
public class CSceneMessageHandler : CMwNod
{
    private CMwNod? script;
    private GameBoxRefTable.File? scriptFile;

    [NodeMember]
    [AppliedWithChunk<Chunk0A01F001>]
    public CMwNod? Script { get => script; set => script = value; }
    
    public GameBoxRefTable.File? ScriptFile => scriptFile;

    internal CSceneMessageHandler()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CSceneMessageHandler 0x000 chunk
    /// </summary>
    [Chunk(0x0A01F000)]
    public class Chunk0A01F000 : Chunk<CSceneMessageHandler>
    {
        public Node? U01;

        public override void ReadWrite(CSceneMessageHandler n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CSceneMessageHandler 0x001 chunk
    /// </summary>
    [Chunk(0x0A01F001)]
    public class Chunk0A01F001 : Chunk<CSceneMessageHandler>
    {
        public override void ReadWrite(CSceneMessageHandler n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.script, ref n.scriptFile); // OnContactScript/OnClickScript
        }
    }

    #endregion
}
