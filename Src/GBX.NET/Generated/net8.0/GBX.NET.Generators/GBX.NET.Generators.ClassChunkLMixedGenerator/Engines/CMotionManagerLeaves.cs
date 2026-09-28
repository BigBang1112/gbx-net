namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x0804D000</remarks>
[Class(0x0804D000)]
public partial class CMotionManagerLeaves : CMotionManager, IClass
{
    [Hexadecimal] public static new uint Id => 0x0804D000;




    private CSceneMobilLeaves? mobilLeaves;
    [AppliedWithChunk<Chunk0804C000>]
    public CSceneMobilLeaves? MobilLeaves { get => mobilLeaves; set => mobilLeaves = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CMotionManagerLeaves"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMotionManagerLeaves() { }


    /// <summary>
    /// CMotionManagerLeaves 0x000 chunk
    /// </summary>
    [Chunk(0x0804C000)]
    public partial class Chunk0804C000 : Chunk<CMotionManagerLeaves>
    {
        /// <inheritdoc />
        public override uint Id => 0x0804C000;


        public override void ReadWrite(CMotionManagerLeaves n, GbxReaderWriter rw)
        {
            rw.NodeRef<CSceneMobilLeaves>(ref n.mobilLeaves);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0804C000 => new Chunk0804C000(),
        _ => base.NewChunk(chunkId),
    };
}
