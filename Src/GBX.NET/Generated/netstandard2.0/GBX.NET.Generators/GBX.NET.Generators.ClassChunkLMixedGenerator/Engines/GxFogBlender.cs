namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04008000</remarks>
[Class(0x04008000)]
public partial class GxFogBlender : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x04008000;




    private List<Key>? keys;
    [AppliedWithChunk<Chunk04008000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="GxFogBlender"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public GxFogBlender() { }


    /// <summary>
    /// GxFogBlender 0x000 chunk
    /// </summary>
    [Chunk(0x04008000)]
    public partial class Chunk04008000 : Chunk<GxFogBlender>
    {
        /// <inheritdoc />
        public override uint Id => 0x04008000;

        public bool U01;
        public int U02;

        public override void ReadWrite(GxFogBlender n, GbxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Int32(ref U02);
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }


    public sealed partial class Key : IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private GxFog? fog;
        public GxFog? Fog { get => fog; set => fog = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.NodeRef<GxFog>(ref fog);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x04008000 => new Chunk04008000(),
        _ => base.NewChunk(chunkId),
    };
}
