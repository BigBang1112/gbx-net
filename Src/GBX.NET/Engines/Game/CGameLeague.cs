namespace GBX.NET.Engines.Game;

/// <summary>
/// League (or zone in other words).
/// </summary>
/// <remarks>ID: 0x0308E000</remarks>
[Node(0x0308E000)]
[NodeExtension("League")]
public class CGameLeague : CMwNod
{
    #region Fields

    private string path;
    private string name;
    private string description;
    private string login;
    private Uri flagUrl;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    public string Path { get => path; set => path = value; }

    [NodeMember(ExactlyNamed = true)]
    public string Name { get => name; set => name = value; }

    [NodeMember(ExactlyNamed = true)]
    public string Description { get => description; set => description = value; }

    [NodeMember(ExactlyNamed = true)]
    public string Login { get => login; set => login = value; }

    [NodeMember]
    public Uri FlagUrl { get => flagUrl; set => flagUrl = value; }

    #endregion

    #region Constructors

    protected CGameLeague()
    {
        path = null!;
        name = null!;
        description = null!;
        login = null!;
        flagUrl = null!;
    }

    #endregion

    #region Methods

    public override string ToString()
    {
        return $"{base.ToString()} {{ \"{path}|{name}\" }}";
    }

    #endregion

    #region Chunks

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
            rw.Uri(ref n.flagUrl!);
        }
    }

    #endregion

    #endregion
}
