namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09021000</remarks>
[Class(0x09021000)]
public partial class CPlugBitmapRenderLightFromMap : CPlugBitmapRender, IClass
{
    [Hexadecimal] public static new uint Id => 0x09021000;




    private int objectCountPerAxisMin;
    [AppliedWithChunk<Chunk09021001>]
    public int ObjectCountPerAxisMin { get => objectCountPerAxisMin; set => objectCountPerAxisMin = value; }

    private int objectCountPerAxisMax;
    [AppliedWithChunk<Chunk09021001>]
    public int ObjectCountPerAxisMax { get => objectCountPerAxisMax; set => objectCountPerAxisMax = value; }

    private float cameraNearZ_FactorInObject;
    [AppliedWithChunk<Chunk09021001>]
    public float CameraNearZ_FactorInObject { get => cameraNearZ_FactorInObject; set => cameraNearZ_FactorInObject = value; }

    private float cameraFarZ_ToAdd;
    [AppliedWithChunk<Chunk09021001>]
    public float CameraFarZ_ToAdd { get => cameraFarZ_ToAdd; set => cameraFarZ_ToAdd = value; }

    private float remapMin_Night;
    [AppliedWithChunk<Chunk09021001>]
    public float RemapMin_Night { get => remapMin_Night; set => remapMin_Night = value; }

    private float remapMax_Night;
    [AppliedWithChunk<Chunk09021001>]
    public float RemapMax_Night { get => remapMax_Night; set => remapMax_Night = value; }

    private float remapMin_DayAmb;
    [AppliedWithChunk<Chunk09021001>]
    public float RemapMin_DayAmb { get => remapMin_DayAmb; set => remapMin_DayAmb = value; }

    private float remapMax_DayAmb;
    [AppliedWithChunk<Chunk09021001>]
    public float RemapMax_DayAmb { get => remapMax_DayAmb; set => remapMax_DayAmb = value; }

    private float remapMin_DayDir;
    [AppliedWithChunk<Chunk09021001>]
    public float RemapMin_DayDir { get => remapMin_DayDir; set => remapMin_DayDir = value; }

    private float remapMax_DayDir;
    [AppliedWithChunk<Chunk09021001>]
    public float RemapMax_DayDir { get => remapMax_DayDir; set => remapMax_DayDir = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugBitmapRenderLightFromMap"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugBitmapRenderLightFromMap() { }


    /// <summary>
    /// CPlugBitmapRenderLightFromMap 0x001 chunk
    /// </summary>
    [Chunk(0x09021001)]
    public partial class Chunk09021001 : Chunk<CPlugBitmapRenderLightFromMap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09021001;


        public override void ReadWrite(CPlugBitmapRenderLightFromMap n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.objectCountPerAxisMin);
            rw.Int32(ref n.objectCountPerAxisMax);
            rw.Single(ref n.cameraNearZ_FactorInObject);
            rw.Single(ref n.cameraFarZ_ToAdd);
            rw.Single(ref n.remapMin_Night);
            rw.Single(ref n.remapMax_Night);
            rw.Single(ref n.remapMin_DayAmb);
            rw.Single(ref n.remapMax_DayAmb);
            rw.Single(ref n.remapMin_DayDir);
            rw.Single(ref n.remapMax_DayDir);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09021001 => new Chunk09021001(),
        _ => base.NewChunk(chunkId),
    };
}
