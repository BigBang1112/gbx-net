namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0303C000</remarks>
[Class(0x0303C000)]
public partial class CGameCtnDecorationTerrainModifier : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0303C000;




    private CPlugGameSkin? remapping;
    [AppliedWithChunk<Chunk0303C000>]
    public CPlugGameSkin? Remapping { get => remappingFile?.GetNode(ref remapping) ?? remapping; set => remapping = value; }
    private Components.GbxRefTableFile? remappingFile;
    public Components.GbxRefTableFile? RemappingFile { get => remappingFile; set => remappingFile = value; }
    public CPlugGameSkin? GetRemapping(GbxReadSettings settings = default, bool exceptions = false) => remappingFile?.GetNode(ref remapping, settings, exceptions) ?? remapping;

    private string? remapFolder;
    [AppliedWithChunk<Chunk0303C000>]
    public string? RemapFolder { get => remapFolder; set => remapFolder = value; }

    private string? idName;
    [AppliedWithChunk<Chunk0303C001>]
    public string? IdName { get => idName; set => idName = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnDecorationTerrainModifier"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnDecorationTerrainModifier() { }


    /// <summary>
    /// CGameCtnDecorationTerrainModifier 0x000 chunk
    /// </summary>
    [Chunk(0x0303C000)]
    public partial class Chunk0303C000 : Chunk<CGameCtnDecorationTerrainModifier>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303C000;


        public override void ReadWrite(CGameCtnDecorationTerrainModifier n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugGameSkin>(ref n.remapping, ref n.remappingFile);
            rw.String(ref n.remapFolder);
        }
    }

    /// <summary>
    /// CGameCtnDecorationTerrainModifier 0x001 chunk
    /// </summary>
    [Chunk(0x0303C001)]
    public partial class Chunk0303C001 : Chunk<CGameCtnDecorationTerrainModifier>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303C001;


        public override void ReadWrite(CGameCtnDecorationTerrainModifier n, GbxReaderWriter rw)
        {
            rw.Id(ref n.idName);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0303C000 => new Chunk0303C000(),
        0x0303C001 => new Chunk0303C001(),
        _ => base.NewChunk(chunkId),
    };
}
