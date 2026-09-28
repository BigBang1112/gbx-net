namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09001000</remarks>
[Class(0x09001000)]
public abstract partial class CPlugAudio : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x09001000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugAudio"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugAudio() { }


    /// <summary>
    /// CPlugAudio 0x001 chunk
    /// </summary>
    [Chunk(0x09001001)]
    public partial class Chunk09001001 : Chunk<CPlugAudio>
    {
        /// <inheritdoc />
        public override uint Id => 0x09001001;

        public string? U01;

        public override void ReadWrite(CPlugAudio n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09001001 => new Chunk09001001(),
        _ => base.NewChunk(chunkId),
    };
}
