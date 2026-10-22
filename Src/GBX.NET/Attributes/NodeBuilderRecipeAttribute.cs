namespace GBX.NET.Attributes;

/// <summary>
/// Replaces the inferred chunk selection for one game's generated node builder.
/// </summary>
/// <remarks>
/// Use this only when the ordinary game writer qualifiers do not describe a unique
/// creation recipe. Chunk types may be inherited and may include header chunks.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class NodeBuilderRecipeAttribute(GameVersion game, params Type[] chunks) : Attribute
{
    public GameVersion Game { get; } = game;
    public Type[] Chunks { get; } = chunks;
}
