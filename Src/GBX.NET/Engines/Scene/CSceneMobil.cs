namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A011000</remarks>
[Node(0x0A011000)]
[NodeExtension("Mobil")]
public class CSceneMobil : CSceneObject
{
    private CHmsItem? item;
    private CSceneObjectLink?[]? objectLink;
    private CSceneMessageHandler? messageHandler;

    [NodeMember(ExactlyNamed = true)]
    public CHmsItem? Item { get => item; set => item = value; }
    
    [NodeMember(ExactlyNamed = true)]
    public CSceneObjectLink?[]? ObjectLink { get => objectLink; set => objectLink = value; }
    
    [NodeMember(ExactlyNamed = true)]
    public CSceneMessageHandler? MessageHandler { get => messageHandler; set => messageHandler = value; }

    protected CSceneMobil()
    {
        
    }

    /// <summary>
    /// CSceneMobil 0x003 chunk
    /// </summary>
    [Chunk(0x0A011003)]
    public class Chunk0A011003 : Chunk<CSceneMobil>
    {
        public override void ReadWrite(CSceneMobil n, GameBoxReaderWriter rw)
        {
            rw.ArrayNode<CSceneObjectLink>(ref n.objectLink);
        }
    }

    /// <summary>
    /// CSceneMobil 0x004 chunk
    /// </summary>
    [Chunk(0x0A011004)]
    public class Chunk0A011004 : Chunk<CSceneMobil>
    {
        public CMwNod? U01;

        public override void ReadWrite(CSceneMobil n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01); // MotionSolid?
        }
    }

    /// <summary>
    /// CSceneMobil 0x005 chunk
    /// </summary>
    [Chunk(0x0A011005)]
    public class Chunk0A011005 : Chunk<CSceneMobil>
    {
        public override void Read(CSceneMobil n, GameBoxReader r)
        {
            n.item = Parse<CHmsItem>(r, 0x06003000, progress: null, ignoreZeroIdChunk: true); // direct node
        }

        public override void Write(CSceneMobil n, GameBoxWriter w)
        {
            if (n.item is null)
            {
                w.Write(0);
            }
            else
            {
                n.item.Write(w);
            }
        }
    }

    /// <summary>
    /// CSceneMobil 0x006 chunk
    /// </summary>
    [Chunk(0x0A011006)]
    public class Chunk0A011006 : Chunk<CSceneMobil>
    {
        public override void ReadWrite(CSceneMobil n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CSceneMessageHandler>(ref n.messageHandler);
        }
    }
}
