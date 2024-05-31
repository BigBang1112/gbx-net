namespace GbxExplorerOld.Client.Models;

public class TypeModel
{
    public Type Type { get; }
    public bool Selected { get; set; } = true;

    public TypeModel(Type type)
    {
        Type = type;
    }
}
