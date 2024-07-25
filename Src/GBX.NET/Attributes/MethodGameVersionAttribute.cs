namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class MethodGameVersionAttribute(GameVersion game, params int[] version) : Attribute
{
    public GameVersion Game { get; } = game;
    public int[] Version { get; } = version;
}

