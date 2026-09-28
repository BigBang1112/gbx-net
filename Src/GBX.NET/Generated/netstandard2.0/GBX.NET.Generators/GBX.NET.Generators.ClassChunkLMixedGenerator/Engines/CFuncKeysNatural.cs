namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05030000</remarks>
[Class(0x05030000)]
public partial class CFuncKeysNatural : CFuncKeys, IClass
{
    [Hexadecimal] public static new uint Id => 0x05030000;




    private int[]? naturals;
    [AppliedWithChunk<Chunk05030000>]
    public int[]? Naturals { get => naturals; set => naturals = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncKeysNatural"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncKeysNatural() { }


    /// <summary>
    /// CFuncKeysNatural 0x000 chunk
    /// </summary>
    [Chunk(0x05030000)]
    public partial class Chunk05030000 : Chunk<CFuncKeysNatural>
    {
        /// <inheritdoc />
        public override uint Id => 0x05030000;


        public override void ReadWrite(CFuncKeysNatural n, GbxReaderWriter rw)
        {
            rw.Array<int>(ref n.naturals!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x05030000 => new Chunk05030000(),
        _ => base.NewChunk(chunkId),
    };
}
