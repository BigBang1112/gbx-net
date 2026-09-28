namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090D8000</remarks>
[Class(0x090D8000)]
public partial class CPlugFxLightning : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x090D8000;




    private CPlugSound? sound;
    [AppliedWithChunk<Chunk090D8000>]
    public CPlugSound? Sound { get => soundFile?.GetNode(ref sound) ?? sound; set => sound = value; }
    private Components.GbxRefTableFile? soundFile;
    public Components.GbxRefTableFile? SoundFile { get => soundFile; set => soundFile = value; }
    public CPlugSound? GetSound(GbxReadSettings settings = default, bool exceptions = false) => soundFile?.GetNode(ref sound, settings, exceptions) ?? sound;

    /// <summary>
    /// Creates a new instance of <see cref="CPlugFxLightning"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugFxLightning() { }


    /// <summary>
    /// CPlugFxLightning 0x000 chunk
    /// </summary>
    [Chunk(0x090D8000)]
    public partial class Chunk090D8000 : Chunk<CPlugFxLightning>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090D8000;

        public int Version { get; set; }

        public External<CFuncKeysReal>[]? U01;
        public Components.GbxRefTableFile? U01File;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;

        public override void ReadWrite(CPlugFxLightning n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugSound>(ref n.sound, ref n.soundFile);
            rw.ArrayNodeRef_deprec<CFuncKeysReal>(ref U01!);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            if (Version >= 1)
            {
                rw.Single(ref U06);
                rw.Single(ref U07);
                rw.Single(ref U08);
                rw.Single(ref U09);
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090D8000 => new Chunk090D8000(),
        _ => base.NewChunk(chunkId),
    };
}
