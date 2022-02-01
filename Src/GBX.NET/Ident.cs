namespace GBX.NET;

/// <summary>
/// Identifier defined by ID, collection and author. Also known as "meta".
/// </summary>
public record Ident(string Id, Collection Collection, string Author)
{
    public Ident(string id) : this(id, new Collection(), "")
    {

    }

    public Ident() : this(string.Empty)
    {

    }

    public override string ToString()
    {
        return $"(\"{Id}\", \"{Collection}\", \"{Author}\")";
    }

    public static implicit operator Ident((string Id, Collection Collection, string Author) v)
    {
        return new(v.Id, v.Collection, v.Author);
    }

    public static implicit operator (string Id, Collection Collection, string Author)(Ident v)
    {
        return (v.Id, v.Collection, v.Author);
    }

    public static readonly Ident Empty = new();
}
