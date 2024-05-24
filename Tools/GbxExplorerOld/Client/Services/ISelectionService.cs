using GbxExplorerOld.Client.Components;

namespace GbxExplorerOld.Client.Services;

public interface ISelectionService
{
    NodeTreeElement? Element { get; set; }

    event Action<NodeTreeElement?>? SelectionChanged;

    void Update();
}