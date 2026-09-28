namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090D4000</remarks>
[Class(0x090D4000)]
public partial class CPlugFogVolume : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x090D4000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugFogVolume"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugFogVolume() { }


    /// <summary>
    /// CPlugFogVolume 0x001 chunk
    /// </summary>
    [Chunk(0x090D4001)]
    public partial class Chunk090D4001 : Chunk<CPlugFogVolume>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090D4001;

        public int Version { get; set; }

        public string? U01;

        public override void ReadWrite(CPlugFogVolume n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090D4001 => new Chunk090D4001(),
        _ => base.NewChunk(chunkId),
    };
}
