namespace GBX.NET.Components;

public sealed class GbxRefTableDirectory : IDirectory
{
    public string Name { get; set; } = string.Empty;
    public IDirectory? Parent { get; set; }
    public IList<IDirectory> Children { get; } = new List<IDirectory>();
    public IList<IFile> Files { get; } = new List<IFile>();

    public IDirectory? Directory(string name)
    {
        return Children.FirstOrDefault(x => x.Name == name);
    }

    public IFile? File(string name)
    {
        return Files.FirstOrDefault(x => x.Name == name);
    }

    public override string ToString()
    {
        return string.IsNullOrEmpty(Name) ? "(root)" : Name;
    }
}
