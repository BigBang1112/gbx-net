using GBX.NET.Managers;

namespace GBX.NET;

/// <summary>
/// [CMwId] A struct that handles Id (lookback string) by either a string or an index.
/// </summary>
public readonly record struct Id
{
    /// <summary>
    /// An empty <see cref="Id"/> with default values.
    /// </summary>
    public static readonly Id Empty = new();

    /// <summary>
    /// Represents an ID of the collection. Null if the <see cref="Id"/> is string-defined.
    /// </summary>
    public int? Number { get; }

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
        Number = collectionId;
    }

    /// <summary>
    /// Gets the block size of the collection, if it is known.
    /// </summary>
    /// <returns>Size of the block.</returns>
    /// <exception cref="NotSupportedException">Collection was not found to have a block size.</exception>
    public Int3 GetBlockSize() => ToString() switch
    {
        "Coast" => new Int3(16, 4, 16),
        "Desert" or "Speed" or "Snow" or "Alpine" => new Int3(32, 16, 32),
        "Rally" or "Bay" or "Stadium" or "Valley" or "Lagoon" or "Stadium2020" => new Int3(32, 8, 32),
        "Island" => new Int3(64, 8, 64),
        "Canyon" => new Int3(64, 16, 64),
        _ => throw new NotSupportedException("Block size not supported for this collection"),
    };

    /// <summary>
    /// Converts the <see cref="Id"/> to string, also into its readable form if the <see cref="Id"/> is presented by collection ID.
    /// </summary>
    /// <returns>If collection is ID-represented, the ID is converted to <see cref="string"/> based from the collection ID list. If it's string-represented, <see cref="String"/> is returned instead.</returns>
    public override string ToString()
    {
        if (Number is null)
        {
            return String ?? string.Empty;
        }

        return CollectionManager.GetName(Number.Value) ?? Number.Value.ToString();
    }

    /// <summary>
    /// Implicitly converts an <see cref="Id"/> to a string.
    /// </summary>
    /// <param name="id">The <see cref="Id"/>.</param>
    public static implicit operator string(Id id) => id.ToString();


    /// <summary>
    /// Implicitly converts an <see cref="int"/> to an <see cref="Id"/>.
    /// </summary>
    /// <param name="id">The <see cref="int"/>.</param>
    public static implicit operator Id(int id) => new(id);
}
