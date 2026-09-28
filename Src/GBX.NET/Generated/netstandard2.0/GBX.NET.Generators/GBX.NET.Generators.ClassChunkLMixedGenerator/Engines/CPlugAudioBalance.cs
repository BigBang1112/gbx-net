namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09034000</remarks>
[Class(0x09034000)]
public partial class CPlugAudioBalance : CPlugAudio, IClass
{
    [Hexadecimal] public static new uint Id => 0x09034000;




    private Group[]? groups;
    [AppliedWithChunk<Chunk09034000>]
    public Group[]? Groups { get => groups; set => groups = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugAudioBalance"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugAudioBalance() { }


    /// <summary>
    /// CPlugAudioBalance 0x000 chunk
    /// </summary>
    [Chunk(0x09034000)]
    public partial class Chunk09034000 : Chunk<CPlugAudioBalance>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09034000;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public CFuncKeysReal? U08;
        public Components.GbxRefTableFile? U08File;
        public CFuncKeysReal? U09;
        public Components.GbxRefTableFile? U09File;
        public CFuncKeysReal? U10;
        public Components.GbxRefTableFile? U10File;
        public CFuncKeysReal? U11;
        public Components.GbxRefTableFile? U11File;
        public CFuncKeysReal? U12;
        public Components.GbxRefTableFile? U12File;
        public float U13;
        public float U14;
        public float U15;
        public float U16;
        public float U17;

        public override void ReadWrite(CPlugAudioBalance n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ArrayReadableWritable<Group>(ref n.groups!, version: Version);
            if (Version >= 1)
            {
                rw.Single(ref U01);
                if (Version >= 2)
                {
                    rw.Single(ref U02);
                    if (Version >= 6)
                    {
                        rw.Single(ref U03);
                        if (Version >= 7)
                        {
                            rw.Single(ref U04);
                            rw.Single(ref U05);
                            rw.Single(ref U06);
                            rw.Single(ref U07);
                            rw.NodeRef<CFuncKeysReal>(ref U08, ref U08File);
                            rw.NodeRef<CFuncKeysReal>(ref U09, ref U09File);
                            if (Version >= 8)
                            {
                                rw.NodeRef<CFuncKeysReal>(ref U10, ref U10File);
                                rw.NodeRef<CFuncKeysReal>(ref U11, ref U11File);
                                if (Version >= 9)
                                {
                                    rw.NodeRef<CFuncKeysReal>(ref U12, ref U12File);
                                    if (Version >= 10)
                                    {
                                        rw.Single(ref U13);
                                        rw.Single(ref U14);
                                        rw.Single(ref U15);
                                        if (Version >= 11)
                                        {
                                            rw.Single(ref U16);
                                            if (Version >= 12)
                                            {
                                                rw.Single(ref U17);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugAudioBalance 0x001 chunk
    /// </summary>
    [Chunk(0x09034001)]
    public partial class Chunk09034001 : Chunk<CPlugAudioBalance>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09034001;

        public int Version { get; set; }

        public float U01;
        public float U02;
        public float U03;
        public bool U04;

        public override void ReadWrite(CPlugAudioBalance n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Boolean(ref U04);
        }
    }


    public sealed partial class Group : IReadableWritable
    {

        private Line? u01;
        public Line? U01 { get => u01; set => u01 = value; }

        private Line? u02;
        public Line? U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.ReadableWritable<Line>(ref u01, version: v);
            rw.ReadableWritable<Line>(ref u02, version: v);
        }
    }

    public sealed partial class Line : IReadableWritable
    {

        private float u01;
        public float U01 { get => u01; set => u01 = value; }

        private float u02;
        public float U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Single(ref u01);
            rw.Single(ref u02);
            rw.Single(ref u03);
            if (v >= 3)
            {
                rw.Single(ref u04);
                if (v >= 6)
                {
                    rw.Single(ref u05);
                }
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09034000 => new Chunk09034000(),
        0x09034001 => new Chunk09034001(),
        _ => base.NewChunk(chunkId),
    };
}
