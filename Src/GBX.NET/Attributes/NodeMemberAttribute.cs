namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NodeMemberAttribute : Attribute
{
    public bool ExactlyNamed { get; set; }
    public string? ExactName { get; set; }
}
