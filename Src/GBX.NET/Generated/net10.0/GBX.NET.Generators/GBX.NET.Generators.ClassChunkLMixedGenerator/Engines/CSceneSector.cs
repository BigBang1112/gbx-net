namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A004000</remarks>
[Class(0x0A004000)]
public partial class CSceneSector : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A004000;




    private int scene;
    [AppliedWithChunk<Chunk0A004000>]
    public int Scene { get => scene; set => scene = value; }

    private CHmsZone? zone;
    [AppliedWithChunk<Chunk0A004000>]
    public CHmsZone? Zone { get => zone; set => zone = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CSceneSector"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneSector() { }


    /// <summary>
    /// CSceneSector 0x000 chunk
    /// </summary>
    [Chunk(0x0A004000)]
    public partial class Chunk0A004000 : Chunk<CSceneSector>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A004000;


        public override void ReadWrite(CSceneSector n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.scene);
            rw.NodeRef<CHmsZone>(ref n.zone);
        }
    }

    /// <summary>
    /// CSceneSector 0x001 chunk
    /// </summary>
    [Chunk(0x0A004001)]
    public partial class Chunk0A004001 : Chunk<CSceneSector>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A004001;

        public Iso4 U01;

        public override void ReadWrite(CSceneSector n, GbxReaderWriter rw)
        {
            rw.Iso4(ref U01);
        }
    }

    /// <summary>
    /// CSceneSector 0x002 chunk
    /// </summary>
    [Chunk(0x0A004002)]
    public partial class Chunk0A004002 : Chunk<CSceneSector>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A004002;

        public string? U01;

        public override void ReadWrite(CSceneSector n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    /// <summary>
    /// CSceneSector 0x004 chunk
    /// </summary>
    [Chunk(0x0A004004)]
    public partial class Chunk0A004004 : Chunk<CSceneSector>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A004004;

        public BoxAligned U01;

        public override void ReadWrite(CSceneSector n, GbxReaderWriter rw)
        {
            rw.BoxAligned(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A004000 => new Chunk0A004000(),
        0x0A004001 => new Chunk0A004001(),
        0x0A004002 => new Chunk0A004002(),
        0x0A004004 => new Chunk0A004004(),
        _ => base.NewChunk(chunkId),
    };
}
