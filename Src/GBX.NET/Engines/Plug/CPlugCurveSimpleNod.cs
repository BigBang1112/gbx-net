namespace GBX.NET.Engines.Plug;

public partial class CPlugCurveSimpleNod
{
    private PlugCurve curve = new();
    /// <remarks>Applied with: 0x000 body chunk</remarks>
    [AppliedWithChunk<Chunk09185000>]
    public PlugCurve Curve { get => curve; set => curve = value; }

    [Obsolete("Use Curve.Version instead.")]
    public int Version2
    {
        get => Curve.Version;
        set => Curve.Version = value;
    }

    [Obsolete("Use Curve.Count instead.")]
    public int Count
    {
        get => Curve.Count;
        set => Curve.Count = value;
    }
}
