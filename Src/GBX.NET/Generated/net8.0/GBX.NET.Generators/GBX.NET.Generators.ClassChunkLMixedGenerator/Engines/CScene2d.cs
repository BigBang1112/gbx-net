namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A002000</remarks>
[Class(0x0A002000)]
public partial class CScene2d : CScene, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A002000;




    private Rect overlay;
    [AppliedWithChunk<Chunk0A002000>]
    public Rect Overlay { get => overlay; set => overlay = value; }

    private CSceneSector? sector;
    [AppliedWithChunk<Chunk0A002000>]
    public CSceneSector? Sector { get => sector; set => sector = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CScene2d"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CScene2d() { }


    /// <summary>
    /// CScene2d 0x000 chunk
    /// </summary>
    [Chunk(0x0A002000)]
    public partial class Chunk0A002000 : Chunk<CScene2d>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A002000;


        public override void ReadWrite(CScene2d n, GbxReaderWriter rw)
        {
            rw.Rect(ref n.overlay);
            rw.NodeRef<CSceneSector>(ref n.sector);
        }
    }

    /// <summary>
    /// CScene2d 0x003 chunk
    /// </summary>
    [Chunk(0x0A002003)]
    public partial class Chunk0A002003 : Chunk<CScene2d>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A002003;

        /// <inheritdoc />
        public override bool Ignore => true;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A002000 => new Chunk0A002000(),
        0x0A002003 => new Chunk0A002003(),
        _ => base.NewChunk(chunkId),
    };
}
