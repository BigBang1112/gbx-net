namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A00E000</remarks>
[Class(0x0A00E000)]
public partial class CSceneSoundSource : CScenePoc, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A00E000;




    private CHmsSoundSource? soundSource;
    /// <summary>
    /// Gliding/AfterBurnout/Burnout
    /// </summary>
    [AppliedWithChunk<Chunk0A00E000>]
    public CHmsSoundSource? SoundSource { get => soundSource; set => soundSource = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CSceneSoundSource"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneSoundSource() { }


    /// <summary>
    /// CSceneSoundSource 0x000 chunk
    /// </summary>
    [Chunk(0x0A00E000)]
    public partial class Chunk0A00E000 : Chunk<CSceneSoundSource>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A00E000;


        public override void ReadWrite(CSceneSoundSource n, GbxReaderWriter rw)
        {
            rw.Node<CHmsSoundSource>(ref n.soundSource); // Gliding/AfterBurnout/Burnout
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A00E000 => new Chunk0A00E000(),
        _ => base.NewChunk(chunkId),
    };
}
