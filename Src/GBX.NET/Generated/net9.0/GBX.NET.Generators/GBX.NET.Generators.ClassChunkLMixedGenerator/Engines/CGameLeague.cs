namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0308E000</remarks>
[Class(0x0308E000)]
public partial class CGameLeague : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0308E000;




    private string? path;
    [AppliedWithChunk<Chunk0308E001>]
    [AppliedWithChunk<Chunk0308E002>]
    public string? Path { get => path; set => path = value; }

    private string? name;
    [AppliedWithChunk<Chunk0308E001>]
    [AppliedWithChunk<Chunk0308E002>]
    public string? Name { get => name; set => name = value; }

    private string? description;
    [AppliedWithChunk<Chunk0308E001>]
    [AppliedWithChunk<Chunk0308E002>]
    public string? Description { get => description; set => description = value; }

    private string? login;
    [AppliedWithChunk<Chunk0308E001>]
    [AppliedWithChunk<Chunk0308E002>]
    public string? Login { get => login; set => login = value; }

    private string? flagUrl;
    [AppliedWithChunk<Chunk0308E001>]
    [AppliedWithChunk<Chunk0308E002>]
    public string? FlagUrl { get => flagUrl; set => flagUrl = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameLeague"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameLeague() { }


    /// <summary>
    /// CGameLeague 0x001 chunk
    /// </summary>
    [Chunk(0x0308E001)]
    public partial class Chunk0308E001 : Chunk<CGameLeague>
    {
        /// <inheritdoc />
        public override uint Id => 0x0308E001;

        public byte U01;

        public override void ReadWrite(CGameLeague n, GbxReaderWriter rw)
        {
            rw.String(ref n.path);
            rw.String(ref n.name);
            rw.String(ref n.description);
            rw.String(ref n.login);
            rw.Byte(ref U01);
            rw.String(ref n.flagUrl);
        }
    }

    /// <summary>
    /// CGameLeague 0x002 chunk
    /// </summary>
    [Chunk(0x0308E002)]
    public partial class Chunk0308E002 : Chunk<CGameLeague>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0308E002;

        public int Version { get; set; }

        public string? U01;
        public string? U02;
        public byte U03;

        public override void ReadWrite(CGameLeague n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.String(ref U01);
            rw.String(ref U02);
            rw.String(ref n.name);
            rw.String(ref n.path);
            rw.String(ref n.description);
            rw.String(ref n.login);
            rw.Byte(ref U03);
            rw.String(ref n.flagUrl);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0308E001 => new Chunk0308E001(),
        0x0308E002 => new Chunk0308E002(),
        _ => base.NewChunk(chunkId),
    };
}
