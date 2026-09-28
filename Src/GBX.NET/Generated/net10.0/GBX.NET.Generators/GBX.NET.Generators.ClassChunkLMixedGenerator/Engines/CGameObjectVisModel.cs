namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E007000</remarks>
[Class(0x2E007000)]
public partial class CGameObjectVisModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E007000;




    private string? soundRefSpawn;
    [AppliedWithChunk<Chunk2E007002>]
    public string? SoundRefSpawn { get => soundRefSpawn; set => soundRefSpawn = value; }

    private string? soundRefUnspawn;
    [AppliedWithChunk<Chunk2E007002>]
    public string? SoundRefUnspawn { get => soundRefUnspawn; set => soundRefUnspawn = value; }

    private string? soundRefGrab;
    [AppliedWithChunk<Chunk2E007002>]
    public string? SoundRefGrab { get => soundRefGrab; set => soundRefGrab = value; }

    private string? soundRefSmashed;
    [AppliedWithChunk<Chunk2E007002>]
    public string? SoundRefSmashed { get => soundRefSmashed; set => soundRefSmashed = value; }

    private string? soundRefPermanent;
    [AppliedWithChunk<Chunk2E007002>]
    public string? SoundRefPermanent { get => soundRefPermanent; set => soundRefPermanent = value; }

    private Iso4 soundLocPermanent;
    [AppliedWithChunk<Chunk2E007002>]
    public Iso4 SoundLocPermanent { get => soundLocPermanent; set => soundLocPermanent = value; }


    /// <summary>
    /// CGameObjectVisModel 0x001 chunk
    /// </summary>
    [Chunk(0x2E007001)]
    public partial class Chunk2E007001 : Chunk<CGameObjectVisModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E007001;

    }

    /// <summary>
    /// CGameObjectVisModel 0x002 skippable chunk
    /// </summary>
    [Chunk(0x2E007002)]
    public partial class Chunk2E007002 : SkippableChunk<CGameObjectVisModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E007002;

        public int Version { get; set; }


        public override void ReadWrite(CGameObjectVisModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref n.soundRefSpawn);
            rw.String(ref n.soundRefUnspawn);
            rw.String(ref n.soundRefGrab);
            rw.String(ref n.soundRefSmashed);
            rw.String(ref n.soundRefPermanent);
            rw.Iso4(ref n.soundLocPermanent);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E007001 => new Chunk2E007001(),
        0x2E007002 => new Chunk2E007002(),
        _ => base.NewChunk(chunkId),
    };
}
