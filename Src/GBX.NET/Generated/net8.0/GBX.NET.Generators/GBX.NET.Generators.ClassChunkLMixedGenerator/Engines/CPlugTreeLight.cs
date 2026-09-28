namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09062000</remarks>
[Class(0x09062000)]
public partial class CPlugTreeLight : CPlugTree, IClass
{
    [Hexadecimal] public static new uint Id => 0x09062000;




    private CPlugLight? plugLight;
    [AppliedWithChunk<Chunk09062004>]
    public CPlugLight? PlugLight { get => plugLightFile?.GetNode(ref plugLight) ?? plugLight; set => plugLight = value; }
    private Components.GbxRefTableFile? plugLightFile;
    public Components.GbxRefTableFile? PlugLightFile { get => plugLightFile; set => plugLightFile = value; }
    public CPlugLight? GetPlugLight(GbxReadSettings settings = default, bool exceptions = false) => plugLightFile?.GetNode(ref plugLight, settings, exceptions) ?? plugLight;

    /// <summary>
    /// Creates a new instance of <see cref="CPlugTreeLight"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugTreeLight() { }


    /// <summary>
    /// CPlugTreeLight 0x004 chunk
    /// </summary>
    [Chunk(0x09062004)]
    [ChunkGameVersion(GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4)]
    public partial class Chunk09062004 : Chunk<CPlugTreeLight>
    {
        /// <inheritdoc />
        public override uint Id => 0x09062004;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TMSX | GameVersion.TMNESWC | GameVersion.TMF | GameVersion.TMT | GameVersion.MP4;


        public override void ReadWrite(CPlugTreeLight n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugLight>(ref n.plugLight, ref n.plugLightFile);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09062004 => new Chunk09062004(),
        _ => base.NewChunk(chunkId),
    };
}
