namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
internal sealed class ChunkGameVersionAttribute(GameVersion game, params int[] version) : Attribute
{
    public GameVersion Game { get; } = game;
    public int[] Version { get; } = version;
}

