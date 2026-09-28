namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05014000</remarks>
[Class(0x05014000)]
public partial class CFuncShaders : CFuncShader, IClass
{
    [Hexadecimal] public static new uint Id => 0x05014000;




    private List<CFuncShader>? funcShaders;
    [AppliedWithChunk<Chunk05014000>]
    public List<CFuncShader>? FuncShaders { get => funcShaders; set => funcShaders = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncShaders"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncShaders() { }


    /// <summary>
    /// CFuncShaders 0x000 chunk
    /// </summary>
    [Chunk(0x05014000)]
    public partial class Chunk05014000 : Chunk<CFuncShaders>
    {
        /// <inheritdoc />
        public override uint Id => 0x05014000;


        public override void ReadWrite(CFuncShaders n, GbxReaderWriter rw)
        {
            rw.ListNodeRef<CFuncShader>(ref n.funcShaders!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x05014000 => new Chunk05014000(),
        _ => base.NewChunk(chunkId),
    };
}
