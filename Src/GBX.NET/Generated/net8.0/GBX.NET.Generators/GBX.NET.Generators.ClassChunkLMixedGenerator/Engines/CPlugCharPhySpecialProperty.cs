namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F2000</remarks>
[Class(0x090F2000)]
public partial class CPlugCharPhySpecialProperty : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090F2000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugCharPhySpecialProperty"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugCharPhySpecialProperty() { }


    /// <summary>
    /// CPlugCharPhySpecialProperty 0x000 chunk
    /// </summary>
    [Chunk(0x090F2000)]
    public partial class Chunk090F2000 : Chunk<CPlugCharPhySpecialProperty>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090F2000;

        public int Version { get; set; }

        public int U01;
        public float U02;
        public float U03;
        public float U04;
        public bool U05;
        public int U06;
        public float U07;
        public int U08;
        public float U09;
        public int U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;
        public int U15;
        public bool U16;

        public override void ReadWrite(CPlugCharPhySpecialProperty n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
            if (Version <= 1)
            {
                rw.Single(ref U02);
                rw.Single(ref U03);
                rw.Single(ref U04);
            }
            rw.Boolean(ref U05);
            rw.Int32(ref U06);
            rw.Single(ref U07);
            rw.Int32(ref U08);
            rw.Single(ref U09);
            if (Version >= 1)
            {
                rw.Int32(ref U10);
            }
            if (Version >= 2)
            {
                rw.Single(ref U11);
                rw.Single(ref U12);
                rw.Single(ref U13);
                rw.Single(ref U14);
                if (Version >= 3)
                {
                    rw.Int32(ref U15);
                    if (Version >= 4)
                    {
                        rw.Boolean(ref U16);
                    }
                }
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090F2000 => new Chunk090F2000(),
        _ => base.NewChunk(chunkId),
    };
}
