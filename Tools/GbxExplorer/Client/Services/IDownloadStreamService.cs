namespace GbxExplorer.Client.Services;

public interface IDownloadStreamService
{
    Task DownloadAsync(string fileName, byte[] data);
    Task DownloadAsync(string fileName, MemoryStream stream);
}