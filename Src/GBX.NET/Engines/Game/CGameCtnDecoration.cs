namespace GBX.NET.Engines.Game;

[Node(0x03038000)]
[NodeExtension("TMDecoration")]
[NodeExtension("Decoration")]
public class CGameCtnDecoration : CGameCtnCollector
{
    private CGameCtnDecorationSize? decoSize;
    private CGameCtnDecorationAudio? decoAudio;
    private CGameCtnDecorationMood? decoMood;
    private CPlugDecoratorSolid? decoratorSolidWarp;
    private CGameCtnDecorationTerrainModifier? terrainModifierCovered;
    private CGameCtnDecorationTerrainModifier? terrainModifierBase;

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnDecorationSize? DecoSize { get => decoSize; set => decoSize = value; }
    
    [NodeMember(ExactlyNamed = true)]
    public CGameCtnDecorationAudio? DecoAudio { get => decoAudio; set => decoAudio = value; }
    
    [NodeMember(ExactlyNamed = true)]
    public CGameCtnDecorationMood? DecoMood { get => decoMood; set => decoMood = value; }
    
    [NodeMember(ExactlyNamed = true)]
    public CPlugDecoratorSolid? DecoratorSolidWarp { get => decoratorSolidWarp; set => decoratorSolidWarp = value; }

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnDecorationTerrainModifier? TerrainModifierCovered { get => terrainModifierCovered; set => terrainModifierCovered = value; }
    
    [NodeMember(ExactlyNamed = true)]
    public CGameCtnDecorationTerrainModifier? TerrainModifierBase { get => terrainModifierBase; set => terrainModifierBase = value; }

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

    #region 0x014 chunk

    /// <summary>
    /// CGameCtnDecoration 0x014 chunk
    /// </summary>
    [Chunk(0x03038014)]
    public class Chunk03038014 : Chunk<CGameCtnDecoration>
    {
        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugDecoratorSolid>(ref n.decoratorSolidWarp);
        }
    }

    #endregion

    #region 0x015 chunk

    /// <summary>
    /// CGameCtnDecoration 0x015 chunk
    /// </summary>
    [Chunk(0x03038015)]
    public class Chunk03038015 : Chunk<CGameCtnDecoration>
    {
        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationTerrainModifier>(ref n.terrainModifierCovered);
        }
    }

    #endregion

    #region 0x016 chunk

    /// <summary>
    /// CGameCtnDecoration 0x016 chunk
    /// </summary>
    [Chunk(0x03038016)]
    public class Chunk03038016 : Chunk<CGameCtnDecoration>
    {
        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnDecorationTerrainModifier>(ref n.terrainModifierBase);
        }
    }

    #endregion

    #endregion
}
