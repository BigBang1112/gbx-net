namespace GBX.NET.Engines.GameData;

/// <summary>
/// Waypoint.
/// </summary>
/// <remarks>ID: 0x2E009000</remarks>
[Node(0x2E009000)]
public class CGameWaypointSpecialProperty : CMwNod
{
    #region Fields

    private int? spawn;
    private string? tag;
    private int order;

    #endregion

    #region Properties

    [NodeMember]
    public int? Spawn
    {
        get => spawn;
        set => spawn = value;
    }

    [NodeMember]
    public string? Tag
    {
        get => tag;
        set => tag = value;
    }

    [NodeMember]
    public int Order
    {
        get => order;
        set => order = value;
    }

    #endregion

    #region Constructors

    protected CGameWaypointSpecialProperty()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameWaypointSpecialProperty 0x000 chunk
    /// </summary>
    [Chunk(0x2E009000)]
    public class Chunk2E009000 : Chunk<CGameWaypointSpecialProperty>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameWaypointSpecialProperty n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version == 1)
            {
                rw.Int32(ref n.spawn);
                rw.Int32(ref n.order);
            }
            else if (version == 2)
            {
                rw.String(ref n.tag);
                rw.Int32(ref n.order);
            }
        }
    }

    #endregion

    #region 0x001 skippable chunk

    /// <summary>
    /// CGameWaypointSpecialProperty 0x001 skippable chunk
    /// </summary>
    [Chunk(0x2E009001)]
    public class Chunk2E009001 : SkippableChunk<CGameWaypointSpecialProperty>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGameWaypointSpecialProperty n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    #endregion

    #endregion
}
