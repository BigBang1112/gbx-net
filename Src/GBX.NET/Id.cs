namespace GBX.NET;

/// <summary>
/// A struct that handles Id (lookback string) by either a string or an index.
/// </summary>
public readonly record struct Id
{
    public static readonly Id Empty = new();

    /// <summary>
    /// Represents an ID of the collection. Null if the <see cref="Id"/> is string-defined.
    /// </summary>
    public int? Index { get; }

    /// <summary>
    /// Represents the string of the <see cref="Id"/>. Null if the <see cref="Id"/> is presented as a collection ID.
    /// </summary>
    public string? String { get; }

    /// <summary>
    /// Constructs an <see cref="Id"/> struct from a string representation.
    /// </summary>
    /// <param name="str">An Id string.</param>
    public Id(string str)
    {
        String = str;
    }

    /// <summary>
    /// Constructs an <see cref="Id"/> struct from an ID reprentation.
    /// </summary>
    /// <param name="collectionId">A collection ID from the collection ID list (specified ID doesn't have to be available in the list).</param>
    public Id(int collectionId)
    {
        Index = collectionId;
    }

    public Int3 GetBlockSize() => ToString() switch
    {
        "Coast" => (16, 4, 16),
        "Desert" or "Speed" or "Snow" or "Alpine" => (32, 16, 32),
        "Rally" or "Bay" or "Stadium" or "Valley" or "Lagoon" or "Stadium2020" => (32, 8, 32),
        "Island" => (64, 8, 64),
        "Canyon" => (64, 16, 64),
        _ => throw new NotSupportedException("Block size not supported for this collection"),
    };

    /// <summary>
    /// Converts the <see cref="Id"/> to string, also into its readable form if the <see cref="Id"/> is presented by collection ID.
    /// </summary>
    /// <returns>If collection is ID-represented, the ID is converted to <see cref="string"/> based from the collection ID list. If it's string-represented, <see cref="String"/> is returned instead.</returns>
    public override string ToString()
    {
        if (Index is null)
        {
            return String ?? "";
        }

        if (NodeManager.TryGetCollectionName(Index.Value, out string? value))
        {
            return value!;
        }

        return Index.Value.ToString();
    }

    public static implicit operator Id(string a)
    {
        if (string.IsNullOrEmpty(a))
        {
            return new Id();
        }

        if (int.TryParse(a, out int collectionID))
        {
            return new Id(collectionID);
        }
        
        return new Id(a);
    }

    public static implicit operator Id(int a) => new(a);
    public static implicit operator string(Id a) => a.ToString();
}
