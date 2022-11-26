namespace GBX.NET.Engines.Plug;

[Node(0x09088000)]
public class CPlugBitmapRenderCubeMap : CPlugBitmapRender
{
    private int cubeFaceCount;
    private float nearZ;
    private float farZ;
    private float minDistToUpdate;

    internal CPlugBitmapRenderCubeMap()
	{
        
	}

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09088002>]
    public int CubeFaceCount { get => cubeFaceCount; set => cubeFaceCount = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09088002>]
    public float NearZ { get => nearZ; set => nearZ = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09088002>]
    public float FarZ { get => farZ; set => farZ = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09088002>]
    public float MinDistToUpdate { get => minDistToUpdate; set => minDistToUpdate = value; }

    #region 0x001 chunk

    /// <summary>
    /// CPlugBitmapRenderCubeMap 0x001 chunk
    /// </summary>
    [Chunk(0x09088001)]
    public class Chunk09088001 : Chunk<CPlugBitmapRenderCubeMap>
    {
        public uint U01;

        public override void ReadWrite(CPlugBitmapRenderCubeMap n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01); // DoData
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CPlugBitmapRenderCubeMap 0x002 chunk
    /// </summary>
    [Chunk(0x09088002)]
    public class Chunk09088002 : Chunk<CPlugBitmapRenderCubeMap>
    {
        public override void ReadWrite(CPlugBitmapRenderCubeMap n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.cubeFaceCount);
            rw.Single(ref n.nearZ);
            rw.Single(ref n.farZ);
            rw.Single(ref n.minDistToUpdate);
        }
    }

    #endregion
}
