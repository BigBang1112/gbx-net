namespace GBX.NET.Engines.Plug;

[Node(0x90EA000)]
public class CPlugVehiclePhyModel : CMwNod
{
    private CMwRefBuffer? refBuffer;
    private GameBoxRefTable.File? refBufferFile;
    private CPlugVehiclePhyTunings? tunings;
    private GameBoxRefTable.File? tuningsFile;
    private CPlugVehicleCarPhyShape? phyShape;
    private OccupantSlot[]? occupantSlots;

    [NodeMember]
    [AppliedWithChunk<Chunk090EA002>]
    public CMwRefBuffer? RefBuffer
    {
        get => refBuffer = GetNodeFromRefTable(refBuffer, refBufferFile) as CMwRefBuffer;
        set => refBuffer = value;
    }

    [NodeMember]
    [AppliedWithChunk<Chunk090EA003>]
    public CPlugVehiclePhyTunings? Tunings
    {
        get => tunings = GetNodeFromRefTable(tunings, tuningsFile) as CPlugVehiclePhyTunings;
        set => tunings = value;
    }

    [NodeMember]
    [AppliedWithChunk<Chunk090EA008>]
    public CPlugVehicleCarPhyShape? PhyShape { get => phyShape; set => phyShape = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090EA008>]
    public OccupantSlot[]? OccupantSlots { get => occupantSlots; set => occupantSlots = value; }

    internal CPlugVehiclePhyModel()
    {
    }

    #region 0x002 chunk

    /// <summary>
    /// CPlugVehiclePhyModel 0x002 chunk
    /// </summary>
    [Chunk(0x090EA002)]
    public class Chunk090EA002 : Chunk<CPlugVehiclePhyModel>
    {
        public override void ReadWrite(CPlugVehiclePhyModel n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CMwRefBuffer>(ref n.refBuffer, ref n.refBufferFile);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CPlugVehiclePhyModel 0x003 chunk
    /// </summary>
    [Chunk(0x090EA003)]
    public class Chunk090EA003 : Chunk<CPlugVehiclePhyModel>
    {
        public override void ReadWrite(CPlugVehiclePhyModel n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugVehiclePhyTunings>(ref n.tunings, ref n.tuningsFile);
        }
    }

    #endregion

    #region 0x008 chunk

    /// <summary>
    /// CPlugVehiclePhyModel 0x008 chunk
    /// </summary>
    [Chunk(0x090EA008)]
    public class Chunk090EA008 : Chunk<CPlugVehiclePhyModel>, IVersionable
    {
        public Vec3[]? U01;

        public int Version { get; set; } = 8;

        public override void ReadWrite(CPlugVehiclePhyModel n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            rw.NodeRef<CPlugVehicleCarPhyShape>(ref n.phyShape);

            if (Version >= 1)
            {
                rw.ArrayArchive<OccupantSlot>(ref n.occupantSlots, Version);
            }

            if (Version >= 6)
            {
                rw.Array<Vec3>(ref U01);

                if (U01?.Length > 4)
                {
                    throw new Exception("Invalid array length");
                }
            }
        }
    }

    #endregion

    public class OccupantSlot : IReadableWritable
    {
        private byte u01;
        private string? u02;
        private Vec3? u03;
        private bool? u04;
        private int? u05;
        private float? u06;
        private int? u07;
        private int? u08;
        private bool? u09;
        private byte? u10;
        private bool? u11;
        private float? u12;

        public byte U01 { get => u01; set => u01 = value; }
        public string? U02 { get => u02; set => u02 = value; }
        public Vec3? U03 { get => u03; set => u03 = value; }
        public bool? U04 { get => u04; set => u04 = value; }
        public int? U05 { get => u05; set => u05 = value; }
        public float? U06 { get => u06; set => u06 = value; }
        public int? U07 { get => u07; set => u07 = value; }
        public int? U08 { get => u08; set => u08 = value; }
        public bool? U09 { get => u09; set => u09 = value; }
        public byte? U10 { get => u10; set => u10 = value; }
        public bool? U11 { get => u11; set => u11 = value; }
        public float? U12 { get => u12; set => u12 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Byte(ref u01);
            
            if (version >= 2)
            {
                rw.Id(ref u02);
                
                if (version >= 3)
                {
                    rw.Vec3(ref u03);

                    if (version >= 4)
                    {
                        rw.Boolean(ref u04);

                        if (u04.GetValueOrDefault())
                        {
                            rw.Int32(ref u05);
                            rw.Single(ref u06);
                            rw.Int32(ref u07);
                            rw.Int32(ref u08);

                            if (version >= 5)
                            {
                                rw.Boolean(ref u09);
                            }

                            if (version >= 7)
                            {
                                rw.Byte(ref u10);

                                if (version >= 8)
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
