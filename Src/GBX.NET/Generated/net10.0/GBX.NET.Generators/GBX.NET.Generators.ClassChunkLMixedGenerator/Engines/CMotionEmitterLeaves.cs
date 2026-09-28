namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x0804C000</remarks>
[Class(0x0804C000)]
public partial class CMotionEmitterLeaves : CMotionManaged, IClass
{
    [Hexadecimal] public static new uint Id => 0x0804C000;




    private CMotionManagerLeaves? managerModel;
    [AppliedWithChunk<Chunk0804C001>]
    public CMotionManagerLeaves? ManagerModel { get => managerModelFile?.GetNode(ref managerModel) ?? managerModel; set => managerModel = value; }
    private Components.GbxRefTableFile? managerModelFile;
    public Components.GbxRefTableFile? ManagerModelFile { get => managerModelFile; set => managerModelFile = value; }
    public CMotionManagerLeaves? GetManagerModel(GbxReadSettings settings = default, bool exceptions = false) => managerModelFile?.GetNode(ref managerModel, settings, exceptions) ?? managerModel;

    private Vec3 pos;
    [AppliedWithChunk<Chunk0804C001>]
    public Vec3 Pos { get => pos; set => pos = value; }

    private Vec3 radius;
    [AppliedWithChunk<Chunk0804C001>]
    public Vec3 Radius { get => radius; set => radius = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CMotionEmitterLeaves"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMotionEmitterLeaves() { }


    /// <summary>
    /// CMotionEmitterLeaves 0x001 chunk
    /// </summary>
    [Chunk(0x0804C001)]
    public partial class Chunk0804C001 : Chunk<CMotionEmitterLeaves>
    {
        /// <inheritdoc />
        public override uint Id => 0x0804C001;


        public override void ReadWrite(CMotionEmitterLeaves n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMotionManagerLeaves>(ref n.managerModel, ref n.managerModelFile);
            rw.Vec3(ref n.pos);
            rw.Vec3(ref n.radius);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0804C001 => new Chunk0804C001(),
        _ => base.NewChunk(chunkId),
    };
}
