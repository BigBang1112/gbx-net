using GbxExplorerOld.Client.Components.ValueRenderers;

namespace GbxExplorerOld.Client.Services;

public class ValueRendererService : IValueRendererService
{
    private Dictionary<Type, Type>? cache;

    public Type this[Type? type] => ComponentPerType.GetType<ValueRenderer>(type, ref cache, typeof(EnumValueRenderer));
}
