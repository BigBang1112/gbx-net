namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ClassAttribute(uint id) : Attribute
{
    public uint Id { get; } = id;
}
