namespace GBX.NET;

/// <summary>
/// A struct that can handle GameBox collection/environment values by either a name or a value.
/// </summary>
public struct Collection
{
    /// <summary>
    /// Represents an ID defined kind of collection. If set, the ID is converted to its proper name from the <see cref="Resources.CollectionID"/> list using the <see cref="ToString"/> method. Always available if <see cref="Name"/> is null.
    /// </summary>
    public int? ID { get; }
    /// <summary>
    /// Represents a name defined kind of collection. If set, collection is stored as a regular <see cref="Id"/> in GBX. Always avaliable if <see cref="ID"/> is null.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Constructs a Collection struct from a name representation.
    /// </summary>
    /// <param name="name">A collection name.</param>
    public Collection(string name)
    {
        ID = null;
        Name = name;
    }

    /// <summary>
    /// Constructs a Collection struct from an ID reprentation.
    /// </summary>
    /// <param name="collectionID">A collection ID from the <see cref="Resources.CollectionID"/> list (specified ID doesn't have to be available in the list).</param>
    public Collection(int collectionID)
    {
        ID = collectionID;
        Name = null;
    }

    public Int3 GetBlockSize()
    {
        return ToString() switch
        {
            "Desert" => (32, 16, 32),
            "Snow" => (32, 16, 32),
            "Rally" => (32, 16, 32),
            "Island" => (64, 8, 64),
            "Bay" => (32, 8, 32),
            "Coast" => (16, 8, 16),
            "Valley" => (32, 8, 32),
            "Stadium" => (32, 8, 32),
            "Canyon" => (64, 16, 64),
            "Lagoon" => (32, 8, 32),
            _ => throw new Exception(),
        };
    }

    /// <summary>
    /// Converts the collection to a readable string.
    /// </summary>
    /// <returns>If collection is ID-represented, the ID is converted to <see cref="string"/> based from the <see cref="Resources.CollectionID"/> list. If it's name-represented, <see cref="Name"/> is returned instead.</returns>
    public override string ToString()
    {
        if (!ID.HasValue)
            return Name ?? "";
        if (Id.CollectionIDs.TryGetValue(ID.Value, out string? value))
            return value;
        return ID.Value.ToString();
    }

    /// <summary>
    /// Converts the collection to a GBX-ready format.
    /// </summary>
    /// <param name="lookbackable">A lookbackable to look for existing strings from.</param>
    /// <returns>Returns a ready-to-use <see cref="Id"/>.</returns>
    public Id ToId(ILookbackable lookbackable)
    {
        if (ID.HasValue)
            return new Id(ID.ToString(), lookbackable);
        return new Id(Name, lookbackable);
    }

    public static implicit operator Collection(string a) => new(a);
    public static implicit operator Collection(Id a)
    {
        if (string.IsNullOrEmpty(a))
            return new Collection();
        else if (int.TryParse(a, out int collectionID))
            return new Collection(collectionID);
        else
            return new Collection(a);
    }
    public static implicit operator Collection(int a) => new(a);
    public static implicit operator string(Collection a) => a.ToString();
}
