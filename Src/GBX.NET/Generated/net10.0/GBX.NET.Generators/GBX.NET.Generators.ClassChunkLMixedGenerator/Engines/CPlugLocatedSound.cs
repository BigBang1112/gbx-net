namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09061000</remarks>
[Class(0x09061000)]
public partial class CPlugLocatedSound : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x09061000;




    private CPlugSound? sound;
    [AppliedWithChunk<Chunk09061000>]
    public CPlugSound? Sound { get => soundFile?.GetNode(ref sound) ?? sound; set => sound = value; }
    private Components.GbxRefTableFile? soundFile;
    public Components.GbxRefTableFile? SoundFile { get => soundFile; set => soundFile = value; }
    public CPlugSound? GetSound(GbxReadSettings settings = default, bool exceptions = false) => soundFile?.GetNode(ref sound, settings, exceptions) ?? sound;

    private Iso4 loc;
    [AppliedWithChunk<Chunk09061000>]
    public Iso4 Loc { get => loc; set => loc = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugLocatedSound"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugLocatedSound() { }


    /// <summary>
    /// CPlugLocatedSound 0x000 chunk
    /// </summary>
    [Chunk(0x09061000)]
    public partial class Chunk09061000 : Chunk<CPlugLocatedSound>
    {
        /// <inheritdoc />
        public override uint Id => 0x09061000;


        public override void ReadWrite(CPlugLocatedSound n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugSound>(ref n.sound, ref n.soundFile);
            rw.Iso4(ref n.loc);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09061000 => new Chunk09061000(),
        _ => base.NewChunk(chunkId),
    };
}
