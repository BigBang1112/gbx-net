namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
internal sealed class ChunkGameVersionAttribute(GameVersion game) : Attribute
{
    public GameVersion Game { get; } = game;
    public int Version { get; set; }
}

