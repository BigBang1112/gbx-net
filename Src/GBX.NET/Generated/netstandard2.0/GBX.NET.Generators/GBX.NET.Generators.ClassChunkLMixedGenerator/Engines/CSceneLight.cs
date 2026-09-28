namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A00B000</remarks>
[Class(0x0A00B000)]
public partial class CSceneLight : CScenePoc, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A00B000;




    private CHmsLight? light;
    [AppliedWithChunk<Chunk0A00B000>]
    public CHmsLight? Light { get => light; set => light = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CSceneLight"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneLight() { }


    /// <summary>
    /// CSceneLight 0x000 chunk
    /// </summary>
    [Chunk(0x0A00B000)]
    public partial class Chunk0A00B000 : Chunk<CSceneLight>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A00B000;


        public override void ReadWrite(CSceneLight n, GbxReaderWriter rw)
        {
            rw.Node<CHmsLight>(ref n.light);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A00B000 => new Chunk0A00B000(),
        _ => base.NewChunk(chunkId),
    };
}
