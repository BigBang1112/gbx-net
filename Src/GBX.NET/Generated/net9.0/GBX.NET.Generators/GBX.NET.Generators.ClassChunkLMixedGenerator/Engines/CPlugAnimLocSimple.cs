namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F8000</remarks>
[Class(0x090F8000)]
public partial class CPlugAnimLocSimple : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090F8000;




    private int rotPeriod;
    [AppliedWithChunk<Chunk090F8000>]
    public int RotPeriod { get => rotPeriod; set => rotPeriod = value; }

    private int transPeriod;
    [AppliedWithChunk<Chunk090F8000>]
    public int TransPeriod { get => transPeriod; set => transPeriod = value; }

    private float transY;
    [AppliedWithChunk<Chunk090F8000>]
    public float TransY { get => transY; set => transY = value; }

    private int axis;
    [AppliedWithChunk<Chunk090F8000>]
    public int Axis { get => axis; set => axis = value; }

    private int rotPeriodMax;
    [AppliedWithChunk<Chunk090F8000>]
    public int RotPeriodMax { get => rotPeriodMax; set => rotPeriodMax = value; }

    private int transPeriodMax;
    [AppliedWithChunk<Chunk090F8000>]
    public int TransPeriodMax { get => transPeriodMax; set => transPeriodMax = value; }

    private byte rotFunc;
    [AppliedWithChunk<Chunk090F8000>]
    public byte RotFunc { get => rotFunc; set => rotFunc = value; }

    private float rotAngle;
    [AppliedWithChunk<Chunk090F8000>]
    public float RotAngle { get => rotAngle; set => rotAngle = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugAnimLocSimple"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugAnimLocSimple() { }


    /// <summary>
    /// CPlugAnimLocSimple 0x000 chunk
    /// </summary>
    [Chunk(0x090F8000)]
    public partial class Chunk090F8000 : Chunk<CPlugAnimLocSimple>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090F8000;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CPlugAnimLocSimple n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.VersionInt32(this);
            rw.Int32(ref n.rotPeriod);
            rw.Int32(ref n.transPeriod);
            rw.Single(ref n.transY);
            if (Version >= 1)
            {
                rw.Int32(ref n.axis);
                if (Version >= 2)
                {
                    rw.Int32(ref n.rotPeriodMax);
                    rw.Int32(ref n.transPeriodMax);
                    if (Version >= 3)
                    {
                        rw.Byte(ref n.rotFunc);
                        rw.Single(ref n.rotAngle);
                    }
                }
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090F8000 => new Chunk090F8000(),
        _ => base.NewChunk(chunkId),
    };
}
