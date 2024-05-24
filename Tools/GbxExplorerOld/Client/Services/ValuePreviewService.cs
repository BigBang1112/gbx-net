using GbxExplorerOld.Client.Components.ValuePreviews;

namespace GbxExplorerOld.Client.Services;

public class ValuePreviewService : IValuePreviewService
{
    private Dictionary<Type, Type>? cache;

    public Type this[Type? type] => ComponentPerType.GetType<ValuePreview>(type, ref cache, enumType: null);
}
