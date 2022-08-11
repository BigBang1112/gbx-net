namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class NodeAttribute : Attribute
{
    public uint ID { get; }

    public NodeAttribute(uint id)
    {
        ID = id;
    }
}
