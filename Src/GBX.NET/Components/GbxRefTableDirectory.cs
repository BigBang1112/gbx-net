namespace GBX.NET.Components;

public sealed class GbxRefTableDirectory : IDirectory
{
    public string Name { get; set; } = string.Empty;
    public IDirectory? Parent { get; set; }
    public IList<IDirectory> Children { get; } = new List<IDirectory>();
    public IList<IFile> Files { get; } = new List<IFile>();

    public override string ToString()
    {
        return string.IsNullOrEmpty(Name) ? "(root)" : Name;
    }
}
