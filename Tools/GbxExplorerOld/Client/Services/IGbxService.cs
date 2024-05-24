using GbxExplorerOld.Client.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace GbxExplorerOld.Client.Services;

public interface IGbxService
{
    List<GbxModel> List { get; }
    GbxModel? SelectedGbx { get; set; }
    Task<GbxModelBase?> ImportGbxAsync(IBrowserFile file, Func<string, Task> asyncEvent, CancellationToken cancellationToken = default);
}