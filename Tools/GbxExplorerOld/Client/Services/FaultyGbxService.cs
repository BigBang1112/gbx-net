using GbxExplorerOld.Client.Models;

namespace GbxExplorerOld.Client.Services;

public class FaultyGbxService : IFaultyGbxService
{
    public Dictionary<Exception, FaultyGbxModel> FaultyGbxs { get; }

    public FaultyGbxService()
    {
        FaultyGbxs = new Dictionary<Exception, FaultyGbxModel>();
    }

    public void ResetHover()
    {
        foreach (var faultyGbx in FaultyGbxs)
        {
            faultyGbx.Value.Hovered = false;
        }
    }

    public void ResetSelection()
    {
        foreach (var faultyGbx in FaultyGbxs)
        {
            faultyGbx.Value.Selected = false;
        }
    }
}
