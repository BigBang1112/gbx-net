namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A014000</remarks>
[Class(0x0A014000)]
public partial class CSceneObjectLink : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A014000;




    private CSceneObject? @object;
    [AppliedWithChunk<Chunk0A00F000>]
    [AppliedWithChunk<Chunk0A014001>]
    public CSceneObject? Object { get => @object; set => @object = value; }

    private Iso4 relativeLocation;
    [AppliedWithChunk<Chunk0A00F000>]
    [AppliedWithChunk<Chunk0A014001>]
    public Iso4 RelativeLocation { get => relativeLocation; set => relativeLocation = value; }

    private CSceneMobil? mobil;
    [AppliedWithChunk<Chunk0A014001>]
    public CSceneMobil? Mobil { get => mobilFile?.GetNode(ref mobil) ?? mobil; set => mobil = value; }
    private Components.GbxRefTableFile? mobilFile;
    public Components.GbxRefTableFile? MobilFile { get => mobilFile; set => mobilFile = value; }
    public CSceneMobil? GetMobil(GbxReadSettings settings = default, bool exceptions = false) => mobilFile?.GetNode(ref mobil, settings, exceptions) ?? mobil;

    private string? name;
    [AppliedWithChunk<Chunk0A014001>]
    public string? Name { get => name; set => name = value; }

    private string? mobilTreeId;
    [AppliedWithChunk<Chunk0A014002>]
    public string? MobilTreeId { get => mobilTreeId; set => mobilTreeId = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CSceneObjectLink"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneObjectLink() { }


    /// <summary>
    /// CSceneObjectLink 0x000 chunk
    /// </summary>
    [Chunk(0x0A00F000)]
    public partial class Chunk0A00F000 : Chunk<CSceneObjectLink>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A00F000;

        public bool U01;

        public override void ReadWrite(CSceneObjectLink n, GbxReaderWriter rw)
        {
            rw.NodeRef<CSceneObject>(ref n.@object);
            rw.Iso4(ref n.relativeLocation);
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSceneObjectLink 0x001 chunk
    /// </summary>
    [Chunk(0x0A00F001)]
    public partial class Chunk0A00F001 : Chunk<CSceneObjectLink>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A00F001;

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public bool U07;

        public override void ReadWrite(CSceneObjectLink n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Boolean(ref U07);
        }
    }

    /// <summary>
    /// CSceneObjectLink 0x002 chunk
    /// </summary>
    [Chunk(0x0A00F002)]
    public partial class Chunk0A00F002 : Chunk<CSceneObjectLink>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A00F002;

        public bool U01;

        public override void ReadWrite(CSceneObjectLink n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CSceneObjectLink 0x001 chunk
    /// </summary>
    [Chunk(0x0A014001)]
    public partial class Chunk0A014001 : Chunk<CSceneObjectLink>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A014001;

        public bool U01;
        /// <summary>
        /// array of tree ids?
        /// </summary>
        public int U02;
        public int U03;
        public int U05;
        public bool U06;

        public override void ReadWrite(CSceneObjectLink n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            if (U01)
            {
                rw.NodeRef<CSceneMobil>(ref n.mobil, ref n.mobilFile);
                rw.Int32(ref U02); // array of tree ids?
                if (U02==2)
                {
                    rw.Int32(ref U03);
                }
                rw.Id(ref n.name);
                rw.Int32(ref U05);
            }
            if (!U01)
            {
                rw.NodeRef<CSceneObject>(ref n.@object);
            }
            rw.Iso4(ref n.relativeLocation);
            rw.Boolean(ref U06);
        }
    }

    /// <summary>
    /// CSceneObjectLink 0x002 chunk
    /// </summary>
    [Chunk(0x0A014002)]
    public partial class Chunk0A014002 : Chunk<CSceneObjectLink>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A014002;

        public bool U01;
        public bool U02;

        public override void ReadWrite(CSceneObjectLink n, GbxReaderWriter rw)
        {
            rw.Id(ref n.mobilTreeId);
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
        }
    }

    /// <summary>
    /// CSceneObjectLink 0x003 chunk
    /// </summary>
    [Chunk(0x0A014003)]
    public partial class Chunk0A014003 : Chunk<CSceneObjectLink>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A014003;

        public string? U01;

        public override void ReadWrite(CSceneObjectLink n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A00F000 => new Chunk0A00F000(),
        0x0A00F001 => new Chunk0A00F001(),
        0x0A00F002 => new Chunk0A00F002(),
        0x0A014001 => new Chunk0A014001(),
        0x0A014002 => new Chunk0A014002(),
        0x0A014003 => new Chunk0A014003(),
        _ => base.NewChunk(chunkId),
    };
}
