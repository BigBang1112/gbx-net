namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x08037000</remarks>
[Class(0x08037000)]
public partial class CMotionSkel : CMotionSkelSimple, IClass
{
    [Hexadecimal] public static new uint Id => 0x08037000;




    private CFuncKeysSkel? keysSkel;
    [AppliedWithChunk<Chunk08037000>]
    public CFuncKeysSkel? KeysSkel { get => keysSkel; set => keysSkel = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CMotionSkel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMotionSkel() { }


    /// <summary>
    /// CMotionSkel 0x000 chunk
    /// </summary>
    [Chunk(0x08037000)]
    public partial class Chunk08037000 : Chunk<CMotionSkel>
    {
        /// <inheritdoc />
        public override uint Id => 0x08037000;


        public override void ReadWrite(CMotionSkel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncKeysSkel>(ref n.keysSkel);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x08037000 => new Chunk08037000(),
        _ => base.NewChunk(chunkId),
    };
}
