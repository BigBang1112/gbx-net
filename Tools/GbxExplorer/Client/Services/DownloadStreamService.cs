using Microsoft.JSInterop;

namespace GbxExplorer.Client.Services;

public class DownloadStreamService : IDownloadStreamService
{
    private readonly record struct DownloadStruct(string FileName);

    private readonly IJSRuntime _js;

    public DownloadStreamService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task DownloadAsync(string fileName, byte[] data)
    {
        await _js.InvokeVoidAsync("saveAsFile", fileName, data);
    }

    public async Task DownloadAsync(string fileName, MemoryStream stream)
    {
        await DownloadAsync(fileName, stream.ToArray());
    }
}
