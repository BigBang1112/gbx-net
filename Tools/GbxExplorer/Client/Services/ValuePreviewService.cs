using GbxExplorer.Client.Components.ValuePreviews;

namespace GbxExplorer.Client.Services;

public class ValuePreviewService : IValuePreviewService
{
    private Dictionary<Type, Type>? cache;

    public Type this[Type? type] => ComponentPerType.GetType<ValuePreview>(type, ref cache, enumType: null);
}
