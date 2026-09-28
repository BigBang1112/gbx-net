namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03072000</remarks>
[Class(0x03072000)]
public partial class CGameControlCameraTarget : CGameControlCamera, IClass
{
    [Hexadecimal] public static new uint Id => 0x03072000;




    private Iso4 location;
    [AppliedWithChunk<Chunk03072001>]
    public Iso4 Location { get => location; set => location = value; }

    private bool isUpLinked;
    [AppliedWithChunk<Chunk03072001>]
    public bool IsUpLinked { get => isUpLinked; set => isUpLinked = value; }

    private bool interpolate;
    [AppliedWithChunk<Chunk03072001>]
    public bool Interpolate { get => interpolate; set => interpolate = value; }

    private float lookAtFactor;
    [AppliedWithChunk<Chunk03072001>]
    public float LookAtFactor { get => lookAtFactor; set => lookAtFactor = value; }

    private bool canUseRelativeTargetLocation;
    [AppliedWithChunk<Chunk03072002>]
    public bool CanUseRelativeTargetLocation { get => canUseRelativeTargetLocation; set => canUseRelativeTargetLocation = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameControlCameraTarget"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameControlCameraTarget() { }


    /// <summary>
    /// CGameControlCameraTarget 0x001 chunk
    /// </summary>
    [Chunk(0x03072001)]
    public partial class Chunk03072001 : Chunk<CGameControlCameraTarget>
    {
        /// <inheritdoc />
        public override uint Id => 0x03072001;


        public override void ReadWrite(CGameControlCameraTarget n, GbxReaderWriter rw)
        {
            rw.Iso4(ref n.location);
            rw.Boolean(ref n.isUpLinked);
            rw.Boolean(ref n.interpolate);
            rw.Single(ref n.lookAtFactor);
        }
    }

    /// <summary>
    /// CGameControlCameraTarget 0x002 chunk
    /// </summary>
    [Chunk(0x03072002)]
    public partial class Chunk03072002 : Chunk<CGameControlCameraTarget>
    {
        /// <inheritdoc />
        public override uint Id => 0x03072002;


        public override void ReadWrite(CGameControlCameraTarget n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.canUseRelativeTargetLocation);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03072001 => new Chunk03072001(),
        0x03072002 => new Chunk03072002(),
        _ => base.NewChunk(chunkId),
    };
}
