namespace GBX.NET.Engines.Plug;

public partial class CPlugMaterialCustom
{
    [Obsolete("Use Bitmaps instead.")]
    public Bitmap[]? Textures { get => Bitmaps; set => Bitmaps = value; }

    public partial class Bitmap
    {
        [Obsolete("Use LayerName instead.")]
        public string? Name { get => LayerName; set => LayerName = value; }

        [Obsolete("Use BitmapOrImage instead.")]
        public CMwNod? Texture { get => BitmapOrImage; set => BitmapOrImage = value; }

        [Obsolete("Use TexAddressU instead.")]
        public int U02 { get => TexAddressU; set => TexAddressU = value; }

        [Obsolete("Use TexAddressV instead.")]
        public int U03 { get => TexAddressV; set => TexAddressV = value; }
    }

    [Obsolete("Use GpuFxColors instead.")]
    public GpuFx[]? GpuFxs1 { get => GpuFxColors; set => GpuFxColors = value; }

    [Obsolete("Use GpuFxReals instead.")]
    public GpuFx[]? GpuFxs2 { get => GpuFxReals; set => GpuFxReals = value; }

    public partial class GpuFx
    {
        [Obsolete("Use Name instead.")]
        public string U01 { get => Name ?? ""; set => Name = value; }

        [Obsolete("Use Values instead.")]
        public float[][] U03 { get => Values; set => Values = value; }
    }

    public partial class BitmapSkip
    {
        [Obsolete("Use LayerName instead.")]
        public string? Name { get => LayerName; set => LayerName = value; }

        [Obsolete("Use IsSkipped instead.")]
        public bool U01 { get => IsSkipped; set => IsSkipped = value; }
    }

    public partial class DefineNat
    {
        [Obsolete("Use Name instead.")]
        public string? U01 { get => Name; set => Name = value; }

        [Obsolete("Use Value instead.")]
        public int U02 { get => Value; set => Value = value; }
    }

    public partial class CBuffer
    {
        [Obsolete("Use ByteSize instead.")]
        public int DataSize { get => ByteSize; set => ByteSize = value; }

        [Obsolete("Use Stage instead.")]
        public int U01 { get => (int)Stage; set => Stage = (EGpuStage)value; }

        [Obsolete("Use Data instead.")]
        public byte[]? U02 { get => Data; set => Data = value; }
    }

    public partial class VisibleFilterData
    {
        [Obsolete("Use BitMask instead.")]
        public short U01 { get => BitMask; set => BitMask = value; }

        [Obsolete("Use BitValue instead.")]
        public short U02 { get => BitValue; set => BitValue = value; }
    }
}
