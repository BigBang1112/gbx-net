namespace GBX.NET.Engines.Game;

/// <summary>
/// Group of maps.
/// </summary>
/// <remarks>ID: 0x0308F000</remarks>
[Node(0x0308F000)]
public class CGameCtnChallengeGroup : CMwNod
{
    #region Fields

    private string? name;
    private MapInfo[]? mapInfos;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0308F002>]
    public string? Name
    {
        get => name;
        set => name = value;
    }

    [NodeMember]
    [AppliedWithChunk<Chunk0308F006>]
    [AppliedWithChunk<Chunk0308F00B>]
    public MapInfo[]? MapInfos
    {
        get => mapInfos;
        set => mapInfos = value;
    }

    #endregion

    #region Constructors

    internal CGameCtnChallengeGroup()
    {

    }

    #endregion

    #region Methods

    public override string ToString()
    {
        return name ?? base.ToString();
    }

    #endregion

    #region Chunks

    #region 0x002 chunk (name)

    /// <summary>
    /// CGameCtnChallengeGroup 0x002 chunk (name)
    /// </summary>
    [Chunk(0x0308F002, "name")]
    public class Chunk0308F002 : Chunk<CGameCtnChallengeGroup>
    {
        public override void ReadWrite(CGameCtnChallengeGroup n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.name);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnChallengeGroup 0x003 chunk
    /// </summary>
    [Chunk(0x0308F003)]
    public class Chunk0308F003 : Chunk<CGameCtnChallengeGroup>
    {
        public int U01;

        public override void ReadWrite(CGameCtnChallengeGroup n, GameBoxReaderWriter rw)
        {
            for (var i = 0; i < 3; i++)
            {
                rw.Int32(ref U01);
            }
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CGameCtnChallengeGroup 0x004 chunk
    /// </summary>
    [Chunk(0x0308F004)]
    public class Chunk0308F004 : Chunk<CGameCtnChallengeGroup>
    {
        public int U01;

        public override void ReadWrite(CGameCtnChallengeGroup n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // CSystemFidFile array

            if (U01 > 0)
            {
                throw new Exception("U01 > 0");
            }
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameCtnChallengeGroup 0x005 chunk
    /// </summary>
    [Chunk(0x0308F005)]
    public class Chunk0308F005 : Chunk<CGameCtnChallengeGroup>
    {
        public int U01;

        public override void ReadWrite(CGameCtnChallengeGroup n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x006 chunk (old map infos)

    /// <summary>
    /// CGameCtnChallengeGroup 0x006 chunk (old map infos)
    /// </summary>
    [Chunk(0x0308F006, "old map infos")]
    public class Chunk0308F006 : Chunk<CGameCtnChallengeGroup>
    {
        public bool U01;

        public override void ReadWrite(CGameCtnChallengeGroup n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);

            if (!U01)
            {
                rw.Array(ref n.mapInfos, (i, r) => new MapInfo(Metadata: r.ReadIdent()),
                    (x, w) => { w.Write(x.Metadata); });
            }
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CGameCtnChallengeGroup 0x007 chunk
    /// </summary>
    [Chunk(0x0308F007)]
    public class Chunk0308F007 : Chunk<CGameCtnChallengeGroup>
    {
        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;

        public override void ReadWrite(CGameCtnChallengeGroup n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);

            // These are no longer in the game code xd
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
        }
    }

    #endregion

    #region 0x009 chunk

    /// <summary>
    /// CGameCtnChallengeGroup 0x009 chunk
    /// </summary>
    [Chunk(0x0308F009)]
    public class Chunk0308F009 : Chunk<CGameCtnChallengeGroup>
    {
        public string? U01;

        public override void ReadWrite(CGameCtnChallengeGroup n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
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

            rw.Array(ref n.mapInfos, r =>
            {
                var metadata = r.ReadIdent();
                var filePath = r.ReadString();
                var u01 = default(bool?);

                if (version == 0)
                {
                    u01 = r.ReadBoolean();
                }

                return new MapInfo(metadata, filePath, u01);
            },
            (x, w) =>
            {
                w.Write(x.Metadata);
                w.Write(x.FilePath);

                if (version == 0)
                {
                    w.Write(x.U01.GetValueOrDefault());
                }
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    /// <summary>
    /// SChallenge
    /// </summary>
    /// <param name="Metadata">Metadata.</param>
    /// <param name="FilePath">File path.</param>
    /// <param name="U01"></param>
    public record MapInfo(Ident Metadata, string? FilePath = null, bool? U01 = null)
    {
        public override string ToString()
        {
            if (FilePath is null)
            {
                return Metadata.ToString();
            }

            return $"{FilePath} {Metadata}";
        }
    }

    #endregion
}
