namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class ClassAttribute(uint id) : Attribute
{
    public uint Id { get; } = id;
}
