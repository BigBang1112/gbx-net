namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E00C000</remarks>
[Class(0x2E00C000)]
public partial class CGameTeleporterModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E00C000;




    private Iso4 spawnLoc;
    [AppliedWithChunk<Chunk2E00C000>]
    public Iso4 SpawnLoc { get => spawnLoc; set => spawnLoc = value; }

    private CGameSpawnModel? spawn;
    [AppliedWithChunk<Chunk2E00C000>]
    public CGameSpawnModel? Spawn { get => spawn; set => spawn = value; }

    private CPlugSurface? triggerShape;
    [AppliedWithChunk<Chunk2E00C000>]
    public CPlugSurface? TriggerShape { get => triggerShapeFile?.GetNode(ref triggerShape) ?? triggerShape; set => triggerShape = value; }
    private Components.GbxRefTableFile? triggerShapeFile;
    public Components.GbxRefTableFile? TriggerShapeFile { get => triggerShapeFile; set => triggerShapeFile = value; }
    public CPlugSurface? GetTriggerShape(GbxReadSettings settings = default, bool exceptions = false) => triggerShapeFile?.GetNode(ref triggerShape, settings, exceptions) ?? triggerShape;

    private Vec3 centerPos;
    [AppliedWithChunk<Chunk2E00C000>]
    public Vec3 CenterPos { get => centerPos; set => centerPos = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameTeleporterModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameTeleporterModel() { }


    /// <summary>
    /// CGameTeleporterModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E00C000)]
    public partial class Chunk2E00C000 : Chunk<CGameTeleporterModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00C000;

        public int Version { get; set; }


        public override void ReadWrite(CGameTeleporterModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version == 0)
            {
                rw.Iso4(ref n.spawnLoc);
            }
            if (Version >= 1)
            {
                rw.NodeRef<CGameSpawnModel>(ref n.spawn);
            }
            rw.NodeRef<CPlugSurface>(ref n.triggerShape, ref n.triggerShapeFile);
            if (Version >= 2)
            {
                rw.Vec3(ref n.centerPos);
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E00C000 => new Chunk2E00C000(),
        _ => base.NewChunk(chunkId),
    };
}
