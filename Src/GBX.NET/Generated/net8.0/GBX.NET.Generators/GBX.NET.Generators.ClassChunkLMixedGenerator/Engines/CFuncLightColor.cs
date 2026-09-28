namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05019000</remarks>
[Class(0x05019000)]
public partial class CFuncLightColor : CFuncLight, IClass
{
    [Hexadecimal] public static new uint Id => 0x05019000;




    private Vec4 color0;
    [AppliedWithChunk<Chunk05019001>]
    [AppliedWithChunk<Chunk05019002>]
    public Vec4 Color0 { get => color0; set => color0 = value; }

    private Vec4 color1;
    [AppliedWithChunk<Chunk05019001>]
    [AppliedWithChunk<Chunk05019002>]
    public Vec4 Color1 { get => color1; set => color1 = value; }

    private CPlugFileImg? image;
    [AppliedWithChunk<Chunk05019002>]
    public CPlugFileImg? Image { get => imageFile?.GetNode(ref image) ?? image; set => image = value; }
    private Components.GbxRefTableFile? imageFile;
    public Components.GbxRefTableFile? ImageFile { get => imageFile; set => imageFile = value; }
    public CPlugFileImg? GetImage(GbxReadSettings settings = default, bool exceptions = false) => imageFile?.GetNode(ref image, settings, exceptions) ?? image;

    /// <summary>
    /// Creates a new instance of <see cref="CFuncLightColor"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncLightColor() { }


    /// <summary>
    /// CFuncLightColor 0x001 chunk
    /// </summary>
    [Chunk(0x05019001)]
    public partial class Chunk05019001 : Chunk<CFuncLightColor>
    {
        /// <inheritdoc />
        public override uint Id => 0x05019001;


        public override void ReadWrite(CFuncLightColor n, GbxReaderWriter rw)
        {
            rw.Vec4(ref n.color0);
            rw.Vec4(ref n.color1);
        }
    }

    /// <summary>
    /// CFuncLightColor 0x002 chunk
    /// </summary>
    [Chunk(0x05019002)]
    public partial class Chunk05019002 : Chunk05019001
    {
        /// <inheritdoc />
        public override uint Id => 0x05019002;


        public override void ReadWrite(CFuncLightColor n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.NodeRef<CPlugFileImg>(ref n.image, ref n.imageFile);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x05019001 => new Chunk05019001(),
        0x05019002 => new Chunk05019002(),
        _ => base.NewChunk(chunkId),
    };
}
