using GbxExplorer.Client.Models;

namespace GbxExplorer.Client.Services;

public interface IFaultyGbxService
{
    Dictionary<Exception, FaultyGbxModel> FaultyGbxs { get; }

    void ResetHover();
    void ResetSelection();
}