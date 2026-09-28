namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03168000</remarks>
[Class(0x03168000)]
public partial class CGamePodiumInfo : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03168000;




    private External<CMwNod>[]? mediaClipFids;
    [AppliedWithChunk<Chunk03168000>]
    public External<CMwNod>[]? MediaClipFids { get => mediaClipFids; set => mediaClipFids = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGamePodiumInfo"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePodiumInfo() { }


    /// <summary>
    /// CGamePodiumInfo 0x000 chunk
    /// </summary>
    [Chunk(0x03168000)]
    public partial class Chunk03168000 : Chunk<CGamePodiumInfo>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03168000;

        public int Version { get; set; }


        public override void ReadWrite(CGamePodiumInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayNodeRef<CMwNod>(ref n.mediaClipFids!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03168000 => new Chunk03168000(),
        _ => base.NewChunk(chunkId),
    };
}
