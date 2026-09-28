namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03140000</remarks>
[Class(0x03140000)]
public partial class CGamePlayerProfileChunk_PackagesInfos : CGamePlayerProfileChunk, IClass
{
    [Hexadecimal] public static new uint Id => 0x03140000;




    private PackageInfo[]? packagesInfos;
    [AppliedWithChunk<Chunk03140000>]
    public PackageInfo[]? PackagesInfos { get => packagesInfos; set => packagesInfos = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGamePlayerProfileChunk_PackagesInfos"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGamePlayerProfileChunk_PackagesInfos() { }


    /// <summary>
    /// CGamePlayerProfileChunk_PackagesInfos 0x000 skippable chunk
    /// </summary>
    [Chunk(0x03140000)]
    public partial class Chunk03140000 : SkippableChunk<CGamePlayerProfileChunk_PackagesInfos>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03140000;

        public int Version { get; set; }


        public override void ReadWrite(CGamePlayerProfileChunk_PackagesInfos n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<PackageInfo>(ref n.packagesInfos!);
        }
    }


    public sealed partial class PackageInfo : IReadableWritable
    {

        private byte[]? checksum;
        public byte[]? Checksum { get => checksum; set => checksum = value; }

        private string? key;
        public string? Key { get => key; set => key = value; }

        private DateTimeOffset? startTimestamp;
        public DateTimeOffset? StartTimestamp { get => startTimestamp; set => startTimestamp = value; }

        private DateTimeOffset? endTimestamp;
        public DateTimeOffset? EndTimestamp { get => endTimestamp; set => endTimestamp = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Data(ref checksum!, 32);
            rw.String(ref key);
            rw.UnixTime(ref startTimestamp);
            rw.UnixTime(ref endTimestamp);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03140000 => new Chunk03140000(),
        _ => base.NewChunk(chunkId),
    };
}
