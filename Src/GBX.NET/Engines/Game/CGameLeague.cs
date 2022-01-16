namespace GBX.NET.Engines.Game;

/// <summary>
/// League (0x0308E000)
/// </summary>
/// <remarks>Or zone in other words.</remarks>
[Node(0x0308E000)]
public class CGameLeague : CMwNod
{
    private string path;
    private string name;
    private string description;
    private string login;
    private Uri flagUrl;

    public string Path
    {
        get => path;
        set => path = value;
    }

    public string Name
    {
        get => name;
        set => name = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }

    public string Login
    {
        get => login;
        set => login = value;
    }

    public Uri FlagUrl
    {
        get => flagUrl;
        set => flagUrl = value;
    }

    protected CGameLeague()
    {
        path = null!;
        name = null!;
        description = null!;
        login = null!;
        flagUrl = null!;
    }

    public override string ToString()
    {
        return $"{base.ToString()} {{ \"{path}|{name}\" }}";
    }

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
}
