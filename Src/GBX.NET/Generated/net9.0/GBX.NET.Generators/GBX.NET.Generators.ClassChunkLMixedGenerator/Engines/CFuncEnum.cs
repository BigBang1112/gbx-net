namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x0500E000</remarks>
[Class(0x0500E000)]
public partial class CFuncEnum : CFunc, IClass
{
    [Hexadecimal] public static new uint Id => 0x0500E000;




    private int wantedCount;
    [AppliedWithChunk<Chunk0500E005>]
    public int WantedCount { get => wantedCount; set => wantedCount = value; }

    private External<CPlug>[]? values;
    [AppliedWithChunk<Chunk0500E005>]
    public External<CPlug>[]? Values { get => values; set => values = value; }

    private string? name;
    [AppliedWithChunk<Chunk0500E005>]
    public string? Name { get => name; set => name = value; }

    private Int2 atlas;
    [AppliedWithChunk<Chunk0500E005>]
    public Int2 Atlas { get => atlas; set => atlas = value; }

    private int[]? valuesAtlasYx;
    [AppliedWithChunk<Chunk0500E005>]
    public int[]? ValuesAtlasYx { get => valuesAtlasYx; set => valuesAtlasYx = value; }

    private Vec2 minTexCoord;
    [AppliedWithChunk<Chunk0500E005>]
    public Vec2 MinTexCoord { get => minTexCoord; set => minTexCoord = value; }

    private Vec2 maxTexCoord;
    [AppliedWithChunk<Chunk0500E005>]
    public Vec2 MaxTexCoord { get => maxTexCoord; set => maxTexCoord = value; }

    private CMwRefBuffer? iconIndexs;
    [AppliedWithChunk<Chunk0500E006>]
    public CMwRefBuffer? IconIndexs { get => iconIndexsFile?.GetNode(ref iconIndexs) ?? iconIndexs; set => iconIndexs = value; }
    private Components.GbxRefTableFile? iconIndexsFile;
    public Components.GbxRefTableFile? IconIndexsFile { get => iconIndexsFile; set => iconIndexsFile = value; }
    public CMwRefBuffer? GetIconIndexs(GbxReadSettings settings = default, bool exceptions = false) => iconIndexsFile?.GetNode(ref iconIndexs, settings, exceptions) ?? iconIndexs;

    private Vec2 texSize;
    [AppliedWithChunk<Chunk0500E007>]
    public Vec2 TexSize { get => texSize; set => texSize = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncEnum"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncEnum() { }


    /// <summary>
    /// CFuncEnum 0x005 chunk
    /// </summary>
    [Chunk(0x0500E005)]
    public partial class Chunk0500E005 : Chunk<CFuncEnum>
    {
        /// <inheritdoc />
        public override uint Id => 0x0500E005;


        public override void ReadWrite(CFuncEnum n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.wantedCount);
            rw.ArrayNodeRef<CPlug>(ref n.values!);
            rw.Id(ref n.name);
            rw.Int2(ref n.atlas);
            rw.Array<int>(ref n.valuesAtlasYx!);
            rw.Vec2(ref n.minTexCoord);
            rw.Vec2(ref n.maxTexCoord);
        }
    }

    /// <summary>
    /// CFuncEnum 0x006 chunk
    /// </summary>
    [Chunk(0x0500E006)]
    public partial class Chunk0500E006 : Chunk<CFuncEnum>
    {
        /// <inheritdoc />
        public override uint Id => 0x0500E006;


        public override void ReadWrite(CFuncEnum n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwRefBuffer>(ref n.iconIndexs, ref n.iconIndexsFile);
        }
    }

    /// <summary>
    /// CFuncEnum 0x007 chunk
    /// </summary>
    [Chunk(0x0500E007)]
    public partial class Chunk0500E007 : Chunk<CFuncEnum>
    {
        /// <inheritdoc />
        public override uint Id => 0x0500E007;


        public override void ReadWrite(CFuncEnum n, GbxReaderWriter rw)
        {
            rw.Vec2(ref n.texSize);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0500E005 => new Chunk0500E005(),
        0x0500E006 => new Chunk0500E006(),
        0x0500E007 => new Chunk0500E007(),
        _ => base.NewChunk(chunkId),
    };
}
