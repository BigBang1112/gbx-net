namespace GBX.NET.Engines.Game;

/// <summary>
/// Group of maps (0x0308F000)
/// </summary>
[Node(0x0308F000)]
public class CGameCtnChallengeGroup : CMwNod
{
    #region Fields

    private string? _default;
    private MapInfo[]? mapInfos;

    #endregion

    #region Properties

    [NodeMember]
    public string? Default
    {
        get => _default;
        set => _default = value;
    }

    [NodeMember]
    public MapInfo[]? MapInfos
    {
        get => mapInfos;
        set => mapInfos = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnChallengeGroup()
    {

    }

    #endregion

    #region Chunks

    #region 0x002 chunk (default)

    /// <summary>
    /// CGameCtnChallengeGroup 0x002 chunk (default)
    /// </summary>
    [Chunk(0x0308F002, "default")]
    public class Chunk0308F002 : Chunk<CGameCtnChallengeGroup>
    {
        public override void ReadWrite(CGameCtnChallengeGroup n, GameBoxReaderWriter rw)
        {
            rw.String(ref n._default);
        }
    }

    #endregion

    #region 0x00B chunk (map infos)

    /// <summary>
    /// CGameCtnChallengeGroup 0x00B chunk (map infos)
    /// </summary>
    [Chunk(0x0308F00B, "map infos")]
    public class Chunk0308F00B : Chunk<CGameCtnChallengeGroup>, IVersionable
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnChallengeGroup n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.Array(ref n.mapInfos, (i, r) => new MapInfo()
            {
                Metadata = r.ReadIdent(),
                FilePath = r.ReadString()
            },
            (x, w) =>
            {
                w.Write(x.Metadata);
                w.Write(x.FilePath);
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public class MapInfo
    {
        public Ident Metadata { get; set; } = new Ident();
        public string FilePath { get; set; } = string.Empty;

        public override string ToString()
        {
            return Path.GetFileName(FilePath);
        }
    }

    #endregion
}
