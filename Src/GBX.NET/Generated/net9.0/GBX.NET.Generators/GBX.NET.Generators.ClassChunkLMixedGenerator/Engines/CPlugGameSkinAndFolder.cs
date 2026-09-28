namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0915D000</remarks>
[Class(0x0915D000)]
public partial class CPlugGameSkinAndFolder : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0915D000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugGameSkinAndFolder"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugGameSkinAndFolder() { }


    /// <summary>
    /// CPlugGameSkinAndFolder 0x000 chunk
    /// </summary>
    [Chunk(0x0915D000)]
    public partial class Chunk0915D000 : Chunk<CPlugGameSkinAndFolder>
    {
        /// <inheritdoc />
        public override uint Id => 0x0915D000;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CPlugGameSkinAndFolder 0x001 chunk
    /// </summary>
    [Chunk(0x0915D001)]
    public partial class Chunk0915D001 : Chunk<CPlugGameSkinAndFolder>
    {
        /// <inheritdoc />
        public override uint Id => 0x0915D001;

        public string? U01;

        public override void ReadWrite(CPlugGameSkinAndFolder n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0915D000 => new Chunk0915D000(),
        0x0915D001 => new Chunk0915D001(),
        _ => base.NewChunk(chunkId),
    };
}
