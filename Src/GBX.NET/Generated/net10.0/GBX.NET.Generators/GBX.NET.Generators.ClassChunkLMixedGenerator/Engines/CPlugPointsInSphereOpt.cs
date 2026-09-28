namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09066000</remarks>
[Class(0x09066000)]
public partial class CPlugPointsInSphereOpt : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x09066000;




    private Pack[]? packs;
    [AppliedWithChunk<Chunk09066000>]
    public Pack[]? Packs { get => packs; set => packs = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugPointsInSphereOpt"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugPointsInSphereOpt() { }


    /// <summary>
    /// CPlugPointsInSphereOpt 0x000 chunk
    /// </summary>
    [Chunk(0x09066000)]
    public partial class Chunk09066000 : Chunk<CPlugPointsInSphereOpt>
    {
        /// <inheritdoc />
        public override uint Id => 0x09066000;

        public Vec3[]? U01;

        public override void ReadWrite(CPlugPointsInSphereOpt n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<Pack>(ref n.packs!);
            rw.Array<Vec3>(ref U01!);
        }
    }


    public sealed partial class Pack : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09066000 => new Chunk09066000(),
        _ => base.NewChunk(chunkId),
    };
}
