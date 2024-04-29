namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class ChunkAttribute(uint id, string? description = null) : Attribute
{
    public uint Id { get; } = id;
    public string? Description { get; } = description;
}

