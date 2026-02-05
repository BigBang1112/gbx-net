namespace GbxExplorerOld.Client.Sections.SectionEditor;

public class Note(string name, int version, long createdAt, long updatedAt, string body, List<string>? refs = null)
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = name;
    public int Version { get; set; } = version;
    public long CreatedAt { get; set; } = createdAt;
    public long UpdatedAt { get; set; } = updatedAt;
    public string Body { get; set; } = body;
    public List<string> Refs { get; set; } = refs ??
    [
        "System.Runtime.dll",
        "System.Console.dll",
        "netstandard.dll",
        "GBX.NET.dll",
        "TmEssentials.dll",
        "System.Linq.dll",
        "System.Collections.dll",
        "System.Private.CoreLib.dll",
        "System.Core.dll",
        "System.Threading.Tasks.dll",
        "GbxExplorerOld.Client.dll"
    ];
}