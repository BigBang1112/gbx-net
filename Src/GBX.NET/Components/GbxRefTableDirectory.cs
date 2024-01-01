namespace GBX.NET.Components;

public sealed class GbxRefTableDirectory : IDirectory
{
    public string Name { get; set; } = string.Empty;
    public IDirectory? Parent { get; set; }
    public IDictionary<string, IDirectory> Children { get; } = new Dictionary<string, IDirectory>();
    public IDictionary<string, IFile> Files { get; } = new Dictionary<string, IFile>();

    public override string ToString()
    {
        return string.IsNullOrEmpty(Name) ? "(root)" : Name;
    }
}
