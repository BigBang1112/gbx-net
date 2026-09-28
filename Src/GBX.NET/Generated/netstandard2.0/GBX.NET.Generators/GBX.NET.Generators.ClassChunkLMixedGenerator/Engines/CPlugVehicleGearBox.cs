namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09094000</remarks>
[Class(0x09094000)]
public partial class CPlugVehicleGearBox : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x09094000;




    /// <summary>
    /// Creates a new instance of <see cref="CPlugVehicleGearBox"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVehicleGearBox() { }


    /// <summary>
    /// CPlugVehicleGearBox 0x001 chunk
    /// </summary>
    [Chunk(0x09094001)]
    public partial class Chunk09094001 : Chunk<CPlugVehicleGearBox>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09094001;

        public int Version { get; set; }

        public float[]? U01;
        public bool U02;
        public int U03;
        public int U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float? U11;
        public float? U12;

        public override void ReadWrite(CPlugVehicleGearBox n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Array<float>(ref U01!);
            rw.Boolean(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            if (Version >= 1)
            {
                rw.Single(ref U11);
                if (Version >= 2)
                {
                    rw.Single(ref U12);
                }
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09094001 => new Chunk09094001(),
        _ => base.NewChunk(chunkId),
    };
}
