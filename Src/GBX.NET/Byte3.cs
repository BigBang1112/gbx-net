namespace GBX.NET;

/// <summary>
/// Represents a three-dimensional vector with byte components (X, Y, Z).
/// </summary>
/// <param name="X">The X-component of the vector.</param>
/// <param name="Y">The Y-component of the vector.</param>
/// <param name="Z">The Z-component of the vector.</param>
public readonly record struct Byte3(byte X, byte Y, byte Z)
{
    /// <summary>
    /// Vector with all components set to zero.
    /// </summary>
    public static readonly Byte3 Zero = new();

    /// <summary>
    /// Returns a string representation of the Byte3 vector.
    /// </summary>
    /// <returns>A string representation of the form "&lt;X, Y, Z&gt;".</returns>
    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}>";
    }

    /// <summary>
    /// Implicitly converts a tuple of bytes to an Byte3 vector.
    /// </summary>
    /// <param name="v">The tuple containing X, Y, and Z components.</param>
    public static implicit operator Byte3((byte X, byte Y, byte Z) v) => new(v.X, v.Y, v.Z);

    /// <summary>
    /// Implicitly converts an Byte3 vector to a tuple of bytes.
    /// </summary>
    /// <param name="v">The Byte3 vector to convert.</param>
    public static implicit operator (byte X, byte Y, byte Z)(Byte3 v) => (v.X, v.Y, v.Z);
}