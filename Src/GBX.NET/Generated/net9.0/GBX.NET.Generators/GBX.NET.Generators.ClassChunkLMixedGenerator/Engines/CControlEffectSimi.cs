namespace GBX.NET.Engines.Control;

/// <remarks>ID: 0x07010000</remarks>
[Class(0x07010000)]
public partial class CControlEffectSimi : CControlEffect, IClass
{
    [Hexadecimal] public static new uint Id => 0x07010000;




    private List<Key>? keys;
    [AppliedWithChunk<Chunk07010001>]
    [AppliedWithChunk<Chunk07010002>]
    [AppliedWithChunk<Chunk07010004>]
    [AppliedWithChunk<Chunk07010005>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    private bool centered;
    [AppliedWithChunk<Chunk07010001>]
    [AppliedWithChunk<Chunk07010002>]
    [AppliedWithChunk<Chunk07010004>]
    [AppliedWithChunk<Chunk07010005>]
    public bool Centered { get => centered; set => centered = value; }

    private EColorBlendMode colorBlendMode;
    [AppliedWithChunk<Chunk07010004>]
    [AppliedWithChunk<Chunk07010005>]
    public EColorBlendMode ColorBlendMode { get => colorBlendMode; set => colorBlendMode = value; }

    private bool isContinousEffect;
    [AppliedWithChunk<Chunk07010004>]
    [AppliedWithChunk<Chunk07010005>]
    public bool IsContinousEffect { get => isContinousEffect; set => isContinousEffect = value; }

    private bool isInterpolated;
    [AppliedWithChunk<Chunk07010005>]
    public bool IsInterpolated { get => isInterpolated; set => isInterpolated = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CControlEffectSimi"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CControlEffectSimi() { }


    /// <summary>
    /// CControlEffectSimi 0x001 chunk
    /// </summary>
    [Chunk(0x07010001)]
    public partial class Chunk07010001 : Chunk<CControlEffectSimi>
    {
        /// <inheritdoc />
        public override uint Id => 0x07010001;


        public override void ReadWrite(CControlEffectSimi n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 1);
            rw.Boolean(ref n.centered);
        }
    }

    /// <summary>
    /// CControlEffectSimi 0x002 chunk
    /// </summary>
    [Chunk(0x07010002)]
    public partial class Chunk07010002 : Chunk<CControlEffectSimi>
    {
        /// <inheritdoc />
        public override uint Id => 0x07010002;


        public override void ReadWrite(CControlEffectSimi n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 2);
            rw.Boolean(ref n.centered);
        }
    }

    /// <summary>
    /// CControlEffectSimi 0x004 chunk
    /// </summary>
    [Chunk(0x07010004)]
    public partial class Chunk07010004 : Chunk<CControlEffectSimi>
    {
        /// <inheritdoc />
        public override uint Id => 0x07010004;


        public override void ReadWrite(CControlEffectSimi n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 4);
            rw.Boolean(ref n.centered);
            rw.EnumInt32<EColorBlendMode>(ref n.colorBlendMode);
            rw.Boolean(ref n.isContinousEffect);
        }
    }

    /// <summary>
    /// CControlEffectSimi 0x005 chunk
    /// </summary>
    [Chunk(0x07010005)]
    public partial class Chunk07010005 : Chunk<CControlEffectSimi>
    {
        /// <inheritdoc />
        public override uint Id => 0x07010005;


        public override void ReadWrite(CControlEffectSimi n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!, version: 5);
            rw.Boolean(ref n.centered);
            rw.EnumInt32<EColorBlendMode>(ref n.colorBlendMode);
            rw.Boolean(ref n.isContinousEffect);
            rw.Boolean(ref n.isInterpolated);
        }
    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private Vec2 position;
        public Vec2 Position { get => position; set => position = value; }

        private float rotation;
        public float Rotation { get => rotation; set => rotation = value; }

        private Vec2 scale;
        public Vec2 Scale { get => scale; set => scale = value; }

        private float opacity = 1;
        public float Opacity { get => opacity; set => opacity = value; }

        private float depth = 0.5f;
        public float Depth { get => depth; set => depth = value; }

        private float u01;
        public float U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Vec2(ref position);
            rw.Single(ref rotation);
            rw.Vec2(ref scale);
            if (v >= 1)
            {
                rw.Single(ref opacity);
                if (v >= 2)
                {
                    rw.Single(ref depth);
                    if (v >= 3)
                    {
                        rw.Single(ref u01);
                        rw.Single(ref u02);
                        rw.Single(ref u03);
                        rw.Single(ref u04);
                    }
                }
            }
        }
    }


    public enum EColorBlendMode
    {
        Set,
        Mult,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x07010001 => new Chunk07010001(),
        0x07010002 => new Chunk07010002(),
        0x07010004 => new Chunk07010004(),
        0x07010005 => new Chunk07010005(),
        _ => base.NewChunk(chunkId),
    };
}
