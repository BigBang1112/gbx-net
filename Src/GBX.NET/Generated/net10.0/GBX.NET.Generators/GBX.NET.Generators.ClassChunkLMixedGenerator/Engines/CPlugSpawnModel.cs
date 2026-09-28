namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0917A000</remarks>
[Class(0x0917A000)]
public partial class CPlugSpawnModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0917A000;




    private Iso4 loc;
    [AppliedWithChunk<Chunk0917A000>]
    public Iso4 Loc { get => loc; set => loc = value; }

    private float torqueX;
    [AppliedWithChunk<Chunk0917A000>]
    public float TorqueX { get => torqueX; set => torqueX = value; }

    private TimeInt32 torqueDuration;
    [AppliedWithChunk<Chunk0917A000>]
    public TimeInt32 TorqueDuration { get => torqueDuration; set => torqueDuration = value; }

    private Vec3 defaultGravitySpawn;
    [AppliedWithChunk<Chunk0917A000>]
    public Vec3 DefaultGravitySpawn { get => defaultGravitySpawn; set => defaultGravitySpawn = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugSpawnModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugSpawnModel() { }


    /// <summary>
    /// CPlugSpawnModel 0x000 chunk
    /// </summary>
    [Chunk(0x0917A000)]
    public partial class Chunk0917A000 : Chunk<CPlugSpawnModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0917A000;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CPlugSpawnModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Iso4(ref n.loc);
            rw.Single(ref n.torqueX);
            rw.TimeInt32(ref n.torqueDuration);
            rw.Vec3(ref n.defaultGravitySpawn);
            rw.Int32(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0917A000 => new Chunk0917A000(),
        _ => base.NewChunk(chunkId),
    };
}
