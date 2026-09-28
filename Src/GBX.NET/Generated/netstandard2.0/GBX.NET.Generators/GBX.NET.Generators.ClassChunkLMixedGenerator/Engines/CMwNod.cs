namespace GBX.NET.Engines.MwFoundations;

/// <remarks>ID: 0x01001000</remarks>
public partial class CMwNod
{
    [Hexadecimal] public static new uint Id => 0x01001000;





    /// <summary>
    /// CMwNod 0x000 chunk
    /// </summary>
    [Chunk(0x01001000)]
    public partial class Chunk01001000 : Chunk<CMwNod>
    {
        /// <inheritdoc />
        public override uint Id => 0x01001000;

        public string? U01;

        public override void ReadWrite(CMwNod n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
        }
    }



}
