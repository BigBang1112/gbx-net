namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09006000</remarks>
[Class(0x09006000)]
public partial class CPlugVisual : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x09006000;




    private Int3[]? subVisuals;
    /// <summary>
    /// SSubVisual?
    /// </summary>
    [AppliedWithChunk<Chunk09006005>]
    public Int3[]? SubVisuals { get => subVisuals; set => subVisuals = value; }

    private Split[]? splits;
    /// <summary>
    /// SSplit array
    /// </summary>
    [AppliedWithChunk<Chunk0900600B>]
    public Split[]? Splits { get => splits; set => splits = value; }

    private BitmapElemToPack[]? bitmapElemToPacks;
    [AppliedWithChunk<Chunk0900600E>]
    [AppliedWithChunk<Chunk0900600F>]
    public BitmapElemToPack[]? BitmapElemToPacks { get => bitmapElemToPacks; set => bitmapElemToPacks = value; }

    private int morphCount;
    [AppliedWithChunk<Chunk09006010>]
    public int MorphCount { get => morphCount; set => morphCount = value; }


    /// <summary>
    /// CPlugVisual 0x001 chunk
    /// </summary>
    [Chunk(0x09006001)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk09006001 : Chunk<CPlugVisual>
    {
        /// <inheritdoc />
        public override uint Id => 0x09006001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;

        public string? U01;

        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    /// <summary>
    /// CPlugVisual 0x004 chunk
    /// </summary>
    [Chunk(0x09006004)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk09006004 : Chunk<CPlugVisual>
    {
        /// <inheritdoc />
        public override uint Id => 0x09006004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;

        public CMwNod? U01;

        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
        }
    }

    /// <summary>
    /// CPlugVisual 0x005 chunk
    /// </summary>
    [Chunk(0x09006005)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk09006005 : Chunk<CPlugVisual>
    {
        /// <inheritdoc />
        public override uint Id => 0x09006005;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            rw.Array<Int3>(ref n.subVisuals!); // SSubVisual?
        }
    }

    /// <summary>
    /// CPlugVisual 0x006 chunk
    /// </summary>
    [Chunk(0x09006006)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk09006006 : Chunk<CPlugVisual>
    {
        /// <inheritdoc />
        public override uint Id => 0x09006006;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC;

    }

    /// <summary>
    /// CPlugVisual 0x007 chunk
    /// </summary>
    [Chunk(0x09006007)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX)]
    public partial class Chunk09006007 : Chunk<CPlugVisual>
    {
        /// <inheritdoc />
        public override uint Id => 0x09006007;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX;

        public bool U01;

        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CPlugVisual 0x008 chunk
    /// </summary>
    [Chunk(0x09006008)]
    [ChunkGameVersion(GameVersion.TM10)]
    public partial class Chunk09006008 : Chunk<CPlugVisual>
    {
        /// <inheritdoc />
        public override uint Id => 0x09006008;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10;

    }

    /// <summary>
    /// CPlugVisual 0x009 chunk
    /// </summary>
    [Chunk(0x09006009)]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk09006009 : Chunk<CPlugVisual>
    {
        /// <inheritdoc />
        public override uint Id => 0x09006009;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;

        public float U01;

        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVisual 0x00A chunk
    /// </summary>
    [Chunk(0x0900600A)]
    [ChunkGameVersion(GameVersion.TMSX)]
    public partial class Chunk0900600A : Chunk<CPlugVisual>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900600A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX;

    }

    /// <summary>
    /// CPlugVisual 0x00B chunk
    /// </summary>
    [Chunk(0x0900600B)]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0900600B : Chunk<CPlugVisual>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900600B;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<Split>(ref n.splits!); // SSplit array
        }
    }

    /// <summary>
    /// CPlugVisual 0x00C chunk
    /// </summary>
    [Chunk(0x0900600C)]
    [ChunkGameVersion(GameVersion.TMNESWC)]
    public partial class Chunk0900600C : Chunk<CPlugVisual>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900600C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMNESWC;

    }

    /// <summary>
    /// CPlugVisual 0x00D chunk
    /// </summary>
    [Chunk(0x0900600D)]
    public partial class Chunk0900600D : Chunk<CPlugVisual>
    {
        /// <inheritdoc />
        public override uint Id => 0x0900600D;

    }

    /// <summary>
    /// CPlugVisual 0x00E chunk
    /// </summary>
    [Chunk(0x0900600E)]
    [ChunkGameVersion(GameVersion.TMF)]
    public partial class Chunk0900600E : Chunk0900600D
    {
        /// <inheritdoc />
        public override uint Id => 0x0900600E;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF;


        public override void Read(CPlugVisual n, GbxReader r)
        {
            base.Read(n, r);
            n.BitmapElemToPacks = r.ReadArrayReadable<BitmapElemToPack>();
        }

        public override void Write(CPlugVisual n, GbxWriter w)
        {
            base.Write(n, w);
            w.WriteArrayWritable<BitmapElemToPack>(n.BitmapElemToPacks);
        }
    }

    /// <summary>
    /// CPlugVisual 0x00F chunk
    /// </summary>
    [Chunk(0x0900600F)]
    [ChunkGameVersion(GameVersion.TMT | GameVersion.MP4, 4, 5)]
    public partial class Chunk0900600F : Chunk0900600E
    {
        /// <inheritdoc />
        public override uint Id => 0x0900600F;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMT | GameVersion.MP4;

    }

    /// <summary>
    /// CPlugVisual 0x010 chunk
    /// </summary>
    [Chunk(0x09006010)]
    [ChunkGameVersion(GameVersion.TMT | GameVersion.MP4, 0, 0)]
    public partial class Chunk09006010 : Chunk<CPlugVisual>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09006010;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMT | GameVersion.MP4;

        public int Version { get; set; }


        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.morphCount);
            if (n.MorphCount>0)
            {
                throw new ("");
            }
        }
    }


    public sealed partial class BitmapElemToPack : IReadable, IWritable
    {
        public int U01 { get; set; }
        public int U02 { get; set; }
        public int U03 { get; set; }
        public int U04 { get; set; }
        public int U05 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadInt32();
            U02 = r.ReadInt32();
            U03 = r.ReadInt32();
            U04 = r.ReadInt32();
            U05 = r.ReadInt32();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(U01);
            w.Write(U02);
            w.Write(U03);
            w.Write(U04);
            w.Write(U05);
        }
    }

    public sealed partial class Split : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private BoxAligned boundingBox;
        public BoxAligned BoundingBox { get => boundingBox; set => boundingBox = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.BoxAligned(ref boundingBox);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09006001 => new Chunk09006001(),
        0x09006004 => new Chunk09006004(),
        0x09006005 => new Chunk09006005(),
        0x09006006 => new Chunk09006006(),
        0x09006007 => new Chunk09006007(),
        0x09006008 => new Chunk09006008(),
        0x09006009 => new Chunk09006009(),
        0x0900600A => new Chunk0900600A(),
        0x0900600B => new Chunk0900600B(),
        0x0900600C => new Chunk0900600C(),
        0x0900600D => new Chunk0900600D(),
        0x0900600E => new Chunk0900600E(),
        0x0900600F => new Chunk0900600F(),
        0x09006010 => new Chunk09006010(),
        _ => base.NewChunk(chunkId),
    };
}
