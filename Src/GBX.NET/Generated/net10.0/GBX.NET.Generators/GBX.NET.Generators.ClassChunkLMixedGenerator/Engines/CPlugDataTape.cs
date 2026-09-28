namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090CE000</remarks>
[Class(0x090CE000)]
public partial class CPlugDataTape : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090CE000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugDataTape"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugDataTape() { }


    /// <summary>
    /// CPlugDataTape 0x001 chunk
    /// </summary>
    [Chunk(0x090CE001)]
    public partial class Chunk090CE001 : Chunk<CPlugDataTape>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090CE001;

        public int Version { get; set; }

        public int U01;
        public int[]? U02;
        public int[]? U03;
        public byte[]? U04;
        public int U05;
        public byte[]? U06;

        public override void ReadWrite(CPlugDataTape n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.Array<int>(ref U02!);
            rw.Array<int>(ref U03!);
            rw.Data(ref U04);
            rw.Int32(ref U05);
            rw.Data(ref U06);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090CE001 => new Chunk090CE001(),
        _ => base.NewChunk(chunkId),
    };
}
