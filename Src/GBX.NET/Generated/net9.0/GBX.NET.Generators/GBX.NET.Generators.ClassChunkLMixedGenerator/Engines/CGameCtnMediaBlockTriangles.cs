namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03029000</remarks>
[Class(0x03029000)]
public partial class CGameCtnMediaBlockTriangles : CGameCtnMediaBlock, IClass
{
    [Hexadecimal] public static new uint Id => 0x03029000;





    /// <summary>
    /// CGameCtnMediaBlockTriangles 0x001 chunk
    /// </summary>
    [Chunk(0x03029001)]
    public partial class Chunk03029001 : Chunk<CGameCtnMediaBlockTriangles>
    {
        /// <inheritdoc />
        public override uint Id => 0x03029001;

    }

    /// <summary>
    /// CGameCtnMediaBlockTriangles 0x002 skippable chunk
    /// </summary>
    [Chunk(0x03029002)]
    public partial class Chunk03029002 : SkippableChunk<CGameCtnMediaBlockTriangles>
    {
        /// <inheritdoc />
        public override uint Id => 0x03029002;

        public int U01;

        public override void ReadWrite(CGameCtnMediaBlockTriangles n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03029001 => new Chunk03029001(),
        0x03029002 => new Chunk03029002(),
        _ => base.NewChunk(chunkId),
    };
}
