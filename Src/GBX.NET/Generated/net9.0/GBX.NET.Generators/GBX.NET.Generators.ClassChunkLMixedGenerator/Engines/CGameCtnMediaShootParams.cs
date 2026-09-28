namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03060000</remarks>
[Class(0x03060000)]
public partial class CGameCtnMediaShootParams : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03060000;




    private int videoFps;
    [AppliedWithChunk<Chunk03060001>]
    [AppliedWithChunk<Chunk03060002>]
    public int VideoFps { get => videoFps; set => videoFps = value; }

    private int sizeX;
    [AppliedWithChunk<Chunk03060001>]
    [AppliedWithChunk<Chunk03060002>]
    public int SizeX { get => sizeX; set => sizeX = value; }

    private int sizeY;
    [AppliedWithChunk<Chunk03060001>]
    [AppliedWithChunk<Chunk03060002>]
    public int SizeY { get => sizeY; set => sizeY = value; }

    private bool hq;
    [AppliedWithChunk<Chunk03060001>]
    [AppliedWithChunk<Chunk03060002>]
    public bool Hq { get => hq; set => hq = value; }

    private int hqSampleCountPerAxe;
    [AppliedWithChunk<Chunk03060001>]
    [AppliedWithChunk<Chunk03060002>]
    public int HqSampleCountPerAxe { get => hqSampleCountPerAxe; set => hqSampleCountPerAxe = value; }

    private bool hqSoftShadows;
    [AppliedWithChunk<Chunk03060001>]
    [AppliedWithChunk<Chunk03060002>]
    public bool HqSoftShadows { get => hqSoftShadows; set => hqSoftShadows = value; }

    private bool hqAmbientOcc;
    [AppliedWithChunk<Chunk03060001>]
    [AppliedWithChunk<Chunk03060002>]
    public bool HqAmbientOcc { get => hqAmbientOcc; set => hqAmbientOcc = value; }

    private bool isAudioStream;
    [AppliedWithChunk<Chunk03060001>]
    [AppliedWithChunk<Chunk03060002>]
    public bool IsAudioStream { get => isAudioStream; set => isAudioStream = value; }

    private EStereo3d stereo3d;
    [AppliedWithChunk<Chunk03060001>]
    [AppliedWithChunk<Chunk03060002>]
    public EStereo3d Stereo3d { get => stereo3d; set => stereo3d = value; }

    private VideoEnc? videoEncoding;
    [AppliedWithChunk<Chunk03060002>]
    public VideoEnc? VideoEncoding { get => videoEncoding; set => videoEncoding = value; }

    private AudioEnc? audioEncoding;
    [AppliedWithChunk<Chunk03060002>]
    public AudioEnc? AudioEncoding { get => audioEncoding; set => audioEncoding = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaShootParams"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaShootParams() { }


    /// <summary>
    /// CGameCtnMediaShootParams 0x001 chunk
    /// </summary>
    [Chunk(0x03060001)]
    public partial class Chunk03060001 : Chunk<CGameCtnMediaShootParams>
    {
        /// <inheritdoc />
        public override uint Id => 0x03060001;

        public bool U01;

        public override void ReadWrite(CGameCtnMediaShootParams n, GbxReaderWriter rw)
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

    /// <summary>
    /// CGameCtnMediaShootParams 0x002 chunk
    /// </summary>
    [Chunk(0x03060002)]
    public partial class Chunk03060002 : Chunk<CGameCtnMediaShootParams>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03060002;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public bool U03;
        public bool U04;
        public bool U05;
        public bool U06;
        public int U07;
        public int U08;
        public bool U09;
        public int U10;

        public override void ReadWrite(CGameCtnMediaShootParams n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref n.videoFps);
            rw.Int32(ref n.sizeX);
            rw.Int32(ref n.sizeY);
            rw.Boolean(ref n.hq);
            rw.Int32(ref n.hqSampleCountPerAxe);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref U05);
            rw.Boolean(ref U06);
            rw.Boolean(ref n.hqSoftShadows);
            rw.Boolean(ref n.hqAmbientOcc);
            rw.Boolean(ref n.isAudioStream);
            rw.EnumInt32<EStereo3d>(ref n.stereo3d);
            rw.Int32(ref U07);
            rw.Int32(ref U08);
            rw.Boolean(ref U09);
            rw.Int32(ref U10);
            rw.ReadableWritable<VideoEnc>(ref n.videoEncoding);
            rw.ReadableWritable<AudioEnc>(ref n.audioEncoding);
        }
    }


    public sealed partial class AudioEnc : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private float u01;
        public float U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.Single(ref u01);
        }
    }

    public sealed partial class VideoEnc : IReadableWritable
    {

        private int version;
        public int Version { get => version; set => version = value; }

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref this.version);
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
        }
    }


    public enum EStereo3d
    {
        None,
        RedNCyan,
        LeftNRight,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03060001 => new Chunk03060001(),
        0x03060002 => new Chunk03060002(),
        _ => base.NewChunk(chunkId),
    };
}
