namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090BA000</remarks>
[Class(0x090BA000)]
public partial class CPlugSkel : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x090BA000;





    /// <summary>
    /// CPlugSkel 0x000 chunk
    /// </summary>
    [Chunk(0x090BA000)]
    public partial class Chunk090BA000 : Chunk<CPlugSkel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090BA000;

    }


    public sealed partial class Joint : IReadableWritable
    {

        private string? name;
        public string? Name { get => name; set => name = value; }

        private short parentIndex;
        public short ParentIndex { get => parentIndex; set => parentIndex = value; }

        private Quat globalJoint;
        public Quat GlobalJoint { get => globalJoint; set => globalJoint = value; }

        private Vec3 u01;
        public Vec3 U01 { get => u01; set => u01 = value; }

        private Iso4 u02;
        public Iso4 U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref name);
            rw.Int16(ref parentIndex);
            if (v <= 14)
            {
                rw.Quat(ref globalJoint);
                rw.Vec3(ref u01);
            }
            if (v >= 1)
            {
                rw.Iso4(ref u02);
            }
        }
    }

    public sealed partial class JointExpr : IReadableWritable
    {

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
        }
    }

    public sealed partial class Socket : IReadableWritable
    {

        private string? name;
        public string? Name { get => name; set => name = value; }

        private short u01;
        public short U01 { get => u01; set => u01 = value; }

        private Iso4 u02;
        public Iso4 U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref name);
            rw.Int16(ref u01);
            rw.Iso4(ref u02);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090BA000 => new Chunk090BA000(),
        _ => base.NewChunk(chunkId),
    };
}
