namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0305C000</remarks>
[Class(0x0305C000)]
public partial class CGameCtnZone : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0305C000;




    private int height;
    [AppliedWithChunk<Chunk0305C003>]
    [AppliedWithChunk<Chunk0305C008>]
    public int Height { get => height; set => height = value; }

    private string? zoneId;
    [AppliedWithChunk<Chunk0305C003>]
    public string? ZoneId { get => zoneId; set => zoneId = value; }

    private string? surfaceId;
    [AppliedWithChunk<Chunk0305C003>]
    public string? SurfaceId { get => surfaceId; set => surfaceId = value; }

    private int depth;
    [AppliedWithChunk<Chunk0305C004>]
    public int Depth { get => depth; set => depth = value; }

    private bool oldZone;
    [AppliedWithChunk<Chunk0305C004>]
    public bool OldZone { get => oldZone; set => oldZone = value; }

    private bool hasWater;
    [AppliedWithChunk<Chunk0305C005>]
    public bool HasWater { get => hasWater; set => hasWater = value; }

    private bool isLargeZone;
    [AppliedWithChunk<Chunk0305C006>]
    public bool IsLargeZone { get => isLargeZone; set => isLargeZone = value; }

    private float visualTopGroundHeight;
    [AppliedWithChunk<Chunk0305C007>]
    public float VisualTopGroundHeight { get => visualTopGroundHeight; set => visualTopGroundHeight = value; }

    private string? waterId;
    [AppliedWithChunk<Chunk0305C00B>]
    public string? WaterId { get => waterId; set => waterId = value; }

    private string? forcedParentZoneFrontierId;
    [AppliedWithChunk<Chunk0305C00C>]
    public string? ForcedParentZoneFrontierId { get => forcedParentZoneFrontierId; set => forcedParentZoneFrontierId = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnZone"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnZone() { }


    /// <summary>
    /// CGameCtnZone 0x003 chunk
    /// </summary>
    [Chunk(0x0305C003)]
    public partial class Chunk0305C003 : Chunk<CGameCtnZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305C003;

        public int U01;

        public override void ReadWrite(CGameCtnZone n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.height);
            rw.Id(ref n.zoneId);
            rw.Id(ref n.surfaceId);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnZone 0x004 skippable chunk
    /// </summary>
    [Chunk(0x0305C004)]
    public partial class Chunk0305C004 : SkippableChunk<CGameCtnZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305C004;


        public override void ReadWrite(CGameCtnZone n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.depth);
            rw.Boolean(ref n.oldZone);
        }
    }

    /// <summary>
    /// CGameCtnZone 0x005 chunk
    /// </summary>
    [Chunk(0x0305C005)]
    public partial class Chunk0305C005 : Chunk<CGameCtnZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305C005;


        public override void ReadWrite(CGameCtnZone n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.hasWater);
        }
    }

    /// <summary>
    /// CGameCtnZone 0x006 chunk
    /// </summary>
    [Chunk(0x0305C006)]
    public partial class Chunk0305C006 : Chunk<CGameCtnZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305C006;


        public override void ReadWrite(CGameCtnZone n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isLargeZone);
        }
    }

    /// <summary>
    /// CGameCtnZone 0x007 chunk
    /// </summary>
    [Chunk(0x0305C007)]
    public partial class Chunk0305C007 : Chunk<CGameCtnZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305C007;


        public override void ReadWrite(CGameCtnZone n, GbxReaderWriter rw)
        {
            rw.Single(ref n.visualTopGroundHeight);
        }
    }

    /// <summary>
    /// CGameCtnZone 0x008 chunk
    /// </summary>
    [Chunk(0x0305C008)]
    public partial class Chunk0305C008 : Chunk<CGameCtnZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305C008;


        public override void ReadWrite(CGameCtnZone n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.height);
        }
    }

    /// <summary>
    /// CGameCtnZone 0x00B chunk
    /// </summary>
    [Chunk(0x0305C00B)]
    public partial class Chunk0305C00B : Chunk<CGameCtnZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305C00B;


        public override void ReadWrite(CGameCtnZone n, GbxReaderWriter rw)
        {
            rw.Id(ref n.waterId);
        }
    }

    /// <summary>
    /// CGameCtnZone 0x00C chunk
    /// </summary>
    [Chunk(0x0305C00C)]
    public partial class Chunk0305C00C : Chunk<CGameCtnZone>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305C00C;


        public override void ReadWrite(CGameCtnZone n, GbxReaderWriter rw)
        {
            rw.Id(ref n.forcedParentZoneFrontierId);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0305C003 => new Chunk0305C003(),
        0x0305C004 => new Chunk0305C004(),
        0x0305C005 => new Chunk0305C005(),
        0x0305C006 => new Chunk0305C006(),
        0x0305C007 => new Chunk0305C007(),
        0x0305C008 => new Chunk0305C008(),
        0x0305C00B => new Chunk0305C00B(),
        0x0305C00C => new Chunk0305C00C(),
        _ => base.NewChunk(chunkId),
    };
}
