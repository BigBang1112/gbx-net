namespace GBX.NET.Engines.Hms;

public partial class CHmsLightMapCache
{
    public Id Collection { get; set; }

    [Obsolete("Use LightAmbSampleCount instead.")]
    public int AmbSample { get => LightAmbSampleCount; set => LightAmbSampleCount = value; }

    [Obsolete("Use LightDirSampleCount instead.")]
    public int DirSamples { get => LightDirSampleCount; set => LightDirSampleCount = value; }

    [Obsolete("Use LightPntSampleCount instead.")]
    public int PntSamples { get => LightPntSampleCount; set => LightPntSampleCount = value; }

    [Obsolete("Use TimeWriteMostRecentSolid instead.")]
    public DateTime? TimeWrite { get => TimeWriteMostRecentSolid; set => TimeWriteMostRecentSolid = value; }

    public partial class SMap
    {
        [Obsolete("Use BlockPerMap.X instead.")]
        public int U01 { get => BlockPerMap.X; set => BlockPerMap = new(value, BlockPerMap.Y); }

        [Obsolete("Use BlockPerMap.Y instead.")]
        public int U02 { get => BlockPerMap.Y; set => BlockPerMap = new(BlockPerMap.X, value); }

        [Obsolete("Use UsedBlockCount instead.")]
        public int U03 { get => UsedBlockCount; set => UsedBlockCount = value; }

        [Obsolete("Use OutsideBlockCount.X instead.")]
        public int U04 { get => OutsideBlockCount.X; set => OutsideBlockCount = new(value, OutsideBlockCount.Y); }

        [Obsolete("Use OutsideBlockCount.Y instead.")]
        public int U05 { get => OutsideBlockCount.Y; set => OutsideBlockCount = new(OutsideBlockCount.X, value); }
    }

    public partial class SFrame
    {
        public float ReplayTime { get; set; } = float.MinValue;
        public Vec3 LAmbient { get; set; } = new(-1, -1, -1);
    }

    public sealed partial class Frame
    {
        [WebpData] // NOT always but mostly better to have
        public byte[]? Data { get; set; }

        [WebpData] // NOT always but mostly better to have
        public byte[]? Data2 { get; set; }

        [WebpData] // NOT always but mostly better to have
        public byte[]? Data3 { get; set; }
    }
}
