namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0904F000</remarks>
[Class(0x0904F000)]
public partial class CPlugTree : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x0904F000;




    private List<CPlugTree> children = new List<CPlugTree>();
    [AppliedWithChunk<Chunk0904F006>]
    public List<CPlugTree> Children { get => children; set => children = value; }

    private CFuncTree? funcTree;
    [AppliedWithChunk<Chunk0904F011>]
    public CFuncTree? FuncTree { get => funcTreeFile?.GetNode(ref funcTree) ?? funcTree; set => funcTree = value; }
    private Components.GbxRefTableFile? funcTreeFile;
    public Components.GbxRefTableFile? FuncTreeFile { get => funcTreeFile; set => funcTreeFile = value; }
    public CFuncTree? GetFuncTree(GbxReadSettings settings = default, bool exceptions = false) => funcTreeFile?.GetNode(ref funcTree, settings, exceptions) ?? funcTree;

    private CPlugVisual? visual;
    [AppliedWithChunk<Chunk0904F016>]
    public CPlugVisual? Visual { get => visual; set => visual = value; }

    private CPlug? shader;
    [AppliedWithChunk<Chunk0904F016>]
    public CPlug? Shader { get => shaderFile?.GetNode(ref shader) ?? shader; set => shader = value; }
    private Components.GbxRefTableFile? shaderFile;
    public Components.GbxRefTableFile? ShaderFile { get => shaderFile; set => shaderFile = value; }
    public CPlug? GetShader(GbxReadSettings settings = default, bool exceptions = false) => shaderFile?.GetNode(ref shader, settings, exceptions) ?? shader;

    private CPlug? surface;
    [AppliedWithChunk<Chunk0904F016>]
    public CPlug? Surface { get => surface; set => surface = value; }

    private CPlugTreeGenerator? generator;
    [AppliedWithChunk<Chunk0904F016>]
    public CPlugTreeGenerator? Generator { get => generator; set => generator = value; }


    /// <summary>
    /// CPlugTree 0x006 chunk
    /// </summary>
    [Chunk(0x0904F006)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0904F006 : Chunk<CPlugTree>
    {
        /// <inheritdoc />
        public override uint Id => 0x0904F006;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CPlugTree n, GbxReaderWriter rw)
        {
            rw.ListNodeRef_deprec<CPlugTree>(ref n.children!);
        }
    }

    /// <summary>
    /// CPlugTree 0x00C chunk
    /// </summary>
    [Chunk(0x0904F00C)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX)]
    public partial class Chunk0904F00C : Chunk<CPlugTree>
    {
        /// <inheritdoc />
        public override uint Id => 0x0904F00C;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX;

        /// <summary>
        /// could be array of Ids
        /// </summary>
        public int U01;

        public override void ReadWrite(CPlugTree n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01); // could be array of Ids
            if (U01>0)
            {
                throw new ("");
            }
        }
    }

    /// <summary>
    /// CPlugTree 0x00D chunk
    /// </summary>
    [Chunk(0x0904F00D)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0904F00D : Chunk<CPlugTree>
    {
        /// <inheritdoc />
        public override uint Id => 0x0904F00D;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;

        public string? U01;
    }

    /// <summary>
    /// CPlugTree 0x011 chunk (FuncTree)
    /// </summary>
    [Chunk(0x0904F011, "FuncTree")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0904F011 : Chunk<CPlugTree>
    {
        /// <inheritdoc />
        public override uint Id => 0x0904F011;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CPlugTree n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncTree>(ref n.funcTree, ref n.funcTreeFile);
        }
    }

    /// <summary>
    /// CPlugTree 0x015 chunk (flags+location, TM1.0)
    /// </summary>
    [Chunk(0x0904F015, "flags+location, TM1.0")]
    [ChunkGameVersion(GameVersion.TM10)]
    public partial class Chunk0904F015 : Chunk<CPlugTree>
    {
        /// <inheritdoc />
        public override uint Id => 0x0904F015;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10;

    }

    /// <summary>
    /// CPlugTree 0x016 chunk (properties)
    /// </summary>
    [Chunk(0x0904F016, "properties")]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0904F016 : Chunk<CPlugTree>
    {
        /// <inheritdoc />
        public override uint Id => 0x0904F016;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CPlugTree n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugVisual>(ref n.visual);
            rw.NodeRef<CPlug>(ref n.shader, ref n.shaderFile);
            rw.NodeRef<CPlug>(ref n.surface);
            rw.NodeRef<CPlugTreeGenerator>(ref n.generator);
        }
    }

    /// <summary>
    /// CPlugTree 0x017 chunk
    /// </summary>
    [Chunk(0x0904F017)]
    public partial class Chunk0904F017 : Chunk<CPlugTree>
    {
        /// <inheritdoc />
        public override uint Id => 0x0904F017;

        public CPlugVisual? U01;

        public override void ReadWrite(CPlugTree n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugVisual>(ref U01);
        }
    }

    /// <summary>
    /// CPlugTree 0x018 chunk (flags+location, unused)
    /// </summary>
    [Chunk(0x0904F018, "flags+location, unused")]
    public partial class Chunk0904F018 : Chunk<CPlugTree>
    {
        /// <inheritdoc />
        public override uint Id => 0x0904F018;

    }

    /// <summary>
    /// CPlugTree 0x019 chunk (flags+location, TMSX)
    /// </summary>
    [Chunk(0x0904F019, "flags+location, TMSX")]
    [ChunkGameVersion(GameVersion.TMSX)]
    public partial class Chunk0904F019 : Chunk<CPlugTree>
    {
        /// <inheritdoc />
        public override uint Id => 0x0904F019;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX;

    }

    /// <summary>
    /// CPlugTree 0x01A chunk (flags+location, latest)
    /// </summary>
    [Chunk(0x0904F01A, "flags+location, latest")]
    [ChunkGameVersion(GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0904F01A : Chunk<CPlugTree>
    {
        /// <inheritdoc />
        public override uint Id => 0x0904F01A;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0904F006 => new Chunk0904F006(),
        0x0904F00C => new Chunk0904F00C(),
        0x0904F00D => new Chunk0904F00D(),
        0x0904F011 => new Chunk0904F011(),
        0x0904F015 => new Chunk0904F015(),
        0x0904F016 => new Chunk0904F016(),
        0x0904F017 => new Chunk0904F017(),
        0x0904F018 => new Chunk0904F018(),
        0x0904F019 => new Chunk0904F019(),
        0x0904F01A => new Chunk0904F01A(),
        _ => base.NewChunk(chunkId),
    };
}
