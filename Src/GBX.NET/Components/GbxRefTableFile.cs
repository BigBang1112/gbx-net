namespace GBX.NET.Components;

public sealed class GbxRefTableFile(IDirectory parent) : IFile
{
    public IDirectory Parent { get; set; } = parent;
}
