namespace GbxExplorer.Client.Services;

public class OpenChunkService : IOpenChunkService
{
    public HashSet<Type> OpenedChunks { get; } = new();
}
