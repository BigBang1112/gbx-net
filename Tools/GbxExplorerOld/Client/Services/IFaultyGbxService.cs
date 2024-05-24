using GbxExplorerOld.Client.Models;

namespace GbxExplorerOld.Client.Services;

public interface IFaultyGbxService
{
    Dictionary<Exception, FaultyGbxModel> FaultyGbxs { get; }

    void ResetHover();
    void ResetSelection();
}