namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09118000</remarks>
[Class(0x09118000)]
public partial class CPlugPolyLine3 : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x09118000;




    private Vec3[]? poss;
    [AppliedWithChunk<Chunk09118000>]
    public Vec3[]? Poss { get => poss; set => poss = value; }

    private Vec3[]? lefts;
    [AppliedWithChunk<Chunk09118000>]
    public Vec3[]? Lefts { get => lefts; set => lefts = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugPolyLine3"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugPolyLine3() { }


    /// <summary>
    /// CPlugPolyLine3 0x000 chunk
    /// </summary>
    [Chunk(0x09118000)]
    public partial class Chunk09118000 : Chunk<CPlugPolyLine3>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09118000;

        public int Version { get; set; }

        public bool U01;
        public int U02;
        public bool U03;
        public bool U04;
        public bool U05;
        public bool U06;
        public int U07;
        public byte U08;
        public byte U09;
        public string? U10;

        public override void ReadWrite(CPlugPolyLine3 n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Array<Vec3>(ref n.poss!);
            if (Version >= 2)
            {
                rw.Array<Vec3>(ref n.lefts!);
                if (Version == 3)
                {
                    rw.Boolean(ref U01);
                    rw.Int32(ref U02);
                }
                if (Version >= 4)
                {
                    if (Version == 4)
                    {
                        rw.Boolean(ref U03);
                    }
                    rw.Boolean(ref U04);
                    rw.Boolean(ref U05);
                    if (Version >= 5)
                    {
                        rw.Boolean(ref U06);
                        if (Version >= 6)
                        {
                            rw.Int32(ref U07);
                            if (Version >= 7)
                            {
                                rw.Byte(ref U08);
                                if (Version >= 8)
                                {
                                    rw.Byte(ref U09);
                                    rw.Id(ref U10);
                                }
                            }
                        }
                    }
                }
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09118000 => new Chunk09118000(),
        _ => base.NewChunk(chunkId),
    };
}
