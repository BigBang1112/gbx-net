namespace GbxExplorer.Client.Services;

public interface IOpenChunkService
{
    HashSet<Type> OpenedChunks { get; }
}