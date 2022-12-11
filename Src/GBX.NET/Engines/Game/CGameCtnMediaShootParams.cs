namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - shoot parameters.
/// </summary>
/// <remarks>ID: 0x03060000</remarks>
[Node(0x03060000)]
public class CGameCtnMediaShootParams : CMwNod // CGameCtnMediaVideoParams
{
    #region Enums

    public enum EStereo3d
    {
        None,
        RedNCyan,
        LeftNRight
    }

    #endregion

    #region Fields

    private int videoFps;
    private int sizeX;
    private int sizeY;
    private bool hq;
    private int hqSampleCountPerAxe;
    private bool hqSoftShadows;
    private bool hqAmbientOcc;
    private bool isAudioStream;
    private EStereo3d stereo3d;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03060001>]
    public int VideoFps { get => videoFps; set => videoFps = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03060001>]
    public int SizeX { get => sizeX; set => sizeX = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03060001>]
    public int SizeY { get => sizeY; set => sizeY = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03060001>]
    public bool Hq { get => hq; set => hq = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03060001>]
    public int HqSampleCountPerAxe { get => hqSampleCountPerAxe; set => hqSampleCountPerAxe = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03060001>]
    public bool HqSoftShadows { get => hqSoftShadows; set => hqSoftShadows = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03060001>]
    public bool HqAmbientOcc { get => hqAmbientOcc; set => hqAmbientOcc = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03060001>]
    public bool IsAudioStream { get => isAudioStream; set => isAudioStream = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03060001>]
    public EStereo3d Stereo3d { get => stereo3d; set => stereo3d = value; }

    #endregion

    internal CGameCtnMediaShootParams()
    {

    }

    #region Chunks

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaShootParams 0x001 chunk
    /// </summary>
    [Chunk(0x03060001)]
    public class Chunk03060001 : Chunk<CGameCtnMediaShootParams>
    {
        public bool U01;

        public override void ReadWrite(CGameCtnMediaShootParams n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.videoFps);
            rw.Int32(ref n.sizeX);
            rw.Int32(ref n.sizeY);
            rw.Boolean(ref n.hq);
            rw.Int32(ref n.hqSampleCountPerAxe);
            rw.Boolean(ref U01);
            rw.Boolean(ref n.hqSoftShadows);
            rw.Boolean(ref n.hqAmbientOcc);
            rw.Boolean(ref n.isAudioStream);
            rw.EnumInt32<EStereo3d>(ref n.stereo3d);
        }
    }

    #endregion

    #endregion
}
