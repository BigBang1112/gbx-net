namespace GBX.NET.Engines.Game;

[Node(0x03060000)]
public class CGameCtnMediaShootParams : CMwNod // CGameCtnMediaVideoParams
{
    private int videoFps;
    private int sizeX;
    private int sizeY;
    private bool hq;
    private int hqSampleCountPerAxe;
    private bool hqSoftShadows;
    private bool hqAmbientOcc;
    private bool isAudioStream;
    private int stereo3d;

    public int VideoFps
    {
        get => videoFps;
        set => videoFps = value;
    }

    public int SizeX
    {
        get => sizeX;
        set => sizeX = value;
    }

    public int SizeY
    {
        get => sizeY;
        set => sizeY = value;
    }

    public bool Hq
    {
        get => hq;
        set => hq = value;
    }

    public int HqSampleCountPerAxe
    {
        get => hqSampleCountPerAxe;
        set => hqSampleCountPerAxe = value;
    }

    public bool HqSoftShadows
    {
        get => hqSoftShadows;
        set => hqSoftShadows = value;
    }

    public bool HqAmbientOcc
    {
        get => hqAmbientOcc;
        set => hqAmbientOcc = value;
    }

    public bool IsAudioStream
    {
        get => isAudioStream;
        set => isAudioStream = value;
    }

    public int Stereo3d
    {
        get => stereo3d;
        set => stereo3d = value;
    }

    protected CGameCtnMediaShootParams()
    {

    }

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
            rw.Int32(ref n.stereo3d);
        }
    }
}
