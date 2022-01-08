namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class NodeExtensionAttribute : Attribute
{
    public string Extension { get; }

    public NodeExtensionAttribute(string extension)
    {
        Extension = extension;
    }
}