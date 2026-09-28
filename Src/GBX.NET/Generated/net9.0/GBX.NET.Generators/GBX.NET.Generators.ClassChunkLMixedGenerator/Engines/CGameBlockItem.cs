namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E025000</remarks>
[Class(0x2E025000)]
public partial class CGameBlockItem : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E025000;





    /// <summary>
    /// CGameBlockItem 0x000 chunk
    /// </summary>
    [Chunk(0x2E025000)]
    public partial class Chunk2E025000 : Chunk<CGameBlockItem>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E025000;

    }


    public sealed partial class MobilProperties : IReadableWritable
    {

        private byte flags;
        public byte Flags { get => flags; set => flags = value; }

        private CPlugStaticObjectModel? staticObject;
        public CPlugStaticObjectModel? StaticObject { get => staticObject; set => staticObject = value; }

        private CPlugSurface? shape;
        public CPlugSurface? Shape { get => shape; set => shape = value; }

        private BoxAligned u01;
        public BoxAligned U01 { get => u01; set => u01 = value; }

        private Vec3 u02;
        public Vec3 U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Byte(ref flags);
            if ((Flags&1)!=0)
            {
                rw.NodeRef<CPlugStaticObjectModel>(ref staticObject);
            }
            if ((Flags&2)!=0)
            {
                rw.NodeRef<CPlugSurface>(ref shape);
            }
            if ((Flags&4)!=0)
            {
                rw.BoxAligned(ref u01);
            }
            if ((Flags&8)!=0)
            {
                rw.Vec3(ref u02);
            }
        }
    }

    public sealed partial class Mobil : IReadableWritable
    {

        private int id;
        public int Id { get => id; set => id = value; }

        private CPlugCrystal? crystal;
        public CPlugCrystal? Crystal { get => crystal; set => crystal = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref id);
            rw.NodeRef<CPlugCrystal>(ref crystal);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E025000 => new Chunk2E025000(),
        _ => base.NewChunk(chunkId),
    };
}
