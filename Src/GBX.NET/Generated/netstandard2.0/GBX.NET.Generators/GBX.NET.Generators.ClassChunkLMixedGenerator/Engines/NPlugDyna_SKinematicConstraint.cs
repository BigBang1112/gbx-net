namespace GBX.NET.Engines.Meta;

/// <remarks>ID: 0x2F0CA000</remarks>
[Class(0x2F0CA000)]
public partial class NPlugDyna_SKinematicConstraint : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x2F0CA000;




    private int subVersion;
    public int SubVersion { get => subVersion; set => subVersion = value; }

    private AnimFunc? transAnimFunc;
    public AnimFunc? TransAnimFunc { get => transAnimFunc; set => transAnimFunc = value; }

    private AnimFunc? rotAnimFunc;
    public AnimFunc? RotAnimFunc { get => rotAnimFunc; set => rotAnimFunc = value; }

    private EShaderTcType shaderTcType;
    public EShaderTcType ShaderTcType { get => shaderTcType; set => shaderTcType = value; }

    private int shaderTcVersion;
    public int ShaderTcVersion { get => shaderTcVersion; set => shaderTcVersion = value; }

    private AnimFuncNat[]? shaderTcAnimFunc;
    public AnimFuncNat[]? ShaderTcAnimFunc { get => shaderTcAnimFunc; set => shaderTcAnimFunc = value; }

    private TransSubTextureIn? shaderTcDataTransSub;
    public TransSubTextureIn? ShaderTcDataTransSub { get => shaderTcDataTransSub; set => shaderTcDataTransSub = value; }

    private EAxis transAxis;
    public EAxis TransAxis { get => transAxis; set => transAxis = value; }

    private float transMin;
    public float TransMin { get => transMin; set => transMin = value; }

    private float transMax;
    public float TransMax { get => transMax; set => transMax = value; }

    private EAxis rotAxis;
    public EAxis RotAxis { get => rotAxis; set => rotAxis = value; }

    private float angleMinDeg;
    public float AngleMinDeg { get => angleMinDeg; set => angleMinDeg = value; }

    private float angleMaxDeg;
    public float AngleMaxDeg { get => angleMaxDeg; set => angleMaxDeg = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.VersionInt32(this);
        rw.Int32(ref subVersion);
        rw.ReadableWritable<AnimFunc>(ref transAnimFunc);
        rw.ReadableWritable<AnimFunc>(ref rotAnimFunc);
        rw.EnumInt32<EShaderTcType>(ref shaderTcType);
        rw.Int32(ref shaderTcVersion);
        rw.ArrayReadableWritable<AnimFuncNat>(ref shaderTcAnimFunc!);
        if ((int)ShaderTcType==1)
        {
            rw.ReadableWritable<TransSubTextureIn>(ref shaderTcDataTransSub);
        }
        rw.EnumByte<EAxis>(ref transAxis);
        rw.Single(ref transMin);
        rw.Single(ref transMax);
        rw.EnumByte<EAxis>(ref rotAxis);
        rw.Single(ref angleMinDeg);
        rw.Single(ref angleMaxDeg);
    }



    public sealed partial class AnimFuncNat : IReadableWritable
    {

        private TimeInt32 duration;
        public TimeInt32 Duration { get => duration; set => duration = value; }

        private int textureId;
        public int TextureId { get => textureId; set => textureId = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeInt32(ref duration);
            rw.Int32(ref textureId);
        }
    }

    public sealed partial class SubAnimFunc : IReadableWritable
    {

        private AnimEase ease;
        public AnimEase Ease { get => ease; set => ease = value; }

        private bool reverse;
        public bool Reverse { get => reverse; set => reverse = value; }

        private TimeInt32 duration;
        public TimeInt32 Duration { get => duration; set => duration = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.EnumByte<AnimEase>(ref ease);
            rw.Boolean(ref reverse, asByte: true);
            rw.TimeInt32(ref duration);
        }
    }

    public sealed partial class AnimFunc : IReadableWritable
    {

        private bool isDuration;
        public bool IsDuration { get => isDuration; set => isDuration = value; }

        private SubAnimFunc[]? subFuncs;
        public SubAnimFunc[]? SubFuncs { get => subFuncs; set => subFuncs = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Boolean(ref isDuration);
            rw.ArrayReadableWritable<SubAnimFunc>(ref subFuncs!);
        }
    }

    public sealed partial class TransSubTextureIn : IReadableWritable
    {

        private int nbSubTexture;
        public int NbSubTexture { get => nbSubTexture; set => nbSubTexture = value; }

        private int nbSubTexturePerLine;
        public int NbSubTexturePerLine { get => nbSubTexturePerLine; set => nbSubTexturePerLine = value; }

        private int nbSubTexturePerColumn;
        public int NbSubTexturePerColumn { get => nbSubTexturePerColumn; set => nbSubTexturePerColumn = value; }

        private bool topToBottom;
        public bool TopToBottom { get => topToBottom; set => topToBottom = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref nbSubTexture);
            rw.Int32(ref nbSubTexturePerLine);
            rw.Int32(ref nbSubTexturePerColumn);
            rw.Boolean(ref topToBottom);
        }
    }


    public enum EShaderTcType
    {
        None,
        TransSubTexture,
    }

    public enum AnimEase
    {
        Constant,
        Linear,
        QuadIn,
        QuadOut,
        QuadInOut,
        CubicIn,
        CubicOut,
        CubicInOut,
        QuartIn,
        QuartOut,
        QuartInOut,
        QuintIn,
        QuintOut,
        QuintInOut,
        SineIn,
        SineOut,
        SineInOut,
        ExpIn,
        ExpOut,
        ExpInOut,
        CircIn,
        CircOut,
        CircInOut,
        BackIn,
        BackOut,
        BackInOut,
        ElasticIn,
        ElasticOut,
        ElasticInOut,
        ElasticIn2,
        ElasticOut2,
        ElasticInOut2,
        BounceIn,
        BounceOut,
        BounceInOut,
    }

    public enum EAxis
    {
        X,
        Y,
        Z,
    }

}
