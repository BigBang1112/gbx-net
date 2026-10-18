namespace GbxCLI;

public sealed class CSharpGlobals<T>
{
    public T Value { get; }

    public CSharpGlobals(T value)
    {
        Value = value;
    }
}
