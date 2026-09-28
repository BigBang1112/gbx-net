namespace GBX.NET.Engines.Plug;

/// <summary>
/// Entity data in a timeline.
/// </summary>
/// <remarks>ID: 0x0911F000</remarks>
[Class(0x0911F000)]
public partial class CPlugEntRecordData : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0911F000;





    /// <summary>
    /// CPlugEntRecordData 0x000 chunk
    /// </summary>
    [Chunk(0x0911F000)]
    public partial class Chunk0911F000 : Chunk<CPlugEntRecordData>
    {
        /// <inheritdoc />
        public override uint Id => 0x0911F000;

    }


    public sealed partial class EntRecordDesc : IReadableWritable
    {

        private uint classId;
        public uint ClassId { get => classId; set => classId = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private byte[]? u04;
        public byte[]? U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.UInt32(ref classId);
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Data(ref u04);
            rw.Int32(ref u05);
        }
    }

    public sealed partial class NoticeRecordDesc : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private uint? classId;
        public uint? ClassId { get => classId; set => classId = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            if (v >= 4)
            {
                rw.UInt32(ref classId);
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0911F000 => new Chunk0911F000(),
        _ => base.NewChunk(chunkId),
    };
}
