namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E00E000</remarks>
[Class(0x2E00E000)]
public partial class CGameSpawnModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E00E000;




    private Iso4 loc;
    [AppliedWithChunk<Chunk2E00E000>]
    public Iso4 Loc { get => loc; set => loc = value; }

    private bool underground;
    [AppliedWithChunk<Chunk2E00E000>]
    public bool Underground { get => underground; set => underground = value; }

    private float torqueX;
    [AppliedWithChunk<Chunk2E00E000>]
    public float TorqueX { get => torqueX; set => torqueX = value; }

    private int torqueDuration;
    [AppliedWithChunk<Chunk2E00E000>]
    public int TorqueDuration { get => torqueDuration; set => torqueDuration = value; }

    private Vec3 defaultGravitySpawn;
    [AppliedWithChunk<Chunk2E00E000>]
    public Vec3 DefaultGravitySpawn { get => defaultGravitySpawn; set => defaultGravitySpawn = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameSpawnModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameSpawnModel() { }


    /// <summary>
    /// CGameSpawnModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E00E000)]
    public partial class Chunk2E00E000 : Chunk<CGameSpawnModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00E000;

        public int Version { get; set; }


        public override void ReadWrite(CGameSpawnModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Iso4(ref n.loc);
            rw.Boolean(ref n.underground);
            if (Version >= 1)
            {
                rw.Single(ref n.torqueX);
                if (Version >= 2)
                {
                    rw.Int32(ref n.torqueDuration);
                    if (Version >= 3)
                    {
                        rw.Vec3(ref n.defaultGravitySpawn);
                    }
                }
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E00E000 => new Chunk2E00E000(),
        _ => base.NewChunk(chunkId),
    };
}
