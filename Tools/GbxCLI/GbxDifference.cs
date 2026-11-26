namespace GbxCLI;

/// <summary>
/// A property that differs between two Gbx files, found by <see cref="GbxPropertyComparer"/>.
/// </summary>
/// <param name="Path">Path of the property, such as <c>Node.Blocks[3].Coord</c>.</param>
/// <param name="Left">The value in the first Gbx, described on one line.</param>
/// <param name="Right">The value in the second Gbx, described on one line.</param>
internal sealed record GbxDifference(string Path, string Left, string Right)
{
    /// <summary>
    /// Describes a value that doesn't exist on one side, such as a dictionary key or a chunk.
    /// </summary>
    public const string Missing = "(missing)";

    /// <summary>
    /// The difference as a sentence, such as <c>Node.MapName from A to B.</c>
    /// </summary>
    public string ToSentence() => $"{Path} from {Left} to {Right}.";

    /// <summary>
    /// The difference as a line, such as <c>Node.MapName: A -> B</c>.
    /// </summary>
    public override string ToString() => $"{Path}: {Left} -> {Right}";
}
