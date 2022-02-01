namespace GBX.NET.Engines.Plug;

/// <summary>
/// Tree (0x0904F000)
/// </summary>
[Node(0x0904F000)]
public class CPlugTree : CPlug
{
    private IList<CPlugTree?> children;
    private string? name;
    private CPlugVisual? visual;
    private CPlugSurface? surface;
    private CPlug? shader;
    private int? shaderIndex;
    private CPlugTreeGenerator? generator;

    public IList<CPlugTree?> Children
    {
        get => children;
        set => children = value;
    }

    public string? Name
    {
        get => name;
        set => name = value;
    }

    public CPlugVisual? Visual
    {
        get => visual;
        set => visual = value;
    }

    public CPlugSurface? Surface
    {
        get => surface;
        set => surface = value;
    }

    public CPlug? Shader
    {
        get => shader = GetNodeFromRefTable(shader, shaderIndex) as CPlug;
        set => shader = value;
    }

    public CPlugTreeGenerator? Generator
    {
        get => generator;
        set => generator = value;
    }

    protected CPlugTree()
    {
        children = null!;
    }

    public override string ToString() => $"{base.ToString()} {{ \"{name}\" }}";

    [Chunk(0x0904F006)]
    public class Chunk0904F006 : Chunk<CPlugTree>
    {
        public int U01;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // list version
            rw.ListNode<CPlugTree>(ref n.children!);
        }

        public override async Task ReadWriteAsync(CPlugTree n, GameBoxReaderWriter rw, ILogger? logger, CancellationToken cancellationToken = default)
        {
            rw.Int32(ref U01); // list version
            n.children = (await rw.ListNodeAsync<CPlugTree>(n.children!, cancellationToken))!;
        }
    }

    [Chunk(0x0904F00C)]
    public class Chunk0904F00C : Chunk<CPlugTree>
    {
        public int U01;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            // could be array of IDs
        }
    }

    [Chunk(0x0904F00D)]
    public class Chunk0904F00D : Chunk<CPlugTree>
    {
        public CMwNod? U02;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.name);
            rw.NodeRef(ref U02); // node
        }
    }

    [Chunk(0x0904F011)]
    public class Chunk0904F011 : Chunk<CPlugTree>
    {
        public CMwNod? U01;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01); // FuncTree
        }
    }

    [Chunk(0x0904F015)]
    public class Chunk0904F015 : Chunk<CPlugTree>
    {
        public int U01;
        public float[]? U02;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // DoData

            if ((U01 & 4) != 0)
            {
                rw.Array<float>(ref U02, 12); // Iso4
            }
        }
    }

    [Chunk(0x0904F016)]
    public class Chunk0904F016 : Chunk<CPlugTree>
    {
        public int U03;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugVisual>(ref n.visual); // CPlugVisual?
            rw.NodeRef<CPlug>(ref n.shader, ref n.shaderIndex); // definitely Shader, can have CPlugShaderApply or CPlugMaterial
            rw.NodeRef<CPlugSurface>(ref n.surface); // CPlugSurface? CPlugTreeGenerator?
            rw.NodeRef<CPlugTreeGenerator>(ref n.generator);
        }

        public override async Task ReadWriteAsync(CPlugTree n, GameBoxReaderWriter rw, ILogger? logger, CancellationToken cancellationToken = default)
        {
            n.visual = await rw.NodeRefAsync<CPlugVisual>(n.visual, cancellationToken);
            rw.NodeRef<CPlug>(ref n.shader, ref n.shaderIndex);
            n.surface = await rw.NodeRefAsync<CPlugSurface>(n.surface, cancellationToken);
            n.generator = await rw.NodeRefAsync<CPlugTreeGenerator>(n.generator, cancellationToken);
        }
    }

    [Chunk(0x0904F019)]
    public class Chunk0904F019 : Chunk<CPlugTree>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            var flags = rw.Int32();

            if ((flags & 4) != 0)
            {
                rw.Single(ref U01);
                rw.Single(ref U02);
                rw.Single(ref U03);
                rw.Single(ref U04);
                rw.Single(ref U05);
                rw.Single(ref U06);
                rw.Single(ref U07);
                rw.Single(ref U08);
                rw.Single(ref U09);

                rw.Single(ref U10);
                rw.Single(ref U11);
                rw.Single(ref U12);
            }
        }
    }

    [Chunk(0x0904F01A)]
    public class Chunk0904F01A : Chunk<CPlugTree>
    {
        public int flags;

        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;

        public int Flags
        {
            get => flags;
            set => flags = value;
        }

        public override void ReadWrite(CPlugTree n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref flags); // Flags?

            if ((flags & 4) != 0)
            {
                // Iso4
                rw.Single(ref U02);
                rw.Single(ref U03);
                rw.Single(ref U04);
                rw.Single(ref U05);
                rw.Single(ref U06);
                rw.Single(ref U07);
                rw.Single(ref U08);
                rw.Single(ref U09);
                rw.Single(ref U10);

                rw.Single(ref U11);
                rw.Single(ref U12);
                rw.Single(ref U13);
                //
            }
        }
    }
}
