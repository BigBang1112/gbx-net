namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03008000</remarks>
[Class(0x03008000)]
public partial class CGameNod : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03008000;




    private string? name;
    [AppliedWithChunk<Chunk03008000>]
    public string? Name { get => name; set => name = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameNod"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameNod() { }


    /// <summary>
    /// CGameNod 0x000 chunk
    /// </summary>
    [Chunk(0x03008000)]
    public partial class Chunk03008000 : Chunk<CGameNod>
    {
        /// <inheritdoc />
        public override uint Id => 0x03008000;


        public override void ReadWrite(CGameNod n, GbxReaderWriter rw)
        {
            rw.Id(ref n.name);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03008000 => new Chunk03008000(),
        _ => base.NewChunk(chunkId),
    };
}
