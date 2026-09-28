namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x031B7000</remarks>
[Class(0x031B7000)]
public partial class CGameUserFileList : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x031B7000;




    private FileInfo[]? files;
    [AppliedWithChunk<Chunk031B7000>]
    public FileInfo[]? Files { get => files; set => files = value; }


    /// <summary>
    /// CGameUserFileList 0x000 chunk
    /// </summary>
    [Chunk(0x031B7000)]
    public partial class Chunk031B7000 : Chunk<CGameUserFileList>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031B7000;

        public int Version { get; set; }


        public override void ReadWrite(CGameUserFileList n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<FileInfo>(ref n.files!, version: Version);
        }
    }


    public sealed partial class FileInfo : IReadableWritable
    {
    }


    public enum FileType
    {
        Map,
        Ghost,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x031B7000 => new Chunk031B7000(),
        _ => base.NewChunk(chunkId),
    };
}
