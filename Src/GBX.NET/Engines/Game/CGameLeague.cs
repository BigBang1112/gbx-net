namespace GBX.NET.Engines.Game;

/// <summary>
/// League (or zone in other words).
/// </summary>
/// <remarks>ID: 0x0308E000</remarks>
[Node(0x0308E000)]
[NodeExtension("League")]
public class CGameLeague : CMwNod
{
    private string path;
    private string name;
    private string description;
    private string login;
    private string flagUrl;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0308E001))]
    public string Path { get => path; set => path = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0308E001))]
    public string Name { get => name; set => name = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0308E001))]
    public string Description { get => description; set => description = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0308E001))]
    public string Login { get => login; set => login = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0308E001))]
    public string FlagUrl { get => flagUrl; set => flagUrl = value; }

    protected CGameLeague()
    {
        path = "";
        name = "";
        description = "";
        login = "";
        flagUrl = "";
    }

    public override string ToString()
    {
        return $"{base.ToString()} {{ \"{path}|{name}\" }}";
    }

    #region 0x001 chunk

    /// <summary>
    /// CGameLeague 0x001 chunk
    /// </summary>
    [Chunk(0x0308E001)]
    public class Chunk0308E001 : Chunk<CGameLeague>
    {
        public byte U03;

        public override void ReadWrite(CGameLeague n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.path!);
            rw.String(ref n.name!);
            rw.String(ref n.description!);
            rw.String(ref n.login!);
            rw.Byte(ref U03);
            rw.String(ref n.flagUrl!);
        }
    }

    #endregion
}
