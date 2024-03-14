namespace GBX.NET;

public interface IDirectory
{
    IDirectory? Parent { get; set; }
    IList<IDirectory> Children { get; }
    IList<IFile> Files { get; }
}
