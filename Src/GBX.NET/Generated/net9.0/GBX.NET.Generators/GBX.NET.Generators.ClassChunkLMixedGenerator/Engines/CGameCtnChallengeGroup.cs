namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0308F000</remarks>
[Class(0x0308F000)]
public partial class CGameCtnChallengeGroup : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0308F000;




    private string? name;
    [AppliedWithChunk<Chunk0308F002>]
    public string? Name { get => name; set => name = value; }

    private List<MapInfo>? mapInfos;
    [AppliedWithChunk<Chunk0308F006>]
    [AppliedWithChunk<Chunk0308F00A>]
    [AppliedWithChunk<Chunk0308F00B>]
    public List<MapInfo>? MapInfos { get => mapInfos; set => mapInfos = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnChallengeGroup"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnChallengeGroup() { }


    /// <summary>
    /// CGameCtnChallengeGroup 0x002 chunk (name)
    /// </summary>
    [Chunk(0x0308F002, "name")]
    public partial class Chunk0308F002 : Chunk<CGameCtnChallengeGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308F002;


        public override void ReadWrite(CGameCtnChallengeGroup n, GbxReaderWriter rw)
        {
            rw.String(ref n.name);
        }
    }

    /// <summary>
    /// CGameCtnChallengeGroup 0x003 chunk
    /// </summary>
    [Chunk(0x0308F003)]
    public partial class Chunk0308F003 : Chunk<CGameCtnChallengeGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308F003;

        public int U01;

        public override void ReadWrite(CGameCtnChallengeGroup n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U01);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnChallengeGroup 0x004 chunk
    /// </summary>
    [Chunk(0x0308F004)]
    public partial class Chunk0308F004 : Chunk<CGameCtnChallengeGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308F004;

        public int U01;

        public override void ReadWrite(CGameCtnChallengeGroup n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            if (U01>0)
            {
                throw new ("");
            }
        }
    }

    /// <summary>
    /// CGameCtnChallengeGroup 0x005 chunk
    /// </summary>
    [Chunk(0x0308F005)]
    public partial class Chunk0308F005 : Chunk<CGameCtnChallengeGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308F005;

        public int U01;

        public override void ReadWrite(CGameCtnChallengeGroup n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnChallengeGroup 0x006 chunk
    /// </summary>
    [Chunk(0x0308F006)]
    public partial class Chunk0308F006 : Chunk<CGameCtnChallengeGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308F006;

        public bool U01;

        public override void ReadWrite(CGameCtnChallengeGroup n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.ListReadableWritable<MapInfo>(ref n.mapInfos!);
        }
    }

    /// <summary>
    /// CGameCtnChallengeGroup 0x007 chunk
    /// </summary>
    [Chunk(0x0308F007)]
    public partial class Chunk0308F007 : Chunk<CGameCtnChallengeGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308F007;

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;

        public override void ReadWrite(CGameCtnChallengeGroup n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
        }
    }

    /// <summary>
    /// CGameCtnChallengeGroup 0x009 chunk
    /// </summary>
    [Chunk(0x0308F009)]
    public partial class Chunk0308F009 : Chunk<CGameCtnChallengeGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308F009;

        public string? U01;

        public override void ReadWrite(CGameCtnChallengeGroup n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnChallengeGroup 0x00A chunk
    /// </summary>
    [Chunk(0x0308F00A)]
    public partial class Chunk0308F00A : Chunk<CGameCtnChallengeGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308F00A;


        public override void ReadWrite(CGameCtnChallengeGroup n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<MapInfo>(ref n.mapInfos!, version: 1);
        }
    }

    /// <summary>
    /// CGameCtnChallengeGroup 0x00B chunk
    /// </summary>
    [Chunk(0x0308F00B)]
    public partial class Chunk0308F00B : Chunk<CGameCtnChallengeGroup>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0308F00B;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnChallengeGroup n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<MapInfo>(ref n.mapInfos!, version: Version + 2);
        }
    }


    public sealed partial class MapInfo : IReadableWritable
    {

        private Ident? metadata;
        public Ident? Metadata { get => metadata; set => metadata = value; }

        private string? filePath;
        public string? FilePath { get => filePath; set => filePath = value; }

        private bool u01;
        public bool U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Ident(ref metadata);
            if (v >= 2)
            {
                rw.String(ref filePath);
            }
            if (v >= 1)
            {
                if (v <= 2)
                {
                    rw.Boolean(ref u01);
                }
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0308F002 => new Chunk0308F002(),
        0x0308F003 => new Chunk0308F003(),
        0x0308F004 => new Chunk0308F004(),
        0x0308F005 => new Chunk0308F005(),
        0x0308F006 => new Chunk0308F006(),
        0x0308F007 => new Chunk0308F007(),
        0x0308F009 => new Chunk0308F009(),
        0x0308F00A => new Chunk0308F00A(),
        0x0308F00B => new Chunk0308F00B(),
        _ => base.NewChunk(chunkId),
    };
}
