namespace GBX.NET;

public interface IDirectory
{
    IDirectory? Parent { get; set; }
    IDictionary<string, IDirectory> Children { get; }
    IDictionary<string, IFile> Files { get; }
}
