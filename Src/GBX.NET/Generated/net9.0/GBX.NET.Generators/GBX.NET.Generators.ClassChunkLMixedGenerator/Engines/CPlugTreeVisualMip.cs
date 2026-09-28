namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09015000</remarks>
[Class(0x09015000)]
public partial class CPlugTreeVisualMip : CPlugTree, IClass
{
    [Hexadecimal] public static new uint Id => 0x09015000;





    /// <summary>
    /// CPlugTreeVisualMip 0x002 chunk
    /// </summary>
    [Chunk(0x09015002)]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk09015002 : Chunk<CPlugTreeVisualMip>
    {
        /// <inheritdoc />
        public override uint Id => 0x09015002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09015002 => new Chunk09015002(),
        _ => base.NewChunk(chunkId),
    };
}
