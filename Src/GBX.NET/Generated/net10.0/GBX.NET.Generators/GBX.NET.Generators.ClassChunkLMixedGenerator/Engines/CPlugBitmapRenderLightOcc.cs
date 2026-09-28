namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0909F000</remarks>
[Class(0x0909F000)]
public partial class CPlugBitmapRenderLightOcc : CPlugBitmapRender, IClass
{
    [Hexadecimal] public static new uint Id => 0x0909F000;




    private float fovY;
    [AppliedWithChunk<Chunk0909F000>]
    public float FovY { get => fovY; set => fovY = value; }

    private float opacity;
    [AppliedWithChunk<Chunk0909F000>]
    public float Opacity { get => opacity; set => opacity = value; }

    private CPlugBitmap? bitmapToModulate;
    [AppliedWithChunk<Chunk0909F000>]
    public CPlugBitmap? BitmapToModulate { get => bitmapToModulateFile?.GetNode(ref bitmapToModulate) ?? bitmapToModulate; set => bitmapToModulate = value; }
    private Components.GbxRefTableFile? bitmapToModulateFile;
    public Components.GbxRefTableFile? BitmapToModulateFile { get => bitmapToModulateFile; set => bitmapToModulateFile = value; }
    public CPlugBitmap? GetBitmapToModulate(GbxReadSettings settings = default, bool exceptions = false) => bitmapToModulateFile?.GetNode(ref bitmapToModulate, settings, exceptions) ?? bitmapToModulate;

    /// <summary>
    /// Creates a new instance of <see cref="CPlugBitmapRenderLightOcc"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugBitmapRenderLightOcc() { }


    /// <summary>
    /// CPlugBitmapRenderLightOcc 0x000 chunk
    /// </summary>
    [Chunk(0x0909F000)]
    public partial class Chunk0909F000 : Chunk<CPlugBitmapRenderLightOcc>
    {
        /// <inheritdoc />
        public override uint Id => 0x0909F000;


        public override void ReadWrite(CPlugBitmapRenderLightOcc n, GbxReaderWriter rw)
        {
            rw.Single(ref n.fovY);
            rw.Single(ref n.opacity);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapToModulate, ref n.bitmapToModulateFile);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0909F000 => new Chunk0909F000(),
        _ => base.NewChunk(chunkId),
    };
}
