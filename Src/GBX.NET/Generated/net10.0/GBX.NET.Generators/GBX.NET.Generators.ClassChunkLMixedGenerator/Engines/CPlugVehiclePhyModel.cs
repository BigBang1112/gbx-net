namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090EA000</remarks>
[Class(0x090EA000)]
public partial class CPlugVehiclePhyModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090EA000;




    private CMwRefBuffer? refBuffer;
    [AppliedWithChunk<Chunk090EA002>]
    public CMwRefBuffer? RefBuffer { get => refBufferFile?.GetNode(ref refBuffer) ?? refBuffer; set => refBuffer = value; }
    private Components.GbxRefTableFile? refBufferFile;
    public Components.GbxRefTableFile? RefBufferFile { get => refBufferFile; set => refBufferFile = value; }
    public CMwRefBuffer? GetRefBuffer(GbxReadSettings settings = default, bool exceptions = false) => refBufferFile?.GetNode(ref refBuffer, settings, exceptions) ?? refBuffer;

    private CPlugVehiclePhyTunings? tunings;
    [AppliedWithChunk<Chunk090EA003>]
    public CPlugVehiclePhyTunings? Tunings { get => tuningsFile?.GetNode(ref tunings) ?? tunings; set => tunings = value; }
    private Components.GbxRefTableFile? tuningsFile;
    public Components.GbxRefTableFile? TuningsFile { get => tuningsFile; set => tuningsFile = value; }
    public CPlugVehiclePhyTunings? GetTunings(GbxReadSettings settings = default, bool exceptions = false) => tuningsFile?.GetNode(ref tunings, settings, exceptions) ?? tunings;

    private CPlugVehicleCarPhyShape? phyShape;
    [AppliedWithChunk<Chunk090EA008>]
    public CPlugVehicleCarPhyShape? PhyShape { get => phyShape; set => phyShape = value; }

    private OccupantSlot[]? occupantSlots;
    [AppliedWithChunk<Chunk090EA008>]
    public OccupantSlot[]? OccupantSlots { get => occupantSlots; set => occupantSlots = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugVehiclePhyModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVehiclePhyModel() { }


    /// <summary>
    /// CPlugVehiclePhyModel 0x002 chunk
    /// </summary>
    [Chunk(0x090EA002)]
    public partial class Chunk090EA002 : Chunk<CPlugVehiclePhyModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090EA002;


        public override void ReadWrite(CPlugVehiclePhyModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwRefBuffer>(ref n.refBuffer, ref n.refBufferFile);
        }
    }

    /// <summary>
    /// CPlugVehiclePhyModel 0x003 chunk
    /// </summary>
    [Chunk(0x090EA003)]
    public partial class Chunk090EA003 : Chunk<CPlugVehiclePhyModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090EA003;


        public override void ReadWrite(CPlugVehiclePhyModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugVehiclePhyTunings>(ref n.tunings, ref n.tuningsFile);
        }
    }

    /// <summary>
    /// CPlugVehiclePhyModel 0x008 chunk
    /// </summary>
    [Chunk(0x090EA008)]
    public partial class Chunk090EA008 : Chunk<CPlugVehiclePhyModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090EA008;

        public int Version { get; set; }

        public Vec3[]? U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CPlugVehiclePhyModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugVehicleCarPhyShape>(ref n.phyShape);
            if (Version >= 1)
            {
                rw.ArrayReadableWritable<OccupantSlot>(ref n.occupantSlots!, version: Version);
            }
            if (Version >= 6)
            {
                rw.Array<Vec3>(ref U01!);
                if (Version >= 10)
                {
                    if (Version <= 13)
                    {
                        rw.Int32(ref U02);
                    }
                    if (Version >= 12)
                    {
                        rw.Int32(ref U03);
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugVehiclePhyModel 0x009 chunk
    /// </summary>
    [Chunk(0x090EA009)]
    public partial class Chunk090EA009 : Chunk<CPlugVehiclePhyModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090EA009;

        public int Version { get; set; }

        public CMwRefBuffer? U01;
        public CMwRefBuffer? U02;
        public Components.GbxRefTableFile? U02File;

        public override void ReadWrite(CPlugVehiclePhyModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 1)
            {
                rw.NodeRef<CMwRefBuffer>(ref U01);
                rw.NodeRef<CMwRefBuffer>(ref U02, ref U02File);
            }
        }
    }


    public sealed partial class OccupantSlot : IReadableWritable
    {

        private byte u01;
        public byte U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private Vec3 u03;
        public Vec3 U03 { get => u03; set => u03 = value; }

        private bool u04;
        public bool U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        private int u07;
        public int U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        private bool u09;
        public bool U09 { get => u09; set => u09 = value; }

        private byte u10;
        public byte U10 { get => u10; set => u10 = value; }

        private bool u11;
        public bool U11 { get => u11; set => u11 = value; }

        private float u12;
        public float U12 { get => u12; set => u12 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Byte(ref u01);
            if (v >= 2)
            {
                rw.Id(ref u02);
                if (v >= 3)
                {
                    rw.Vec3(ref u03);
                    if (v >= 4)
                    {
                        rw.Boolean(ref u04);
                        if (U04)
                        {
                            rw.Int32(ref u05);
                            rw.Single(ref u06);
                            rw.Int32(ref u07);
                            rw.Int32(ref u08);
                            if (v >= 5)
                            {
                                rw.Boolean(ref u09);
                                if (v >= 7)
                                {
                                    rw.Byte(ref u10);
                                    if (v >= 8)
                                    {
                                        rw.Boolean(ref u11);
                                        rw.Single(ref u12);
                                    }
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
        0x090EA002 => new Chunk090EA002(),
        0x090EA003 => new Chunk090EA003(),
        0x090EA008 => new Chunk090EA008(),
        0x090EA009 => new Chunk090EA009(),
        _ => base.NewChunk(chunkId),
    };
}
