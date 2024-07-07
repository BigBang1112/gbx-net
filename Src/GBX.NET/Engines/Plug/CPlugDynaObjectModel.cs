namespace GBX.NET.Engines.Plug;

public partial class CPlugDynaObjectModel : IVersionable
{
	private bool isStatic;
	private bool dynamizeOnSpawn;
	private CPlugSolid2Model? mesh;
	private CPlugSurface? dynaShape;
    private CPlugSurface? staticShape;
    private float breakSpeedKmh;
    private float mass;
	private float lightAliveDurationScMin;
	private float lightAliveDurationScMax;
	private int u01;
	private int u02;
	private byte u03;
	private byte u04;
	private int u05;
	private int u06;
	private byte u07;
	private int u08;
	private int u09;
	private CPlugAnimLocSimple? locAnim;
	private int u10;
	private bool locAnimIsPhysical;
	private CPlugDynaWaterModel? waterModel;

    public int Version { get; set; }

	public bool IsStatic { get => isStatic; set => isStatic = value; }
	public bool DynamizeOnSpawn { get => dynamizeOnSpawn; set => dynamizeOnSpawn = value; }
	public CPlugSolid2Model? Mesh { get => mesh; set => mesh = value; }
	public CPlugSurface? DynaShape { get => dynaShape; set => dynaShape = value; }
    public CPlugSurface? StaticShape { get => staticShape; set => staticShape = value; }
    public float BreakSpeedKmh { get => breakSpeedKmh; set => breakSpeedKmh = value; }
    public float Mass { get => mass; set => mass = value; }
	public float LightAliveDurationScMin { get => lightAliveDurationScMin; set => lightAliveDurationScMin = value; }
	public float LightAliveDurationScMax { get => lightAliveDurationScMax; set => lightAliveDurationScMax = value; }
	public int U01 { get => u01; set => u01 = value; }
	public int U02 { get => u02; set => u02 = value; }
	public byte U03 { get => u03; set => u03 = value; }
	public byte U04 { get => u04; set => u04 = value; }
	public int U05 { get => u05; set => u05 = value; }
	public int U06 { get => u06; set => u06 = value; }
	public byte U07 { get => u07; set => u07 = value; }
	public int U08 { get => u08; set => u08 = value; }
	public int U09 { get => u09; set => u09 = value; }
	public CPlugAnimLocSimple? LocAnim { get => locAnim; set => locAnim = value; }
	public int U10 { get => u10; set => u10 = value; }
	public bool LocAnimIsPhysical { get => locAnimIsPhysical; set => locAnimIsPhysical = value; }
	public CPlugDynaWaterModel? WaterModel { get => waterModel; set => waterModel = value; }

#if NET8_0_OR_GREATER
    static void IClass.Read<T>(T node, GbxReaderWriter rw)
    {
        node.ReadWrite(rw);
    }
#endif

	public override void ReadWrite(GbxReaderWriter rw)
	{
		rw.VersionInt32(this);
		rw.Boolean(ref isStatic);
		rw.Boolean(ref dynamizeOnSpawn);
		rw.NodeRef<CPlugSolid2Model>(ref mesh);
		rw.NodeRef<CPlugSurface>(ref dynaShape);
		rw.NodeRef<CPlugSurface>(ref staticShape);
		rw.Single(ref breakSpeedKmh);
        rw.Single(ref mass);
        rw.Single(ref lightAliveDurationScMin);
        rw.Single(ref lightAliveDurationScMax);
		rw.Int32(ref u01);
        rw.Int32(ref u02);
        rw.Byte(ref u03);
        rw.Byte(ref u04);
        rw.Int32(ref u05);
        rw.Int32(ref u06);
		rw.Byte(ref u07);
		rw.Int32(ref u08);
		rw.Int32(ref u09);
		rw.NodeRef(ref locAnim);
		rw.Int32(ref u10);
		rw.Boolean(ref locAnimIsPhysical);
		rw.NodeRef<CPlugDynaWaterModel>(ref waterModel);
    }
}
