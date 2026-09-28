namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09088000</remarks>
[Class(0x09088000)]
public partial class CPlugBitmapRenderCubeMap : CPlugBitmapRender, IClass
{
    [Hexadecimal] public static new uint Id => 0x09088000;




    private int cubeFaceCount;
    [AppliedWithChunk<Chunk09088002>]
    public int CubeFaceCount { get => cubeFaceCount; set => cubeFaceCount = value; }

    private float nearZ;
    [AppliedWithChunk<Chunk09088002>]
    public float NearZ { get => nearZ; set => nearZ = value; }

    private float farZ;
    [AppliedWithChunk<Chunk09088002>]
    public float FarZ { get => farZ; set => farZ = value; }

    private float minDistToUpdate;
    [AppliedWithChunk<Chunk09088002>]
    public float MinDistToUpdate { get => minDistToUpdate; set => minDistToUpdate = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugBitmapRenderCubeMap"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugBitmapRenderCubeMap() { }


    /// <summary>
    /// CPlugBitmapRenderCubeMap 0x001 chunk
    /// </summary>
    [Chunk(0x09088001)]
    public partial class Chunk09088001 : Chunk<CPlugBitmapRenderCubeMap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09088001;

        public uint U01;

        public override void ReadWrite(CPlugBitmapRenderCubeMap n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }

    /// <summary>
    /// CPlugBitmapRenderCubeMap 0x002 chunk
    /// </summary>
    [Chunk(0x09088002)]
    public partial class Chunk09088002 : Chunk<CPlugBitmapRenderCubeMap>
    {
        /// <inheritdoc />
        public override uint Id => 0x09088002;


        public override void ReadWrite(CPlugBitmapRenderCubeMap n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.cubeFaceCount);
            rw.Single(ref n.nearZ);
            rw.Single(ref n.farZ);
            rw.Single(ref n.minDistToUpdate);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09088001 => new Chunk09088001(),
        0x09088002 => new Chunk09088002(),
        _ => base.NewChunk(chunkId),
    };
}
