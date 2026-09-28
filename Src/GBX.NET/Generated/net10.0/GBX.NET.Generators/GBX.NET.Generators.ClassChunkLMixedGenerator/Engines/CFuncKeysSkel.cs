namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05006000</remarks>
[Class(0x05006000)]
public partial class CFuncKeysSkel : CFuncKeys, IClass
{
    [Hexadecimal] public static new uint Id => 0x05006000;




    private CFuncSkel? skel;
    [AppliedWithChunk<Chunk05006000>]
    public CFuncSkel? Skel { get => skel; set => skel = value; }


    /// <summary>
    /// CFuncKeysSkel 0x000 chunk
    /// </summary>
    [Chunk(0x05006000)]
    public partial class Chunk05006000 : Chunk<CFuncKeysSkel>
    {
        /// <inheritdoc />
        public override uint Id => 0x05006000;


        public override void ReadWrite(CFuncKeysSkel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncSkel>(ref n.skel);
        }
    }

    /// <summary>
    /// CFuncKeysSkel 0x001 chunk
    /// </summary>
    [Chunk(0x05006001)]
    public partial class Chunk05006001 : Chunk<CFuncKeysSkel>
    {
        /// <inheritdoc />
        public override uint Id => 0x05006001;

    }


    public sealed partial class Loc : IReadable, IWritable
    {
        public Quat U01 { get; set; }
        public Vec3 U02 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadQuat();
            U02 = r.ReadVec3();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(U01);
            w.Write(U02);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x05006000 => new Chunk05006000(),
        0x05006001 => new Chunk05006001(),
        _ => base.NewChunk(chunkId),
    };
}
