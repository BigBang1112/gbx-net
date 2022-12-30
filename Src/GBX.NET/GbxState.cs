namespace GBX.NET;

public class GbxState
{
    private IList<string>? idStrings;
    private SortedDictionary<int, Node?>? auxNodes;
    private SortedDictionary<int, GameBoxRefTable.File?>? extAuxNodes;

    public int? IdVersion { get; internal set; }
    public IList<string> IdStrings => idStrings ??= new List<string>();
    public SortedDictionary<int, Node?> AuxNodes => auxNodes ??= new();
    public SortedDictionary<int, GameBoxRefTable.File?> ExtAuxNodes => extAuxNodes ??= new();

    public bool Encapsulated { get; }

    public GbxState(bool encapsulated = false)
    {
        Encapsulated = encapsulated;
    }
}
