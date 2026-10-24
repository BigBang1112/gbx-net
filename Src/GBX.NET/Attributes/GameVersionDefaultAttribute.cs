namespace GBX.NET.Attributes;

/// <summary>
/// Records a member's default value in a particular game without changing its initialization.
/// </summary>
/// <param name="game">Game in which the default value applies.</param>
/// <param name="defaultValue">Default value recorded by the layout.</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public sealed class GameVersionDefaultAttribute(GameVersion game, object? defaultValue = null) : Attribute
{
    public GameVersion Game { get; } = game;
    public object? DefaultValue { get; } = defaultValue;

    /// <summary>
    /// Unevaluated ChunkL expression for a non-literal default. Null when <see cref="DefaultValue"/> records a literal value.
    /// </summary>
    public string? DefaultExpression { get; set; }
}
