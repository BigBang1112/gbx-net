namespace GbxExplorer.Client.Services;

public interface IDownloadStreamService
{
    void Download(string fileName, byte[] data);
    void Download(string fileName, MemoryStream stream);
    Task DownloadAsync(string fileName, byte[] data);
    Task DownloadAsync(string fileName, MemoryStream stream);
}