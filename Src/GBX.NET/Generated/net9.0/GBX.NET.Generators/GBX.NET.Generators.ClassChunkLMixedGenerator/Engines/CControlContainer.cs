namespace GBX.NET.Engines.Control;

/// <remarks>ID: 0x07002000</remarks>
[Class(0x07002000)]
public partial class CControlContainer : CControlBase, IClass
{
    [Hexadecimal] public static new uint Id => 0x07002000;




    private bool acceptOwnControls;
    [AppliedWithChunk<Chunk07002005>]
    public bool AcceptOwnControls { get => acceptOwnControls; set => acceptOwnControls = value; }

    private bool useScript;
    [AppliedWithChunk<Chunk07002005>]
    public bool UseScript { get => useScript; set => useScript = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CControlContainer"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CControlContainer() { }


    /// <summary>
    /// CControlContainer 0x005 chunk
    /// </summary>
    [Chunk(0x07002005)]
    public partial class Chunk07002005 : Chunk<CControlContainer>
    {
        /// <inheritdoc />
        public override uint Id => 0x07002005;

        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CControlContainer n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.acceptOwnControls);
            rw.Boolean(ref n.useScript);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x07002005 => new Chunk07002005(),
        _ => base.NewChunk(chunkId),
    };
}
