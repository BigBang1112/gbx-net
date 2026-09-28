namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x0500D000</remarks>
[Class(0x0500D000)]
public partial class CFuncTreeTranslate : CFuncTree, IClass
{
    [Hexadecimal] public static new uint Id => 0x0500D000;




    private Vec3 startPoint;
    [AppliedWithChunk<Chunk0500D000>]
    public Vec3 StartPoint { get => startPoint; set => startPoint = value; }

    private Vec3 endPoint;
    [AppliedWithChunk<Chunk0500D000>]
    public Vec3 EndPoint { get => endPoint; set => endPoint = value; }

    private EFlags flags;
    [AppliedWithChunk<Chunk0500D001>]
    public EFlags Flags { get => flags; set => flags = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncTreeTranslate"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncTreeTranslate() { }


    /// <summary>
    /// CFuncTreeTranslate 0x000 chunk
    /// </summary>
    [Chunk(0x0500D000)]
    public partial class Chunk0500D000 : Chunk<CFuncTreeTranslate>
    {
        /// <inheritdoc />
        public override uint Id => 0x0500D000;


        public override void ReadWrite(CFuncTreeTranslate n, GbxReaderWriter rw)
        {
            rw.Vec3(ref n.startPoint);
            rw.Vec3(ref n.endPoint);
        }
    }

    /// <summary>
    /// CFuncTreeTranslate 0x001 chunk
    /// </summary>
    [Chunk(0x0500D001)]
    public partial class Chunk0500D001 : Chunk<CFuncTreeTranslate>
    {
        /// <inheritdoc />
        public override uint Id => 0x0500D001;


        public override void ReadWrite(CFuncTreeTranslate n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EFlags>(ref n.flags);
        }
    }



    public enum EFlags
    {
        None,
        PingPong,
        Smooth,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0500D000 => new Chunk0500D000(),
        0x0500D001 => new Chunk0500D001(),
        _ => base.NewChunk(chunkId),
    };
}
