namespace GBX.NET;

public interface IGbxState
{
    int? IdVersion { get; set; }
    IList<string> IdStrings { get; }
    SortedDictionary<int, Node?> AuxNodes { get; }
}
