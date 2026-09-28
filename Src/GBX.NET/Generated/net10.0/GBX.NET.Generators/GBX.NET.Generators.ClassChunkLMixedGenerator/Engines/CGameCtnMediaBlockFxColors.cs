namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03080000</remarks>
[Class(0x03080000)]
public partial class CGameCtnMediaBlockFxColors : CGameCtnMediaBlockFx, IClass, CGameCtnMediaBlock.IHasKeys
{
    [Hexadecimal] public static new uint Id => 0x03080000;

    IEnumerable<IKey> IHasKeys.Keys => Keys ?? [];



    private List<Key>? keys;
    [AppliedWithChunk<Chunk03080000>]
    [AppliedWithChunk<Chunk03080001>]
    [AppliedWithChunk<Chunk03080002>]
    [AppliedWithChunk<Chunk03080003>]
    public List<Key>? Keys { get => keys; set => keys = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockFxColors"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockFxColors() { }


    /// <summary>
    /// CGameCtnMediaBlockFxColors 0x000 chunk
    /// </summary>
    [Chunk(0x03080000)]
    public partial class Chunk03080000 : Chunk<CGameCtnMediaBlockFxColors>
    {
        /// <inheritdoc />
        public override uint Id => 0x03080000;


        public override void ReadWrite(CGameCtnMediaBlockFxColors n, GbxReaderWriter rw)
        {
            rw.ListReadableWritable<Key>(ref n.keys!);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockFxColors 0x001 chunk
    /// </summary>
    [Chunk(0x03080001)]
    public partial class Chunk03080001 : Chunk03080000
    {
        /// <inheritdoc />
        public override uint Id => 0x03080001;

    }

    /// <summary>
    /// CGameCtnMediaBlockFxColors 0x002 chunk
    /// </summary>
    [Chunk(0x03080002)]
    public partial class Chunk03080002 : Chunk03080000
    {
        /// <inheritdoc />
        public override uint Id => 0x03080002;

    }

    /// <summary>
    /// CGameCtnMediaBlockFxColors 0x003 chunk
    /// </summary>
    [Chunk(0x03080003)]
    public partial class Chunk03080003 : Chunk03080000
    {
        /// <inheritdoc />
        public override uint Id => 0x03080003;

    }


    public sealed partial class Key : IKey, IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private float intensity;
        public float Intensity { get => intensity; set => intensity = value; }

        private float blendZ;
        public float BlendZ { get => blendZ; set => blendZ = value; }

        private float distance;
        public float Distance { get => distance; set => distance = value; }

        private float farDistance;
        public float FarDistance { get => farDistance; set => farDistance = value; }

        private float inverse;
        public float Inverse { get => inverse; set => inverse = value; }

        private float hue;
        public float Hue { get => hue; set => hue = value; }

        private float saturation;
        /// <summary>
        /// from center
        /// </summary>
        public float Saturation { get => saturation; set => saturation = value; }

        private float brightness;
        /// <summary>
        /// from center
        /// </summary>
        public float Brightness { get => brightness; set => brightness = value; }

        private float contrast;
        /// <summary>
        /// from center
        /// </summary>
        public float Contrast { get => contrast; set => contrast = value; }

        private Vec3 rgb;
        public Vec3 Rgb { get => rgb; set => rgb = value; }

        private float u01;
        public float U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float farInverse;
        public float FarInverse { get => farInverse; set => farInverse = value; }

        private float farHue;
        public float FarHue { get => farHue; set => farHue = value; }

        private float farSaturation;
        /// <summary>
        /// from center
        /// </summary>
        public float FarSaturation { get => farSaturation; set => farSaturation = value; }

        private float farBrightness;
        /// <summary>
        /// from center
        /// </summary>
        public float FarBrightness { get => farBrightness; set => farBrightness = value; }

        private float farContrast;
        /// <summary>
        /// from center
        /// </summary>
        public float FarContrast { get => farContrast; set => farContrast = value; }

        private Vec3 farRgb;
        public Vec3 FarRgb { get => farRgb; set => farRgb = value; }

        private float farU01;
        public float FarU01 { get => farU01; set => farU01 = value; }

        private float farU02;
        public float FarU02 { get => farU02; set => farU02 = value; }

        private float farU03;
        public float FarU03 { get => farU03; set => farU03 = value; }

        private float farU04;
        public float FarU04 { get => farU04; set => farU04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.TimeSingle(ref time);
            rw.Single(ref intensity);
            rw.Single(ref blendZ);
            rw.Single(ref distance);
            rw.Single(ref farDistance);
            rw.Single(ref inverse);
            rw.Single(ref hue);
            rw.Single(ref saturation); // from center
            rw.Single(ref brightness); // from center
            rw.Single(ref contrast); // from center
            rw.Vec3(ref rgb);
            rw.Single(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref farInverse);
            rw.Single(ref farHue);
            rw.Single(ref farSaturation); // from center
            rw.Single(ref farBrightness); // from center
            rw.Single(ref farContrast); // from center
            rw.Vec3(ref farRgb);
            rw.Single(ref farU01);
            rw.Single(ref farU02);
            rw.Single(ref farU03);
            rw.Single(ref farU04);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03080000 => new Chunk03080000(),
        0x03080001 => new Chunk03080001(),
        0x03080002 => new Chunk03080002(),
        0x03080003 => new Chunk03080003(),
        _ => base.NewChunk(chunkId),
    };
}
