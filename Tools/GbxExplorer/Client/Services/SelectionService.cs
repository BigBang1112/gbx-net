using GbxExplorer.Client.Components;

namespace GbxExplorer.Client.Services;

public class SelectionService : ISelectionService
{
    private NodeTreeElement? element;

    public NodeTreeElement? Element
    {
        get => element;
        set
        {
            if (element == value) return;

            SelectionChanged?.Invoke(value);
            element?.Update(parent: false);
            element = value;
        }
    }

    public event Action<NodeTreeElement?>? SelectionChanged;

    public void Update()
    {
        SelectionChanged?.Invoke(element);
    }
}
