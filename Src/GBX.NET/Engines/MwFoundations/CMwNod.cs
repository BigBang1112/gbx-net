namespace GBX.NET.Engines.MwFoundations;

/// <summary>
/// The base of every existing node.
/// </summary>
/// <remarks>ID: 0x01001000</remarks>
[Node(0x01001000)]
public class CMwNod : Node
{
    public string[]? Dependencies { get; set; }

    protected CMwNod()
    {

    }
}
