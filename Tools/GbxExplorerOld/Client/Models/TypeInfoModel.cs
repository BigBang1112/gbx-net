namespace GbxExplorerOld.Client.Models;

public class TypeInfoModel
{
    public Type Type { get; }
    public Type ElementType { get; }
    public Type[]? GenericArguments { get; }
    public string Name { get; }
    public string? StandardName { get; }
    public string PropTypeName { get; }
    public bool? IsNullable { get; }
    public int? ArrayRank { get; }
    public bool IsNodeType { get; }
    public string? Engine { get; }

    public TypeInfoModel(Type type,
                         Type elementType,
                         Type[]? genericArguments,
                         string name,
                         string? standardName,
                         string propTypeName,
                         bool? isNullable,
                         int? arrayRank,
                         bool isNodeType,
                         string? engine)
    {
        Type = type;
        ElementType = elementType;
        GenericArguments = genericArguments;
        Name = name;
        StandardName = standardName;
        PropTypeName = propTypeName;
        IsNullable = isNullable;
        ArrayRank = arrayRank;
        IsNodeType = isNodeType;
        Engine = engine;
    }
}
