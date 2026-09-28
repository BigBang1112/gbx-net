namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09091000</remarks>
[Class(0x09091000)]
public partial class CPlugBitmapRenderSub : CPlugBitmapRender, IClass
{
    [Hexadecimal] public static new uint Id => 0x09091000;




    private CPlugShader? shaderToForce;
    [AppliedWithChunk<Chunk09091000>]
    public CPlugShader? ShaderToForce { get => shaderToForce; set => shaderToForce = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugBitmapRenderSub"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugBitmapRenderSub() { }


    /// <summary>
    /// CPlugBitmapRenderSub 0x000 chunk
    /// </summary>
    [Chunk(0x09091000)]
    public partial class Chunk09091000 : Chunk<CPlugBitmapRenderSub>
    {
        /// <inheritdoc />
        public override uint Id => 0x09091000;


        public override void ReadWrite(CPlugBitmapRenderSub n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugShader>(ref n.shaderToForce);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09091000 => new Chunk09091000(),
        _ => base.NewChunk(chunkId),
    };
}
