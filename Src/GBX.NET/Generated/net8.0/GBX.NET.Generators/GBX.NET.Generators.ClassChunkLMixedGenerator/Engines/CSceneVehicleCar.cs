namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A02B000</remarks>
[Class(0x0A02B000)]
public partial class CSceneVehicleCar : CSceneVehicle, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A02B000;




    private VisualVehicle[]? visualVehicles;
    [AppliedWithChunk<Chunk0A02B009>]
    public VisualVehicle[]? VisualVehicles { get => visualVehicles; set => visualVehicles = value; }


    /// <summary>
    /// CSceneVehicleCar 0x003 chunk
    /// </summary>
    [Chunk(0x0A02B003)]
    public partial class Chunk0A02B003 : Chunk<CSceneVehicleCar>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A02B003;

    }

    /// <summary>
    /// CSceneVehicleCar 0x005 chunk
    /// </summary>
    [Chunk(0x0A02B005)]
    public partial class Chunk0A02B005 : Chunk<CSceneVehicleCar>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A02B005;

        public CMwNod? U01;
        public Vec3 U02;

        public override void ReadWrite(CSceneVehicleCar n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
            rw.Vec3(ref U02);
        }
    }

    /// <summary>
    /// CSceneVehicleCar 0x007 chunk
    /// </summary>
    [Chunk(0x0A02B007)]
    public partial class Chunk0A02B007 : Chunk<CSceneVehicleCar>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A02B007;

        public CMwNod? U01;
        public Vec3 U02;

        public override void ReadWrite(CSceneVehicleCar n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
            rw.Vec3(ref U02);
        }
    }

    /// <summary>
    /// CSceneVehicleCar 0x008 chunk
    /// </summary>
    [Chunk(0x0A02B008)]
    public partial class Chunk0A02B008 : Chunk<CSceneVehicleCar>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A02B008;

    }

    /// <summary>
    /// CSceneVehicleCar 0x009 chunk
    /// </summary>
    [Chunk(0x0A02B009)]
    public partial class Chunk0A02B009 : Chunk<CSceneVehicleCar>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A02B009;


        public override void ReadWrite(CSceneVehicleCar n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<VisualVehicle>(ref n.visualVehicles!);
        }
    }

    /// <summary>
    /// CSceneVehicleCar 0x00B chunk
    /// </summary>
    [Chunk(0x0A02B00B)]
    public partial class Chunk0A02B00B : Chunk<CSceneVehicleCar>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A02B00B;

        public CMwNod? U01;
        public CMwNod? U02;

        public override void ReadWrite(CSceneVehicleCar n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
            rw.NodeRef<CMwNod>(ref U02);
        }
    }

    /// <summary>
    /// CSceneVehicleCar 0x00C chunk
    /// </summary>
    [Chunk(0x0A02B00C)]
    public partial class Chunk0A02B00C : Chunk<CSceneVehicleCar>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A02B00C;

        public float U01;
        public float U02;
        public BoxAligned U03;

        public override void ReadWrite(CSceneVehicleCar n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.BoxAligned(ref U03);
        }
    }

    /// <summary>
    /// CSceneVehicleCar 0x014 chunk
    /// </summary>
    [Chunk(0x0A02B014)]
    public partial class Chunk0A02B014 : Chunk<CSceneVehicleCar>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A02B014;

    }


    public sealed partial class VisualVehicle : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private string? u03;
        public string? U03 { get => u03; set => u03 = value; }

        private string? u04;
        public string? U04 { get => u04; set => u04 = value; }

        private bool u05;
        public bool U05 { get => u05; set => u05 = value; }

        private bool u06;
        public bool U06 { get => u06; set => u06 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.Int32(ref u02);
            rw.Id(ref u03);
            rw.Id(ref u04);
            rw.Boolean(ref u05);
            rw.Boolean(ref u06);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A02B003 => new Chunk0A02B003(),
        0x0A02B005 => new Chunk0A02B005(),
        0x0A02B007 => new Chunk0A02B007(),
        0x0A02B008 => new Chunk0A02B008(),
        0x0A02B009 => new Chunk0A02B009(),
        0x0A02B00B => new Chunk0A02B00B(),
        0x0A02B00C => new Chunk0A02B00C(),
        0x0A02B014 => new Chunk0A02B014(),
        _ => base.NewChunk(chunkId),
    };
}
