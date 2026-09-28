namespace GBX.NET.Engines.MwFoundations;

/// <remarks>ID: 0x01058000</remarks>
[Class(0x01058000)]
public partial class CMwCmdExpClassParam : CMwCmdExpClass, IClass
{
    [Hexadecimal] public static new uint Id => 0x01058000;




    /// <summary>
    /// Creates a new instance of <see cref="CMwCmdExpClassParam"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMwCmdExpClassParam() { }


    /// <summary>
    /// CMwCmdExpClassParam 0x001 chunk
    /// </summary>
    [Chunk(0x01058001)]
    public partial class Chunk01058001 : Chunk<CMwCmdExpClassParam>
    {
        /// <inheritdoc />
        public override uint Id => 0x01058001;

        /// <inheritdoc />
        public override bool Ignore => true;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x01058001 => new Chunk01058001(),
        _ => base.NewChunk(chunkId),
    };
}
