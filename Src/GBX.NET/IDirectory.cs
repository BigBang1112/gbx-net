namespace GBX.NET;

public interface IDirectory
{
    string Name { get; set; }
    IDirectory? Parent { get; set; }
    IList<IDirectory> Children { get; }
    IList<IFile> Files { get; }

    IDirectory? Directory(string name);
    IFile? File(string name);
}
