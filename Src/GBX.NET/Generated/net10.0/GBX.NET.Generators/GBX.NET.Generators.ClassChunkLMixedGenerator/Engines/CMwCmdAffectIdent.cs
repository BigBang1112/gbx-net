namespace GBX.NET.Engines.MwFoundations;

/// <remarks>ID: 0x01032000</remarks>
[Class(0x01032000)]
public partial class CMwCmdAffectIdent : CMwCmdInst, IClass
{
    [Hexadecimal] public static new uint Id => 0x01032000;




    private CMwCmdExp? value;
    [AppliedWithChunk<Chunk01032000>]
    public CMwCmdExp? Value { get => value; set => value = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CMwCmdAffectIdent"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMwCmdAffectIdent() { }


    /// <summary>
    /// CMwCmdAffectIdent 0x000 chunk
    /// </summary>
    [Chunk(0x01032000)]
    public partial class Chunk01032000 : Chunk<CMwCmdAffectIdent>
    {
        /// <inheritdoc />
        public override uint Id => 0x01032000;

        public string? U01;
        public bool U02;

        public override void ReadWrite(CMwCmdAffectIdent n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.NodeRef<CMwCmdExp>(ref n.value);
            rw.Boolean(ref U02);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x01032000 => new Chunk01032000(),
        _ => base.NewChunk(chunkId),
    };
}
