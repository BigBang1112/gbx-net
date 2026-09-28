namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0910B000</remarks>
[Class(0x0910B000)]
public partial class CPlugCamShakeModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0910B000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugCamShakeModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugCamShakeModel() { }


    /// <summary>
    /// CPlugCamShakeModel 0x000 chunk
    /// </summary>
    [Chunk(0x0910B000)]
    public partial class Chunk0910B000 : Chunk<CPlugCamShakeModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0910B000;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float? U07;
        public int? U08;
        public int? U09;
        public float? U10;
        public float? U11;
        public float? U12;
        public float? U13;
        public float? U14;

        public override void ReadWrite(CPlugCamShakeModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            if (Version >= 2)
            {
                rw.Single(ref U07);
                if (Version >= 3)
                {
                    rw.Int32(ref U08);
                    rw.Int32(ref U09);
                    rw.Single(ref U10);
                    rw.Single(ref U11);
                    rw.Single(ref U12);
                    rw.Single(ref U13);
                    rw.Single(ref U14);
                }
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0910B000 => new Chunk0910B000(),
        _ => base.NewChunk(chunkId),
    };
}
