namespace GbxExplorerOld.Client.Services;

public class OpenChunkService : IOpenChunkService
{
    public HashSet<Type> OpenedChunks { get; } = new();
}
