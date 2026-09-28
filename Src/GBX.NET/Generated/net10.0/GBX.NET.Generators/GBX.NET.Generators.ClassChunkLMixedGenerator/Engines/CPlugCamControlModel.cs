namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0910C000</remarks>
[Class(0x0910C000)]
public partial class CPlugCamControlModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0910C000;




    private CPlugCamShakeModel? shake;
    [AppliedWithChunk<Chunk0910C000>]
    public CPlugCamShakeModel? Shake { get => shake; set => shake = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugCamControlModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugCamControlModel() { }


    /// <summary>
    /// CPlugCamControlModel 0x000 chunk
    /// </summary>
    [Chunk(0x0910C000)]
    public partial class Chunk0910C000 : Chunk<CPlugCamControlModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0910C000;

        public int Version { get; set; }


        public override void ReadWrite(CPlugCamControlModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugCamShakeModel>(ref n.shake);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0910C000 => new Chunk0910C000(),
        _ => base.NewChunk(chunkId),
    };
}
