namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03126000</remarks>
[Class(0x03126000)]
public partial class CGameCtnMediaBlockDOF : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03126000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03126000>]
    [AppliedWithChunk<Chunk03126001>]
    [AppliedWithChunk<Chunk03126002>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockDOF"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockDOF() { }


    /// <summary>
    /// CGameCtnMediaBlockDOF 0x000 chunk
    /// </summary>
    [Chunk(0x03126000)]
    public partial class Chunk03126000 : Chunk<CGameCtnMediaBlockDOF>
    {
        /// <inheritdoc />
        public override uint Id => 0x03126000;


        public override void ReadWrite(CGameCtnMediaBlockDOF n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 0);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockDOF 0x001 chunk
    /// </summary>
    [Chunk(0x03126001)]
    public partial class Chunk03126001 : Chunk<CGameCtnMediaBlockDOF>
    {
        /// <inheritdoc />
        public override uint Id => 0x03126001;


        public override void ReadWrite(CGameCtnMediaBlockDOF n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 1);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockDOF 0x002 chunk
    /// </summary>
    [Chunk(0x03126002)]
    public partial class Chunk03126002 : Chunk<CGameCtnMediaBlockDOF>
    {
        /// <inheritdoc />
        public override uint Id => 0x03126002;


        public override void ReadWrite(CGameCtnMediaBlockDOF n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 2);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float zFocus;
        public float ZFocus { get => zFocus; set => zFocus = value; }

        private float lensSize;
        public float LensSize { get => lensSize; set => lensSize = value; }

        private int? target;
        public int? Target { get => target; set => target = value; }

        private Vec3? targetPosition;
        public Vec3? TargetPosition { get => targetPosition; set => targetPosition = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref zFocus);
            rw.Single(ref lensSize);
            if (v >= 1)
            {
                rw.Int32(ref target);
                if (v >= 2)
                {
                    rw.Vec3(ref targetPosition);
                }
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03126000 => new Chunk03126000(),
        0x03126001 => new Chunk03126001(),
        0x03126002 => new Chunk03126002(),
        _ => base.NewChunk(chunkId),
    };
}
