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

    public Uri FlagUrl
    {
        get => flagUrl;
        set => flagUrl = value;
    }

    private CGameLeague()
    {
        path = null!;
        name = null!;
        flagUrl = null!;
    }

    public override string ToString()
    {
        return path + "|" + name;
    }

    [Chunk(0x0308E001)]
    public class Chunk0308E001 : Chunk<CGameLeague>
    {
        public int U01;
        public int U02;
        public byte U03;

        public override void ReadWrite(CGameLeague n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.path!);
            rw.String(ref n.name!);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Byte(ref U03);
            rw.Uri(ref n.flagUrl!);
        }
    }
}
