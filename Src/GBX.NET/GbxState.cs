namespace GBX.NET;

public class GbxState
{
    private IList<string>? idStrings;
    private SortedDictionary<int, Node?>? auxNodes;

    public int? IdVersion { get; internal set; }
    public IList<string> IdStrings => idStrings ??= new List<string>();
    public SortedDictionary<int, Node?> AuxNodes => auxNodes ??= new();
}
