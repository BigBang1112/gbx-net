namespace GbxExplorer.Client.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class AppliesForTypeAttribute : Attribute
{
    public Type[] Types { get; }

    public AppliesForTypeAttribute(Type type)
	{
        Types = new[] { type };
    }
    
    public AppliesForTypeAttribute(Type[] types)
    {
        Types = types;
    }
}
