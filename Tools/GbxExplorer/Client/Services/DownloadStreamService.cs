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

    public void Download(string fileName, byte[] data)
    {
        if (_js is not IJSUnmarshalledRuntime wasmJs)
        {
            throw new NotSupportedException("Sync Download is not supported.");
        }

        wasmJs.InvokeUnmarshalled<DownloadStruct, byte[], object>("saveAsFileUnmarshalled", new(fileName), data);
    }

    public void Download(string fileName, MemoryStream stream)
    {
        Download(fileName, stream.ToArray());
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
