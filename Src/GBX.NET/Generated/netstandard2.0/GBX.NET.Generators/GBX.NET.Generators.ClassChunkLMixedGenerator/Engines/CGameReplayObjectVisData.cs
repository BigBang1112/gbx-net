namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x031A8000</remarks>
[Class(0x031A8000)]
public partial class CGameReplayObjectVisData : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x031A8000;




    private List<ObjectVisKey>? keys;
    [AppliedWithChunk<Chunk031A8000>]
    public List<ObjectVisKey>? Keys { get => keys; set => keys = value; }

    private List<ObjectVis>? visList;
    [AppliedWithChunk<Chunk031A8000>]
    public List<ObjectVis>? VisList { get => visList; set => visList = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameReplayObjectVisData"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameReplayObjectVisData() { }


    /// <summary>
    /// CGameReplayObjectVisData 0x000 chunk
    /// </summary>
    [Chunk(0x031A8000)]
    public partial class Chunk031A8000 : Chunk<CGameReplayObjectVisData>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x031A8000;

        public int Version { get; set; }


        public override void ReadWrite(CGameReplayObjectVisData n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListReadableWritable<ObjectVisKey>(ref n.keys!);
            rw.ListReadableWritable<ObjectVis>(ref n.visList!);
        }
    }


    public sealed partial class ObjectVis : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
        }
    }

    public sealed partial class ObjectVisKey : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        private int u05;
        public int U05 { get => u05; set => u05 = value; }

        private int u06;
        public int U06 { get => u06; set => u06 = value; }

        private int u07;
        public int U07 { get => u07; set => u07 = value; }

        private int u08;
        public int U08 { get => u08; set => u08 = value; }

        private int u09;
        public int U09 { get => u09; set => u09 = value; }

        private int u10;
        public int U10 { get => u10; set => u10 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
            rw.Int32(ref u06);
            rw.Int32(ref u07);
            rw.Int32(ref u08);
            rw.Int32(ref u09);
            rw.Int32(ref u10);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x031A8000 => new Chunk031A8000(),
        _ => base.NewChunk(chunkId),
    };
}
