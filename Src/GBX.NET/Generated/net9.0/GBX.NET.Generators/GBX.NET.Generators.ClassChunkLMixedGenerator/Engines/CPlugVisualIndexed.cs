namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0906A000</remarks>
[Class(0x0906A000)]
public partial class CPlugVisualIndexed : CPlugVisual3D, IClass
{
    [Hexadecimal] public static new uint Id => 0x0906A000;





    /// <summary>
    /// CPlugVisualIndexed 0x000 chunk
    /// </summary>
    [Chunk(0x0906A000)]
    [ChunkGameVersion(GameVersion.TM10)]
    public partial class Chunk0906A000 : Chunk<CPlugVisualIndexed>
    {
        /// <inheritdoc />
        public override uint Id => 0x0906A000;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10;

    }

    /// <summary>
    /// CPlugVisualIndexed 0x001 chunk
    /// </summary>
    [Chunk(0x0906A001)]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0906A001 : Chunk<CPlugVisualIndexed>
    {
        /// <inheritdoc />
        public override uint Id => 0x0906A001;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0906A000 => new Chunk0906A000(),
        0x0906A001 => new Chunk0906A001(),
        _ => base.NewChunk(chunkId),
    };
}
