using GBX.NET.Components;

namespace GBX.NET;

public sealed record External<T> where T : CMwNod
{
    private T? node;

    public T? Node
    {
        get => File?.GetNode(ref node) ?? node;
        set => node = value;
    }

    public GbxRefTableFile? File { get; set; }

    public External(T? node, GbxRefTableFile? file)
    {
        Node = node;
        File = file;
    }

    public T? GetNode(GbxReadSettings settings = default) => File?.GetNode(ref node, settings) ?? node;
}
