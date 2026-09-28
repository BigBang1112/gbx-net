namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x08028000</remarks>
[Class(0x08028000)]
public partial class CMotions : CMotion, IClass
{
    [Hexadecimal] public static new uint Id => 0x08028000;




    private List<CMotion>? motions;
    [AppliedWithChunk<Chunk08028001>]
    public List<CMotion>? Motions { get => motions; set => motions = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CMotions"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMotions() { }


    /// <summary>
    /// CMotions 0x001 chunk
    /// </summary>
    [Chunk(0x08028001)]
    public partial class Chunk08028001 : Chunk<CMotions>
    {
        /// <inheritdoc />
        public override uint Id => 0x08028001;


        public override void ReadWrite(CMotions n, GbxReaderWriter rw)
        {
            rw.ListNodeRef<CMotion>(ref n.motions!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x08028001 => new Chunk08028001(),
        _ => base.NewChunk(chunkId),
    };
}
