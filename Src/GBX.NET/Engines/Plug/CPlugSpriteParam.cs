namespace GBX.NET.Engines.Plug;

[Node(0x090AC000)]
public class CPlugSpriteParam : CPlug
{
    private Vec3 globalDirection;
    private Vec2 pivotPoint;
    private float globalDirTiltFactor;
    private float textureHeightInWorld;
    private float visibleMaxDistAtFov90;
    private float visibleMinScreenHeight01;

    public Vec3 GlobalDirection { get => globalDirection; set => globalDirection = value; }
    public Vec2 PivotPoint { get => pivotPoint; set => pivotPoint = value; }
    public float GlobalDirTiltFactor { get => globalDirTiltFactor; set => globalDirTiltFactor = value; }
    public float TextureHeightInWorld { get => textureHeightInWorld; set => textureHeightInWorld = value; }
    public float VisibleMaxDistAtFov90 { get => visibleMaxDistAtFov90; set => visibleMaxDistAtFov90 = value; }
    public float VisibleMinScreenHeight01 { get => visibleMinScreenHeight01; set => visibleMinScreenHeight01 = value; }

    internal CPlugSpriteParam()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CPlugSpriteParam 0x000 chunk
    /// </summary>
    [Chunk(0x090AC000)]
    public class Chunk090AC000 : Chunk<CPlugSpriteParam>
    {
        public uint U01;

        public override void ReadWrite(CPlugSpriteParam n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01); // DoData
            rw.Vec3(ref n.globalDirection);
            rw.Vec2(ref n.pivotPoint);
            rw.Single(ref n.globalDirTiltFactor);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CPlugSpriteParam 0x001 chunk
    /// </summary>
    [Chunk(0x090AC001)]
    public class Chunk090AC001 : Chunk090AC000, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugSpriteParam n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            
            base.ReadWrite(n, rw);

            rw.Single(ref n.textureHeightInWorld);

            if (version >= 1)
            {
                rw.Single(ref n.visibleMaxDistAtFov90);
                rw.Single(ref n.visibleMinScreenHeight01);
            }
        }
    }

    #endregion
}
