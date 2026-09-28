namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03166000</remarks>
[Class(0x03166000)]
public partial class CGameCtnMediaBlockCameraEffectInertialTracking : CGameCtnMediaBlockCameraEffect, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x03166000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk03166000>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk03166000>]
    public TimeSingle End { get => end; set => end = value; }

    private bool tracking;
    [AppliedWithChunk<Chunk03166000>]
    public bool Tracking { get => tracking; set => tracking = value; }

    private bool autoZoom;
    [AppliedWithChunk<Chunk03166000>]
    public bool AutoZoom { get => autoZoom; set => autoZoom = value; }

    private bool autoFocus;
    [AppliedWithChunk<Chunk03166000>]
    public bool AutoFocus { get => autoFocus; set => autoFocus = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockCameraEffectInertialTracking"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockCameraEffectInertialTracking() { }


    /// <summary>
    /// CGameCtnMediaBlockCameraEffectInertialTracking 0x000 chunk
    /// </summary>
    [Chunk(0x03166000)]
    public partial class Chunk03166000 : Chunk<CGameCtnMediaBlockCameraEffectInertialTracking>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03166000;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockCameraEffectInertialTracking n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Boolean(ref n.tracking);
            rw.Boolean(ref n.autoZoom);
            rw.Boolean(ref n.autoFocus);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03166000 => new Chunk03166000(),
        _ => base.NewChunk(chunkId),
    };
}
