namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03161000</remarks>
[Class(0x03161000)]
public partial class CGameCtnMediaBlockCameraEffectScript : CGameCtnMediaBlockCameraEffect, IClass, CGameCtnMediaBlock.IHasTwoKeys, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03161000;

    TimeSingle IHasTwoKeys.Start { get => Start.GetValueOrDefault(); set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End.GetValueOrDefault(); set => End = value; }
    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private string script = string.Empty;
    [AppliedWithChunk<Chunk03161000>]
    public string Script { get => script; set => script = value; }

    private TimeSingle? start;
    [AppliedWithChunk<Chunk03161000>]
    public TimeSingle? Start { get => start; set => start = value; }

    private TimeSingle? end;
    [AppliedWithChunk<Chunk03161000>]
    public TimeSingle? End { get => end; set => end = value; }

    private List<Key>? keys;
    [AppliedWithChunk<Chunk03161000>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockCameraEffectScript"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockCameraEffectScript() { }


    /// <summary>
    /// CGameCtnMediaBlockCameraEffectScript 0x000 chunk
    /// </summary>
    [Chunk(0x03161000)]
    public partial class Chunk03161000 : Chunk<CGameCtnMediaBlockCameraEffectScript>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03161000;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockCameraEffectScript n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref n.script);
            if (Version == 0)
            {
                rw.TimeSingleNullable(ref n.start);
                rw.TimeSingleNullable(ref n.end);
            }
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float a;
        public float A { get => a; set => a = value; }

        private float b;
        public float B { get => b; set => b = value; }

        private float c;
        public float C { get => c; set => c = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref a);
            rw.Single(ref b);
            rw.Single(ref c);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03161000 => new Chunk03161000(),
        _ => base.NewChunk(chunkId),
    };
}
