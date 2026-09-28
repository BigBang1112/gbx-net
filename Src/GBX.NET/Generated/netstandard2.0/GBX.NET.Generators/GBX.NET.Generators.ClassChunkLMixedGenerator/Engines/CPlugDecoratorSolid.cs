namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090A3000</remarks>
[Class(0x090A3000)]
public partial class CPlugDecoratorSolid : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090A3000;




    private CPlugDecoratorTree[]? treeDecorators;
    [AppliedWithChunk<Chunk090A3000>]
    public CPlugDecoratorTree[]? TreeDecorators { get => treeDecorators; set => treeDecorators = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugDecoratorSolid"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugDecoratorSolid() { }


    /// <summary>
    /// CPlugDecoratorSolid 0x000 chunk
    /// </summary>
    [Chunk(0x090A3000)]
    public partial class Chunk090A3000 : Chunk<CPlugDecoratorSolid>
    {
        /// <inheritdoc />
        public override uint Id => 0x090A3000;


        public override void ReadWrite(CPlugDecoratorSolid n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CPlugDecoratorTree>(ref n.treeDecorators!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090A3000 => new Chunk090A3000(),
        _ => base.NewChunk(chunkId),
    };
}
