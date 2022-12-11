namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A003000</remarks>
[Node(0x0A003000)]
[Node(0x0A001000)]
public class CSceneLayout : CMwNod
{
    private CSceneConfig? sceneConfig;
    private ExternalNode<CMwNod>[]? motionManagerWeathers;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0A001003>]
    public CSceneConfig? SceneConfig { get => sceneConfig; set => sceneConfig = value; }
    
    [NodeMember]
    [AppliedWithChunk<Chunk0A001005>]
    public ExternalNode<CMwNod>[]? MotionManagerWeathers { get => motionManagerWeathers; set => motionManagerWeathers = value; }

    internal CSceneLayout()
    {
        
    }

    #region 0x003 chunk

    /// <summary>
    /// CScene 0x003 chunk
    /// </summary>
    [Chunk(0x0A001003)]
    public class Chunk0A001003 : Chunk<CSceneLayout>
    {
        public override void ReadWrite(CSceneLayout n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CSceneConfig>(ref n.sceneConfig);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CScene 0x004 chunk
    /// </summary>
    [Chunk(0x0A001004)]
    public class Chunk0A001004 : Chunk<CSceneLayout>
    {
        public uint[]? U01;

        public override void ReadWrite(CSceneLayout n, GameBoxReaderWriter rw)
        {
            rw.Array<uint>(ref U01);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CScene 0x005 chunk
    /// </summary>
    [Chunk(0x0A001005)]
    public class Chunk0A001005 : Chunk<CSceneLayout>
    {
        public override void ReadWrite(CSceneLayout n, GameBoxReaderWriter rw)
        {
            rw.Int32(10);
            rw.ArrayNode<CMwNod>(ref n.motionManagerWeathers);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CScene 0x006 chunk
    /// </summary>
    [Chunk(0x0A001006), IgnoreChunk]
    public class Chunk0A001006 : Chunk<CSceneLayout>
    {
        
    }

    #endregion
}
