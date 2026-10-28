namespace GBX.NET.Builders;

// One descriptor per declaring node property is shared by every game stage and
// derived node builder. The generated delegate calls the property directly.
internal sealed class NodeBuilderProperty<TValue>(string name, Action<object, TValue> assign, Func<TValue, TValue>? copy = null)
{
    public string Name { get; } = name;

    public TValue Copy(TValue value) => copy is null ? value : copy(value);
    public void Assign(object node, TValue value) => assign(node, Copy(value));
}
