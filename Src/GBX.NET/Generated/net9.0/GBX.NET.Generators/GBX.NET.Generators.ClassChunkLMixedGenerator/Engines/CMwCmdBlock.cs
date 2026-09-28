namespace GBX.NET.Engines.MwFoundations;

/// <remarks>ID: 0x01030000</remarks>
[Class(0x01030000)]
public partial class CMwCmdBlock : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x01030000;




    private CMwCmdScript[]? cmds;
    [AppliedWithChunk<Chunk01030004>]
    public CMwCmdScript[]? Cmds { get => cmds; set => cmds = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CMwCmdBlock"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMwCmdBlock() { }


    /// <summary>
    /// CMwCmdBlock 0x004 chunk
    /// </summary>
    [Chunk(0x01030004)]
    public partial class Chunk01030004 : Chunk<CMwCmdBlock>
    {
        /// <inheritdoc />
        public override uint Id => 0x01030004;


        public override void ReadWrite(CMwCmdBlock n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CMwCmdScript>(ref n.cmds!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x01030004 => new Chunk01030004(),
        _ => base.NewChunk(chunkId),
    };
}
