namespace GBX.NET.Engines.Plug;

public partial class CPlugCurveSimpleNod
{
    private int version2;
    [AppliedWithChunk<Chunk09185000>]
    public int Version2 { get => version2; set => version2 = value; }

    private int count;
    [AppliedWithChunk<Chunk09185000>]
    public int Count { get => count; set => count = value; }

    public partial class Chunk09185000 : IVersionable
    {
        public int Version { get; set; }

        public byte U01;
        public int U02;
        public short U03;
        public ulong[]? U04;

        public override void ReadWrite(CPlugCurveSimpleNod n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.version2);
            rw.Int32(ref n.count);
            if (n.Version2 == 1)
            {
                rw.Byte(ref U01);
            }
            if (n.Version2 == 2)
            {
                rw.Int32(ref U02);
            }
            if (n.Version2 >= 3)
            {
                rw.Int16(ref U03);
            }
            rw.Array(ref U04, n.count);
        }
    }
}
