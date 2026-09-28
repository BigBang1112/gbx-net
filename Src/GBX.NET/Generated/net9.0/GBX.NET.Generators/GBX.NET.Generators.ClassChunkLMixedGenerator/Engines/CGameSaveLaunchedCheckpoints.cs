namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03262000</remarks>
[Class(0x03262000)]
public partial class CGameSaveLaunchedCheckpoints : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03262000;




    /// <summary>
    /// Creates a new instance of <see cref="CGameSaveLaunchedCheckpoints"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameSaveLaunchedCheckpoints() { }


    /// <summary>
    /// CGameSaveLaunchedCheckpoints 0x000 chunk
    /// </summary>
    [Chunk(0x03262000)]
    public partial class Chunk03262000 : Chunk<CGameSaveLaunchedCheckpoints>
    {
        /// <inheritdoc />
        public override uint Id => 0x03262000;

        /// <inheritdoc />
        public override bool Ignore => true;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03262000 => new Chunk03262000(),
        _ => base.NewChunk(chunkId),
    };
}
