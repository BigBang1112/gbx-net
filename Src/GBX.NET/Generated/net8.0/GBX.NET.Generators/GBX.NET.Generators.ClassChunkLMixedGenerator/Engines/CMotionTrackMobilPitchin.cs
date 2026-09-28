namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x08041000</remarks>
[Class(0x08041000)]
public partial class CMotionTrackMobilPitchin : CMotionTrack, IClass
{
    [Hexadecimal] public static new uint Id => 0x08041000;




    private CMwNod? sea;
    [AppliedWithChunk<Chunk08041002>]
    public CMwNod? Sea { get => sea; set => sea = value; }

    private float flottaison;
    [AppliedWithChunk<Chunk08041002>]
    public float Flottaison { get => flottaison; set => flottaison = value; }

    private float tangage;
    [AppliedWithChunk<Chunk08041002>]
    public float Tangage { get => tangage; set => tangage = value; }

    private float roulis;
    [AppliedWithChunk<Chunk08041002>]
    public float Roulis { get => roulis; set => roulis = value; }

    private float offsetHauteur;
    [AppliedWithChunk<Chunk08041002>]
    public float OffsetHauteur { get => offsetHauteur; set => offsetHauteur = value; }

    private EPitchinMode pitchinMode;
    [AppliedWithChunk<Chunk08041002>]
    public EPitchinMode PitchinMode { get => pitchinMode; set => pitchinMode = value; }

    private float periodDelta;
    [AppliedWithChunk<Chunk08041002>]
    public float PeriodDelta { get => periodDelta; set => periodDelta = value; }

    private float maxAngle;
    [AppliedWithChunk<Chunk08041002>]
    public float MaxAngle { get => maxAngle; set => maxAngle = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CMotionTrackMobilPitchin"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMotionTrackMobilPitchin() { }


    /// <summary>
    /// CMotionTrackMobilPitchin 0x002 chunk
    /// </summary>
    [Chunk(0x08041002)]
    public partial class Chunk08041002 : Chunk<CMotionTrackMobilPitchin>
    {
        /// <inheritdoc />
        public override uint Id => 0x08041002;


        public override void ReadWrite(CMotionTrackMobilPitchin n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref n.sea);
            rw.Single(ref n.flottaison);
            rw.Single(ref n.tangage);
            rw.Single(ref n.roulis);
            rw.Single(ref n.offsetHauteur);
            rw.EnumInt32<EPitchinMode>(ref n.pitchinMode);
            rw.Single(ref n.periodDelta);
            rw.Single(ref n.maxAngle);
        }
    }



    public enum EPitchinMode
    {
        Normal,
        Simple,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x08041002 => new Chunk08041002(),
        _ => base.NewChunk(chunkId),
    };
}
