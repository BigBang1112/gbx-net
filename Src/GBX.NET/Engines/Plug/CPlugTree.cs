using GBX.NET.Engines.Function;

namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0904F000</remarks>
[Node(0x0904F000)]
public class CPlugTree : CPlug
{
    private IList<CPlugTree?> children;
    private string? name;
    private CFuncTree? funcTree;
    private CPlugVisual? visual;
    private CPlugSurface? surface;
    private CPlug? shader;
    private GameBoxRefTable.File? shaderFile;
    private CPlugTreeGenerator? generator;
    private Iso4? translation;

    [NodeMember(ExactName = "Childs")]
    [AppliedWithChunk(typeof(Chunk0904F006))]
    public IList<CPlugTree?> Children { get => children; set => children = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0904F00D))]
    public string? Name { get => name; set => name = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0904F016))]
    public CPlugVisual? Visual { get => visual; set => visual = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0904F016))]
    public CPlugSurface? Surface { get => surface; set => surface = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0904F016))]
    public CPlug? Shader
    {
        get => shader = GetNodeFromRefTable(shader, shaderFile) as CPlug;
        set => shader = value;
    }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0904F016))]
    public CPlugTreeGenerator? Generator { get => generator; set => generator = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0904F015))]
    [AppliedWithChunk(typeof(Chunk0904F018))]
    [AppliedWithChunk(typeof(Chunk0904F019))]
    [AppliedWithChunk(typeof(Chunk0904F01A))]
    public Iso4? Translation { get => translation; set => translation = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0904F011))]
    public CFuncTree? FuncTree { get => funcTree; set => funcTree = value; }

    internal CPlugTree()
    {
        children = Array.Empty<CPlugTree>();
    }

    public override string ToString() => $"{base.ToString()} {{ \"{name}\" }}";

    #region Chunks

    #region 0x006 chunk

    /// <summary>
    /// CPlugTree 0x006 chunk
    /// </summary>
    [Chunk(0x0904F006)]
    public class Chunk0904F006 : Chunk<CPlugTree>
    {
        private int listVersion = 10;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref listVersion);
            rw.ListNode<CPlugTree>(ref n.children!);
        }

        public override async Task ReadWriteAsync(CPlugTree n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            rw.Int32(ref listVersion);
            n.children = (await rw.ListNodeAsync<CPlugTree>(n.children!, cancellationToken))!;
        }
    }

    #endregion

    #region 0x00C chunk

    /// <summary>
    /// CPlugTree 0x00C chunk
    /// </summary>
    [Chunk(0x0904F00C)]
    public class Chunk0904F00C : Chunk<CPlugTree>
    {
        public int U01;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            // could be array of Ids

            if (U01 > 0)
            {
                throw new NotSupportedException("U01 > 0");
            }
        }
    }

    #endregion

    #region 0x00D chunk

    /// <summary>
    /// CPlugTree 0x00D chunk
    /// </summary>
    [Chunk(0x0904F00D)]
    public class Chunk0904F00D : Chunk<CPlugTree>
    {
        public string? U02;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.name, tryParseToInt32: true);
            rw.Id(ref U02); // doesn't hint anything
        }
    }

    #endregion

    #region 0x011 chunk (FuncTree)

    /// <summary>
    /// CPlugTree 0x011 chunk (FuncTree)
    /// </summary>
    [Chunk(0x0904F011, "FuncTree")]
    public class Chunk0904F011 : Chunk<CPlugTree>
    {
        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CFuncTree>(ref n.funcTree); // FuncTree
        }
    }

    #endregion

    #region 0x015 chunk (translation)

    /// <summary>
    /// CPlugTree 0x015 chunk (translation)
    /// </summary>
    [Chunk(0x0904F015, "translation")]
    public class Chunk0904F015 : Chunk<CPlugTree>
    {
        public int Flags;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref Flags);

            if ((Flags & 4) != 0)
            {
                rw.Iso4(ref n.translation);
            }
        }
    }

    #endregion

    #region 0x016 chunk (properties)

    /// <summary>
    /// CPlugTree 0x016 chunk (properties)
    /// </summary>
    [Chunk(0x0904F016, "properties")]
    public class Chunk0904F016 : Chunk<CPlugTree>
    {
        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugVisual>(ref n.visual); // CPlugVisual?
            rw.NodeRef<CPlug>(ref n.shader, ref n.shaderFile); // definitely Shader, can have CPlugShaderApply or CPlugMaterial
            rw.NodeRef<CPlugSurface>(ref n.surface); // CPlugSurface? CPlugTreeGenerator?
            rw.NodeRef<CPlugTreeGenerator>(ref n.generator);
        }

        public override async Task ReadWriteAsync(CPlugTree n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            n.visual = await rw.NodeRefAsync<CPlugVisual>(n.visual, cancellationToken);
            rw.NodeRef<CPlug>(ref n.shader, ref n.shaderFile);
            n.surface = await rw.NodeRefAsync<CPlugSurface>(n.surface, cancellationToken);
            n.generator = await rw.NodeRefAsync<CPlugTreeGenerator>(n.generator, cancellationToken);
        }
    }

    #endregion

    #region 0x018 chunk (translation)

    /// <summary>
    /// CPlugTree 0x018 chunk (translation)
    /// </summary>
    [Chunk(0x0904F018, "translation")]
    public class Chunk0904F018 : Chunk0904F015
    {

    }

    #endregion

    #region 0x019 chunk (translation)

    /// <summary>
    /// CPlugTree 0x019 chunk (translation)
    /// </summary>
    [Chunk(0x0904F019, "translation")]
    public class Chunk0904F019 : Chunk0904F015
    {
        
    }

    #endregion

    #region 0x01A chunk (translation)

    /// <summary>
    /// CPlugTree 0x01A chunk (translation)
    /// </summary>
    [Chunk(0x0904F01A, "translation")]
    public class Chunk0904F01A : Chunk0904F015
    {
        
    }

    #endregion

    #endregion
}
