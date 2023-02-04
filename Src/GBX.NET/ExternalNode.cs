namespace GBX.NET;

public record ExternalNode<T>(T? Node, GameBoxRefTable.File? File) where T : Node
{
    public static ExternalNode<T> Empty { get; } = new(null, null);

    public bool IsEmpty => Node is null && File is null;

    public override string ToString()
    {
        if (Node is not null)
        {
            return Node.ToString();
        }

        if (File is not null)
        {
            return File.ToString();
        }

        return "Empty";
    }
}
