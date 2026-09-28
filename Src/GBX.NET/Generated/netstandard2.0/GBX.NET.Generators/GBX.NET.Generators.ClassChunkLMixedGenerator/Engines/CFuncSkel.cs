namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05005000</remarks>
[Class(0x05005000)]
public partial class CFuncSkel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x05005000;




    private string[]? bones;
    [AppliedWithChunk<Chunk05005002>]
    public string[]? Bones { get => bones; set => bones = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncSkel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncSkel() { }


    /// <summary>
    /// CFuncSkel 0x002 chunk
    /// </summary>
    [Chunk(0x05005002)]
    public partial class Chunk05005002 : Chunk<CFuncSkel>
    {
        /// <inheritdoc />
        public override uint Id => 0x05005002;


        public override void ReadWrite(CFuncSkel n, GbxReaderWriter rw)
        {
            rw.ArrayId(ref n.bones!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x05005002 => new Chunk05005002(),
        _ => base.NewChunk(chunkId),
    };
}
