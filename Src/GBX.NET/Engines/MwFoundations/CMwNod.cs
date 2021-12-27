namespace GBX.NET.Engines.MwFoundations;

/// <summary>
/// Node (0x01001000)
/// </summary>
/// <remarks>The base of every existing node.</remarks>
[Node(0x01001000)]
public class CMwNod : Node
{
    public string[]? Dependencies { get; set; }

    protected CMwNod()
    {

    }
}
