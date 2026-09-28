namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05036000</remarks>
[Class(0x05036000)]
public partial class CFuncEnvelope : CFunc, IClass
{
    [Hexadecimal] public static new uint Id => 0x05036000;




    private float keyFrameValue0;
    [AppliedWithChunk<Chunk05036000>]
    public float KeyFrameValue0 { get => keyFrameValue0; set => keyFrameValue0 = value; }

    private float keyFrameValue1;
    [AppliedWithChunk<Chunk05036000>]
    public float KeyFrameValue1 { get => keyFrameValue1; set => keyFrameValue1 = value; }

    private float keyFrameValue2;
    [AppliedWithChunk<Chunk05036000>]
    public float KeyFrameValue2 { get => keyFrameValue2; set => keyFrameValue2 = value; }

    private float keyFrameValue3;
    [AppliedWithChunk<Chunk05036000>]
    public float KeyFrameValue3 { get => keyFrameValue3; set => keyFrameValue3 = value; }

    private float keyFramePos1;
    [AppliedWithChunk<Chunk05036000>]
    [AppliedWithChunk<Chunk05036000>]
    public float KeyFramePos1 { get => keyFramePos1; set => keyFramePos1 = value; }

    private float frequency;
    [AppliedWithChunk<Chunk05036000>]
    public float Frequency { get => frequency; set => frequency = value; }

    private float amplitude;
    [AppliedWithChunk<Chunk05036000>]
    public float Amplitude { get => amplitude; set => amplitude = value; }

    private EModulation modFunc;
    [AppliedWithChunk<Chunk05036000>]
    public EModulation ModFunc { get => modFunc; set => modFunc = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CFuncEnvelope"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CFuncEnvelope() { }


    /// <summary>
    /// CFuncEnvelope 0x000 chunk
    /// </summary>
    [Chunk(0x05036000)]
    public partial class Chunk05036000 : Chunk<CFuncEnvelope>
    {
        /// <inheritdoc />
        public override uint Id => 0x05036000;


        public override void ReadWrite(CFuncEnvelope n, GbxReaderWriter rw)
        {
            rw.Single(ref n.keyFrameValue0);
            rw.Single(ref n.keyFrameValue1);
            rw.Single(ref n.keyFrameValue2);
            rw.Single(ref n.keyFrameValue3);
            rw.Single(ref n.keyFramePos1);
            rw.Single(ref n.keyFramePos1);
            rw.Single(ref n.frequency);
            rw.Single(ref n.amplitude);
            rw.EnumInt32<EModulation>(ref n.modFunc);
        }
    }



    public enum EModulation
    {
        Cos,
        Sin,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x05036000 => new Chunk05036000(),
        _ => base.NewChunk(chunkId),
    };
}
