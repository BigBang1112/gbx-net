namespace GbxExplorerOld.Client.Services;

public interface IOpenChunkService
{
    HashSet<Type> OpenedChunks { get; }
}