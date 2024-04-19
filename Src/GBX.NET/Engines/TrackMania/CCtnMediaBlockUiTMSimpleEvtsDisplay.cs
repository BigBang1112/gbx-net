namespace GBX.NET.Engines.TrackMania;

public partial class CCtnMediaBlockUiTMSimpleEvtsDisplay
{
    [AppliedWithChunk<Chunk24092000>]
    [AppliedWithChunk<Chunk24092002>]
    public EDisplayMode DisplayMode { get; set; }

    public partial class Chunk24092000
    {
        public override void Read(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GbxReader r)
        {
            n.DisplayMode = (EDisplayMode)r.ReadInt32();
        }

        public override void Write(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GbxWriter w)
        {
            w.Write(n.DisplayMode == EDisplayMode.Always || n.DisplayMode == EDisplayMode.Never);
        }
    }

    public partial class Chunk24092002
    {
        public override void Read(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GbxReader r)
        {
            var displayMode = r.ReadBoolean();
            if (displayMode) n.DisplayMode = EDisplayMode.Never;
        }

        public override void Write(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GbxWriter w)
        {
            w.Write(n.DisplayMode == EDisplayMode.Never);
        }
    }
}
