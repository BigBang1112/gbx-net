using GbxExplorer.Client.Components;

namespace GbxExplorer.Client.Services;

public interface ISelectionService
{
    NodeTreeElement? Element { get; set; }

    event Action<NodeTreeElement?>? SelectionChanged;

    void Update();
}