namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E00B000</remarks>
[Class(0x2E00B000)]
public partial class CGameGateModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E00B000;




    private CGameArmorModel? armor;
    [AppliedWithChunk<Chunk2E00B000>]
    public CGameArmorModel? Armor { get => armorFile?.GetNode(ref armor) ?? armor; set => armor = value; }
    private Components.GbxRefTableFile? armorFile;
    public Components.GbxRefTableFile? ArmorFile { get => armorFile; set => armorFile = value; }
    public CGameArmorModel? GetArmor(GbxReadSettings settings = default, bool exceptions = false) => armorFile?.GetNode(ref armor, settings, exceptions) ?? armor;

    private CPlugSurface? shape;
    [AppliedWithChunk<Chunk2E00B000>]
    public CPlugSurface? Shape { get => shapeFile?.GetNode(ref shape) ?? shape; set => shape = value; }
    private Components.GbxRefTableFile? shapeFile;
    public Components.GbxRefTableFile? ShapeFile { get => shapeFile; set => shapeFile = value; }
    public CPlugSurface? GetShape(GbxReadSettings settings = default, bool exceptions = false) => shapeFile?.GetNode(ref shape, settings, exceptions) ?? shape;

    private CPlugSound? gateOpenSound;
    [AppliedWithChunk<Chunk2E00B001>]
    public CPlugSound? GateOpenSound { get => gateOpenSoundFile?.GetNode(ref gateOpenSound) ?? gateOpenSound; set => gateOpenSound = value; }
    private Components.GbxRefTableFile? gateOpenSoundFile;
    public Components.GbxRefTableFile? GateOpenSoundFile { get => gateOpenSoundFile; set => gateOpenSoundFile = value; }
    public CPlugSound? GetGateOpenSound(GbxReadSettings settings = default, bool exceptions = false) => gateOpenSoundFile?.GetNode(ref gateOpenSound, settings, exceptions) ?? gateOpenSound;

    private CPlugSound? gateCloseSound;
    [AppliedWithChunk<Chunk2E00B001>]
    public CPlugSound? GateCloseSound { get => gateCloseSoundFile?.GetNode(ref gateCloseSound) ?? gateCloseSound; set => gateCloseSound = value; }
    private Components.GbxRefTableFile? gateCloseSoundFile;
    public Components.GbxRefTableFile? GateCloseSoundFile { get => gateCloseSoundFile; set => gateCloseSoundFile = value; }
    public CPlugSound? GetGateCloseSound(GbxReadSettings settings = default, bool exceptions = false) => gateCloseSoundFile?.GetNode(ref gateCloseSound, settings, exceptions) ?? gateCloseSound;

    private CPlugSound? gateSound;
    [AppliedWithChunk<Chunk2E00B001>]
    public CPlugSound? GateSound { get => gateSoundFile?.GetNode(ref gateSound) ?? gateSound; set => gateSound = value; }
    private Components.GbxRefTableFile? gateSoundFile;
    public Components.GbxRefTableFile? GateSoundFile { get => gateSoundFile; set => gateSoundFile = value; }
    public CPlugSound? GetGateSound(GbxReadSettings settings = default, bool exceptions = false) => gateSoundFile?.GetNode(ref gateSound, settings, exceptions) ?? gateSound;

    /// <summary>
    /// Creates a new instance of <see cref="CGameGateModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameGateModel() { }


    /// <summary>
    /// CGameGateModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E00B000)]
    public partial class Chunk2E00B000 : Chunk<CGameGateModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00B000;

        public int Version { get; set; }


        public override void ReadWrite(CGameGateModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CGameArmorModel>(ref n.armor, ref n.armorFile);
            rw.NodeRef<CPlugSurface>(ref n.shape, ref n.shapeFile);
        }
    }

    /// <summary>
    /// CGameGateModel 0x001 chunk
    /// </summary>
    [Chunk(0x2E00B001)]
    public partial class Chunk2E00B001 : Chunk<CGameGateModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E00B001;

        public int Version { get; set; }


        public override void ReadWrite(CGameGateModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugSound>(ref n.gateOpenSound, ref n.gateOpenSoundFile);
            rw.NodeRef<CPlugSound>(ref n.gateCloseSound, ref n.gateCloseSoundFile);
            if (Version >= 1)
            {
                rw.NodeRef<CPlugSound>(ref n.gateSound, ref n.gateSoundFile);
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E00B000 => new Chunk2E00B000(),
        0x2E00B001 => new Chunk2E00B001(),
        _ => base.NewChunk(chunkId),
    };
}
