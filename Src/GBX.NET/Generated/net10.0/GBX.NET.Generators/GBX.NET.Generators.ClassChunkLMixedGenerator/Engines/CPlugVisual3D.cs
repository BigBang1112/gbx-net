namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0902C000</remarks>
[Class(0x0902C000)]
public partial class CPlugVisual3D : CPlugVisual, IClass
{
    [Hexadecimal] public static new uint Id => 0x0902C000;





    /// <summary>
    /// CPlugVisual3D 0x002 chunk
    /// </summary>
    [Chunk(0x0902C002)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0902C002 : Chunk<CPlugVisual3D>
    {
        /// <inheritdoc />
        public override uint Id => 0x0902C002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;

        public CMwNod? U01;

        public override void ReadWrite(CPlugVisual3D n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
        }
    }

    /// <summary>
    /// CPlugVisual3D 0x003 chunk
    /// </summary>
    [Chunk(0x0902C003)]
    [ChunkGameVersion(GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC)]
    public partial class Chunk0902C003 : Chunk<CPlugVisual3D>
    {
        /// <inheritdoc />
        public override uint Id => 0x0902C003;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10 | GameVersion.TMSX | GameVersion.TMNESWC;

    }

    /// <summary>
    /// CPlugVisual3D 0x004 chunk
    /// </summary>
    [Chunk(0x0902C004)]
    [ChunkGameVersion(GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk0902C004 : Chunk<CPlugVisual3D>
    {
        /// <inheritdoc />
        public override uint Id => 0x0902C004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0902C002 => new Chunk0902C002(),
        0x0902C003 => new Chunk0902C003(),
        0x0902C004 => new Chunk0902C004(),
        _ => base.NewChunk(chunkId),
    };
}
