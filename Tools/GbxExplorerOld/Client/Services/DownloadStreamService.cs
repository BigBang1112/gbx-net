using GBX.NET;
using Microsoft.JSInterop;
namespace GbxExplorerOld.Client.Services;

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
    
    public async Task DownloadAsync(string fileName, IGbx gbx)
    {
        using var resultMs = new MemoryStream();
        gbx.Save(resultMs);
        await DownloadAsync(fileName, resultMs.ToArray());
    }
}
