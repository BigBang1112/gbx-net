using GbxExplorer.Client.Components.ValueRenderers;

namespace GbxExplorer.Client.Services;

public class ValueRendererService : IValueRendererService
{
    private Dictionary<Type, Type>? cache;

    public Type this[Type? type] => ComponentPerType.GetType<ValueRenderer>(type, ref cache, typeof(EnumValueRenderer));
}
